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
        WPS.Content = "Gemiddelde tekens per seconde: " + storystatistics[1];
    }


    private void OnBack(object sender, RoutedEventArgs e)
    {
        //_cleanup();
        UserControlController.MainWindowChange(this, new StudentMain());
    }
}