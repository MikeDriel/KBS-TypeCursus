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
        public int TypingIndex { get; set; } //the index of the current letter that is being typed 
        public ExerciseController exerciseController;
        public string Story = "het leven is een tekening die je inkleurt. Op 5 december komt Sinterklaas met zwarte Piet naar jouw schoorsteen toe.";
        public List<char> _charListCorrect;
        public List<char> _charListTyped;
        public char _currentChar;


        public StoryExerciseController()
        {
            exerciseController = new ExerciseController(2);
            _charListCorrect = new List<char>();
            _charListTyped = new List<char>();
            _currentChar = exerciseController.CurrentChar;
            TypingIndex = 0;
        }

        public void SetCharListBack()
        {
            foreach (char character in Story)
            {
                _charListCorrect.Add(character);
            }
            //_charListTyped = _charListCorrect;
        }

        public void CheckIfLetterIsCorrectStory()
        {
            //checks if list isnt empty

            if (_currentChar == _charListCorrect[TypingIndex])
            {
                _charListTyped.Add(_currentChar);
               
                if (TypingIndex == _charListCorrect.Count)
                {
                    ExerciseEvent?.Invoke(this, new ExerciseEventArgs(true, true));
                    
                }
                else
                {
                    ExerciseEvent?.Invoke(this, new ExerciseEventArgs(true, false));
                    TypingIndex++;
                }
            }
            else
            {
                ExerciseEvent?.Invoke(this, new ExerciseEventArgs(false, false));
            }
           


            //if (_charListCorrect.Count >= 1)
            //{
            //    //checks if the last keypress is equal to the first letter in the queue
            //    if (_charListCorrect[_typingIndex] == _currentChar)
            //    {
            //        //if it is, add letter to _charListFront
            //        //_charListFront.Add(_currentChar);
            //        //flag for correct letter (color)


            //        if (_charListFront.Count == _charListCorrect.Count)
            //        {
            //            ExerciseEvent?.Invoke(this, new ExerciseEventArgs(true, true));
            //        }
            //        else
            //        {
            //            ExerciseEvent?.Invoke(this, new ExerciseEventArgs(true, false));
            //        }
            //        _typingIndex++;
            //    }
            //    else
            //    {
            //        //_charListFront.Add(_currentChar);
            //        //flag for wrong letter (color)
            //        ExerciseEvent?.Invoke(this, new ExerciseEventArgs(false, false));
            //        _typingIndex++;
            //    }
            //}

        }



    }
}
