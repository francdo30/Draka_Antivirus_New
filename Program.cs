using System;
using System.Security.Permissions;
using System.Xml.Linq;
using static Guna.UI2.Native.WinApi;

namespace ScanAutomatique
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();            
            UsbInterface f = new UsbInterface();
            Download NewDownload = new Download();
            
            Application.Run(f);
        }       

    }
}