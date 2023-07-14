using System.Data.SqlClient;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media;

using System.Data;
using Microsoft.VisualBasic;
using System.Xml.Linq;
using Task_Tracker;

namespace Task_Tracker
{
    public partial class Create_Tasks : UserControl
    {
        string status = "";
        string priority = "";
        DateTime dueDate;
        DateTime created_at;
        DateTime updated_at;
        int p_id = 0;
        string comment = "";
        string user = "";
        string name = "";
        string description = "";
        string attachment = "";

        SqlConnection conn;

        public class ListItem
        {
            public string UserId { get; set; }
            public string Text { get; set; }
            public ListItem(string UserId, string text)
            { this.UserId = UserId; Text = text; }
        }

        List<ListItem> listItems = new List<ListItem>();
        DataRowView project;
        MainWindow window;

        public Create_Tasks(DataRowView selectedRow,MainWindow window)
        {
            InitializeComponent();
            conn = new SqlConnection("Data Source = localhost; Initial Catalog = TaskTracking; Integrated Security = True");
            conn.Open();
            project = selectedRow;
            p_id = (Int32)project["id"];
            User_ComboBox();  //Initialize users selection combobox.
            this.window = window;
            window.panelHeader.Visibility = Visibility.Visible;
            window.title.Text = "Create Task";

        }
        //ComboBox for Selecting the Users 
        private void User_ComboBox()
        {
            SqlCommand command = new SqlCommand("SELECT tu.userid,u.name FROM users u,ticket_users tu where tu.ticket_id = @p_id and u.userid = tu.userid", conn);
            command.Parameters.AddWithValue("p_id", p_id);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                listItems.Add(new ListItem(reader.GetString(0), reader.GetString(1)));
            }
            reader.Close();
            comboBox1.ItemsSource = listItems;
        }
        private void User_selection(object sender, SelectionChangedEventArgs e)
        {
            ListItem selected = (ListItem)comboBox1.SelectedItem;
            if (selected != null)
            {
                user = selected.UserId;
            }
        }
        private void DueDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DueDate.SelectedDate == null)
                return;
            dueDate = DueDate.SelectedDate.Value;
        }
        //To get the Priority of the Task 
        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            var value = (ComboBoxItem)combo.SelectedValue;
            priority = (string)value.Content;
        }

        //To set the Status of a New Task
        private void comboBox3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            var value = (ComboBoxItem)combo.SelectedValue;
            status = (string)value.Content;
        }
        // To store file and its metadata
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                // Dispaly file Information

                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);

                string filename = fileInfo.Name;
                string fileSize = string.Format("{0} {1}", (fileInfo.Length / 1.049e+6).ToString("0.0"), "MB");

                fileName.Text = filename + "  " + fileSize;
                attachment = filename;

                //Adding file to Server or database

            }
        }
        private bool FieldValidation()
        {
            //Initializing rest of the fields.

            created_at = DateTime.Now;
            updated_at = DateTime.Now;
            comment = Comment.Text;
            name = nameTxt.Text;
            description = descriptionText.Text;

            if (user.Equals(""))
            {
                error.Text = "*User\nRequired";
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
        //All insert statements would be Here else foreign key constraint gets violated
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            //Insert data into Tickets Table
            if (FieldValidation() == false)
                return;

            int n = insertTicket(); //inserts ticket and return the generated ticket id

            if (n > 0)
            {
                //Insert users associated to new task 
                insert_Ticket_Users(n);

                //Insert Project/Task/Service details to its respective tables
                insert_Type_Table(n);

                //Add file metadata to Attachments table
                insert_Attachment(n);

                error.Foreground = Brushes.Green;
                error.Text = "*Task\nCreated";
                sentNotify(n);
            }
            else
            {
                error.Text = "*Internal\nError";
            }
        }
        private int insertTicket()
        {
            string query = "insert into tickets values(@priority,@dueDate,@created_at,@updated_at,@parent_id,@type_name,@manager)";
            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("priority", priority);
            command.Parameters.AddWithValue("dueDate", dueDate);
            command.Parameters.AddWithValue("created_at", created_at);
            command.Parameters.AddWithValue("updated_at", updated_at);
            command.Parameters.AddWithValue("parent_id", p_id);
            command.Parameters.AddWithValue("type_name", "Task");
            command.Parameters.AddWithValue("manager",DBNull.Value);

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
        private void insert_Ticket_Users(int id)
        {
            
            string query = "insert into Ticket_Users values(@userid,@ticket_id)";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("userid", user);
            command.Parameters.AddWithValue("ticket_id", id);
            command.ExecuteNonQuery();
        }
        private void insert_Type_Table(int id)
        {
            string query = "insert into tasks values(@name,@description,@ticket_id,@Comment,@status)";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("name", name);
            command.Parameters.AddWithValue("description", description.Equals("") ? DBNull.Value : description);
            command.Parameters.AddWithValue("ticket_id", id);
            command.Parameters.AddWithValue("Comment", comment.Equals("") ? DBNull.Value : comment);
            command.Parameters.AddWithValue("status", status);

            command.ExecuteNonQuery();
        }
        private void insert_Attachment(int id)
        {
            if (attachment.Equals(""))
            {
                return;
            }
            string query = "insert into Attachments values(@filename,@ticket_id)";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("filename", attachment);
            command.Parameters.AddWithValue("ticket_id", id);
            command.ExecuteNonQuery();
        }
        private void sentNotify(int id)
        {
            string email;
            GmailVerification gmail = new GmailVerification();
            string query = "select email from users u,ticket_users t where t.userid=u.userid and t.ticket_id = @id";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();
            if(reader.Read())
            {
                email = reader.GetString(0);
                gmail.NotifyMessage("Task", email);
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            window.open(new Project_Dashboard(project,window));
        }
    }
}
