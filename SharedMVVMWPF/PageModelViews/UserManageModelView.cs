using MEFL.Contract;
using MEFL.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace MEFL.PageModelViews
{
    public class UserManageModelView : PageModelViewBase
    {
        public AccountCollection Accounts { get => APIData.APIModel.AccountConfigs; set { APIData.APIModel.AccountConfigs = value; Invoke("Accounts"); } }
        public Contract.AccountBase SelectedAccount { get => APIData.APIModel.SelectedAccount; set { APIData.APIModel.SelectedAccount = value; } }
        public ICommand AddAccountCommand { get; set; }
        public UserManageModelView()
        {
            AddAccountCommand = new AddAccountCommand();
        }
    }

    public static class UserManageModel
    {
        public static UserManageModelView ModelView { get; set; }
        static UserManageModel()
        {
            ModelView = new UserManageModelView();
        }
    }
    public class CurrectAccountWaring : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class AddAccountCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            foreach (MyPageBase item in FindControl.FromTag("AddAccountPage", (App.Current.Resources["MainPage"] as Grid)))
            {
                if (item.Tag as String == "AddAccountPage")
                {
                    GC.SuppressFinalize(item);
                    item.Hide();
                    (App.Current.Resources["MainPage"] as Grid).Children.Remove(item);
                }
            }
            for (int i = 0; i < (App.Current.Resources["MainPage"] as Grid).Children.Count; i++)
            {
                if (((App.Current.Resources["MainPage"] as Grid).Children[i] as MyPageBase).Tag == "AddNewAccount")
                {
                    (App.Current.Resources["MainPage"] as Grid).Children.RemoveAt(i);
                }
            }
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new SpecialPages.AddNewAccount() { Tag = "AddAccountPage", Visibility = Visibility.Hidden});
            MyPageBase From = null;
            foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    From = item;
                }
            }
            foreach (MyPageBase item in FindControl.FromTag("AddAccountPage", (App.Current.Resources["MainPage"] as Grid)))
            {
                item.Show(From);
            }
        }
    }

    public class AccountsToUI : IValueConverter
    {
        StackPanel panel = new StackPanel();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            panel.Children.Clear();
            var Target = value as ObservableCollection<Contract.AccountBase>;
            foreach (var item in Target)
            {
                if (item != APIData.APIModel.SelectedAccount)
                {
                    var element = new Controls.MyItemsCardItem() { Title = item.UserName, SubTitle = item.UserType, Icon = item.ProfileAvator,DataContext=item };
                    element.MouseDown += Element_MouseDown;
                    panel.Children.Add(element);
                }
            }
            return panel;
        }

        private void Element_MouseDown(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < (App.Current.Resources["MainPage"] as Grid).Children.Count; i++)
            {
                if (((App.Current.Resources["MainPage"] as Grid).Children[i] as MyPageBase).Tag == "AddNewAccount")
                {
                    (App.Current.Resources["MainPage"] as Grid).Children.RemoveAt(i);
                }
            }
            if(!(((sender as MyItemsCardItem).DataContext as AccountBase).ManagePage is FrameworkElement))
            {
                MessageBox.Show("应该是个FrameworkElement");
                return;
            }
            var Content = ((sender as MyItemsCardItem).DataContext as AccountBase).ManagePage;
            Content.OnAccountDeleted -= Content_OnAccountDeleted;
            Content.OnAccountDeleted += Content_OnAccountDeleted;
            Content.OnCanceled -= Content_OnCanceled;
            Content.OnCanceled += Content_OnCanceled;
            Content.OnSelected -= Content_OnSelected;
            Content.OnSelected += Content_OnSelected;
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new SpecialPages.AddAccountPage() { Tag = "AddNewAccount", Visibility = Visibility.Hidden,Content = Content as FrameworkElement});
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

            account.Dispose();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
