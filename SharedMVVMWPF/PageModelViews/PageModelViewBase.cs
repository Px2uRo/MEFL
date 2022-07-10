using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MEFL.PageModelViews
{
    public class PageModelViewBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Invoke(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }


}
