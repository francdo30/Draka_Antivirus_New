using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScanAutomatique
{
    public partial class UsbInterface : Form
    {
        private System.Threading.ManualResetEvent _busy = new System.Threading.ManualResetEvent(false);
        public static string targetPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string name_db = "ScanDataBase.db";
        public static string path = "";
        public static string sourceFile = targetPath + "MD5Base.txt";
        string db1 = "rr";
        Color[] colors = { Color.Aqua, Color.Green, Color.Blue, Color.Black, Color.DeepSkyBlue, Color.Red };

        // nous sommes dans la classe Scan qui sera auto appelé dans le constructeur        

        private const int WM_DEVICECHANGE = 0x219;
        private const int DBT_DEVICEARRIVAL = 0x8000;
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        private const int DBT_DEVTYP_VOLUME = 0x00000002;
        //private bool isVisibleCore = false;
        public UsbInterface()
        {
            InitializeComponent();
            MessageBox.Show("Click sur okey pour déclencher le scan automatique ");
            ScanTotal scanT = new ScanTotal();
            
            //SetVisibleCore(isVisibleCore);
            /*this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width,
                                   Screen.PrimaryScreen.WorkingArea.Height - this.Height);*/
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width ,
                                   Screen.PrimaryScreen.WorkingArea.Height );
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case WM_DEVICECHANGE:
                    switch ((int)m.WParam)
                    {
                        case DBT_DEVICEARRIVAL:
                            //listBox1.Items.Add("New Device Arrived");
                            //MessageBox.Show("Clé branché, En attente du lancement du Scan Auto");
                            int devType = Marshal.ReadInt32(m.LParam, 4);
                            if (devType == DBT_DEVTYP_VOLUME)
                            {
                                DevBroadcastVolume vol;
                                vol = (DevBroadcastVolume)Marshal.PtrToStructure(m.LParam, typeof(DevBroadcastVolume));
                                /*listBox1.Items.Add("Mask is " + vol.Mask);
                                listBox1.Items.Add("Letter is " + GetLetter(vol.Mask));*/
                                path = GetLetter(vol.Mask).ToString() + @":\";
                                //MessageBox.Show("Chemin : " + path);
                                //File.Copy(sourceFile, path);
                                CheckRemove(path);
                            }
                            break;

                        case DBT_DEVICEREMOVECOMPLETE:
                            //listBox1.Items.Add("Device Removed");
                            MessageBox.Show("Clé débranché");
                            //CheckRemove(path);
                            break;
                    }
                    break;
            }
        }

        public void CheckRemove(string str)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady == true)
                {
                    double free, total;
                    free = d.TotalFreeSpace / Math.Pow(1024, 3);
                    total = d.TotalSize / Math.Pow(1024, 3);
                    //MessageBox.Show("ll : "+d);
                    string usb = d.Name;
                    
                    //MessageBox.Show("Total Free : " + free + " Total size : " + total);
                    if (usb.Equals(str))
                    {
                        Form1 DetectionUSB = new Form1(str);
                        DetectionUSB.StartPosition = FormStartPosition.Manual;
                        DetectionUSB.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - DetectionUSB.Width,
                                               Screen.PrimaryScreen.WorkingArea.Height - DetectionUSB.Height);
                        //DirectoryInfo lecteurUSB = new DirectoryInfo(d.RootDirectory.ToString());
                        //MessageBox.Show("Je suis la clé usb branché : "+ lecteurUSB.FullName);
                        //int Taille = (int)(total - free);
                        //MessageBox.Show("gggg : " + Taille);

                        // ici, on appel la méthode scan complet sur la clé connecté                        
                        DetectionUSB.Show();
                    }
                }


            }
            /*string tt = @"\Program Files\" + str;
            MessageBox.Show(tt);*/
        }

        // les sous méthodes

        public char GetLetter(int mask)
        {
            int ch = 0;
            for (; ch < 26; ch++)
            {
                if ((mask & 0x1) == 0x1)
                    break;
                mask >>= 1;
            }
            ch += 0x41;
            return (char)ch;
        }

        private void UsbInterface_Resize(object sender, System.EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }

        private void UsbInterface_Load(object sender, EventArgs e)
        {
           /* this.Visible = false;
            this.WindowState = FormWindowState.Normal;*/
           this.Hide(); 
        }
    }
}
