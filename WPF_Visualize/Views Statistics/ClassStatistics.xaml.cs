using Controller;
using Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPF_Visualize.ViewLogic;
using WPF_Visualize.Views_Navigate;
namespace WPF_Visualize.Views_Statistics;

/// <summary>
///     Interaction logic for ClassStatistics.xaml
/// </summary>
public partial class ClassStatistics : UserControl
{
    private readonly Database _database;

    public ClassStatistics(int classid)
    {
        _database = new Database();

        StatisticsClassId = classid;
        SClassId = classid;

        InitializeComponent();
        InitializeClassStatistics();
        AddPupilsToStackPanel();
    }
    private List<Pupil> ClassStatisticsList { get; set; }
    private List<string> UserIds { get; set; }

    public int StatisticsClassId { get; set; }
    public static int SClassId { get; set; }

    /// <summary>
    ///     Prepares the ClassStatisticsList which is needed to
    ///     display the pupils on the stackpanel.
    /// </summary>
    private void InitializeClassStatistics()
    {
        UserIds = _database.GetClass(StatisticsClassId); //get the amount of pupils.
        ClassStatisticsList = _database.GenerateClassStatistics(UserIds.Select(int.Parse).ToList(), StatisticsClassId);
    }

    /// <summary>
    ///     Adds the pupils to the stackpanel.
    /// </summary>
    private void AddPupilsToStackPanel()
    {

        foreach (Pupil pupil in ClassStatisticsList)
        {
            StackPanel stackpanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            Label labelname = new Label
            {
                Content = $"{pupil.Lastname}, {pupil.Firstname}",
                FontSize = 25,
                Foreground = Brushes.White,
                Width = 260
            };

            Label labelscore = new Label
            {
                Content = pupil.Score,
                Foreground = Brushes.White,
                FontSize = 25,
                Width = 250
            };

            Label labelassignmentscompleted = new Label
            {
                Content = pupil.AssignmentsMade,
                Foreground = Brushes.White,
                FontSize = 25,
                Width = 250
            };

            Button button = new Button
            {
                Content = "Meer info",
                Style = (Style)FindResource("StatisticsMoreInfoButton"),
                FontSize = 16
            };

            button.Click += (sender, args) => UserControlController.MainWindowChange(this, new Statistics(pupil.PupilID));

            stackpanel.Children.Add(labelname);
            stackpanel.Children.Add(labelscore);
            stackpanel.Children.Add(labelassignmentscompleted);
            stackpanel.Children.Add(button);

            StudentsPanel.Children.Add(stackpanel);
        }
    }

    /// <summary>
    ///     Order ClassStatisticsList by name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnName(object sender, RoutedEventArgs e)
    {
        ClassStatisticsList = ClassStatisticsList.OrderBy(x => x.Lastname).ToList();
        StudentsPanel.Children.Clear();
        AddPupilsToStackPanel();
    }

    /// <summary>
    ///     Order ClassStatisticsList by score.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnScore(object sender, RoutedEventArgs e)
    {
        ClassStatisticsList = ClassStatisticsList.OrderByDescending(x => x.Score).ToList();
        StudentsPanel.Children.Clear();
        AddPupilsToStackPanel();
    }

    /// <summary>
    ///     Order ClassStatisticsList by assignments made.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnAssignmentsMade(object sender, RoutedEventArgs e)
    {
        ClassStatisticsList = ClassStatisticsList.OrderByDescending(x => x.AssignmentsMade).ToList();
        StudentsPanel.Children.Clear();
        AddPupilsToStackPanel();
    }

    private void OnBack(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new TeacherMain(LoginController.GetUserId()));
    }
}