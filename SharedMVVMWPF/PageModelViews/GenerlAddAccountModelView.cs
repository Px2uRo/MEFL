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
    public class GenerlAddAccountModelView:PageModelViewBase
    {
        public AccountBase Account { get => GenerlAddAccountModel.Account; set { GenerlAddAccountModel.Account = value;Invoke("Account"); } }
        public ICommand AddCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public GenerlAddAccountModelView(AccountBase account)
        {
            Account = account;
            AddCommand = new AddCommand();
            CancelCommand = new CancelCommand();
            GenerlAddAccountModel.ModelView = this;
        }
    }

    public class CancelCommand : ICommand
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
            GenerlAddAccountModel.Account.Dispose();
        }
    }

    public class AddCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            UserManageModel.ModelView.SelectedAccount = GenerlAddAccountModel.Account;
            MEFL.APIData.APIModel.AccountConfigs.Add(GenerlAddAccountModel.Account);
            UserManageModel.ModelView.Invoke("Accounts");
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
                if (((App.Current.Resources["MainPage"] as Grid).Children[i] as FrameworkElement).Tag as String== "AddAccountPage")
                {
                    (App.Current.Resources["MainPage"] as Grid).Children.RemoveAt(i);
                }
            }
        }
    }

    public static class GenerlAddAccountModel
    {
        public static AccountBase Account { get; set; }
        public static GenerlAddAccountModelView ModelView { get; set; }
    }
}
