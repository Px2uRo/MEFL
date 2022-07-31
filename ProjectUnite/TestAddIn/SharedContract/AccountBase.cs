using MEFL.Arguments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MEFL.Contract
{
    public abstract class AccountBase:IDisposable
    {
        public abstract object ProfileAvator { get; set; }
        public abstract string UserName { get; set; }
        public abstract string Uuid { get; set; }
        public abstract string AccessToken { get; set; }
        public abstract string ClientID { get; set; }
        public abstract string Xuid { get; set; }
        public abstract string UserType { get; set; }
        public abstract string UserProperties { get; set; }
        public abstract object WelcomeWords { get; set; }
        public abstract string EmailAddress { get; set; }
        public abstract FrameworkElement ManagePage { get; }
        public abstract void Dispose();
        public abstract void LaunchGameAction(SettingArgs args);
    }
}
