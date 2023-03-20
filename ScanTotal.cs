using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using Timer = System.Windows.Forms.Timer;

namespace ScanAutomatique
{
    public partial class ScanTotal : Form
    {
        private System.Threading.ManualResetEvent _busy = new System.Threading.ManualResetEvent(false);
        public static string targetPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string name_db = "WanDataBase.db";
        public static string sourceFile = targetPath + name_db;
        public static string pathSignature = targetPath + "Test.txt";
        Database db1 = new Database();
        Color[] colors = { Color.Aqua, Color.Green, Color.Blue, Color.Black, Color.DeepSkyBlue, Color.Red };
        private Timer time;
        
        string path = targetPath + "Error_Log.txt";
        string path1 = targetPath + "FullScanDraka.txt";

        String obt = "Pause";
        int j;
        int count;
        int i;
        int virus;
        int files;
        public ScanTotal()
        {
            InitializeComponent();            
            backgroundWorkerScanTotal.WorkerReportsProgress = true;
            backgroundWorkerScanTotal.WorkerSupportsCancellation = true;
            InitTimer();
        }

        // la minuterie du scan automatique

        public void InitTimer()
        {
            time = new Timer();
            time.Tick += new EventHandler(timer1_Tick);
            time.Interval = 1080000; // in miliseconds 1080000 pour 30min, 7200000 pour 2h
            time.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!File.Exists(sourceFile))
            {
                try
                {
                    sourceFile = db1.createDatabase(name_db);
                }
                catch (Exception ex)
                {
                    AutoClosingMessageBox.Show("Exception géré" + ex.Message);
                }
            }
            else
            {
                //AutoClosingMessageBox.Show("Base de données Existe deja ");
                //MessageBox.Show("Base de données Existe deja ");
            }

            if (backgroundWorkerScanTotal.IsBusy != true)
            {
                count = 0;
                virus = 0;
                files = 0;
                backgroundWorkerScanTotal.RunWorkerAsync();
                AutoClosingMessageBox.Show("Outstanding Global Scan ");
            }
            AutoClosingMessageBox.Show("Automatic scan of your computer every 30 minutes");
        }

        // nos fonction utiles
        private string BytesToHex(byte[] bytes)
        {
            // write each byte as two char hex output.
            return String.Concat(Array.ConvertAll(bytes, x => x.ToString("X2")));
        }

        private void Dowork(object sender, DoWorkEventArgs e)
        {           
            // Do not access the form's BackgroundWorker reference directly.
            // Instead, use the reference provided by the sender parameter.
            BackgroundWorker bw = sender as BackgroundWorker;
            //RunWorkerCompletedEventArgs eh;
            // Extract the argument.
            //int arg = (int)e.Argument;

            // Start the time-consuming operation.
            //e.Result = 

            // If the operation was canceled by the user,
            // set the DoWorkEventArgs.Cancel property to true.        

            TimeConsumingOperation(bw, e);
        }

        private void TimeConsumingOperation(BackgroundWorker bw, DoWorkEventArgs e)
        {
            //MessageBox.Show("rerererererere  : ");
            List<String> search = new List<String>();
            try
            {
                string cheminement = @"C:\Users";//\" + Environment.UserName+ @"\Documents"; //@"D:\";
                search.AddRange(fichiers(cheminement));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.Message);
            }


            Scancp perso = new Scancp();
            perso.filesCount = search.Count;

            int max = search.Count;
            int Tempmax = max;

