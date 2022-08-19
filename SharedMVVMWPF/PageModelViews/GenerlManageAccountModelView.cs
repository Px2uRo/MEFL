using MEFL.Contract;
using MEFL.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MEFL.PageModelViews
{
    public class GenerlManageAccountModelView:PageModelViewBase
    {
        public AccountBase Account { get => GenerlManageAccountModel.Account; set { GenerlManageAccountModel.Account = value;Invoke("Account"); } }
        public ICommand SelectCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public static string CurrectName; 
        public static string CurrectUuid;
        public GenerlManageAccountModelView(AccountBase account)
        {
            Account = account;
            SelectCommand = new SelectAccountCommand();
            BackCommand = new BackAccountCommand();
            RemoveCommand = new RemoveAccountCommand();
            if (APIData.APIModel.SelectedAccount!=null)
            {
                CurrectName = GenerlManageAccountModel.Account.UserName.ToString();
                CurrectUuid = GenerlManageAccountModel.Account.Uuid.ToString();
            }
            GenerlManageAccountModel.ModelView = this;
        }
    }

    internal class SelectAccountCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (GenerlManageAccountModel.Account != UserManageModel.ModelView.SelectedAccount)
            {
                UserManageModel.ModelView.SelectedAccount = GenerlManageAccountModel.Account;
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
    }

    internal class BackAccountCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
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
    }

    public class RemoveAccountCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            UserManageModel.ModelView.SelectedAccount = GenerlManageAccountModel.Account;
            MEFL.APIData.APIModel.AccountConfigs.Remove(GenerlManageAccountModel.Account);
            if (UserManageModel.ModelView.SelectedAccount == UserManageModel.ModelView.SelectedAccount)
            {
                if(MEFL.APIData.APIModel.AccountConfigs.Count >= 1)
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
            GenerlManageAccountModel.Account.Dispose();
        }
    }

    public static class GenerlManageAccountModel
    {
        public static AccountBase Account { get; set; }
        public static GenerlManageAccountModelView ModelView { get; set; }
    }
}
