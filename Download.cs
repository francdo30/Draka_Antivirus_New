using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScanAutomatique
{
    public partial class Download : Form
    {
        private string Nomachine = Environment.UserName;
        private int i = 0;
        public Download()
        {
            InitializeComponent();
            Watcher();
        }

        void Watcher()
        {
            try
            {
                //MessageBox.Show("uuuuuuuuuuuuu");

                watcher.Path = @"C:\Users\"+ Nomachine +@"\Downloads";
                watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

                watcher.Filter = "*.*";
                watcher.Created += OnCreated;
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;
                //MessageBox.Show("oooooooooooooooooo");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            i = i + 1;
            //string path = @"C:\Users\" + MachineName + @"\Downloads";
            //MessageBox.Show("on m'a appelé ");
            string path = e.FullPath;
            string NamePath = Path.GetDirectoryName(path);
            //MessageBox.Show("rrrrrrrrrrrr : " + path);
            //MessageBox.Show("eeeeeeeeeeee : " + NamePath);
            //ScanDownloa d scan = new ScanDownload(watcher.Path);*/
            
            if (i == 1)
            {
                MessageBox.Show("Download complete !" + NamePath);

                Form1 DetectionUSB = new Form1(path);
                DetectionUSB.StartPosition = FormStartPosition.Manual;
                DetectionUSB.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - DetectionUSB.Width,
                                       Screen.PrimaryScreen.WorkingArea.Height - DetectionUSB.Height);

                DetectionUSB.Show();
            }
            else
            {
                MessageBox.Show("Scan en cours !!!!!!!");
                i = 0;
            }
        }

        private void Download_Load(object sender, EventArgs e)
        {
            // scan
        }
    }
}
