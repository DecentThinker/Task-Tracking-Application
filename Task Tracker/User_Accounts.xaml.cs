using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Task_Tracker
{
    /// <summary>
    /// Interaction logic for User_Accounts.xaml
    /// </summary>
    public partial class User_Accounts : UserControl
    {
        SqlConnection connection;
        MainWindow window;
        public User_Accounts(MainWindow window)
        {
            InitializeComponent();
            connection = new SqlConnection("Data Source = localhost; Initial Catalog = TaskTracking; Integrated Security = True");
            connection.Open();
            Create_Table();
            this.window = window;
            window.title.Text = "Users";
        }
        private void Create_Table()
        {
            string query = "select * from users";
            SqlCommand cmd = new SqlCommand(query, connection);
            DataTable dataTable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTable);
            adapter.Dispose();

            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void Add_User(object sender, RoutedEventArgs e)
        {
            window.title.Text = "Add User";
            window.open(new RegisterUser(window,this));
        }

        private void Delete_User(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dataGrid.SelectedItem;
            if (row!=null)
            {
                string query = "delete from users where userid=@userid";
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                sqlCommand.Parameters.AddWithValue("userid", (string)row["userid"]);
                sqlCommand.ExecuteNonQuery();
                Create_Table();
            }
            
        }
    }
}
