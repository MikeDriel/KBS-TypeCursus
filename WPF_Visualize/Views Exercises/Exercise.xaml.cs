using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Controller;
using Model;
using WPF_Visualize.ViewLogic;

namespace WPF_Visualize;

/// <summary>
///     Interaction logic for UserControl1.xaml
/// </summary>
public partial class Exercise : UserControl
{
    public static StatisticsController? s_StatisticsController;
    private readonly ExerciseController _controller;

    private readonly Rectangle _rectangleLetterToType =
        new() { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle

    private readonly Rectangle _rectangleLetterTyped =
        new() { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle

	public Exercise(int choice)
	{
		InitializeComponent();
		_controller = new ExerciseController(choice, Difficulty.Niveau1);
		int maxTime;
		if (choice == 2)
		{
			maxTime = 240;
			LettersTypedLabel.Visibility = Visibility.Hidden;
			LettersTodoLabel.Visibility = Visibility.Hidden;
			LetterToTypeLabel.Visibility = Visibility.Hidden;
		}
		else
		{
			maxTime = 5;
			RichTextBoxStory.Visibility = Visibility.Hidden;
		}
		
		s_StatisticsController = new StatisticsController(maxTime);
		//subscribe events
		_controller.ExerciseEvent += ExerciseEvent;
		s_StatisticsController.LiveStatisticsEvent += SetLiveStatistics;

        MoveLetterToTypeBoxOnCanvas();
        ChangeTextOnScreen();
        InitializeProgressBar();

        //Keyboard Rectangels initializing
        KeyboardCanvas.Children.Add(_rectangleLetterToType); //adds rectangle on screen
        KeyboardCanvas.Children.Add(_rectangleLetterTyped);
        _rectangleLetterTyped.Visibility = Visibility.Hidden;
    }

    private void InitializeProgressBar()
    {
        ProgressBar.Minimum = 0;
        ProgressBar.Maximum = _controller.CharacterList.Count;
    }

    //Connects events to the button 
    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this);
        window.TextInput += TextInputPress;
    }

    private void TextInputPress(object sender, TextCompositionEventArgs e)
    {
        // check if button pressed was enter
        if (e.Text == "\r")
        {
            return;
        }

        if (e.Text == "\b")
        {
            if (_controller.Choice == 2)
            {
                _controller.OnBack();
                ProgressBar.Value = _controller.Progress;
                ChangeTextOnScreen();
            }
        }
        else
        {
            _controller.CurrentChar = e.Text[0];
            _controller.CheckIfLetterIsCorrect();
            MoveLetterToTypeBoxOnCanvas();
            if (!s_StatisticsController!.IsRunning)
            {
                s_StatisticsController.StartTimer();
            }

            ProgressBar.Value = _controller.Progress;
        }
    }

    //updates values on view
    private void ChangeTextOnScreen()
    {
        if (_controller.Choice == 2)
        {
            SetRichBox();
        }
        else
        {
            if (_controller.CharacterList.Count >= 1)
            {
                //Displays the content to the application
                LetterToTypeLabel.Content = string.Join(' ', _controller.CharacterList[0]);
                LettersTodoLabel.Content = string.Join(' ', _controller.CharacterList).Remove(0, 1);
                LettersTypedLabel.Content = string.Join(' ', _controller.TypedCharsList);
            }
        }

        SetLiveStatistics(this, new LiveStatisticsEventArgs(false));
    }

    private void SetRichBox()
    {
        RichTextBoxStory.Document.Blocks.Clear();
        var i = 0;
        var paragraph = new Paragraph();
        foreach (var typedChar in _controller.TypedCharsList)
        {
            var runColor = new Run();
            runColor.Text = typedChar.ToString();
            if (typedChar == _controller.CorrectCharsList[i])
            {
                runColor.Foreground = Brushes.Green;
            }
            else
            {
                if (typedChar == ' ')
                {
                    runColor.Text = "_";
                }

                runColor.Foreground = Brushes.Red;
            }

            paragraph.Inlines.Add(runColor);

            i++;
        }

        var runWhite = new Run();

        foreach (var charTotype in _controller.CharacterList) runWhite.Text += charTotype;

        paragraph.Inlines.Add(runWhite);
        RichTextBoxStory.Document.Blocks.Add(paragraph);
    }

