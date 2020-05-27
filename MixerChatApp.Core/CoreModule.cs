using Microsoft.Extensions.Configuration;
using MixerChatApp.Core.APIs;
using MixerChatApp.Core.Interfaces;
using MixerChatApp.Core.Models;
using MixerChatApp.Core.Services;
using MixerChatApp.Core.Views;
using Newtonsoft.Json;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.IO;

namespace MixerChatApp.Core
{
    public class CoreModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var rm = containerProvider.Resolve<IRegionManager>();
            rm.RegisterViewWithRegion(RegionName.BouyomiRegionName, typeof(BouyomiSetting));
            rm.RegisterViewWithRegion(RegionName.MixerRegionName, typeof(MixerSetting));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            var entity = new JsonEntity();
            if (!File.Exists(path)) {
                using (var fs = File.CreateText(path)) {
                    entity.BouyomiPort = 50001;
                    entity.BouyomiHost = "127.0.0.1";
                    var text = JsonConvert.SerializeObject(entity, Formatting.Indented);
                    fs.Write(text);
                    fs.Close();
                }
            }
            var bulder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = bulder.Build();
            containerRegistry.RegisterInstance(configuration);
            containerRegistry.RegisterSingleton<IChatService, ChatService>();
            containerRegistry.RegisterSingleton<IBouyomiService, BouyomiService>();
            containerRegistry.RegisterSingleton<IOAuthManagerable, OAuthManager>();
            containerRegistry.Register<IFileService, FileService>();
            containerRegistry.RegisterSingleton<ISettingDomain, SettingDomain>();
            containerRegistry.RegisterDialog<Setting>(RegionName.SettingRegionName);
            containerRegistry.RegisterInstance(new MixerAPI());
        }

        private class JsonEntity : ISettingDomain
        {
            public bool IsSending { get; set; }
            public bool IsSaveUserInformation { get; set; }
            public int BouyomiPort { get; set; }
            public string BouyomiHost { get; set; }

            public void Save()
            {
                throw new System.NotImplementedException();
            }

            public void SaveToken()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}