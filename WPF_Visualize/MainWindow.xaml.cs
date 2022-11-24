using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace WPF_Visualize
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        public MainWindow()
		{
            InitializeComponent();
            this.contentControl.Content = new LetterExercise();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.contentControl.Content = new ExerciseSelect();
        }

        private void Keydown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.Key);
            Trace.WriteLine(e.Key);
        }
    }
}
