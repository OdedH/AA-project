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
using System.Data.OleDb;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for AddFeedback.xaml
    /// </summary>
    public partial class AddFeedback : Window
    {
        private OleDbConnection connection = new OleDbConnection();
        private Dictionary<string, string> alg_results = new Dictionary<string, string>();
        public AddFeedback()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\Database.accdb;
            Persist Security Info=False";
            LoadData(Application.Current.Properties["CurrentCorpus"].ToString());
        }

        private void LoadData(string current_corpus)
        {
            if (LoadDocuments(current_corpus) == true)
            {
                LoadAuthors(current_corpus);
            }
        }

        private bool LoadDocuments(string current_corpus)
        {
            connection.Open();
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;

            command.CommandText = "SELECT * FROM [Document] WHERE StrComp([CorpusName],'" + current_corpus + "',1)= 0 ";
            OleDbDataReader LoadDocument_reader = command.ExecuteReader();
            int count = 0;

            while (LoadDocument_reader.Read())
            {
                string document = LoadDocument_reader.GetValue(0).ToString();
                string alg_result = LoadDocument_reader.GetValue(2).ToString();
                alg_results.Add(document, alg_result);
                combo_document.Items.Add(document);
                count++;
            }
            if (count == 0)
            {
                MessageBox.Show("No documents available for feedback.");
                connection.Close();
                return false;
            }
            LoadDocument_reader.Close();
            connection.Close();
            return true;
        }

        private void LoadAuthors(string current_corpus)
        {
            connection.Open();
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;

            command.CommandText = "SELECT * FROM [Author] WHERE StrComp([CorpusName],'" + current_corpus + "',1)= 0";
            OleDbDataReader LoadAuthor_reader = command.ExecuteReader();
            int count = 0;
            combo_correct_author.Items.Clear();
            while (LoadAuthor_reader.Read())
            {
                string author = LoadAuthor_reader.GetValue(2).ToString();
                combo_correct_author.Items.Add(author);
                count++;
            }
            if (count == 0)
            {
                MessageBox.Show("No existing authors available in this corpus.");
                connection.Close();
                return;
            }
            LoadAuthor_reader.Close();
            connection.Close();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void ComboBoxFeedback_Changed(object sender, SelectionChangedEventArgs e)
        {
            switch (combo_feedback.SelectedItem == feedback_incorrect)
            {
                case true:
                    checkbox_add_author.Visibility = System.Windows.Visibility.Visible;
                    break;
                default:
                    checkbox_add_author.IsChecked = false;
                    checkbox_add_author.Visibility = System.Windows.Visibility.Hidden;
                    lbl_correct_author.Visibility = System.Windows.Visibility.Hidden;
                    combo_correct_author.Visibility = System.Windows.Visibility.Hidden;
                    break;
            }
        }

        private void checkbox_add_author_Checked(object sender, RoutedEventArgs e)
        {
            lbl_correct_author.Visibility = System.Windows.Visibility.Visible;
            combo_correct_author.Visibility = System.Windows.Visibility.Visible;
        }

        private void checkbox_add_author_Unchecked(object sender, RoutedEventArgs e)
        {
            lbl_correct_author.Visibility = System.Windows.Visibility.Hidden;
            combo_correct_author.Visibility = System.Windows.Visibility.Hidden;
        }

        private void combo_document_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            result_author.Text = "";
            string selected_doc = (sender as ComboBox).SelectedItem as string;
            if (alg_results[selected_doc] != DBNull.Value.ToString())
            {
                result_author.Text = alg_results[selected_doc];
                LoadAuthors(Application.Current.Properties["CurrentCorpus"].ToString());
                combo_correct_author.Items.Remove(result_author.Text.Trim());
            }
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            string document = combo_document.Text;
            string author = result_author.Text;
            string feedback = combo_feedback.Text;
            string admin = Application.Current.Properties["CurrentUserName"].ToString();
            string corpus = Application.Current.Properties["CurrentCorpus"].ToString();
            if (document == "" || author == "" || feedback == "")
            {
                MsgBox msg = new MsgBox();
                msg.Title = "ERROR";
                msg.text.Content = "Please fill in all fields \nin order to save the feedback.";
                msg.Show();
                msg.Topmost = true;
                return;
            }
            if (feedback == feedback_correct.Content.ToString())
            {
                InsertFeedbackToDB(document, author, admin, true, corpus);
            }
            else if (checkbox_add_author.IsChecked == true)
            {
                if (combo_correct_author.Text == "")
                {
                    MsgBox msg = new MsgBox();
                    msg.Title = "ERROR";
                    msg.text.Content = "Please fill in the Correct Author \nin order to save the feedback.";
                    msg.Show();
                    msg.Topmost = true;
                    return;
                }
                InsertFeedbackToDB(document, author, admin, false, corpus, combo_correct_author.Text);
            }
            else
            {
                InsertFeedbackToDB(document, author, admin, false, corpus);
            }
        }

        private void InsertFeedbackToDB(string document, string author, string admin, bool IsCorrect, string corpus, string new_author = "")
        {
            connection.Open();
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            //first check for no duplication
            int record_id = 0;
            string action = "add";
            command.CommandText = "SELECT * FROM [Feedback] WHERE StrComp([DocumentName],'" + document + "',1)= 0 " +
                                  "AND StrComp([AuthorName],'" + author + "',1)= 0 " +
                                  "AND StrComp([AdminName],'" + admin + "',1)= 0 " +
                                  "AND StrComp([CorpusName],'" + corpus + "',1)= 0 ";
            OleDbDataReader addFeedback_reader = command.ExecuteReader();
            int count = 0;
            while (addFeedback_reader.Read())
            {
                record_id = Convert.ToInt32(addFeedback_reader.GetValue(0).ToString());
                count++;
            }
            if (count != 0)
            {
                action = "update";
            }
            addFeedback_reader.Close();

            // add feedback
            if (action == "add")
            {
                command.CommandText = "INSERT INTO [Feedback]  ([DocumentName], [AuthorName], [AdminName], [IsCorrect], [NewAuthorName], [CorpusName])" +
                                      "VALUES('" + document + "', '" + author + "', '" + admin + "', " + IsCorrect + ", '" + new_author + "', '" + corpus + "')";
                command.ExecuteNonQuery();

                //check if insert succeeded
                command.CommandText = "SELECT * FROM [Feedback] WHERE StrComp([DocumentName],'" + document + "',1)= 0 " +
                                      "AND StrComp([AuthorName],'" + author + "',1)= 0 " +
                                      "AND StrComp([AdminName],'" + admin + "',1)= 0" +
                                      "AND [IsCorrect] = " + IsCorrect + " AND StrComp([NewAuthorName],'" + new_author + "',1)= 0" +
                                      "AND StrComp([CorpusName],'" + corpus + "',1)= 0 "; 
                addFeedback_reader = command.ExecuteReader();
                count = 0;
                while (addFeedback_reader.Read())
                {
                    count++;
                }
                if (count == 0)
                {
                    MessageBox.Show("Failed");
                    connection.Close();
                    return;
                }
                ShowSuccessMsg();
                connection.Close();
            }
            // update feedback
            else
            {
                command.CommandText = "UPDATE [Feedback] SET [IsCorrect] = " + IsCorrect + ", [NewAuthorName] = '" + new_author + "' " +
                                      "WHERE [ID] = " + record_id;
                command.ExecuteNonQuery();

                //check if insert succeeded
                command.CommandText = "SELECT * FROM [Feedback] WHERE StrComp([DocumentName],'" + document + "',1)= 0 " +
                                      "AND StrComp([AuthorName],'" + author + "',1)= 0 " +
                                      "AND StrComp([AdminName],'" + admin + "',1)= 0" +
                                      "AND [IsCorrect] = " + IsCorrect + " AND StrComp([NewAuthorName],'" + new_author + "',1)= 0" +
                                      "AND StrComp([CorpusName],'" + corpus + "',1)= 0 "; 
                addFeedback_reader = command.ExecuteReader();
                count = 0;
                while (addFeedback_reader.Read())
                {
                    count++;
                }
                if (count == 0)
                {
                    MessageBox.Show("Failed");
                    connection.Close();
                    return;
                }
                ShowSuccessMsg();
                connection.Close();
            }

        }

        private void ShowSuccessMsg()
        {
            MsgBox msg = new MsgBox();
            msg.Title = "SUCCESS";
            msg.text.Content = "Feedback was updated in the system.";
            msg.Show();
        }
    }
}
