using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ScanAutomatique
{   
    internal class BaseDonnees
    {
        public static string name_db = "objectionable_websites.db";
        public static string name_db3 = "websites";
        public static string targetPath = AppDomain.CurrentDomain.BaseDirectory;
        string path = targetPath + "Error_Log.txt";


        public BaseDonnees()
        {
            // ici nous sommes dans le construteur
        }

        public void ErrorMessage(string sql, string e)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine(DateTime.Now.ToString() + " " + "Request:" + " " + sql + " " + "Error_Message:" + e);
                tw.Close();
            }

            else if (File.Exists(path))
            {
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine(DateTime.Now.ToString() + " " + "Request:" + " " + sql + " " + "Error_Message:" + e);
                tw.Close();
            }
        }

        /* 
         * Création d'une base de données SQLite 
         */
        public string createDatabase(string name_db)
        {
            string sourceFile = targetPath + name_db;
            //MessageBox.Show("Etape 1 ");
            if (File.Exists(sourceFile) != true)
            {
                try
                {                    
                    File.Create(sourceFile);
                    AutoClosingMessageBox.Show("BD creer ");
                }
                catch (Exception ex)
                {
                    /*MessageBox.Show("Internal Error : " + ex.Message);
                    Console.WriteLine("Internal Error : " + ex.Message);*/
                    ErrorMessage("Create Database ", ex.ToString());
                    return null;
                }
            }
            else
            {
                MessageBox.Show("la base de données existe deja ");
            }

            return sourceFile;
        }

        /* 
        * Inserer des données dans une base de données SQLite
        */
        public Boolean insertData(string sourceFile, string sql)
        {
            if (File.Exists(sourceFile) == true)
            {
                try
                {
                    var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                    var cmd = new SQLiteCommand(sql, con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    //AutoClosingMessageBox.Show("Insertion réussi");
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Internal Error : " + ex.Message);
                    Console.WriteLine("Internal Error : " + ex.Message);
                    ErrorMessage(sql, ex.ToString());
                    return false;
                }
            }
            else
            {
                CreateTable(sourceFile, "websites");
            }
            MessageBox.Show("Insertion réussi");
            return true;
        }

        // creation d'une table de la base de donnée
        public Boolean CreateTable(string sourceFile, string nameTable)
        {
            if (!File.Exists(sourceFile))
            {
                sourceFile = createDatabase(name_db);                
            }
            else
            {
                String sql = "CREATE TABLE " + nameTable + "(Id Integer primary key autoincrement, date text, taille text, directory text, statut text)" + ";";
                string myCommand = "CREATE TABLE " + nameTable + "(url varchar(128) not null primary key, status varchar(128), date varchar(128))";

                if (nameTable == "Quarantaine")
                {
                    MessageBox.Show("je suis la table quarantaine ");
                    try
                    {
                        var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                        var cmd = new SQLiteCommand(sql, con);
                        con.Open();                        
                        cmd.ExecuteNonQuery();                        
                        con.Close();
                        AutoClosingMessageBox.Show("la table créé avec succes");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Internal Error : " + ex.Message);
                        //Console.WriteLine("Internal Error : " + ex.Message);
                        ErrorMessage(sql, ex.ToString());
                        return false;
                    }
                }
                else if (nameTable == "parentControl")
                {
                    try
                    {
                        var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                        var cmd = new SQLiteCommand(myCommand, con);
                        con.Open();
                        //MessageBox.Show("Table creer etape1");
                        cmd.ExecuteNonQuery();
                        //MessageBox.Show("Table creer etape2");
                        con.Close();
                        //AutoClosingMessageBox.Show("la table parentControl a été créer avec succes");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Internal Error : " + ex.Message);
                        //Console.WriteLine("Internal Error : " + ex.Message);
                        ErrorMessage(sql, ex.ToString());
                        return false;
                    }
                }
                else if (nameTable == "FullScan")
                {
                    string myCommand1 = "CREATE TABLE " + nameTable + "(Id INTEGER AUTOINCREMENT not null primary key ,date text , taille text, statut varchar(128))";

                    try
                    {
                        var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                        var cmd = new SQLiteCommand(myCommand1, con);
                        con.Open();
                        //MessageBox.Show("Table creer etape1");
                        cmd.ExecuteNonQuery();
                        //MessageBox.Show("Table creer etape2");
                        con.Close();
                        //  AutoClosingMessageBox.Show("la table FullScan a été créer avec succes");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Internal Error : " + ex.Message);
                        //Console.WriteLine("Internal Error : " + ex.Message);
                        ErrorMessage(sql, ex.ToString());
                        return false;
                    }
                }
                else if (nameTable == "websites")
                {
                    //MessageBox.Show("Table website");
                    
                    string sql88 = "CREATE TABLE " + nameTable + "(Id Integer primary key Autoincrement , url text, statut text, date text)";

                    try
                    {
                        //MessageBox.Show("Je suis là");
                        var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                        var cmd = new SQLiteCommand(sql88, con);
                        //MessageBox.Show("Je suis la-bas");
                        con.Open();
                        //MessageBox.Show("Table creer etape1");
                        cmd.ExecuteNonQuery();
                        //MessageBox.Show("Table creer etape2");
                        con.Close();
                        //  AutoClosingMessageBox.Show("la table FullScan a été créer avec succes");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Internal Error : " + ex.Message);
                        //Console.WriteLine("Internal Error : " + ex.Message);
                        ErrorMessage(sql, ex.ToString());
                        return false;
                    }
                }
            }
            //MessageBox.Show("Table creer avec succes");
            return true;
        }

        /* 
         * Sélectionner une donnée dans une base de données SQLite
         */
        public Object[] searchData(string sourceFile, string sql)
        {
            Object[] element = null;
            //MessageBox.Show("la recherche encours");
            if (File.Exists(sourceFile) == true)
            {
                //AutoClosingMessageBox.Show("oui la base de données existe");
                try
                {
                    var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                    var cmd = new SQLiteCommand(sql, con);
                    con.Open();

                    SQLiteDataReader datas = cmd.ExecuteReader();
                    if (datas.HasRows == true)
                    {
                        while (datas.Read())
                        {
                            element = new Object[datas.FieldCount];
                            for (var i = 0; i < datas.FieldCount; i++)
                            {
                                element[i] = datas.GetValue(i);
                            }
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Internal Error : " + ex.Message);
                    Console.WriteLine("Internal Error : " + ex.Message);
                    ErrorMessage(sql, ex.ToString());
                }
            }

            return element;
        }


    }
}
