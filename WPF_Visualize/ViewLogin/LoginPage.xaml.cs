using Controller;
using System.Windows;
using System.Windows.Controls;
using WPF_Visualize.ViewClass;
using WPF_Visualize.ViewLogic;
using WPF_Visualize.Views_Navigate;
namespace WPF_Visualize.ViewLogin;

/// <summary>
///     Interaction logic for LoginPage.xaml
/// </summary>
public partial class LoginPage : UserControl
{
    private readonly LoginController _loginController;

    public LoginPage(bool isTeacher)
    {
        InitializeComponent();
        _loginController = new LoginController(isTeacher);
        _setText();
        _loginController.LoginEvent += LoginEvent;
    }

    private void _setText()
    {
        if (_loginController.IsTeacher)
        {
            WelcomeBanner.Content = "Login leraar";
            LoginKey.Content = "Email";
        }
        else
        {
            WelcomeBanner.Content = "Login Leerling";
            LoginKey.Content = "Gebruikersnaam";
        }
    }

    private void OnLogIn(object sender, RoutedEventArgs e)
    {
        //UserControlController.MainWindowChange(this, new StudentMain());
        // try
        // {
        _loginController.CheckLogin(LoginKeyTextBox.Text, PasswordPasswordBox.Password);
        // }
        // catch (Exception exception)
        // {
        //     Console.WriteLine(exception);
        //     throw new Exception();
        // }
    }

    private void LoginEvent(object sender, LoginEventArgs e)
    {
        if (e.IsLoggedin)
        {
            if (e.IsTeacher)
            {
                UserControlController.MainWindowChange(this, new ClassSelect());
            }
            else
            {
                UserControlController.MainWindowChange(this, new StudentMain());
            }
        }
        else
        {
            ErrorMessage.Content = "Fout: Inloggegevens zijn niet correct";
        }
    }

    private void OnBack(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new LoginChoose());
    }
}