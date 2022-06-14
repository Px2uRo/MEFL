using System.ComponentModel;

namespace MEFL.ControlModelViews
{
    public class PageModelView : INotifyPropertyChanged
    {
#if CSHARP7_3
        public event PropertyChangedEventHandler PropertyChanged;
#else
        public event PropertyChangedEventHandler? PropertyChanged;
#endif
    }
}
