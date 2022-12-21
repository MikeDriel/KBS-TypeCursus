﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Controller;
using WPF_Visualize.ViewLogic;
using static System.Formats.Asn1.AsnWriter;

namespace WPF_Visualize.Views_Statistics;

/// <summary>
///     Interaction logic for ResultatenOefening.xaml
/// </summary>
public partial class ResultsExercise : UserControl
{
    public static ExerciseStatisticsController s_ExerciseStatisticsController;
    public ResultsExercise(ExerciseStatisticsController statisticsController)
    {
        s_ExerciseStatisticsController = statisticsController;
        InitializeComponent();
        _InitializeLabels();
        s_ExerciseStatisticsController.SendStatisticInformationToDatabase(Math.Round(s_ExerciseStatisticsController.CharactersPerSecond.Values.Average(), 1));
        
    }

    //sets all labels
    private void _InitializeLabels()
	{
        var wps = s_ExerciseStatisticsController.CharactersPerSecond.Values.Average();
		wps = Math.Round(wps, 1);
		//calculate the percentage of correct typed characters
		var correctPercentage = s_ExerciseStatisticsController.NumberCorrect /
                                (s_ExerciseStatisticsController.NumberCorrect +
                                 (double)s_ExerciseStatisticsController.NumberOfMistakes) *
                                100;
		correctPercentage = Math.Round(correctPercentage, 1);


        //sets the labels
        Score.Content = "Score: " + s_ExerciseStatisticsController._InitializeScore();
        Totaltime.Content = "Totale tijd: " + s_ExerciseStatisticsController.CurrentTime.ToString("mm:ss") ;
        MistakeCount.Content = "Aantal fouten: " + s_ExerciseStatisticsController.NumberOfMistakes;
        WPS.Content = "Gemiddelde tekens per seconde: " + wps;
        CorrectCount.Content = "Aantal goed: " + s_ExerciseStatisticsController.NumberCorrect;
        CorrectPercentage.Content = $"Percentage goed: {correctPercentage}%";
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