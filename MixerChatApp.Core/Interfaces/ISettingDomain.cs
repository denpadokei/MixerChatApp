using System;
using System.Collections.Generic;
using System.Text;

namespace MixerChatApp.Core.Interfaces
{
    public interface ISettingDomain
    {
        bool IsSending { get; set; }
        bool IsSaveUserInformation { get; set; }
        int BouyomiPort { get; set; }
        string BouyomiHost { get; set; }
        void Save();
        void SaveToken();
    }
}
