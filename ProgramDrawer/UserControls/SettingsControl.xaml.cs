using MahApps.Metro;
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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsControl : UserControl, INotifyPropertyChanged
    {
        #region Properties
        public ObservableCollection<Accent> Accents
        {
            get { return new ObservableCollection<Accent>(ThemeManager.Accents); }
        }

        private Accent _accent;
        public Accent Accent
        {
            get { return _accent; }
            set { _accent = value; OnPropertyChanged("Accent"); }
        }

        private AppTheme _appTheme;
        public AppTheme AppTheme
        {
            get { return _appTheme; }
            set { _appTheme = value; OnPropertyChanged("AppTheme"); }
        }
        #endregion

        public SettingsControl()
        {
            Accent = ThemeManager.GetAccent(Properties.Settings.Default.AccentColor);
            AppTheme = ThemeManager.GetAppTheme(Properties.Settings.Default.BaseColor);

            DataContext = this;
            InitializeComponent();

            Properties.Settings.Default.SettingsSaving += ApplySettings;
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion

        private void ApplySettings(object sender, CancelEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current,
                ThemeManager.GetAccent(Properties.Settings.Default.AccentColor),
                ThemeManager.GetAppTheme(Properties.Settings.Default.BaseColor));
        }

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.AccentColor = Accent.Name;
            Properties.Settings.Default.BaseColor = AppTheme.Name; 
            Properties.Settings.Default.Save();
        }

        private void CancelSettings(object sender, RoutedEventArgs e)
        {
            Accent = ThemeManager.GetAccent(Properties.Settings.Default.AccentColor);
            AppTheme = ThemeManager.GetAppTheme(Properties.Settings.Default.BaseColor);
        }
    }

    public class AppThemeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as AppTheme).Name == "BaseDark";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? ThemeManager.GetAppTheme("BaseDark") : ThemeManager.GetAppTheme("BaseLight");
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

        #region PropertyChanged event handler
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
