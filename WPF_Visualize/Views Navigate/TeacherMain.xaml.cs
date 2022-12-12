using System.Windows;
using System.Windows.Controls;
using Controller;
using WPF_Visualize.ViewLogic;
using WPF_Visualize.ViewLogin;

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

    private void OnLogOut(object sender, RoutedEventArgs e)
    {
        LoginController.LogOut();
        UserControlController.MainWindowChange(this, new LoginChoose());
    }
}