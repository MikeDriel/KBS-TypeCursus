using Controller;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WPF_Visualize.ViewLogic;

namespace WPF_Visualize;

/// <summary>
///     Interaction logic for LeaderBoard.xaml
/// </summary>
public partial class LeaderBoard : UserControl
{
	StatisticsController statisticsController = new();
	private List<List<string>> LeaderBoardList { get; set; }
	public LeaderBoard()
	{
		LeaderBoardList = statisticsController.LeaderBoardList;
		//sort leaderboardlist by score
		LeaderBoardList.Sort((x, y) => int.Parse(y[1]).CompareTo(int.Parse(x[1])));

		InitializeComponent();
		InitializeLabels();
	}

	public void InitializeLabels()
	{
		Number1.Content = LeaderBoardList[0][0].ToString();
		Number2.Content = LeaderBoardList[1][0].ToString();
		Number3.Content = LeaderBoardList[2][0].ToString();
	}
	private void OnBack(object sender, RoutedEventArgs e)
	{
		//_cleanup();
		UserControlController.MainWindowChange(this, new StudentMain());
	}
}