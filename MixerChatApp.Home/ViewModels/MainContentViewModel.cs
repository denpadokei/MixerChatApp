using Microsoft.Win32;
using MixerChatApp.Core;
using MixerChatApp.Core.Interfaces;
using MixerChatApp.Home.Models;
using MixerLib.Events;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Windows;
using Unity;

namespace MixerChatApp.Home.ViewModels
{
    public class MainContentViewModel : BindableBase
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        /// <summary>説明 を取得、設定</summary>
        private ObservableSynchronizedCollection<CommentEntity> queue_;
        /// <summary>説明 を取得、設定</summary>
        public ObservableSynchronizedCollection<CommentEntity> Queue
        {
            get => this.queue_;

            set => this.SetProperty(ref this.queue_, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private SortedObservableCollection<CommentEntity, DateTime> sortedQueue_;
        /// <summary>説明 を取得、設定</summary>
        public SortedObservableCollection<CommentEntity, DateTime> SortedQueue
        {
            get => this.sortedQueue_;

            set => this.SetProperty(ref this.sortedQueue_, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private SynchronizationContextCollection<CommentEntity> collection_;
        /// <summary>説明 を取得、設定</summary>
        public SynchronizationContextCollection<CommentEntity> Collection
        {
            get => this.collection_;

            set => this.SetProperty(ref this.collection_, value);
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
        private bool isSending_;
        /// <summary>説明 を取得、設定</summary>
        public bool IsSending
        {
            get => this.isSending_;

            set => this.SetProperty(ref this.isSending_, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private string message_;
        /// <summary>説明 を取得、設定</summary>
        public string Message
        {
            get => this.message_;

            set => this.SetProperty(ref this.message_, value);
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // コマンド
        private DelegateCommand startCommand_;
        public DelegateCommand StartCommand =>
            startCommand_ ?? (startCommand_ = new DelegateCommand(ExecuteStartCommand));

        async void ExecuteStartCommand()
        {
            try {
                await this._chatService.StartClient();
                this._chatService.Client.ChatMessage -= this.MixerChat;
                this._chatService.Client.ChatMessage += this.MixerChat;
            }
            catch (Exception e) {
                Debug.WriteLine($"{e.Message}");
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // コマンド用メソッド
        private DelegateCommand showSettingCommand_;
        public DelegateCommand ShowSettingCommand =>
            showSettingCommand_ ?? (showSettingCommand_ = new DelegateCommand(ExecuteShowSettingCommand));

        void ExecuteShowSettingCommand()
        {
            var param = new DialogParameters() { { "IsSending", this.IsSending } };
            this._dialogService?.Show(RegionName.SettingRegionName, param, result =>
            {
                if (result.Result == ButtonResult.OK) {
                    this.IsSending = result.Parameters.GetValue<bool>("IsSending");
                }
            });
        }

        private DelegateCommand sendCommand_;
        public DelegateCommand SendCommand =>
            sendCommand_ ?? (sendCommand_ = new DelegateCommand(ExecuteSendCommand));

        async void ExecuteSendCommand()
        {
            var result = await this._chatService?.SendMessage(this.Message);
            if (result) {
                this.Queue.Add(new CommentEntity(this._chatService.Client.UserName, this.Message));
                this.Message = "";
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // オーバーライドメソッド
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(this.ChannelName)) {
                this._chatService.ChannelName = this.ChannelName;
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // パブリックメソッド
        [InjectionMethod]
        public void Init()
        {
            WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.AddHandler(
                this._oAuthManager, nameof(INotifyPropertyChanged.PropertyChanged), this.OnOauthPropertyChanged);
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        private async void MixerChat(object sender, ChatMessageEventArgs e)
        {
            this.Queue.Add(new CommentEntity(e.UserName, e.Message));
            if (this.IsSending) {
                await this._bouyomiService.SendMessage(e.Message);
            }
        }

        private void OnOauthPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IOAuthManagerable manager && e.PropertyName == nameof(manager.Tokens)) {
                this._chatService.Token = manager.Tokens.AccessToken;
                this._chatService.ExpiresAt = manager.Tokens.ExpiresAt;
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        [Dependency]
        public IChatService _chatService;
        [Dependency]
        public IBouyomiService _bouyomiService;
        [Dependency]
        public IDialogService _dialogService;
        [Dependency]
        public IOAuthManagerable _oAuthManager;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        public MainContentViewModel()
        {
            this.Queue = new ObservableSynchronizedCollection<CommentEntity>();
            this.SortedQueue = this.Queue.ToSyncedSortedObservableCollection(key => key.CommentDate, null, true);
            this.Collection = this.SortedQueue.ToSyncedSynchronizationContextCollection(SynchronizationContext.Current);
            this.IsSending = true;
        }
        #endregion
    }
}
