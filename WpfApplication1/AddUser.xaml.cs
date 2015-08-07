using System;
using System.Collections.Generic;
using System.Data.OleDb;
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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        private OleDbConnection connection = new OleDbConnection();
        public AddUser()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\Database.accdb;
            Persist Security Info=False";
            //connection.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=|DataDirectory|/Database.accdb;";
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            Username_txt.Text = "";
            Password_txt.Text = "";
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
            this.Close();
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (Username_txt.Text == "" || Password_txt.Text == "")
            {
                MsgBox msg = new MsgBox();
                msg.Title = "ERROR";
                msg.text.Content = "Please enter a valid \nUserName and Password";
                msg.Show();
                return;
            }
            connection.Open();
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            //first check for no duplication
            command.CommandText = "SELECT * FROM [User] WHERE StrComp([UserName],'" + Username_txt.Text + "',1)= 0";
            OleDbDataReader addUser_reader = command.ExecuteReader();
            int count = 0;
            while (addUser_reader.Read())
            {
                count++;
            }
            addUser_reader.Close();
            if (count != 0)
            {
                MsgBox msg = new MsgBox();
                msg.Title = "ERROR";
                msg.text.Content = "Username already exists";
                msg.Show();
                connection.Close();
                return;
            }           
            //add the new user
            string UserType = null;
            switch (Type_CB.Text)
            {
                case "System Admin":
                    UserType = "SA";
                    break;
                case "Admin":
                    UserType = "A";
                    break;
                default:
                    UserType = "R";
                    break;

            }
            command.CommandText = "INSERT INTO [User]  ( [UserName], [Password], [Type] )" +
                "VALUES('" + Username_txt.Text + "','" + Password_txt.Text + "','" + UserType + "')";
            command.ExecuteNonQuery();
            command.CommandText = "SELECT * FROM [User] WHERE UserName = '" + Username_txt.Text + "'";
            addUser_reader = command.ExecuteReader();
            count = 0;
            while (addUser_reader.Read())
            {
                count++;
            }
            if (count == 0)
            {
                MsgBox msg = new MsgBox();
                msg.Title = "ERROR";
                msg.text.Content = "Failed";
                msg.Show();
                connection.Close();
                return;
            }
            else
            {
                MsgBox msg = new MsgBox();
                msg.Title = "SUCCESS";
                msg.text.Content = "The User was successfully added";
                msg.Show();
                msg.Topmost = true;
            }
            connection.Close();
            Application.Current.MainWindow.Show();
            this.Close();
        }

    }
}
