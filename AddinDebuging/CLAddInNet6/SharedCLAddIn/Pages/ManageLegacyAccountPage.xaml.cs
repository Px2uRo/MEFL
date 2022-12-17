using MEFL.Contract;
using Newtonsoft.Json;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MEFL.CLAddIn.Pages
{
    /// <summary>
    /// AddALegacyAccount.xaml 的交互逻辑
    /// </summary>
    public partial class ManageALegacyAccountPage : UserControl,IManageAccountPage
    {
        public ManageALegacyAccountPage()
        {
            InitializeComponent();
        }

        public event IManageAccountPage.Canceled OnCanceled;
        public event IManageAccountPage.AccountDeleted OnAccountDeleted;
        public event IManageAccountPage.Selected OnSelected;

        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {

        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            //MEFL.APIData.APIModel.LegacyAccounts.Remove(GenerlManageAccountModel.Account);
            //RegManager.Write("LegacyAccounts", JsonConvert.SerializeObject(APIData.APIModel.LegacyAccounts));
        }

        private void MyButton_Click_1(object sender, RoutedEventArgs e)
        {
            OnCanceled.Invoke(this,this.DataContext as AccountBase);
        }

        private void MyButton_Click_2(object sender, RoutedEventArgs e)
        {
            OnSelected.Invoke(this, this.DataContext as AccountBase);
        }

        private void MyButton_Click_3(object sender, RoutedEventArgs e)
        {
            OnAccountDeleted.Invoke(this, this.DataContext as AccountBase);
        }
    }
}
