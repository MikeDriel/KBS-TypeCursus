using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Controller;
using WPF_Visualize.ViewLogic;

namespace WPF_Visualize;

/// <summary>
///     Interaction logic for UserControl1.xaml
/// </summary>
public partial class Exercise : UserControl
{
    public static StatisticsController? StatisticsController;
    private readonly ExerciseController _controller;

    private readonly Rectangle _rectangleLetterToType =
        new() { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle

    private readonly Rectangle _rectangleLetterTyped =
        new() { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle

    public Exercise(int choice)
    {
        InitializeComponent();
        _controller = new ExerciseController(choice);
        StatisticsController = new StatisticsController();
        //subscribe events
        _controller.ExerciseEvent += ExerciseEvent;
        StatisticsController.LiveStatisticsEvent += SetLiveStatistics;

        MoveLetterToTypeBoxOnCanvas();
        ChangeTextOnScreen();
        InitializeProgressBar();
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
        window.KeyDown += HandleKeyPress;
    }

    //Handles the keypresses from the user-input
    private void HandleKeyPress(object sender, KeyEventArgs e)
    {
        if (e.Key.ToString().Equals("Space"))
            _controller.CurrentChar = ' ';
        else
            _controller.CurrentChar = e.Key.ToString().ToLower()[0];

        _controller.CheckIfLetterIsCorrect();
        MoveLetterToTypeBoxOnCanvas();
        if (!StatisticsController!.IsRunning) StatisticsController.StartTimer();
        ProgressBar.Value = _controller.Progress;
    }

    //updates values on view
    private void ChangeTextOnScreen()
    {
        if (_controller.CharacterList.Count >= 1)
        {
            //Displays the content to the application
            LetterToTypeLabel.Content = string.Join(' ', _controller.CharacterList[0]);
            LettersTodoLabel.Content = string.Join(' ', _controller.CharacterList).Remove(0, 1);
            LettersTypedLabel.Content = string.Join(' ', _controller.TypedChars);
        }

        SetLiveStatistics(this, new LiveStatisticsEventArgs(false));
    }

    //updates statistics on view
    private void SetLiveStatistics(object? sender, LiveStatisticsEventArgs e)
    {
        Dispatcher?.InvokeAsync(() =>
        {
            if (e.SetTextRed) LetterToTypeLabel.Foreground = Brushes.Red;
            var statistics = StatisticsController.GetStatistics();
            LiveStatisticsScreen.Content = statistics;
            TimeLeftLabel.Content = StatisticsController.TimeLeft;
        });
    }

    //moves the highlighted box
    private void MoveLetterToTypeBoxOnCanvas() //Moves box on canvas that displays which letter has to be typed
    {
        if (_controller.CharacterList.Count >= 1)
        {
            var posX = _controller.Coordinates[_controller.CharacterList[0]][0]; //sets posx
            var posY = _controller.Coordinates[_controller.CharacterList[0]][1]; //sets posy

            if (_controller.CharacterList[0] == ' ')
                _rectangleLetterToType.Width = 359;
            else
                _rectangleLetterToType.Width = 33;

            Canvas.SetTop(_rectangleLetterToType, posY);
            Canvas.SetLeft(_rectangleLetterToType, posX);
        }
    }

    private void
        MoveLetterTypedBoxOnCanvas(bool isGood,
            char charTyped) //Moves box on canvas that displays which letter has to be typed
    {
        var posX = _controller.Coordinates[charTyped][0]; //sets posx
        var posY = _controller.Coordinates[charTyped][1]; //sets posy
        _rectangleLetterTyped.Visibility = Visibility.Visible;
        if (charTyped == ' ')
            _rectangleLetterTyped.Width = 359;
        else
            _rectangleLetterTyped.Width = 33;
        if (isGood)
            _rectangleLetterTyped.Fill = Brushes.LawnGreen;
        else
            _rectangleLetterTyped.Fill = Brushes.Red;
        Canvas.SetTop(_rectangleLetterTyped, posY);
        Canvas.SetLeft(_rectangleLetterTyped, posX);
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
        window.KeyDown -= HandleKeyPress;
        _controller.CurrentChar = '.';
    }


    //Methods for events to fire
    private void MistakeMade()
    {
        //if the letter is wrong, add a mistake and update the screen
        StatisticsController?.WrongAnswer(_controller.CurrentChar);
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

        StatisticsController?.RightAnswer();

        //Makes the letter black again
        LetterToTypeLabel.Foreground = Brushes.White;
    }

    //EVENTS
    private void ExerciseEvent(object? sender, ExerciseEventArgs e)
    {
        if (e.IsCorrect)
            CorrectAnswer();
        else
            MistakeMade();
        if (e.IsFinished) ExerciseFinished();

        ChangeTextOnScreen();
    }
}