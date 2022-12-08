using Model;

namespace Controller;

public class ExerciseController
{
    public Database Database = new();

    public Random Random = new(); //random number generator
    private int _choice; //user's choice

    public ExerciseController(int choice)
    {
        _choice = choice;
        CharacterList = new List<char>();
        //CharacterQueue = new Queue<char>();
        TypedCharsList = new List<char>();
        CorrectCharsList = new List<char>();

        //Coordinates
        Coordinates =
            new
                Dictionary<char, int[]>() //Makes dictionary with every coordinate for the canvas to display the rectangle
                {
                    { 'a', new[] { 73, 85 } },
                    { 'b', new[] { 257, 127 } },
                    { 'c', new[] { 175, 127 } },
                    { 'd', new[] { 155, 85 } },
                    { 'e', new[] { 143, 43 } },
                    { 'f', new[] { 196, 85 } },
                    { 'g', new[] { 237, 84 } },
                    { 'h', new[] { 278, 85 } },
                    { 'i', new[] { 347, 43 } },
                    { 'j', new[] { 319, 85 } },
                    { 'k', new[] { 360, 85 } },
                    { 'l', new[] { 401, 85 } },
                    { 'm', new[] { 339, 127 } },
                    { 'n', new[] { 298, 127 } },
                    { 'o', new[] { 388, 43 } },
                    { 'p', new[] { 429, 43 } },
                    { 'q', new[] { 62, 43 } },
                    { 'r', new[] { 184, 43 } },
                    { 's', new[] { 114, 85 } },
                    { 't', new[] { 224, 43 } },
                    { 'u', new[] { 306, 43 } },
                    { 'v', new[] { 216, 127 } },
                    { 'w', new[] { 102, 43 } },
                    { 'x', new[] { 134, 127 } },
                    { 'y', new[] { 265, 43 } },
                    { 'z', new[] { 93, 127 } },
                    { ' ', new[] { 123, 169 } }
                };

        if (choice == 0) // LetterExercise
            GenerateLetterData();

        if (choice == 1) // WordExercise
            GenerateWordData();

        if (choice == 2) // StoryExercise 
        {
            GenerateStoryData();
        }
    }

    public List<char> CharacterList { get; set; } //list which holds all the letters of the alphabet
    public List<char> CorrectCharsList { get; set; } //list which holds all the letters that the user has typed
    public List<char> TypedCharsList { get; set; } //list which holds all the letters that have been typed

    public Dictionary<char, int[]>
        Coordinates { get; set; } //dictionary which holds all the coordinates of the keyboard positions

    public char CurrentChar { get; set; } //the current letter that is being typed
    public char DequeuedChar { get; set; } //the current letter that is being typed
    public int Progress { get; set; }

    // events
    public event EventHandler<ExerciseEventArgs> ExerciseEvent;

    /// <summary>
    ///     Generates the alphabet data for the list. Also copies data to the queue for logic use.
    /// </summary>
    public void GenerateLetterData()
    {
        for (var i = 0; i < 35; i++) CharacterList.Add((char)Random.Next(97, 123));
    }

    public void GenerateWordData()
    {
        //get the words from the database and choose how many you want
        CharacterList = Database.GetWord(10);
    }

    public void GenerateStoryData()
    {
        string Story =
            "het leven is een tekening die je inkleurt in december komt sinterklaas met zwarte piet naar jouw schoorsteen toe";
        //get the words from the database and choose how many you want
        foreach (var character in Story)
        {
        CharacterList.Add(character);
        }
    }


    /// <summary>
    ///     Logic to check if letter is correct or incorrect.
    /// </summary>
    public void CheckIfLetterIsCorrect()
    {
        //checks if list isnt empty
        if (CharacterList.Count >= 1)
        {
            if (_choice == 2)
            {
                Progress++;
                DequeuedChar = CharacterList[0];
                CorrectCharsList.Add(DequeuedChar);
                CharacterList.RemoveAt(0);
                TypedCharsList.Add(CurrentChar);
            }
            //checks if the last keypress is equal to the first letter in the queue
            if (CharacterList[0] == CurrentChar)
            {
                if (_choice != 2)
                {
                    Progress++;
                    //if it is, remove the letter from the List
                    DequeuedChar = CharacterList[0];
                    TypedCharsList.Add(DequeuedChar);
                    CharacterList.RemoveAt(0);
                }

                if (CharacterList.Count == 0)
                    ExerciseEvent?.Invoke(this, new ExerciseEventArgs(true, true));
                else
                    ExerciseEvent?.Invoke(this, new ExerciseEventArgs(true, false));
            }
            else
            {
                ExerciseEvent?.Invoke(this, new ExerciseEventArgs(false, false));
            }
        }
    }
}

/// <summary>
///     Event for exercise
/// </summary>
public class ExerciseEventArgs : EventArgs
{
    public bool IsCorrect;
    public bool IsFinished;

    public ExerciseEventArgs(bool isCorrect, bool isFinished)
    {
        IsCorrect = isCorrect;
        IsFinished = isFinished;
    }
}