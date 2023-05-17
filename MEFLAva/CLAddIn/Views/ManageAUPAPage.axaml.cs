using Avalonia;
using Avalonia.Controls;
using MEFL.Contract;

namespace CLAddIn.Views
{
    public partial class ManageAUPAPage : UserControl,IManageAccountPage
    {
        public ManageAUPAPage()
        {
            InitializeComponent();
        }

        public event IManageAccountPage.AccountDeleted OnAccountDeleted;
        public event IManageAccountPage.Selected OnSelected;
        public event EventHandler<EventArgs> Quited;

        public void WindowSizeChanged(Size size)
        {

        }
    }
}
