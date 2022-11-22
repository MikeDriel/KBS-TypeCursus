using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    internal abstract class User
    {
        public string Name { get; set; }
        public string Username { get; set; }
        private string _password;

        public User(string name, string username, string password)
        {
            Name = name;
            Username = username;
            _password = password;
        }

        public bool Login(string password)
        {
            return _password == password;
        }
    }
}
