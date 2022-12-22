using Controller;
using Model;
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
    /// Interaction logic for ClassSelect.xaml
    /// </summary>
    public partial class ClassSelect : UserControl
    {
        public Database Database = new();
        int user_id = LoginController.GetUserId();
        public ClassSelect()
        {
            InitializeComponent();
            AddButtonsForClasses();
        }

        private void AddButtonsForClasses()
        {

            List<int> classes = Database.GetClasses(user_id);
            foreach (int classId in classes)
            {
                string className = Database.GetClassName(classId);
                var button = new Button
                {
                    Content = className,
                    Style = (Style)FindResource("ClassSelecterButton"),

                };
                button.Click += (sender, args) => UserControlController.MainWindowChange(this, new TeacherMain(classId));
                ClassStackPanel.Children.Add(button);
            }
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
        
        private void OnClassAdd(object sender, RoutedEventArgs e)
        {
            UserControlController.MainWindowChange(this, new ClassSettings());
        }

    }
}
