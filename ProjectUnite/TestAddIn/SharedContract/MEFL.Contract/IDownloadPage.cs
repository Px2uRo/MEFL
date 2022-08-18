using System.Collections.ObjectModel;

namespace MEFL.Contract;

public interface IDownloadPage
{
	ObservableCollection<DownloadPageItemPair> GetPairs();
}
