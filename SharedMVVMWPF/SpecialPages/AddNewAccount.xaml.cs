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
        private AddAccountItem Legacy = new AddAccountItem() { Width=400,Height=60, AddAccountContent = new AddALegacyAccountPage(),FinnalReturn=new MEFLLegacyAccount(String.Empty,Guid.NewGuid().ToString()) };
        public AddNewAccount()
        {
            InitializeComponent();
            //todo i18n thx
            Legacy.Content= new TextBlock() { Text="离线账户",HorizontalAlignment=HorizontalAlignment.Center,VerticalAlignment=VerticalAlignment.Center,FontSize=30,FontWeight=FontWeight.FromOpenTypeWeight(999)};
            Legacy.MouseDown += Item_MouseDown;
            MyStackPanel.Children.Add(Legacy);
        }

        private void Item_MouseDown(object sender, MouseButtonEventArgs e)
        {

            for (int i = 0; i < (App.Current.Resources["MainPage"] as Grid).Children.Count; i++)
            {
                if ((App.Current.Resources["MainPage"] as Grid).Children[i]==this)
                {
                    (App.Current.Resources["MainPage"] as Grid).Children.RemoveAt(i);
                }
            }
            MyPageBase From = new MyPageBase();
            foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    From = item;
                }
            }
            GenerlAddAccountModel.ModelView = new GenerlAddAccountModelView((sender as AddAccountItem).FinnalReturn);
            (sender as AddAccountItem).AddAccountContent.DataContext = GenerlAddAccountModel.ModelView;
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new SpecialPages.AddAccountPage() { Tag= "AddAccountPage",Content=(sender as AddAccountItem).AddAccountContent });
            foreach (MyPageBase item in FindControl.FromTag("AddAccountPage", (App.Current.Resources["MainPage"] as Grid)))
            {
                item.Show(From);
            }
        }
    }
}
