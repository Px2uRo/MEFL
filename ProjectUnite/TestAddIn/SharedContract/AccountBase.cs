using MEFL.Arguments;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.Contract
{
    public abstract class AccountBase
    {
        public abstract string UserName { get; }
        public abstract string Uuid { get; }
        public abstract string AccessToken { get; }
        public abstract string ClientID { get; }
        public abstract string Xuid { get; }
        public abstract string UserType { get; }
        public abstract string UserProperties { get; }
        public abstract object WelcomeWords { get; }
        public abstract void LaunchGameAction(AccountBase account);
        public abstract void ChangeSettingAction(SettingArgs args);
    }
}
