using System;
using System.Windows;
using System.Windows.Controls;
using Model;
using WPF_Visualize.ViewLogic;
using WPF_Visualize.Views_Navigate;

namespace WPF_Visualize
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Database database = new Database();
        public MainWindow()
        {
            InitializeComponent();
            
            this.ContentControl.Content = UserControlController.Content;
            UserControlController.OnWindowChange += MainWindow_OnWindowChange;
            
        }

        private void MainWindow_OnWindowChange(object source, OnWindowChangeEventArgs e)
        {
            this.ContentControl.Content = UserControlController.Content;
        }

        private void OnExercise_Select(object sender, RoutedEventArgs e)
        {
            this.ContentControl.Content = new ExerciseSelect();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!database.IsServerConnected())
            {
                this.ContentControl.Content = new NoDataBaseConnection();
            }
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