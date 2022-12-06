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
using OxyPlot;
using OxyPlot.Series;
using System.Windows.Media.Animation;

namespace WPF_Visualize
{
	/// <summary>
	/// Interaction logic for ResultatenOefening.xaml
	/// </summary>
	public partial class ResultatenOefening : UserControl
	{
		public ResultatenOefening()
		{
			InitializeComponent();
            _InitializeLabels();
		}

        private void _InitializeLabels()
        {
            WordsPerMinute.Content = "5";
            AmountWrong.Content = "5";
            //MVF.Content = " ";
        }
        private void OnBack(object sender, RoutedEventArgs e)
        {
            //_cleanup();
            UserControlController.MainWindowChange(this, new ExerciseSelect());
        }
    }
}
