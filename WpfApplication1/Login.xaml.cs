using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private OleDbConnection connection = new OleDbConnection();
        public Login()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\Database.accdb;
            Persist Security Info=False";
            //connection.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=|DataDirectory|/Database.accdb;";
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            if (User_txt.Text == "")
            {
                
                MsgBox msg = new MsgBox();
                msg.Title = "ERROR";
                msg.text.Content = "Please enter a valid User Name";
                msg.Show();
                return;
            }          
            connection.Open();
            OleDbCommand check_user_cmd = new OleDbCommand();
            check_user_cmd.Connection = connection;
            check_user_cmd.CommandText = "SELECT * FROM [User] WHERE UserName='" + User_txt.Text + "'AND Password='" + Password_txt.Password + "'";
            OleDbDataReader reader = check_user_cmd.ExecuteReader();
            int count = 0;
            string userType = null;
            while (reader.Read())
            {
                userType = reader.GetString(2); 
                count++;  
            }
            reader.Close();
            connection.Close();
            if (count == 0)
            {
                MsgBox msg = new MsgBox();
                msg.Title = "ERROR";
                msg.text.Content = "Username or Password is not correct";
                msg.Show();
                return;
            }

            Application.Current.Properties["CurrentUserName"] = User_txt.Text;
            Application.Current.Properties["CurrentUserType"] = userType;
            Application.Current.Properties["CurrentCorpus"] = "";
            Application.Current.MainWindow.Show();
            this.Close();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
            this.Close();
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            User_txt.Text = "";
            Password_txt.Password = "";
        } 
    }
}
