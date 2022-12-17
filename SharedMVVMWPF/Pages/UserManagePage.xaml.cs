using System.Windows;
using System.Windows.Controls;
using MEFL.Contract;
using MEFL.Controls;
using MEFL.PageModelViews;

namespace MEFL.Pages
{
    /// <summary>
    /// UserManagePage.xaml 的交互逻辑
    /// </summary>
    public partial class UserManagePage : MyPageBase
    {
        public UserManagePage()
        {
            InitializeComponent();
            this.DataContext = UserManageModel.ModelView;
        }

        private void ManageCurret(object sender, RoutedEventArgs e)
        {
            foreach (MyPageBase item in FindControl.FromTag("AddNewAccount", (App.Current.Resources["MainPage"] as Grid)))
            {
                (item as SpecialPages.AddAccountPage).ClearValue(ContentProperty);
                (App.Current.Resources["MainPage"] as Grid).Children.Remove(item);
            }
            var Content = (this.DataContext as UserManageModelView).SelectedAccount.ManagePage;
            if(!(Content is FrameworkElement)) {
                MessageBox.Show("不是FrameElement我不支持");
                return;
            }
            Content.OnAccountDeleted -= Content_OnAccountDeleted;
            Content.OnAccountDeleted += Content_OnAccountDeleted;
            Content.OnCanceled -= Content_OnCanceled;
            Content.OnCanceled += Content_OnCanceled;
            Content.OnSelected -= Content_OnSelected;
            Content.OnSelected += Content_OnSelected;
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new SpecialPages.AddAccountPage() { Tag = "AddNewAccount", Visibility = Visibility.Hidden, Content = Content });
            MyPageBase From = null;
            foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    From = item;
                }
            }
            foreach (MyPageBase item in FindControl.FromTag("AddNewAccount", (App.Current.Resources["MainPage"] as Grid)))
            {
                item.Show(From);
            }
            for (int i = 0; i < (App.Current.Resources["MainPage"] as Grid).Children.Count; i++)
            {
                if (((App.Current.Resources["MainPage"] as Grid).Children[i] as MyPageBase).Tag == "AddAccountPage")
                {
                    (App.Current.Resources["MainPage"] as Grid).Children.RemoveAt(i);
                }
            }
        }

        private void Content_OnSelected(object sender, AccountBase account)
        {
            if (account != UserManageModel.ModelView.SelectedAccount)
            {
                UserManageModel.ModelView.SelectedAccount = account;
            }
            MyPageBase From = null;
            foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    From = item;
                }
            }
            foreach (MyPageBase item in FindControl.FromTag("UserManagePage", (App.Current.Resources["MainPage"] as Grid)))
            {
                item.Show(From);
            }
            for (int i = 0; i < (App.Current.Resources["MainPage"] as Grid).Children.Count; i++)
            {
                if ((App.Current.Resources["MainPage"] as Grid).Children[i] == From)
                {
                    (App.Current.Resources["MainPage"] as Grid).Children.RemoveAt(i);
                }
            }
            UserManageModel.ModelView.Invoke("SelectedAccount");
            UserManageModel.ModelView.Invoke("Accounts");
        }

        private void Content_OnCanceled(object sender, AccountBase account)
        {
            MyPageBase From = null;
            foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    From = item;
                }
            }
            foreach (MyPageBase item in FindControl.FromTag("UserManagePage", (App.Current.Resources["MainPage"] as Grid)))
            {
                item.Show(From);
            }
            for (int i = 0; i < (App.Current.Resources["MainPage"] as Grid).Children.Count; i++)
            {
                if ((App.Current.Resources["MainPage"] as Grid).Children[i] == From)
                {
                    (App.Current.Resources["MainPage"] as Grid).Children.RemoveAt(i);
                }
            }
        }

        private void Content_OnAccountDeleted(object sender, AccountBase account)
        {
            UserManageModel.ModelView.SelectedAccount = account;
            MEFL.APIData.APIModel.AccountConfigs.Remove(account);
            if (UserManageModel.ModelView.SelectedAccount == UserManageModel.ModelView.SelectedAccount)
            {
                if (MEFL.APIData.APIModel.AccountConfigs.Count >= 1)
                {
                    UserManageModel.ModelView.SelectedAccount = APIData.APIModel.AccountConfigs[0];
                }
                else
                {
                    UserManageModel.ModelView.SelectedAccount = null;
                }
            }
            UserManageModel.ModelView.Invoke("Accounts");
            UserManageModel.ModelView.Invoke("SelectedAccount");
            MyPageBase From = null;
            foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    From = item;
                }
            }
            foreach (MyPageBase item in FindControl.FromTag("UserManagePage", (App.Current.Resources["MainPage"] as Grid)))
            {
                item.Show(From);
            }
            for (int i = 0; i < (App.Current.Resources["MainPage"] as Grid).Children.Count; i++)
            {
                if ((App.Current.Resources["MainPage"] as Grid).Children[i] == From)
                {
                    (App.Current.Resources["MainPage"] as Grid).Children.RemoveAt(i);
                }
            }
            account.Dispose();
        }
    }
}
