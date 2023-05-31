using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker_Modules;

namespace TaskTracker_Tests.Module_Tests
{
    public class UserServiceTests
    {
        private UserService userService;

        [SetUp]
        public void SetUp()
        {
            // Initialize the UserService before each test
            userService = new UserService();
        }

        [Test]
        public void Register_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            string username = "user1";
            string password = "password1";

            // Act
            bool isRegistered = userService.Register(username, password);

            // Assert
            Assert.IsTrue(isRegistered);
        }

        [Test]
        public void Register_ExistingUser_ReturnsFalse()
        {
            // Arrange
            string username = "user1";
            string password = "password1";

            // Register the user initially
            userService.Register(username, password);

            // Act
            bool isRegistered = userService.Register(username, "newPassword");

            // Assert
            Assert.IsFalse(isRegistered);
        }

        [Test]
        public void Authenticate_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            string username = "user1";
            string password = "password1";

            // Register and authenticate the user initially
            userService.Register(username, password);

            // Act
            bool isAuthenticated = userService.Authenticate(username, password);

            // Assert
            Assert.IsTrue(isAuthenticated);
        }

        [Test]
        public void Authenticate_InvalidCredentials_ReturnsFalse()
        {
            // Arrange
            string username = "user1";
            string password = "password1";

            // Register and authenticate the user initially
            userService.Register(username, password);

            // Act
            bool isAuthenticated = userService.Authenticate(username, "incorrectPassword");

            // Assert
            Assert.IsFalse(isAuthenticated);
        }
    }
}
