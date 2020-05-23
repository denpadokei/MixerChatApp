using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace MixerChatApp.Core.Interfaces
{
    public interface IBouyomiService : INotifyPropertyChanged
    {
        public int Port { get; set; }
        public string Host { get; set; }
        Task SendMessage(string message);
    }
}
