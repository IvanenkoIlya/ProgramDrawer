using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ProgramDrawer.Model
{
    public abstract class ProgramItemBase
    {
        public string ProgramName { get; set; }

        private string _imageLocation;
        public string ImageLocation
        {
            get { return _imageLocation; }
            set
            {
                _imageLocation = value;
                UpdateBannerImage();
            }
        }

        [JsonIgnore]
        public BitmapImage BannerImage { get; private set; }

        protected void UpdateBannerImage()
        {
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

        public abstract void LaunchProgram(object sender, RoutedEventArgs e);
        public abstract void ChangeProperties(object sender, RoutedEventArgs e);
    }
}
