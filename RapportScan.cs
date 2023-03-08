using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScanAutomatique
{
    public partial class RapportScan : Form
    {
        private System.Threading.ManualResetEvent _busy = new System.Threading.ManualResetEvent(false);
        public static string targetPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string name_db = "WanDataBase.db";
        public static string sourceFile = targetPath + name_db;
        public static string pathSignature = targetPath + "Test.txt";
        Database db1 = new Database();
        private int i = 0;
        private int j = 0;
        private int k = 0;
        public RapportScan(int a, int b, int c)
        {
            InitializeComponent();
            this.i = a;
            this.j = b;
            this.k = c;
            Chargement();  // appel de la fonction de rapport scan
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // changement de couleur encours
        }

        public void Chargement()
        {
            DateTime date = DateTime.Now;
            string date1 = date.ToString("dddd, dd MMMM yyyy");
            //string time = DateTime.Now.ToString("h:mm:ss tt");
            string time = DateTime.UtcNow.ToString("hh:mm:ss tt");
            label6.Text  = i.ToString();     // 
            label8.Text  = j.ToString();    //  
            label10.Text = k.ToString();   //
            label11.Text = date1;         //  date heure scan
            label4.Text = time;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // ici il est question de supprimer les fichiers stockés dans la quarantaine
            Console.WriteLine("lune");
            // selectionner les données de la base de données
            string sql = "SELECT * FROM Quarantaine;";
            List<Object[]> liste = new List<Object[]>();
            try
            {
                liste = db1.selectDatas(sourceFile, sql);
            }
            catch(Exception ex) 
            { 
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.ToString());
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // ici on va faire la methode As safe, récupérer un fichier suprimé d'un repertoire
            Console.WriteLine("soleil");
        }
    }
}
