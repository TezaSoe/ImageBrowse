using Microsoft.Practices.Unity;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.IO;

namespace ImageBrowse.ViewModels
{
    public class UserControl1ViewModel : BindableBase
    {
        [Dependency]
        public IRegionManager RegionManager { get; set; }

        private string _FolderPath;
        public string FolderPath
        {
            get { return _FolderPath; }
            set { SetProperty(ref _FolderPath, value); }
        }

        public UserControl1ViewModel()
        {
        }

        /// <summary>
        /// Image Folder Path Add Event
        /// </summary>
        /// ▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
        private DelegateCommand _ImageFolderSelectCommand;
        public DelegateCommand ImageFolderSelectCommand =>
            _ImageFolderSelectCommand ?? (_ImageFolderSelectCommand = new DelegateCommand(ImageFolderSelectEvent));

        void ImageFolderSelectEvent()
        {
            try
            {
                // Create dialog instance
                var dialog = new CommonOpenFileDialog("Select folder");

                if (!string.IsNullOrEmpty(FolderPath))
                    dialog.InitialDirectory = FolderPath;

                // Set the IsFolderPicker into folder style property to make the selection format
                dialog.IsFolderPicker = true;

                // Show dialog
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    FolderPath = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// ▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲
    }
}
