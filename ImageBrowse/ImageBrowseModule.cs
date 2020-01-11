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
            //DatabaseContextのDI
            //_container.RegisterType<DbContext, DbContext>(new ContainerControlledLifetimeManager());

            //PropetyDI
            //_container.RegisterType<ISaveService, SaveService>("ByAll");

            //ServicesのDI
            //_container.RegisterType<IService, Service>();

            //RepositoriesのDI
            //_container.RegisterType<IVerifyFormRepository, VerifyFormRepository>();

            //Viewの遷移先のDI
            //_container.RegisterTypeForNavigation<MainWindow>();
            _container.RegisterTypeForNavigation<UserControl1>();

            //ViewのDI
            //_container.RegisterType<object, MainWindow>(nameof(MainWindow));
            _container.RegisterType<object, UserControl1>(nameof(UserControl1));

            //_container.RegisterInstance(typeof(ApplicationCommands), new ApplicationCommands());
            //_container.RegisterSingleton<ApplicationCommands>();

            _regionManager.RequestNavigate("MainRegion", nameof(UserControl1));
        }
    }
}
