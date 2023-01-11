using System.Diagnostics;
using Controller;
using Model;
namespace TestProject;

[TestFixture]
public class ExerciseControllerTests
{
    private ExerciseController controller;

    [TestCase(10)]
    [TestCase(100)]
    [TestCase(200)]
    [TestCase(500)]
    [TestCase(2000)]
    // check if the generated letters are all correct and of the same level as we requested
    public void LetterNiveauCorrect(int count)
    {
        foreach (Difficulty level in Enum.GetValues(typeof(Difficulty)))
        {
            LoginController.s_UserId = 1;
            Database database = new Database();
            controller = new ExerciseController(TypeExercise.Letter, level);
            controller.GenerateLetterData(level, count);
            foreach (char letter in controller.CharacterList)
            {
                if (!(database.AlphabetWithPoints[letter] <= (int)level))
                {
                    Assert.Fail();
                }
            }
        }
    }

    [TestCase(10)]
    [TestCase(100)]
    [TestCase(200)]
    [TestCase(500)]
    [TestCase(2000)]
    // check if the generated words are all correct and of the same level as we requested
    public void WordNiveauCorrect(int count)
    {
        foreach (Difficulty level in Enum.GetValues(typeof(Difficulty)))
        {
            LoginController.s_UserId = 1;
            Database database = new Database();
            controller = new ExerciseController(TypeExercise.Word, level);
            controller.GenerateWordData(level, count);
            List<string> words = new List<string>();
            string wordToAdd = "";
            // get all the words from the characterlist
            foreach (char letter in controller.CharacterList)
            {
                if (letter == ' ')
                {
                    words.Add(wordToAdd);
                    wordToAdd = "";
                }
                else
                {
                    wordToAdd += letter;
                }
            }
            foreach (string word in words)
            {
                if (!(database.GetWordDifficulty(word) <= level))
                {
                    Assert.Fail();
                }
            }
        }
    }

    [Test]
    public void OnBackTest()
    {
        controller = new ExerciseController(TypeExercise.Story);
        controller.CharacterList = new List<char>();
        controller.DequeuedChar = 'a';
        string charlist = "Hij zat op de b";
        string correctCharList = "ank";
        string typedCharList = "lnk";
        foreach (char letter in correctCharList)
        {
            controller.CorrectCharsList.Add(letter);
        }
        foreach (char letter in typedCharList)
        {
            controller.TypedCharsList.Add(letter);
        }
        foreach (char letter in charlist)
        {
            controller.CharacterList.Add(letter);
        }
        controller.OnBack();
        if (controller.DequeuedChar == 'n' && controller.CharacterList[0] == 'a')
        {
            Assert.Pass();
        }
        else
        {
            Assert.Fail();
        }
    }
}