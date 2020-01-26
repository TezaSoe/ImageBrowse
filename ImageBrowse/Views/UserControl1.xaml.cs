using System;
using System.Windows;
using System.Windows.Controls;

namespace ImageBrowse.Views
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        const double maxControlWidth = 1912;//1920
        const double maxControlHeight = 969;//1080
        private void ThumbnailView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // To Call ViewModel Command Firstly.
                //var btn = sender as Button;
                //btn.Command.Execute("Load");

                var el = (sender as FrameworkElement);
                //var parentWin = Window.GetWindow(this);
                //var currentWidth = parentWin.ActualWidth / 1920 * 1100;
                //var currentHeight = parentWin.ActualHeight / 1040 * 670;
                //if (maxControlHeight > currentHeight)
                //    currentHeight = currentHeight - (maxControlHeight - currentHeight) * 0.04;

                var pvWindow = new ImagesViewDialog
                {
                    SelectedFolderPath = $@"{FolderPath.Text}",
                    //Width = currentWidth,
                    //Height = currentHeight
                };
                pvWindow.Owner = Window.GetWindow(el);
                pvWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
