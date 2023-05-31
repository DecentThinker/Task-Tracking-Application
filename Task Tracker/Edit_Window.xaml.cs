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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Task_Tracker
{
    /// <summary>
    /// Interaction logic for Edit_Window.xaml
    /// </summary>
    public partial class Edit_Window : Window
    {
        string userid;
        string type;
        MainWindow window;
        SqlConnection sqlConnection;
        public Edit_Window(MainWindow window ,string question, string userid,string type)
        {
            InitializeComponent();
            label.Content = question;
            textBox.Text = "";
            this.userid = userid;
            this.window = window;
            this.type = type;    
            sqlConnection = new SqlConnection("Data Source=localhost;Initial Catalog=TaskTracking;Integrated Security=True");
            sqlConnection.Open();
        }
        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (Answer.Equals(""))
            {
                MessageBox.Show("Enter the "+type);
            }
            else
            {
                string content = textBox.Text;
                string query="";

                //update database
                if (type == "E-Mail")
                    query = "update users set email=@content where userid=@userid";
                else if (type == "Name")
                    query = "update users set name=@content where userid=@userid";
                else if(type=="Password")
                    query = "update users set pass=@content where userid=@userid";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("content", content);
                sqlCommand.Parameters.AddWithValue("userid", userid);
                int res = sqlCommand.ExecuteNonQuery();
                if (res > 0)
                {
                    MessageBox.Show(type+" Updated");
                }
                DialogResult = true;

                //Update profile
                query = "select * from users where userid=@userid";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
                adapter.SelectCommand.Parameters.AddWithValue("userid", userid);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                window.set_profile(dt);
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
    }
}
