using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Forms = System.Windows.Forms;

namespace ProgramDrawer.UserControls
{
    /// <summary>
    /// Interaction logic for AddProgramControl.xaml
    /// </summary>
    public partial class AddProgramControl : UserControl, INotifyPropertyChanged
    {
        #region Properties
        private string _programName;
        public string ProgramName
        {
            get { return _programName; }
            set { _programName = value; OnPropertyChanged("ProgramName"); }
        }

        private string _programLocation;
        public string ProgramLocation
        {
            get { return _programLocation; }
            set { _programLocation = value; OnPropertyChanged("BannerImageLocation"); }
        }

        private string _bannerImageLocation;
        public string BannerImageLocation
        {
            get { return _bannerImageLocation; }
            set { _bannerImageLocation = value; OnPropertyChanged("BannerImageLocation"); }
        }
        #endregion

        #region Events
        public event EventHandler Cancel;
        protected void OnCancel()
        {
            Cancel?.Invoke(this, new EventArgs());
        }

        public event EventHandler Save;
        protected void OnSave()
        {
            Save?.Invoke(this, new EventArgs());
        }
        #endregion

        public AddProgramControl()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void SelectProgramLocation(object sender, RoutedEventArgs e)
        {
            Forms.OpenFileDialog dlg = new Forms.OpenFileDialog
            {
                Filter = "Executables (.EXE)|*.EXE"
                // TODO setup stuff here
            };

            // Drawer closes if this gets called
            // Possible solution is to have a lock for drawer and then release it afterwards and return focus

            if (dlg.ShowDialog() == Forms.DialogResult.OK)
            {
                ProgramLocation = dlg.FileName;
            }
        }

        private void SelectBannerImage(object sender, RoutedEventArgs e)
        {
            Forms.OpenFileDialog dlg = new Forms.OpenFileDialog
            {
                Filter = "Image Files (.JPG,.PNG)|*.JPG,*.PNG"
                // TODO setup stuff here
            };

            // Drawer closes if this gets called

            if (dlg.ShowDialog() == Forms.DialogResult.OK)
            {
                BannerImageLocation = dlg.FileName;
            }
        }

        private void CreateProgramItem(object sender, RoutedEventArgs e)
        {
            OnSave();
        }

        private void CancelCreation(object sender, RoutedEventArgs e)
        {
            OnCancel();
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
