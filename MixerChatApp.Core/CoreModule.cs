using MixerChatApp.Core.APIs;
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
            rm.RegisterViewWithRegion(RegionName.BouyomiRegionName, typeof(BouyomiSetting));
            rm.RegisterViewWithRegion(RegionName.MixerRegionName, typeof(MixerSetting));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IChatService, ChatService>();
            containerRegistry.RegisterSingleton<IBouyomiService, BouyomiService>();
            containerRegistry.RegisterSingleton<IOAuthManagerable, OAuthManager>();
            containerRegistry.RegisterSingleton<ISettingDomain, SettingDomain>();
            containerRegistry.RegisterDialog<Setting>(RegionName.SettingRegionName);
            containerRegistry.RegisterInstance(new MixerAPI());
        }
    }
}