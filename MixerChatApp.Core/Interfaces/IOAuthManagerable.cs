using Microsoft.Mixer.ShortcodeOAuth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace MixerChatApp.Core.Interfaces
{
    public interface IOAuthManagerable : INotifyPropertyChanged
    {
        string Code { get; set; }
        List<string> Scorp { get; set; }
        public OAuthTokens Tokens { get; set; }
        Task RunAsync();
    }
}
