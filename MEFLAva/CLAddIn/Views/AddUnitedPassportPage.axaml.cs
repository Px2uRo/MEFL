using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using MEFL.Callers;
using MEFL.CLAddIn;
using MEFL.CLAddIn.AccountsManagement;
using MEFL.Contract;

namespace CLAddIn.Views
{
    public partial class AddUnitedPassportPage : UserControl, IAddAccountContent
    {
        public AddUnitedPassportPage()
        {
            InitializeComponent();
            CancelBtn.Click += CancelBtn_Click;
            AddBtn.Click += AddBtn_Click;
        }

        private void AddBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var cl = new CoreLaunching.Accounts.UnitedPassportAccount() { EmailAddress = AccountNameTB.Text,
                Password=PWTB.Text,ServerID=ServerID.Text,
                Nide8AuthPath=System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"CoreLaunching\\Nide8Auth.jar")};
            if (cl.GetToken())
            {
                var ac = new MEFLUnitedPassportAccount(cl);
                var lnq = Model.UPList.Where((x => x.Uuid.ToString() == ac.Uuid.ToString())).ToArray();
                foreach (var item in lnq)
                {
                    UPList.RemoveOne(item);
                    AccountCaller.Remove(item);
                }
                Model.UPList.AddOne(ac);
                OnAccountAdd?.Invoke(this,ac);
            }
            else
            {

            }
        }

        private void CancelBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Quited.Invoke(this, e);
        }

        public event IAddAccountContent.AccountAdd OnAccountAdd;
        public event EventHandler<EventArgs> Quited;

        public void WindowSizeChanged(Size size)
        {

        }

        internal void Refresh()
        {

        }
    }
}
