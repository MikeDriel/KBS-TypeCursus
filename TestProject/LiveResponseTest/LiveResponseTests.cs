using Controller;
namespace TestProject.LiveResponseTest;

[TestFixture]
public class LiveResponseTests
{


    [SetUp]
    public void Setup()
    {
        _statisticsController = new ExerciseStatisticsController(5);
    }
    private ExerciseStatisticsController _statisticsController;

    [Test]
    //test for live response, testing if the if statements of the GetStatistics method works as expected
    public void GetStatistics_StandardVariables_ResultsEqual()
    {
        string result = _statisticsController.GetStatistics();
        string expectedResult = $" 0 fout \r\n 0% goed \r\n {_statisticsController.CurrentTime.ToString("mm:ss")}";
        Assert.AreEqual(result, expectedResult);
    }
}