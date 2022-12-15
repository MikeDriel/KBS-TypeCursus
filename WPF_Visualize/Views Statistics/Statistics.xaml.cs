﻿using System.Linq;
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
        letterstatistics = StatsController.LetterStatistics;
        wordstatistics = StatsController.WordStatistics;
        storystatistics = StatsController.StoryStatistics;

        InitializeComponent();
        _InitializeLabels();
    }
    
    private void _InitializeLabels()
    {
        //sets the labels for letter statistics
        LKPS.Content = letterstatistics[5];
        LTGF.Content = letterstatistics[3];
        LTGA.Content = letterstatistics[2];
        LAGO.Content = letterstatistics[4];
        LSC.Content = letterstatistics[6];

        //sets the labels for word statistics
        WKPS.Content = wordstatistics[5];
        WTGF.Content = wordstatistics[3];
        WTGA.Content = wordstatistics[2];
        WAGO.Content = wordstatistics[4];
        WSC.Content = wordstatistics[6];

        //sets the labels for story statistics
        SKPS.Content = storystatistics[5];
        STGF.Content = storystatistics[3];
        STGA.Content = storystatistics[2];
        SAGO.Content = storystatistics[4];
        SSC.Content = storystatistics[6];
    }


    private void OnBack(object sender, RoutedEventArgs e)
    {
        //_cleanup();
        UserControlController.MainWindowChange(this, new StudentMain());
    }
}