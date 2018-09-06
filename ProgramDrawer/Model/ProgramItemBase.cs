using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ProgramDrawer.Model
{
    public abstract class ProgramItemBase : INotifyPropertyChanged, ICloneable
    {
        #region ProgramName
        private string _programName;
        public string ProgramName
        {
            get { return _programName; }
            set { _programName = value; OnPropertyChanged("ProgramName"); }
        }
        #endregion

        #region ImageLocation
        private string _imageLocation;

        public string ImageLocation
        {
            get { return _imageLocation; }
            set
            {
                _imageLocation = value;
                UpdateBannerImage();
                OnPropertyChanged("ImageLocation");
                OnPropertyChanged("BannerImage");
            }
        }
        #endregion

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

        public abstract object Clone();
        public abstract void LaunchProgram(object sender, RoutedEventArgs e);

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
