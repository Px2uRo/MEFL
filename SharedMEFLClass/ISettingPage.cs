using System.Collections.ObjectModel;

namespace MEFL
{
#if WPF
    public interface ISettingPage:IBaseAddIn
    {
        public ObservableCollection<IconTitlePagePair> Contents();
    }

    public class IconTitlePagePair
    {
        public string Title;
        public object Icon;
        public object Page;
    }
#endif
}
