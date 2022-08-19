using MEFL.Arguments;
using MEFL.CLAddIn.Pages;
using MEFL.Contract;
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

        public bool UseAccountAPI => false;
    }

    [Export(typeof(IPages))]
    public class Pages : IPages
    {
        public Dictionary<object, MyPageBase> IconAndPage => new Dictionary<object, MyPageBase>() {
            {"MCER",new MCERPage() }
        };
    }
}
