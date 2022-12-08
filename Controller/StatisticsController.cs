using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Controller
{
    public class StatisticsController
    {
        public int NumberOfMistakes { get; private set; }
        public int NumberCorrect { get; private set; }
        private bool _hasBeenWrong;
        public DateTime CurrentTime { get; set; }
        public int TimeLeft { get; set; }
        private int _maxTimePerKey = 5;
        private System.Timers.Timer _timer;
        public bool IsRunning { get; set; }
        public event EventHandler<LiveStatisticsEventArgs> LiveStatisticsEvent;
        private char? _lastKey;
        private char _currentKey;
        private bool _timeUp;
        public StatisticsController()
        {
            IsRunning = false;
            CurrentTime = new DateTime();
            NumberOfMistakes = 0;
            NumberCorrect = 0;
            _timer = new System.Timers.Timer(180000);
            _timer.Elapsed += OnTimedEvent;
            _hasBeenWrong = false;
            _lastKey = null;
            _timeUp = false;
        }

        public void StartTimer()
        {
            if (!_timer.Enabled)
            {
                _timer.Start();
                _timer.Interval = 1000;
                IsRunning = true;
            }
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

            return $" {NumberOfMistakes} fout \r\n {PercentGood}% goed \r\n {CurrentTime.ToString("mm:ss")}";
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            if (!_timeUp)
            {
                if (TimeLeft == 0)
                {
                    WrongAnswer();
                    _timeUp = true;
                    _hasBeenWrong = true;
                }
            }

            LiveStatisticsEvent?.Invoke(this, new LiveStatisticsEventArgs(_timeUp));
            if (!_hasBeenWrong)
            {
                TimeLeft--;
            }

            CurrentTime = CurrentTime.AddSeconds(1);
        }

        public void WrongAnswer(char currentKey)
        {
            _lastKey = _currentKey;
            _currentKey = currentKey;
            if (_lastKey != _currentKey)
            {
                WrongAnswer();
            }
        }

        public void WrongAnswer()
        {
            NumberOfMistakes++;
            LiveStatisticsEvent?.Invoke(this, new LiveStatisticsEventArgs(false));
        }

        public void RightAnswer()
        {
            NumberCorrect++;
            _hasBeenWrong = false;
            TimeLeft = _maxTimePerKey;
            _timeUp = false;
            LiveStatisticsEvent?.Invoke(this, new LiveStatisticsEventArgs(false));
        }
    }

    //EVENT FOR LIVE STATISTICS UPDATE
    public class LiveStatisticsEventArgs : EventArgs
    {
        public bool SetTextRed;

        public LiveStatisticsEventArgs(bool setTextRed)
        {
            SetTextRed = setTextRed;
        }
    }
}