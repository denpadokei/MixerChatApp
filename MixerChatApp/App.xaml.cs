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
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            if (!File.Exists(path)) {
                using (var fs = File.CreateText(path)) {
                    var text = JsonConvert.SerializeObject(new SettingEntity(), Formatting.Indented);
                    fs.Write(text);
                    fs.Close();
                }
            }
            var bulder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = bulder.Build();
            containerRegistry.RegisterInstance(configuration);
            containerRegistry.Register<ILoggerFactory, NullLoggerFactory>();
        }

        private class SettingEntity
        {
            [JsonProperty("AcsessToken")]
            public string AcsessToken { get; set; }
            [JsonProperty("ExporesAt")]
            public string ExporesAt { get; set; }
        }
    }
}
