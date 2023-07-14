
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using Task_Tracker;

namespace Task_Tracker
{
    /// <summary>
    /// Interaction logic for RegisterUser.xaml
    /// </summary>
    public partial class RegisterUser : UserControl
    {
        MainWindow window;
        UserControl userControl;
        SqlConnection conn;
        string type_name = "Employee";
        string Names = "";
        string Userid = "";
        string Email = "";
        string Password = "";
        string ConfirmPassword = "";
        string otp="";
        public RegisterUser()
        {
            InitializeComponent();
            conn = new SqlConnection("Data Source=localhost;Initial Catalog=TaskTracking;Integrated Security=True");
            conn.Open();
        }

        public RegisterUser(MainWindow window):this()
        {
            this.window = window;
            cancel.Visibility=Visibility.Collapsed;
        }
        public RegisterUser(MainWindow window, User_Accounts userControl):this()
        {
            this.userControl = userControl;
            this.window = window;
            logo.Visibility= Visibility.Collapsed;
            copyright.Visibility= Visibility.Collapsed;
            back.Visibility= Visibility.Collapsed;
            Verify.Visibility= Visibility.Collapsed;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int x = type.SelectedIndex;
            if(x==0)
            {
                type_name = "Project Manager";
            }
        }
        private void Button_Click_Register(object sender, RoutedEventArgs e)
        {
            Names = name.Text;
            Userid = userid.Text;
            Email = email.Text;
            Password = password.Text;
            ConfirmPassword = confirmPassword.Text;

            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("Connection Error !");
            }
            else
            {
                if (Names.Equals("") || Userid.Equals("") || Email.Equals("") || Password.Equals("") || ConfirmPassword.Equals(""))
                {
                    error.Content = " * Fill All Details ";
                    return;
                }
                _ = RegisterAsync();
            }
        }
        //Validate if all fields are correctly filled.
        public bool PasswordValidation()
        {
            error.Content = string.Empty;
            if (Password.Length >= 8)
            {
                bool containsSpecialChar = false;
                bool containsNumber = false;
                bool upperCase = false;

                // Check if the string contains a special character
                if (Regex.IsMatch(Password, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"))
                    containsSpecialChar = true;

                // Check if the string contains a number
                if (Regex.IsMatch(Password, @"\d"))
                    containsNumber = true;
                // Cherck if it has UpperCase Character 
                foreach (char c in Password)
                {
                    if (Char.IsUpper(c))
                    {
                        upperCase = true;
                        break;
                    }
                }
                //Check if the Password and Confirm password matches 
                if (!Password.Equals(ConfirmPassword))
                {
                    error.Content = "Passwords Do Not Match !";
                    password.Clear();
                    confirmPassword.Clear();
                    return false;
                }

                //If they all match return true
                if (containsSpecialChar && containsNumber && upperCase)
                {
                    error.Content = String.Empty;
                    return true;
                }
                else // if they dont return false 
                {
                    error.Content = "Password must contain a Digit,\nSpecial and Upper Case Character !";
                    return false;
                }
            }
            else //if password length is less than 8
            {
                error.Content = "Password must contain \n at least 8 Characters";
                return false;
            }
        }
        //Verifies if a username is already taken or not 
        private bool UserIdVerification()
        {
            string query = "select count(*) from users where userid=@userid";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("userid", Userid);
            int count = (int)command.ExecuteScalar();
            if (count == 1)
            {
                error.Content = "Username Already Exists\n Use Another !";
                userid.Clear();
                return false;
            }
            return true;
        }
        //Verifies if a Email is already taken or not 
        private bool EmailVerification()
        {
            string query = "select count(*) from users where email=@email";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("email", Email);
            int count = (int)command.ExecuteScalar();
            if (count == 1)
            {
                error.Content = "Email Already Exists\n Use Another !";
                email.Clear();
            }
            else
            {
                if (OTP.Text == otp)
                {
                    return true;
                }
                else
                {
                    error.Content = "Check Email or OTP";
                }
            }
            return false;
        }
        //Registers a a user if all the fields are correctly filled.
        private async Task RegisterAsync()
        {
            if (UserIdVerification() == false || EmailVerification() == false || PasswordValidation() == false)
            {
                return;
            }

            string query = "insert into users values (@v1,@v2,@v3,@v4,@v5)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("v1", Names);
            cmd.Parameters.AddWithValue("v2", Userid);
            cmd.Parameters.AddWithValue("v3", Email);
            cmd.Parameters.AddWithValue("v4", Password);
            cmd.Parameters.AddWithValue("v5", type_name);
            cmd.CommandType = CommandType.Text;

            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                error.Foreground = Brushes.Green;
                error.Content = "Successfully Registered";
                await Task.Delay(1000);
            }
            window.open(new RegisterUser(window));
        }
        private void Button_Click_Login(object sender, RoutedEventArgs e)
        {
            window.open(new LoginPage(window));
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            window.open(new User_Accounts(window));
        }
        private string create_OTP()
        {
            string characters = "0123456789";

            // Generate a random OTP of the specified length
            Random random = new Random();
            string otp = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                otp += characters[random.Next(characters.Length)];
            }
            return otp;
        }

        private void Button_Click_Verify(object sender, RoutedEventArgs e)
        {
            if (email.Text == "")
                return;
            otp = create_OTP();
            GmailVerification gverify = new GmailVerification();
            bool res = gverify.OTPMessage(otp,email.Text);
            otpLabel.Visibility = Visibility.Visible;
            OTP.Visibility = Visibility.Visible;
            error.Foreground = Brushes.Green;
            error.Content = "OTP Sent";
        }
    }
}