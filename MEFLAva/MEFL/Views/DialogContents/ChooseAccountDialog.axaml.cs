using Avalonia.Controls;
using MEFL.APIData;
using MEFL.AvaControls;
using MEFL.Contract;
using MEFL.PageModelViews;
using System;

namespace MEFL.Views.DialogContents
{
    public partial class ChooseAccountDialog : UserControl,IDialogContent
    {
        public static ChooseAccountDialog UI = new ChooseAccountDialog();
        internal static AccountViewModel VM = new AccountViewModel();
        public ChooseAccountDialog()
        {
            InitializeComponent();
            this.DataContext = VM;
            AddBtn.Click += AddBtn_Click;
            CancelBtn.Click += CancelBtn_Click;
        }

        private void CancelBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Quited?.Invoke(this, e);
        }

        private void AddBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            AddAccountPart1.UI.Reload();
            ContentDialog.Show(AddAccountPart1.UI);
        }

        internal void ReLoad()
        {
            foreach (var item in APIModel.AccountConfigs)
            {

            }
        }

        public event EventHandler<EventArgs> Quited;
    }

    internal class AccountViewModel : PageModelViewBase
    {
        public bool DidNotChoose => APIModel.SelectedAccount == null;
    }
}
