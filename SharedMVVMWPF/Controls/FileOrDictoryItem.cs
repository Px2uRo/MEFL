using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MEFL.Controls
{
    public class FileOrDictoryItem: UserControl
    {
        static FileOrDictoryItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileOrDictoryItem), new FrameworkPropertyMetadata(typeof(FileOrDictoryItem)));
        }
    }
}
