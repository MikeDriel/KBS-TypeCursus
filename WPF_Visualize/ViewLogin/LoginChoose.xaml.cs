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
using WPF_Visualize.ViewLogic;

namespace WPF_Visualize.ViewLogin
{
    /// <summary>
    /// Interaction logic for LoginChoose.xaml
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
}
