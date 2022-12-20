﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPF_Visualize.ViewLogic;
using static System.Formats.Asn1.AsnWriter;

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
        Exercise.StatisticsController.SendStatisticInformationToDatabase(Math.Round(Exercise.StatisticsController.CharactersPerSecond.Values.Average(), 1)) ;
    }


	//sets all labels
	private void _InitializeLabels()
	{
		var wps = Exercise.s_StatisticsController.CharactersPerSecond.Values.Average();
		wps = Math.Round(wps, 1);
		//calculate the percentage of correct typed characters
		var correctPercentage = Exercise.s_StatisticsController.NumberCorrect /
		                        (Exercise.s_StatisticsController.NumberCorrect +
		                         (double)Exercise.s_StatisticsController.NumberOfMistakes) *
		                        100;
		correctPercentage = Math.Round(correctPercentage, 1);


        //sets the labels
        Totaltime.Content = "Totale tijd: " + Exercise.s_StatisticsController.CurrentTime.ToString("mm:ss") ;
        MistakeCount.Content = "Aantal fouten: " + Exercise.s_StatisticsController.NumberOfMistakes;
        WPS.Content = "Gemiddelde tekens per seconde: " + wps;
        CorrectCount.Content = "Aantal goed: " + Exercise.s_StatisticsController.NumberCorrect;
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