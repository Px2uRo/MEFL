using Avalonia.Controls;
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
                LegacyList.AddOne(account);
                OnAccountAdd.Invoke(this,account);
            }
        }

        private void CancelBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Quited?.Invoke(this, e);
        }

        public event IAddAccountContent.AccountAdd OnAccountAdd;
        public event EventHandler<EventArgs> Quited;
    }
}