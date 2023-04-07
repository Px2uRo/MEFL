using Avalonia.Controls;
using MEFL.Contract;

namespace CLAddIn.Views
{
    public partial class AddLegacyAccountPage : UserControl,IAddAccountContent
    {
        public AddLegacyAccountPage()
        {
            InitializeComponent();
        }

        public event IAddAccountContent.Canceled OnCanceled;
        public event IAddAccountContent.AccountAdd OnAccountAdd;
        public event EventHandler<EventArgs> Quited;
    }
}
