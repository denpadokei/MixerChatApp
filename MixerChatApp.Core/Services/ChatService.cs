using Microsoft.Extensions.Configuration;
using MixerChatApp.Core.Interfaces;
using MixerLib;
using MixerLib.Events;
using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Unity;

namespace MixerChatApp.Core.Services
{
    public class ChatService : BindableBase, IChatService
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        /// <summary>説明 を取得、設定</summary>
        private IAuthorization auth_;
        /// <summary>説明 を取得、設定</summary>
        public IAuthorization Auth
        {
            get => this.auth_;

            set => this.SetProperty(ref this.auth_, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private IMixerClient client_;
        /// <summary>説明 を取得、設定</summary>
        public IMixerClient Client
        {
            get => this.client_;

            set => this.SetProperty(ref this.client_, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private string channelName_;
        /// <summary>説明 を取得、設定</summary>
        public string ChannelName
        {
            get => this.channelName_;

            set => this.SetProperty(ref this.channelName_, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private string token_;
        /// <summary>説明 を取得、設定</summary>
        public string Token
        {
            get => this.token_;

            set => this.SetProperty(ref this.token_, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private DateTimeOffset expiresAt_;
        /// <summary>説明 を取得、設定</summary>
        public DateTimeOffset ExpiresAt
        {
            get => this.expiresAt_;

            set => this.SetProperty(ref this.expiresAt_, value);
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
        [InjectionMethod]
        public void Init()
        {
            if (DateTimeOffset.TryParse(this._configurationRoot[this.EXPIRES_AT_NAME], out var datetime) && (DateTimeOffset.UtcNow - datetime) < new TimeSpan(6, 0, 0)) {
                this.Token = this._configurationRoot[this.ACSESS_TOKEN_NAME];
                this.ExpiresAt = datetime;
            }
        }

        public async Task StartClient()
        {
            try {
                if (!string.IsNullOrEmpty(this.Token)) {
                    
                    this.Auth = new Auth.ImplicitGrant(this.Token);
                }
                else {
                    this.Auth = null;
                }
                 
                if (this.Client != null) {
                    this.Client.ChatMessage -= this.GetChat;
                    this.Client.Dispose();
                    this.Client = null;
                }
                Debug.WriteLine($"{this.Auth.AuthMethod}");
                this.Client = await MixerClient.StartAsync(this.ChannelName, this.Auth);
                if (this.Client != null) {
                    this.Client.ChatMessage += this.GetChat;
                }
            }
            catch (Exception e) {
                Debug.WriteLine($"{e.Message}");
            }
        }

        public async Task<bool> SendMessage(string message)
        {
            try {
                var result = await this.Client.SendMessageAsync(message);
                Debug.WriteLine($"{result}");
                return result;
            }
            catch (Exception e) {
                Debug.WriteLine($"{e.Message}");
                return false;
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        private async void GetChat(object sender, ChatMessageEventArgs e)
        {
#if DEBUG
            var tmp = await this.Client.RestClient.GetChatAuthKeyAndEndpointsAsync();
            Debug.WriteLine($"AuthKey : {tmp.Authkey}");
            foreach (var item in tmp.Endpoints) {
                Debug.WriteLine($"EndPoint : {item}");
            }
#endif
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // JSON

        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        //private readonly string SETTING_NAME = "MIXER_CHAT_APP_";
        private readonly string ACSESS_TOKEN_NAME = "AcsessToken";
        private readonly string EXPIRES_AT_NAME = "ExporesAt";
        [Dependency]
        public IConfigurationRoot _configurationRoot;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        public ChatService()
        {
            
        }
        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                    this.Client?.Dispose();
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~ChatService()
        // {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion
        #endregion
    }
}
