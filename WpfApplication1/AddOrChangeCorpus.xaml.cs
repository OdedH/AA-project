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
//using System.Windows.Shapes;
using System.IO;
using System.Data.OleDb;
//using WpfApplication1.UserDataSetTableAdapters;


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for AddOrChangeCorpus.xaml
    /// </summary>
    public partial class AddOrChangeCorpus : Window
    {
        private string DestinationPath = @".\Data";
        private OleDbConnection connection = new OleDbConnection();
        public AddOrChangeCorpus()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\Database.accdb;
            Persist Security Info=False";
            LoadDataForAddDocument(Application.Current.Properties["CurrentCorpus"].ToString());
        }

        private bool LoadDataForAddDocument(string current_corpus)
        {
            if (current_corpus == "")
            {
                MsgBox msg = new MsgBox();
                msg.Title = "INFO";
                msg.text.Content = "In order to add single documents \nplease choose corpus first via \n'Change Settings'.";
                msg.Show();
                msg.Topmost = true;

                author_combo.IsEnabled = false;
                btn_Add_Doc.IsEnabled = false;
                return false;
            }
            LoadAuthors(current_corpus);
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

            while (LoadAuthor_reader.Read())
            {
                string author = LoadAuthor_reader.GetValue(2).ToString();
                author_combo.Items.Add(author);
                count++;
            }
            if (count == 0)
            {
                MsgBox msg = new MsgBox();
                msg.Title = "INFO";
                msg.text.Content = "No existing authors available \nin this corpus.";
                msg.Show();
                msg.Topmost = true;
                connection.Close();
                author_combo.IsEnabled = false;
                btn_Add_Doc.IsEnabled = false;
                return;
            }
            author_combo.IsEnabled = true;
            btn_Add_Doc.IsEnabled = true;
            LoadAuthor_reader.Close();
            connection.Close();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
            this.Close();
        }

        private void Add_Corpus_Click(object sender, RoutedEventArgs e)
        {
            if (this.corpus_name.Text == "")
            {
                MsgBox msg = new MsgBox();
                msg.Title = "ERROR";
                msg.text.Content = "Please enter a corpus name.";
                msg.Show();
                return;
            }
            // insert corpus name to DB
            string corpus_name = this.corpus_name.Text;
            if (InsertCorpusToDB(corpus_name) == false)
            {
                return;
            }

            //Create OpenFolderDialog 
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            //Get the selected folder path
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string SourcePath = dialog.SelectedPath;

                foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                    SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(DestinationPath + "\\" + corpus_name);
                    Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath + "\\" + corpus_name));
                }

                HashSet<string> authors_hash = new HashSet<string>();
                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                    SearchOption.AllDirectories))
                {
                    string lastFolderName = Path.GetFileName(Path.GetDirectoryName(newPath));
                    authors_hash.Add(lastFolderName);
                    if (check_txt_file(newPath))
                    {
                        File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath + "\\" + corpus_name), true);
                    }
                }
                //insert authors to DB
                InsertAuthorsToDB(authors_hash.ToArray(), corpus_name);
                MsgBox success_msg = new MsgBox();
                success_msg.Title = "SUCCESS";
                success_msg.text.Content = corpus_name + " was added.";
                success_msg.Show();
            }
            Clear();

        }

        private bool check_txt_file(string file)
        {
            if (System.IO.Path.GetExtension(file) == ".txt")
            {
                return true;
            }
            else
            {
                MsgBox msg = new MsgBox();
                msg.Title = "ERROR";
                msg.text.Content = file + "\n was not added to the corpus.\n" +
                                "Corpus files must be .txt files.\n" +
                                "Please use a file converter \nif you wish to add this file.";
                msg.Show();
                msg.Topmost = true;
                return false;
            }
        }

        private bool InsertAuthorsToDB(string[] authors_hash, string corpus_name)
        {
            connection.Open();
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            foreach (string author in authors_hash)
            {
                //first check for no duplication
                command.CommandText = "SELECT * FROM [Author] WHERE StrComp([CorpusName],'" + corpus_name + "',1)= 0 " +
                                      "AND StrComp([AuthorName],'" + author + "',1)= 0";
                OleDbDataReader addAuthor_reader = command.ExecuteReader();
                int count = 0;
                while (addAuthor_reader.Read())
                {
                    count++;
                }
                if (count != 0)
                {
                    MsgBox msg = new MsgBox();
                    msg.Title = "ERROR";
                    msg.text.Content = "Author name '" + author + "' already exists.\nPlease fix file structure.";
                    msg.Show();
                    connection.Close();
                    return false;
                }
                addAuthor_reader.Close();

                //add the new corpus
                command.CommandText = "INSERT INTO [Author]  ([CorpusName], [AuthorName])" +
                                      "VALUES('" + corpus_name + "', '" + author + "')";
                command.ExecuteNonQuery();

                //check if insert succeeded
                command.CommandText = "SELECT * FROM [Author] WHERE StrComp([CorpusName],'" + corpus_name + "',1)= 0 " +
                                      "AND StrComp([AuthorName],'" + author + "',1)= 0";
                addAuthor_reader = command.ExecuteReader();
                count = 0;
                while (addAuthor_reader.Read())
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
                    return false;
                }
                addAuthor_reader.Close();
            }
            connection.Close();
            return true;
        }

        private bool InsertCorpusToDB(string corpus_name)
        {
            connection.Open();
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            //first check for no duplication
            command.CommandText = "SELECT * FROM [Corpus] WHERE StrComp([CorpusName],'" + corpus_name + "',1)= 0";
            OleDbDataReader addCorpus_reader = command.ExecuteReader();
            int count = 0;
            while (addCorpus_reader.Read())
            {
                count++;
            }
            if (count != 0)
            {
                MsgBox msg = new MsgBox();
                msg.Title = "ERROR";
                msg.text.Content = "Corpus name already exists";
                msg.Show();
                connection.Close();
                return false;
            }
            addCorpus_reader.Close();

            //add the new corpus
            command.CommandText = "INSERT INTO [Corpus]  ([CorpusName])" +
                                  "VALUES('" + corpus_name + "')";
            command.ExecuteNonQuery();

            //check if insert succeeded
            command.CommandText = "SELECT * FROM [Corpus] WHERE CorpusName = '" + corpus_name + "'";
            addCorpus_reader = command.ExecuteReader();
            count = 0;
            while (addCorpus_reader.Read())
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
                return false;
            }
            else
            {
                connection.Close();
                return true;
            }
        }

        private void Add_Document_Click(object sender, RoutedEventArgs e)
        {
            //check selected author
            if (author_combo.Text == "")
            {
                MsgBox msg = new MsgBox();
                msg.Title = "ERROR";
                msg.text.Content = "Please choose an author from the list.";
                msg.Show();
                return;
            }
            string author = author_combo.Text;
            string corpus = Application.Current.Properties["CurrentCorpus"].ToString();
            //Create OpenFileDialog 
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string src_path = openFileDialog.InitialDirectory + openFileDialog.FileName;
                string file_name = Path.GetFileName(openFileDialog.FileName);
                string targetPath = System.IO.Path.Combine(DestinationPath, corpus, author, file_name);
                if (check_txt_file(src_path))
                {
                    File.Copy(src_path, targetPath, true);
                    MsgBox success_msg = new MsgBox();
                    success_msg.Title = "SUCCESS";
                    success_msg.text.Content = file_name + " was added.";
                    success_msg.Show();
                }
            }
            Clear();
        }

        private void Clear()
        {
            corpus_name.Text = "";
            author_combo.Text = "";
        }

    }
}
