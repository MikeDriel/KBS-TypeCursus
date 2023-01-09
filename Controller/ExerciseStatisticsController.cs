using Model;
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
    public bool IsRunning { get; set; }
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

    private void UpdateCharactersPerSecond()
    {
        int Key = CurrentTime.Second + CurrentTime.Minute * 60 + CurrentTime.Hour * 3600;
        if (!CharactersPerSecond.ContainsKey(Key))
        {
            CharactersPerSecond.Add(Key, NumberCorrect - _numberOfCorrectLastSecond);
        }

        _numberOfCorrectLastSecond = NumberCorrect;
    }

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
        if (_maxTime < _longestTimePerChar)
        {
            TimeLeft = _maxTime;
        }
        _timeUp = false;
        LiveStatisticsEvent?.Invoke(this, new LiveStatisticsEventArgs(false));
    }

    public int _InitializeScore()
    {
        // Calculation = ((Correct answers - Incorrect answers) / total time ) * difficulty 
        // difficulty is a placeholder as it's not in this current branch
        double score;
        if (CurrentTime.Second != 0)
        {
            score = (NumberCorrect - NumberOfMistakes) / (CurrentTime.Second + (double)CurrentTime.Minute * 60 + (double)CurrentTime.Hour * 3600) * 4;
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
            _database.UpdatePupilStatistics(UserId, ExerciseController.s_Choice, NumberCorrect, NumberOfMistakes, keyPerSec, _InitializeScore());
        }
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