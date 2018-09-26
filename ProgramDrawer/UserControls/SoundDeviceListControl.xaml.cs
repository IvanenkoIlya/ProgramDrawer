using AudioSwitcher.AudioApi;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProgramDrawer.UserControls
{
    /// <summary>
    /// Interaction logic for SoundDeviceManager.xaml
    /// </summary>
    public partial class SoundDeviceListControl : UserControl, INotifyPropertyChanged
    {
        #region DependencyProperties
        public static readonly DependencyProperty AudioTitleProperty =
            DependencyProperty.Register("AudioTitle", typeof(string), typeof(SoundDeviceListControl),
                new FrameworkPropertyMetadata(""));
        public string AudioTitle
        {
            get { return (string)GetValue(AudioTitleProperty); }
            set { SetValue(AudioTitleProperty, value); }
        }

        public static readonly DependencyProperty AudioDeviceListProperty =
            DependencyProperty.Register("AudioDeviceList", typeof(ObservableCollection<Device>), typeof(SoundDeviceListControl),
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

        public static readonly DependencyProperty SelectedAudioDeviceProperty =
            DependencyProperty.Register("SelectedaudioDevice", typeof(Device), typeof(SoundDeviceListControl),
                new FrameworkPropertyMetadata(null));
        public Device SelectedAudioDevice
        {
            get { return (Device)GetValue(SelectedAudioDeviceProperty); }
            set
            {
                SetValue(SelectedAudioDeviceProperty, value);
                OnPropertyChanged("SelectedAudioDevice");
            }
        }
        #endregion


        public SoundDeviceListControl()
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

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((Device)e.AddedItems[0]).SetAsDefault();
            ((Device)e.AddedItems[0]).SetAsDefaultCommunications();
        }
    }

    public class DeviceTypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
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
