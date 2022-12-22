using System.Windows;
using System.Windows.Controls;
using Model;
using WPF_Visualize.ViewLogic;

namespace WPF_Visualize;

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