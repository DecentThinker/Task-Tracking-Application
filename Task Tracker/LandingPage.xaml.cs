using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Task_Tracker
{
    public partial class LandingPage : UserControl
    {
        SqlConnection conn;
        MainWindow window;
        public LandingPage(MainWindow window)
        {
            InitializeComponent();
            string connstring = "Data Source = localhost; Initial Catalog = TaskTracking; Integrated Security = True";
            conn = new SqlConnection(connstring);
            conn.Open();
            createTable();
            this.window = window;
            window.panelHeader.Visibility = Visibility.Visible;
            window.title.Text = "Projects";
        }
        public void createTable()
        {
            DataTable dt = new DataTable();

            string query1 = "select t.id,p.name ,p.status,t.priority,t.due_date,t.manager,p.descriptions from tickets t , projects p where p.ticket_id = t.id";
            SqlCommand cmd = new SqlCommand(query1, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dataTable1 = new DataTable();
            adapter.Fill(dataTable1);

            dataTable1.Columns.Add("resources", typeof(int));
            dataTable1.Columns.Add("count", typeof(int));


            string query2 = "select count(tu.ticket_id) as resources from Ticket_Users tu join projects p on p.ticket_id = tu.ticket_id group by p.ticket_id";
            cmd = new SqlCommand(query2, conn);
            adapter = new SqlDataAdapter(cmd);
            DataTable dataTable2 = new DataTable();
            adapter.Fill(dataTable2);

            string query3 = "select count(t.id) as count from Tickets t right join projects p on p.ticket_id = t.parent_id group by p.ticket_id";
            cmd = new SqlCommand(query3, conn);
            adapter = new SqlDataAdapter(cmd);
            DataTable dataTable3 = new DataTable();
            adapter.Fill(dataTable3);

            int n = dataTable1.Rows.Count;

            for (int i = 0; i < n; i++)
            {
                dataTable1.Rows[i]["resources"] = dataTable2.Rows[i]["resources"];
                dataTable1.Rows[i]["count"] = dataTable3.Rows[i]["count"];
            }
            dataGrid.ItemsSource = dataTable1.DefaultView;
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // get the selected row from the datagrid
            if (sender is DataGrid dataGrid)
            {
                if (dataGrid.SelectedItem is DataRowView selectedRow)
                {
                    window.open(new Project_Dashboard(selectedRow,window));
                    
                }
            }
        }
        private void Add_Project(object sender, RoutedEventArgs e)
        {
            Create_Project create = new Create_Project(window);
            window.open(create);
        }
        private void Edit_Project(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null)
            {
                Edit_Project editor = new Edit_Project(selectedRow,window);
                window.open(editor);
                createTable();
            }
        }
        private void Delete_Project(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null)
            {
                // Delete the selected row from the DataGrid
                string query = "delete from tickets where id=@id or parent_id=@id";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("id", selectedRow["id"]);
                command.ExecuteNonQuery();
                createTable();
            }
        }

        private void Progress_Project(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
            if (selectedRow != null)
            {
                View_Progress progress = new View_Progress(window,selectedRow);
                window.open(progress);
            }
        }
    }
}