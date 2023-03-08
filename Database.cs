using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanAutomatique
{
    internal class Database
    {// chemin jusqu'au repertoire courant
        public static string targetPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string name_db = "WanDataBase.db";
        public static string sourceFile = targetPath + name_db;
        string path = targetPath + "Error_Log.txt";

        //string path = @"C:\Draka_Antivirus\Draka_Antivirus\bin\Debug\Error_Log.txt";
        /*string path = @"C:\Program Files (x86)\Default Company Name\Setup1\Error_Log.txt";*/

        public void ErrorMessage(string sql, string e)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine(DateTime.Now.ToString() + " " + "Request :" + " " + sql + " " + "Error_Message :" + e);
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

            if (File.Exists(sourceFile) != true)
            {
                //MessageBox.Show("creation de la base de données");
                try
                {
                    File.Create(sourceFile);
                    AutoClosingMessageBox.Show("BD créer = " + sourceFile);
                }
                catch(Exception ex) 
                {
                    MessageBox.Show("Internal Error : " + ex.Message);
                    Console.WriteLine("Internal Error : " + ex.Message);
                    ErrorMessage("Create Database ", ex.ToString());
                    return null;
                }
            }
                return sourceFile;
        }


        /* 
         * Supprimer une base de données SQLite 
         */
        public Boolean deleteDatabase(string name_db)
        {
            string sourceFile = targetPath + name_db;

            if (File.Exists(sourceFile) == true)
            {
                try
                {
                    File.Delete(sourceFile);
                }
                catch (Exception ex)
                {
                    /*MessageBox.Show("Internal Error : " + ex.Message);
                    Console.WriteLine("Internal Error : " + ex.Message);*/
                    ErrorMessage("Delete Database ", ex.ToString());  // truncate table
                    return false;
                }
            }

            return true;
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
            MessageBox.Show("Insertion réussi");
            return true;
        }

        // creation d'une table de la base de donnée
        public Boolean CreateTable(string sourceFile, string nameTable)
        {
            if (!File.Exists(sourceFile))
            {
                sourceFile = createDatabase(name_db);
                MessageBox.Show("Base de données bien creeeeeeeeeeeeeeeeer");
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
                        MessageBox.Show("ouffffffffffffff ");
                        var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                        MessageBox.Show("tttttttttttt ");
                        var cmd = new SQLiteCommand(sql, con);
                        con.Open();
                        MessageBox.Show("Table creer etape1");
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Table creer etape2");
                        con.Close();
                        AutoClosingMessageBox.Show("la table quantaine a été créer avec succes");
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
            }
            //MessageBox.Show("Table creer avec succes");
            return true;
        }
        /* 
         * Sélectionner des données dans une base de données SQLite
         */
        public List<Object[]> selectDatas(string sourceFile, string sql)
        {
            List<Object[]> liste = null;

            if (File.Exists(sourceFile) == true)
            {
                try
                {
                    var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                    var cmd = new SQLiteCommand(sql, con);

                    con.Open();
                    SQLiteDataReader datas = cmd.ExecuteReader();
                    if (datas.HasRows == true)
                    {
                        liste = new List<Object[]>();
                        while (datas.Read())
                        {
                            Object[] element = new Object[datas.FieldCount];
                            for (var i = 0; i < datas.FieldCount; i++)
                            {
                                element[i] = datas.GetValue(i);
                            }

                            liste.Add(element);
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    /*MessageBox.Show("Internal Error : " + ex.Message);*//*
                    Console.WriteLine("Internal Error : " + ex.Message);*/
                    ErrorMessage(sql, ex.ToString());
                }
            }

            return liste;
        }




        /* 
         * Sélectionner des données dans une base de données SQLite
         * Et ajouter une donnée pour incrémenter automatiquement
         * le numéro ou l'identifiant de la table
         */
        public List<Object[]> selectDatasAuto(string sourceFile, string sql)
        {
            List<Object[]> liste = null;

            if (File.Exists(sourceFile) == true)
            {
                try
                {
                    var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                    var cmd = new SQLiteCommand(sql, con);

                    con.Open();
                    SQLiteDataReader datas = cmd.ExecuteReader();
                    if (datas.HasRows == true)
                    {
                        int j = 1;
                        liste = new List<Object[]>();
                        while (datas.Read())
                        {
                            Object[] element = new Object[datas.FieldCount + 1];

                            for (var i = 0; i < (datas.FieldCount + 1); i++)
                            {
                                if (i == 0)
                                {
                                    element[i] = j;
                                }
                                else
                                {
                                    element[i] = datas.GetValue(i - 1);
                                }
                            }

                            liste.Add(element);
                            j = j + 1;
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    /*MessageBox.Show("Internal Error : " + ex.Message);*/
                    /*Console.WriteLine("Internal Error : " + ex.Message);*/
                    ErrorMessage(sql, ex.ToString());
                }
            }

            return liste;
        }

        /* 
         * Sélectionner une donnée dans une base de données SQLite
         */
        public Object[] searchData(string sourceFile, string sql)
        {
            Object[] element = null;

            if (File.Exists(sourceFile) == true)
            {
                MessageBox.Show("eeeeeee");
                try
                {
                    var con = new SQLiteConnection("Data Source  = " + sourceFile + ";");
                    var cmd = new SQLiteCommand(sql, con);

                    con.Open();
                    SQLiteDataReader datas = cmd.ExecuteReader();
                    //MessageBox.Show("eeeeeeetape1");
                    if (datas.HasRows == true)
                    {
                        //MessageBox.Show("Eeetape2");
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

        // suprimer les données d'une table

        public Boolean ClearTable(string name_db)
        {
            string sourceFile = targetPath + name_db;
            string sql = "DELETE Quarantaine WHERE;";
            string sql1 = "VACUUM Quarantaine ;";

            if (File.Exists(sourceFile) == true)
            {
                MessageBox.Show("je suis dans le cleanTable");
                try
                {
                    var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                    var cmd = new SQLiteCommand(sql, con);
                    var cmd1 = new SQLiteCommand(sql1, con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    cmd1.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Table Quarantaine éffacé avec succes");
                    return true;
                }
                catch (Exception ex)
                {
                    //AutoClosingMessageBox.Show("Internal Error : " + ex.Message);
                    Console.WriteLine("Internal Error : " + ex.Message);
                    ErrorMessage(sql, ex.ToString());
                    return false;
                }

            }
            return true;
        }


        /* 
         * Supprimer des données dans une base de données SQLite
         */
        public Boolean deleteData(string sourceFile, string sql)
        {
            if (File.Exists(sourceFile) == true)
            {
                try
                {
                    var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                    var cmd = new SQLiteCommand(sql, con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(" Fichier supprimer de la BD ");
                    con.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    /* MessageBox.Show("Internal Error : " + ex.Message);
                     Console.WriteLine("Internal Error : " + ex.Message);*/
                    ErrorMessage(sql, ex.ToString());
                    return false;
                }
            }

            return false;
        }


    }
}
