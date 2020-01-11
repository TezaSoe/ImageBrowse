using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageBrowse.PhotoCollection
{
    /// <summary>
    /// This class represents a collection of photos in a directory.
    /// </summary>
    public class PhotoCollection : ObservableCollection<Photo>
    {
        private DirectoryInfo _directory;

        public PhotoCollection()
        {
        }

        public PhotoCollection(string path) : this(new DirectoryInfo(path))
        {
        }

        public PhotoCollection(DirectoryInfo directory)
        {
            _directory = directory;
            Update();
        }

        public string Path
        {
            set
            {
                _directory = new DirectoryInfo(value);
                Update();
            }
            get { return _directory.FullName; }
        }

        public DirectoryInfo Directory
        {
            set
            {
                _directory = value;
                Update();
            }
            get { return _directory; }
        }

        private void Update()
        {
            Clear();
            try
            {
                string[] filters = new[] { "*.jpg", "*.jpeg", "*.png", "*.tif", "*.tiff", "*.pdf" };
                var imageFiles = filters.SelectMany(f => System.IO.Directory.GetFiles(Path, f));
                foreach (var imageFile in imageFiles)
                    Add(new Photo(imageFile));
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                MessageBox.Show("No Such Directory");
            }
        }
    }

    /// <summary>
    ///     This class describes a single photo - its location, the image and
    ///     the metadata extracted from the image.
    /// </summary>
    public class Photo
    {
        private readonly Uri _source;

        public Photo(string path)
        {
            Source = path;
            FileName = Path.GetFileName(path);

            if (Path.GetExtension(path).ToLower().Equals(".pdf"))
            {
                IsPdf = true;
            }
            else
            {
                IsPdf = false;
                _source = new Uri(path);
                Thumbnail = CreateThumbnail(path);
            }
        }

        public bool IsPdf { get; set; }

        public string FileName { get; }

        public BitmapSource Thumbnail { get; set; }

        public string Source { get; }

        public override string ToString() => _source?.ToString();

        private BitmapSource CreateThumbnail(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = stream;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.DecodePixelWidth = 320;// 120;
                // bmp.DecodePixelHeight = 120; // alternatively, but not both
                bmp.EndInit();
                bmp.Freeze();
                return bmp;
            }
        }
    }
}
