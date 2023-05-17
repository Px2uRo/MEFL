using Avalonia;
using Avalonia.Controls;
using MEFL.CLAddIn.AccountsManagement;
using MEFL.CLAddIn;
using MEFL.Contract;
using System.Diagnostics;

namespace CLAddIn.Views
{
    public partial class AddMSAccountPage : UserControl,IAddAccountContent
    {
        public AddMSAccountPage()
        {
            InitializeComponent();
            Web.Click += Web_Click;
            TryLogIn.Click += TryLogIn_Click;
            CancelBtn.Click += CancelBtn_Click;
        }

        private void CancelBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Quited?.Invoke(this,e);
        }

        private void TryLogIn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var lnk = WebSite.Text;
            if (lnk.StartsWith("https://login.live.com/oauth20_desktop.srf?code="))
            {
                lnk = lnk.Replace("https://login.live.com/oauth20_desktop.srf?code=", string.Empty);
                var cl = CoreLaunching.MicrosoftAuth.MSAuthAccount.GetInfoWithRefreshTokenFromCode(lnk);
                if (!cl.HasError)
                {
                    var acc = MEFLMicrosoftAccount.LoadFromCL(cl);
                    Model.MicrosoftAccounts.AddOne(acc);
                    OnAccountAdd.Invoke(this, acc);
                }
            }
        }

        private void Web_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
#if true
            Process.Start("explorer.exe", $"\"{CoreLaunching.MicrosoftAuth.MSAuthAccount.MinecraftAppUrl}\"");
#endif
        }

        public event IAddAccountContent.AccountAdd OnAccountAdd;
        public event EventHandler<EventArgs> Quited;

        public void WindowSizeChanged(Size size)
        {

        }

        internal void Refresh()
        {
            WebSite.Text = string.Empty;
        }
    }
}
