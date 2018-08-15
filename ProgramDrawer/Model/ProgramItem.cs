using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace ProgramDrawer.Model
{
    public class ProgramItem : ProgramItemBase
    {
        #region ProgramLocation
        private string _programLocation;
        public string ProgramLocation
        {
            get { return _programLocation; }
            set { _programLocation = value; OnPropertyChanged("ProgramLocation"); }
        }
        #endregion

        [JsonConstructor]    
        public ProgramItem(string ProgramName, string ProgramLocation, string ImageLocation = "")
        {
            this.ProgramName = ProgramName;
            this.ProgramLocation = ProgramLocation;
            this.ImageLocation = ImageLocation;

            UpdateBannerImage();
        }

        public override void LaunchProgram(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(ProgramLocation);
            } catch(InvalidOperationException invalidException)
            {
                Debug.WriteLine(ex);
                // TODO handle exception and possibly animate
            } catch(Win32Exception fileNotFoundException)
            {
                //TODO file not found
            }
        }

        public override object Clone()
        {
            return new ProgramItem(ProgramName, ProgramLocation, ImageLocation);
        }
    }
}
