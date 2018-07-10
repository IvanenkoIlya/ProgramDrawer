using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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
            set { _programLocation = value; OnPropertyChanged("ProgramLocation"); }
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
        private void CancelCreation(object sender, RoutedEventArgs e)
        {
            Cancel?.Invoke(this, new EventArgs());
        }

        public event EventHandler Save;
        private void CreateProgramItem(object sender, RoutedEventArgs e)
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

            (Application.Current.MainWindow as MainWindow).LockDrawer();

            if (dlg.ShowDialog() == Forms.DialogResult.OK)
            {
                ProgramLocation = dlg.FileName;
            }

            try
            {
                (Application.Current.MainWindow as MainWindow).UnlockDrawer();
            } catch( NullReferenceException ex)
            {
                // TODO log exception
            }
            
        }

        private void SelectBannerImage(object sender, RoutedEventArgs e)
        {
            string temp1 = Directory.GetCurrentDirectory();
            string temp2 = @"Resources\ProgramBanners";

            string temp3 = Path.Combine(temp1,temp2);

            Forms.OpenFileDialog dlg = new Forms.OpenFileDialog
            {
                InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"Resources\ProgramBanners"),
                Filter = "Image Files (.JPG,.PNG)|*.JPG;*.PNG"
                // TODO setup stuff here
            };

            (Application.Current.MainWindow as MainWindow).LockDrawer();

            if (dlg.ShowDialog() == Forms.DialogResult.OK)
            {
                BannerImageLocation = dlg.FileName;
            }

            try
            {
                (Application.Current.MainWindow as MainWindow).UnlockDrawer();
            } catch( NullReferenceException ex)
            {
                // TODO log exception
            }
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
