using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Controller;
using Model;
using WPF_Visualize.ViewLogic;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace WPF_Visualize
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	/// 
	
    public partial class LetterExercise : UserControl
	{
        

        Controller.ExerciseController Letter = new Controller.ExerciseController();
        Controller.StatisticsController StatisticsController = new Controller.StatisticsController();


		Rectangle rectangleLetterTyped = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle
        Rectangle rectangleLetterToType = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle

		public LetterExercise()
		{
            InitializeComponent();

			//subscribe events
			Letter.ExerciseEvent += _exerciseEvent;
            StatisticsController.LiveStatisticsEvent += _setLiveStatistics;

            _moveLetterToTypeBoxOnCanvas();
			_changeTextOnScreen();
			KeyboardCanvas.Children.Add(rectangleLetterToType); //adds rectangle on screen
            KeyboardCanvas.Children.Add(rectangleLetterTyped);
        }

        //Connects events to the button 
        private void _userControl_Loaded(object sender, RoutedEventArgs e)
		{
			var window = Window.GetWindow(this);
			window.KeyDown += HandleKeyPress;
		}
		
        //Handles the keypresses from the userinput
        private void HandleKeyPress(object sender, KeyEventArgs e)
		{
			Letter.CurrentLetter = e.Key.ToString().ToLower()[0];
			Letter.CheckIfLetterIsCorrect();
			_moveLetterToTypeBoxOnCanvas();
            StatisticsController.ResetTimeLeft();
            StatisticsController.StartTimer();
        }

		//updates values on view
		private void _changeTextOnScreen()
		{
			if (Letter.AlphabetList.Count >= 1)
			{
				//Displays the content to the application
				LetterToTypeLabel.Content = string.Join(' ', Letter.AlphabetList[0]);
				LettersTodoLabel.Content = string.Join(' ', Letter.AlphabetList).Remove(0, 1);
			}
			_setLiveStatistics(this, null);
		}

        //updates statistics on view
        private void _setLiveStatistics(object sender, LiveStatisticsEventArgs e)
		{
            this.Dispatcher?.Invoke(() =>
            {
                string Statistics = StatisticsController.GetStatistics();
                LiveStatisticsScreen.Content = Statistics;
                TimeLeftLabel.Content = StatisticsController.TimeLeft;
            });
        }

        //moves the highlighted box
        private void _moveLetterToTypeBoxOnCanvas() //Moves box on canvas that displays which letter has to be typed
		{
			if (Letter.AlphabetList.Count >= 1)
			{
				int PosX = Letter.Coordinates[Letter.AlphabetList[0]][0]; //sets posx
				int PosY = Letter.Coordinates[Letter.AlphabetList[0]][1]; //sets posy
				Canvas.SetTop(rectangleLetterToType, PosY);
				Canvas.SetLeft(rectangleLetterToType, PosX);
			}
		}

        private void _moveLetterTypedBoxOnCanvas(bool IsGood, char charTyped) //Moves box on canvas that displays which letter has to be typed
        {
            int PosX = Letter.Coordinates[charTyped][0]; //sets posx
            int PosY = Letter.Coordinates[charTyped][1]; //sets posy
			if (IsGood)
			{
                rectangleLetterTyped.Fill = Brushes.Green;
            }else
			{
                rectangleLetterTyped.Fill = Brushes.Red;
            }
            Canvas.SetTop(rectangleLetterTyped, PosY);
            Canvas.SetLeft(rectangleLetterTyped, PosX);
        }

        //The back button top left
        private void OnBack(object sender, RoutedEventArgs e)
		{
			_cleanup();
			UserControlController.MainWindowChange(this, new ExerciseSelect());
		}

		//cleanup to prevent bugs
		private void _cleanup()
		{
			var window = Window.GetWindow(this);
			window.KeyDown -= HandleKeyPress;
			Letter.CurrentLetter = ' ';
		}


		//Methods for events to fire
		private void _mistakeMade()
		{
			//if the letter is wrong, add a mistake and update the screen
			StatisticsController.NumberOfMistakes++;
			_moveLetterTypedBoxOnCanvas(false, Letter.CurrentLetter);
			this.LetterToTypeLabel.Foreground = Brushes.Red;
		}

		private void _exerciseFinished()
		{
			//adds a empty space if the list is empty
			LetterToTypeLabel.Content = " ";

			//show a message box
			MessageBox.Show("You have finished the exercise!");
			UserControlController.MainWindowChange(this, new ResultatenOefening());
		}

		private void _correctAnswer()
		{
			_moveLetterTypedBoxOnCanvas(true, Letter.CurrentLetter);

			StatisticsController.NumberCorrect++;

			this.LetterToTypeLabel.Foreground = Brushes.Black;

			//adds the letter that you typed to the left label
			LettersTypedLabel.Content += Letter.DequeuedLetter + " ";
		}

		//EVENTS
		private void _exerciseEvent(object sender, ExerciseEventArgs e)
		{
			if (e.IsCorrect)
			{
				_correctAnswer();
			}
			else
			{
				_mistakeMade();
			}
			if (e.IsFinished)
			{
				_exerciseFinished();
			}
			
			_changeTextOnScreen();
		}
	}
}