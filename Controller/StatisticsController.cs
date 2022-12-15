using Model;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public StatisticsController()
        {
            _database = new();
            InitializeLetterStatistics();
            InitializeWordStatistics();
            InitializeStoryStatistics();
            InitializeTotalStatistics();
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
            for (int i = 2; i < LetterStatistics.Count; i++)
            {
                double total = Convert.ToDouble(LetterStatistics[i]) + Convert.ToDouble(WordStatistics[i]) + Convert.ToDouble(StoryStatistics[i]);
                TotalStatistics[i - 2] = total.ToString();
            }
        }



    }
}
