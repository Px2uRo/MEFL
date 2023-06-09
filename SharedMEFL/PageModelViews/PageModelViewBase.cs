﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MEFL.PageModelViews
{
    public class PageModelViewBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
#if WPF
        public void Invoke(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
#elif AVALONIA
        public void Invoke([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
#endif

    }
}
