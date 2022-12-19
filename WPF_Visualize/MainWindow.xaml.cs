using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Model;
using WPF_Visualize.ViewLogic;
using WPF_Visualize.ViewLogin;
using WPF_Visualize.Views_Navigate;

namespace WPF_Visualize;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly Database database = new();

    public MainWindow()
    {
        InitializeComponent();

        contentControl.Content = UserControlController.s_Content;
        UserControlController.OnWindowChange += MainWindow_OnWindowChange;
    }

    private void MainWindow_OnWindowChange(object source, OnWindowChangeEventArgs e)
    {
        contentControl.Content = UserControlController.s_Content;
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        contentControl.Content = new LoadingScreen();
        SshTunnel tunnel = new SshTunnel();

        CheckDatabaseConnectionAsync();
    }

    private async Task CheckDatabaseConnectionAsync()
    {
        if (await database.IsServerConnected())
        {
            contentControl.Content = new LoginChoose();
        }
        else
        {
            contentControl.Content = new NoDatabase();
        }
    }
}

internal class OnWindowChange : EventArgs
{
    public OnWindowChange(UserControl content)
    {
        Content = content;
    }

    public UserControl Content { get; set; }
}