using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;

namespace ProgramDrawer.Model
{
    public class SteamProgramItem : ProgramItemBase
    {
        public int AppID { get; }

        public SteamProgramItem(int AppID, string ProgramName)
        {
            this.AppID = AppID;
            this.ProgramName = ProgramName;

            DownloadBanner();
        }

        [JsonConstructor]
        public SteamProgramItem(int AppID, string ProgramName, string ImageLocation)
        {
            this.AppID = AppID;
            this.ProgramName = ProgramName;
            this.ImageLocation = ImageLocation;
        }

        private void DownloadBanner()
        {
            using (WebClient client = new WebClient())
            {
                // TODO This might need rework
                try
                {
                    client.DownloadFile($@"https://steamcdn-a.opskins.media/steam/apps/{AppID}/header.jpg", $@"Resources\ProgramBanners\{RemoveInvalidFilenameChars(ProgramName)}.jpg");

                    ImageLocation = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\ProgramBanners\{RemoveInvalidFilenameChars(ProgramName)}.jpg");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    ImageLocation = "";
                }
            }
        }

        public override void ChangeProperties(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public override void LaunchProgram(object sender, RoutedEventArgs e)
        {
            Process.Start($@"steam://rungameid/{AppID}");
        }

        private string RemoveInvalidFilenameChars(string value)
        {
            string result = value;
            result = result.Replace("<", string.Empty);
            result = result.Replace(">", string.Empty);
            result = result.Replace(":", string.Empty);
            result = result.Replace("\"", string.Empty);
            result = result.Replace("/", string.Empty);
            result = result.Replace("\\", string.Empty);
            result = result.Replace("|", string.Empty);
            result = result.Replace("?", string.Empty);
            result = result.Replace("*", string.Empty);
            return result;
        }
    }
}
