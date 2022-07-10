using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MEFL.Controls
{
    public class MyComboBoxItem:ListBoxItem
    {
        static MyComboBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyComboBoxItem), new FrameworkPropertyMetadata(typeof(MyComboBoxItem)));
        }


        public Brush Decoration
        {
            get { return (Brush)GetValue(DecorationProperty); }
            set { SetValue(DecorationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Decoration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DecorationProperty =
            DependencyProperty.Register("Decoration", typeof(Brush), typeof(MyComboBoxItem), new PropertyMetadata(new SolidColorBrush(Colors.Gray)));


    }
}
