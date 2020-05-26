using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Mixer.ShortcodeOAuth;
using MixerChatApp.Core.APIs;
using MixerChatApp.Core.Interfaces;
using MixerChatApp.Core.Models;
using MixerLib;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Unity;

namespace MixerChatApp.Core.Services
{
    public class OAuthManager : BindableBase, IOAuthManagerable
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        /// <summary>説明 を取得、設定</summary>
        private OAuthClient client_;
        /// <summary>説明 を取得、設定</summary>
        public OAuthClient Client
        {
            get => this.client_;

            set => this.SetProperty(ref this.client_, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private string cord_;
        /// <summary>説明 を取得、設定</summary>
        public string Code
        {
            get => this.cord_;

            set => this.SetProperty(ref this.cord_, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private string userName_;
        /// <summary>説明 を取得、設定</summary>
        public string UserName
        {
            get => this.userName_;

            set => this.SetProperty(ref this.userName_, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private DateTimeOffset connectDate_;
        /// <summary>説明 を取得、設定</summary>
        public DateTimeOffset ConnectDate
        {
            get => this.connectDate_;

            set => this.SetProperty(ref this.connectDate_, value);
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

        /// <summary>説明 を取得、設定</summary>
        private bool isSaveUserInformation_;
        /// <summary>説明 を取得、設定</summary>
        public bool IsSaveUserInformation
        {
            get => this.isSaveUserInformation_;

            set => this.SetProperty(ref this.isSaveUserInformation_, value);
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
        protected override async void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(IsSaveUserInformation)) {
                if (this.IsSaveUserInformation) {
                    await this.RefreshToken();
                    this._timer.Start();
                }
                else {
                    this._timer.Stop();
                }
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // パブリックメソッド
        public async Task RunAsync()
        {
            try {
                // Create your OAuth client. Specify your client ID, and which permissions you want.
                // Use the helper GrantAsync to get codes. Alternately, you can run
                // the granting/polling loop manually using client.GetSingleCodeAsync.
                var task = this.Client.GrantAsync(code =>
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

                var text = JsonConvert.SerializeObject(this._entity, Formatting.Indented);
                using (var sw = new StreamWriter(@".\appsettings.json")) {
                    sw.Write(text);
                    sw.Close();
                }
                this.ConnectDate = this.Tokens.ExpiresAt;
                this.UserName = await this._aPI.GetUserName(this.Tokens.AccessToken);
                
            }
            catch (OAuthException oe) {
                Debug.WriteLine($"{oe.Message}");
            }
            catch (Exception e) {
                Debug.WriteLine($"{e.Message}");
            }
        }

        public async Task RefreshToken()
        {
            try {
                var token = await this.Client.RefreshAsync(this.Tokens).ConfigureAwait(false);
                this.Tokens = token;
                this.ConnectDate = this.Tokens.ExpiresAt;
                this.UserName = await this._aPI.GetUserName(this.Tokens.AccessToken);
            }
            catch (Exception e) {
                Debug.WriteLine($"{e.Message}");
                throw;
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        private async void TimerEvent(object sender, ElapsedEventArgs e)
        {
            await this.RefreshToken();
        }

        
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        [Dependency]
        public MixerAPI _aPI;
        [Dependency]
        public JsonSettingEntity _entity;

        private readonly System.Timers.Timer _timer;

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
            var timespan = new TimeSpan(5, 55, 0);
            this._timer = new System.Timers.Timer(timespan.TotalMilliseconds);
            this._timer.Elapsed += this.TimerEvent;

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

#if DEBUG
            var bulder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Develop.json");
            var configuration = bulder.Build();

            var clientid = configuration["ClientId"];
#else
                var clientid = this.CLIENT_ID;
#endif
            this.Client = new OAuthClient(
                new OAuthOptions
                {
                    ClientId = clientid,
                    Scopes = this.Scorp.ToArray(),
                });
        }
        #endregion

    }
}
