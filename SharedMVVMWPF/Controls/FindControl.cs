﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MEFL.Controls
{
    public static class FindControl
    {
        public static FrameworkElement[] FromTag(object Tag,Panel Panel)
        {
            List<FrameworkElement> l = new List<FrameworkElement>();
            foreach (FrameworkElement item in (Panel.Children))
            {
                if (item.Tag as String == Tag as String)
                {
                    l.Add(item);
                }
            }
            GC.SuppressFinalize(l);
            return l.ToArray();
        }
    }
}
