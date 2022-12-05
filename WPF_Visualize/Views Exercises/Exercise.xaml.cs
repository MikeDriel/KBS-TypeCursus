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
	
    public partial class Exercise : UserControl
	{
		Controller.ExerciseController _controller;
        Controller.StatisticsController _statisticsController = new();

		Rectangle _rectangleLetterTyped = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle
        Rectangle _rectangleLetterToType = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle

        public StringBuilder sb = new StringBuilder();
		public Exercise(int choice)
		{
			InitializeComponent();
			_controller = new (choice);
			//subscribe events
			_controller.ExerciseEvent += ExerciseEvent;
            _statisticsController.LiveStatisticsEvent += SetLiveStatistics;

            MoveLetterToTypeBoxOnCanvas();
			ChangeTextOnScreen();
			KeyboardCanvas.Children.Add(_rectangleLetterToType); //adds rectangle on screen
            KeyboardCanvas.Children.Add(_rectangleLetterTyped);
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
			MoveLetterToTypeBoxOnCanvas();
            _statisticsController.ResetTimeLeft();
            _statisticsController.StartTimer();
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

        //moves the highlighted box
        private void MoveLetterToTypeBoxOnCanvas() //Moves box on canvas that displays which letter has to be typed
		{
			if (_controller.CharacterList.Count >= 1)
			{
				int posX = _controller.Coordinates[_controller.CharacterList[0]][0]; //sets posx
				int posY = _controller.Coordinates[_controller.CharacterList[0]][1]; //sets posy
				
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

		private void MoveLetterTypedBoxOnCanvas(bool isGood, char charTyped) //Moves box on canvas that displays which letter has to be typed
		{
			int posX = _controller.Coordinates[charTyped][0]; //sets posx
			int posY = _controller.Coordinates[charTyped][1]; //sets posy
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
				_rectangleLetterTyped.Fill = Brushes.Green;
			}
			else
			{
				_rectangleLetterTyped.Fill = Brushes.Red;
			}
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
			_statisticsController.NumberOfMistakes++;
			MoveLetterTypedBoxOnCanvas(false, _controller.CurrentChar);
			this.LetterToTypeLabel.Foreground = Brushes.Red;
		}

		private void ExerciseFinished()
		{
			//adds a empty space if the list is empty
			LetterToTypeLabel.Content = "";

			//show a message box
			MessageBox.Show("You have finished the exercise!");
			UserControlController.MainWindowChange(this, new ResultatenOefening());
		}

		private void CorrectAnswer()
		{
			MoveLetterTypedBoxOnCanvas(true, _controller.CurrentChar);

			_statisticsController.NumberCorrect++;

			this.LetterToTypeLabel.Foreground = Brushes.Black;

			//adds the letter that you typed to the left label
			//LettersTypedLabel.Content += _controller.DequeuedChar + " ";
			
			//sb.Append(_controller.DequeuedChar);
			//LettersTypedLabel.Content = sb.ToString();
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