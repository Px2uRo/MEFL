using MEFL.APIData;
using MEFL.Contract;
using MEFL.Contract.Controls;
using MEFL.Controls;
using MEFL.PageModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MEFL.SpecialPages
{
    /// <summary>
    /// AddNewAccount.xaml 的交互逻辑
    /// </summary>
    public partial class AddNewAccount : MyPageBase
    {

        public AddNewAccount()
        {
            InitializeComponent();
            foreach (var hst in APIModel.Hostings)
            {
                if (hst.IsOpen)
                {
                    if (hst.Permissions.UseAccountAPI)
                    {
                        try
                        {
                            var pages = hst.Account.GetSingUpPage(APIModel.SettingArgs);
                            foreach (var item in pages)
                            {
                                item.MinWidth = 400;
                                item.MinHeight = 60; 
                                item.Width = 400;
                                item.Height = 60;
                                item.MouseDown += Item_MouseDown;
                                MyStackPanel.Children.Add(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debugger.Logger($"无法获取 {hst.FileName} 的登录页面，{ex.Message} at {ex.Source}");
                        }
                    }
                }
            }
        }

        private void Item_OnAccountAdded(object sender,AccountBase account)
        {
            UserManageModel.ModelView.SelectedAccount = account;
            MEFL.APIData.APIModel.AccountConfigs.Add(account);
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
                if (((App.Current.Resources["MainPage"] as Grid).Children[i] as FrameworkElement).Tag as String == "AddAccountPage")
                {
                    (App.Current.Resources["MainPage"] as Grid).Children.RemoveAt(i);
                }
            }
        }

        private void Item_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MyStackPanel.Children.Clear();
            var content = (sender as AddAccountItem).AddAccountContent;
            var NewPage = new AddAccountPage() { Content = content
                ,Tag = "AddAccountPage"
            };
            if(!(content is FrameworkElement))
            {
                MessageBox.Show("添加用户页面不是 FrameworkElement，请联系开发者");
                return;
            }
            content.OnAccountAdd -= Item_OnAccountAdded;
            content.OnAccountAdd += Item_OnAccountAdded;
            content.OnCanceled -= Content_OnCanceled;
            content.OnCanceled += Content_OnCanceled;
            (App.Current.Resources["MainPage"] as Grid).Children.Add(NewPage);
            NewPage.Show(this);
        }

        private void Content_OnCanceled(object sender, AccountBase account)
        {
            throw new NotImplementedException();
        }
    }
}