    //updates statistics on view
    private void SetLiveStatistics(object? sender, LiveStatisticsEventArgs e)
    {
        Dispatcher?.InvokeAsync(() =>
        {
            if (e.SetTextRed)
            {
                LetterToTypeLabel.Foreground = Brushes.Red;
            }

            var statistics = s_StatisticsController.GetStatistics();
            LiveStatisticsScreen.Content = statistics;
            TimeLeftLabel.Content = s_StatisticsController.TimeLeft;
        });
    }

    //moves the highlighted box
    private void MoveLetterToTypeBoxOnCanvas() //Moves box on canvas that displays which letter has to be typed
    {
        try
        {
            if (_controller.CharacterList.Count >= 1)
            {
                var posX = _controller.Coordinates[_controller.CharacterList[0]][0]; //sets posx
                var posY = _controller.Coordinates[_controller.CharacterList[0]][1]; //sets posy

                if (_controller.CharacterList[0] == ' ')
                {
                    _rectangleLetterToType.Width = 359;
                }
                else
                {
                    _rectangleLetterToType.Width = 33;
                }

                Canvas.SetTop(_rectangleLetterToType, posY);
                Canvas.SetLeft(_rectangleLetterToType, posX);
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }

    //Moves box on canvas that displays which letter has to be typed
    private void MoveLetterTypedBoxOnCanvas(bool isGood, char charTyped)
    {
        try
        {
            var posX = _controller.Coordinates[charTyped][0]; //sets posx
            var posY = _controller.Coordinates[charTyped][1]; //sets posy
            _rectangleLetterTyped.Visibility = Visibility.Visible;
            if (charTyped == ' ')
            {
                _rectangleLetterTyped.Width = 359;
            }
            else
            {
                _rectangleLetterTyped.Width = 33;
            }

            if (isGood)
            {
                _rectangleLetterTyped.Fill = Brushes.LawnGreen;
            }
            else
            {
                _rectangleLetterTyped.Fill = Brushes.Red;
            }

            Canvas.SetTop(_rectangleLetterTyped, posY);
            Canvas.SetLeft(_rectangleLetterTyped, posX);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }

    //The back button top left
    private void OnBack(object sender, RoutedEventArgs e)
    {
        Cleanup();
        UserControlController.MainWindowChange(this, new ExerciseSelect());
    }

    //cleanup to prevent bugs
    private void Cleanup()
    {
        var window = Window.GetWindow(this);
        window.TextInput -= TextInputPress;
        _controller.CurrentChar = '.';
    }


    //Methods for events to fire
    private void MistakeMade()
    {
        //if the letter is wrong, add a mistake and update the screen
        if (_controller.Choice == 2)
        {
            s_StatisticsController?.WrongAnswer();
        }
        else
        {
            s_StatisticsController?.WrongAnswer(_controller.CurrentChar);
        }

        MoveLetterTypedBoxOnCanvas(false, _controller.CurrentChar);
        LetterToTypeLabel.Foreground = Brushes.Red;
    }

    private void ExerciseFinished()
    {
        //adds a empty space if the list is empty
        LetterToTypeLabel.Content = "";

        UserControlController.MainWindowChange(this, new ResultatenOefening());
    }

    private void CorrectAnswer()
    {
        MoveLetterTypedBoxOnCanvas(true, _controller.CurrentChar);

        s_StatisticsController?.RightAnswer();

        //Makes the letter black again
        LetterToTypeLabel.Foreground = Brushes.White;
    }

    //EVENTS
    private void ExerciseEvent(object? sender, ExerciseEventArgs e)
    {
        if (e.IsCorrect)
        {
            CorrectAnswer();
        }
        else
        {
            MistakeMade();
        }

        if (e.IsFinished)
        {
            ExerciseFinished();
        }

        ChangeTextOnScreen();
    }
}