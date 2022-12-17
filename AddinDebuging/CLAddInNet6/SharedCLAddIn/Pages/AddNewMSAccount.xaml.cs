using MEFL.Contract;
using Microsoft.Win32;
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

namespace CLAddInNet6
{
    /// <summary>
    /// AddNewMSAccount.xaml 的交互逻辑
    /// </summary>
    public partial class AddNewMSAccount : UserControl,IAddAccountPage
    {
        public AddNewMSAccount()
        {
            InitializeComponent();
        }

        public event IAddAccountPage.AccountAdd OnAccountAdd;
        public event IAddAccountPage.Canceled OnCanceled;

        public AccountBase GetFinalReturn()
        {
            throw new NotImplementedException();
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command\");
            string s = key.GetValue("").ToString();
            var url = @"https://login.live.com/oauth20_authorize.srf?client_id=00000000402b5328^&^response_type=code^&^scope=service^%^3A^%^3Auser.auth.xboxlive.com^%^3A^%^3AMBI_SSL^&^redirect_uri=https^%^3A^%^2F^%^2Flogin.live.com^%^2Foauth20_desktop.srf";
            key.Close();
            key.Dispose();

            s = s.Replace("%1",url);
            var p = new Process();
            p.StartInfo.RedirectStandardInput= true;
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(s);
            p.Close();
            p.Dispose();
        }

        private void MyButton_Click_1(object sender, RoutedEventArgs e)
        {
            OnAccountAdd.Invoke(this,null);
        }
    }
}
