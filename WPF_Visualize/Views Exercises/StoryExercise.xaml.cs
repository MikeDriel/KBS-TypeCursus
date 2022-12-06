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
        Controller.StatisticsController _statisticsController = new();
        
        Rectangle _rectangleLetterTyped = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle
        Rectangle _rectangleLetterToType = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle

        int _typingIndex = 0;
        List<char> _charListBackCorrect = new List<char>();
        List<char> _charListBack = new List<char>();
        List<char> _charListFront = new List<char>();
        string Story = "Het leven is een tekening die je inkleurt. Op 5 december komt Sinterklaas met zwarte Piet naar jouw schoorsteen toe.";
        







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
            _charListBack = _charListBackCorrect;


            //if key is same
            if (e.Key.ToString()[0] == _charListBackCorrect[_typingIndex])
            {
 
                //if key is space
                if (e.Key.ToString().Equals("Space"))
                {
                    //_charListFront + space
                    _charListFront.Add(' ');
                    _typingIndex++;
                }
                //key is not space
                else
                {
                    //_charListFront + e.key
                    char character = char.Parse(e.Key.ToString());
                    _charListFront.Add(character);


                    _typingIndex++;
                }
            }
            //if key is not the same
            else
            {
                //_charListFront + e.key
                //remove letter on back text when key is wrong 
                //do not print wrong letter after first wrong letter
                _charListBack[_typingIndex] = ' ';
                char character = char.Parse(e.Key.ToString());
                _charListFront.Add(character);
                _typingIndex++;
            }

            _statisticsController.ResetTimeLeft();
            _statisticsController.StartTimer();
        }

        //updates values on view
        private void ChangeTextOnScreen()
        {
            //Story = "Het leven is een tekening die je inkleurt. Op 5 december komt Sinterklaas met zwarte Piet naar jouw schoorsteen toe.";
            //Displays the content to the application

            foreach (char character in Story)
            {
                _charListBackCorrect.Add(character);
            }

            foreach (var item in _charListBack)
            {
                StoryTextBoxBack.AppendText(item.ToString());
            }

            StoryTextBoxBack.AppendText(Story);

            foreach (var item in _charListFront)
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
