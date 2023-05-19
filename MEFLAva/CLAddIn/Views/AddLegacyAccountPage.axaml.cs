using Avalonia;
using Avalonia.Controls;
using MEFL.Callers;
using MEFL.CLAddIn;
using MEFL.CLAddIn.AccountsManagement;
using MEFL.Contract;

namespace CLAddIn.Views
{
    public partial class AddLegacyAccountPage : UserControl,IAddAccountContent
    {
        public void Refresh()
        {
            AccountNameTB.Text = string.Empty;
            GuidTB.Text=Guid.NewGuid().ToString();
        }
        public AddLegacyAccountPage()
        {
            InitializeComponent();
            CancelBtn.Click += CancelBtn_Click;
            AddBtn.Click += EnableBtn_Click;
        }

        private void EnableBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if(AccountNameTB.Text == string.Empty)
            {
                return;
            }
            if(Guid.TryParse(GuidTB.Text,out var guid)) 
            {
                var account = new MEFLLegacyAccount(AccountNameTB.Text,guid);
                var lnq = Model.LegacyAccounts.Where((x => x.Uuid.ToString() == guid.ToString())).ToArray();
                foreach (var item in lnq)
                {
                    LegacyList.RemoveOne(item);
                    AccountCaller.Remove(item);
                }
                LegacyList.AddOne(account);
                OnAccountAdd.Invoke(this,account);
            }
        }

        private void CancelBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Quited?.Invoke(this, e);
        }

        public void WindowSizeChanged(Size size)
        {

        }

        public event IAddAccountContent.AccountAdd OnAccountAdd;
        public event EventHandler<EventArgs> Quited;
    }
}
