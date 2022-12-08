using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Controller;
using WPF_Visualize.ViewLogic;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace WPF_Visualize;

/// <summary>
///     Interaction logic for StoryExercise.xaml
/// </summary>

public partial class StoryExercise : UserControl
    {
        Controller.ExerciseController _controller;
        Controller.StatisticsController _statisticsController = new();
        
        Rectangle _rectangleLetterTyped = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle
        Rectangle _rectangleLetterToType = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle
        
        public StringBuilder _sb = new StringBuilder();
        public StoryExercise()
        {
            InitializeComponent();
            _controller = new(2);
            //subscribe events
            _controller.ExerciseEvent += ExerciseEvent;
            _statisticsController.LiveStatisticsEvent += SetLiveStatistics;
            
        
            //rectangles for the letters
            KeyboardCanvas.Children.Add(_rectangleLetterToType); //adds rectangle on screen
            KeyboardCanvas.Children.Add(_rectangleLetterTyped);
            _rectangleLetterTyped.Visibility = Visibility.Hidden;
        
            //Add the text to the textbox

            ChangeTextOnScreen();
        }
        
        //Connects events to the button 
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;
            // window.KeyUp += HandleKeyUp;
        }
        
        //Handles the keypresses from the userinput
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
        
            if (e.Key.ToString().Equals("Space"))
            {
                _controller.CurrentChar = ' ';
            }
            else if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                if(e.Key.ToString().Length == 1)
                {
                    _controller.CurrentChar = e.Key.ToString().ToUpper()[0];
                }
            
            }
            else
            {
                if (e.Key.ToString().Length == 1)
                {
                    _controller.CurrentChar = e.Key.ToString().ToLower()[0];
                } 
            }
            _controller.CheckIfLetterIsCorrect();
        }


        //updates values on view
        private void ChangeTextOnScreen()
        {            

            RichTextBoxStory.Document.Blocks.Clear();
            SetRichBox();
        }

        private void SetRichBox()
        {
            int i = 0;
            Paragraph paragraph = new Paragraph();
            foreach (Char typedChar in _controller.TypedCharsList)
            {
                Run runColor = new Run();
                runColor.Text = typedChar.ToString();
                if (typedChar == _controller.CorrectCharsList[i])
                {
                    runColor.Foreground = Brushes.Green;
                }
                else
                {
                    runColor.Foreground = Brushes.Red;
                }
                paragraph.Inlines.Add(runColor);
                i++;
            }
            
            Run runBlack = new Run();
            runBlack.Foreground = Brushes.Black;
            foreach (char charTotype in _controller.CharacterList)
            {
                runBlack.Text += charTotype;
            }
            paragraph.Inlines.Add(runBlack);
            RichTextBoxStory.Document.Blocks.Add(paragraph);
        }
        
        //updates statistics on view
        private void SetLiveStatistics(object sender, LiveStatisticsEventArgs e)
        {
            //this.Dispatcher?.Invoke(() =>
            //{
            //    string statistics = _statisticsController.GetStatistics();
            //    LiveStatisticsScreen.Content = statistics;
            //    TimeLeftLabel.Content = _statisticsController.TimeLeft;
            //});
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
        
        
        private void ExerciseFinished()
        { 
            UserControlController.MainWindowChange(this, new ResultatenOefening());
        }

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

        private void MoveLetterTypedBoxOnCanvas(bool isGood, char charTyped) //Moves box on canvas that displays which letter has to be typed
        {
            var posX = _controller.Coordinates[charTyped][0]; //sets posx
            var posY = _controller.Coordinates[charTyped][1]; //sets posy
            _rectangleLetterTyped.Visibility = Visibility.Visible;
            if (charTyped == ' ')
                _rectangleLetterTyped.Width = 359;
            else
                _rectangleLetterTyped.Width = 33;
            if (!isGood)
                _rectangleLetterTyped.Fill = Brushes.LawnGreen;
            else
                _rectangleLetterTyped.Fill = Brushes.Red;
            Canvas.SetTop(_rectangleLetterTyped, posY);
            Canvas.SetLeft(_rectangleLetterTyped, posX);
        }
        
        //EVENTS
        private void ExerciseEvent(object sender, ExerciseEventArgs e)
        {
            if (e.IsFinished)
            {
                ExerciseFinished();
            }
            ChangeTextOnScreen();
            MoveLetterTypedBoxOnCanvas(e.IsCorrect, _controller.CurrentChar);
            MoveLetterToTypeBoxOnCanvas();
            
        }
    }
