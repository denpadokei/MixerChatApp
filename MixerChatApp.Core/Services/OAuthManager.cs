using Microsoft.Extensions.Configuration;
using Microsoft.Mixer.ShortcodeOAuth;
using MixerChatApp.Core.Interfaces;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace MixerChatApp.Core.Services
{
    public class OAuthManager : BindableBase, IOAuthManagerable
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        /// <summary>説明 を取得、設定</summary>
        private string cord_;
        /// <summary>説明 を取得、設定</summary>
        public string Code
        {
            get => this.cord_;

            set => this.SetProperty(ref this.cord_, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private List<string> scorp_;
        /// <summary>説明 を取得、設定</summary>
        public List<string> Scorp
        {
            get => this.scorp_;

            set => this.SetProperty(ref this.scorp_, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private OAuthTokens tokens_;
        /// <summary>説明 を取得、設定</summary>
        public OAuthTokens Tokens
        {
            get => this.tokens_;

            set => this.SetProperty(ref this.tokens_, value);
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // コマンド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // コマンド用メソッド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // オーバーライドメソッド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // パブリックメソッド
        public async Task RunAsync()
        {
            try {
                // Create your OAuth client. Specify your client ID, and which permissions you want.


#if DEBUG
                var bulder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(@".\appsettings.Develop.json");
                var configuration = bulder.Build();

                var clientid = configuration["ClientId"];
#else
                var clientid = this.CLIENT_ID;
#endif
                var client = new OAuthClient(
                    new OAuthOptions
                    {
                        ClientId = clientid,
                        Scopes = this.Scorp.ToArray(),
                    });
                // Use the helper GrantAsync to get codes. Alternately, you can run
                // the granting/polling loop manually using client.GetSingleCodeAsync.
                var task = client.GrantAsync(code =>
                {
                    this.Code = code;
                    var url = "https://mixer.com/go?code=" + $"{this.Code}";
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }, CancellationToken.None);
                this.Tokens = await task;
                // and that's it!
                Debug.WriteLine($"Access token: {this.Tokens.AccessToken}");
                Debug.WriteLine($"Refresh token: {this.Tokens.RefreshToken}");
                Debug.WriteLine($"Expires At: {this.Tokens.ExpiresAt}");
                var entity = new SettingEntity() { AcsessToken = this.Tokens.AccessToken, ExporesAt = $"{this.Tokens.ExpiresAt}" };
                var text = JsonConvert.SerializeObject(entity, Formatting.Indented);
                using (var sw = new StreamWriter(@".\appsettings.json")) {
                    sw.Write(text);
                    sw.Close();
                }
            }
            catch (OAuthException oe) {
                Debug.WriteLine($"{oe.Message}");
            }
            catch (Exception e) {
                Debug.WriteLine($"{e.Message}");
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        private string CLIENT_ID = "your client id";
        private class SettingEntity
        {
            [JsonProperty("AcsessToken")]
            public string AcsessToken { get; set; }
            [JsonProperty("ExporesAt")]
            public string ExporesAt { get; set; }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        public OAuthManager()
        {
            this.Scorp = new List<string>()
            {
                "channel:update:self",
                "chat:bypass_links",
                "chat:bypass_slowchat",
                "chat:change_ban",
                "chat:chat",
                "chat:connect",
                "chat:timeout",
                "chat:whisper",
            };
        }
        #endregion

    }
}
