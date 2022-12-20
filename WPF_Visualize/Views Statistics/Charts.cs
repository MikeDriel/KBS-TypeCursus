﻿using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace WPF_Visualize.Views_Statistics;

public class Charts
{
    public Charts()
    {
        Data = new List<DataPoint>();
        Data.Add(new DataPoint(0, 0));

        for (var i = 1; i < Exercise.s_StatisticsController.CharactersPerSecond.Count; i++)
        {
            Data.Add(new DataPoint(i, Exercise.s_StatisticsController.CharactersPerSecond[i]));
            Stats = new PlotModel { Title = "Tekens per seconde", TextColor = OxyColors.White, PlotAreaBorderColor = OxyColors.White };
            Stats.Series.Add(new LineSeries { ItemsSource = Data, Title = "Series 1" });
        }

        var timeAxis = new LinearAxis()
        {
            Position = AxisPosition.Bottom,
            Minimum = 0,
            MinimumMajorStep = 1,
            Title = "Seconden",
            MajorGridlineStyle = LineStyle.Solid,
            TextColor = OxyColors.White,
            MajorGridlineColor = OxyColors.White,
            TicklineColor = OxyColors.White,
            MinorTicklineColor = OxyColors.White,
            AxislineColor = OxyColors.White,
            TitleColor = OxyColors.White,
        };


        var KeyPerSecondAxis = new LinearAxis()
        {
            Position = AxisPosition.Left,
            Minimum = 0,
            MinimumMajorStep = 0.5,
            Title = "Tekens",
            MajorGridlineStyle = LineStyle.Solid,
            TextColor = OxyColors.White,
            MajorGridlineColor = OxyColors.White,
            TicklineColor = OxyColors.White,
            MinorTicklineColor = OxyColors.White,
            AxislineColor = OxyColors.White,
            TitleColor = OxyColors.White,
        };

        Stats.Axes.Add(timeAxis);
        Stats.Axes.Add(KeyPerSecondAxis);
    }

    public IList<DataPoint> Data { get; }
    public PlotModel Stats { get; }
}