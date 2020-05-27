using Microsoft.Mixer.ShortcodeOAuth;
using MixerChatApp.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MixerChatApp.Core.Services
{
    public class FileService : IFileService
    {
        public void Save(ISettingDomain domain)
        {
            var jsontext = JsonConvert.SerializeObject(domain, Formatting.Indented);
            using (var sw = new StreamWriter(@".\appsettings.json")) {
                sw.Write(jsontext);
                sw.Close();
            }
        }

        public void SaveToken(OAuthTokens tokens, bool isDelete)
        {
            try {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (!Directory.Exists(Path.Combine(path, "mixerchat"))) {
                    Directory.CreateDirectory(Path.Combine(path, "mixerchat"));
                }

                if (!isDelete && tokens != null) {
                    using (var sw = new StreamWriter(Path.Combine(path, TEMPFILENAME))) {
                        sw.Write(JsonConvert.SerializeObject(tokens, Formatting.Indented));
                        sw.Close();
                    }
                }
                else {
                    var file = new FileInfo(Path.Combine(path, TEMPFILENAME));
                    file.Delete();
                }
            }
            catch (Exception e) {
                Debug.WriteLine($"{e}");
            }
        }

        public OAuthTokens ReadTokens()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (!File.Exists(Path.Combine(path, TEMPFILENAME))) {
                return null;
            }
            using (var sr = new StreamReader(Path.Combine(path, TEMPFILENAME))) {
                return JsonConvert.DeserializeObject<OAuthTokens>(sr.ReadToEnd());
            }
        }

        private static readonly string TEMPFILENAME = @"mixerchat\e05kj2W8bYEmQT0B";
    }
}
