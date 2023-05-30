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
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
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
                return;
            }
            _ = Process.Start(path);
        }

        private void button1_2_Click(object sender, RoutedEventArgs e)
        {
            _ = Process.Start("C:\\Program Files\\texstudio\\texstudio.exe");
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
                    Console.WriteLine("Get Files" + exc.ToString());
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
                        Console.WriteLine("Get Directories: " + ex.ToString());
                    }
                }
                //throw new Exception("No executable found."); 
                return GetAppPath(keyword, NewDirectories);

            }
        }

    }
}
