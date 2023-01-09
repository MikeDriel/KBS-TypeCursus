﻿using System.Windows;
using System.Windows.Controls;
using Controller;
using Model;
using WPF_Visualize.ViewLogic;
using WPF_Visualize.ViewLogin;
using WPF_Visualize.Views_Statistics;

namespace WPF_Visualize.Views_Navigate;

/// <summary>
///     Interaction logic for TeacherMain.xaml
/// </summary>
public partial class TeacherMain : UserControl
{
    public Database Database = new();
    int classId;
    public TeacherMain(int classId)
    {
        InitializeComponent();
        this.classId = classId;
        setClassNameLabel(this.classId);
    }

    private void setClassNameLabel(int classId)
    {
        var className = Database.GetClassName(classId);
        ClassNameLabel.Content = $"Gekozen klas: {className}";
    }

    private void OnClassStatistics(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new ClassStatistics(this.classId));
    }
    
    private void OnClassSettings(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new ClassSettings(classId));
    }

    private void OnBack(object sender, RoutedEventArgs e)
    {
        
        UserControlController.MainWindowChange(this, new ClassSelect());
    }

    private void OnLogOut(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new LoginChoose());
    }

    // Close the application
    private void OnAfsluiten(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}