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
	public class WordExerciseController
	{
		// events
		public event EventHandler<ExerciseEventArgs> ExerciseEvent;
		
		//letter
		public char CurrentLetter { get; set; } //the current letter that is being typed
		public char DequeuedLetter { get; set; }

		//words
		public List<string> WordList { get; set; }
		public List<char> WordToChar { get; set; }

		public Dictionary<char, int[]> Coordinates { get; set; }
		public Random random = new Random();

		public WordExerciseController()
		{
				
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
		}

		public void GetWordsFromDB()
		{
			WordList.Add("banaan");
			WordList.Add("kaas");
			WordList.Add("aardappel");
			WordList.Add("pieter");

			//randomize wordlist
			WordList = WordList.OrderBy(x => random.Next()).ToList();
		}

		public void CheckIfWordIsCorrect()
		{
			if (WordList.Count >= 1)
			{
				if (WordToChar[0] == CurrentLetter)
				{
					WordToChar.RemoveAt(0);
					WordList.RemoveAt(0); //uhh

					if (WordList.Count == 0)
						ExerciseEvent?.Invoke(this, new ExerciseEventArgs(true, true));
					else
					{
						ExerciseEvent?.Invoke(this, new ExerciseEventArgs(true, false));
					}
				}
			}
			else ExerciseEvent?.Invoke(this, new ExerciseEventArgs(false, false));

		}
	}
}
