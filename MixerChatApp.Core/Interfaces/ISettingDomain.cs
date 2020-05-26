using System;
using System.Collections.Generic;
using System.Text;

namespace MixerChatApp.Core.Interfaces
{
    public interface ISettingDomain
    {
        bool IsSending { get; set; }
        bool IsSaveUserInformation { get; set; }
        void Save();
        void SaveToken();
    }
}
