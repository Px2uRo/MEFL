using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MEFL
{
    public static class ControlModel
    {
        public static double TimeMultiple { get; set; }
        static ControlModel()
        {
            TimeMultiple = 1;
        }
    }
}
