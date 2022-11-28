using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
	public class LetterExersiseController
	{
		public List<char> AlphabetList { get; set; } //list which holds all the letters of the alphabet
		public Queue<char> AlphabetQueue { get; set; } //queue which holds all the letters of the alphabet

		public Random random = new Random();

		public LetterExersiseController()
		{
			AlphabetList = new List<char>();
			AlphabetQueue = new Queue<char>();

			
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
	}
}