using Avalonia;
using Avalonia.Controls;
using MEFL.CLAddIn;
using MEFL.CLAddIn.AccountsManagement;
using MEFL.Contract;
using System.Diagnostics;

namespace CLAddIn.Views
{
    public partial class ManageMSAccount : UserControl,IManageAccountPage
    {
        public ManageMSAccount()
        {
            InitializeComponent();
            WebSite.Click += WebSite_Click;
            QuitBtn.Click += QuitBtn_Click;
            Selec.Click += Selec_Click;
            Delte.Click += Delte_Click;
        }

        private void Delte_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var ms = DataContext as MEFLMicrosoftAccount;
            Model.MicrosoftAccounts.RemoveOne(ms);
            OnAccountDeleted?.Invoke(this,ms);
        }

        private void Selec_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            OnSelected?.Invoke(this,DataContext as AccountBase);
        }

        private void QuitBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Quited?.Invoke(this,EventArgs.Empty);
        }

        private void WebSite_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Process.Start("explorer.exe", "https://www.minecraft.net/zh-hans/msaprofile/mygames");
        }

        public event IManageAccountPage.AccountDeleted OnAccountDeleted;
        public event IManageAccountPage.Selected OnSelected;
        public event EventHandler<EventArgs> Quited;

        public void WindowSizeChanged(Size size)
        {

        }
    }
}
