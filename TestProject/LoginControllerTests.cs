using Controller;
using Model;
namespace TestProject;

public class LoginControllerTests
{
    private LoginController Controller;


    [TestCase("nok", "NOK", false, 2)]
    [TestCase("nok", "afsjhgv", false, null)]
    [TestCase("sok", "NOK", false, null)]
    [TestCase("Luc", "Luccie", false, 6)]
    [TestCase("Mike", "mike", false, null)]
    [TestCase("Mike", "Mike", false, 3)]
    public void CheckLogin(string loginKey, string Password, bool isTeacher, int? expectedUserId)
    {
        Controller = new LoginController(isTeacher);
        Controller.CheckLogin(loginKey, Password);
        if (LoginController.s_UserId == expectedUserId)
        {
            Assert.Pass();
        }
        else
        {
            Assert.Fail();
        }
    }

}
