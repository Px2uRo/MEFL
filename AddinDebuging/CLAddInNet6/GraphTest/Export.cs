using MEFL.Arguments;
using MEFL.CLAddIn.AccountsManagement;
using MEFL.Contract;
using MEFL.Contract.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTest
{
    [Export(typeof(IBaseInfo))]
    public class BaseInfo : IBaseInfo
    {
        public object Title => "GraphTest";

        public object Icon => "GRAPH";

        public Uri PulisherUri => new("https://space.bilibili.com/283605961");

        public Uri ExtensionUri => new("https://space.bilibili.com/283605961?");

        public void SettingsChange(SettingArgs args)
        {

        }
    }
    [Export(typeof(IPermissions))]
    public class Permissions : IPermissions
    {
        public bool UseSettingPageAPI => false;

        public bool UsePagesAPI => false;

        public bool UseGameManageAPI => false;

        public bool UseDownloadPageAPI => false;

        public bool UseAccountAPI => true;
    }
    [Export(typeof(IAccount))]
    public class Account : IAccount
    {
        public AccountBase[] GetSingUpAccounts(SettingArgs args)
        {
            return Model.List.ToArray();
        }

        public AddAccountItem[] GetSingUpPage(SettingArgs args)
        {
           throw new NotImplementedException();
        }
    }
}
