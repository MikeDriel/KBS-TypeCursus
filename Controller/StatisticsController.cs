using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
	public class StatisticsController
	{
		private int _numberOfMistakes;
		private int _numberCorrect;
		private DateTime _currentTime;

		public StatisticsController()
		{
			_currentTime = new DateTime();
			_numberOfMistakes = 0;
			_numberCorrect = 0;
		}

		public void AddMistake()
		{
			_numberOfMistakes++;
		}

		public void AddCorrect()
		{
			_numberCorrect++;
		}

		public string GetStatistics()
		{
			double PercentGood;
			if (_numberCorrect == 0)
			{
				PercentGood = 0;
			}
			else if (_numberOfMistakes == 0)
			{
				PercentGood = 100;
			}
			else
			{
				PercentGood = ((double)_numberCorrect / ((double)_numberCorrect + (double)_numberOfMistakes)) * 100;
				PercentGood = Math.Round(PercentGood, 1);
			}
			return $"{_numberOfMistakes} fout \r\n {PercentGood}% goed \r\n {_currentTime.ToString("mm:ss")}";
		}
	}
}