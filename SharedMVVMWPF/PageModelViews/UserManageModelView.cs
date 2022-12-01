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
                    var element = new Controls.MyItemsCardItem() { Title = item.UserName, SubTitle = item.EmailAddress, Icon = item.ProfileAvator,DataContext=item };
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
            var Content = ((sender as MyItemsCardItem).DataContext as AccountBase).ManagePage as FrameworkElement;
            Content.DataContext = new GenerlManageAccountModelView((sender as MyItemsCardItem).DataContext as AccountBase);
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new SpecialPages.AddAccountPage() { Tag = "AddNewAccount", Visibility = Visibility.Hidden,Content= Content});
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
