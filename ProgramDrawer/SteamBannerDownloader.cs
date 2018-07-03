using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace ProgramDrawer
{
    public class SteamBannerDownloader
    {
        BackgroundWorker worker;
        MainWindow window;
        List<int> appIDs;

        public SteamBannerDownloader(MainWindow window, List<int> appIDs)
        {
            this.window = window;
            this.appIDs = appIDs;

#pragma warning disable IDE0017 // Simplify object initialization
            worker = new BackgroundWorker();
#pragma warning restore IDE0017 // Simplify object initialization
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DownloadBanners;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_Completed;
        }

        public void DownloadSteamBanners()
        {
            if (!worker.IsBusy)
            {
                //window.DownloadProgressBar.Value = 0;
                //window.DownloadProgressBar.Maximum = appIDs.Count;
                //window.DownloadProgressBar.Visibility = Visibility.Visible;

                worker.RunWorkerAsync();
            }
        }

        private void Worker_DownloadBanners(object sender, DoWorkEventArgs e)
        {
            Directory.CreateDirectory(@"Resources\ProgramBanners\");

            using (WebClient client = new WebClient())
            {
                for(int i = 0; i < appIDs.Count;)
                {
                    //client.DownloadFile($@"https://steamcdn-a.opskins.media/steam/apps/{appIDs[i]}/header.jpg", $@"Resources\ProgramBanners\{appIDs[i]}.jpg");
                    worker.ReportProgress(++i);
                }
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation(e.ProgressPercentage, new Duration(TimeSpan.FromMilliseconds(100)))
            {
                EasingFunction = new SineEase()
            };

            //window.DownloadProgressBar.ApplyAnimationClock(ProgressBar.ValueProperty, da.CreateClock());
        }

        private void Worker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            //window.DownloadProgressBar.Visibility = Visibility.Collapsed;
        }

        private List<int> GetInstalledSteamAppIds(string steamInstallLocation)
        {
            return Directory.EnumerateFiles(steamInstallLocation + @"\steamapps")
                .Where(f => Regex.Match(Path.GetFileName(f), @"appmanifest_(\d*).acf").Success) // Could likely optimize into one linq statement?
                .Select(f => Int32.Parse(Regex.Match(Path.GetFileName(f), @"appmanifest_(\d*).acf").Groups[1].Value))
                .ToList();
        }

        private string _steamDirectory;
        public string SteamDirectory
        {
            get
            {
                if (_steamDirectory == null)
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam"))
                    {
                        if (key != null)
                        {
                            _steamDirectory = key.GetValue("SteamPath").ToString().Replace("/", @"\");
                        }
                    }

                return _steamDirectory;
            }
        }
    }
}
