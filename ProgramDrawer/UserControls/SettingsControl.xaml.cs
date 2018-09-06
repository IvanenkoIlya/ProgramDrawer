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
            settings = new Settings();
            RecreateSettings();

            DataContext = settings;
            InitializeComponent();

            Properties.Settings.Default.SettingsSaving += ApplySettings;
        }

        private void ApplySettings(object sender, CancelEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current,
                ThemeManager.GetAccent(Properties.Settings.Default.AccentColor),
                ThemeManager.GetAppTheme(Properties.Settings.Default.BaseColor));
        }

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.AccentColor = settings.SelectedAccent.Name;
            Properties.Settings.Default.BaseColor = settings.DarkTheme ? "BaseDark" : "BaseLight"; 
            Properties.Settings.Default.Save();
        }

        private void CancelSettings(object sender, RoutedEventArgs e)
        {
            Accent accent = ThemeManager.GetAccent(Properties.Settings.Default.AccentColor);

            RecreateSettings();
        }

        private void RecreateSettings()
        {
            settings.DarkTheme = Properties.Settings.Default.BaseColor == "BaseDark";
            settings.SelectedAccent = ThemeManager.GetAccent(Properties.Settings.Default.AccentColor);
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
