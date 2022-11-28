using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WPF_Visualize
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class LetterExercise : UserControl
	{
		Controller.LetterExerciseController Letter = new Controller.LetterExerciseController();
		private char CurrentLetter;

		public LetterExercise()
		{
			InitializeComponent();
			ChangeTextOnScreen();
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

		//Handles the keypresses from the userinput
		private void HandleKeyPress(object sender, KeyEventArgs e)
		{
			CurrentLetter = e.Key.ToString().ToLower()[0];
			CheckIfLetterIsCorrect();
		}

		private void ChangeTextOnScreen()
		{
			if (Letter.AlphabetList.Count >= 1)
			{
				//Displays the content to the application
				LetterToTypeLabel.Content = string.Join(' ', Letter.AlphabetList[0]);
				LettersTodoLabel.Content = string.Join(' ', Letter.AlphabetList).Remove(0, 1);
			}
		}
	}
}