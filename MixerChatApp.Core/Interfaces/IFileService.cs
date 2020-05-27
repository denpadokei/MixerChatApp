using Microsoft.Mixer.ShortcodeOAuth;
using System;
using System.Collections.Generic;
using System.Text;

namespace MixerChatApp.Core.Interfaces
{
    public interface IFileService
    {
        void Save(ISettingDomain domain);
        void SaveToken(OAuthTokens tokensbool, bool isDelete);

        OAuthTokens ReadTokens();
    }
}
