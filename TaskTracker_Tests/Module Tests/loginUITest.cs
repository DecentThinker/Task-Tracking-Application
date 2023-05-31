using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker_Tests.Module_Tests
{
    [TestFixture]
    public class LoginAutomationTests
    {
        private LoginAutomation loginAutomation;

        [SetUp]
        public void SetUp()
        {
            // Initialize the LoginAutomation instance
            loginAutomation = new LoginAutomation();
        }

        [Test]
        public void TestLogin()
        {
            // Start the application
            loginAutomation.StartApplication();

            // Perform login
            loginAutomation.Login("Aman@11", "Exl@7890");

            // Check if login was successful
            bool isLoginSuccessful = loginAutomation.IsLoginSuccessful();
            Assert.IsTrue(isLoginSuccessful, "Login Successful");

            // Close the application
            loginAutomation.CloseApplication();
        }
    }
    class LoginAutomation
    {
        private Application application;
        private Window mainWindow;

        public void StartApplication()
        {
            // Start the WPF application
            application = Application.Launch("C:\\Users\\Aman\\Desktop\\Task Tracking Application\\Task Tracker\\bin\\Debug\\net6.0-windows\\Task Tracker.exe");
            var automation = new UIA3Automation();
            mainWindow = application.GetMainWindow(automation);
        }

        public void Login(string username, string password)
        {
            var usernameTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("Username")).AsTextBox();
            var passwordTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("passwordBox")).AsTextBox();
            var loginButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("login")).AsButton();

            // Enter username and password
            usernameTextBox.Enter(username);
            passwordTextBox.Enter(password);

            // Click the login button
            loginButton.Click();
        }

        public bool IsLoginSuccessful()
        {
            // Check if login was successful by verifying a specific element after login
            Label successLabel = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("error")).AsLabel();
            return successLabel != null && successLabel.Name == "User Authenticated";
        }

        public void CloseApplication()
        {
            // Close the application
            application.Close();
            application.Dispose();
        }
    }
}
