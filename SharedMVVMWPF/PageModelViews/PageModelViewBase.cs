using System.ComponentModel;

namespace MEFL.PageModelViews
{
    public class PageModelViewBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
