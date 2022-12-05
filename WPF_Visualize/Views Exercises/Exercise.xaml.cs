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
		Controller.ExerciseController Controller;
        Controller.StatisticsController StatisticsController = new();

		Rectangle RectangleLetterTyped = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle
        Rectangle RectangleLetterToType = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle

		public Exercise(int choice)
		{
			InitializeComponent();
			Controller = new (choice);
			//subscribe events
			Controller.ExerciseEvent += ExerciseEvent;
            StatisticsController.LiveStatisticsEvent += SetLiveStatistics;

            MoveLetterToTypeBoxOnCanvas();
			ChangeTextOnScreen();
			KeyboardCanvas.Children.Add(RectangleLetterToType); //adds rectangle on screen
            KeyboardCanvas.Children.Add(RectangleLetterTyped);
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
			Controller.CurrentChar = e.Key.ToString().ToLower()[0];
			Controller.CheckIfLetterIsCorrect();
			MoveLetterToTypeBoxOnCanvas();
            StatisticsController.ResetTimeLeft();
            StatisticsController.StartTimer();
        }

		//updates values on view
		private void ChangeTextOnScreen()
		{
			if (Controller.CharacterList.Count >= 1)
			{
				//Displays the content to the application
				LetterToTypeLabel.Content = string.Join(' ', Controller.CharacterList[0]);
				LettersTodoLabel.Content = string.Join(' ', Controller.CharacterList).Remove(0, 1);
			}
			SetLiveStatistics(this, null);
		}

        //updates statistics on view
        private void SetLiveStatistics(object sender, LiveStatisticsEventArgs e)
		{
            this.Dispatcher?.Invoke(() =>
            {
                string Statistics = StatisticsController.GetStatistics();
                LiveStatisticsScreen.Content = Statistics;
                TimeLeftLabel.Content = StatisticsController.TimeLeft;
            });
        }

        //moves the highlighted box
        private void MoveLetterToTypeBoxOnCanvas() //Moves box on canvas that displays which letter has to be typed
		{
			if (Controller.CharacterList.Count >= 1)
			{
				int PosX = Controller.Coordinates[Controller.CharacterList[0]][0]; //sets posx
				int PosY = Controller.Coordinates[Controller.CharacterList[0]][1]; //sets posy
				Canvas.SetTop(RectangleLetterToType, PosY);
				Canvas.SetLeft(RectangleLetterToType, PosX);
			}
		}

		private void MoveLetterTypedBoxOnCanvas(bool IsGood, char charTyped) //Moves box on canvas that displays which letter has to be typed
		{
			int PosX = Controller.Coordinates[charTyped][0]; //sets posx
			int PosY = Controller.Coordinates[charTyped][1]; //sets posy
			if (IsGood)
			{
				RectangleLetterTyped.Fill = Brushes.Green;
			}
			else
			{
				RectangleLetterTyped.Fill = Brushes.Red;
			}
			Canvas.SetTop(RectangleLetterTyped, PosY);
			Canvas.SetLeft(RectangleLetterTyped, PosX);
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
			Controller.CurrentChar = ' ';
		}


		//Methods for events to fire
		private void MistakeMade()
		{
			//if the letter is wrong, add a mistake and update the screen
			StatisticsController.NumberOfMistakes++;
			MoveLetterTypedBoxOnCanvas(false, Controller.CurrentChar);
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
			MoveLetterTypedBoxOnCanvas(true, Controller.CurrentChar);

			StatisticsController.NumberCorrect++;

			this.LetterToTypeLabel.Foreground = Brushes.Black;

			//adds the letter that you typed to the left label
			LettersTypedLabel.Content += Controller.DequeuedChar + " ";
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