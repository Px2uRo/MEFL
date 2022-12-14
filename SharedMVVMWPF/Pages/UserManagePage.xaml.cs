using System.Windows;
using System.Windows.Controls;
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
            var Content = (this.DataContext as UserManageModelView).SelectedAccount.ManagePage;
            Content.DataContext = new GenerlManageAccountModelView((this.DataContext as UserManageModelView).SelectedAccount);
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
    }
}
