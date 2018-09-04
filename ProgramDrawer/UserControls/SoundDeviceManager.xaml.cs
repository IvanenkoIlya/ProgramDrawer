using AudioSwitcher.AudioApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private ObservableCollection<Device> audioDevices;
        public ObservableCollection<Device> AudioDevices
        {
            get { return audioDevices; }
            set { audioDevices = value; OnPropertyChanged("AudioDevices"); }
        }

        public SoundDeviceManager()
        {
            DataContext = this;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
