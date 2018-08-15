using Newtonsoft.Json;
using System;
using System.ComponentModel;
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

        public override void LaunchProgram(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start($@"steam://rungameid/{AppID}");
            } catch( Win32Exception fileNotFoundException)
            {
                //TODO file not found
            }       
        }

        #region Helper functions
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
        #endregion

        public override object Clone()
        {
            return new SteamProgramItem(AppID, ProgramName, ImageLocation);
        }
    }
}
