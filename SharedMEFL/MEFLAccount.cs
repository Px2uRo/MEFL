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
using MEFL.SpecialPages;

namespace MEFL
{
    public static class ManagePageForMEFLLegacyAccount 
    {
        public static ManageALegacyAccountPage page = new ManageALegacyAccountPage();
    }
    public class MEFLLegacyAccount : MEFL.Contract.AccountBase
    {
        public override void Dispose()
        {
            
        }
        private Grid _Avator = new Grid();
        private TextBlock _AvatorText = new TextBlock() { VerticalAlignment = System.Windows.VerticalAlignment.Center, HorizontalAlignment = System.Windows.HorizontalAlignment.Center,FontSize=36,FontWeight=FontWeight.FromOpenTypeWeight(5),Foreground=new SolidColorBrush(Colors.White) };
        [JsonIgnore]
        public override object WelcomeWords {
            get
            {
                return string.Format(App.Current.Resources["I18N_String_MEFLAccounts_WelcomeWords"].ToString(), UserName);
            } set=>throw new NotImplementedException();
        }
        [JsonIgnore]
        public override object ProfileAvator 
        { 
            get 
            {
                if (APIData.APIModel.SelectedAccount == this)
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
            } set => throw new NotImplementedException(); }
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
        private string _uuid;
        public override string Uuid { get => _uuid; set {
                try
                {
                    Guid.Parse(value);
                    _uuid = value;
                }
                catch (Exception ex)
                {
                    
                }
            } 
        }
        [JsonIgnore]
        public override string AccessToken { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        [JsonIgnore] 
        public override string ClientID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [JsonIgnore]
        public override string Xuid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [JsonIgnore]
        public override string UserType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [JsonIgnore]
        public override string UserProperties { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [JsonIgnore]
        public override string EmailAddress { get => "离线"; set => throw new NotImplementedException(); }
        [JsonIgnore]
        public override FrameworkElement ManagePage => ManagePageForMEFLLegacyAccount.page;

        public override void LaunchGameAction(Arguments.SettingArgs args)
        {
            throw new NotImplementedException();
        }
        public MEFLLegacyAccount(string Name,string Uuid)
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
