using Model;
using System.Windows;
using System.Windows.Controls;
using WPF_Visualize.ViewClass;
using WPF_Visualize.ViewLogic;
using WPF_Visualize.ViewLogin;
using WPF_Visualize.Views_Statistics;
namespace WPF_Visualize.Views_Navigate;

/// <summary>
///     Interaction logic for TeacherMain.xaml
/// </summary>
public partial class TeacherMain : UserControl
{
    private readonly int _classId;
    public Database Database = new();

    public TeacherMain(int classId)
    {
        InitializeComponent();
        _classId = classId;
        SetClassNameLabel(_classId);
    }

    private void SetClassNameLabel(int classId)
    {
        string className = Database.GetClassName(classId);
        ClassNameLabel.Content = $"Gekozen klas: {className}";
    }

    private void OnClassStatistics(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new ClassStatistics(_classId));
    }

    private void OnClassSettings(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new ClassSettings(_classId));
    }

    private void OnBack(object sender, RoutedEventArgs e)
    {

        UserControlController.MainWindowChange(this, new ClassSelect());
    }

    private void OnLogOut(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new LoginChoose());
    }

    // Close the application
    private void OnExit(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}