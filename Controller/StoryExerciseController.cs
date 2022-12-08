using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class StoryExerciseController
    {
        public event EventHandler<ExerciseEventArgs> ExerciseEvent;
        public int _typingIndex { get; set; } //the index of the current letter that is being typed 
        public ExerciseController exerciseController;
        public string Story = "het leven is een tekening die je inkleurt. Op 5 december komt Sinterklaas met zwarte Piet naar jouw schoorsteen toe.";
        public List<char> _charListBackCorrect;
        public List<char> _charListBack;
        public List<char> _charListFront;
        public char _currentChar;
        

        public StoryExerciseController()
        {
            exerciseController = new ExerciseController(2);
            _charListBackCorrect = new List<char>();
            _charListBack = new List<char>();
            _charListFront = new List<char>();
            _currentChar = exerciseController.CurrentChar;
            
            
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

                    //checks if the last keypress is equal to the first letter in the queue
                    if (_charListBackCorrect[_typingIndex] == _currentChar)
                    {
                        //if it is, add letter to _charListFront
                        _charListFront.Add(_currentChar);
                        //flag for correct letter (color)


                        if (_charListFront.Count == _charListBackCorrect.Count)
                        {
                            ExerciseEvent?.Invoke(this, new ExerciseEventArgs(true, true));
                        }
                        else
                        {
                            ExerciseEvent?.Invoke(this, new ExerciseEventArgs(true, false));
                        }
                        _typingIndex++;
                    }
                    else
                    {
                        _charListFront.Add(_currentChar);
                        //flag for wrong letter (color)
                        ExerciseEvent?.Invoke(this, new ExerciseEventArgs(false, false));
                        _typingIndex++;
                    }
                
            }
        }



    }
}
