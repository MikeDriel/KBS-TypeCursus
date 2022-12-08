// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
//
// namespace Controller
// {
//     public class StoryExerciseController : ExerciseController
//     {
//         public event EventHandler<ExerciseEventArgs> ExerciseEvent;
//         public string Story = "het leven is een tekening die je inkleurt. Op 5 december komt Sinterklaas met zwarte Piet naar jouw schoorsteen toe.";
//         public List<char> CharListToType;
//
//
//         public StoryExerciseController() : base(2)
//         {
//         }
//
//         public void SetCharListBack()
//         {
//             foreach (char character in Story)
//             {
//             }
//             //_charListTyped = _charListCorrect;
//         }
//
//         public void CheckIfLetterIsCorrectStory()
//         {
//             // //checks if list isnt empty
//             //
//             // if (CurrentChar == CharListCorrect[TypingIndex])
//             // {
//             //     CharListTyped.Add(CurrentChar);
//             //    
//             //     if (TypingIndex == CharListCorrect.Count)
//             //     {
//             //         ExerciseEvent?.Invoke(this, new ExerciseEventArgs(true, true));
//             //         
//             //     }
//             //     else
//             //     {
//             //         ExerciseEvent?.Invoke(this, new ExerciseEventArgs(true, false));
//             //         TypingIndex++;
//             //     }
//             // }
//             // else
//             // {
//             //     ExerciseEvent?.Invoke(this, new ExerciseEventArgs(false, false));
//             // }
//             //
//             
//             base.CheckIfLetterIsCorrect();
//             
//
//
//             //if (_charListCorrect.Count >= 1)
//             //{
//             //    //checks if the last keypress is equal to the first letter in the queue
//             //    if (_charListCorrect[_typingIndex] == _currentChar)
//             //    {
//             //        //if it is, add letter to _charListFront
//             //        //_charListFront.Add(_currentChar);
//             //        //flag for correct letter (color)
//
//
//             //        if (_charListFront.Count == _charListCorrect.Count)
//             //        {
//             //            ExerciseEvent?.Invoke(this, new ExerciseEventArgs(true, true));
//             //        }
//             //        else
//             //        {
//             //            ExerciseEvent?.Invoke(this, new ExerciseEventArgs(true, false));
//             //        }
//             //        _typingIndex++;
//             //    }
//             //    else
//             //    {
//             //        //_charListFront.Add(_currentChar);
//             //        //flag for wrong letter (color)
//             //        ExerciseEvent?.Invoke(this, new ExerciseEventArgs(false, false));
//             //        _typingIndex++;
//             //    }
//             //}
//
//         }
//
//
//
//     }
// }
