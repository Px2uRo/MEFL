using Avalonia.Controls;
using MEFL.Contract;

namespace CLAddIn.Views
{
    public partial class ManageALegacyAccountPage : UserControl,IManageAccountPage
    {
        public ManageALegacyAccountPage()
        {
            InitializeComponent();
        }

        public event IManageAccountPage.Canceled OnCanceled;
        public event IManageAccountPage.AccountDeleted OnAccountDeleted;
        public event IManageAccountPage.Selected OnSelected;
    }
}
