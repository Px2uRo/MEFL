using CoreLaunching.MicrosoftAuth;
using MEFL.CLAddIn.AccountsManagement;
using MEFL.Contract;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MEFL.CLAddIn.Pages
{
    /// <summary>
    /// AddNewMSAccount.xaml 的交互逻辑
    /// </summary>
    public partial class AddNewMSAccount : UserControl,IAddAccountPage
    {
        private static bool Over17763 = Environment.OSVersion.Version >= new Version(10,0,17763,0);
        public AddNewMSAccount()
        {
            InitializeComponent();
        }

        public event IAddAccountPage.AccountAdd OnAccountAdd;
        public event IAddAccountPage.Canceled OnCanceled;

        internal void ResetWebb()
        {
            if (Over17763)
            {
                wbb_border.Visibility = Visibility.Visible;
                wbb.Source = new(CoreLaunching.MicrosoftAuth.MSAuthAccount.MinecraftAppUrl);
            }
            else
            {
                border17763.Visibility = Visibility.Visible;
            }
        }

        private async void Web_Completed(object sender, NavigationEventArgs e)
        {
            var lnk = (sender as WebBrowser).Source.ToString();
            if (lnk.StartsWith("https://login.live.com/oauth20_desktop.srf?code="))
            {
                lnk = lnk.Replace("https://login.live.com/oauth20_desktop.srf?code=",string.Empty);
                var cl = CoreLaunching.MicrosoftAuth.MSAuthAccount.GetInfoWithRefreshTokenFromCode(lnk);
                if (!cl.HasError)
                {
                    var acc = MEFLMicrosoftAccount.LoadFromCL(cl);
                    //Model.MicrosoftAccounts.AddOne(acc);
                    OnAccountAdd.Invoke(this,acc);
                }
                else
                {
                    OnCanceled.Invoke(this);
                }
            }
        }
    }
}
