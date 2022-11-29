using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Model;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace WPF_Visualize
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class LetterExercise : UserControl
	{
		Controller.LetterExerciseController Letter;

		private char _currentLetter;
		private int _numberOfMistakes;
        private int _numberCorrect;
        private string _liveStatistics;
		Rectangle rectangleLetterTyped = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle
        Rectangle rectangleLetterToType = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle

		public LetterExercise()
		{
			InitializeComponent();
			Letter = new Controller.LetterExerciseController();
			_numberOfMistakes = 0;
			_numberCorrect = 0;
			_setStatisticsContent();
            MoveLetterToTypeBoxOnCanvas();
			ChangeTextOnScreen();
			KeyboardCanvas.Children.Add(rectangleLetterToType); //adds rectangle on screen
            KeyboardCanvas.Children.Add(rectangleLetterTyped);
        }

        private void _setStatisticsContent()
        {
			double PercentGood;
			if (_numberCorrect == 0)
			{
				PercentGood = 0;
			}else if(_numberOfMistakes == 0)
			{
				PercentGood = 100;
			}else
			{
				PercentGood = ((double)_numberCorrect / (double)(_numberCorrect + _numberOfMistakes)) * 100;
				PercentGood = Math.Round(PercentGood, 1);
            }
            this.Statistics.Content = $"{_numberOfMistakes} fout \r\n{PercentGood}% goed \r\n10 s";
        }

        //Connects events to the button 
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			var window = Window.GetWindow(this);
			window.KeyDown += HandleKeyPress;
		}

		//logica to check if letter is right or wrong
		public void CheckIfLetterIsCorrect()
		{
			//checks if the last keypress is equal to the first letter in the queue
			if (Letter.AlphabetList[0] == _currentLetter)
			{
                MoveLetterTypedBoxOnCanvas(true, _currentLetter);
                _numberCorrect++;
                this.LetterToTypeLabel.Background = this.LettersTodoLabel.Background;
                //checks if list isnt empty
                if (Letter.AlphabetList.Count >= 1)
				{
					//if it is, remove the letter from the queue
					Letter.AlphabetList.RemoveAt(0);

					char dequeuedLetter = Letter.AlphabetQueue.Dequeue();

					//adds the letter that you typed to the left label
					LettersTypedLabel.Content += dequeuedLetter + " ";

					//and change the text on the screen
					ChangeTextOnScreen();
				}

				if (Letter.AlphabetList.Count == 0)
				{
					//adds a empty space if the list is empty
					LetterToTypeLabel.Content = " ";

					//show a message box
					MessageBox.Show("You have finished the exercise!");
				}
			}
			else
			{
                //if the letter is wrong, add a mistake and update the screen
                _numberOfMistakes++;
                MoveLetterTypedBoxOnCanvas(false, _currentLetter);
                _setStatisticsContent();
                this.LetterToTypeLabel.Background = Brushes.Red;

            }
		}

		//Handles the keypresses from the userinput
		private void HandleKeyPress(object sender, KeyEventArgs e)
		{
			_currentLetter = e.Key.ToString().ToLower()[0];
			CheckIfLetterIsCorrect();
			MoveLetterToTypeBoxOnCanvas();
		}

		//updates values on view
		private void ChangeTextOnScreen()
		{
			if (Letter.AlphabetList.Count >= 1)
			{
				//Displays the content to the application
				LetterToTypeLabel.Content = string.Join(' ', Letter.AlphabetList[0]);
				LettersTodoLabel.Content = string.Join(' ', Letter.AlphabetList).Remove(0, 1);
				_setStatisticsContent();
			}
            
		}

		//moves the highlighted box
		private void MoveLetterToTypeBoxOnCanvas() //Moves box on canvas that displays which letter has to be typed
		{
			int PosX = Letter.Coordinates[Letter.AlphabetList[0]][0]; //sets posx
			int PosY = Letter.Coordinates[Letter.AlphabetList[0]][1]; //sets posy
			Canvas.SetTop(rectangleLetterToType, PosY);
			Canvas.SetLeft(rectangleLetterToType, PosX);
		}

        private void MoveLetterTypedBoxOnCanvas(bool IsGood, char charTyped) //Moves box on canvas that displays which letter has to be typed
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
			Cleanup();
			UserControlController.InvokeEvent(this, new ExerciseSelect());
		}

		//cleanup to prevent bugs
		private void Cleanup()
		{
			var window = Window.GetWindow(this);
			window.KeyDown -= HandleKeyPress;
			Letter = null;
			_currentLetter = ' ';
		}
	}
}