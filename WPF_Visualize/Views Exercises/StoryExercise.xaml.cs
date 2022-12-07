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

namespace WPF_Visualize
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    /// 

    public partial class StoryExercise : UserControl
    {
        Controller.ExerciseController _controller;
        Controller.StoryExerciseController _storyController;
        Controller.StatisticsController _statisticsController = new();
        
        Rectangle _rectangleLetterTyped = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle
        Rectangle _rectangleLetterToType = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle

    
       







    public StringBuilder _sb = new StringBuilder();
        public StoryExercise()
        {
            InitializeComponent();
            _storyController = new();
            //subscribe events
            _storyController.ExerciseEvent += ExerciseEvent;
            _statisticsController.LiveStatisticsEvent += SetLiveStatistics;

            _storyController.SetCharListBack();
            ChangeTextOnScreen();
            KeyboardCanvas.Children.Add(_rectangleLetterToType); //adds rectangle on screen
            KeyboardCanvas.Children.Add(_rectangleLetterTyped);
            _rectangleLetterTyped.Visibility = Visibility.Hidden;
        }

        //Connects events to the button 
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;
            window.KeyUp += HandleKeyUp;
        }

        //Handles the keypresses from the userinput
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {

            if (e.Key.ToString().Equals("Space"))
            {
                _storyController._currentChar = ' ';
            }
            else 
            {
                if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                {
                    if(e.Key.ToString().Length == 1)
                    {
                        _storyController._currentChar = e.Key.ToString().ToUpper()[0];
                    }
                    
                }
                else
                {
                    if (e.Key.ToString().Length == 1)
                    {
                        _storyController._currentChar = e.Key.ToString().ToLower()[0];
                    } 
                }
                
            }


            _storyController.CheckIfLetterIsCorrectStory();
            _statisticsController.ResetTimeLeft();
            _statisticsController.StartTimer();
        }


        //updates values on view
        private void ChangeTextOnScreen()
        {
            StoryTextBoxBack.SelectAll();
            StoryTextBoxBack.Selection.Text = "";
            foreach (var item in _storyController._charListBack)
            {
                StoryTextBoxBack.AppendText(item.ToString());
            }

            StoryTextBoxFront.SelectAll();
            StoryTextBoxFront.Selection.Text = "";
            foreach (var item in _storyController._charListFront)
            {
                StoryTextBoxFront.AppendText(item.ToString());
            }


            



            //StoryTextBoxBack.AppendText(_charListBack.ToString());
            //StoryTextBoxFront.AppendText(_charListFront.ToString());
            SetLiveStatistics(this, null);
        }

        //updates statistics on view
        private void SetLiveStatistics(object sender, LiveStatisticsEventArgs e)
        {
            this.Dispatcher?.Invoke(() =>
            {
                string statistics = _statisticsController.GetStatistics();
                LiveStatisticsScreen.Content = statistics;
                TimeLeftLabel.Content = _statisticsController.TimeLeft;
            });
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
            _storyController._currentChar = '.';
        }

   

   



        //Methods for events to fire
        private void MistakeMade()
        {
            //if the letter is wrong, add a mistake and update the screen
            _statisticsController.NumberOfMistakes++;
        }

        private void ExerciseFinished()
        {
            //adds a empty space if the list is empty

            //show a message box
            MessageBox.Show("You have finished the exercise!");
            UserControlController.MainWindowChange(this, new ResultatenOefening());
        }

        private void CorrectAnswer()
        {

            _statisticsController.NumberCorrect++;

            //Makes the letter black again
        }

        //EVENTS
        private void ExerciseEvent(object sender, ExerciseEventArgs e)
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
}
