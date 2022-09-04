using System.ComponentModel;

namespace MEFL.CLAddIn.PageModelViews
{
    public class ModelViewBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void RasieEvnet(string Property)
        {
            PropertyChanged.Invoke(this,new PropertyChangedEventArgs(Property));
        }
    }
}
