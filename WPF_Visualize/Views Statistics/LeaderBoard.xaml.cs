using Controller;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPF_Visualize.ViewLogic;

namespace WPF_Visualize;

/// <summary>
///     Interaction logic for LeaderBoard.xaml
/// </summary>
public partial class LeaderBoard : UserControl
{
	StatisticsController statisticsController;
	private List<List<string>> _leaderBoardList { get; set; }
	public LeaderBoard(int userID)
	{
		statisticsController = new StatisticsController(userID);

		_leaderBoardList = statisticsController.LeaderBoardList;
		_leaderBoardList.Sort((x, y) => int.Parse(y[3]).CompareTo(int.Parse(x[3])));

		InitializeComponent();
		InitializeLabels();
		AddLeaderboardToStackPanel(_leaderBoardList);
	}

	/// <summary>
	/// Gives the top 3 pupils a place on the podium.
	/// </summary>
	public void InitializeLabels()
	{
		for (int i = 0; i < 3; i++)
		{
			if (_leaderBoardList[i][1].Length + _leaderBoardList[i][2].Length > 6)
			{
				_leaderBoardList[i][2] = _leaderBoardList[i][2].Substring(0, 1) + ".";
			}
		} 

		//blank spaces needed for alignment
		Number1.Content = $"   {_leaderBoardList[0][1]} {_leaderBoardList[0][2]} \n {_leaderBoardList[0][3]} punten";
		Number2.Content = $"   {_leaderBoardList[1][1]} {_leaderBoardList[1][2]} \n {_leaderBoardList[1][3]} punten";
		Number3.Content = $"   {_leaderBoardList[2][1]} {_leaderBoardList[2][2]} \n {_leaderBoardList[2][3]} punten";
	}

	/// <summary>
	/// Adds the leaderboard to stack panel.
	/// The name of the current user is put in bold.
	/// </summary>
	/// <param name="leaderboard"></param>
	private void AddLeaderboardToStackPanel(List<List<string>> leaderboard)
	{
		leaderboard.RemoveRange(0, 3); //remove top 3.
		int position = 4;
		foreach (List<string> pupil in leaderboard)
		{
			FontWeight fontweight = new FontWeight();

			//check if the pupil is the logged in user.
			if (pupil[0] == LoginController.s_UserId.ToString())
			{
				fontweight = FontWeights.Bold;
			}
			else
			{
				fontweight = FontWeights.Normal;
			}

			var stackpanel = new StackPanel
			{
				Orientation = Orientation.Horizontal,
			};

			var labelposition = new Label
			{
				Content = $"{position}",
				FontSize = 25,
				Foreground = Brushes.White,
				Width = 110,
				FontWeight = fontweight
			};

			var labelname = new Label
			{
				Content = pupil[1] + " " + pupil[2],
				Foreground = Brushes.White,
				FontSize = 25,
				Width = 260,
				FontWeight = fontweight
			};

			var labelscore = new Label
			{
				Content = pupil[3],
				Foreground = Brushes.White,
				FontSize = 25,
				Width = 50,
				FontWeight = fontweight
			};

			stackpanel.Children.Add(labelposition);
			stackpanel.Children.Add(labelname);
			stackpanel.Children.Add(labelscore);
			
			StudentsPanel.Children.Add(stackpanel);

			position++; //increase position.
		}
	}

	private void OnBack(object sender, RoutedEventArgs e)
	{
		//_cleanup();
		UserControlController.MainWindowChange(this, new StudentMain());
	}
}