using Model;

namespace Controller
{
    public class LoginController
    {
        public bool IsTeacher { get; set; }
        public event EventHandler<LoginEventArgs> LoginEvent;
        private Database _db = new Database();
        public static int UserId { get; private set; }

        public LoginController(bool isTeacher)
        {
            this.IsTeacher = isTeacher;
        }
        

        //CHECK IF THE GIVEN LOGIN INFORMATION IS RIGHT AND SET THE UserId ACCORDINGLY
        public void CheckLogin(string? loginKey, string password)
        {
            if (loginKey == "" || password == "")
            {
                LoginEvent?.Invoke(this, new LoginEventArgs(false, IsTeacher));
                return;
            }
            string? correctPasswordWithId = _db.GetPasswordWithId(IsTeacher, loginKey);
            string[] array = correctPasswordWithId.Split(',');
            string? correctPassword = array[0];
            password = _db.HashPassword(password);
            
            if (correctPassword != null && correctPassword == password)
            {
                LoginEvent?.Invoke(this, new LoginEventArgs(true, IsTeacher));
                UserId = int.Parse(array[1]);
            }
            else
            {
                LoginEvent?.Invoke(this, new LoginEventArgs(false, IsTeacher));
            }
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