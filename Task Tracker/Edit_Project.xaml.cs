using Microsoft.VisualBasic;
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
    /// Interaction logic for Edit_Project.xaml
    /// </summary>
    public partial class Edit_Project : UserControl
    {
        int id;
        string status = "";
        string priority = "";
        DateTime dueDate;
        DateTime updated_at;
        string name = "";
        string description = "";

        SqlConnection conn;

        public class ListItem
        {
            public string UserId { get; set; }
            public string Text { get; set; }
            public bool IsSelected { get; set; }
            public ListItem(string UserId, string text, bool selected)
            { this.UserId = UserId; Text = text; IsSelected = selected; }
        }

        List<ListItem> listItems = new List<ListItem>();
        List<string> marked = new List<string>();
        DataRowView project;
        MainWindow window;
        public Edit_Project()
        {
            InitializeComponent();
            conn = new SqlConnection("Data Source = localhost; Initial Catalog = TaskTracking; Integrated Security = True");
            conn.Open();
         
        }
        public Edit_Project(DataRowView selectedRow, MainWindow window) : this()
        {
            project = selectedRow;
            User_ComboBox();  //Initialize users selection combobox with all users and selected users .
            fill();           //fills the data from selected row of data grid to their respective fields.
            this.window = window;
            window.panelHeader.Visibility = Visibility.Visible;
            window.title.Text = "Edit Project";
        }
        //Function to create id for Tickets table.
        private void fill()
        {
            id = (int)project["id"]; //this is the ticket id of the project
            mark_users();            //marks the users which are already assigned to selected project
            foreach (ComboBoxItem boxItem in comboBox2.Items)
            {
                if (boxItem.Content.ToString() == (string)project["priority"])
                {
                    boxItem.IsSelected = true;
                }
            }
            foreach (ComboBoxItem boxItem in comboBox3.Items)
            {
                if (boxItem.Content.ToString() == (string)project["status"])
                {
                    boxItem.IsSelected = true;
                }
            }
            DueDate.SelectedDate = (DateTime)project["due_date"];
            nameTxt.Text = (string)project["name"];
            descriptionText.Text = (string)project["descriptions"];
        }
        //ComboBox for Selecting the Users for Project from all users  
        private void User_ComboBox()
        {
            SqlCommand command = new SqlCommand("SELECT userid,name FROM users ", conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                listItems.Add(new ListItem(reader.GetString(0), reader.GetString(1), false));
            }
            reader.Close();
            comboBox1.ItemsSource = listItems;
        }
        //Marks the already Selected users in Project in all users list.
        private void mark_users()
        {
            //select those userid from ticket_users which are assigned to that particular project
            string query = "select userid from ticket_users where ticket_id=@id";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("id", id);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                marked.Add(reader.GetString(0));
            }
            reader.Close();
            foreach (ListItem item in listItems)
            {
                if (marked.Contains(item.UserId))
                {
                    item.IsSelected = true;
                    usersBlock.Text += item.Text + "\n";
                }
            }
        }
        //Check to Add Users.
        private void Users_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox user = ((CheckBox)sender);
            usersBlock.Text += user.Content + "\n";
        }
        //Uncheck to Remove Users.
        private void Users_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox user = ((CheckBox)sender);
            usersBlock.Text = usersBlock.Text.Replace(user.Content + "\n", "");
        }
        //To select the due date of the Task
        private void DueDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DueDate.SelectedDate == null)
                return;
            dueDate = DueDate.SelectedDate.Value;
        }
        //To set the Priority
        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            var value = (ComboBoxItem)combo.SelectedValue;
            priority = (string)value.Content;
        }
        //To set the Status
        private void comboBox3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            var value = (ComboBoxItem)combo.SelectedValue;
            status = (string)value.Content;
        }
        //Submit button creates the Project
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            //Insert data into Tickets Table
            if (FieldValidation() == false)
                return;

            int n = updateTicket();

            if (n == 1)
            {
                //Insert users associated to new task 
                update_Ticket_Users();

                //Insert Project/Task/Service details to its respective tables
                update_Project();

                //Ticket generated
                error.Foreground = Brushes.Green;
                error.Text = "*Project\nUpdated";
            }
            else
            {
                error.Text = "*Internal\nError";
            }
        }
        //Validates if all mandatory fields are filled or not
        private bool FieldValidation()
        {
            //Initializing rest of the fields.
            updated_at = DateTime.Now;
            name = nameTxt.Text;
            description = descriptionText.Text;

            if (usersBlock.Text.Equals(""))
            {
                error.Text = "*Users\nRequired";
                return false;
            }
            if (DueDate.SelectedDate == null)
            {
                error.Text = "*Due Date\nRequired";
                return false;
            }
            if (name.Equals(""))
            {
                error.Text = "*Name\nRequired";
                return false;
            }
            return true;
        }
        //updates tickets table
        private int updateTicket()
        {
            string query = "update tickets set priority=@priority,due_Date=@dueDate,updated_at=@updated_at where id=@id";
            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("priority", priority);
            command.Parameters.AddWithValue("dueDate", dueDate);
            command.Parameters.AddWithValue("updated_at", updated_at);
            return command.ExecuteNonQuery();
        }
        //Update the ticket_users table
        private void update_Ticket_Users()
        {
            //Delete All previous users 
            string del = "delete from Ticket_Users where ticket_id=@id";
            SqlCommand command = new SqlCommand(del, conn);
            command.Parameters.AddWithValue("id", id);
            command.ExecuteNonQuery();

            //insert new list of users
            foreach (ListItem item in listItems)
            {
                if (item.IsSelected)
                {
                    string up = "insert into Ticket_Users values(@userid,@ticket_id)";
                    command = new SqlCommand(up, conn);
                    command.Parameters.AddWithValue("userid", item.UserId);
                    command.Parameters.AddWithValue("ticket_id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
        //Updates the Project Table
        private void update_Project()
        {
            string query = "update projects set name=@name,descriptions=@description, status=@status where ticket_id=@id";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("name", name);
            command.Parameters.AddWithValue("description", description.Equals("") ? DBNull.Value : description);
            command.Parameters.AddWithValue("status", status);
            command.Parameters.AddWithValue("id", id);
            command.ExecuteNonQuery();
        }
        //To close the Edit Project Window .
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            window.open(new LandingPage(window));
        }
    }
}
