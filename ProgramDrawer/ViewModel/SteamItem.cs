namespace ProgramDrawer.ViewModel
{
    public class SteamItem : ProgramItem
    {
        #region Properties
        private int _appID;
        public int AppID
        {
            get { return _appID; }
            set { _appID = value; OnPropertyChanged("AppID"); }
        }
        #endregion
    }
}
