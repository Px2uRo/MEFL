using Avalonia;
using Avalonia.Controls;
using CoreLaunching.Accounts;
using MEFL.CLAddIn;
using MEFL.CLAddIn.AccountsManagement;
using MEFL.Contract;
using System.Diagnostics;

namespace CLAddIn.Views
{
    public partial class ManageAUPAPage : UserControl,IManageAccountPage
    {
        public ManageAUPAPage()
        {
            InitializeComponent();
            BackBtn.Click += BackBtn_Click;
            Selec.Click += Selec_Click;
            PWBtn.Click += PWBtn_Click;
            WebSiteBtn.Click += WebSiteBtn_Click;
            Delte.Click += Delte_Click;
        }

        private void Delte_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dc = DataContext as MEFLUnitedPassportAccount;
            UPList.RemoveOne(dc);
            OnAccountDeleted?.Invoke(this,dc);
        }

        private void PWBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Process.Start("explorer.exe", "https://login.mc-user.com:233/account/changepw");
        }

        private void Selec_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            OnSelected?.Invoke(this,DataContext as AccountBase);
        }

        private void BackBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Quited?.Invoke(this, e);
        }

        private void WebSiteBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dc = DataContext as MEFLUnitedPassportAccount;
            Process.Start("explorer.exe", $"https://login.mc-user.com:233/{dc.ServerID}/skin");
        }

        public event IManageAccountPage.AccountDeleted OnAccountDeleted;
        public event IManageAccountPage.Selected OnSelected;
        public event EventHandler<EventArgs> Quited;

        public void WindowSizeChanged(Size size)
        {

        }
    }
}
