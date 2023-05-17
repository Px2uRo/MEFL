using Avalonia;
using Avalonia.Controls;
using MEFL.CLAddIn;
using MEFL.CLAddIn.AccountsManagement;
using MEFL.Contract;

namespace CLAddIn.Views
{
    public partial class ManageALegacyAccountPage : UserControl,IManageAccountPage
    {
        public ManageALegacyAccountPage()
        {
            InitializeComponent();
            DataContextChanged += ManageALegacyAccountPage_DataContextChanged;
            CancelBtn.Click += CancelBtn_Click;
            SelecteBtn.Click += SelecteBtn_Click;
            DeleteBtn.Click += DeleteBtn_Click;
        }

        private void ManageALegacyAccountPage_DataContextChanged(object? sender, EventArgs e)
        {
            var dc = this.DataContext as MEFLLegacyAccount;
            NameBox.Text = dc.UserName;
            GuidBox.Text = dc.Uuid.ToString();
        }

        private void DeleteBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var a = DataContext as MEFLLegacyAccount;
            Model.LegacyAccounts.RemoveOne(a);
            OnAccountDeleted?.Invoke(this,a);
        }

        private void SelecteBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var a = DataContext as MEFLLegacyAccount;
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                ErrorInfoBox.Text = "请不要让用户名留空";
                return;
            }
            if (Guid.TryParse(GuidBox.Text,out var guid))
            {
                a.UserName = NameBox.Text;
                a.Uuid = guid;
                Model.LegacyAccounts.WriteReg();
            }
            else
            {
                ErrorInfoBox.Text = "不合法 Uuid ！";
                return;
            }
            OnSelected?.Invoke(this, a);
        }

        private void CancelBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            this.Quited?.Invoke(this, e);
        }

        public event IManageAccountPage.AccountDeleted OnAccountDeleted;
        public event IManageAccountPage.Selected OnSelected;
        public event EventHandler<EventArgs> Quited;

        public void WindowSizeChanged(Size size)
        {

        }
    }
}
