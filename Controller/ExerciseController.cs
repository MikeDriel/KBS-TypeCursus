using Model;

namespace Controller;

public class ExerciseController
{
    public Database Database = new();

    public Random Random = new(); //random number generator

    
    public ExerciseController(int choice, Difficulty difficulty)
    {
        Choice = choice;
        CharacterList = new List<char>();
        TypedCharsList = new List<char>();
        CorrectCharsList = new List<char>();

        //Coordinates
        //Makes dictionary with every coordinate for the canvas to display the rectangle
        Coordinates = new Dictionary<char, int[]>
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
            { '1', new[] { 41, 1 } },
            { '2', new[] { 82, 1 } },
            { '3', new[] { 123, 1 } },
            { '4', new[] { 164, 1 } },
            { '5', new[] { 205, 1 } },
            { '6', new[] { 245, 1 } },
            { '7', new[] { 286, 1 } },
            { '8', new[] { 327, 1 } },
            { '9', new[] { 368, 1 } },
            { '0', new[] { 409, 1 } },
            { ' ', new[] { 123, 169 } }
        };
        
        // Generate the correct data for the selected exercise
        switch (choice)
        {
            case 0 :
                GenerateLetterData(Database.getNiveau(LoginController.GetUserId(), TypeExercise.Letter));
                break;
            case 1:
                GenerateWordData(Database.getNiveau(LoginController.GetUserId(), TypeExercise.Word),20);
                break;
            case 2:
                GenerateStoryData();
                break;
        }
    }

    public int Choice { get; } //user's choice

    public List<char> CharacterList { get; set; } //list which holds all the letters of the alphabet
    public List<char> CorrectCharsList { get; set; } //list which holds all the letters that the user has typed
    public List<char> TypedCharsList { get; set; } //list which holds all the letters that have been typed

    public Dictionary<char, int[]> Coordinates { get; set; } //dictionary which holds all the coordinates of the keyboard positions

    public char CurrentChar { get; set; } //the current letter that is being typed
    public char DequeuedChar { get; set; } //the current letter that is being typed
    public int Progress { get; set; }

    // events
    public event EventHandler<ExerciseEventArgs> ExerciseEvent;

    /// <summary>
    ///     Generates the alphabet data for the list.
    /// </summary>
    public void GenerateLetterData(Difficulty difficulty)
    {
        List<char> letters = new List<char>();

        foreach (var charWithPoints in Database.AlphabetWithPoints)
        {
            if (charWithPoints.Value <= (int)difficulty)
            {
                letters.Add(charWithPoints.Key);
            }
        }
        for (var i = 0; i < 35; i++)
        {
            CharacterList.Add(letters[Random.Next(0, letters.Count)]);
        }
    }

    public void GenerateWordData(Difficulty difficulty, int amount)
    {
        //get the words from the database and choose how many you want
        CharacterList = Database.GetWord(difficulty, amount);
    }

    public void GenerateStoryData()
    {
        string StoryString = Database.GetStory();
        foreach (char character in StoryString)
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
            if (Choice == 2)
            {
                Progress++;
                DequeuedChar = CharacterList[0];
                CorrectCharsList.Add(DequeuedChar);
                CharacterList.RemoveAt(0);
                TypedCharsList.Add(CurrentChar);
            }


            //checks if the last keypress is equal to the first letter in the queue
            if ((DequeuedChar == CurrentChar && Choice == 2) || (Choice != 2 && CharacterList[0] == CurrentChar))
            {
                if (Choice != 2)
                {
                    Progress++;
                    //if it is, remove the letter from the List
                    DequeuedChar = CharacterList[0];
                    TypedCharsList.Add(DequeuedChar);
                    CharacterList.RemoveAt(0);
                }

                if (CharacterList.Count == 0)
                {
                    ExerciseEvent?.Invoke(this, new ExerciseEventArgs(true, true));
                }
                else
                {
                    ExerciseEvent?.Invoke(this, new ExerciseEventArgs(true, false));
                }
            }
            else
            {
                ExerciseEvent?.Invoke(this, new ExerciseEventArgs(false, false));
            }
        }
    }

    public void OnBack()
    {
        if (TypedCharsList.Count > 0)
        {
            Progress--;
            CharacterList.Insert(0, DequeuedChar);
            TypedCharsList.RemoveAt(TypedCharsList.Count - 1);
            CorrectCharsList.RemoveAt(CorrectCharsList.Count - 1);
            if (CorrectCharsList.Count > 0)
            {
                DequeuedChar = CorrectCharsList[CorrectCharsList.Count - 1];
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

    public ExerciseEventArgs(bool isCorrect,
        bool isFinished)
    {
        IsCorrect = isCorrect;
        IsFinished = isFinished;
    } 
}