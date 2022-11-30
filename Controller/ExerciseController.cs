using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
	public class ExerciseController
	{


		// events
		public event EventHandler<ExerciseEventArgs> ExerciseEvent;
		
		public List<char> AlphabetList { get; set; } //list which holds all the letters of the alphabet
		public Queue<char> AlphabetQueue { get; set; } //queue which holds all the letters of the alphabet
		public Dictionary<char, int[]> Coordinates { get; set; }
		
		public Random random = new Random();
		public char CurrentLetter {get; set;} //the current letter that is being typed

		public char DequeuedLetter { get; set; }

		public ExerciseController()
		{
			AlphabetList = new List<char>();
			AlphabetQueue = new Queue<char>();
			
			//Coordinates
			Coordinates = new Dictionary<char, int[]>() //Makes dictionary with every coordinate for the canvas to display the rectangle
			{
                {'a', new int[] { 73, 85 } },
                {'b', new int[] { 257, 127 } },
                {'c', new int[] { 175, 127 } },
                {'d', new int[] { 155, 85 } },
                {'e', new int[] { 143, 43 } },
                {'f', new int[] { 196, 85 } },
                {'g', new int[] { 237, 84 } },
                {'h', new int[] { 278, 85 } },
                {'i', new int[] { 347, 43 } },
                {'j', new int[] { 319, 85 } },
                {'k', new int[] { 360, 85 } },
                {'l', new int[] { 401, 85 } },
                {'m', new int[] { 339, 127 } },
                {'n', new int[] { 298, 127 } },
                {'o', new int[] { 388, 43 } },
                {'p', new int[] { 429, 43 } },
                {'q', new int[] { 62, 43 } },
                {'r', new int[] { 184, 43 } },
                {'s', new int[] { 114, 85 } },
                {'t', new int[] { 224, 43 } },
                {'u', new int[] { 306, 43 } },
                {'v', new int[] { 216, 127 } },
                {'w', new int[] { 102, 43 } },
                {'x', new int[] { 134, 127 } },
                {'y', new int[] { 265, 43 } },
                {'z', new int[] { 93, 127 } },
            };

            GenerateLetterData();
		}

		//generates the alphabet data for the list. Also copies data to the queue for logic use
		public void GenerateLetterData()
		{
			for (int i = 0; i < 26; i++)
			{
				AlphabetList.Add((char)(i + 97));
			}
			RandomizeAlphabet();

			foreach (char letter in AlphabetList)
			{
				AlphabetQueue.Enqueue(letter);
			}
		}

		public void RandomizeAlphabet()
		{
			//randomize the alphabet
			AlphabetList = AlphabetList.OrderBy(x => random.Next()).ToList();
		}

		//logica to check if letter is right or wrong
		public void CheckIfLetterIsCorrect()
		{
			//checks if the last keypress is equal to the first letter in the queue
			if (AlphabetList[0] == CurrentLetter)
			{
			

				//checks if list isnt empty
				if (AlphabetList.Count >= 1)
				{
					//if it is, remove the letter from the queue
					AlphabetList.RemoveAt(0);
					
					DequeuedLetter = AlphabetQueue.Dequeue();

					ExerciseEvent?.Invoke(this, new ExerciseEventArgs(true, false));
				}
				if (AlphabetList.Count == 0)
				{
					ExerciseEvent?.Invoke(this, new ExerciseEventArgs(true, true));
				}
			}
			else
			{
				ExerciseEvent?.Invoke(this, new ExerciseEventArgs(false, false));
			}
		}		
	}



	//EVENT FOR EXCERCISE
	public class ExerciseEventArgs : EventArgs
	{
		public bool IsCorrect;
		public bool IsFinished;

		public ExerciseEventArgs(bool isCorrect, bool isFinished)
		{
			IsCorrect = isCorrect;
			IsFinished = isFinished;
		}
	}
}