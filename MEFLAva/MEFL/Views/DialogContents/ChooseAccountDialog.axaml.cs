using Avalonia.Controls;
using MEFL.APIData;
using MEFL.AvaControls;
using MEFL.Contract;
using MEFL.InfoControls;
using MEFL.PageModelViews;
using System;
using System.Linq;

namespace MEFL.Views.DialogContents
{
    public partial class ChooseAccountDialog : UserControl,IDialogContent
    {
        public static ChooseAccountDialog UI = new ChooseAccountDialog();
        public ChooseAccountDialog()
        {
            InitializeComponent();
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
            if (CurrectAccountBD.Child != null)
            {
                var child = CurrectAccountBD.Child as AccountsControl;
                child.Icon.Child = null;
                CurrectAccountBD.Child = null;
                GC.SuppressFinalize(child);
            }
            if (APIModel.SelectedAccount==null)
            {
                HintTB.IsVisible= true;
            }
            else
            {
                HintTB.IsVisible = false;
                var cont = new AccountsControl();
                cont.Icon.Child = APIModel.SelectedAccount.ProfileAvator;
                cont.DataContext = APIModel.SelectedAccount;
                CurrectAccountBD.Child = cont;
            }
            for (int i = 0; i < Accounts.Children.Count; i++)
            {
                (Accounts.Children[i] as AccountsControl).Icon.Child= null;
                GC.SuppressFinalize(Accounts.Children[i]);
                Accounts.Children.Remove(Accounts.Children[i]);
                i--;
            }
            var lnq = APIModel.AccountConfigs.Where((x)=>APIModel.SelectedAccount!=x).ToArray();
            foreach (var item in lnq)
            {
                var cont = new AccountsControl();
                cont.Icon.Child = item.ProfileAvator;
                cont.DataContext= item;
                Accounts.Children.Add(cont);
            }
        }

        public event EventHandler<EventArgs> Quited;
    }
}
