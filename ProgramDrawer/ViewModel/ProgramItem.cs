using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ProgramDrawer.ViewModel
{
    public class ProgramItem : ObservableObject
    {
        #region Properties
        private string _programName;
        public string ProgramName
        {
            get { return _programName; }
            set { _programName = value; OnPropertyChanged("ProgramName"); }
        }

        private string _imageLocation = @".\Resources\ProgramBanners\default.png"; // TODO
        public string ImageLocation
        {
            get { return _imageLocation; }
            set { _imageLocation = value; OnPropertyChanged("ImageLocation"); }
        }

        public BitmapImage BannerImage { get { return new BitmapImage(new Uri(ImageLocation, UriKind.Relative)); } }
        #endregion

        public void ChangeSettings(object sender, RoutedEventArgs e)
        {

        }

        public void LaunchProgram(object sender, RoutedEventArgs e)
        {

        }
    }
}
