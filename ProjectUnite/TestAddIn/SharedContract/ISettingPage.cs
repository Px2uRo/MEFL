using System.Collections.ObjectModel;

namespace MEFL.Contract;

public interface ISettingPage
{
	ObservableCollection<IconTitlePagePair> Contents();
}
