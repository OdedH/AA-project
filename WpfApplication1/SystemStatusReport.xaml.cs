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
    /// Interaction logic for SystemStatusReport.xaml
    /// </summary>
    public partial class SystemStatusReport : Window
    {
        public SystemStatusReport()
        {
            InitializeComponent();
            if (System.IO.File.Exists(System.IO.Path.GetFullPath("./Scripts/SystemStatusReport.png")))
            {
                Image_Input.Source = null;
                System.IO.File.Delete(System.IO.Path.GetFullPath("./Scripts/SystemStatusReport.png"));
            }
            CreateReport();
        }

        private void CreateReport()
        {
            // Set Parameters
            if (System.IO.File.Exists(System.IO.Path.GetFullPath("./Scripts/SystemStatusReport.png")))
            {
                Image_Input.Source = null;
                System.IO.File.Delete(System.IO.Path.GetFullPath("./Scripts/SystemStatusReport.png"));
            }
            string corpus_path = "./Data/" + Application.Current.Properties["CurrentCorpus"].ToString();

            string parameters = "build_histogram " + System.IO.Path.GetFullPath(corpus_path) + " SystemStatusReport";
            string result = Utils.run_cmd(System.IO.Path.GetFullPath("./Scripts/API.py"), parameters);
            if (result != "NULL")
            {
                try
                {

                    string filename = System.IO.Path.GetFullPath("./Scripts/SystemStatusReport.png");
                    BitmapImage image = new BitmapImage();

                    using (var stream =
                        new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                    {
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = stream;
                        image.EndInit();
                    }

                    Image_Input.Source = image;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    return;
                }
            }
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
