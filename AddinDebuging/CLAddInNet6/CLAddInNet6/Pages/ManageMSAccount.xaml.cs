using MEFL.CLAddIn;
using MEFL.CLAddIn.AccountsManagement;
using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace CLAddInNet6.Pages
{
    /// <summary>
    /// ManageMSAccount.xaml 的交互逻辑
    /// </summary>
    public partial class ManageMSAccount : UserControl, IManageAccountPage
    {
        public ManageMSAccount()
        {
            InitializeComponent();
        }

        public event IManageAccountPage.Canceled OnCanceled;
        public event IManageAccountPage.AccountDeleted OnAccountDeleted;
        public event IManageAccountPage.Selected OnSelected;

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            OnCanceled.Invoke(this,DataContext as AccountBase);
        }

        private void MyButton_Click_1(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe","https://www.minecraft.net/zh-hans/msaprofile/mygames");
        }

        private void MyButton_Click_2(object sender, RoutedEventArgs e)
        {
            OnSelected.Invoke(this, DataContext as AccountBase);
        }

        private void MyButton_Click_3(object sender, RoutedEventArgs e)
        {
            Model.MicrosoftAccounts.RemoveOne(this.DataContext as MEFLMicrosoftAccount);
            OnAccountDeleted.Invoke(this, DataContext as AccountBase);
        }
    }
}
