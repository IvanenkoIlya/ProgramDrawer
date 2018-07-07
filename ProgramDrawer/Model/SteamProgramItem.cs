using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;

namespace ProgramDrawer.Model
{
    public class SteamProgramItem : ProgramItemBase
    {
        public int AppID { get; }
        
        public SteamProgramItem(int AppID)
        {
            this.AppID = AppID;

            GetGameInfo();
            DownloadBanner();
        }

        [JsonConstructor]
        public SteamProgramItem(int AppID, string ProgramName, string ImageLocation)
        {
            this.AppID = AppID;
            this.ProgramName = ProgramName;
            this.ImageLocation = ImageLocation;
        }

        private void GetGameInfo()
        {
            string gameInfoJson = "";
            SteamGameInfo result = null;

            HttpWebRequest request = (HttpWebRequest) WebRequest.Create($@"https://api.steampowered.com/ISteamUserStats/GetSchemaForGame/v2/?key=C246687750FD8733B2712304AB703D3A&format=json&appid={AppID}&l=");
            request.Timeout = 10000;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using(StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        gameInfoJson = sr.ReadToEnd();
                    }
                }

                result = JsonConvert.DeserializeObject<SteamGameInfo>(gameInfoJson);
            } catch(WebException e)
            {
                if(e.Status == WebExceptionStatus.Timeout)
                {
                    // Timeout exceeded
                }
            }

            if (result != null)
                ProgramName = result.Game.GameName;
        }

        private void DownloadBanner()
        {
            using(WebClient client = new WebClient())
            {
                client.DownloadFile($@"https://steamcdn-a.opskins.media/steam/apps/{AppID}/header.jpg", $@"Resources\ProgramBanners\{ProgramName}.jpg");
            }

            ImageLocation = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\ProgramBanners\{ProgramName}.jpg");
        }

        public override void ChangeProperties(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public override void LaunchProgram(object sender, RoutedEventArgs e)
        {
            Process.Start($@"steam://rungameid/{AppID}");
        }
    }

    public class SteamGameInfo
    {
        public SteamGame Game { get; set; }

        public class SteamGame
        {
            public string GameName { get; set; }
            public int GameVersion { get; set; }

            /*
             * If ever implement steam user, can include these to get user stats
             */

            //public GameStats AvailableGameStats { get; set; }

            //public class GameStats
            //{
            //    public List<Achievment> Achievements { get; set; }
            //    public List<Stat> Stats { get; set; }

            //    public class Achievment
            //    {
            //        public string Name { get; set; }
            //        public bool DefaultValue { get; set; }
            //        public string DisplayName { get; set; }
            //        public bool Hidden { get; set; }
            //        public string Description { get; set; }
            //        public string Icon { get; set; }
            //        public string IconGray { get; set; }
            //    }

            //    public class Stat
            //    {
            //        public string Name { get; set; }
            //        public int DefaultValue { get; set; }
            //        public string DisplayName { get; set; }
            //    }
            //}   
        }
    }
}
