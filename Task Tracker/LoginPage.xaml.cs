using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Markup;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading.Tasks;

namespace Task_Tracker
{
    public partial class LoginPage : UserControl
    {
        SqlConnection sqlConnection;
        private MainWindow window;
        string username = "", password = "";
        public LoginPage(MainWindow mainWindow)
        {
            InitializeComponent();
            window= mainWindow;
            sqlConnection = new SqlConnection("Data Source = localhost; Initial Catalog = TaskTracking; Integrated Security = True");
            sqlConnection.Open();
            window.panelHeader.Visibility = Visibility.Collapsed;
        }
        private async void Button_Click_Login(object sender, RoutedEventArgs e)
        {
            username = "";
            password = "";
            if (Username.Text == "" || passwordBox.Password == "")
            {
                error.Content = "* Fill All Details";
            }
            else
            {
                username = Username.Text;
                password = passwordBox.Password;
                await AuthenticateAsync();
            }
            sqlConnection.Close();
        }

        private async Task AuthenticateAsync()
        {
            string query = "select * from users where userid=@username and pass=@password";
            SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
            adapter.SelectCommand.Parameters.AddWithValue("username", username);
            adapter.SelectCommand.Parameters.AddWithValue("password", password);

            DataTable dt = new DataTable();
            adapter.Fill(dt);
            int count = dt.Rows.Count;

            if (count == 1)
            {
                error.Foreground = Brushes.Green;
                error.Content = "User Authenticated";
                await Task.Delay(2000);
                window.open(new LandingPage(window));
                window.set_profile(dt);
            }
            else
            {
                error.Content = "* Wrong User Id \n and Password";
            }
            sqlConnection.Close();
            Username.Clear();
            passwordBox.Clear();
        }
        private void Button_Click_Register(object sender, RoutedEventArgs e)
        {
            window.open(new RegisterUser(window));
            
        }
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            new PasswordReset("Enter E-Mail").Show();
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            passwordBox.Visibility = Visibility.Collapsed;
            textBox.Visibility = Visibility.Visible;
            textBox.Text = passwordBox.Password;
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            textBox.Visibility = Visibility.Collapsed;
            passwordBox.Visibility = Visibility.Visible;
        }
    }
}
