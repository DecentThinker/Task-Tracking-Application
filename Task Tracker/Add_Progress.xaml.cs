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
    /// Interaction logic for Add_Progress.xaml
    /// </summary>
    public partial class Add_Progress : Window
    {
        DataRowView task;
        DataTable table;
        MainWindow window;
        SqlConnection conn;

        string user = "";
        string comments = "";
        DateTime time;
        string task_name="";
        int id;
        public Add_Progress(MainWindow window ,DataRowView task)
        {
            InitializeComponent();
            conn = new SqlConnection("Data Source=localhost;Initial Catalog=TaskTracking;Integrated Security=True");
            conn.Open();
            this.task = task;
            this.window = window;
            table = window.get_profile();
        }
        private void Submit_View(object sender, RoutedEventArgs e)
        {
            create_Progress();
            if(comments=="")
            {
                Result.Text = "* Write a comment";
            }
            else
            {    
                if(insert_Progress()>0)
                {
                    Result.Foreground = Brushes.Green;
                    Result.Text = "* Progress Added";
                }
                else
                {
                    Result.Text = "* Error";
                }
            }
        }
        private void create_Progress()
        {
            id = (Int32)task["parent_id"];
            user = (string)table.Rows[0]["userid"];
            comments = comment.Text;
            time = DateTime.Now;
            task_name = (string)task["name"];
        }
        private int insert_Progress()
        {
            string query = "insert into progress values(@comment,@time,@user,@ticket_id,@task_name)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("ticket_id", id);
            cmd.Parameters.AddWithValue ("user", user);
            cmd.Parameters.AddWithValue("time", time);
            cmd.Parameters.AddWithValue("comment",comments);
            cmd.Parameters.AddWithValue("task_name", task_name);
            return cmd.ExecuteNonQuery();
        }

        private void Close_View(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }    
    }
}
