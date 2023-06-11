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
using System.Xml;

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

        public XmlDocument doc = new XmlDocument();
        public Button LastRightClickedButton;

        //public ICommand ButtonRC;

        public MainWindow()
        {
            
            InitializeComponent();
            DataContext = this;
            //myDictionary.Add("button1_2", new List<string>());
            //Button1.Content = "Hallo";
        }
        
        private void ButtonRC_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ButtonRC_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //Button clickedButton = sender as Button;

            if (e.OriginalSource is Button clickedButton)
            {
                //string s = TextInput;
                LastRightClickedButton = clickedButton;
                clickedButton.Background = Brushes.Red;
                ContextMenu cm = FindResource("cm") as ContextMenu;
                cm.PlacementTarget = clickedButton;
                cm.IsOpen = true;
            }
            //MessageBox.Show(sender.ToString() + "The New command was invoked");
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

            //foreach (string path in myDictionary[_identifier])
            //{
            //    _ = Process.Start(path);
            //}

            string pathToAdd = GetAppPath(TextInput, DefaultSearchDirectories);

            doc.Load(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\paths.xml"));
            XmlNode node = doc.DocumentElement.SelectSingleNode("/buttons");

            XmlNode newNode = doc.CreateNode(XmlNodeType.Element, "path", "");
            newNode.InnerText = pathToAdd;
            

            foreach (XmlNode subnode in doc.DocumentElement.SelectSingleNode("/buttons").ChildNodes)
            {
                if (subnode.Attributes["nr"].InnerText.Equals("1_3", StringComparison.Ordinal))
                {
                    node = subnode;
                    break;
                }
            }

            node.AppendChild(newNode);
            doc.Save(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\paths.xml"));
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

        private void button1_3_Click(object sender, RoutedEventArgs e)
        {
            string indentifier = "1_3";
            doc.Load(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\paths.xml"));
            XmlNode node = doc.DocumentElement.SelectSingleNode("/buttons");

            foreach (XmlNode subnode in doc.DocumentElement.SelectSingleNode("/buttons").ChildNodes)
            {
                if (subnode.Attributes["nr"].InnerText.Equals(indentifier, StringComparison.Ordinal))
                {
                    node = subnode;
                    break;
                }
            }

            foreach (XmlNode pathNode in node.ChildNodes)
            {
                _ = Process.Start(pathNode.InnerText);
            }
        }

        private void button1_4_Click(object sender, RoutedEventArgs e)
        {
            string _identifier = "button1_4";

            //foreach (string path in myDictionary[_identifier])
            //{
            //    _ = Process.Start(path);
            //}

            string pathToAdd = GetAppPath(TextInput, DefaultSearchDirectories);

            doc.Load(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\paths.xml"));
            XmlNode node = doc.DocumentElement.SelectSingleNode("/buttons");


            foreach (XmlNode subnode in doc.DocumentElement.SelectSingleNode("/buttons").ChildNodes)
            {
                if (subnode.Attributes["nr"].InnerText.Equals("1_3", StringComparison.Ordinal))
                {
                    node = subnode;
                    break;
                }
            }

            foreach (XmlNode subnode in node)
            {
                if(subnode.InnerText.Equals(pathToAdd, StringComparison.Ordinal))
                {
                    _ = node.RemoveChild(subnode);
                }
            }

            
            doc.Save(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\paths.xml"));
        }

        private void cm_addPath_Click(object sender, RoutedEventArgs e)
        {

            TextBox textBox = LastRightClickedButton.FindName("TextBox_Input") as TextBox;

            // Datenbindung aktualisieren
            textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();

            string pathToAdd = ManageCollection_QuickSearch(TextInput);

            if (!pathToAdd.Any())
            {
                pathToAdd = GetAppPath(TextInput, DefaultSearchDirectories);
            }
             
            Trace.WriteLine("PATH: " + pathToAdd);

            XmlNode pathNode = ManagePathCollection_Add(pathToAdd);

            doc.Load(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\paths.xml"));
            XmlNode node = doc.DocumentElement.SelectSingleNode("/buttons");

            XmlNode newNode = doc.ImportNode(pathNode, true);


            foreach (XmlNode subnode in doc.DocumentElement.SelectSingleNode("/buttons").ChildNodes)
            {
                if (subnode.Attributes["nr"].InnerText.Equals(LastRightClickedButton.Uid, StringComparison.Ordinal))                                   // <----- "2_1" muss ersetzt werden -> irgendwie aktuellen nutton handle getten!
                {
                    node = subnode;
                    break;
                }
            }

            _ = node.AppendChild(newNode);
            doc.Save(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\paths.xml"));
        }

        private void buttonClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string identifier = b.Uid;
            
            doc.Load(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\paths.xml"));
            XmlNode node = doc.DocumentElement.SelectSingleNode("/buttons");

            foreach (XmlNode subnode in doc.DocumentElement.SelectSingleNode("/buttons").ChildNodes)
            {
                if (subnode.Attributes["nr"].InnerText.Equals(identifier, StringComparison.Ordinal))
                {
                    node = subnode;
                    break;
                }
            }

            foreach (XmlNode pathNode in node.ChildNodes)
            {
                _ = Process.Start(pathNode.InnerText);
            }
        }

        private void cm_removePath_Click(object sender, RoutedEventArgs e)
        {
            TextBox textBox = LastRightClickedButton.FindName("TextBox_Input") as TextBox;

            // Datenbindung aktualisieren
            textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();

            string pathToAdd = GetAppPath(TextInput, DefaultSearchDirectories);



            doc.Load(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\paths.xml"));
            XmlNode node = doc.DocumentElement.SelectSingleNode("/buttons");


            foreach (XmlNode subnode in doc.DocumentElement.SelectSingleNode("/buttons").ChildNodes)
            {
                if (subnode.Attributes["nr"].InnerText.Equals(LastRightClickedButton.Uid, StringComparison.Ordinal))
                {
                    node = subnode;
                    break;
                }
            }

            foreach (XmlNode subnode in node)
            {
                if (subnode.InnerText.Equals(pathToAdd, StringComparison.Ordinal))
                {
                    _ = node.RemoveChild(subnode);
                }
            }


            doc.Save(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\paths.xml"));
        }

        private XmlNode ManagePathCollection_Add(string path)
        {
            doc.Load(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\pathcollection.xml"));
            XmlNode node = doc.DocumentElement.SelectSingleNode("/paths");
            bool AlreadyInCollection = false;
            int CollectionCount = 0;
            XmlNode OldNode = null;
            foreach (XmlNode subnode in node.ChildNodes)
            {
                CollectionCount++;
                if (subnode.InnerText.Equals(path, StringComparison.Ordinal))
                {
                    OldNode = subnode;
                    AlreadyInCollection = true;
                    break;
                }
            }

            if (!AlreadyInCollection)
            {
                XmlAttribute attr = doc.CreateAttribute("id");
                attr.Value = CollectionCount.ToString();
                XmlNode newNode = doc.CreateNode(XmlNodeType.Element, "path", "");
                newNode.InnerText = path;
                newNode.Attributes.SetNamedItem(attr);
                _ = node.AppendChild(newNode);
                doc.Save(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\pathcollection.xml"));
                return newNode;
            }
            return OldNode;
        }

        private string ManageCollection_QuickSearch(string path)
        {
            if (!path.Any())
            {
                return "";
            }
            doc.Load(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\pathcollection.xml"));
            XmlNode node = doc.DocumentElement.SelectSingleNode("/paths");
            foreach (XmlNode subnode in node.ChildNodes)
            {
                if (subnode.InnerText.ToUpper().Contains(path.ToUpper(), StringComparison.Ordinal))
                {
                    Trace.WriteLine("Found in Collection!");
                    return subnode.InnerText;
                }
            }
            return "";
        }

    }   
}
