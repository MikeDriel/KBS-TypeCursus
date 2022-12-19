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

	string Number1Name;
	string Number2Name;
	string Number3Name;

	List<string> LeaderBoardList = new();
	public LeaderBoard()
	{
		//LeaderBoardList = statisticsController.LeaderBoardList;

		//Number1Name = LeaderBoardList[0].ToString();
		//Number2Name = LeaderBoardList[1].ToString();
		//Number3Name = LeaderBoardList[2].ToString();
		

		InitializeComponent();
		InitializeLabels();
	}

	public void InitializeLabels()
	{
		Number1.Content = Number1Name;
		Number2.Content = Number2Name;
		Number3.Content = Number3Name;
	}
	private void OnBack(object sender, RoutedEventArgs e)
	{
		//_cleanup();
		UserControlController.MainWindowChange(this, new StudentMain());
	}
}