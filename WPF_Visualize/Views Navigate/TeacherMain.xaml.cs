using System.Windows;
using System.Windows.Controls;
using Controller;
using WPF_Visualize.ViewLogic;
using WPF_Visualize.ViewLogin;
using WPF_Visualize.Views_Statistics;

namespace WPF_Visualize.Views_Navigate;

/// <summary>
///     Interaction logic for TeacherMain.xaml
/// </summary>
public partial class TeacherMain : UserControl
{
    public TeacherMain()
    {
        InitializeComponent();
    }

    private void OnClassStatistics(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new ClassStatistics());
    }

    private void OnClassSettings(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new ClassSettings());
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