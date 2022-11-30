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
        private Dictionary<string, string> _loginInformation = new Dictionary<string, string>();
        public event EventHandler<LoginEventArgs> LoginEvent;

        public LoginController(bool IsTeacher)
        {
            this.IsTeacher = IsTeacher;
            SetLoginInformation();
        }


        private void SetLoginInformation()
        {
            if (IsTeacher)
            {
                _loginInformation.Add("teacher@gmail.com", "teacher");
                _loginInformation.Add("teacher2@gmail.com", "teacher2");
                _loginInformation.Add("teacher3@gmail.com", "teacher3");
            }
            else
            {
                _loginInformation.Add("student", "student");
                _loginInformation.Add("student2", "student2");
                _loginInformation.Add("student3", "student3");
            }
        }

        public void CheckLogin(string LoginKey, string password)
        {
            
            if (_loginInformation.ContainsKey(LoginKey))
            {
                if (_loginInformation[LoginKey] == password)
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
