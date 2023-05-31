using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker_Modules
{
    public class LoginService
    {
        private Dictionary<string, string> users;

        public LoginService()
        {
            // Initialize the user credentials
            users = new Dictionary<string, string>
            {
                { "user1", "password1" },
                { "user2", "password2" },
                { "user3", "password3" }
            };
        }

        public bool Authenticate(string username, string password)
        {
            // Check if the provided username and password match the stored credentials
            if (users.ContainsKey(username))
            {
                string storedPassword = users[username];
                return password == storedPassword;
            }

            return false;
        }
    }
}
