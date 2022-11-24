using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Model;

namespace WPF_Visualize
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class LetterExercise : UserControl
    {
        public LetterExerciseDC dataContext = new LetterExerciseDC();

        public Queue<Letter> LettersToType = new Queue<Letter>();
        public Queue<Letter> LettersTyped = new Queue<Letter>();






        public LetterExercise()
        {
            //LetterExerciseDC.LettersToType.Enqueue(new Letter('w', System.Drawing.Color.Black));
            LettersToType.Enqueue(new Letter('f', System.Drawing.Color.Black));
            LettersToType.Enqueue(new Letter('j', System.Drawing.Color.Black));
            LettersToType.Enqueue(new Letter('h', System.Drawing.Color.Black));
            InitializeComponent();
            Keyboard.Focus(UserControl);
        }

        private void Textbox_textchanged(object sender, TextChangedEventArgs e)
        {
            Keyboard.Focus(textbox);
            currentlabel.Content += textbox.Text;
        }

        /*
        private void Keydown(object sender, KeyEventArgs e)
        {
            Keyboard.Focus(UserControl);
            currentlabel.Content = e.Key.ToString();

            Console.WriteLine(e.Key);
            Trace.WriteLine(e.Key);
        }
        */

    }
}
