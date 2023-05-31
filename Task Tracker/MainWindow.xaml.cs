using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Policy;
using System.Text;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Task_Tracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string userid;
        DataTable profile;
        public MainWindow()
        {
            InitializeComponent();
            open(new LoginPage(this));
        }
        public void set_profile(DataTable new_profile)
        {
            profile=new_profile;
            string role = (string)profile.Rows[0]["type_name"];
            userid = (string)profile.Rows[0]["userid"];
            if (role=="Admin")
            {
                All.IsEnabled = true;
            }
        }
        public DataTable get_profile()
        {
            return (DataTable)profile;  
        }
        public  void open(UserControl userControl)
        {
            content.Content = userControl;
        }

        private void Click_Home(object sender, RoutedEventArgs e)
        {
            open(new LandingPage(this));

        }
        private void Click_Logout(object sender, RoutedEventArgs e)
        {
            open(new LoginPage(this));
        }

        private void View_Account(object sender, RoutedEventArgs e)
        {
            View_Profile user = new View_Profile(profile);
            user.ShowDialog();
            if (DialogResult == true)
            {
                user.Close();
            }
        }
        private void Email_Edit(object sender, RoutedEventArgs e)
        {
            Edit_Window email = new Edit_Window(this,"Enter E-Email", userid,"E-Mail");
            email.ShowDialog();
            if(DialogResult == true) { email.Close(); }
            
        }
        private void Name_Edit(object sender, RoutedEventArgs e)
        {
            Edit_Window name = new Edit_Window(this,"Enter Name", userid, "Name");
            name.ShowDialog();
            if (DialogResult == true) { name.Close(); }
        }
        private void Password_Change(object sender, RoutedEventArgs e)
        {
            Edit_Window password = new Edit_Window(this,"Enter New Password", userid, "Password");
            password.ShowDialog();
            if (DialogResult == true) { password.Close(); }

        }

        private void All_Account(object sender, RoutedEventArgs e)
        {
            open(new User_Accounts(this));
        }
    }
}
