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
		Controller.LetterExersiseController Letter = new Controller.LetterExersiseController();
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
				//if it is, remove the letter from the queue
				Letter.AlphabetList.RemoveAt(0);

				char dequeuedLetter = Letter.AlphabetQueue.Dequeue();

				LettersTypedLabel.Content += dequeuedLetter + " ";

				//and change the text on the screen
				ChangeTextOnScreen();
			}
			else
			{
				//if it is not, show a message box
				MessageBox.Show("Wrong letter!");
			}
		}

		private void HandleKeyPress(object sender, KeyEventArgs e)
		{
			//Do work
			CurrentLetter = e.Key.ToString().ToLower()[0];
			CheckIfLetterIsCorrect();
		}

		private void ChangeTextOnScreen()
		{
			//checks if the queue is empty
			if (Letter.AlphabetList.Count == 0)
			{
				//if it is, show a message box
				MessageBox.Show("You have finished the exercise!");
			}
			else
			{
				// Create a paragraph and add the Run and Bold to it.
				LetterToTypeLabel.Content = string.Join(' ', Letter.AlphabetList[0]);
				LettersTodoLabel.Content = string.Join(' ', Letter.AlphabetList).Remove(0, 1);
			}
		}
	}
}