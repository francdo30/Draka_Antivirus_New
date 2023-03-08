using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanAutomatique
{
    internal class ScanDownload
    {    
        public ScanDownload(string path) 
        {
            Form1 maforme = new Form1(path);
            maforme.StartPosition = FormStartPosition.Manual;
            maforme.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - maforme.Width,
                                   Screen.PrimaryScreen.WorkingArea.Height - maforme.Height);

            maforme.Show();
        }
        public void Changmt(string str)
        {
            
        }

    }
}
