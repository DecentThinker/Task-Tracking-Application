using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Task_Tracker
{
    /// <summary>
    /// Interaction logic for View_Profile.xaml
    /// </summary>
    public partial class View_Profile : Window
    {
        DataTable profile;
        public View_Profile(DataTable profile)
        {
            InitializeComponent();
            this.profile = profile;
            set_profile();
        }

        private void set_profile()
        {
            username.Text = profile.Rows[0]["userid"].ToString();
            name.Text = profile.Rows[0]["name"].ToString();
            email.Text = profile.Rows[0]["email"].ToString();
            type.Text = profile.Rows[0]["type_name"].ToString();
        }

        private void Close_View(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
