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

namespace WPF_Visualize.Views_Statistics
{
	/// <summary>
	/// Interaction logic for ClassStatistics.xaml
	/// </summary>
	public partial class ClassStatistics : UserControl
	{
		private Database _database;

		public int StatisticsClassId { get; set; }
		public List<List<string>> ClassStatisticsList { get; private set; }
		public List<string> UserIds { get; private set; }
		public bool IsTeacher { get; set; } = true;

		public ClassStatistics(int classid)
		{
			_database = new();

			SetStatisticsClassID(classid);

			InitializeComponent();
			InitializeClassStatistics();
			AddPupilsToStackPanel();
		}

		private void InitializeClassStatistics()
		{
			UserIds = _database.GetClass(StatisticsClassId); //get the amount of pupils.
			ClassStatisticsList = _database.GenerateClassStatistics(UserIds.Select(int.Parse).ToList(), StatisticsClassId);
		}

		private void SetStatisticsClassID(int classid)
		{
			StatisticsClassId = classid;
		}
		
		public int GetStatisticsClassID()
		{
			return StatisticsClassId;
		}

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
					Content = "null",
					Foreground = Brushes.White,
					FontSize = 25,
					Width = 500,
				};

				var button = new Button
				{
					Content = "Meer info",
					Style = (Style)FindResource("ClassSelecterButton"),
				};
				button.Click += (sender, args) => UserControlController.MainWindowChange(this, new Statistics(int.Parse(pupil[0])));


				stackpanel.Children.Add(labelname);
				stackpanel.Children.Add(labelscore);
				stackpanel.Children.Add(labelassignmentscompleted);
				StudentsPanel.Children.Add(button);

				StudentsPanel.Children.Add(stackpanel);
			}
		}

		private void OnBack(object sender, RoutedEventArgs e)
		{
			//_cleanup();
			UserControlController.MainWindowChange(this, new TeacherMain(LoginController.GetUserId()));
		}
	}
}
