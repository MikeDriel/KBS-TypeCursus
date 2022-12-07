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
using OxyPlot;
using OxyPlot.Series;
using System.Windows.Media.Animation;
using Controller;

namespace WPF_Visualize
{
	/// <summary>
	/// Interaction logic for ResultatenOefening.xaml
	/// </summary>
	public partial class ResultatenOefening : UserControl
	{
		public ResultatenOefening()
		{
			InitializeComponent();
            _InitializeLabels();
		}

        
        
        //sets all labels
        private void _InitializeLabels()
        {
            //calculate the amount of characters typed per second
            double wps = (double)Exercise._statisticsController.NumberCorrect / (double)Exercise._statisticsController.CurrentTime.Second;
            wps = Math.Round(wps, 1);
            //calculate the percentage of correct typed characters
            double correctPercentage = ((double)Exercise._statisticsController.NumberCorrect / ((double)Exercise._statisticsController.NumberCorrect + (double)Exercise._statisticsController.NumberOfMistakes)) * 100;
            correctPercentage = Math.Round(correctPercentage, 1);


            //sets the labels
            this.Totaltime.Content = Exercise._statisticsController.CurrentTime.ToString("mm:ss");
            this.MistakeCount.Content = Exercise._statisticsController.NumberOfMistakes;
            this.WPS.Content = wps;
            this.CorrectCount.Content = Exercise._statisticsController.NumberCorrect;
            this.CorrectPercentage.Content = $"{correctPercentage}%";
            _InitializeFeedback(correctPercentage);
        }

        //logic for feedback label
        private void _InitializeFeedback(double percentage)
        {
            if(percentage >= 80)
            {
                this.Feedback.Content = "Heel goed gedaan!";
            } else if(percentage >= 55)
            {
                this.Feedback.Content = "Goed gedaan, maar het kan beter!";
            } else if(percentage >= 40)
            {
                this.Feedback.Content = "Helaas, je hebt nog geen voldoende. Maar je komt dichtbij";
            } else
            {
                this.Feedback.Content = "Helaas, je hebt nog geen voldoende";
            }


        }




        
        
        private void OnBack(object sender, RoutedEventArgs e)
        {
            //_cleanup();
            UserControlController.MainWindowChange(this, new ExerciseSelect());
        }
    }
}
