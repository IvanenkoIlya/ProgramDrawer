using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ProgramDrawer
{
    public class ProgramItem
    {
        public string ProgramName { get; set; }
        public string ImageLocation { get; set; }
        public BitmapImage BannerImage { get; set; } = new BitmapImage(new Uri(@"Resources/ProgramBanners/default.png", UriKind.Relative));

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
