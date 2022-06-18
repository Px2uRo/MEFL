using MEFL.Contract;
using MEFL.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;

namespace TestAddIn
{
    [Export(typeof(IBaseInfo))]
    public class BaseInfo : IBaseInfo
    {
        public object Title => null;

        public object Icon => new MyIcon();

        public Uri PulisherUri => new Uri("https://space.bilibili.com/283605961");

        public Uri ExtensionUri => new Uri("https://space.bilibili.com/283605961");
    }

    [Export(typeof(IPermissions))]
    public class Permissions : IPermissions
    {
        public bool UseSeetingPageAPI => true;

        public bool UsePagesAPI => true;
    }

    [Export(typeof(ISettingPage))]
    public class SettingPage : ISettingPage
    {
        public ObservableCollection<IconTitlePagePair> Contents()
        {
            var result = new ObservableCollection<IconTitlePagePair>();
            var item = new IconTitlePagePair();
            item.Title = "TEST";
            item.Page = new FrameworkElement();
            result.Add(item);
            return result;
        }
    }

    [Export(typeof(IPages))]
    public class Pages : IPages
    {
        public Dictionary<object, MyPageBase> IconAndPage
        {
            get
            {
                var res = new Dictionary<object, MyPageBase>();
                res.Add(new MyIcon(), new MyPage());
                return res;
                res = null;
            }
        }
    }
}
