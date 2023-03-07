using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Collections.ObjectModel;
using Final_Space_Agents_Launcher.Views;

namespace Final_Space_Agents_Launcher.LauncherData
{

    public class VersionManager
    {

        public Dictionary<string, string> VersionLinkPairs = new Dictionary<string, string>();
        private Home WindowClass;
        public VersionManager(Home WindowClass)
        {
            Console.WriteLine("preinitversmanager");
            this.WindowClass = WindowClass;
            Console.WriteLine("preinitversmanager2");
            //WindowClass.PlayButton.IsEnabled = false;
            Console.WriteLine("preinitversmanager3");
            //WindowClass.VersionSelector.IsEnabled = false;
            Console.WriteLine("preinitversmanager4");
            Init();
            Console.WriteLine("postinit");
        }

        private void Init()
        {
            Console.WriteLine("versmanagerinit1");
            WebClient c = new WebClient();
            Console.WriteLine("versmanagerinit2");
            c.DownloadStringCompleted += C_DownloadStringCompleted;
            Console.WriteLine("versmanagerinit3");
            c.DownloadStringAsync(new Uri("https://drive.google.com/uc?export=download&id=1_YIJb2KuainHLdMjvX9FGmhGwSub-TXs"));
            Console.WriteLine("versmanagerinit4");
        }

        private void C_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Console.WriteLine("versmancdownload1");
            string temp = e.Result;
            System.Diagnostics.Debug.WriteLine("Result" + e.Result);
            Console.WriteLine("versmancdownload2");
            string[] VersionLinks = temp.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            System.Diagnostics.Debug.WriteLine("VersionLinks: " + VersionLinks[0]);
            Console.WriteLine("versmancdownload3");
            ObservableCollection<string> VersionsToDisplay = new ObservableCollection<string>();
            Console.WriteLine("versmancdownload4");
            for (int i = 0; i < VersionLinks.Length; i++)
            {
                string[] Version_Link = VersionLinks[i].Split(' ');
                Console.WriteLine("versmancdownload5");
                System.Diagnostics.Debug.WriteLine("Here: " + Version_Link[0]);
                System.Diagnostics.Debug.WriteLine("Here2: " + Version_Link[1]);

                VersionLinkPairs.Add(Version_Link[0], Version_Link[1]);
                System.Diagnostics.Debug.WriteLine("LinkPairs: " + VersionLinkPairs["0.1"]);
                Console.WriteLine("versmancdownload6");

                VersionsToDisplay.Add(Version_Link[0]);
                Console.WriteLine("versmancdownload7");
            }
            Console.WriteLine("versmancdownload8");
            WindowClass.VersionSelector.ItemsSource = VersionsToDisplay;
            Console.WriteLine("versmancdownload9");
            WindowClass.VersionSelector.Items.Refresh();
            Console.WriteLine("versmancdownload10");
            WindowClass.PlayButton.IsEnabled = true;
            Console.WriteLine("versmancdownload11");
            WindowClass.VersionSelector.IsEnabled = true;
            Console.WriteLine("versmancdownload12");
        }

    }

}