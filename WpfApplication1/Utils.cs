using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class Utils
    {
        public static string run_cmd(string cmd, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.WorkingDirectory = Path.GetDirectoryName(cmd);
            start.FileName = "C:\\Users\\mguttman\\AppData\\Local\\Continuum\\Anaconda\\python.exe";
            start.Arguments = string.Format("{0} {1}", cmd, args);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            try
            {
                using (Process process = Process.Start(start))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        if (args.Split(new string[] { " " }, StringSplitOptions.None).Length > 3)
                        {
                            string[] lines = result.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                            MsgBox msg = new MsgBox();
                            msg.Title = "INFO";
                            msg.text.Content = string.Join(Environment.NewLine, lines.Take(lines.Length - 2));
                            msg.Show();
                            msg.Topmost = true;
                            return lines[lines.Length - 2];
                        }
                        else
                        {
                            return result;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MsgBox msg = new MsgBox();
                msg.Title = "ERROR";
                msg.text.Content = "There is a problem in \nthe directory structure \n" +
                                   "or in the learning process.\n";
                msg.Show();
                msg.Topmost = true;
                return "NULL";
            }
        }
    }
}
