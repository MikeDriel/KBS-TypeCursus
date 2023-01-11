using Model;
namespace Controller;

public class LoginController
{
    private readonly Database _db;

    public LoginController(bool isTeacher)
    {
        _db = new Database();
        IsTeacher = isTeacher;
        s_IsTeacher = isTeacher;
    }

    // boolean to check if the user is a teacher or a student
    public bool IsTeacher { get; set; }
    public static bool s_IsTeacher { get; set; }
    // static int to store the id of the user
    public static int? s_UserId { get; set; }

    public event EventHandler<LoginEventArgs>? LoginEvent;
    
    /// <summary>
    /// Check if the given login information is right and set the UserId accordingly
    /// </summary>
    /// <param name="loginKey"></param>
    /// <param name="password"></param>
    public void CheckLogin(string? loginKey, string password)
    {
        if (loginKey == "" || password == "")
        {
            LoginEvent?.Invoke(this, new LoginEventArgs(false, IsTeacher));
            return;
        }

        string[]? correctPasswordWithId = _db.GetPasswordWithId(IsTeacher, loginKey);
        string? correctPassword = correctPasswordWithId[0];
        password = _db.HashPassword(password);

        if (correctPassword != null && correctPassword == password)
        {
            s_UserId = int.Parse(correctPasswordWithId[1]);
            LoginEvent?.Invoke(this, new LoginEventArgs(true, IsTeacher));
            s_UserId = int.Parse(correctPasswordWithId[1]);
            if (!IsTeacher)
            {
                _db.CheckIfPupilStatisticsExist(LoginController.GetUserId());
            }
        }
        else
        {
            s_UserId = null;
            LoginEvent?.Invoke(this, new LoginEventArgs(false, IsTeacher));
        }
    }

    /// <summary>
    /// Method for the the user logs out to set the userId to null
    /// </summary>
    public static void LogOut()
    {
        s_UserId = null;
    }

    /// <summary>
    /// Method to het the userId 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static int GetUserId()
    {
        if (s_UserId != null)
        {
            return (int)s_UserId;
        }
        throw new Exception("UserId is null");
    }
}

//EVENT FOR EXCERCISE
public class LoginEventArgs : EventArgs
{
    public bool IsLoggedIn { get; set; }
    public bool IsTeacher { get; set; }

    public LoginEventArgs(bool isLoggedin, bool isTeacher)
    {
        IsLoggedIn = isLoggedin;
        IsTeacher = isTeacher;
    }
}