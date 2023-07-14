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
using System.Xml.Linq;

namespace Task_Tracker
{
    /// <summary>
    /// Interaction logic for Create_Project.xaml
    /// </summary>
    public partial class Create_Project : UserControl
    {
        int id = 0;
        string status = "";
        string priority = "";
        DateTime dueDate;
        DateTime created_at;
        DateTime updated_at;
        string name = "";
        string description = "";

        SqlConnection conn;

        DataTable table;
        public class ListItem
        {
            public string UserId { get; set; }
            public string Text { get; set; }
            public bool IsSelected { get; set; }
            public ListItem(string UserId, string text, bool selected)
            { this.UserId = UserId; Text = text; IsSelected = selected; }
        }

        List<ListItem> listItems = new List<ListItem>();
        MainWindow window;

        public Create_Project(MainWindow window)
        {
            InitializeComponent();
            conn = new SqlConnection("Data Source = localhost; Initial Catalog = TaskTracking; Integrated Security = True");
            conn.Open();
            User_ComboBox();  //Initialize users selection combobox.
            this.window = window;
            window.panelHeader.Visibility = Visibility.Visible;
            window.title.Text = "Create Project";
            table=window.get_profile();
        }
        //ComboBox for Selecting the Users 
        private void User_ComboBox()
        {
            SqlCommand command = new SqlCommand("SELECT userid, name FROM users where type_name='Employee' ", conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {

                listItems.Add(new ListItem(reader.GetString(0), reader.GetString(1), false));
            }
            reader.Close();
            comboBox1.ItemsSource = listItems;
        }

        //Button to Add Users.
        private void Users_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox user = ((CheckBox)sender);
            usersBlock.Text += user.Content + "\n";
        }
        //Button to Remove Users.
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
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            //Insert data into Tickets Table
            if (FieldValidation() == false)
                return;

            id = insertTicket();

            if (id > 0)
            {
                //Insert users associated to new task 
                insert_Ticket_Users();

                //Insert Project/Task/Service details to its respective tables
                insert_Type_Table();

                //Ticket generated
                error.Foreground = Brushes.Green;
                error.Text = "*Project\nCreated";
                sentNotify();
            }
            else
            {
                error.Text = "*Internal\nError";
            }
        }
        private bool FieldValidation()
        {
            //Initializing rest of the fields.

            created_at = DateTime.Now;
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
        private int insertTicket()
        {
            string query = "insert into tickets values(@priority,@dueDate,@created_at,@updated_at,@parent_id,@type_name,@manager)";
            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("priority", priority);
            command.Parameters.AddWithValue("dueDate", dueDate);
            command.Parameters.AddWithValue("created_at", created_at);
            command.Parameters.AddWithValue("updated_at", updated_at);
            command.Parameters.AddWithValue("parent_id", DBNull.Value);
            command.Parameters.AddWithValue("type_name", "Project");
            command.Parameters.AddWithValue("manager", (string)table.Rows[0]["name"]);
            command.ExecuteNonQuery();

            int id = 0;
            query = "select max(id) from tickets";
            command = new SqlCommand(query, conn);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                id = reader.GetInt32(0);
            }
            reader.Close();
            return id;
        }

        private void insert_Ticket_Users()
        {
            SqlCommand sqlCommand;
            foreach (ListItem item in listItems)
            {
                if (item.IsSelected)
                {
                    string query = "insert into Ticket_Users values(@userid,@ticket_id)";
                    sqlCommand = new SqlCommand(query, conn);
                    sqlCommand.Parameters.AddWithValue("userid", item.UserId);
                    sqlCommand.Parameters.AddWithValue("ticket_id", id);
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
        private void insert_Type_Table()
        {
            string query = "insert into  projects values(@name,@description,@ticket_id,@status)";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("name", name);
            command.Parameters.AddWithValue("description", description.Equals("") ? DBNull.Value : description);
            command.Parameters.AddWithValue("ticket_id", id);
            command.Parameters.AddWithValue("status", status);

            command.ExecuteNonQuery();
        }
        private void sentNotify()
        {
            List<string> emails = new List<string>();
            GmailVerification gmail = new GmailVerification();
            string query = "select email from users u,ticket_users t where t.userid=u.userid and t.ticket_id = @id";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                emails.Add(reader.GetString(0));    
            }
            foreach(string email in emails)
            {
                gmail.NotifyMessage("Project",email);
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            window.open(new LandingPage(window));
        }
    }
}
