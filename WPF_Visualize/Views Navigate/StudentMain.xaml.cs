using System.Windows;
using System.Windows.Controls;
using Controller;
using WPF_Visualize.ViewLogic;
using WPF_Visualize.ViewLogin;

namespace WPF_Visualize;

/// <summary>
///     Interaction logic for StudentMain.xaml
/// </summary>
public partial class StudentMain : UserControl
{
    public StudentMain()
    {
        InitializeComponent();
    }

    private void OnExerciseSelect(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new ExerciseSelect());
    }

    private void OnStatistics(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new Statistics((int)LoginController.UserId));
    }

    private void OnLeaderBoard(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new LeaderBoard());
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