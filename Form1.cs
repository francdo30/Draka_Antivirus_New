using System.ComponentModel;
using Microsoft.VisualBasic.FileIO;
using FileSystem = Microsoft.VisualBasic.FileIO.FileSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace ScanAutomatique
{
    public partial class Form1 : Form
    {
        private System.Threading.ManualResetEvent _busy = new System.Threading.ManualResetEvent(false);
        public static string targetPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string name_db = "ScanDataBase.db";
        public static string sourceFile = ""; //targetPath + name_db;
        Database db1 = new Database();
        public static string pathSignature = targetPath + "Test.txt";
        Color[] colors = { Color.Aqua, Color.Green, Color.Blue, Color.Black, Color.DeepSkyBlue, Color.Red };

        //string path = targetPath + @"C:\Users\maboa\OneDrive\Documents\Visual Studio 2019\Projects\drakashield-av\Draka Antivirus\bin\Debug\Error_Log.txt";
        string path = targetPath + "Error_Log.txt";

        String obt = "Pause";
        int j;  
        int count;
        int i;       
        int virus;
        int files;
        int compte;
        string cheminUSB;
        public Form1(string str)
        {
            InitializeComponent();
            this.cheminUSB = str;
            //MessageBox.Show("valeur : " + str);
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            progressBar1.Minimum = 0; 
            if(backgroundWorker1.IsBusy != true )
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        // nos fonction utiles
        private string BytesToHex(byte[] bytes)
        {
            // write each byte as two char hex output.
            return String.Concat(Array.ConvertAll(bytes, x => x.ToString("X2")));
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
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
            //  MessageBox.Show("rerererererere  : " + cheminUSB);
            List<String> search = new List<String>();
            try
            {
                string cheminement = cheminUSB; //@"D:\";
                search.AddRange(fichiers(cheminement));
                
            }catch(Exception ex)
            {               
                Console.WriteLine("Exception : "+ex.Message);
            }

            try
            {
                progressBar1.Invoke(new MethodInvoker(delegate
                {
                    progressBar1.Maximum = search.Count;
                }));
            }catch(Exception ex)
            {
                MessageBox.Show("Exception : "+ex.Message);
            }
            Scancp perso = new Scancp();
            perso.filesCount = search.Count;

            int max = search.Count;
            int Tempmax = max;            

            if(max == 0)
            {
                progressBar1.Maximum = 100;
                
                search.Add(cheminUSB);                
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
                        else if (button3.Text == "Continuer")
                        {
                            do
                            {
                                Thread.Sleep(500);
                            } while (button3.Text == "Continuer");
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
                            // FICHIER VIRUS POUR TEST.

                            /*using (StreamWriter sw = new StreamWriter(pathSignature))
                            {
                                sw.WriteLine(BytesToHex(hash)); 
                                sw.NewLine = "\n";
                                sw.Close(); 
                            }*/
                            File.AppendAllText(pathSignature, BytesToHex(hash) + Environment.NewLine);


                            if (md5signatures.Contains(BytesToHex(hash)))
                            {
                                perso.statut = "Infected";
                                virus += 1;
                                perso.virus = virus;
                                string detection = BytesToHex(hash);
                                AutoClosingMessageBox.Show("Menace détecté : " + detection);
                                MoveItem(chemin, perso.statut, detection);
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
                    try
                    {
                        progressBar1.Invoke(new MethodInvoker(delegate
                        {
                            progressBar1.Value = progressBar1.Maximum ;                            
                        }));                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    perso.item = item;
                    DateLine(max, perso, Tempmax);
                    Tempmax = Tempmax - 1;
                }
            }
            // action effectuer si le retour du chemin n'est pas vide
            else
            {
                //MessageBox.Show("sokotoooooo");
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
                        else if (button3.Text == "Continuer")
                        {
                            do
                            {
                                Thread.Sleep(500);
                            } while (button3.Text == "Continuer");
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
                            // FICHIER VIRUS POUR TEST.

                            /*using (StreamWriter sw = new StreamWriter(pathSignature))
                            {
                                sw.WriteLine(BytesToHex(hash)); 
                                sw.NewLine = "\n";
                                sw.Close(); 
                            }*/
                            File.AppendAllText(pathSignature, BytesToHex(hash) + Environment.NewLine);


                            if (md5signatures.Contains(BytesToHex(hash)))
                            {
                                perso.statut = "Infected";
                                virus += 1;
                                perso.virus = virus;
                                string detection = BytesToHex(hash);
                                AutoClosingMessageBox.Show("Menace détecté : " + detection);
                                MoveItem(chemin, perso.statut, detection);
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
                    try
                    {
                        progressBar1.Invoke(new MethodInvoker(delegate
                        {
                            progressBar1.Increment(1);
                        }));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    perso.item = item;
                    DateLine(max, perso, Tempmax);
                    Tempmax = Tempmax - 1;
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;
            MessageBox.Show("appel :");

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Scan Cancel");
                PartialScan("Interrompu");
            }
            else if (e.Error != null)
            {
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine(DateTime.Now.ToString() + " " + "Error_Message:" + e.Error.ToString());
                tw.Close();
                progressBar1.Value = 0;
                label9.Text = "";                
                MessageBox.Show("Scan Cancel or paused");
                labelDirectory.Text = cheminUSB;
                PartialScan("Interrompu");
            }
            else
            {
                Thread.Sleep(50);
                label8.ForeColor = colors[1];                
                label8.Text = "Processing Completed...";
                folderBrowserDialog1.SelectedPath.Trim();
                PartialScan("Terminee");
                labelDirectory.Text = cheminUSB;
                AutoClosingMessageBox.Show("Scan Complete");
                this.Hide();
                //MessageBox.Show("folder : " + compte);
                //RapportScan rapport = new RapportScan(files, compte, virus);
                RapportScan rapport = new RapportScan(files, compte, virus);
                rapport.StartPosition = FormStartPosition.Manual;
                rapport.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - rapport.Width,
                                       Screen.PrimaryScreen.WorkingArea.Height - rapport.Height);

                rapport.Show();                
                AutoClosingMessageBox.Show("rapport de scan ouvert");
                Thread.Sleep(10000);
                rapport.Close();
            }
           
            labelDirectory.Text = cheminUSB;            
        }

        private void DateLine(int max, Scancp scan, int Tempmax)
        {
            int scanend, intervale, time, days, hours, minutes, seconds;

            //scanend = scan.file; increlenter les secondes puis les minutes, puis les heures
            scanend = Tempmax;  //increlenter les secondes puis les minutes, puis les heures

            j = max / 100;

            intervale = 1;
            time = (scanend * intervale) / 4;
            days = time / 86400;
            hours = time / 3600;
            minutes = time / 60 % 60;
            seconds = time % 60;
            // **************************************************
            if (label9.InvokeRequired)
            {
               label9.Invoke(new MethodInvoker(delegate
                {
                    label8.ForeColor = colors[2];
                    label8.Text = "Processing Completed...";
                    labelDirectory.Text = cheminUSB;

                    count = count + 1;
                    Console.WriteLine(count);
                    if (max < 100)
                    {
                        if (count < (max - (max%2)))
                        {
                            label9.Text = i.ToString() + "%";
                            i += 5;
                            if (i > 95)
                            {
                                i = 99;
                            }
                        }                          
                        else
                        {
                            i = 100;
                            label9.Text = i.ToString() + "%";
                        }
                    }
                    else
                    {
                        label9.Text = i.ToString() + "%";
                        if (count % j == 0)
                        {
                            i = i + 1;
                            if (count < j * 100)
                            {
                                label9.Text = i.ToString() + "%";
                            }
                            else
                            {
                                if (count == max - 2)
                                {
                                    i = 100;
                                    label9.Text = i.ToString() + "%";
                                }
                                else
                                {
                                    i = 99;
                                    label9.Text = i.ToString() + "%";
                                }
                            }
                        }
                    }
                    //MessageBox.Show(scan.filesCount.ToString() );                                    
                    // MessageBox.Show("aaaaaaa");
                }));
            }
            else
            {
                //listViewItem = data.value;
                //listView1.Items.Add(scan.item);
            }

            // **************************************************

        }
        private void PartialScan(string Etat)
        {
           /* try
            {
                Boolean checktable = db1.CreateTable(sourceFile, "FullScan");

                if (checktable == true)
                {
                    MessageBox.Show("table FullScan créer ");
                }
                else
                {
                    MessageBox.Show("la table FullScan Existe deja ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("la table FullScan genere une exception " + ex);
            }*/
            string etat = Etat;
            DateTime date = DateTime.Now;
            string date1 = date.ToString("yyyy:MM:dd  hh:mm:ss");
            //string duree = label6.Text;
            string totalAnalyser = "";
            //if (label4.Text.Contains("Fichiers scanner : ")) totalAnalyser = label4.Text.Replace("Fichiers scanner : ", "");
            string virus = "";
            //if (label2.Text.Contains(" virus détectées")) virus = label2.Text.Replace(" virus détectées", "");
            string scantype = "Scan Partiel";
            string status = "Fait";
            /*string sql = "insert into FullScan  (etat,date,duree,totalanalyser,virusdetect,typescan,status) values(";
            sql = sql + "'" + etat + "', ";
            sql = sql + "'" + date1 + "', ";
            sql = sql + "'" + duree + "', ";
            sql = sql + "'" + totalAnalyser + "', ";
            sql = sql + "'" + virus + "', ";
            sql = sql + "'" + scantype + "', ";
            sql = sql + "'" + status + "')";*/


            /*try
            {
                Boolean error = db1.insertData(sourceFile, sql);

                if (error == true)
                {
                    Console.WriteLine("Good Scan");
                }
                else
                {
                    Console.WriteLine("Bad no complete scan");
                }
            }
            catch (Exception ex)
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                    TextWriter tw = new StreamWriter(path, true);
                    tw.WriteLine(DateTime.Now.ToString() + " " + "Request:" + " " + sql + " " + "Error_Message:" + ex);
                    tw.Close();
                }

                else if (File.Exists(path))
                {
                    TextWriter tw = new StreamWriter(path, true);
                    tw.WriteLine(DateTime.Now.ToString() + " " + "Request:" + " " + sql + " " + "Error_Message:" + ex);
                    tw.Close();
                }
            }*/
        }
        private void MoveItem(string directory, string etat, string hash)
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
                       /* try
                        {
                            Boolean checktable = db1.CreateTable(sourceFile, "Quarantaine");

                            if (checktable == true)
                            {
                                MessageBox.Show("table quarantaine créer ");
                            }
                            else
                            {
                                MessageBox.Show("la table quarantaine Existe deja ");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("la table quarantaine genere une exception " + ex);
                        }*/

                        string chemin = directory;
                        string fileName = file;
                        string NewDirectory = root;
                        DateTime date = DateTime.Now;
                        string date1 = date.ToString("dddd, dd MMMM yyyy");
                        string taille = " ";
                        string Etat = etat;
                        string editeur = " ";
                        string action = " Mettre en quarantaine ";
                        string detection = file + " -> " + hash;

                        string sql = "insert into Quarantaine (chemin,nomfichier,nouveldirection,date,taille,etat,editeur,action,detection) values(";
                        sql = sql + "'" + chemin + "', ";
                        sql = sql + "'" + fileName + "', ";
                        sql = sql + "'" + NewDirectory + "', ";
                        sql = sql + "'" + date1 + "', ";
                        sql = sql + "'" + taille + "', ";
                        sql = sql + "'" + Etat + "', ";
                        sql = sql + "'" + editeur + "', ";
                        sql = sql + "'" + action + "', ";
                        sql = sql + "'" + detection + "')";

                        /*try
                        {
                            Boolean error = db1.insertData(sourceFile, sql);
                            if (error == true)
                            {
                                //MessageBox.Show("Virus detectees et enregistrer en quarantaine");
                                //AutoClosingMessageBox.Show("Virus detectees et enregistrer en quarantaine", " Draka Quarantaine ", 3000);
                            }
                            else
                            {
                                Console.WriteLine("Virus detectees", " Draka Quarantaine ", 3000);
                                MessageBox.Show("Quarantaine échoué");
                                //AutoClosingMessageBox.Show("Quarantaine échoué");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("exception : " + ex);
                        }*/


                        FileSystem.MoveFile(directory, root, true);
                        AutoClosingMessageBox.Show("Threat eliminated");
                    }
                    else
                    {
                        Directory.CreateDirectory(subdir);                        
                        FileSystem.MoveFile(directory, root, true);                       
                        AutoClosingMessageBox.Show("Quarantine failed and file deleted");
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

        private void button1_Click_1(object sender, EventArgs e)
        {            
            labelDirectory.Text = cheminUSB;
            i = 0;
            j = 0;
            count = 0;
            progressBar1.Value = 0;
            label9.Text = " % ";

            virus = 0;
            files = 0;
                       
            Thread.Sleep(100);
            label8.Text = "initiailisations... ";            
        }

        
        private void button3_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                if (obt == "Continuer")
                {
                    obt = "Pause";
                    button3.Text = obt;
                    button3.Visible = true;

                }
                else
                {
                    obt = "Continuer";
                    button3.Text = obt;
                    button3.Visible = true;

                }
            }

        }      

        public List<string> fichiers(string dir)
        {
            //MessageBox.Show(" : ttttttttttttttt ");
            string root = @"C:\Program Files(x86)\Default Company Name\Setup1\Quarantaine\";

            List<string> f = new List<string>();
            try
            {
                List<string> dirs = Directory.GetDirectories(dir.ToString()).ToList();
                
                if (dirs.Count > 0)
                {
                    compte = compte + 1;                   
                    foreach (string item in dirs)
                    {
                        //MessageBox.Show(" : "+item);
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
                //MessageBox.Show("Exception rrrrrrrrrrrr : " + ex.Message);
                Console.WriteLine("Exception : " + ex.Message);
            }
            try
            {                
                f.AddRange(Directory.GetFiles(dir.ToString(), "*.*", System.IO.SearchOption.TopDirectoryOnly).ToList());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                Console.WriteLine(ex.ToString());
                //MessageBox.Show("Exception iiiiiiiiiiiii : " + ex.Message);
            }
            
            return f;
        }

        
    }   
}
