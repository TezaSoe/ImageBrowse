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
        /// Image Folder Path追加イベント
        /// </summary>
        /// ▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
        private DelegateCommand _ImageFolderSelectCommand;
        public DelegateCommand ImageFolderSelectCommand =>
            _ImageFolderSelectCommand ?? (_ImageFolderSelectCommand = new DelegateCommand(ImageFolderSelectEvent));

        void ImageFolderSelectEvent()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new CommonOpenFileDialog("フォルダーの選択");

                if (!string.IsNullOrEmpty(FolderPath))
                    dialog.InitialDirectory = FolderPath;

                // 選択形式をフォルダースタイルにする IsFolderPicker プロパティを設定
                dialog.IsFolderPicker = true;

                // ダイアログを表示
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
