using System.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using WPF_Visualize.ViewLogic;
using Controller;
using System.Collections.Generic;

namespace WPF_Visualize;

/// <summary>
///     Interaction logic for Statistics.xaml
/// </summary>
public partial class Statistics : UserControl
{
    StatisticsController StatsController = new StatisticsController();
    List<string> letterstatistics;
    List<string> wordstatistics;
    List<string> storystatistics;
    List<string> totalstatistics;
    //index [0] is PupilID
    //index [1] is Type
    //index [2] is AmountCorrect
    //index [3] is AmountFalse
    //index [4] is AssignmentsMade
    //index [5] is KeyPerSec
    //index [6] is Score

    public Statistics()
    {
        letterstatistics = StatsController.GetLetterStatisticsFromDatabase();
        wordstatistics = StatsController.GetWordStatisticsFromDatabase();
        storystatistics = StatsController.GetStoryStatisticsFromDatabase();

        InitializeComponent();
        _InitializeLabels();
    }
    
    private void _InitializeLabels()
    {
        //sets the labels
        KPS.Content = "Gemiddelde typsnelheid: " +  storystatistics[5] + " tekens per seconden";
        TGF.Content = "Totaal gemaakte fouten: " + storystatistics[3];
        TGA.Content = "Totaal goede antwoorden: " + storystatistics[2];
        AGO.Content = "Aantal gemaakte opdrachten: " + storystatistics[4];
        SC.Content = "Score: " + storystatistics[6];
    }


    private void OnBack(object sender, RoutedEventArgs e)
    {
        //_cleanup();
        UserControlController.MainWindowChange(this, new StudentMain());
    }
}