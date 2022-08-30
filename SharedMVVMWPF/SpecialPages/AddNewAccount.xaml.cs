using MEFL.APIData;
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
                                item.AddAccountContent.DataContext = new GenerlAddAccountModelView(item.FinnalReturn);
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

        private void Item_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var NewPage = new AddAccountPage() { Content = (sender as AddAccountItem).AddAccountContent,DataContext=new GenerlAddAccountModelView((sender as AddAccountItem).FinnalReturn),Tag= "AddNewAccount" };
            (App.Current.Resources["MainPage"] as Grid).Children.Add(NewPage);
            NewPage.Show(this);
        }
    }
}
