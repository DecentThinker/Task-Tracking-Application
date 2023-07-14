using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Task_Tracker
{
    /// <summary>
    /// Interaction logic for Edit_Control.xaml
    /// </summary>
    public partial class Edit_Control : UserControl
    {
        SqlConnection conn;
        MainWindow window;
        DataTable profile;
        string userid = "";
        string query = "";
        string otp = "";
        string _Name = "";
        string Email = "";
        string Pass = "";
        bool verified = false;
        public Edit_Control(MainWindow window)
        {
            InitializeComponent();
            conn = new SqlConnection("Data Source=localhost;Initial Catalog=TaskTracking;Integrated Security=True");
            conn.Open();
            this.window = window;
            profile = window.get_profile();
            _Name = (string)profile.Rows[0]["name"];
            Email = (string)profile.Rows[0]["email"];
            name.Text = _Name;
            email.Text = Email;
            window.title.Text = "Edit Details";
            userid = (string)profile.Rows[0]["userid"]; 
        }

        private void Button_Click_Verify(object sender, RoutedEventArgs e)
        {
            if (!email.IsEnabled)
                return;
            GmailVerification gmail = new GmailVerification();
            otp = create_OTP();
            bool valid  = gmail.OTPMessage(otp,email.Text);
            if(valid)
            {
                error.Foreground = Brushes.Green;
                error.Content = "OTP Sent";
                OTP.IsEnabled = true;   
            }
        }
        private void Click_Name(object sender, RoutedEventArgs e)
        {
            name.IsEnabled = true;
            submit1.IsEnabled = true;
        }
        private void Click_Email(object sender, RoutedEventArgs e)
        {
            email.IsEnabled = true;
            Verify.IsEnabled = true;
           
            submit1.IsEnabled = true;
        }
        private void Click_Password(object sender, RoutedEventArgs e)
        {
            old_password.IsEnabled = true;
            new_password.IsEnabled = true;
            confirmPassword.IsEnabled = true;
            submit_pass.IsEnabled = true;
        }
        private void Submit_Details(object sender, RoutedEventArgs e)
        {
            query = "update users set name=@name,email=@email where userid=@userid";
            SqlCommand sqlCommand = new SqlCommand(query, conn);
            if(name.Text != "")
            {
                _Name = name.Text;
            }
            if (otp!="" && OTP.Text == otp)
            {
                error.Foreground = Brushes.Green;
                Email = email.Text;
                error.Content = "E-Mail Verified";
            }
            sqlCommand.Parameters.AddWithValue("name", _Name);
            sqlCommand.Parameters.AddWithValue("email", Email);
            sqlCommand.Parameters.AddWithValue("userid", userid);
            int res = sqlCommand.ExecuteNonQuery();
            if (res > 0)
            {
                profile.Rows[0]["name"] = _Name;
                profile.Rows[0]["email"] = Email;
                window.set_profile(profile);    
                MessageBox.Show("Details Updated");
            }
        }
        private void Submit_Password(object sender, RoutedEventArgs e)
        {
            Pass = (string)profile.Rows[0]["pass"];
            if(old_password.Text == Pass)
            {
                if(PasswordValidation(new_password.Text,confirmPassword.Text))
                {
                    query = "update users set pass=@password where userid=@userid";
                    SqlCommand sqlCommand = new SqlCommand(query, conn);
                    sqlCommand.Parameters.AddWithValue("password", confirmPassword.Text);
                    sqlCommand.Parameters.AddWithValue("userid", userid);
                    int res = sqlCommand.ExecuteNonQuery();
                    if (res > 0)
                    {
                        profile.Rows[0]["pass"] = new_password.Text;
                        window.set_profile(profile);
                        MessageBox.Show("Password Updated");
                    }
                }
                else
                {
                    error2.Content = "Password Not Matched";
                    return;
                }
            }
            else
            {
                error2.Content = "Wrong Old Password";
                return;
            } 
        }
        private bool PasswordValidation(String Password,string ConfirmPassword)
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
                    new_password.Clear();
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
    }
}
