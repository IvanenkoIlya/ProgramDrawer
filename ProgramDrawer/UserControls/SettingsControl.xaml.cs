using MahApps.Metro;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ProgramDrawer.UserControls
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        Settings settings;

        public SettingsControl()
        {
            settings = new Settings()
            {
                SelectedAccent = ThemeManager.DetectAppStyle(Application.Current).Item2
            };   
            
            DataContext = settings;
            InitializeComponent();
        }

        private void ApplySettings(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, settings.SelectedAccent, 
                settings.DarkTheme ? ThemeManager.GetAppTheme("BaseDark") : ThemeManager.GetAppTheme("BaseLight"));
        }

        private void CancelSettings(object sender, RoutedEventArgs e)
        {
            Tuple<AppTheme, Accent> tuple = ThemeManager.DetectAppStyle(Application.Current);

            settings.DarkTheme = tuple.Item1 == ThemeManager.GetAppTheme("BaseDark");

            settings.SelectedAccent = tuple.Item2;
        }
    }

    public class Settings : INotifyPropertyChanged
    {
        private bool _darkTheme = true;
        public bool DarkTheme
        {
            get { return _darkTheme; }
            set { _darkTheme = value; OnPropertyChanged("DarkTheme"); }
        }

        private Accent _selectedAccent;
        public Accent SelectedAccent
        {
            get { return _selectedAccent; }
            set { _selectedAccent = value; OnPropertyChanged("SelectedAccent"); }
        }

        public ObservableCollection<Accent> Accents
        {
            get { return new ObservableCollection<Accent>(ThemeManager.Accents); }
        }

        #region PropertyChanged event handler
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
