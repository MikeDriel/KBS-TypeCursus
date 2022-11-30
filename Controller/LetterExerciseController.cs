using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
	public class LetterExerciseController
	{
		public List<char> AlphabetList { get; set; } //list which holds all the letters of the alphabet
		public Queue<char> AlphabetQueue { get; set; } //queue which holds all the letters of the alphabet

        public Dictionary<char, int[]> Coordinates { get; set; }

        public Random random = new Random();

		public LetterExerciseController()
		{
			AlphabetList = new List<char>();
			AlphabetQueue = new Queue<char>();
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

		/// <summary>
		/// Generates the alphabet data for the list. Also copies data to the queue for logic use
		/// </summary>
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

		/// <summary>
		/// Randomizes the letters in the alphabet.
		/// </summary>
		public void RandomizeAlphabet()
		{
			AlphabetList = AlphabetList.OrderBy(x => random.Next()).ToList();
		}
	}
}