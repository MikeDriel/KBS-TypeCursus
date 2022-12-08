using System.Windows;
using System.Windows.Controls;
using WPF_Visualize.ViewLogic;

namespace WPF_Visualize.ViewLogin;

/// <summary>
///     Interaction logic for LoginChoose.xaml
/// </summary>
public partial class LoginChoose : UserControl
{
    public LoginChoose()
    {
        InitializeComponent();
    }

    private void OnStudent(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new LoginPage(false));
    }


    private void OnTeacher(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new LoginPage(true));
    }
}