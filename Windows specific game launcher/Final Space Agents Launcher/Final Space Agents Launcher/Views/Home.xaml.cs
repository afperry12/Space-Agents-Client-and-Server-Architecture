using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
using Final_Space_Agents_Launcher;
using Final_Space_Agents_Launcher.LauncherData;

namespace Final_Space_Agents_Launcher.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public string VersionToDownload;

        public Button PlayButton;
        public ComboBox VersionSelector;
        public WebBrowser UpdateBoard;

        public VersionManager versionManager;

        public Home()
        {
            InitializeComponent();
            versionManager = new VersionManager(this);
            this.DataContext = this;
        }

        private void Button_Initialized(object sender, EventArgs e)
        {
            Console.WriteLine("buttoninit");
            PlayButton = (Button)sender;
        }

        private void ComboBox_Initialized(object sender, EventArgs e)
        {
            Console.WriteLine("comboboxinit");
            VersionSelector = (ComboBox)sender;
        }

        private void WebBrowser_Initialized(object sender, EventArgs e)
        {
            Console.WriteLine("webbrowserinit");
            UpdateBoard = (WebBrowser)sender;
        }

        private GamePaths paths;
        private void Button_click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("buttonclick");
            VersionSelector.IsEnabled = false;
            PlayButton.IsEnabled = false;

            paths = new GamePaths(VersionToDownload);

            if (File.Exists(paths.ExecutableFile))
            {
                Process.Start(paths.ExecutableFile);
                Environment.Exit(0);
                Console.WriteLine("buttonclick2");
            }

            try
            {
                FileDownloader downloader = new FileDownloader();
                Console.WriteLine("buttonclick3" + VersionToDownload);

                if (versionManager.VersionLinkPairs.TryGetValue("0.2", out string temp))
                {
                    Console.WriteLine("buttonclick4");
                    downloader.DownloadFileCompleted += Downloader_DownloadFileCompleted;
                    downloader.DownloadFileAsync(temp, $@"{paths.GameVersionFile}\Build({VersionToDownload}).zip");
                }
                Console.WriteLine("buttonclick5");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ex.ToString());
                PlayButton.IsEnabled = true;
                VersionSelector.IsEnabled = true;
            }
        }

        private void Downloader_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Console.WriteLine("downloader");
            try
            {
                ZipFile.ExtractToDirectory($@"{paths.GameVersionFile}\Build({VersionToDownload}).zip", paths.RootPath);
                Console.WriteLine("downloader1");
                File.Delete($@"{paths.GameVersionFile}\Build({VersionToDownload}).zip");
                Console.WriteLine("downloader2");
                Process.Start(paths.ExecutableFile);
                Console.WriteLine("downloader3");
                Environment.Exit(0);
                Console.WriteLine("downloader4");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ex.ToString());
                PlayButton.IsEnabled = true;
                VersionSelector.IsEnabled = true;
            }
            Console.WriteLine("downloader5");
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine("combobox1");
            VersionToDownload = VersionSelector.SelectedItem.ToString();
            Console.WriteLine("combobox2");
        }

    }
}
