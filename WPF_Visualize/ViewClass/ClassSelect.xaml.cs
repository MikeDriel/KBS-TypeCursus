using Controller;
using Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WPF_Visualize.ViewLogic;
using WPF_Visualize.ViewLogin;
using WPF_Visualize.Views_Navigate;
namespace WPF_Visualize.ViewClass;

/// <summary>
///     Interaction logic for ClassSelect.xaml
/// </summary>
public partial class ClassSelect : UserControl
{
    private readonly int _userId = LoginController.GetUserId();
    public Database Database = new Database();
    public ClassSelect()
    {
        InitializeComponent();
        AddButtonsForClasses();
    }

    private void AddButtonsForClasses()
    {

        List<int> classes = Database.GetClasses(_userId);
        foreach (int classId in classes)
        {
            string className = Database.GetClassName(classId);
            Button button = new Button
            {
                Content = className,
                Style = (Style)FindResource("ClassSelecterButton")

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