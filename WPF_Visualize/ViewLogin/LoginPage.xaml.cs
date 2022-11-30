using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Controller;
using WPF_Visualize.ViewLogic;
using WPF_Visualize.Views_Navigate;

namespace WPF_Visualize.ViewLogin
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        LoginController loginController;
        public LoginPage(bool IsTeacher)
        {
            InitializeComponent();
            loginController = new LoginController(IsTeacher);
            _setText();
            loginController.LoginEvent += LoginEvent;
        }

        private void _setText()
        {
            if (loginController.IsTeacher)
            {
                WelcomeBanner.Content = "Login leraar";
                LoginKey.Content = "Email:";
            }
            else
            {
                WelcomeBanner.Content = "Login Leerling";
                LoginKey.Content = "Gebruikersnaam:";
            }
        }

        private void OnLogIn(object sender, RoutedEventArgs e)
        {
            loginController.CheckLogin(LoginKeyTextBox.Text, PasswordPasswordBox.Password);
        }

        private void LoginEvent(object sender, LoginEventArgs e)
        {
            if (e.IsLoggedin)
            {
                if (e.IsTeacher)
                {
                    UserControlController.MainWindowChange(this, new TeacherMain());
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
}
