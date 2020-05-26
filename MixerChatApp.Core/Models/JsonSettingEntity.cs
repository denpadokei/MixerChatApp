using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MixerChatApp.Core.Models
{
    public class JsonSettingEntity
    {
        [JsonProperty("IsSending")]
        public bool IsSending { get; set; }
        [JsonProperty("IsSaveUserInformation")]
        public bool IsSaveUserInformation { get; set; }
    }
}
