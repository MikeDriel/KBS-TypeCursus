using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class LoginController
    {
        public bool IsTeacher { get; set; }
        public event EventHandler<LoginEventArgs> LoginEvent;

        public LoginController(bool IsTeacher)
        {
            this.IsTeacher = IsTeacher;
        }


        private string GetPassword(string LoginKey)
        {
            string password = null;
            return password;
        }

        public void CheckLogin(string LoginKey, string password)
        {
            string CorrectPassword = GetPassword(LoginKey);
            
            if (CorrectPassword != null)
            {
                if (CorrectPassword == password)
                {
                    LoginEvent?.Invoke(this, new LoginEventArgs(true, IsTeacher));
                    return;
                }
            }
            LoginEvent?.Invoke(this, new LoginEventArgs(false, IsTeacher));
        }
        
        
    }

    //EVENT FOR EXCERCISE
    public class LoginEventArgs : EventArgs
    {
        public bool IsLoggedin;
        public bool IsTeacher;

        public LoginEventArgs(bool IsLoggedin, bool IsTeacher)
        {
            this.IsLoggedin = IsLoggedin;
            this.IsTeacher = IsTeacher;
        }
    }
}