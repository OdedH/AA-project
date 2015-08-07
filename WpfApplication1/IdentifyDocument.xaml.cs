using System;
using System.Collections.Generic;
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
using System.IO;
using System.Data.OleDb;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for IdentifyDocument.xaml
    /// </summary>
    public partial class IdentifyDocument : Window
    {
        private string document_to_classify_path = "";
        private OleDbConnection connection = new OleDbConnection();
        private string current_filename = "";
        public IdentifyDocument()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\Database.accdb;
            Persist Security Info=False";
            string userType = Application.Current.Properties["CurrentUserType"].ToString();
            if (userType == "A" || userType == "SA")
            {
                btn_Feedback.Visibility = System.Windows.Visibility.Visible;
            }
            //Check settings
            if (Application.Current.Properties["CurrentCorpus"].ToString() != "")
            {
                this.Show();
                return;
            }
            MsgBox msg = new MsgBox();
            msg.Title = "ERROR";
            msg.text.Content = "Please choose corpus first \nvia 'Change Settings' ";
            msg.Topmost = true;
            Application.Current.MainWindow.Show();
            msg.Show();
            this.Close();
        }

        private void btn_Load_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".txt";
            dlg.Filter = "TEXT Files (*.txt)|*.txt";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                documentText.Text = System.IO.Path.GetFileName(filename);
                document_to_classify_path = filename;
                current_filename = System.IO.Path.GetFileName(filename);
            }
        }

        private void btn_Identify_Click(object sender, RoutedEventArgs e)
        {
            //first add current filename into DB , add only after user pressed identify
            connection.Open();
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            //check document doesnt exist
            command.CommandText = "SELECT * FROM [Document]  WHERE " +
                                   "[DocumentName] ='" + current_filename +
                                   "' AND [CorpusName] ='"
                                   + Application.Current.Properties["CurrentCorpus"].ToString() +"'";
            OleDbDataReader reader = command.ExecuteReader();
            //while (reader.Read())
            //{
            //    MessageBox.Show((reader.GetString(2) == DBNull.Value.ToString()).ToString());
            //}
            
            if (reader.HasRows == false)
            {
                //if document doesnt exist insert to DB
                OleDbCommand insert_command = new OleDbCommand();
                insert_command.Connection = connection;
                insert_command.CommandText = "INSERT INTO [Document]  ([DocumentName], [CorpusName], [AuthorName])" +
                          "VALUES('" + current_filename + "','" +
                          Application.Current.Properties["CurrentCorpus"].ToString() + "','" + 
                          DBNull.Value + "')";
                insert_command.ExecuteNonQuery();
            }
            connection.Close();
            CallIdentify();
        }

        private int IdentifyRegular()
        {
            int classifier = -1;
            connection.Open();
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM [AdminCorpusSettings]  WHERE " +
                      "[CorpusName] ='" + Application.Current.Properties["CurrentCorpus"].ToString() +
                      "' ORDER BY [UpdateDate] ";       
            OleDbDataReader reader = command.ExecuteReader();
            if(reader.HasRows)
            {
                reader.Read();
                classifier = reader.GetInt32(7);
            }
            connection.Close();
            return classifier;
        }

        private int IdentifyAdmin()
        {
            int classifier = -1;
            connection.Open();
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM [AdminCorpusSettings]  WHERE " +
                      "[CorpusName] ='" + Application.Current.Properties["CurrentCorpus"].ToString() +
                      "' AND [AdminName] ='" + Application.Current.Properties["CurrentUserName"].ToString() +
                      "'";
            OleDbDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                classifier = reader.GetInt32(7);
            }
            connection.Close();
            return classifier;
        }

        private void updateAuthor(string corpus, string documentName, string authorname)
        {
            connection.Open();
            OleDbCommand insert_command = new OleDbCommand();
            insert_command.Connection = connection;
            insert_command.CommandText = "UPDATE [Document] SET [AuthorName]='" + authorname +
                      "' WHERE [CorpusName]='" + corpus + "' AND [DocumentName]='"
                      + documentName + "'";
            insert_command.ExecuteNonQuery();
            connection.Close();
        }

        private void CallIdentify()
        {
            //find classifier number
            int classifier = -1;
            if (Application.Current.Properties["CurrentUserType"].ToString() == "R")
            {
                classifier = IdentifyRegular();
            }
            else
            {
                classifier = IdentifyAdmin();
            }
            if (classifier == -1)
            {
                MsgBox msg = new MsgBox();
                msg.Title = "ERROR";
                msg.text.Content = "No classifier available for these settings.\nPlease change settings";
                msg.Show();
                msg.Topmost = true;
                Application.Current.MainWindow.Show();
                this.Close();
                return;
            }
            //run the Algoritem with the classifier:
            string result = "";
            string parameters = "classify_object " + document_to_classify_path + " " + classifier.ToString();
            try
            {
                result = Utils.run_cmd(System.IO.Path.GetFullPath("./Scripts/API.py"), parameters);
            }
            catch (Exception e)
            {
                MsgBox msg = new MsgBox();
                msg.Title = "ERROR";
                msg.text.Content = "Error occured during classification.\nPlease check the parameters.\n" +
                                    e.Message;
                msg.Show();
                msg.Topmost = true;
                return;
            }
            authorText.Text = result;
            updateAuthor(Application.Current.Properties["CurrentCorpus"].ToString(), current_filename, result);
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
            this.Close();
        }

        private void btn_Feedback_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new AddFeedback().ShowDialog();
            ShowDialog();
        }

    }
}
