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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.Properties["CurrentCorpus"] = "";
            Application.Current.Properties["CurrentUserName"] = "";
            Application.Current.Properties["CurrentUserType"] = "";
            Application.Current.Properties["CurrentRunTime"] = "";
            Application.Current.Properties["CurrentFeaturePercentage"] = "";
            Application.Current.Properties["CurrentMinVariance"] = "";
        }

        public void Enable_btn_login()
        {
            btn_login.Visibility = System.Windows.Visibility.Visible;
            btn_exit.Visibility = System.Windows.Visibility.Visible;
            btn_change_settings.Visibility = System.Windows.Visibility.Hidden;
            btn_add_corpus.Visibility = System.Windows.Visibility.Hidden;
            btn_add_user.Visibility = System.Windows.Visibility.Hidden;
            btn_Identify_document.Visibility = System.Windows.Visibility.Hidden;
            btn_view_report.Visibility = System.Windows.Visibility.Hidden;
        }

        public void Enable_btn_user()
        {
            Enable_btn_login();
            btn_Identify_document.Visibility = System.Windows.Visibility.Visible;
            btn_view_report.Visibility = System.Windows.Visibility.Visible;
            btn_change_settings.Visibility = System.Windows.Visibility.Visible;
        }

        public void Enable_btn_admin()
        {
            Enable_btn_user();
            btn_add_corpus.Visibility = System.Windows.Visibility.Visible;
            btn_Identify_document.Visibility = System.Windows.Visibility.Visible;
            btn_view_report.Visibility = System.Windows.Visibility.Visible;
        }

        public void Enable_btn_SA()
        {
            Enable_btn_admin();
            btn_add_user.Visibility = System.Windows.Visibility.Visible;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Hide();
        }

        private void ChangeSettings_Click(object sender, RoutedEventArgs e)
        {
            ChangeSettings changeSettingsWindow = new ChangeSettings();
            changeSettingsWindow.Show();
            this.Hide();
        }

        private void IdentifyDocument_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            IdentifyDocument identifyDocumentWindow = new  IdentifyDocument();
        }

        private void AddCorpus_Click(object sender, RoutedEventArgs e)
        {
            AddOrChangeCorpus addOrChangeCorpusWindow = new AddOrChangeCorpus();
            addOrChangeCorpusWindow.Show();
            this.Hide();
        }

        private void ViewReports_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ViewReports viewReportWindow = new ViewReports();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBoxResult result = MessageBox.Show("Are You Sure?", "Author Detector", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUser addUserWindow = new AddUser();
            addUserWindow.Show();
            this.Hide();
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            switch (Application.Current.Properties["CurrentUserType"].ToString())
            {
                case "SA":
                    this.Enable_btn_SA();
                    break;
                case "A":
                    this.Enable_btn_admin();
                    break;
                case "R":
                    this.Enable_btn_user();
                    break;
                default:
                    this.Enable_btn_login();
                    break;
            }
        }

    }
}
