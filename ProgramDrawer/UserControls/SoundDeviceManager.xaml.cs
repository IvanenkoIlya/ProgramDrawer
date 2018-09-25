using AudioSwitcher.AudioApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProgramDrawer.UserControls
{
    /// <summary>
    /// Interaction logic for SoundDeviceManager.xaml
    /// </summary>
    public partial class SoundDeviceManager : UserControl, INotifyPropertyChanged
    {
        #region DependencyProperties
        public static readonly DependencyProperty AudioTitleProperty =
            DependencyProperty.Register("AudioTitle", typeof(string), typeof(SoundDeviceManager),
                new FrameworkPropertyMetadata(""));
        public string AudioTitle
        {
            get { return (string)GetValue(AudioTitleProperty); }
            set { SetValue(AudioTitleProperty, value); }
        }

        public static readonly DependencyProperty AudioDeviceListProperty =
            DependencyProperty.Register("AudioDeviceList", typeof(ObservableCollection<Device>), typeof(SoundDeviceManager),
                new FrameworkPropertyMetadata(null));
        public ObservableCollection<Device> AudioDeviceList
        {
            get { return (ObservableCollection<Device>)GetValue(AudioDeviceListProperty); }
            set
            {
                SetValue(AudioDeviceListProperty, value);
                OnPropertyChanged("AudioDeviceList");
            }
        }
        #endregion

        public SoundDeviceManager()
        {
            DataContext = this;
            InitializeComponent();
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }

    public class DeviceTypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return Application.Current.FindResource("appbar_3d_3ds");
            if(!(value is DeviceIcon))
                return Application.Current.FindResource("appbar_question");

            switch ((DeviceIcon)value)
            {
                case DeviceIcon.Speakers:
                    return Application.Current.FindResource("appbar_sound_2");
                case DeviceIcon.DesktopMicrophone:
                    return Application.Current.FindResource("appbar_microphone");
                case DeviceIcon.Phone:
                    return Application.Current.FindResource("appbar_phone");                
                case DeviceIcon.Monitor:
                    return Application.Current.FindResource("appbar_monitor");
                case DeviceIcon.Headphones:
                    return Application.Current.FindResource("appbar_hardware_headphones");
                case DeviceIcon.Headset:
                    return Application.Current.FindResource("appbar_hardware_headset");
                case DeviceIcon.Kinect:
                case DeviceIcon.Unknown:
                case DeviceIcon.Digital:
                case DeviceIcon.StereoMix:
                case DeviceIcon.LineIn:
                default:
                    return Application.Current.FindResource("appbar_question");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
