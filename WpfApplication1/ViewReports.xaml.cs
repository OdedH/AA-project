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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for ViewReports.xaml
    /// </summary>
    public partial class ViewReports : Window
    {
        public ViewReports()
        {
            InitializeComponent();
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

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
            this.Close();
        }

        private void Success_Rate_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            SuccessRateReport report = new SuccessRateReport();
            report.ShowDialog();
            //new SuccessRateReport().ShowDialog();
            ShowDialog();
        }

        private void System_Status_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            SystemStatusReport report = new SystemStatusReport();
            report.ShowDialog();
            ShowDialog();
        }
    }
}
