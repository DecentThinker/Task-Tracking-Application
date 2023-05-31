using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Resources;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using Task_Tracker;

namespace Task_Tracker
{
    /// <summary>
    /// Interaction logic for Project_Dashboard.xaml
    /// </summary>
    public partial class Project_Dashboard : UserControl
    {
        int id;
        MainWindow window;
        SqlConnection conn;
        DataRowView project;
        DataTable dataTable1;
        public Project_Dashboard(DataRowView selectedRow,MainWindow window)
        {
            InitializeComponent();
            string connstring = "Data Source = localhost; Initial Catalog = TaskTracking; Integrated Security = True";
            conn = new SqlConnection(connstring);
            conn.Open();
            project = selectedRow;
            //Ticket id of the projrect
            id = (int)project["id"];
            createTable();
            project_Resources();
            this.window = window;
            window.panelHeader.Visibility = Visibility.Visible;
            string name = (string)project["name"];
            window.title.Text = name;
            dataGrid.ItemsSource = dataTable1.DefaultView;
        }
        private void project_Resources()
        {
            List<string> users = new List<string>(); 
            string query = "SELECT u.name FROM users u,ticket_users tu where tu.ticket_id = @id and u.userid = tu.userid";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("id",id);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) 
            {
                users.Add(reader.GetString(0));
            }
            reader.Close();
            resources.ItemsSource = users;
        }
        private void createTable()
        {
            //name of project to display on top
          

            string query1 = "select p.name, p.descriptions, p.status, p.comment, t.id,  t.priority, t.created_at, t.due_date, t.parent_id  from tickets t , tasks p where p.ticket_id = t.id and t.parent_id=@id";
            SqlCommand cmd = new SqlCommand(query1, conn);
            cmd.Parameters.AddWithValue("id", id);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            dataTable1 = new DataTable();
            adapter.Fill(dataTable1);
            adapter.Dispose();

            dataTable1.Columns.Add("resources", typeof(string));

            int count = dataTable1.Rows.Count;

            int opened = 0, closed = 0, missed = 0;
            DateTime current = DateTime.Now;

            for (int i = 0; i < count; i++)
            {
                string resource = "";
                string query2 = "select distinct(u.name) as resources from users u,ticket_users tu,tickets t where tu.ticket_id = @id and tu.UserId=u.userid";
                cmd = new SqlCommand(query2, conn);
                cmd.Parameters.AddWithValue("id",dataTable1.Rows[i]["id"]);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    resource = reader.GetString(0);
                }
                reader.Close();    
                dataTable1.Rows[i]["resources"] = resource;

                string status = (string)dataTable1.Rows[i]["status"];
                if(status.Equals("Closed"))
                {
                    closed++;
                }
                else
                {
                    opened++;
                }
                DateTime date =(DateTime)dataTable1.Rows[i]["due_date"];
                if(date < current )
                {
                    missed++;
                }
            }
            total.Text = count.ToString();
            open.Text = opened.ToString();
            close.Text = closed.ToString();
            missing.Text = missed.ToString();
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null)
            {
                View_Task view = new View_Task(selectedRow);
                view.ShowDialog();
                if(view.DialogResult==true)
                {
                    view.Close();
                }
            }
        }
        private void Create_task(object sender, RoutedEventArgs e)
        {
            Create_Tasks create = new Create_Tasks(project,window);
            window.open(create);
            createTable();
        }
        private void Edit_Task(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null)
            {
                Edit_Task editor = new Edit_Task(project,selectedRow,window);
                window.open(editor);
                createTable();
            }
        }

        private void Delete_Task(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null)
            {
                // Delete the selected row from the DataGrid
                string query = "delete from tickets where id=@id";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("id", selectedRow["id"]);
                command.ExecuteNonQuery();
                createTable();
            }
        }
        private void Add_Progress(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null)
            {
                Add_Progress add = new Add_Progress(window,selectedRow);
                add.ShowDialog();
                if (add.DialogResult == true)
                {
                    add.Close();
                }
            }
        }
        private void Total_Tasks(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = dataTable1.DefaultView;
        }

        private void Open_Tasks(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = dataTable1.Clone();

            foreach (DataRow row in dataTable1.Rows)
            {
                if ((string)row["status"] != "Closed")
                {
                    dataTable.ImportRow(row);    
                }
            }
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void Closed_Tasks(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = dataTable1.Clone();

            foreach (DataRow row in dataTable1.Rows)
            {
                if ((string)row["status"] == "Closed")
                {
                    dataTable.ImportRow(row);
                }
            }
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void Missed_Tasks(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = dataTable1.Clone();
            DateTime current = DateTime.Now;

            foreach (DataRow row in dataTable1.Rows)
            {
                DateTime date = (DateTime)row["due_date"];
                if (date < current)
                {
                    dataTable.ImportRow(row);
                }
            }
            dataGrid.ItemsSource = dataTable.DefaultView;
        }
    }
}
