using System;
using System.CodeDom;
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
    /// Interaction logic for View_Progress.xaml
    /// </summary>
    public partial class View_Progress : UserControl
    {
        MainWindow window;
        DataRowView project;

        SqlConnection conn;

        public class Progress
        {
            public string name { get; set; }
            public string comment { get; set; }
            public DateTime time { get; set; }
            public string type { get; set; }
            public string task_name { get; set; }

            public Progress(string name,string comment, DateTime time, string type, string task_name)
            {
                this.name = name;
                this.comment = comment;
                this.time = time;
                this.type = type;
                this.task_name = task_name;
            }
        }
        public View_Progress(MainWindow window, DataRowView project)
        {
            InitializeComponent();
            string connstring = "Data Source = localhost; Initial Catalog = TaskTracking; Integrated Security = True";
            conn = new SqlConnection(connstring);
            conn.Open();
            this.window = window;
            this.project = project;
            window.title.Text = (string)project["name"];
            create_list();   
        }

        List<Progress> progressList = new List<Progress>();
        private void create_list()
        {
            // using ticket id of the project.
            int id = (Int32)project["id"];


            string query = "select * from progress where ticket_id=@id";
            SqlCommand cmd = new SqlCommand(query,conn);
            cmd.Parameters.AddWithValue("id", id);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable data = new DataTable();
            adapter.Fill(data);

            string userid = "",comment="",task_name="";
            DateTime time;
            int n = data.Rows.Count;
            for(int i=0;i<n;i++)
            {
                userid = (string)data.Rows[i]["user"];
                comment = (string)data.Rows[i]["comment"];
                time = (DateTime)data.Rows[i]["time"];
                task_name = (string)data.Rows[i]["task_name"];

                query = "select name,type_name from users where userid=@userid";

                cmd=new SqlCommand(query,conn);
                cmd.Parameters.AddWithValue("userid", userid);
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read()) 
                {
                    progressList.Add(new Progress(reader.GetString(0) ,comment,time,reader.GetString(1),task_name));
                }
                reader.Close();
            }
            Progress_list.ItemsSource = progressList;
        }
    }
}
