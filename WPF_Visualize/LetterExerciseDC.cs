using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace WPF_Visualize
{
    public class LetterExerciseDC
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        public Queue<Letter> LettersToType = new Queue<Letter>();
        public Queue<Letter> LettersTyped = new Queue<Letter>();
        


        public LetterExerciseDC()
        {

        }

        //eventhandler keypressed
        //queue letterstyped
        //dequeue lettertotype


    }
}
