using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MEFL.Controls
{
    public class MyWindow:Window
    {
        static MyWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyWindow), new FrameworkPropertyMetadata(typeof(MyWindow)));
        }
    }
}
