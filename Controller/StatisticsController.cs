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
        public Database Database = new();

        public StatisticsController()
        {

        }

        public List<string> GetLetterStatisticsFromDatabase()
        {
            return Database.GetStatisticsDB(0, LoginController.UserId.ToString());
        }
        public List<string> GetWordStatisticsFromDatabase()
        {
            return Database.GetStatisticsDB(1, LoginController.UserId.ToString());
        }
        public List<string> GetStoryStatisticsFromDatabase()
        {
            return Database.GetStatisticsDB(2, LoginController.UserId.ToString());
        }


    }
}
