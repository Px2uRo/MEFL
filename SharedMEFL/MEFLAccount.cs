using MEFL.Arguments;
using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL
{
    public class MEFLLegacyAccount : MEFL.Contract.AccountBase
    {
        public string GetSetUserName { get; set; }
        public override string UserName => GetSetUserName;
        public string GetSetUUid { get; set; }

        public override string Uuid => GetSetUUid;

        public string GetSetAccessToken { get; set; }
        public override string AccessToken => GetSetAccessToken;
        public string GetSetClientID { get; set; }
        public override string ClientID => GetSetClientID;
        public string GetSetXuid { get; set; }

        public override string Xuid => GetSetXuid;
        public string GetSetUserType { get; set; }

        public override string UserType => GetSetUserType;

        public string GetSetUserProperties { get; set; }

        public override string UserProperties => GetSetUserProperties;

        public override object WelcomeWords 
        { 
            get
            {
                return string.Format(App.Current.Resources["I18N_String_MEFLAccounts_WelcomeWords"].ToString(),UserName);
            } 
        }

        public override void ChangeSettingAction(SettingArgs args)
        {
            
        }

        public override void LaunchGameAction(AccountBase account)
        {
            
        }
    }
}
