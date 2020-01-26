using ImageBrowse.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

namespace ImageBrowse
{
    public class ImageBrowseModule : IModule
    {
        private IRegionManager _regionManager;
        private IUnityContainer _container;

        public ImageBrowseModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            //View transition destination DI
            _container.RegisterTypeForNavigation<UserControl1>();

            //View DI
            _container.RegisterType<object, UserControl1>(nameof(UserControl1));

            _regionManager.RequestNavigate("MainRegion", nameof(UserControl1));
        }
    }
}
