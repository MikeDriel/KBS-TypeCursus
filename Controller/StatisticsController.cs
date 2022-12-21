using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
	public class StatisticsController
	{
		private Database _database;
		public List<string> LetterStatistics { get; private set; }
		public List<string> WordStatistics { get; private set; }
		public List<string> StoryStatistics { get; private set; }
		public List<string> TotalStatistics { get; private set; }
		public List<List<string>> LeaderBoardList { get; private set; }
		public List<string> UserIds { get; private set; }
		public string PupilName { get; private set; }
		public int ClassId { get; private set; }

		public StatisticsController()
		{
			_database = new();
			InitializeLetterStatistics();
			InitializeWordStatistics();
			InitializeStoryStatistics();
			InitializeTotalStatistics();
			InitializePupilName();
			InitializeLeaderboard();
		}

		/// <summary>
		/// Initializes the leaderboard.
		/// Gets the ClassID to determine which group of pupils to show.
		/// Gets all PupilID's from the database, needed to be able to show all pupils.
		/// Finally, generate the leaderboard.
		/// </summary>
		private void InitializeLeaderboard()
		{
			ClassId = _database.GetClassId(LoginController.UserId.ToString());
			UserIds = _database.GetClass(ClassId); //get the amount of pupils.
			LeaderBoardList = _database.GenerateLeaderboard(UserIds.Select(int.Parse).ToList(), ClassId);
		}
		private void InitializeLetterStatistics()
		{
			LetterStatistics = _database.GetStatisticsDB(0, LoginController.UserId.ToString());
		}
		private void InitializeWordStatistics()
		{
			WordStatistics = _database.GetStatisticsDB(1, LoginController.UserId.ToString());
		}
		private void InitializeStoryStatistics()
		{
			StoryStatistics = _database.GetStatisticsDB(2, LoginController.UserId.ToString());
		}
		private void InitializeTotalStatistics()
		{
			TotalStatistics = new List<string>();
			for (int i = 2; i < LetterStatistics.Count; i++)
			{
				double total = Convert.ToDouble(LetterStatistics[i]) + Convert.ToDouble(WordStatistics[i]) + Convert.ToDouble(StoryStatistics[i]);
				TotalStatistics.Add(total.ToString());
			}
		}
		private void InitializePupilName()
		{
			PupilName = _database.GetStatisticsNameDB(LoginController.UserId.ToString());
		}
	}
}
