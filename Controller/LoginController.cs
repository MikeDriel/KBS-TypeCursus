
using Model;

namespace Controller
{
    public class LoginController
    {
        public bool IsTeacher { get; set; }
        public event EventHandler<LoginEventArgs> LoginEvent;
        private Database _db = new Database();

        public LoginController(bool isTeacher)
        {
            this.IsTeacher = isTeacher;
        }


        private string? GetPassword(string loginKey)
        {
            return _db.GetPassword(IsTeacher, loginKey);
        }

        public void CheckLogin(string? loginKey, string password)
        {
            string? correctPassword = GetPassword(loginKey);
            
            if (correctPassword != null && loginKey != null && correctPassword == password)
            {
                LoginEvent?.Invoke(this, new LoginEventArgs(true, IsTeacher));
                return;
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