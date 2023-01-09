﻿using Model;
namespace Controller;

public class LoginController
{
    private readonly Database _db;

    public LoginController(bool isTeacher)
    {
        _db = new Database();
        IsTeacher = isTeacher;
        S_IsTeacher = isTeacher;
    }

    public bool IsTeacher { get; set; }
    public static bool S_IsTeacher { get; set; }
    public static int? s_UserId { get; set; }

    public event EventHandler<LoginEventArgs>? LoginEvent;


    //CHECK IF THE GIVEN LOGIN INFORMATION IS RIGHT AND SET THE UserId ACCORDINGLY
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
        throw new Exception("UserId is null");
    }
}

//EVENT FOR EXCERCISE
public class LoginEventArgs : EventArgs
{
    public bool IsLoggedIn;
    public bool IsTeacher;

    public LoginEventArgs(bool isLoggedin, bool isTeacher)
    {
        IsLoggedIn = isLoggedin;
        IsTeacher = isTeacher;
    }
}