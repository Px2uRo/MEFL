using MEFL.PageModelViews;
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

namespace MEFL.SpecialPages
{
    /// <summary>
    /// AddALegacyAccount.xaml 的交互逻辑
    /// </summary>
    public partial class ManageALegacyAccountPage : UserControl
    {
        public ManageALegacyAccountPage()
        {
            InitializeComponent();
        }

        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            ErrorInfoBox.Text = e.Error.ErrorContent.ToString();
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            MEFL.APIData.APIModel.LegacyAccounts.Remove(GenerlManageAccountModel.Account);
            RegManager.Write("LegacyAccounts", JsonConvert.SerializeObject(APIData.APIModel.LegacyAccounts));
        }
    }
}
