using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using OxyPlot;
using OxyPlot.Series;

namespace WPF_Visualize.Views_Statistics
{
    public class Charts
    {
        public Charts()
        {
            Data = new List<DataPoint>
            {
                new DataPoint(0, 4),
                new DataPoint(1, 6),
                new DataPoint(2, 2),
                new DataPoint(3, 0),
                new DataPoint(4, 7)
            };


            this.Stats = new PlotModel { Title = "Statistics" };
            this.Stats.Series.Add(new LineSeries { ItemsSource = Data, Title = "Series 1" });

        }

        public IList<DataPoint> Data { get; private set; }
        public PlotModel Stats { get; private set; }
    


   

        /*
        public Charts()
        {
            
            this.Stats = new PlotModel { Title = "Statistics" };
            this.Stats.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
            
        }

        public PlotModel Stats { get; private set; }
        

    */
    }
}
