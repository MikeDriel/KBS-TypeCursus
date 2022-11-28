using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WPF_Visualize
{
    /// <summary>
    /// Interaction logic for ExerciseSelect.xaml
    /// </summary>
    public partial class ExerciseSelect : UserControl
    {
        public ExerciseSelect()
        {
            InitializeComponent();
        }

        private void OnBack(object sender, RoutedEventArgs e)
        {
            UserControlController.InvokeEvent(this, new StudentMain());
        }

        private void OnLetterExcersice_Select(object sender, RoutedEventArgs e)
        {
			UserControlController.InvokeEvent(this, new LetterExercise());
		}
    }
}
