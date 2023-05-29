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
                path = GetAppPath(TextInput);
            }
            catch (Exception ex)
            {
                return;
            }
            _ = Process.Start(path);
        }

        private void button1_2_Click(object sender, RoutedEventArgs e)
        {
            _ = Process.Start("C:\\Windows\\explorer.exe");
        }

        private string GetAppPath(string keyword)
        {
            string Searchpattern = "*" + keyword + "*" + ".exe";
            List<string> results = Directory.GetFiles("C:\\Program Files (x86)", Searchpattern).ToList();
            results.AddRange(Directory.GetFiles("C:\\Program Files", Searchpattern).ToList());
            results.AddRange(Directory.GetFiles("C:\\Windows", Searchpattern).ToList());

            if (results.Any())
            {
                return results[0];
            }
            else { throw new Exception("No executable found."); }
        }

    }
}
