using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker_Modules
{
    public class UserService
    {
        private Dictionary<string, string> users;

        public UserService()
        {
            // Initialize the user dictionary
            users = new Dictionary<string, string>();
        }

        public bool Register(string username, string password)
        {
            if (users.ContainsKey(username))
            {
                // User already exists
                return false;
            }

            users.Add(username, password);
            return true;
        }

        public bool Authenticate(string username, string password)
        {
            if (users.ContainsKey(username))
            {
                string storedPassword = users[username];
                return password == storedPassword;
            }

            return false;
        }
    }
}
