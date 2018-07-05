using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ProgramDrawer
{
    public class ProgramItem
    {
        public string ProgramName { get; set; }
        public string ProgramLocation { get; set; }
        public string ImageLocation { get; set; }
        public BitmapImage BannerImage { get; private set; }
    
        public ProgramItem(string programName, string programLocation, string imageLocation = "")
        {
            ProgramName = programName;
            ProgramLocation = programLocation;
            ImageLocation = imageLocation;

            if (File.Exists(ImageLocation))
                BannerImage = new BitmapImage(new Uri(ImageLocation));
            else
            {
                BannerImage = new BitmapImage();
                BannerImage.BeginInit();
                BannerImage.StreamSource = Application.GetResourceStream(new Uri(@"Resources/ProgramBanners/default.png", UriKind.Relative)).Stream;
                BannerImage.EndInit();
            }
        }

        private void PrintProgramName(object sender, EventArgs e)
        {
            Console.WriteLine($"{e.GetType()} {ProgramName}");
        }

        public void ChangeSettings(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"{ProgramName} change settings clicked");
        }

        public void LaunchProgram(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"{ProgramName} launch program clicked");
        }
    }
}
