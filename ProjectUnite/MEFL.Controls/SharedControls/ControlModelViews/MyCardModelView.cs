﻿using System.ComponentModel;

namespace MEFL.ControlModelViews
{
    public class MyCardModelView : INotifyPropertyChanged
    {
#if CSHARP7_3
        public event PropertyChangedEventHandler PropertyChanged;
#else
        public event PropertyChangedEventHandler? PropertyChanged;
#endif

        public double TimeMultiple { get; set; }

        public MyCardModelView()
        {
            TimeMultiple = ControlModel.TimeMultiple;
        }
    }
}