            foreach (String item in search)
            {
                try
                {
                    this.files += 1;
                    string chemin = item;

                    if (bw.CancellationPending == true)
                    {
                        //listView1.Visible = true;
                        e.Cancel = true;
                        break;
                    }
                    else
                    {
                        // read all bytes of file so we can send them to the MD5 hash algo

                        Byte[] allBytes = File.ReadAllBytes(item);
                        System.Security.Cryptography.HashAlgorithm md5Algo = null;
                        md5Algo = new System.Security.Cryptography.MD5CryptoServiceProvider();

                        // compute the Hash (MD5) on the bytes we got from the file
                        // compute the Hash (MD5) on the bytes we got from the file

                        byte[] hash = md5Algo.ComputeHash(allBytes);
                        Console.WriteLine(BytesToHex(hash));
                        //MessageBox.Show("Signature : " + BytesToHex(hash));
                        perso.file = files;
                        var md5signatures = File.ReadAllLines("MD5Base.txt");
                        if (md5signatures.Contains(BytesToHex(hash)))
                        {
                            perso.statut = "Infected";
                            virus += 1;
                            perso.virus = virus;
                            string detection = BytesToHex(hash);
                            AutoClosingMessageBox.Show("Nouvelle Ménace détecté : " + detection);
                            MoveItem(chemin, perso.statut, perso.virus, detection);
                        }
                        else
                        {
                            perso.statut = "Clean";
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                perso.item = item;
                //DateLine(max, perso, Tempmax);
                Tempmax = Tempmax - 1;
            }
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;
            MessageBox.Show("appel :");
        }

        private void RunWorkerComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Scan Cancel");
                //PartialScan("Interrompu");
            }
            else if (e.Error != null)
            {
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine(DateTime.Now.ToString() + " " + "Error_Message:" + e.Error.ToString());
                tw.Close();
                MessageBox.Show("Scan Cancel or paused");
                //PartialScan("Interrompu");
            }
            else
            {
                Thread.Sleep(50);                
                MessageBox.Show("Scan Complete");
                RapportScan rapport = new RapportScan(files, count, virus);
                rapport.StartPosition = FormStartPosition.Manual;
                rapport.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - rapport.Width,
                                       Screen.PrimaryScreen.WorkingArea.Height - rapport.Height);

                rapport.Show();
                AutoClosingMessageBox.Show("rapport de scan ouvert");
                Thread.Sleep(10000);
                rapport.Close();
                
            }
        }

        private void MoveItem(string directory, string etat, int taille, string hash)
        {
            //string subdir = @"D:\job\AGMA Organization technology inc\Draka new verison\Draka Antivirus\Draka Antivirus\Draka Antivirus\bin\Debug\Quarantaine\";
            //string root = @"D:\job\AGMA Organization technology inc\Draka new verison\Draka Antivirus\Draka Antivirus\Draka Antivirus\bin\Debug\Quarantaine\";
            //string subdir = @"C:\Program Files(x86)\Default Company Name\Setup1\Quarantaine\";
            string root = @"C:\Program Files(x86)\Default Company Name\Setup1\Quarantaine\";

            // test directory
            string subdir = @"Draka Antivirus\Draka Antivirus\Draka Antivirus\bin\Debug\Quarantaine\";
            //string root = @"Draka Antivirus\Draka Antivirus\Draka Antivirus\bin\Debug\Quarantaine\";*/

            try
            {
                string file = Path.GetFileName(directory);
                root = root + file;

                if (FileSystem.FileExists(directory))
                {
                    if (Directory.Exists(subdir))
                    {
                        /* MessageBox.Show("non il n'exixte pas");
                         Directory.CreateDirectory(subdir); */
                        try
                        {
                            Boolean checktable = db1.CreateTable(sourceFile, "Quarantaine");

                            if (checktable == true)
                            {
                                AutoClosingMessageBox.Show("table quarantaine créer ");
                            }
                            else
                            {
                                AutoClosingMessageBox.Show("la table quarantaine Existe deja ");
                            }
                        }
                        catch (Exception ex)
                        {
                            AutoClosingMessageBox.Show("la table quarantaine genere une exception " + ex);
                        }

                        // Ajout de ce scan à la base de données

                        DateTime date = DateTime.Now;
                        string date1 = date.ToString(" dddd, dd MMMM yyyy --> hh:mm:ss tt");
                        string statut = "Scan Total";
                        MessageBox.Show("valeur de virus : "+virus);
                        //string virus8 = "9";
                        string sql = "insert into Quarantaine (date,taille,directory,statut) values(";
                        sql = sql + "'" + date1 + "', ";
                        sql = sql + "'" + virus + "', ";
                        sql = sql + "'" + directory + "', ";
                        sql = sql + "'" + statut + "')";

                        try
                        {
                            Boolean error = db1.insertData(sourceFile, sql);
                           
                            if (error == true)
                            {
                                //MessageBox.Show("Virus detectees et enregistrer en quarantaine");
                                AutoClosingMessageBox.Show("Virus detectees et enregistrer en quarantaine", " Draka Quarantaine ", 3000);

                                // Ajout de ligne de la base de données à mon fichier FullScanDraka

                            }
                            else
                            {
                                Console.WriteLine("Virus detectees", " Draka Quarantaine ", 3000);
                                //MessageBox.Show("Quarantaine échoué");
                                AutoClosingMessageBox.Show("Quarantaine échoué");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("exception : " + ex);
                        }

                        // suppression du fichier

                        FileSystem.MoveFile(directory, root, true);
                        AutoClosingMessageBox.Show("Fichier Supprimé .");
                    }
                    else
                    {
                        Directory.CreateDirectory(subdir);
                        FileSystem.MoveFile(directory, root, true);
                    }

                }
                else
                {
                    Console.WriteLine("Monexeption = " + file);
                    Console.WriteLine("Le fichier n'existe pas dans le repertoire : " + directory);
                }
            }
            catch (Exception ex)
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                    TextWriter tw = new StreamWriter(path, true);
                    tw.WriteLine(DateTime.Now.ToString() + " " + "Error_Message:" + ex);
                    tw.Close();
                }
                else if (File.Exists(path))
                {
                    TextWriter tw = new StreamWriter(path, true);
                    tw.WriteLine(DateTime.Now.ToString() + " " + "Error_Message:" + ex);
                    tw.Close();
                }
            }
        }

        public List<string> fichiers(string dir)
        {
            string root = @"C:\Program Files(x86)\Default Company Name\Setup1\Quarantaine\";

            List<string> f = new List<string>();
            try
            {
                List<string> dirs = Directory.GetDirectories(dir.ToString()).ToList();
                // ici je prends la liste de repertoire à scanner
                //count = dirs.Count();

                if (dirs.Count > 0)
                {
                    count = count + 1;
                    foreach (string item in dirs)
                    {
                        try
                        {
                            f.AddRange(fichiers(item));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            MessageBox.Show(" Exeption généré ");                        
                            FileSystem.MoveFile(item, root, true);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.Message);       
            }
            try
            {
                f.AddRange(Directory.GetFiles(dir.ToString(), "*.*", System.IO.SearchOption.TopDirectoryOnly).ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());            
            }

            return f;
        }
    }
}
