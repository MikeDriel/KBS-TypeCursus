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
using WPF_Visualize.ViewLogic;

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
			this.contentControl.Content = UserControlController.Content;
			UserControlController.OnWindowChange += MainWindow_OnWindowChange;
		}

		private void MainWindow_OnWindowChange(object source, OnWindowChangeEventArgs e)
		{
			this.contentControl.Content = UserControlController.Content;
		}

		private void OnExercise_Select(object sender, RoutedEventArgs e)
		{
			this.contentControl.Content = new ExerciseSelect();
		}
	}

	internal class OnWindowChange : EventArgs
	{
		public UserControl Content { get; set; }
		public OnWindowChange(UserControl content)
		{
			Content = content;
		}
	}
}
