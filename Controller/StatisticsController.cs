﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Controller
{
	public class StatisticsController
	{
        public int NumberOfMistakes { get; set; }
        public int NumberCorrect{ get; set; }
        private int _numberOfCorrectPerSecond;
        public DateTime CurrentTime { get; set; }
        public int TimeLeft { get; set; }
        private int _maxTimePerKey = 5;
        private System.Timers.Timer _timer = new System.Timers.Timer(1000);
        public event EventHandler<LiveStatisticsEventArgs> LiveStatisticsEvent;
        public Dictionary<int, int> CharactersPerSecond { get; set; }

        public StatisticsController()
		{
			CurrentTime = new DateTime();
			NumberOfMistakes = 0;
			NumberCorrect = 0;
            _numberOfCorrectPerSecond = 0;
            _timer.Elapsed += OnTimedEvent;
            CharactersPerSecond = new Dictionary<int, int>();
            }

        public void StartTimer()
        {
			if (!_timer.Enabled)
			{
                _timer.Start();
            }
        }

		public void ResetTimeLeft()
		{
            TimeLeft = _maxTimePerKey;
        }

        public string GetStatistics()
		{
			double PercentGood;
			if (NumberCorrect == 0)
			{
				PercentGood = 0;
			}
			else if (NumberOfMistakes == 0)
			{
				PercentGood = 100;
			}
			else
			{
				PercentGood = ((double)NumberCorrect / ((double)NumberCorrect + (double)NumberOfMistakes)) * 100;
				PercentGood = Math.Round(PercentGood, 1);
			}
			return $"{NumberOfMistakes} fout \r\n {PercentGood}% goed \r\n {CurrentTime.ToString("mm:ss")}";
		}

        private void UpdateCharactersPerSecond()
        {
            
            
            if (!CharactersPerSecond.ContainsKey(CurrentTime.Second + (CurrentTime.Minute*60) + (CurrentTime.Hour*3600)))
            {
                CharactersPerSecond.Add((CurrentTime.Second + (CurrentTime.Minute * 60) + (CurrentTime.Hour * 3600)), NumberCorrect - _numberOfCorrectPerSecond);
            }
           
            _numberOfCorrectPerSecond = NumberCorrect;
        }


        

        private void OnTimedEvent(object sender, EventArgs e)
        {
            if (TimeLeft == 0)
            {
                NumberOfMistakes++;
                TimeLeft = _maxTimePerKey;
            }
            LiveStatisticsEvent?.Invoke(this, new LiveStatisticsEventArgs());            
            TimeLeft--;
            UpdateCharactersPerSecond();
            CurrentTime = CurrentTime.AddSeconds(1);
        }
    }

    //EVENT FOR LIVE STATISCTICS UPDATE
    public class LiveStatisticsEventArgs : EventArgs
    {

        public LiveStatisticsEventArgs()
        {
        }
    }
}