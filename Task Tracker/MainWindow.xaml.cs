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
        public string role;
        public MainWindow()
        {
            InitializeComponent();
            open(new LoginPage(this));
        }
        public void set_profile(DataTable new_profile)
        {
            profile=new_profile;
            role = (string)profile.Rows[0]["type_name"];
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
        private void Edit_Account(object sender, RoutedEventArgs e)
        {
            Edit_Control editor = new Edit_Control(this);
            open(editor);
            
        }

        private void All_Account(object sender, RoutedEventArgs e)
        {
            open(new User_Accounts(this));
        }
    }
}
