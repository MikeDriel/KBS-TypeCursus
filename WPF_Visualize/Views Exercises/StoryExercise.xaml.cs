using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Controller;
using WPF_Visualize.ViewLogic;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace WPF_Visualize
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    /// 

    public partial class StoryExercise : UserControl
    {
        Controller.ExerciseController _controller;
        Controller.StoryExerciseController _storyController;
        Controller.StatisticsController _statisticsController = new();

        Rectangle _rectangleLetterTyped = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle
        Rectangle _rectangleLetterToType = new Rectangle { Width = 33, Height = 33, Fill = Brushes.Gray, Opacity = 0.75 }; //Makes rectangle










        public StringBuilder _sb = new StringBuilder();
        public StoryExercise()
        {
            InitializeComponent();
            _storyController = new();
            //subscribe events
            _storyController.ExerciseEvent += ExerciseEvent;
            _statisticsController.LiveStatisticsEvent += SetLiveStatistics;

            _storyController.SetCharListBack();
            ChangeTextOnScreen();

            //rectangles for the letters
            KeyboardCanvas.Children.Add(_rectangleLetterToType); //adds rectangle on screen
            KeyboardCanvas.Children.Add(_rectangleLetterTyped);
            _rectangleLetterTyped.Visibility = Visibility.Hidden;

            //Add the text to the textbox
            RichTextBoxStory.Document.Blocks.Clear();
            RichTextBoxStory.Document.Blocks.Add(new Paragraph(new Run(_storyController.Story)));
        }

        //Connects events to the button 
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;
            window.KeyUp += HandleKeyUp;
        }

        //Handles the keypresses from the userinput
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {

            //if (e.Key.ToString().Equals("Space"))
            //{
            //    _storyController._currentChar = ' ';
            //}
            //else 
            //{
            //    if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            //    {
            //        if(e.Key.ToString().Length == 1)
            //        {
            //            _storyController._currentChar = e.Key.ToString().ToUpper()[0];
            //        }

            //    }
            //    else
            //    {
            //        if (e.Key.ToString().Length == 1)
            //        {
            //            _storyController._currentChar = e.Key.ToString().ToLower()[0];
            //        } 
            //    }

            //}

            _storyController._currentChar = e.Key.ToString().ToLower()[0];
            _storyController.CheckIfLetterIsCorrectStory();
            _statisticsController.ResetTimeLeft();
            _statisticsController.StartTimer();
        }


        private void HandleKeyUp(object sender, KeyEventArgs e)
        {


        }


        //updates values on view
        private void ChangeTextOnScreen()
        {
            //RichTextBoxStory.SelectAll();
            //RichTextBoxStory.Selection.Text = "";
            //foreach (var item in _storyController._charListTyped)
            //{
            //    RichTextBoxStory.AppendText(item.ToString());
            //}

            ////StoryTextBoxFront.SelectAll();
            ////StoryTextBoxFront.Selection.Text = "";
            ////foreach (var item in _storyController._charListFront)
            ////{
            ////    StoryTextBoxFront.AppendText(item.ToString());
            ////}

            ////RichTextBoxStory.AppendText(_charListBack.ToString());
            ////StoryTextBoxFront.AppendText(_charListFront.ToString());
            //SetLiveStatistics(this, null);
        }

        //updates statistics on view
        private void SetLiveStatistics(object sender, LiveStatisticsEventArgs e)
        {
            //this.Dispatcher?.Invoke(() =>
            //{
            //    string statistics = _statisticsController.GetStatistics();
            //    LiveStatisticsScreen.Content = statistics;
            //    TimeLeftLabel.Content = _statisticsController.TimeLeft;
            //});
        }

        //The back button top left
        private void OnBack(object sender, RoutedEventArgs e)
        {
            Cleanup();
            UserControlController.MainWindowChange(this, new ExerciseSelect());
        }

        //cleanup to prevent bugs
        private void Cleanup()
        {
            var window = Window.GetWindow(this);
            window.KeyDown -= HandleKeyPress;
            _storyController._currentChar = '.';
        }

        //Methods for events to fire
        private void MistakeMade()
        {
            //if the letter is wrong, add a mistake and update the screen
            _statisticsController.NumberOfMistakes++;
            ColorTheKs(false);

        }

        private void ExerciseFinished()
        {
            //adds a empty space if the list is empty

            //show a message box
            MessageBox.Show("You have finished the exercise!");
            UserControlController.MainWindowChange(this, new ResultatenOefening());
        }

        private void CorrectAnswer()
        {
            _statisticsController.NumberCorrect++;
            ColorTheKs(true);
        }

        //private void ColorTheKs(bool correct)
        //{
        //    int index = _storyController.TypingIndex;
        //    indexLabel.Content = index;
        //    //// Get the starting position of the entire text
        //    //TextPointer startPosition = RichTextBoxStory.Document.ContentStart;

        //    //// Get the position at a specific offset (character position)
        //    //TextPointer position = startPosition.GetPositionAtOffset(index);

        //    //// Get the ending position by moving forward 10 characters
        //    //TextPointer endPosition = position.GetPositionAtOffset(index);

        //    //// Create a new TextRange object that represents the text
        //    //TextRange selectedText = new TextRange(position, endPosition);

        //    //// Re-select the text
        //    //RichTextBoxStory.Selection.Select(selectedText.Start, selectedText.End);



        //    TextPointer start = RichTextBoxStory.Document.ContentStart.GetPositionAtOffset(index);
        //    TextPointer end = start.GetPositionAtOffset(1);
        //    TextRange range = new TextRange(start, end);

        //    if (correct)
        //    {
        //        range.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
        //    }
        //    else
        //    {
        //        range.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
        //    }

        //}

        void ColorTheKs(bool correct)
        {
            // Set the color of the character at the specified index
            int index = _storyController.TypingIndex;
            TextPointer start = RichTextBoxStory.Document.ContentStart.GetPositionAtOffset(index);
            TextPointer end = start.GetPositionAtOffset(1);
            TextRange range = new TextRange(start, end);

            // Set the color to red or green depending on the value of the 'correct' parameter
            if (correct)
            {
                range.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
            }
            else
            {
                range.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            }
        }

        //EVENTS
        private void ExerciseEvent(object sender, ExerciseEventArgs e)
        {
            if (e.IsCorrect)
            {
                CorrectAnswer();
            }
            else
            {
                MistakeMade();
            }
            if (e.IsFinished)
            {
                ExerciseFinished();
            }

            ChangeTextOnScreen();
        }




    }
}
