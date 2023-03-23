using MEFL.Arguments;
using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Configuration;
using System.ComponentModel;
using CoreLaunching.MicrosoftAuth;

namespace MEFL.CLAddIn
{
    public class MEFLMicrosoftAccount : AccountBase
    {
        [JsonIgnore]
        public override FrameworkElement ProfileAvator { get => new FrameworkElement();}
        public override string UserName { get; set; }
        [JsonIgnore]
        public override Guid Uuid { get; set; }
        [JsonIgnore]
        public override string AccessToken { get; set; }
        [JsonIgnore]
        public override string ClientID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [JsonIgnore]
        public override string Xuid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [JsonIgnore]
        public override string UserType { get ; set ; }
        [JsonIgnore]
        public override string UserProperties { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [JsonIgnore]
        public override object WelcomeWords { get => $"欢迎微软账户{UserName}"; }
        [JsonIgnore]
        public override IManageAccountPage ManagePage => throw new NotImplementedException();
        public override void LaunchGameAction(SettingArgs args)
        {
            var info = CoreLaunching.MicrosoftAuth.MSAuthAccount.GetInfoWithRefreshTokenFromRefreshToken(RefreshToken);
            AccessToken= info.AccessToken;
            RefreshToken= info.RefreshToken;
        }

        internal static MEFLMicrosoftAccount LoadFromCL(MSAPlayerInfoWithRefreshToken cl)
        {
            var res = new MEFLMicrosoftAccount();
            res.AccessToken = cl.AccessToken;
            res.RefreshToken = cl.RefreshToken;
            res.UserType = "msa";
            res.UserName = cl.Name;
            res.Uuid = new(cl.Id);
            return res;
        }
        public string RefreshToken { get; set; }
    }
}
