using Model;
using System.Windows;
using System.Windows.Controls;
using WPF_Visualize.ViewLogic;
using WPF_Visualize.Views_Exercises;
namespace WPF_Visualize.Views_Navigate;

/// <summary>
///     Interaction logic for ExerciseSelect.xaml
/// </summary>
public partial class ExerciseSelect : UserControl
{
    public ExerciseSelect()
    {
        InitializeComponent();
    }

    private void OnBack(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new StudentMain());
    }

    private void OnLetterExcersice_Select(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new Exercise(TypeExercise.Letter));
    }

    private void OnWordExercise(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new Exercise(TypeExercise.Word));
    }

    private void OnStoryExercise(object sender, RoutedEventArgs e)
    {
        UserControlController.MainWindowChange(this, new Exercise(TypeExercise.Story));
    }
}