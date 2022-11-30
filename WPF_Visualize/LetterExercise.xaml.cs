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

		private char CurrentLetter;
		Rectangle rectangle = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle

		public LetterExercise()
		{
			InitializeComponent();
			Letter = new Controller.LetterExerciseController();
			MoveBoxOnCanvas();
			ChangeTextOnScreen();
			KeyboardCanvas.Children.Add(rectangle); //adds rectangle on screen
		}

		/// <summary>
		/// Connects events to the button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			var window = Window.GetWindow(this);
			window.KeyDown += HandleKeyPress;
		}

		/// <summary>
		/// Logica to check if letter is right or wrong
		/// </summary>
		public void CheckIfLetterIsCorrect()
		{
			//checks if the last keypress is equal to the first letter in the queue
			if (Letter.AlphabetList[0] == CurrentLetter)
			{
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
				//if it is not, show a message box
				MessageBox.Show("Wrong letter!");
			}
		}

		/// <summary>
		/// Handles the keypresses from the userinput
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void HandleKeyPress(object sender, KeyEventArgs e)
		{
			CurrentLetter = e.Key.ToString().ToLower()[0];
			CheckIfLetterIsCorrect();
			MoveBoxOnCanvas();
		}

		/// <summary>
		/// Updates values on view
		/// </summary>
		private void ChangeTextOnScreen()
		{
			if (Letter.AlphabetList.Count >= 1)
			{
				//Displays the content to the application
				LetterToTypeLabel.Content = string.Join(' ', Letter.AlphabetList[0]);
				LettersTodoLabel.Content = string.Join(' ', Letter.AlphabetList).Remove(0, 1);
			}
		}

		/// <summary>
		/// Moves the highlighted box
		/// </summary>
		private void MoveBoxOnCanvas() //Moves box on canvas that displays which letter has to be typed
		{
			int PosX = Letter.Coordinates[Letter.AlphabetList[0]][0]; //sets posx
			int PosY = Letter.Coordinates[Letter.AlphabetList[0]][1]; //sets posy
			Canvas.SetTop(rectangle, PosY);
			Canvas.SetLeft(rectangle, PosX);
		}

		/// <summary>
		/// The back button that sits top-left of the application.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnBack(object sender, RoutedEventArgs e)
		{
			Cleanup();
			UserControlController.MainWindowChange(this, new ExerciseSelect());
		}

		/// <summary>
		/// Cleanup to prevent bugs
		/// </summary>
		private void Cleanup()
		{
			var window = Window.GetWindow(this);
			window.KeyDown -= HandleKeyPress;
			Letter = null;
			CurrentLetter = ' ';
		}
	}
}