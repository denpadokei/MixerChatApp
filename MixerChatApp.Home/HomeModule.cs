using MixerChatApp.Core;
using MixerChatApp.Home.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace MixerChatApp.Home
{
    public class HomeModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var rm = containerProvider.Resolve<IRegionManager>();
            rm.RegisterViewWithRegion(RegionName.MainContentRegion, typeof(MainContent));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}