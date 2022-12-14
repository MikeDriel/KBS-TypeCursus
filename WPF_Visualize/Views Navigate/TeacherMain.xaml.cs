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
    public TeacherMain(int classId)
    {
        InitializeComponent();
        setClassNameLabel(classId);
    }

    private void setClassNameLabel(int classId)
    {
        var className = Database.GetClassName(classId);
        ClassNameLabel.Content = $"Gekozen klas: {className}";
    }

    private void OnClassStatistics(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new ClassStatistics());
    }

    private void OnClassSettings(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new ClassSettings());
    }

    private void OnBack(object sender, RoutedEventArgs e)
    {
        
        UserControlController.MainWindowChange(this, new ClassSelect());
    }

    private void OnLogOut(object sender, RoutedEventArgs e)
    {
        LoginController.LogOut();
        UserControlController.MainWindowChange(this, new LoginChoose());
    }

    // Close the application
    private void OnAfsluiten(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}