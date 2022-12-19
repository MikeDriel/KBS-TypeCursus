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
        public string PupilName { get; private set; }


        public StatisticsController()
        {
            _database = new();
            InitializeLetterStatistics();
            InitializeWordStatistics();
            InitializeStoryStatistics();
            InitializeTotalStatistics();
            InitializePupilName();
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
                double total;
                if (i != 5) //if the position in the array isnt the Key per second value (position 5)
                            //this has to be checked since KPS is calculated in a different way
                {
                    total = Convert.ToDouble(LetterStatistics[i]) + Convert.ToDouble(WordStatistics[i]) + Convert.ToDouble(StoryStatistics[i]);
                } else
                {
                    //equation to calculate average Keys per second 
                    total = ((Convert.ToDouble(LetterStatistics[i]) * Convert.ToDouble(LetterStatistics[4])) +
                        (Convert.ToDouble(WordStatistics[i]) * Convert.ToDouble(WordStatistics[4])) +
                        (Convert.ToDouble(StoryStatistics[i]) * Convert.ToDouble(StoryStatistics[4]))) /
                        (Convert.ToDouble(LetterStatistics[4]) + Convert.ToDouble(WordStatistics[4]) + Convert.ToDouble(StoryStatistics[4]));
                }

                
                total = Math.Round(total, 1);
                TotalStatistics.Add((total).ToString());
            }
        }
        private void InitializePupilName()
        {
            PupilName = _database.GetStatisticsNameDB(LoginController.UserId.ToString());
        }


    }
}
