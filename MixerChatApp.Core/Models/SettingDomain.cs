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
        public bool IsSending
        {
            get => this.isSending_;

            set => this.SetProperty(ref this.isSending_, value);
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
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (this._isInit) {
                return;
            }

            if (this._entity != null) {
                if (args.PropertyName == nameof(this.IsSending)) {
                    this._entity.IsSending = this.IsSending;
                }
                else if (args.PropertyName == nameof(this.IsSaveUserInformation)) {
                    this._entity.IsSaveUserInformation = this.IsSaveUserInformation;
                    this._oAuthManager.IsSaveUserInformation = this.IsSaveUserInformation;
                    if (!this.IsSaveUserInformation) {
                        this.SaveToken();
                    }
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
            this.IsSaveUserInformation = bool.Parse(this._configuration["IsSaveUserInformation"]);
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (this.IsSaveUserInformation && File.Exists(Path.Combine(path, TEMPFILENAME))) {
                using (var sr = new StreamReader(Path.Combine(path, TEMPFILENAME))) {
                    this._oAuthManager.Tokens = JsonConvert.DeserializeObject<OAuthTokens>(sr.ReadToEnd());
                    sr.Close();
                }
                try {
                    await this._oAuthManager.RefreshToken();
                }
                catch (Exception e) {
                    Debug.WriteLine($"{e}");
                }
                this._oAuthManager.IsSaveUserInformation = this.IsSaveUserInformation;
            }
            WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.AddHandler(
                this._oAuthManager, nameof(INotifyPropertyChanged.PropertyChanged), this.OnOauthPropertyChanged);
            this._isInit = false;
        }
        public void SaveToken()
        {
            try {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (!Directory.Exists(Path.Combine(path, "mixerchat"))) {
                    Directory.CreateDirectory(Path.Combine(path, "mixerchat"));
                }

                if (this.IsSaveUserInformation && this._oAuthManager.Tokens != null) {
                    using (var sw = new StreamWriter(Path.Combine(path, TEMPFILENAME))) {
                        sw.Write(JsonConvert.SerializeObject(this._oAuthManager.Tokens, Formatting.Indented));
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
        public void Save()
        {
            var jsontext = JsonConvert.SerializeObject(this._entity, Formatting.Indented);
            using (var sw = new StreamWriter(@".\appsettings.json")) {
                sw.Write(jsontext);
                sw.Close();
            }
        }
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
        public JsonSettingEntity _entity;
        [Dependency]
        public IConfigurationRoot _configuration;
        [Dependency]
        public IOAuthManagerable _oAuthManager;
        [Dependency]
        public IChatService _chatService;

        private static readonly string TEMPFILENAME = @"mixerchat\e05kj2W8bYEmQT0B";
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
