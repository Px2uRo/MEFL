using System.Collections.ObjectModel;

namespace MEFL.Contract
{
#if WPF&&CONTRACT
    public interface ISettingPage
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
