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
using WPF_Visualize.ViewLogin;

namespace WPF_Visualize.Views_Navigate
{
    /// <summary>
    /// Interaction logic for TeacherMain.xaml
    /// </summary>
    public partial class TeacherMain : UserControl
    {
        public TeacherMain()
        {
            InitializeComponent();
        }

        private void OnLogOut(object sender, RoutedEventArgs e)
        {
            UserControlController.MainWindowChange(this, new LoginChoose());
        }
    }
}
