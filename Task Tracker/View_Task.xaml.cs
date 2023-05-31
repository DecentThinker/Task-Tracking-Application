using System;
using System.Collections.Generic;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Task_Tracker
{
    /// <summary>
    /// Interaction logic for View_Task.xaml
    /// </summary>
    public partial class View_Task : Window
    {
        DataRowView task;
        SqlConnection conn;
        public View_Task(DataRowView row)
        {
            conn=new SqlConnection("Data Source=localhost;Initial Catalog=TaskTracking;Integrated Security=True");
            conn.Open();
            InitializeComponent();
            task=row;
            fill_details();
            DataContext = this;    
        }
        public string file { get; set; }
        private void fill_details()
        {
            int id = (Int32)task["id"];

            //Display basic details

            name.Text = task["name"].ToString();
            description.Text = task["descriptions"].ToString();
            status.Text = task["status"].ToString();
            comment.Text = task["comment"].ToString();

            //Display filename attached to task.

            string query = "select filename from attachments where ticket_id=@id";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("id", id);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read()) { file = reader.GetString(0); }
        }
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            //saves file to desired directory
            
        }
        private void Close_View(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
