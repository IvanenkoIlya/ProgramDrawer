using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace ProgramDrawer.UserControls
{
    /// <summary>
    /// Interaction logic for SoundDevicesControl.xaml
    /// </summary>
    public partial class SoundDevicesControl : UserControl
    {
        public SoundDevicesControl()
        {
            InitializeComponent();

            OutputDevices.AudioDeviceList = new ObservableCollection<Device>(new CoreAudioController().GetPlaybackDevices().Where(x => x.State == DeviceState.Active));
            InputDevices.AudioDeviceList = new ObservableCollection<Device>(new CoreAudioController().GetCaptureDevices().Where(x => x.State == DeviceState.Active));
        }
    }
}
