using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Automation;

namespace ScanAutomatique
{
    internal class ControleNavigateur
    {
        public ControleNavigateur()
        {
            // ici nous sommes dans le constructeur de la classe.
        }

        // ici nous avons chacune des méthodes de la classe

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public static string GetFirefoxUrl(Process process)  // pour firefox  et microsorft edge
        {
            if (process == null)
                throw new ArgumentNullException("process");

            if (process.MainWindowHandle == IntPtr.Zero)
                return null;

            AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
            if (element == null)
                return null;

            AutomationElement doc = element.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Document));
            if (doc == null)
                return null;

            return ((ValuePattern)doc.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
        }

        public static string GetInternetExplorerUrl(Process process)   // pour internet explorer
        {
            if (process == null)
                throw new ArgumentNullException("process");

            if (process.MainWindowHandle == IntPtr.Zero)
                return null;

            AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
            if (element == null)
                return null;

            AutomationElement rebar = element.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, "ReBarWindow32"));
            if (rebar == null)
                return null;

            AutomationElement edit = rebar.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

            return ((ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
        }

        public static string GetChromeUrl(Process process)       //  pour google chrome
        {
            if (process == null)
                throw new ArgumentNullException("process");

            if (process.MainWindowHandle == IntPtr.Zero)
                return null;

            AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
            if (element == null)
                return null;

            AutomationElement edit = element.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
            return ((ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
        }

        public static string GetURL(IntPtr intPtr, string programName)
        {
            string temp = null;
            string url = "";
            if (programName.Equals("opera"))
            {
                Process[] procsOpera = Process.GetProcessesByName("opera");
                foreach (Process opera in procsOpera)
                {
                    // the chrome process must have a window
                    if (opera.MainWindowHandle == IntPtr.Zero)
                    {
                        continue;
                    }

                    // find the automation element
                    AutomationElement elm = AutomationElement.FromHandle(opera.MainWindowHandle);
                    AutomationElement elmUrlBar = elm.FindFirst(TreeScope.Descendants,
                      new PropertyCondition(AutomationElement.NameProperty, "Address field"));

                    // if it can be found, get the value from the URL bar
                    if (elmUrlBar != null)
                    {
                        AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
                        if (patterns.Length > 0)
                        {
                            ValuePattern val = (ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0]);
                            temp = val.Current.Value.ToString();
                            url = val.Current.Value.ToString();
                            MessageBox.Show("url : " + url);
                        }
                        else
                        {
                            temp = "";
                            url = "";
                        }
                    }
                    else
                    {
                        temp = "";
                        url = "";
                    }
                }
            }
            url = temp;
            return temp;
        }

        public static string Methode4()  // pour l'url du navigateur google chrome l'url courant
        {
            string url = "";
            Process[] procsChrome = Process.GetProcessesByName("chrome");
            //MessageBox.Show(" uuuu : " + procsChrome.Length);
            foreach (Process chrome in procsChrome)
            {
                // the chrome process must have a window
                if (chrome.MainWindowHandle == IntPtr.Zero)
                {
                    continue;
                }
                //MessageBox.Show("url : " + chrome);
                // find the automation element
                AutomationElement elm = AutomationElement.FromHandle(chrome.MainWindowHandle);

                //MessageBox.Show("url : " + elm.ToString());
                //AutomationElement elmUrlBar = elm.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Address and search bar"));
                AutomationElement elmUrlBar = elm.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
                //MessageBox.Show("url : " + elmUrlBar);
                // if it can be found, get the value from the URL bar
                if (elmUrlBar != null)
                {
                    AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
                    if (patterns.Length > 0)
                    {
                        ValuePattern val = (ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0]);
                        Console.WriteLine("Chrome URL found: " + val.Current.Value);
                        url =  (val.Current.Value);
                        //label1.Text = (val.Current.Value);
                        //MessageBox.Show(" uuuu : " + val.Current.Value);
                    }
                    else
                    {
                        //MessageBox.Show(" uuuu : " + patterns.Length);
                    }
                }
                else
                {
                    //MessageBox.Show("La situation se complique ");
                }
            }
            return url;
        }

        public void Methode5()  // pour l'url du navigateur google chrome l'url courant
        {
            AutomationElement.RootElement
            .FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, "chrome_WidgetWin_1"))
            .SetFocus();
            SendKeys.SendWait("^l");
            var elmUrlBar = AutomationElement.FocusedElement;
            var valuePattern = (ValuePattern)elmUrlBar.GetCurrentPattern(ValuePattern.Pattern);
            //label1.Text = (valuePattern.Current.Value);
            MessageBox.Show("tt : " + valuePattern.Current.Value);
            Console.WriteLine(valuePattern.Current.Value);
        }

        // cette méthode permet d'appeller le javascript dans un code C# à travers un fichier html

        public void testJS()
        {
            WebBrowser browser = new WebBrowser();
            //using System.IO;  
            string applicationDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            string myFile = Path.Combine(applicationDirectory, "Nav.html");
            browser.Url = new Uri("file:///" + myFile);
            /*string curDir1 = AppDomain.CurrentDomain.BaseDirectory;
            string curDir = Directory.GetCurrentDirectory();

            browser.Url = new Uri("String.Format(file:///{0}/test.html", curDir);*/
            browser.ScriptErrorsSuppressed = true;
            MessageBox.Show("chargement");

            /*object[] bbjs = { textBox.Text.Replace("x", x.ToString()) };
            int y = Convert.ToInt32(double.Parse()));*/
            //browser.Document.InvokeScript("geturl").ToString();
            string url = (browser.Document.InvokeScript("geturl").ToString());
            MessageBox.Show("NAME : " + url);
        }




    }
}
