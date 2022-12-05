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
        Controller.StatisticsController _statisticsController = new();

        Rectangle _rectangleLetterTyped = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle
        Rectangle _rectangleLetterToType = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle

        public StringBuilder _sb = new StringBuilder();
        public StoryExercise(int choice)
        {
            InitializeComponent();
            _controller = new(choice);
            //subscribe events
            _controller.ExerciseEvent += ExerciseEvent;
            _statisticsController.LiveStatisticsEvent += SetLiveStatistics;

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
        }

        //Handles the keypresses from the userinput
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString().Equals("Space"))
            {
                _controller.CurrentChar = ' ';
            }
            else
            {
                _controller.CurrentChar = e.Key.ToString().ToLower()[0];
            }

            _controller.CheckIfLetterIsCorrect();
            _statisticsController.ResetTimeLeft();
            _statisticsController.StartTimer();
        }

        //updates values on view
        private void ChangeTextOnScreen()
        {
            string Story = "Het leven is een tekening die je inkleurt. Op 5 december komt Sinterklaas met zwarte Piet naar jouw schoorsteen toe.";
                //Displays the content to the application
                StoryTextBox.AppendText(Story);
           
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
            _controller.CurrentChar = '.';
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
