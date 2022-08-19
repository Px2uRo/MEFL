using MEFL.Arguments;
using MEFL.CLAddIn;
using MEFL.CLAddIn.Pages;
using MEFL.Contract;
using MEFL.Contract.Controls;
using MEFL.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace SharedCLAddIn
{
    [Export(typeof(IBaseInfo))]
    public class BaseInfo : IBaseInfo
    {
        public object Title => "CoreLaunching fo MEFL";

        public object Icon => "MEFL.jpg";

        public Uri PulisherUri => new Uri("https://space.bilibili.com/283605961");

        public Uri ExtensionUri => throw new NotImplementedException("https://space.bilibili.com/283605961");

        public void SettingsChange(SettingArgs args)
        {
            //throw new NotImplementedException();
        }
    }

    [Export(typeof(IPermissions))]
    public class Permissions : IPermissions
    {
        public bool UseSettingPageAPI =>false;

        public bool UsePagesAPI => true;

        public bool UseGameManageAPI => false;

        public bool UseDownloadPageAPI => false;

        public bool UseAccountAPI => true;
    }

    [Export(typeof(IPages))]
    public class Pages : IPages
    {
        public Dictionary<object, MyPageBase> IconAndPage => new Dictionary<object, MyPageBase>() {
            {"MCER",new MCERPage() }
        };
    }

    [Export(typeof(IAccount))]
    public class Account : IAccount
    {
        public List<AccountBase> GetSingUpAccounts(SettingArgs args)
        {
            return new List<AccountBase>();
        }

        public List<AddAccountItem> GetSingUpPage(SettingArgs args)
        {
            var res = new List<AddAccountItem>();
            var Legacy = new AddAccountItem() { Width = 400, Height = 60, AddAccountContent = new AddALegacyAccountPage(), FinnalReturn = new MEFLLegacyAccount(String.Empty, Guid.NewGuid().ToString()) };
            res.Add(Legacy);
            return res;
        }

        //private AddAccountItem Legacy = new AddAccountItem() { Width=400,Height=60, AddAccountContent = new AddALegacyAccountPage(),FinnalReturn=new MEFLLegacyAccount(String.Empty,Guid.NewGuid().ToString()) };
        //todo i18n thx
        //Legacy.Content= new TextBlock() { Text="离线账户",HorizontalAlignment=HorizontalAlignment.Center,VerticalAlignment=VerticalAlignment.Center,FontSize=30,FontWeight=FontWeight.FromOpenTypeWeight(999)};
        //Legacy.MouseDown += Item_MouseDown;
        //MyStackPanel.Children.Add(Legacy);

    }
}
