﻿using Model;
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

        public List<string> GetStatisticsFromDatabase()
        {
            return Database.GetStatisticsDB();
        }


    }
}
