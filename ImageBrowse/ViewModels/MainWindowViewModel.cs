using Microsoft.Practices.Unity;
using Prism.Mvvm;
using Prism.Regions;

namespace ImageBrowse.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        [Dependency]
        public IRegionManager RegionManager { get; set; }
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {
            Title = "ImageGalary";
        }
    }
}
