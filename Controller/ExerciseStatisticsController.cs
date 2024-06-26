﻿using Model;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Controller;

public class ExerciseStatisticsController
{
    private readonly Database _database;
    private readonly int _longestTimePerChar = 30;
    private readonly int _maxTime;
    private readonly Timer _timer;
    private char _currentKey;
    private bool _hasBeenWrong;

    private char? _lastKey;

    //int that contains the amount of correct typed characters before the current second (necessary to calculate the amount of characters typed in 1 certain second)
    private int _numberOfCorrectLastSecond;
    private bool _timeUp;

    /// <summary>
    /// Constructor for the ExerciseStatisticsController to initialize the class when needed
    /// </summary>
    /// <param name="maxTime"></param>
    public ExerciseStatisticsController(int maxTime)
    {
        _database = new Database();
        _maxTime = maxTime;
        TimeLeft = maxTime;
        CharactersPerSecond = new Dictionary<int, int>();
        CurrentTime = new DateTime();
        IsRunning = false;

        NumberOfMistakes = 0;
        NumberCorrect = 0;

        _timer = new Timer(180000);
        _timer.Elapsed += OnTimedEvent;
        _hasBeenWrong = false;
        _lastKey = null;
        _timeUp = false;
        _numberOfCorrectLastSecond = 0;
    }

    public DateTime CurrentTime { get; set; }

    //Dictionary which holds the amount of characters typed correct for every second passed
    public Dictionary<int, int> CharactersPerSecond { get; set; }

    public int NumberOfMistakes { get; private set; }

    public int NumberCorrect { get; private set; }

    // bool to know if the timer is running
    public bool IsRunning { get; set; }

    // the time the user has left for the exercise or the character
    public int TimeLeft { get; set; }
    public event EventHandler<LiveStatisticsEventArgs> LiveStatisticsEvent;

    public void StartTimer()
    {
        if (!_timer.Enabled)
        {
            _timer.Start();
            _timer.Interval = 1000;
            IsRunning = true;
        }
    }

    /// <summary>
    /// method to get the statistics in a string that is ready to be placed in the label
    /// </summary>
    public string GetStatistics()
    {
        double percentGood;
        if (NumberCorrect == 0)
        {
            percentGood = 0;
        }
        else if (NumberOfMistakes == 0)
        {
            percentGood = 100;
        }
        else
        {
            percentGood = NumberCorrect / (NumberCorrect + (double)NumberOfMistakes) * 100;
            percentGood = Math.Round(percentGood, 1);
        }

        return $" {NumberOfMistakes} fout \r\n {percentGood}% goed \r\n {CurrentTime.ToString("mm:ss")}";
    }

    /// <summary>
    /// Method to set the amount in the dictionary of typed chars per second
    /// </summary>
    private void UpdateCharactersPerSecond()
    {
        int Key = CurrentTime.Second + CurrentTime.Minute * 60 + CurrentTime.Hour * 3600;
        if (!CharactersPerSecond.ContainsKey(Key))
        {
            CharactersPerSecond.Add(Key, NumberCorrect - _numberOfCorrectLastSecond);
        }

        _numberOfCorrectLastSecond = NumberCorrect;
    }

    /// <summary>
    /// event for when the timer is done
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="elapsedEventArgs"></param>
    private void OnTimedEvent(object? sender, ElapsedEventArgs elapsedEventArgs)
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

        UpdateCharactersPerSecond();
        CurrentTime = CurrentTime.AddSeconds(1);
    }

    /// <summary>
    /// actions to do when the answer is wrong and it is because of a wrongly typed key
    /// </summary>
    /// <param name="currentKey"></param>
    public void WrongAnswer(char currentKey)
    {
        _lastKey = _currentKey;
        _currentKey = currentKey;
        if (_lastKey != _currentKey)
        {
            WrongAnswer();
        }
    }

    /// <summary>
    /// actions to do when the answer is wrong because of a time out
    /// </summary>
    public void WrongAnswer()
    {
        NumberOfMistakes++;
        LiveStatisticsEvent?.Invoke(this, new LiveStatisticsEventArgs(false));
    }
    
    
    public void RightAnswer()
    {
        NumberCorrect++;
        _hasBeenWrong = false;
        if (_maxTime < _longestTimePerChar)
        {
            TimeLeft = _maxTime;
        }

        _timeUp = false;
        LiveStatisticsEvent?.Invoke(this, new LiveStatisticsEventArgs(false));
    }

    /// <summary>
    /// Method to calculate the gained or lost score from the made exercise
    /// </summary>
    /// <returns></returns>
    public int InitializeScore()
    {
        // Calculation = ((Correct answers - Incorrect answers) / total time ) * difficulty 
        // difficulty is a placeholder as it's not in this current branch
        double score;
        if (CurrentTime.Second != 0)
        {
            score = (NumberCorrect - NumberOfMistakes) /
                (CurrentTime.Second + (double)CurrentTime.Minute * 60 + (double)CurrentTime.Hour * 3600) * 4;
        }
        else
        {
            score = (NumberCorrect - NumberOfMistakes) * 4;
        }

        Math.Round(score, 0);
        return (int)score;
    }


    //public void UpdatePupilStatistics(int pupilId, int type, int amountCorrect, int amountFalse, int keyPerSec, int score)
    public void SendStatisticInformationToDatabase(double keyPerSec)
    {
        if (LoginController.s_UserId != null)
        {
            int UserId = (int)LoginController.s_UserId;
            _database.UpdatePupilStatistics(UserId, ExerciseController.SChoice, NumberCorrect, NumberOfMistakes,
                keyPerSec, InitializeScore());
        }
    }
}

//EVENT FOR LIVE STATISTICS UPDATE
public class LiveStatisticsEventArgs : EventArgs
{
    public bool SetTextRed { get; set; }

    public LiveStatisticsEventArgs(bool setTextRed)
    {
        SetTextRed = setTextRed;
    }
}