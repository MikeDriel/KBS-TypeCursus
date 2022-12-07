using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class StoryExerciseController
    {
        public int _typingIndex { get; set; } //the index of the current letter that is being typed 
        public ExerciseController exerciseController;
        public string Story = "Het leven is een tekening die je inkleurt. Op 5 december komt Sinterklaas met zwarte Piet naar jouw schoorsteen toe.";
        public List<char> _charListBackCorrect;
        public List<char> _charListBack;
        public List<char> _charListFront;

        public StoryExerciseController()
        {
            exerciseController = new ExerciseController(2);
            _charListBackCorrect = new List<char>();
            _charListBack = new List<char>();
            _charListFront = new List<char>();
        }

        public void SetCharListBack()
        {
            foreach (char character in Story)
            {
                _charListBackCorrect.Add(character);
            }
            _charListBack = _charListBackCorrect;
        }

        public void CheckIfLetterIsCorrectStory()
        {
            {//checks if list isnt empty
                if (_charListBackCorrect.Count >= 1)
                {
                    //checks if the last keypress is equal to the first letter in the queue
                    if (_charListBackCorrect[0] == exerciseController.CurrentChar)
                    {

                        //if it is, remove the letter from the queue
                        exerciseController.CharacterList.RemoveAt(0);

                        DequeuedChar = CharacterQueue.Dequeue();
                        TypedChars.Add(DequeuedChar);

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
        }

        
    }
}
