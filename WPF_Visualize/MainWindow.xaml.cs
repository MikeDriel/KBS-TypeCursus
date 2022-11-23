using System;
using System.Collections.Generic;
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
            this.contentControl.Content = UserControlEvent.Content;
            UserControlEvent.OnWindowChange += MainWindow_OnWindowChange;
        }
        
        private void MainWindow_OnWindowChange(object source, OnWindowChangeEventArgs e)
        {
            this.contentControl.Content = UserControlEvent.Content;
        }

    }

    public class OnWindowChangeEventArgs : EventArgs
    {
        public UserControl Content { get; set; }
        public OnWindowChangeEventArgs(UserControl content)
        {
            this.Content = content;
        }
    }
}
