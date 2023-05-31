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

namespace Task_Tracker
{
    public partial class Edit_Task : UserControl
    {
        int id = 0;
        string status = "";
        string priority = "";
        DateTime dueDate;
        DateTime updated_at;
        int p_id = 0;
        string comment = "";
        string name = "";
        string description = "";
        string attachment = "";
        string user = "";

        SqlConnection conn;

        public class ListItem
        {
            public string UserId { get; set; }
            public string Text { get; set; }

            public bool IsSelected { get; set; }
            public ListItem(string UserId, string text, bool isSelected)
            {
                this.UserId = UserId; Text = text; IsSelected = isSelected;
            }
        }

        List<ListItem> listItems = new List<ListItem>();
        List<string> marked = new List<string>();
        DataRowView project;
        DataRowView task;
        MainWindow window;

        public Edit_Task()
        {
            InitializeComponent();
            conn = new SqlConnection("Data Source = localhost; Initial Catalog = TaskTracking; Integrated Security = True");
            conn.Open();
        }
        public Edit_Task(DataRowView project, DataRowView task, MainWindow window) : this()
        {
            this.project = project;
            this.task = task;
            id = (Int32)task["id"];     // ticket id of the task
            p_id = (Int32)task["parent_id"]; // parent id of the task
            User_ComboBox();  //Initialize users selection combobox.
            fill();
            this.window = window;
            window.panelHeader.Visibility = Visibility.Visible;
            window.title.Text = "Edit Task";
        }
        //Function to create id for Tickets table.
        private void fill()
        {
            mark_users();            
            //marks the users which are already assigned to selected task
            foreach (ComboBoxItem boxItem in comboBox2.Items)
            {
                if (boxItem.Content.ToString() == (string)task["priority"])
                {
                    boxItem.IsSelected = true;
                }
            }
            foreach (ComboBoxItem boxItem in comboBox3.Items)
            {
                if (boxItem.Content.ToString() == (string)task["status"])
                {
                    boxItem.IsSelected = true;
                }
            }
            DueDate.SelectedDate = (DateTime)task["due_date"];
            nameTxt.Text = (string)task["name"];
            descriptionText.Text = (string)task["descriptions"];
            fileName.Text = fetch_filename(); //find and assign filename associated to particular task id
            Comment.Text = (string)task["comment"];
        }
        //Find filename related to task and delete it also
        private string fetch_filename()
        {
            //Fetch the current filename
            string file = "";
            SqlCommand command = new SqlCommand("SELECT a.filename from attachments a where a.ticket_id=@id", conn);
            command.Parameters.AddWithValue("id", id);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                file = reader.GetString(0);
            }
            reader.Close();

            //Delete that file if exist 
            if (file.Equals(""))
            {
                command = new SqlCommand("Delete from attachments where ticket_id=@id", conn);
                command.Parameters.AddWithValue("id", id);
                command.ExecuteNonQuery();
            }
            return file;
        }
        //ComboBox for Selecting the Users 
        private void User_ComboBox()
        {
            SqlCommand command = new SqlCommand("SELECT tu.userid,u.name FROM users u,ticket_users tu where tu.ticket_id = @p_id and u.userid = tu.userid", conn);
            command.Parameters.AddWithValue("p_id", p_id);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                listItems.Add(new ListItem(reader.GetString(0), reader.GetString(1),false));
            }
            reader.Close();
            comboBox1.ItemsSource = listItems;
        }
        //Marks the already Selected users in Project in all users list.
        private void mark_users()
        {
            string marked_user = "";
            //select those userid from ticket_users which are assigned to that particular project
            string query = "select userid from ticket_users where ticket_id=@id";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("id", id);
            SqlDataReader reader = command.ExecuteReader();
            if(reader.Read())
            {
                marked_user = reader.GetString(0);
            }
            reader.Close();
            foreach (ListItem item in comboBox1.Items)
            {
                if (marked_user.Equals(item.UserId))
                {
                    comboBox1.SelectedItem = item;
                    comboBox1.SelectionChanged += Users_selected;
                }
            }
        }
        private void Users_selected(object sender, SelectionChangedEventArgs e)
        {
            ListItem selected = (ListItem)comboBox1.SelectedItem;
            if (selected != null)
            {
                user = selected.UserId;
            }
        }
        //To select the due date of the Task
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

            updated_at = DateTime.Now;
            comment = Comment.Text;
            name = nameTxt.Text;
            description = descriptionText.Text;

            if (user.Equals(""))
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
        //All insert statements would be Here else foreign key constraint gets violated
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            //Insert data into Tickets Table
            if (FieldValidation() == false)
                return;

            int n = updateTicket(); //updates ticket and return the affected number of rows

            if (n > 0)
            {
                //Insert users associated to new task 
                update_Ticket_Users();

                //Insert Project/Task/Service details to its respective tables
                update_Task();

                //Add file metadata to Attachments table
                update_Attachment();

                error.Foreground = Brushes.Green;
                error.Text = "*Task\nEdited";
            }
            else
            {
                error.Text = "*Internal\nError";
            }
        }
        private int updateTicket()
        {
            string query = "update tickets set priority=@priority,due_date=@dueDate,updated_at=@updated_at where id=@id";
            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("priority", priority);
            command.Parameters.AddWithValue("dueDate", dueDate);
            command.Parameters.AddWithValue("updated_at", updated_at);
            return command.ExecuteNonQuery();
        }
        private void update_Ticket_Users()
        {
            //Delete All previous users 
            string del = "delete from Ticket_Users where ticket_id=@id";
            SqlCommand command = new SqlCommand(del, conn);
            command.Parameters.AddWithValue("id", id);
            command.ExecuteNonQuery();

            //Update with new selected User
            string query = "insert into Ticket_Users values(@userid,@ticket_id)";
            command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("userid", user);
            command.Parameters.AddWithValue("ticket_id", id);
            command.ExecuteNonQuery();

        }
        private void update_Task()
        {
            string query = "update tasks set name=@name,descriptions=@description, status=@status ,comment=@comment where ticket_id=@id";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("name", name);
            command.Parameters.AddWithValue("description", description.Equals("") ? DBNull.Value : description);
            command.Parameters.AddWithValue("status", status);
            command.Parameters.AddWithValue("comment", comment);
            command.Parameters.AddWithValue("id", id);
            command.ExecuteNonQuery();
        }
        private void update_Attachment()
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
        //To Reset All Blocks
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            new Edit_Task(project,task,window);
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            window.open(new Project_Dashboard(project, window));
        }
    }
}
