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

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IChatService, ChatService>();
            containerRegistry.RegisterSingleton<IBouyomiService, BouyomiService>();
        }
    }
}