using System.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using WPF_Visualize.ViewLogic;
using Controller;
using System.Collections.Generic;

namespace WPF_Visualize;

/// <summary>
///     Interaction logic for Statistics.xaml
/// </summary>
public partial class Statistics : UserControl
{
	StatisticsController StatsController;
	List<string> letterstatistics;
	List<string> wordstatistics;
	List<string> storystatistics;
	List<string> totalstatistics;
	string pupilname;
	private int _userID;
	//[0] is PupilID
	//[1] is Type
	//[2] is AmountCorrect
	//[3] is AmountFalse
	//[4] is AssignmentsMade
	//[5] is KeyPerSec
	//[6] is Score

	public Statistics(int userID)
	{
		this._userID = userID;
		StatsController = new StatisticsController(_userID);
		letterstatistics = StatsController.LetterStatistics;
		wordstatistics = StatsController.WordStatistics;
		storystatistics = StatsController.StoryStatistics;
		totalstatistics = StatsController.TotalStatistics;
		pupilname = StatsController.PupilName;

		InitializeComponent();
		_InitializeLabels();
	}

	private void _InitializeLabels()
	{
		//sets name label
		Name.Content = pupilname;

		//sets the labels for total statistics
		TotalKeyPerSecond.Content = totalstatistics[3];  //TotalKeyPerSecond
		TotalMistakes.Content = totalstatistics[1];  //TotalMistakes
		TotalCorrect.Content = totalstatistics[0];  //TotalCorrect
		TotalFinishedExercises.Content = totalstatistics[2];  //TotalFinishedExercises
		TotalScore.Content = totalstatistics[4];   //TotalScore

		//sets the labels for letter statistics
		LetterKeyPerSecond.Content = letterstatistics[5];
		LettersMistakes.Content = letterstatistics[3];
		LettersCorrect.Content = letterstatistics[2];
		LetterFinishedExercises.Content = letterstatistics[4];
		LetterScore.Content = letterstatistics[6];

		//sets the labels for word statistics
		WordKeyPerSecond.Content = wordstatistics[5];
		WordsMistakes.Content = wordstatistics[3];
		WordsCorrect.Content = wordstatistics[2];
		WordFinishedExercises.Content = wordstatistics[4];
		WordScore.Content = wordstatistics[6];

		//sets the labels for story statistics
		StoryKeyPerSecond.Content = storystatistics[5];
		StoryMistakes.Content = storystatistics[3];
		StoryCorrect.Content = storystatistics[2];
		StoryFinishedExercises.Content = storystatistics[4];
		StoryScore.Content = storystatistics[6];
	}

	private void OnBack(object sender, RoutedEventArgs e)
	{
		//_cleanup();
		UserControlController.MainWindowChange(this, new StudentMain());
	}
}