using AnimatedListView;
using ProgramDrawer.Model;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace ProgramDrawer.UserControls
{
    /// <summary>
    /// Interaction logic for ProgramItemControl.xaml
    /// </summary>
    public partial class ProgramItemControl : UserControl
    {
        private double fadeTime = 300;
        
        #region ProgramItemProperty
        public static readonly DependencyProperty ProgramItemProperty =
            DependencyProperty.Register("ProgramItem", typeof(ProgramItemBase), typeof(ProgramItemControl),
                new FrameworkPropertyMetadata(new ProgramItem("Default Program Name", "")));
        public ProgramItemBase ProgramItem
        {
            get { return (ProgramItemBase)GetValue(ProgramItemProperty); }
            set { SetValue(ProgramItemProperty, value); }
        }
        #endregion

        #region ParentProgramListProperty
        public static readonly DependencyProperty ParentProgramListProperty =
            DependencyProperty.Register("ParentProgramList", typeof(ItemsControl), typeof(ProgramItemControl),
                new FrameworkPropertyMetadata(null));
        public ItemsControl ParentProgramList
        {
            get { return (ItemsControl)GetValue(ParentProgramListProperty); }
            set { SetValue(ParentProgramListProperty, value); }
        }
        #endregion

        public ProgramItemControl()
        {
            InitializeComponent();
        }

        private void ToggleFavorite(object sender, MouseButtonEventArgs e)
        {
            ProgramItem.Favorite = !ProgramItem.Favorite;
        }

        private void LaunchProgram(object sender, MouseButtonEventArgs e)
        {
            ProgramItem.LaunchProgram(sender, e);
        }

        private void DeleteProgramItem(object sender, MouseButtonEventArgs e)
        {
            ((ObservableCollectionView<ProgramItemBase>)ParentProgramList.ItemsSource).Remove(ProgramItem);
        }

        private void EditProperties(object sender, MouseButtonEventArgs e)
        {
            if (EditProgramItemGrid.Children.Count == 0)
            {
                EditableProgramItemControl epic = new EditableProgramItemControl((ProgramItemBase)ProgramItem.Clone());

                epic.Cancel += CancelEdit;
                epic.Save += SaveProgramItem;

                EditProgramItemGrid.Children.Add(epic);
            }

            EditProgramItemGrid.Visibility = Visibility.Visible;

            DoubleAnimation fade = new DoubleAnimation(0.0, 1.0, TimeSpan.FromMilliseconds(fadeTime));
            fade.Completed += (send, args) => { DisplayProgramItemGrid.Visibility = Visibility.Hidden; };

            EditProgramItemGrid.ApplyAnimationClock(OpacityProperty,fade.CreateClock());
        }

        private void CancelEdit(object sender, EventArgs e)
        {
            CloseEditGrid();
        }

        private void SaveProgramItem(object sender, EventArgs e)
        {
            ProgramItemBase update = (sender as EditableProgramItemControl).ProgramItem;
            ProgramItem.ProgramName = update.ProgramName;
            ProgramItem.ImageLocation = update.ImageLocation;

            if (update is ProgramItem)
                (ProgramItem as ProgramItem).ProgramLocation = (update as ProgramItem).ProgramLocation;

            CloseEditGrid();
        }

        private void CloseEditGrid()
        {
            EditProgramItemGrid.Children.Clear();
            DisplayProgramItemGrid.Visibility = Visibility.Visible;

            DoubleAnimation fade = new DoubleAnimation(1.0, 0.0, TimeSpan.FromMilliseconds(fadeTime));
            fade.Completed += (send, args) => { EditProgramItemGrid.Visibility = Visibility.Hidden; };

            EditProgramItemGrid.ApplyAnimationClock(OpacityProperty, fade.CreateClock());
        }
    }

    public class FavoriteToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool input = false;
            if (value != null)
                input = (bool)value;
            return input ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
