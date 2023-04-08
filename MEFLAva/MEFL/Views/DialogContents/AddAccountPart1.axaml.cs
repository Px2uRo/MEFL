using Avalonia.Controls;
using MEFL.APIData;
using MEFL.AvaControls;
using MEFL.Contract;
using MEFL.PageModelViews;
using System;
using System.Linq;

namespace MEFL.Views.DialogContents
{
    public partial class AddAccountPart1 : UserControl,IDialogContent
    {
        public static AddAccountPart1 UI = new AddAccountPart1();
        public AddAccountPart1()
        {
            InitializeComponent();
            BackBtn.Click += BackBtn_Click;
        }

        private void BackBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ContentDialog.Show(ChooseAccountDialog.UI);
        }

        public void Reload()
        {
            HasErrors.IsVisible= false;
            for (int i = 0; i < Btns.Children.Count; i++)
            {
                GC.SuppressFinalize(Btns.Children[i]);
                (Btns.Children[i] as Button).Content = null;
                Btns.Children.RemoveAt(i);
                i--;
            }
            try
            {
                var loaded = APIModel.Hostings.Where((h) => h.Loaded&&h.IsOpen).ToArray();
                var abled = loaded.Where((h) => h.Permissions.UseAccountAPI == true).ToArray();
                foreach (var item in abled)
                {
                    foreach (var btn in item.Account.GetSingUpPage(APIModel.SettingArgs))
                    {
                        var nbtn = new Button() { Width=288,Height=80};
                        nbtn.Content = btn.Content;
                        nbtn.Click+=(sender,e) => 
                        {
                            btn.Dialog.OnAccountAdd -= Dialog_OnAccountAdd;
                            btn.Dialog.OnAccountAdd += Dialog_OnAccountAdd;
                            ContentDialog.Show(btn.Dialog);
                            btn.Dialog.Quited -= Dialog_Back;
                            btn.Dialog.Quited += Dialog_Back;
                        };
                        Btns.Children.Add(nbtn);
                    }
                }
            }
            catch (Exception ex)
            {
                HasErrors.IsVisible = true;
                HasErrors.Text = ex.Message;
            }
        }

        private void Dialog_Back(object? sender, EventArgs e)
        {
            ChooseAccountDialog.UI.ReLoad();
            ContentDialog.Show(ChooseAccountDialog.UI);
        }

        private void Dialog_OnAccountAdd(object sender, AccountBase account)
        {
            APIModel.AccountConfigs.Add(account);
            APIModel.SelectedAccount= account;
            (App.Current.Resources["RMPMV"] as RealMainPageModelView).Invoke("AccountName");
            ChooseAccountDialog.UI.ReLoad();
            ContentDialog.Show(ChooseAccountDialog.UI);
        }

        public event EventHandler<EventArgs> Quited;
    }
}
