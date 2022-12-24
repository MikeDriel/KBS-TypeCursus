using Controller;
using Model;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Visualize.ViewLogic;
using WPF_Visualize.Views_Navigate;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using VerticalAlignment = System.Windows.VerticalAlignment;

namespace WPF_Visualize.Views_Statistics
{
	/// <summary>
	/// Interaction logic for ClassStatistics.xaml
	/// </summary>
	public partial class ClassStatistics : UserControl
	{
		private Database _database;
		private List<List<string>> ClassStatisticsList { get; set; }
		private List<string> UserIds { get; set; }
		
		public int StatisticsClassId { get; set; }
		public static int SClassId { get; set; }

		//[0] = pupilid
		//[1] = firstname
		//[2] = lastname
		//[3] = score
		//[4] = assignmentsmade

		public ClassStatistics(int classid)
		{
			_database = new();

			StatisticsClassId = classid;
			SClassId = classid;

			InitializeComponent();
			InitializeClassStatistics();
			AddPupilsToStackPanel();
		}

		/// <summary>
		/// Prepares the ClassStatisticsList which is needed to 
		/// display the pupils on the stackpanel.
		/// </summary>
		private void InitializeClassStatistics()
		{
			UserIds = _database.GetClass(StatisticsClassId); //get the amount of pupils.
			ClassStatisticsList = _database.GenerateClassStatistics(UserIds.Select(int.Parse).ToList(), StatisticsClassId);
		}

		/// <summary>
		/// Adds the pupils to the stackpanel.
		/// </summary>
		private void AddPupilsToStackPanel()
		{

			foreach (List<string> pupil in ClassStatisticsList)
			{
				var stackpanel = new StackPanel
				{
					Orientation = Orientation.Horizontal
				};

				var labelname = new Label
				{
					Content = $"{pupil[1]} {pupil[2]}",
					FontSize = 25,
					Foreground = Brushes.White,
					Width = 260,
				};

				var labelscore = new Label
				{
					Content = pupil[3],
					Foreground = Brushes.White,
					FontSize = 25,
					Width = 250,
				};

				var labelassignmentscompleted = new Label
				{
					Content = pupil[4],
					Foreground = Brushes.White,
					FontSize = 25,
					Width = 250,
				};

				var button = new Button
				{
					Content = "Meer info",
					Style = (Style)FindResource("StatisticsMoreInfoButton"),
					FontSize = 16,
				};

				button.Click += (sender, args) => UserControlController.MainWindowChange(this, new Statistics(int.Parse(pupil[0])));

				stackpanel.Children.Add(labelname);
				stackpanel.Children.Add(labelscore);
				stackpanel.Children.Add(labelassignmentscompleted);
				stackpanel.Children.Add(button);

				StudentsPanel.Children.Add(stackpanel);
			}
		}

		/// <summary>
		/// Order ClassStatisticsList by name.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnName(object sender, RoutedEventArgs e)
		{
			ClassStatisticsList.Sort((x,y) => String.CompareOrdinal(x[1], y[2]));
			StudentsPanel.Children.Clear();
			AddPupilsToStackPanel();
		}

		/// <summary>
		/// Order ClassStatisticsList by score.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnScore(object sender, RoutedEventArgs e)
		{
			ClassStatisticsList = ClassStatisticsList.OrderByDescending(x => x[3]).ToList(); 
			StudentsPanel.Children.Clear();
			AddPupilsToStackPanel();
		}

		/// <summary>
		/// Order ClassStatisticsList by assignments made.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnAssignmentsMade(object sender, RoutedEventArgs e)
		{
			ClassStatisticsList = ClassStatisticsList.OrderByDescending(x => x[4]).ToList();
			StudentsPanel.Children.Clear();
			AddPupilsToStackPanel();
		}

		private void OnBack(object sender, RoutedEventArgs e)
		{
			//_cleanup();
			UserControlController.MainWindowChange(this, new TeacherMain(LoginController.GetUserId()));
		}
	}
}
