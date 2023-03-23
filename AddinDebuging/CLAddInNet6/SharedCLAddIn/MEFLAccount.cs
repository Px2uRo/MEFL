﻿using MEFL.Arguments;
using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Newtonsoft.Json;
using MEFL.CLAddIn.Pages;
using System.Configuration;
using System.ComponentModel;
using MEFL.CLAddIn.Sercurity;
using CoreLaunching.MicrosoftAuth;

namespace MEFL.CLAddIn
{
    internal class MEFLMicrosoftAccount : AccountBase
    {
        [JsonIgnore]
        public override FrameworkElement ProfileAvator { get => new FrameworkElement();}
        public override string UserName { get; set; }
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

        [JsonConverter(typeof(TokenConverter))]
        public string RefreshToken { get; set; }
    }
    public class MEFLLegacyAccount : MEFL.Contract.AccountBase,INotifyPropertyChanged
    {
        public static ManageALegacyAccountPage page = new ManageALegacyAccountPage();
        [JsonIgnore]
        public override bool Selected { get => base.Selected;
            set {
                _Avator.Width = 200;
                _Avator.Height = 200;
                base.Selected = value; 
            } 
        }
        protected override void Dispose(bool disposing)
        {
            GC.SuppressFinalize(_Avator);
            GC.SuppressFinalize(_AvatorText);
            GC.SuppressFinalize(_username);
            GC.SuppressFinalize(_uuid);

            base.Dispose(disposing);
        }
        private Grid _Avator = new Grid();
        private TextBlock _AvatorText = new TextBlock() { VerticalAlignment = System.Windows.VerticalAlignment.Center, HorizontalAlignment = System.Windows.HorizontalAlignment.Center,FontSize=36,FontWeight=FontWeight.FromOpenTypeWeight(5),Foreground=new SolidColorBrush(Colors.White) };
        [JsonIgnore]
        public override object WelcomeWords {
            get
            {
                return string.Format($"欢迎回来 {UserName}");
            }
        }
        [JsonIgnore]
        public override FrameworkElement ProfileAvator 
        { 
            get 
            {
                if (Selected)
                {
                    _Avator.Width = 100;
                    _Avator.Height = 100;
                    _AvatorText.FontSize = 36;
                    return _Avator;
                }
                else
                {
                    _Avator.Width = 25;
                    _Avator.Height = 25;
                    _AvatorText.FontSize = 14;
                    return _Avator;
                }
            }
        }
        private string _username;
        public override string UserName { get => _username; set { _username = value;
                if (value.Length >= 2)
                {
                    _AvatorText.Text = value.Substring(0, 2);
                }
                else
                {
                    if (value.Length == 1)
                    {
                        _AvatorText.Text = value.Substring(0, 1);
                    }
                    else
                    {
                        _AvatorText.Text = String.Empty;
                    }
                }
            } 
        }
        private Guid _uuid;
        public override Guid Uuid { get => _uuid; set {
                _uuid=value;
            } 
        }
        [JsonIgnore]
        public override string AccessToken { get => "0123456789abcdef0123456789ABCDEF"; set => throw new NotImplementedException(); }

        [JsonIgnore] 
        public override string ClientID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [JsonIgnore]
        public override string Xuid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [JsonIgnore]
        public override string UserType { get => "Legacy"; set => throw new NotImplementedException(); }
        [JsonIgnore]
        public override string UserProperties { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [JsonIgnore]
        public override IManageAccountPage ManagePage
        {
            get
            {
                page.DataContext= this;
                return page;
            }
        }

        public override void LaunchGameAction(Arguments.SettingArgs args)
        {
            
        }
        public MEFLLegacyAccount(string Name,Guid Uuid)
        {
            UserName = Name;
            this.Uuid = Uuid;
            _Avator.Children.Add(new Ellipse()
            {
                Fill = new SolidColorBrush(Colors.DarkGray),
            });
            _Avator.Children.Add(_AvatorText);
        }
    }
}
