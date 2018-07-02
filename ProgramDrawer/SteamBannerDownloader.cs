using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
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
    }
}
