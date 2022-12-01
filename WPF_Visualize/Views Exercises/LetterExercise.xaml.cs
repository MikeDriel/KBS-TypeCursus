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
	public partial class LetterExercise : UserControl
	{
		Controller.ExerciseController Letter = new();


		private int _numberOfMistakes = 0;
		private int _numberCorrect = 0;
		private DispatcherTimer _timer = new();
		private DateTime _currentTime;
		private int _timeLeft;
		public int MaxTimePerKey = 5;
		Rectangle _rectangleLetterTyped = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle
		Rectangle _rectangleLetterToType = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle

		public LetterExercise()
		{
			InitializeComponent();

			//subscribe events
			Letter.ExerciseEvent += ExerciseEvent;

			MoveLetterToTypeBoxOnCanvas();
			ChangeTextOnScreen();
			KeyboardCanvas.Children.Add(_rectangleLetterToType); //adds rectangle on screen
			KeyboardCanvas.Children.Add(_rectangleLetterTyped);
		}

		private void SetStatisticsContent()
		{
			double PercentGood;
			if (_numberCorrect == 0)
			{
				PercentGood = 0;
			}
			else if (_numberOfMistakes == 0)
			{
				PercentGood = 100;
			}
			else
			{
				PercentGood = ((double)_numberCorrect / (double)(_numberCorrect + _numberOfMistakes)) * 100;
				PercentGood = Math.Round(PercentGood, 1);
			}
			this.Statistics.Content = $"{_numberOfMistakes} fout \r\n{PercentGood}% goed \r\n{_currentTime.ToString("mm:ss")}";
		}

		//Connects events to the button 
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			var window = Window.GetWindow(this);
			if (window != null) window.KeyDown += HandleKeyPress;
		}

		private void OnTimer(object sender, EventArgs e)
		{
			if (_timeLeft == 0)
			{
				_numberOfMistakes++;
				_timeLeft = MaxTimePerKey;
			}
			this.TimeLeftLabel.Content = _timeLeft;
			_currentTime = _currentTime.AddSeconds(1);
			_timeLeft--;
			SetStatisticsContent();
		}

		//Handles the keypresses from the userinput
		private void HandleKeyPress(object sender, KeyEventArgs e)
		{
			Letter.CurrentChar = e.Key.ToString().ToLower()[0];
			Letter.CheckIfLetterIsCorrect();
			MoveLetterToTypeBoxOnCanvas();
			_timeLeft = MaxTimePerKey;
			if (!_timer.IsEnabled)
			{
				_timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Background, OnTimer, Dispatcher.CurrentDispatcher);
			}
		}

		//updates values on view
		private void ChangeTextOnScreen()
		{
			if (Letter.CharacterList.Count >= 1)
			{
				//Displays the content to the application
				LetterToTypeLabel.Content = string.Join(' ', Letter.CharacterList[0]);
				LettersTodoLabel.Content = string.Join(' ', Letter.CharacterList).Remove(0, 1);
			}
			SetStatisticsContent();
		}

		//moves the highlighted box
		private void MoveLetterToTypeBoxOnCanvas() //Moves box on canvas that displays which letter has to be typed
		{
			if (Letter.CharacterList.Count >= 1)
			{
				int PosX = Letter.Coordinates[Letter.CharacterList[0]][0]; //sets posx
				int PosY = Letter.Coordinates[Letter.CharacterList[0]][1]; //sets posy
				Canvas.SetTop(_rectangleLetterToType, PosY);
				Canvas.SetLeft(_rectangleLetterToType, PosX);
			}
		}

		private void MoveLetterTypedBoxOnCanvas(bool IsGood, char charTyped) //Moves box on canvas that displays which letter has to be typed
		{
			int PosX = Letter.Coordinates[charTyped][0]; //sets posx
			int PosY = Letter.Coordinates[charTyped][1]; //sets posy
			if (IsGood)
			{
				_rectangleLetterTyped.Fill = Brushes.Green;
			}
			else
			{
				_rectangleLetterTyped.Fill = Brushes.Red;
			}
			Canvas.SetTop(_rectangleLetterTyped, PosY);
			Canvas.SetLeft(_rectangleLetterTyped, PosX);
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
			Letter.CurrentChar = ' ';
		}


		//Methods for events to fire
		private void MistakeMade()
		{
			//if the letter is wrong, add a mistake and update the screen
			_numberOfMistakes++;
			MoveLetterTypedBoxOnCanvas(false, Letter.CurrentChar);
			this.LetterToTypeLabel.Foreground = Brushes.Red;
		}

		private void ExerciseFinished()
		{
			//adds a empty space if the list is empty
			LetterToTypeLabel.Content = " ";

			//show a message box
			MessageBox.Show("You have finished the exercise!");
			UserControlController.MainWindowChange(this, new ResultatenOefening());
		}

		private void CorrectAnswer()
		{
			MoveLetterTypedBoxOnCanvas(true, Letter.CurrentChar);

			_numberCorrect++;

			this.LetterToTypeLabel.Foreground = Brushes.Black;

			//adds the letter that you typed to the left label
			LettersTypedLabel.Content += Letter.DequeuedChar + " ";
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