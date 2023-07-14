using System;
using System.Data.SqlClient;
using System.Windows;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Task_Tracker
{
    /// <summary>
    /// Interaction logic for PasswordReset.xaml
    /// </summary>
    public partial class PasswordReset : Window
    {
        SqlConnection sqlConnection;
        string otp = "";
        public PasswordReset(string question, string defaultAnswer = "")
        {
            InitializeComponent();
            label.Content = question;
            textBox.Text = defaultAnswer;
            sqlConnection = new SqlConnection("Data Source=localhost;Initial Catalog=TaskTracking;Integrated Security=True");
            sqlConnection.Open();
        }
        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (Answer.Equals(""))
            {
                MessageBox.Show("Enter the User Id");
            }
            else if(textBox2.Text == otp)
            {
                SqlCommand cmd = new SqlCommand("select pass from users where email=@email", sqlConnection);
                cmd.Parameters.AddWithValue("email", Answer);

                SqlDataReader reader = cmd.ExecuteReader();
                string password = "";
                while (reader.Read())
                {
                    password = reader.GetString(0);
                }
                reader.Close();
                if (password.Equals(""))
                {
                    MessageBox.Show("Invalid E-Mail");
                    textBox.Clear();
                    return;
                }
                passwordLabel.Content = password;
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            textBox.SelectAll();
            textBox.Focus();
        }

        public string Answer
        {
            get { return textBox.Text; }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
        private void Verify_Click(object sender, RoutedEventArgs e)
        {
            if (Answer == "")
                return;
            otp = create_OTP();
            GmailVerification gverify = new GmailVerification();
            bool res = gverify.OTPMessage(otp,Answer);
            if(res)
            {
                passwordLabel.Content = "OTP Sent";
            }
        }
    }
}
