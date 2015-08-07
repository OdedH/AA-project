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
using System.ComponentModel;
using System.IO;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for ChangeSettings.xaml
    /// </summary>
    /// 


    public class ChangeSettingsModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string m_lastCorpusName;

        private string m_featureValue;

        public string FeatureValue
        {
            get { return m_featureValue; }
            set
            {
                m_featureValue = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("FeatureValue"));
                }
            }
        }

        private string m_varianceValue;

        public string VarianceValue
        {
            get { return m_varianceValue; }
            set
            {
                m_varianceValue = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("VarianceValue"));
                }
            }
        }
    }

    public partial class ChangeSettings : Window
    {
        private OleDbConnection connection = new OleDbConnection();
        private ChangeSettingsModel model = new ChangeSettingsModel();
        private bool isLearn = false;
        private int classifierNum = -1;

        public ChangeSettings()
        {
            InitializeComponent();
            this.DataContext = model;
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\Database.accdb;
            Persist Security Info=False";
            connection.Open();

            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * From [AdminCorpusSettings] Where AdminName='" + Application.Current.Properties["CurrentUserName"] + "' " +
                                  "AND [CorpusName]= '" + Application.Current.Properties["CurrentCorpus"] + "'";
            OleDbDataReader line_reader = command.ExecuteReader();
            if (line_reader.HasRows)
            {
                line_reader.Read();
                model.m_lastCorpusName = line_reader.GetString(1);
                slider_Features.Value = line_reader.GetInt32(2) / 10.0;
                slider_Variance.Value = line_reader.GetInt32(3) / 10.0;
                if (!line_reader.GetBoolean(4))
                {
                    ComboBox_RunTime.Text = "Slow";
                }
                else
                {
                    ComboBox_RunTime.Text = "Fast";
                }
                if (line_reader.GetInt32(2) == 0)
                {
                    slider_Features.Value = 0.00001;
                }
                if (line_reader.GetInt32(3) == 0)
                {
                    slider_Variance.Value = 0.00001;
                }
            }
            else
            {
                slider_Features.Value = 10;
                slider_Variance.Value = 8;
            }
            line_reader.Close();
            if (Application.Current.Properties["CurrentUserType"].ToString() == "R")
            {
                ComboBox_RunTime.IsEnabled = false;
                slider_Features.IsEnabled = false;
                slider_Variance.IsEnabled = false;
                btn_Learn.Visibility = System.Windows.Visibility.Hidden;
            }
            model.m_lastCorpusName = Application.Current.Properties["CurrentCorpus"].ToString();
        }

        private void comboBoxCorpusNames(object sender, RoutedEventArgs e)
        {
            List<string> data = new List<string>();
            var comboBox = sender as ComboBox;
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM [Corpus] ORDER BY [CorpusName]";
            OleDbDataReader corpusNames_reader = command.ExecuteReader();
            int corpusSelectedIndex = 0;
            while (corpusNames_reader.Read())
            {
                string value = corpusNames_reader.GetString(0);
                data.Add(value);
                if (model.m_lastCorpusName == value)
                {
                    comboBox.SelectedIndex = corpusSelectedIndex;
                }
                corpusSelectedIndex++;
            }
            comboBox.ItemsSource = data;
            if (Application.Current.Properties["CurrentCorpus"].ToString() == "")
            {
                comboBox.SelectedIndex = 0;
            }
        }

        private void Slider_FeatureValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // ... Get Slider reference.
            var slider = sender as Slider;
            // ... Get Value.
            double value = slider.Value * 10;
            // ... Set Window Title.
            string valueStr = value.ToString("00") + "%";
            model.FeatureValue = valueStr;
            this.isLearn = false;
            this.classifierNum = -1;
        }

        private void Slider_VarianceValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // ... Get Slider reference.
            var slider = sender as Slider;
            // ... Get Value.
            double value = slider.Value * 10;
            // ... Set Window Title.
            string valueStr = value.ToString("00") + "%";
            model.VarianceValue = valueStr;
            this.isLearn = false;
            this.classifierNum = -1;
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
            connection.Close();
            this.Close();
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Properties["CurrentUserType"].ToString() != "R")
            {
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * From [AdminCorpusSettings] " +
                                      "WHERE AdminName='" + Application.Current.Properties["CurrentUserName"] + "' " +
                                      "AND CorpusName='" + ComboBox_Corpus.Text + "'";
                OleDbDataReader line_reader = command.ExecuteReader();
                bool userHasSettings = line_reader.HasRows;
                DateTime myDateTime = DateTime.Now;
                string sqlFormattedDate = myDateTime.ToString("dd-MM-yyyy");
                if (userHasSettings)
                {
                    // classifier exists
                    if (line_reader.Read())
                    {
                        if (line_reader.GetInt32(2) == Convert.ToInt32(model.FeatureValue.Remove(model.FeatureValue.Length - 1)) &&
                            line_reader.GetInt32(3) == Convert.ToInt32(model.VarianceValue.Remove(model.VarianceValue.Length - 1)) &&
                            line_reader.GetBoolean(4) == (ComboBox_RunTime.Text == "Fast") &&
                            line_reader.GetBoolean(5) == true && line_reader.GetInt32(7) != -1) { }

                        else
                        {
                            command.CommandText = "UPDATE [AdminCorpusSettings]" +
                                                  " SET FeaturePrecentege=" + model.FeatureValue.Remove(model.FeatureValue.Length - 1) +
                                                  " , MinVariance=" + model.VarianceValue.Remove(model.VarianceValue.Length - 1) +
                                                  " , Fast=" + (ComboBox_RunTime.Text == "Fast") +
                                                  " , UpdateDate=" + sqlFormattedDate +
                                                  " , IsLearn=" + this.isLearn +
                                                  " , Classifier=" + this.classifierNum +
                                                  " Where AdminName='" + Application.Current.Properties["CurrentUserName"] + "' " +
                                                  "AND [CorpusName]= '" + ComboBox_Corpus.Text + "'";
                        }
                    }
                }
                else
                {
                    command.CommandText = "INSERT INTO [AdminCorpusSettings] (AdminName, CorpusName, FeaturePrecentege, " +
                                                                              "MinVariance, Fast, UpdateDate, IsLearn, Classifier) " +
                                           "VALUES ('" + Application.Current.Properties["CurrentUserName"] +
                                           "' , '" + ComboBox_Corpus.Text +
                                           "' , '" + model.FeatureValue.Remove(model.FeatureValue.Length - 1) +
                                           "' , '" + model.VarianceValue.Remove(model.VarianceValue.Length - 1) +
                                           "' , " + (ComboBox_RunTime.Text == "Fast") +
                                           " , " + sqlFormattedDate + 
                                           " , " + this.isLearn +
                                           ", " + this.classifierNum + ")";
                }
                line_reader.Close();
                command.ExecuteNonQuery();
                connection.Close();
            }
            Application.Current.Properties["CurrentCorpus"] = ComboBox_Corpus.Text;
            Application.Current.Properties["CurrentRunTime"] = ComboBox_RunTime.Text;
            Application.Current.Properties["CurrentFeaturePercentage"] = model.FeatureValue.Remove(model.FeatureValue.Length - 1);
            Application.Current.Properties["CurrentMinVariance"] = model.VarianceValue.Remove(model.VarianceValue.Length - 1);
            Application.Current.MainWindow.Show();
            this.Close();
        }

        private void btn_Learn_Click(object sender, RoutedEventArgs e)
        {
            // Set Parameters
            string corpus_path = "./Data/" + ComboBox_Corpus.Text;
            int speed = -1;
            if (ComboBox_RunTime.Text == "Fast")
            {
                speed = 0;
            }
            else
            {
                speed = 1;
            }
            int feature_percent = Convert.ToInt32(model.FeatureValue.Remove(model.FeatureValue.Length - 1));
            double feature_var = Convert.ToDouble(model.VarianceValue.Remove(model.VarianceValue.Length - 1)) / 100.0;

            string parameters = "build_classifier " + System.IO.Path.GetFullPath(corpus_path) + " " + speed.ToString() + " " + feature_percent.ToString() + " " + feature_var.ToString();
            string result = Utils.run_cmd(System.IO.Path.GetFullPath("./Scripts/API.py"), parameters);
            this.isLearn = true;
            if (result != "NULL")
            {
                try
                {
                    this.classifierNum = Convert.ToInt32(result);
                }
                catch (Exception exception)
                {
                    MsgBox msg = new MsgBox();
                    msg.Title = "ERROR";
                    msg.text.Content = exception.Message;
                    msg.Show();
                    msg.Topmost = true;
                    return;
                }
            }
            btn_Save_Click(sender, e);
        }

        private void ComboBox_RunTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.isLearn = false;
            this.classifierNum = -1;
        }

        private void ComboBox_Corpus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.isLearn = false;
            this.classifierNum = -1;
            if (Application.Current.Properties["CurrentUserType"].ToString() != "R")
            {
                LoadSettingsForCorpus(Application.Current.Properties["CurrentUserName"].ToString(), ComboBox_Corpus.SelectedItem.ToString());
            }
        }

        private void LoadSettingsForCorpus(string admin, string corpus)
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * From [AdminCorpusSettings] WHERE [AdminName]='" + admin + "' " +
                                  "AND [CorpusName]= '" + corpus + "'";
            OleDbDataReader line_reader = command.ExecuteReader();
            if (line_reader.HasRows)
            {
                line_reader.Read();
                model.m_lastCorpusName = line_reader.GetString(1);
                slider_Features.Value = line_reader.GetInt32(2) / 10.0;
                slider_Variance.Value = line_reader.GetInt32(3) / 10.0;
                if (!line_reader.GetBoolean(4))
                {
                    ComboBox_RunTime.Text = "Slow";
                }
                else
                {
                    ComboBox_RunTime.Text = "Fast";
                }

                if (line_reader.GetInt32(2) == 0)
                {
                    slider_Features.Value = 0.00001;
                }
                if (line_reader.GetInt32(3) == 0)
                {
                    slider_Variance.Value = 0.00001;
                }
            }
            else
            {
                ComboBox_RunTime.Text = "Fast";
                slider_Features.Value = 10;
                slider_Variance.Value = 8;
            }
            line_reader.Close();
        }
    }
}
