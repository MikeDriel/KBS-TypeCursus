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
using System.Windows.Shapes;
using WPF_Visualize.ViewLogic;

namespace WPF_Visualize
{
    /// <summary>
    /// Interaction logic for StudentMain.xaml
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
            UserControlController.MainWindowChange(this, new Statistics());
        }

        private void OnLeaderBoard(object sender, RoutedEventArgs e)
        {
            UserControlController.MainWindowChange(this, new LeaderBoard());
        }

        private void OnLog_Out(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
    }
}
