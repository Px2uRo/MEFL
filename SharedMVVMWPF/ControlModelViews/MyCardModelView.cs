using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MEFL.ControlModelViews
{
    public class MyCardModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public double TimeMultiple { get; set; }

        public MyCardModelView()
        {
            TimeMultiple = ControlModel.TimeMultiple;
        }
    }
}
