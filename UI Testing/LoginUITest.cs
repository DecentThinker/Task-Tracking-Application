using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using static System.Net.Mime.MediaTypeNames;
using Application = FlaUI.Core.Application;

namespace TaskTracker_Tests.Module_Tests
{
    public class LoginAutomation
    {
        private Application application;
        private Window mainWindow;

        public void StartApplication()
        {
            // Start the WPF application
            application = Application.Launch("C:\\Users\\Aman\\Desktop\\Task Tracker\\TaskTrackingApp\\bin\\Debug\\net6.0-windows\\TaskTrackingApp.exe");
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
            var successLabel = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("error")).AsLabel();
            return successLabel != null && successLabel.Name == "User Authenticated";
        }

        public void CloseApplication()
        {
            // Close the application
            application.Close();
            application.Dispose();
        }
        public static void run()
        {
            var loginAutomation = new LoginAutomation();

            // Start the application
            loginAutomation.StartApplication();

            // Perform login
            loginAutomation.Login("Aman@11", "Exl@7890");

            // Check if login was successful
            bool isLoginSuccessful = loginAutomation.IsLoginSuccessful();

            // Close the application
            loginAutomation.CloseApplication();
        }
    }

}
