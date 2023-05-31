using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker_Modules;

namespace TaskTracker_Tests.Module_Tests
{
    public class LoginServiceTests
    {
        private LoginService loginService;

        [SetUp]
        public void SetUp()
        {
            // Initialize the LoginService before each test
            loginService = new LoginService();
        }

        [Test]
        public void Authenticate_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            string username = "user1";
            string password = "password1";

            // Act
            bool isAuthenticated = loginService.Authenticate(username, password);

            // Assert
            Assert.IsTrue(isAuthenticated);
        }

        [Test]
        public void Authenticate_InvalidCredentials_ReturnsFalse()
        {
            // Arrange
            string username = "user1";
            string password = "incorrectPassword";

            // Act
            bool isAuthenticated = loginService.Authenticate(username, password);

            // Assert
            Assert.IsFalse(isAuthenticated);
        }
    }
}
