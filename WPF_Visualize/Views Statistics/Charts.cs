using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using OxyPlot;
using OxyPlot.Series;
using Controller;
using System.Security.Cryptography.Xml;

namespace WPF_Visualize.Views_Statistics
{
    public class Charts
    {
        public Charts()
        {
            Data = new List<DataPoint>();
            Data.Add(new DataPoint(0, 0));

            for (int i = 1; i < Exercise._statisticsController.CharactersPerSecond.Count; i++)
            {
                Data.Add(new DataPoint(i, Exercise._statisticsController.CharactersPerSecond[i]));
                this.Stats = new PlotModel { Title = "Tekens per seconde" };
                this.Stats.Series.Add(new LineSeries { ItemsSource = Data, Title = "Series 1" });

            }
        }

        public IList<DataPoint> Data { get; private set; }
        public PlotModel Stats { get; private set; }



    } 
}
