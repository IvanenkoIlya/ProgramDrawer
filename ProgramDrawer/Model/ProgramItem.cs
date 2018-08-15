using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Windows;

namespace ProgramDrawer.Model
{
    public class ProgramItem : ProgramItemBase
    {
        public string ProgramLocation { get; set; }

        [JsonConstructor]    
        public ProgramItem(string ProgramName, string ProgramLocation, string ImageLocation = "")
        {
            this.ProgramName = ProgramName;
            this.ProgramLocation = ProgramLocation;
            this.ImageLocation = ImageLocation;

            UpdateBannerImage();
        }

        public override void ChangeProperties(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public override void LaunchProgram(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(ProgramLocation);
            } catch(InvalidOperationException ex)
            {
                Debug.WriteLine(ex);
                // TODO handle exception and possibly animate
            }   
        }
    }
}
