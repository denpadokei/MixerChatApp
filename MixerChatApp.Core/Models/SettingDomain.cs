using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using StatefulModel;
using MixerChatApp.Core.Interfaces;
using Unity;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Mixer.ShortcodeOAuth;
using System.Windows.Media.TextFormatting;
using System.Windows;
using System.Diagnostics;

namespace MixerChatApp.Core.Models
{
    /// <summary>
    /// 設定保持したりファイルに保存したりするとこ
    /// </summary>
    public class SettingDomain : BindableBase, ISettingDomain
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        /// <summary>説明 を取得、設定</summary>
        private bool isSending_;
        /// <summary>説明 を取得、設定</summary>
        [JsonProperty]
        public bool IsSending
        {
            get => this.isSending_;

            set => this.SetProperty(ref this.isSending_, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private bool isSaveUserInformation_;
        /// <summary>説明 を取得、設定</summary>
        [JsonProperty]
        public bool IsSaveUserInformation
        {
            get => this.isSaveUserInformation_;

            set => this.SetProperty(ref this.isSaveUserInformation_, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private int bouyomiPort_;
        /// <summary>説明 を取得、設定</summary>
        [JsonProperty]
        public int BouyomiPort
        {
            get => this.bouyomiPort_;

            set => this.SetProperty(ref this.bouyomiPort_, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private string bouyomiHost_;
        /// <summary>説明 を取得、設定</summary>
        [JsonProperty]
        public string BouyomiHost
        {
            get => this.bouyomiHost_;

            set => this.SetProperty(ref this.bouyomiHost_, value);
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
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (this._isInit) {
                return;
            }
            
            if (args.PropertyName == nameof(this.IsSaveUserInformation)) {
                this.SaveToken();
            }
            else {
                if (args.PropertyName == nameof(this.BouyomiPort)) {
                    this._bouyomiService.Port = this.BouyomiPort;
                }
                else if (args.PropertyName == nameof(this.BouyomiHost)) {
                    this._bouyomiService.Host = this.BouyomiHost;
                }
                this.Save();
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // パブリックメソッド
        /// <summary>
        /// 色々初期化しすぎてここでこけるとすべてが崩壊する。
        /// </summary>
        [InjectionMethod]
        public async void Init()
        {
            this._isInit = true;
            this.IsSending = bool.Parse(this._configuration["IsSending"]);
            this.BouyomiPort = int.Parse(this._configuration["BouyomiPort"]);
            this.BouyomiHost = this._configuration["BouyomiHost"];
            this.Save();

            this.IsSaveUserInformation = bool.Parse(this._configuration["IsSaveUserInformation"]);
            if (this.IsSaveUserInformation) {
                try {
                    this._oAuthManager.Tokens = this._fileService.ReadTokens();
                    await this._oAuthManager.RefreshToken();
                }
                catch (Exception e) {
                    Debug.WriteLine($"{e}");
                }
                this._oAuthManager.IsSaveUserInformation = this.IsSaveUserInformation;
            }
            this.SaveToken();

            WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.AddHandler(
                this._oAuthManager, nameof(INotifyPropertyChanged.PropertyChanged), this.OnOauthPropertyChanged);
            this._isInit = false;
        }
        public void SaveToken() => this._fileService?.SaveToken(this._oAuthManager.Tokens, !this.IsSaveUserInformation);
        public void Save() => this._fileService?.Save(this);
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        public void OnOauthPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IOAuthManagerable.Tokens)) {
                this.SaveToken();
                this._chatService.Token = this._oAuthManager.Tokens.AccessToken;
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        [Dependency]
        [JsonIgnore]
        public IFileService _fileService;
        [Dependency]
        [JsonIgnore]
        public IConfigurationRoot _configuration;
        [Dependency]
        [JsonIgnore]
        public IOAuthManagerable _oAuthManager;
        [Dependency]
        [JsonIgnore]
        public IChatService _chatService;

        [Dependency]
        [JsonIgnore]
        public IBouyomiService _bouyomiService;

        private bool _isInit;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        public SettingDomain()
        {
            this.IsSending = true;
        }
        #endregion
    }
}
