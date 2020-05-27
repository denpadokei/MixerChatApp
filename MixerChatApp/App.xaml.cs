using Prism.Ioc;
using MixerChatApp.Views;
using System.Windows;
using Prism.Modularity;
using MixerChatApp.Core;
using MixerChatApp.Home;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MixerChatApp.Core.Models;

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
            moduleCatalog.AddModule<CoreModule>();
            moduleCatalog.AddModule<HomeModule>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ILoggerFactory, NullLoggerFactory>();
        }
    }
}
