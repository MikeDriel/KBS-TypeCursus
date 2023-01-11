using Controller;
using System.Windows;
using System.Windows.Controls;
using WPF_Visualize.ViewLogic;
using WPF_Visualize.ViewLogin;
using WPF_Visualize.Views_Statistics;
namespace WPF_Visualize.Views_Navigate;

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
        UserControlController.MainWindowChange(this, new Statistics((int)LoginController.s_UserId));
    }

    private void OnLeaderBoard(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new LeaderBoard((int)LoginController.s_UserId));
    }

    private void OnLogOut(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new LoginChoose());
    }

    // Close the application
    private void OnExit(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}