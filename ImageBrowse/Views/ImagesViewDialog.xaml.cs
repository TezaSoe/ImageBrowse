using ImageBrowse.PhotoCollection;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;

namespace ImageBrowse.Views
{
    /// <summary>
    /// Setting.xaml の相互作用ロジック
    /// </summary>
    public partial class ImagesViewDialog : Window
    {
        //public static int photoCount = 0;
        public PhotoCollection.PhotoCollection Photos;
        public string SelectedFolderPath { get; set; }
        private ObservableCollection<string> _files = new ObservableCollection<string>();
        public ObservableCollection<string> Files
        {
            get
            {
                return _files;
            }
        }
        bool popUpIsClosable = false;

        public ImagesViewDialog()
        {
            try
            {
                InitializeComponent();
                PhotosGroupBox.Style = this.FindResource("default") as Style;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void WindowPopup_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ZoomSlider.Visibility = Visibility.Collapsed;
                Photos = (PhotoCollection.PhotoCollection)(Resources["Photos"] as ObjectDataProvider)?.Data;
                RefreshPhoto(null, null);
                PhotosListBox.Focus();

                toggleWithPopup.Checked += ToggleWithPopup_Checked;
                toggleWithPopup.Unchecked += ToggleWithPopup_UnChecked;
                toggleWithPopup.MouseEnter += ToggleWithPopup_MouseEnter;
                toggleWithPopup.MouseLeave += ToggleWithPopup_MouseLeave;
                ZoomSlider.LostFocus += ZoomSlider_LostFocus;
                ZoomSlider.LostKeyboardFocus += ZoomSlider_LostKeyboardFocus;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Toggle Button Checked Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleWithPopup_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ZoomSlider.Visibility = Visibility.Visible;
                ZoomSlider.Focus();
                popUpIsClosable = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ToggleWithPopup UnChecked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleWithPopup_UnChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                ZoomSlider.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ToggleWithPopup MouseEnter Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleWithPopup_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ToggleWithPopup MouseLeave Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleWithPopup_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                popUpIsClosable = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ZoomSlider LostFocus Events
        /// </summary>
        string nextFocusName = "";
        private void ZoomSlider_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (nextFocusName.Equals(toggleWithPopup.Name)) return;
                toggleWithPopup.IsChecked = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ZoomSlider LostKeyboardFocus Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomSlider_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                nextFocusName = ((FrameworkElement)e?.NewFocus)?.Name;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void _ImagesViewDialog_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (popUpIsClosable)
                {
                    toggleWithPopup.IsChecked = false;
                    popUpIsClosable = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void DropBox_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effects = DragDropEffects.Copy;
                    PhotosGroupBox.Style = this.FindResource("change") as Style;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void DropBox_DragLeave(object sender, DragEventArgs e)
        {
            try
            {
                PhotosGroupBox.Style = this.FindResource("default") as Style;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void DropBox_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    _files.Clear();
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                    CopyFiles(files);
                }

                PhotosGroupBox.Style = this.FindResource("default") as Style;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Application Commands Binding
        public void PasteCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                // Get the DataObject.
                IDataObject data_object = Clipboard.GetDataObject();

                // Look for a file drop.
                if (data_object.GetDataPresent(DataFormats.FileDrop))
                {
                    e.CanExecute = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void PasteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                // Paste.
                // Clear the ListBox.
                List<string> lstFiles = new List<string>();

                // Get the DataObject.
                IDataObject data_object = Clipboard.GetDataObject();

                // Look for a file drop.
                if (data_object.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])data_object.GetData(DataFormats.FileDrop);
                    CopyFiles(files);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void CopyFiles(string[] files)
        {
            try
            {
                bool IsCopied = false;
                List<string> duplicateFiles = new List<string>();
                List<string> missFileTypes = new List<string>();
                List<string> missSource = new List<string>();

                foreach (var file in files)
                {
                    string filename = Path.GetFileName(file);
                    string destinationFullPath = $@"{SelectedFolderPath}\{filename}";

                    string FileExtention = Path.GetExtension(file).ToLower();
                    bool IsOKFileExtention = (FileExtention.Equals(".jpg") || FileExtention.Equals(".jpeg") || FileExtention.Equals(".png") || FileExtention.Equals(".tif") || FileExtention.Equals(".tiff") || FileExtention.Equals(".pdf")) ? true : false;
                    // Duplicate Check
                    if (File.Exists(destinationFullPath))
                    {
                        duplicateFiles.Add(file);
                    }
                    // File Type Error
                    else if (!IsOKFileExtention)
                    {
                        missFileTypes.Add(file);
                    }
                    // Not Found Source
                    else if (!File.Exists(file))
                    {
                        missSource.Add(file);
                    }
                    // Validation OK
                    else
                    {
                        File.Copy(file, destinationFullPath);
                        IsCopied = true;
                    }
                }
                bool IsError = false;
                List<string> errFilesList = new List<string>();
                //errFilesList.Add("下記のファイルがコピー先フォルダーにコピーされない状況です。");
                errFilesList.Add("ファイル選択時に、以下のエラーが発生しました。");
                errFilesList.Add("");
                if (duplicateFiles.Count > 0)
                {
                    IsError = true;
                    //errFilesList.Add("重複ファイルリスト");
                    errFilesList.Add("** 同一ファイル名エラー **");
                    errFilesList.Add("［選択するファイル名は一意にしてください。］");
                    foreach (string filename in duplicateFiles)
                    {
                        errFilesList.Add(filename);
                    }
                    errFilesList.Add("");
                }
                if (missFileTypes.Count > 0)
                {
                    IsError = true;
                    //errFilesList.Add("ファイルタイプエラーリスト");
                    errFilesList.Add("** ファイルタイプエラー **");
                    errFilesList.Add("［選択したファイルの拡張子は対応していません。］");
                    foreach (string filename in missFileTypes)
                    {
                        errFilesList.Add(filename);
                    }
                    errFilesList.Add("");
                }
                if (missSource.Count > 0)
                {
                    IsError = true;
                    //errFilesList.Add("ソースディレクトリが見つからないリスト");
                    errFilesList.Add("** コピー元不明エラー **");
                    errFilesList.Add("［選択したファイルが存在するか確認してください。］");
                    foreach (string filename in missSource)
                    {
                        errFilesList.Add(filename);
                    }
                    errFilesList.Add("");
                }

                if (IsError)
                {
                    //ChangeTrackingWrapper.ShowMessageBox(errFilesList);
                    StringBuilder sb = new StringBuilder();
                    foreach (string err in errFilesList)
                    {
                        sb.AppendLine(err);
                    }
                    MessageBox.Show(sb.ToString());
                }
                if (IsCopied)
                    RefreshPhoto(null, null);
            }
            catch (Exception ex)
            {
                //Handle exceptions - file not found, access denied, no internet connection etc etc
                throw new Exception(ex.Message);
            }
        }

        public void DeleteCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                if (PhotosListBox.SelectedItems.Count > 0)
                {
                    e.CanExecute = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                DeletePhoto(null, null);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public void ShiftDeleteCanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    if (!Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && PhotosListBox.SelectedItems.Count > 0)
        //    {
        //        e.CanExecute = true;
        //    }
        //}

        //public void ShiftDeleteExecuted(object sender, ExecutedRoutedEventArgs e)
        //{
        //    if (!Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
        //    {
        //        DeletePhoto(null, null);
        //    }
        //}

        private void DeletePhoto(object sender, RoutedEventArgs e)
        {
            try
            {
                var SelectedPhotos = PhotosListBox.SelectedItems;

                foreach (Photo SelectedPhoto in SelectedPhotos)
                {
                    string SelectedFilePath = SelectedPhoto.Source;
                    if (File.Exists(SelectedFilePath))
                    {
                        File.Delete(SelectedFilePath);
                    }
                }
                RefreshPhoto(null, null);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RefreshCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                e.CanExecute = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RefreshExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                RefreshPhoto(sender, e);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void RefreshPhoto(object sender, RoutedEventArgs e)
        {
            try
            {
                Photos.Path = SelectedFolderPath;
                FilesCount.Content = Photos.Count + " 個の項目";

                //if (Photos.Count > 0)
                //{
                //    photoCount = Photos.Count;
                //    FilesCount.Content = "【 " + photoCount + " 】ファイルを見つかりました。";
                //}
                //else
                //{
                //    photoCount = 0;
                //    FilesCount.Content = "該当ファイルが見つかりませんでした。";
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new CommonOpenFileDialog("画像ファイルの選択");
                //dialog.InitialDirectory = SelectedFolderPath;
                dialog.Multiselect = true;

                dialog.Filters.Add(new CommonFileDialogFilter("All Scanned Files", "*.JPG;*.JPEG;*.PNG;*.TIF;*.TIFF;*.PDF"));
                dialog.Filters.Add(new CommonFileDialogFilter("JPEG Files", "*.JPG;*.JPEG"));// Joint Photographic Experts Group
                dialog.Filters.Add(new CommonFileDialogFilter("PNG Files", "*.PNG"));// Portable Network Graphic
                dialog.Filters.Add(new CommonFileDialogFilter("Tagged Image Files", "*.TIF;*.TIFF"));// Tagged Image File Format
                dialog.Filters.Add(new CommonFileDialogFilter("PDF Files", "*.PDF"));// Portable Document Format

                // ダイアログを表示
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    string[] files = dialog.FileNames.ToArray();
                    CopyFiles(files);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region SizeChange with proportions
        //private double _aspectRatio;
        //private bool? _adjustingHeight = null;

        //internal enum SWP
        //{
        //    NOMOVE = 0x0002
        //}
        //internal enum WM
        //{
        //    WINDOWPOSCHANGING = 0x0046,
        //    EXITSIZEMOVE = 0x0232,
        //}

        //[StructLayout(LayoutKind.Sequential)]
        //internal struct WINDOWPOS
        //{
        //    public IntPtr hwnd;
        //    public IntPtr hwndInsertAfter;
        //    public int x;
        //    public int y;
        //    public int cx;
        //    public int cy;
        //    public int flags;
        //}

        //[DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //internal static extern bool GetCursorPos(ref Win32Point pt);

        //[StructLayout(LayoutKind.Sequential)]
        //internal struct Win32Point
        //{
        //    public Int32 X;
        //    public Int32 Y;
        //};

        //public static Point GetMousePosition() // mouse position relative to screen
        //{
        //    Win32Point w32Mouse = new Win32Point();
        //    GetCursorPos(ref w32Mouse);
        //    return new Point(w32Mouse.X, w32Mouse.Y);
        //}

        //protected override void OnSourceInitialized(EventArgs e)
        //{
        //    HwndSource hwndSource = (HwndSource)HwndSource.FromVisual(this);
        //    hwndSource.AddHook(DragHook);

        //    _aspectRatio = this.Width / this.Height;
        //}

        //private IntPtr DragHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        //{
        //    switch ((WM)msg)
        //    {
        //        case WM.WINDOWPOSCHANGING:
        //            {
        //                WINDOWPOS pos = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));

        //                if ((pos.flags & (int)SWP.NOMOVE) != 0)
        //                    return IntPtr.Zero;

        //                Window wnd = (Window)HwndSource.FromHwnd(hwnd).RootVisual;
        //                if (wnd == null)
        //                    return IntPtr.Zero;

        //                // determine what dimension is changed by detecting the mouse position relative to the 
        //                // window bounds. if gripped in the corner, either will work.
        //                if (!_adjustingHeight.HasValue)
        //                {
        //                    Point p = GetMousePosition();

        //                    double diffWidth = Math.Min(Math.Abs(p.X - pos.x), Math.Abs(p.X - pos.x - pos.cx));
        //                    double diffHeight = Math.Min(Math.Abs(p.Y - pos.y), Math.Abs(p.Y - pos.y - pos.cy));

        //                    _adjustingHeight = diffHeight > diffWidth;
        //                }

        //                if (_adjustingHeight.Value)
        //                    pos.cy = (int)(pos.cx / _aspectRatio); // adjusting height to width change
        //                else
        //                    pos.cx = (int)(pos.cy * _aspectRatio); // adjusting width to heigth change

        //                Marshal.StructureToPtr(pos, lParam, true);
        //                handled = true;
        //            }
        //            break;
        //        case WM.EXITSIZEMOVE:
        //            _adjustingHeight = null; // reset adjustment dimension and detect again next time window is resized
        //            break;
        //    }

        //    return IntPtr.Zero;
        //}
        #endregion
    }
}
