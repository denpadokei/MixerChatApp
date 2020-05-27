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
        OAuthClient Client { get; set; }
        string Code { get; set; }
        string UserName { get; set; }
        bool IsSaveUserInformation { get; set; }
        DateTimeOffset ConnectDate { get; set; }
        List<string> Scorp { get; set; }
        public OAuthTokens Tokens { get; set; }
        Task<bool> RunAsync();
        Task<bool> RefreshToken();
    }
}
