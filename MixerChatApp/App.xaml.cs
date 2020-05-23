using Prism.Ioc;
using MixerChatApp.Views;
using System.Windows;
using Prism.Modularity;
using MixerChatApp.Core;
using MixerChatApp.Home;

namespace MixerChatApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<CoreModule>();
            moduleCatalog.AddModule<HomeModule>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
