using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MEFL.Controls
{
    public class MyComboBoxItem:UserControl
    {
        static MyComboBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyComboBoxItem), new FrameworkPropertyMetadata(typeof(MyComboBoxItem)));
        }


        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Index.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(MyComboBoxItem), new PropertyMetadata(-1));



    }
}
