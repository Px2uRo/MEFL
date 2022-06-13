using System.Collections.ObjectModel;
using System.Windows;

namespace MEFL.Contract
{
#if WPF&&CONTRACT
    public interface ISettingPage
    {
#if NET4_0
        ObservableCollection<IconTitlePagePair> Contents();

#else
        public ObservableCollection<IconTitlePagePair> Contents();
#endif
    }

    public class IconTitlePagePair
    {
        public string Title;
        public object Icon;
        public object Page;
    }
#endif
    }
