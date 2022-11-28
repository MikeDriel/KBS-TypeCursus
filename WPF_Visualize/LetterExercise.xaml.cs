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
        public LetterExerciseDataContext dataContext = new LetterExerciseDataContext();

        //public Queue<Letter> LettersToType = new Queue<Letter>();
        private List<Letter> LettersToType = new List<Letter>();
        private string LettersToTypeString;
        private List<Letter> LettersDone = new List<Letter>();
        private string LettersDoneString;





        public LetterExercise()
        {
            //LetterExerciseDC.LettersToType.Enqueue(new Letter('w', System.Drawing.Color.Black));
            Letter letter1 = new Letter('f', System.Drawing.Color.Black);
            Letter letter2 = new Letter('u', System.Drawing.Color.Black);
            Letter letter3 = new Letter('c', System.Drawing.Color.Black);
            LettersToType.Add(letter1);
            LettersToType.Add(letter2);
            LettersToType.Add(letter3);
            //LettersToType.Add(letter1.Character.ToString());
            //LettersToType.Add(letter2.Character.ToString());

            //LettersToType.Enqueue(new Letter('j', System.Drawing.Color.Black));
            //LettersToType.Enqueue(new Letter('h', System.Drawing.Color.Black));
            InitializeComponent();
            ConvertListToStrings();
            ChangeTextOnScreen();
        }
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            //Do work
            Debug.WriteLine(e.Key);
            CheckIfCorrectLetterIsTyped(e.Key.ToString().ToLower()[0]);
            ChangeTextOnScreen();
        }

        private void ChangeTextOnScreen()
        {
            LetterTypedLabel.Content = LettersToType[0].Character.ToString();
            LettersTodoLabel.Content = LettersToTypeString;

            foreach (Letter letter in LettersDone)
            {
                
                LettersDoneLabel1.Content += letter.Character.ToString();
                LettersDoneLabel1.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
        }

        private void ConvertListToStrings()
        {
            LettersToTypeString = string.Join(" ", LettersToType.Select(x => x.Character.ToString()));
            LettersToTypeString = LettersToTypeString.Remove(0, 1);
            LettersDoneString = string.Join(" ", LettersDone.Select(x => x.Character.ToString()));
        }

        public void CheckIfCorrectLetterIsTyped(char LetterTyped)
        {
            if (LetterTyped == LettersToType[0].Character)
            {
                LettersDone.Add(LettersToType[0]);
                LettersToType.RemoveAt(0);
                if (LettersToType.Count == 0)
                {
                    LetterTypedLabel.Content = "You did it!";
                }
            }
            else
            {
                Debug.WriteLine("Wrong letter");
            }
            ConvertListToStrings();
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

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
