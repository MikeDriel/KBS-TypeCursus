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
            this.Stats = new PlotModel { Title = "Statistics" };
            this.Stats.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1 ,"cos(x)"));
        }

        public PlotModel Stats { get; private set; }

    }
}
