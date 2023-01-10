using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;
namespace WPF_Visualize.Views_Statistics;

public class Charts
{
    //Creates a List with Datapoints, each datapoint had a x and y value, the x value represents time and the y value the amount of correct answers
    public IList<DataPoint> Data { get; set; }
    public PlotModel Stats { get; set; } //creates the PlotModel/Chart
    public Charts()
    {
        
        Data = new List<DataPoint>(); //initialize the Data List
        
        Data.Add(new DataPoint(0, 0)); //Creates a Datapoint at 0, 0 

        //loop that loops trough the CharactersPerSecond Dictionary, the Dictionary contains the amount of correct answers for eachsecond
        //every key/value pair gets added to the Data List, the key is the x value (time) and the value is the y value (correct answers)
        for (int i = 1; i < ResultsExercise.s_ExerciseStatisticsController.CharactersPerSecond.Count; i++)
        {
            Data.Add(new DataPoint(i, ResultsExercise.s_ExerciseStatisticsController.CharactersPerSecond[i]));
            Data.Add(new DataPoint(i, ResultsExercise.s_ExerciseStatisticsController.CharactersPerSecond[i]));
            Stats = new PlotModel { Title = "Tekens per seconde", TextColor = OxyColors.White, PlotAreaBorderColor = OxyColors.White };
            Stats.Series.Add(new LineSeries { ItemsSource = Data, Title = "Series 1" });
        }

        //Initialize the X axis
        LinearAxis timeAxis = new LinearAxis
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
            TitleColor = OxyColors.White
        };

        //Initialize the Y axis
        LinearAxis KeyPerSecondAxis = new LinearAxis
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
            TitleColor = OxyColors.White
        };

        //adds the axises to the plot
        Stats.Axes.Add(timeAxis);
        Stats.Axes.Add(KeyPerSecondAxis);
    }


}