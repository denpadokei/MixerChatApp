using MixerLib;
using MixerLib.Events;
using System;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MixerChatApp.Core.Interfaces
{
    public interface IChatService : INotifyPropertyChanged, IDisposable
    {
        IAuthorization Auth { get; set; }
        IMixerClient Client { get; set; }
        public string ChannelName { get; set; }
        public string Token { get; set; }
        Task StartClient();
        Task SendMessage(string message);
    }
}
