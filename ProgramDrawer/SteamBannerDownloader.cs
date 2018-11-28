using Microsoft.Win32;
using ProgramDrawer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProgramDrawer
{
    public class SteamBannerDownloader
    {
        public static List<ProgramItemBase> GetSteamProgramItems()
        {
            string steamDirectory = "";

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam"))
            {
                if (key != null)
                {
                    steamDirectory = key.GetValue("SteamPath").ToString().Replace("/", @"\");
                }
            }

            List<ProgramItemBase> SteamItems = new List<ProgramItemBase>();

            if (steamDirectory != "")
            {
                Dictionary<int, string> appIDs = GetInstalledSteamAppIds(steamDirectory);
                foreach (KeyValuePair<int, string> keyValue in appIDs)
                {
                    string programName = GetSteamAppNameFromAcf(keyValue.Value);

                    SteamItems.Add(new SteamProgramItem(keyValue.Key, programName));
                }
            }

            return SteamItems;
        }

        private static Dictionary<int, string> GetInstalledSteamAppIds(string steamInstallLocation)
        {
            List<string> acfFiles = Directory.EnumerateFiles(steamInstallLocation + @"\steamapps")
                .Where(f => Regex.Match(Path.GetFileName(f), @"appmanifest_(\d*).acf").Success).ToList();

            Dictionary<int, string> result = new Dictionary<int, string>();

            foreach (string file in acfFiles)
            {
                result.Add(Int32.Parse(Regex.Match(Path.GetFileName(file), @"appmanifest_(\d*).acf").Groups[1].Value), file);
            }

            result.Remove(228980); // This is Steamworks Common Redistributables, not actually a valid game

            return result;
        }

        private static string GetSteamAppNameFromAcf(string file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("\"name\""))
                    {
                        string[] parts = line.Split('\"');
                        return parts[3];
                    }
                }
            }

            return "";
        }
    }
}
