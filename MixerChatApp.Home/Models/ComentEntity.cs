using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MixerChatApp.Home.Models
{
    public class CommentEntity : BindableBase
    {
        /// <summary>説明 を取得、設定</summary>
        private string userName_;
        /// <summary>説明 を取得、設定</summary>
        public string UserName
        {
            get => this.userName_;

            set => this.SetProperty(ref this.userName_, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private string message_;
        /// <summary>説明 を取得、設定</summary>
        public string Message
        {
            get => this.message_;

            set => this.SetProperty(ref this.message_, value);
        }

        public CommentEntity(string username, string message)
        {
            this.UserName = username;
            this.Message = message;
        }
    }
}
