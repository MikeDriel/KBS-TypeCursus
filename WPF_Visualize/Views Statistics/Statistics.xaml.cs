﻿using Controller;
using Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WPF_Visualize.ViewLogic;
using WPF_Visualize.Views_Navigate;
namespace WPF_Visualize.Views_Statistics;

/// <summary>
///     Interaction logic for Statistics.xaml
/// </summary>
public partial class Statistics : UserControl
{
    private readonly int _userID;
    private readonly Database database;
    private readonly List<string> letterstatistics;
    private readonly string pupilname;
    private readonly StatisticsController StatsController;
    private readonly List<string> storystatistics;
    private readonly List<string> totalstatistics;
    private readonly List<string> wordstatistics;

    public int StatisticsClassId { get; set; }

	//[0] is PupilID
	//[1] is Type
	//[2] is AmountCorrect
	//[3] is AmountFalse
	//[4] is AssignmentsMade
	//[5] is KeyPerSec
	//[6] is Score

	public Statistics(int userID)
    {
        _userID = userID;
        StatsController = new StatisticsController(_userID);
        database = new Database();
        letterstatistics = StatsController.LetterStatistics;
        wordstatistics = StatsController.WordStatistics;
        storystatistics = StatsController.StoryStatistics;
        totalstatistics = StatsController.TotalStatistics;
        pupilname = StatsController.PupilName;

        StatisticsClassId = ClassStatistics.S_ClassId;

        InitializeComponent();
        InitializeLabels();
    }

    private void InitializeLabels()
    {
        //sets name label
        Name.Content = pupilname;

        LetterLevel.Content = database.GetLevel(_userID, TypeExercise.Letter).ToString().Substring(5);
        WordLevel.Content = database.GetLevel(_userID, TypeExercise.Word).ToString().Substring(5);
        StoryLevel.Content = database.GetLevel(_userID, TypeExercise.Story).ToString().Substring(5);

        //sets the labels for total statistics
        TotalKeyPerSecond.Content = totalstatistics[3]; //TotalKeyPerSecond
        TotalMistakes.Content = totalstatistics[1]; //TotalMistakes
        TotalCorrect.Content = totalstatistics[0]; //TotalCorrect
        TotalFinishedExercises.Content = totalstatistics[2]; //TotalFinishedExercises
        TotalScore.Content = totalstatistics[4]; //TotalScore

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

    /// <summary>
    ///     Checks if the current user is a teacher,
    ///     if that's the case it'll go back to the class statistics window.
    ///     Otherwise it'll go back to the main menu for the pupil.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnBack(object sender, RoutedEventArgs e)
    {
        ClassStatistics classStats = new ClassStatistics(StatisticsClassId);
        classStats.StatisticsClassId = StatisticsClassId;
        if (LoginController.s_IsTeacher)
        {
            UserControlController.MainWindowChange(this, classStats);
        }
        else
        {
            UserControlController.MainWindowChange(this, new StudentMain());
        }
    }
}