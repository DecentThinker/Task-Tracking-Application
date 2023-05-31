using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker_Modules
{
    public class UserProfile
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public UserProfile(string username, string fullName, string email)
        {
            Username = username;
            FullName = fullName;
            Email = email;
        }
    }
    public class UserProfileService
    {
        private Dictionary<string, UserProfile> userProfiles;

        public UserProfileService()
        {
            // Initialize the user profile dictionary
            userProfiles = new Dictionary<string, UserProfile>();
        }

        public void AddProfile(UserProfile userProfile)
        {
            userProfiles[userProfile.Username] = userProfile;
        }

        public bool UpdateProfile(string username, string fullName, string email)
        {
            if (userProfiles.ContainsKey(username))
            {
                UserProfile userProfile = userProfiles[username];
                userProfile.FullName = fullName;
                userProfile.Email = email;
                return true;
            }

            return false;
        }

        public bool DeleteProfile(string username)
        {
            return userProfiles.Remove(username);
        }

        public UserProfile GetProfile(string username)
        {
            if (userProfiles.ContainsKey(username))
            {
                return userProfiles[username];
            }

            return null;
        }
    }
}
