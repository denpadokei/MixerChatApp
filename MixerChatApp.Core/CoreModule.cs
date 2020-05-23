using MixerChatApp.Core.Interfaces;
using MixerChatApp.Core.Services;
using MixerChatApp.Core.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace MixerChatApp.Core
{
    public class CoreModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var rm = containerProvider.Resolve<IRegionManager>();
            rm.RegisterViewWithRegion(RegionName.SettingRegionName, typeof(Setting));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IChatService, ChatService>();
            containerRegistry.RegisterSingleton<IBouyomiService, BouyomiService>();
        }
    }
}