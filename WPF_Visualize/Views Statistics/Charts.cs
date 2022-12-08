using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Series;

namespace WPF_Visualize.Views_Statistics;

public class Charts
{
    public Charts()
    {
        Data = new List<DataPoint>();
        Data.Add(new DataPoint(0, 0));

        for (var i = 1; i < Exercise.StatisticsController.CharactersPerSecond.Count; i++)
        {
            Data.Add(new DataPoint(i, Exercise.StatisticsController.CharactersPerSecond[i]));
            Stats = new PlotModel { Title = "Tekens per seconde" };
            Stats.Series.Add(new LineSeries { ItemsSource = Data, Title = "Series 1" });
        }
    }

    public IList<DataPoint> Data { get; }
    public PlotModel Stats { get; }
}