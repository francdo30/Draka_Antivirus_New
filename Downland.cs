using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanAutomatique
{
    internal class Downland
    {
        public Downland() { }

        public static void Watch()
        {
            try
            {
                MessageBox.Show("uuuuuuuuuuuuu");


                using (FileSystemWatcher watcher = new FileSystemWatcher())
                {
                    watcher.Path = @"C:\Users\Ragot_Prod\Downloads";
                    watcher.NotifyFilter = NotifyFilters.Attributes
                                     | NotifyFilters.CreationTime
                                     | NotifyFilters.DirectoryName
                                     | NotifyFilters.FileName
                                     | NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.Security
                                     | NotifyFilters.Size;
                    watcher.Filter = "*.*";
                    //watcher.Created += new FileSystemEventHandler(OnCreated);
                    watcher.Changed += new FileSystemEventHandler(OnChanged);
                    watcher.Created += new FileSystemEventHandler(OnChanged);

                    watcher.IncludeSubdirectories = true;
                    watcher.EnableRaisingEvents = true;

                    MessageBox.Show("ooooooooooooooo");
                    //watcher.SynchronizingObject = this;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void OnCreated(object sender, FileSystemEventArgs e)
        {
            //string path = @"C:\Users\" + MachineName + @"\Downloads";
            MessageBox.Show("on m'a appelé ");
            string path = e.FullPath;
            string NamePath = Path.GetDirectoryName(path);
            MessageBox.Show("rrrrrrrrrrrr : " + path);
            MessageBox.Show("eeeeeeeeeeee : " + NamePath);
            //ScanDownload scan = new ScanDownload(watcher.Path);
            MessageBox.Show("Download complete !" + path);
        }

        public static void OnChanged(object sender, FileSystemEventArgs e)
        {
            MessageBox.Show("on m'a appelé pppppppppppppppppppp ");
        }
    }
}
