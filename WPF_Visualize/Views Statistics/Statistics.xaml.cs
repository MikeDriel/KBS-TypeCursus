using System.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using WPF_Visualize.ViewLogic;
using Controller;

namespace WPF_Visualize;

/// <summary>
///     Interaction logic for Statistics.xaml
/// </summary>
public partial class Statistics : UserControl
{
    StatisticsController StatsController = new StatisticsController();
    
    public Statistics()
    {
        InitializeComponent();
        _InitializeLabels();
    }
    
    private void _InitializeLabels()
    {
        
        var statistics = StatsController.GetStatisticsFromDatabase();

        //sets the labels
        WPS.Content = "Gemiddelde tekens per seconde: " + statistics ;
    }


    private void OnBack(object sender, RoutedEventArgs e)
    {
        //_cleanup();
        UserControlController.MainWindowChange(this, new StudentMain());
    }
}