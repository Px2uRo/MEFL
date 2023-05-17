using Avalonia;
using Avalonia.Controls;
using MEFL.Contract;

namespace CLAddIn.Views
{
    public partial class AddUnitedPassportPage : UserControl, IAddAccountContent
    {
        public AddUnitedPassportPage()
        {
            InitializeComponent();
            CancelBtn.Click += CancelBtn_Click;
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
