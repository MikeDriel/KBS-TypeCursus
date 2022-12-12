using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPF_Visualize.ViewLogic;

namespace WPF_Visualize;

/// <summary>
///     Interaction logic for ResultatenOefening.xaml
/// </summary>
public partial class ResultatenOefening : UserControl
{
    public ResultatenOefening()
    {
        InitializeComponent();
        _InitializeLabels();
    }


    //sets all labels
    private void _InitializeLabels()
    {
        //calculate the amount of characters typed per second
        // var wps = Exercise.StatisticsController.NumberCorrect /
        //           (double)Exercise.StatisticsController.CurrentTime.Second;
        // wps = Math.Round(wps, 1);
        var wps = Exercise.StatisticsController.CharactersPerSecond.Values.Average();
        wps = Math.Round(wps, 1);
        //calculate the percentage of correct typed characters
        var correctPercentage = Exercise.StatisticsController.NumberCorrect /
                                (Exercise.StatisticsController.NumberCorrect +
                                 (double)Exercise.StatisticsController.NumberOfMistakes) *
                                100;
        correctPercentage = Math.Round(correctPercentage, 1);


        //sets the labels
        Totaltime.Content = Exercise.StatisticsController.CurrentTime.ToString("mm:ss");
        MistakeCount.Content = Exercise.StatisticsController.NumberOfMistakes;
        WPS.Content = wps;
        CorrectCount.Content = Exercise.StatisticsController.NumberCorrect;
        CorrectPercentage.Content = $"{correctPercentage}%";
        _InitializeFeedback(correctPercentage);
    }

    //logic for feedback label
    private void _InitializeFeedback(double percentage)
    {
        if (percentage >= 80)
        {
            Feedback.Content = "Heel goed gedaan!";
        }
        else if (percentage >= 55)
        {
            Feedback.Content = "Goed gedaan, maar het kan beter!";
        }
        else if (percentage >= 40)
        {
            Feedback.Content = "Helaas, je hebt nog geen voldoende. Maar je komt dichtbij";
        }
        else
        {
            Feedback.Content = "Helaas, je hebt nog geen voldoende";
        }
    }

    private void OnBack(object sender, RoutedEventArgs e)
    {
        //_cleanup();
        UserControlController.MainWindowChange(this, new ExerciseSelect());
    }
}