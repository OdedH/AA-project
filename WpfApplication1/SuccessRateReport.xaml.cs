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
using System.Windows.Controls.DataVisualization;
using System.Windows.Controls.DataVisualization.Charting;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for SuccessRateReport.xaml
    /// </summary>
    public partial class SuccessRateReport : Window
    {
        public float[] ChartValues { get; set; }
        private OleDbConnection connection = new OleDbConnection();
        private Dictionary<string, AuthorFeedbacks> feedbacks = new Dictionary<string, AuthorFeedbacks>();
         
        public SuccessRateReport()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\Database.accdb;
            Persist Security Info=False";
            ReadDataFromDB(Application.Current.Properties["CurrentCorpus"].ToString());
            LoadColumnChartData();
        }

        private void LoadColumnChartData()
        {
            KeyValuePair<string,double>[] data = new KeyValuePair<string,double>[feedbacks.Count];
            int i=0;
            foreach (KeyValuePair<string,AuthorFeedbacks> feedback in feedbacks)
            {
                data[i] = new KeyValuePair<string, double>(feedback.Key,feedback.Value.correct_percentage*100);
                i++;
            }
            ((ColumnSeries)mcChart.Series[0]).ItemsSource = data;
        }

        private void ReadDataFromDB(string current_corpus)
        {
            connection.Open();
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;

            command.CommandText = "SELECT * FROM [Feedback] WHERE StrComp([CorpusName],'" + current_corpus + "',1)= 0 ";
            OleDbDataReader LoadFeedbacks_reader = command.ExecuteReader();

            int count = 0;
            while (LoadFeedbacks_reader.Read())
            {
                count += 1;
                string author = LoadFeedbacks_reader.GetValue(2).ToString();
                if (!feedbacks.ContainsKey(author))
                {
                    feedbacks.Add(author, new AuthorFeedbacks());
                }
                string isRight = LoadFeedbacks_reader.GetValue(4).ToString();
                if (isRight == "True")
                {
                    feedbacks[author].correct_feedbacks += 1;
                    feedbacks[author].correct_percentage = (double)feedbacks[author].correct_feedbacks / (feedbacks[author].correct_feedbacks + feedbacks[author].incorrect_feedbacks);
                }
                else 
                {
                    feedbacks[author].incorrect_feedbacks += 1;
                    feedbacks[author].correct_percentage = Convert.ToDouble(feedbacks[author].correct_feedbacks / (feedbacks[author].correct_feedbacks + feedbacks[author].incorrect_feedbacks));
                }
            }
            if (count == 0)
            {
                MessageBox.Show("No feedback available for this corpus.");
                connection.Close();
                return;
            }
            LoadFeedbacks_reader.Close();
            connection.Close();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        
    }

    class AuthorFeedbacks
    {
        public int correct_feedbacks { get; set; }
        public int incorrect_feedbacks { get; set; }
        public double correct_percentage { get; set; }

        public AuthorFeedbacks()
        {
            this.correct_feedbacks = 0;
            this.incorrect_feedbacks = 0;
            this.correct_percentage = 0;
        }

        public AuthorFeedbacks(int correct_feedbacks, int incorrect_feedbacks, float correct_percentage)
        {
            this.correct_feedbacks = correct_feedbacks;
            this.incorrect_feedbacks = incorrect_feedbacks;
            this.correct_percentage = correct_percentage;
        }
    }
}
