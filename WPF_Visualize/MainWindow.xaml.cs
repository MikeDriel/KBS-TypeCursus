using System.Windows;
using WPF_Visualize.ViewLogic;

namespace WPF_Visualize;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        contentControl.Content = UserControlController.Content;
        UserControlController.OnWindowChange += MainWindow_OnWindowChange;
    }

    private void MainWindow_OnWindowChange(object source, OnWindowChangeEventArgs e)
    {
        contentControl.Content = UserControlController.Content;
    }
}