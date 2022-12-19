using System.Diagnostics;
using Model;

namespace Controller;

public class LoginController
{
    private readonly Database _db;

    public LoginController(bool isTeacher)
    {
        _db = new Database();
        IsTeacher = isTeacher;
    }

    public bool IsTeacher { get; set; }
    public static int? s_UserId { get; private set; }

    public event EventHandler<LoginEventArgs>? LoginEvent;


    //CHECK IF THE GIVEN LOGIN INFORMATION IS RIGHT AND SET THE UserId ACCORDINGLY
    public void CheckLogin(string? loginKey, string password)
    {
        if (loginKey == "" || password == "")
        {
            LoginEvent?.Invoke(this, new LoginEventArgs(false, IsTeacher));
            return;
        }

        var correctPasswordWithId = _db.GetPasswordWithId(IsTeacher, loginKey);
        var correctPassword = correctPasswordWithId[0];
        password = _db.HashPassword(password);

        if (correctPassword != null && correctPassword == password)
        {
            LoginEvent?.Invoke(this, new LoginEventArgs(true, IsTeacher));
            s_UserId = int.Parse(correctPasswordWithId[1]);
            _db.CheckIfPupilStatisticsExist(LoginController.GetUserId());
        }
        else
        {
            LoginEvent?.Invoke(this, new LoginEventArgs(false, IsTeacher));
        }
    }

    public static void LogOut()
    {
        s_UserId = null;
    }

    public static int GetUserId()
    {
        if (s_UserId != null)
        {
            return (int)s_UserId;
        }
        else
        {
            throw new Exception("UserId is null");
        }
    }
}

//EVENT FOR EXCERCISE
public class LoginEventArgs : EventArgs
{
    public bool IsLoggedin;
    public bool IsTeacher;

    public LoginEventArgs(bool isLoggedin, bool isTeacher)
    {
        IsLoggedin = isLoggedin;
        IsTeacher = isTeacher;
    }
}