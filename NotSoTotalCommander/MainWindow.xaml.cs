using System;
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
using System.IO;

namespace NotSoTotalCommander
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(var drive in Directory.GetLogicalDrives())
            {
                var item = new TreeViewItem();
                {
                    item.Header = drive;
                    item.Tag = drive;
                }
                
                item.Items.Add(null);

                item.Expanded += Folder_Expanded;

                WidokFolderow.Items.Add(item);
            }
        }
        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;

            //jezeli plik zawiera nieuprawnione dane
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;

            item.Items.Clear();

            //pełna sciezka
            var fullPath = (string)item.Tag;

            //pusta lista dla katalogow
            var directories = new List<string>();

            //proba pobrania katalogow z folderu
            //ignorowanie bledow
            try
            {
                var dirs = Directory.GetDirectories(fullPath);

                if (dirs.Length > 0)
                    directories.AddRange(dirs);
            }
            catch
            {

            }

            directories.ForEach(directoryPath =>
            {
                var subItem = new TreeViewItem();
                {
                    subItem.Header = GetFileFolderName(directoryPath);
                    subItem.Tag = directoryPath;
                };


                subItem.Items.Add(null);

                subItem.Expanded += Folder_Expanded;

                item.Items.Add(subItem);
            });
        }

        public static string GetFileFolderName(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            //zamiana slah na backslash
            var normalizedPath = path.Replace('/', '\\');

            //znajdz ostatni backslash w sciezce
            var lastIndex = normalizedPath.LastIndexOf('\\');

            //jezeli nie znajdzie backslasha zwraca sciezke
            if (lastIndex <= 0)
                return path;

            //zwraca nazwe po ostatnim backslashu
            return path.Substring(lastIndex + 1);
        }
    }
}
