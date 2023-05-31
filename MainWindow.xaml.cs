using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace GuiTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    

    public partial class MainWindow : Window
    {
        public new string TextInput { get; set; }

        public List<string> DefaultSearchDirectories = new List<string>() { "C:\\Program Files (x86)", "C:\\Program Files", "C:\\Windows" };

        public Dictionary<string, List<string>> myDictionary = new();
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            myDictionary.Add("button1_2", new List<string>());
            //Button1.Content = "Hallo";
        }
        
        private void button1_1_Click(object sender, RoutedEventArgs e)
        {
            //_ = MessageBox.Show(TextInput.ToString());
            string path;
            try
            {
                path = GetAppPath(TextInput, DefaultSearchDirectories);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                return;
            }

            try
            {
                _ = Process.Start(path);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Acces denied: " + ex.ToString());
            }

            myDictionary["button1_2"].Add(path);
        }

        private void button1_2_Click(object sender, RoutedEventArgs e)
        {
            string _identifier = "button1_2";

            foreach (string path in myDictionary[_identifier])
            {
                _ = Process.Start(path);
            }
           // _ = Process.Start("C:\\Program Files\\texstudio\\texstudio.exe");
        }

        private string GetAppPath(string keyword, List<string> directories)
        {
            string Searchpattern = "*" + keyword + "*" + ".exe";
            List<string> results = new List<string>();

            foreach (var item in directories)
            {
                try
                {
                    results.AddRange(Directory.GetFiles(item, Searchpattern).ToList());
                }
                catch(Exception exc)
                {
                    Trace.WriteLine("Get Files" + exc.ToString());
                }
            }


            if (results.Any())
            {
                return results[0];
            }
            else 
            {
                List<string> NewDirectories = new List<string>();

                foreach (var item in directories)
                {
                    try
                    {
                        NewDirectories.AddRange(Directory.GetDirectories(item).ToList());
                    }
                    catch(Exception ex)
                    {
                        Trace.WriteLine("Get Directories: " + ex.ToString());
                    }
                }
                //throw new Exception("No executable found."); 
                if (NewDirectories.Any())
                {
                    return GetAppPath(keyword, NewDirectories);
                }
                else
                {
                    throw new Exception("No application found!");
                }

            }
        }

    }
}
