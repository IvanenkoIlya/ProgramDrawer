using ProgramDrawer.Model;
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
    public partial class EditableProgramItemControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ProgramItemProperty =
            DependencyProperty.Register("ProgramItem", typeof(ProgramItemBase), typeof(EditableProgramItemControl),
                new FrameworkPropertyMetadata(new ProgramItem("Default Program Name", "")));
        public ProgramItemBase ProgramItem
        {
            get { return (ProgramItemBase)GetValue(ProgramItemProperty); }
            set
            {
                SetValue(ProgramItemProperty, value);
                if(IsInitialized && value is SteamProgramItem)
                    ProgramLocationGrid.Visibility = Visibility.Collapsed;
                OnPropertyChanged("ProgramItem");
            }
        }

        #region Events
        public event EventHandler Cancel;
        private void CancelProgramItem(object sender, RoutedEventArgs e)
        {
            Cancel?.Invoke(this, new EventArgs());
        }

        public event EventHandler Save;
        private void SaveProgramItem(object sender, RoutedEventArgs e)
        {
            Save?.Invoke(this, new EventArgs());
        }
        #endregion

        public EditableProgramItemControl()
        {
            DataContext = ProgramItem;
            InitializeComponent();
            if (ProgramItem is SteamProgramItem)
                ProgramLocationGrid.Visibility = Visibility.Collapsed;
        }

        public EditableProgramItemControl(ProgramItemBase programItem)
        {
            ProgramItem = programItem;
            DataContext = ProgramItem;
            InitializeComponent();
            if (ProgramItem is SteamProgramItem)
                ProgramLocationGrid.Visibility = Visibility.Collapsed;
        }

        private void SelectProgramLocation(object sender, RoutedEventArgs e)
        {
            Forms.OpenFileDialog dlg = new Forms.OpenFileDialog
            {
                Filter = "Executables (.EXE)|*.EXE"
            };

            (Application.Current.MainWindow as MainWindow).LockDrawer();

            if (dlg.ShowDialog() == Forms.DialogResult.OK && ProgramItem is ProgramItem)
            {
                (ProgramItem as ProgramItem).ProgramLocation = dlg.FileName;
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
            Forms.OpenFileDialog dlg = new Forms.OpenFileDialog
            {
                InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"Resources\ProgramBanners"),
                Filter = "Image Files (.JPG,.PNG)|*.JPG;*.PNG"
            };

            (Application.Current.MainWindow as MainWindow).LockDrawer();

            if (dlg.ShowDialog() == Forms.DialogResult.OK)
                ProgramItem.ImageLocation = dlg.FileName;

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
