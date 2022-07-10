using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MEFL.Controls
{
    public class MyItemsCardItem: ContentControl
    {
        static MyItemsCardItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyItemsCardItem), new FrameworkPropertyMetadata(typeof(MyItemsCardItem)));
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(MyItemsCardItem), new PropertyMetadata(string.Empty));



        public string SubTitle
        {
            get { return (string)GetValue(SubTitleProperty); }
            set { SetValue(SubTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SubTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubTitleProperty =
            DependencyProperty.Register("SubTitle", typeof(string), typeof(MyItemsCardItem), new PropertyMetadata(string.Empty));




        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(object), typeof(MyItemsCardItem), new PropertyMetadata(null));


        public object PropertyItems
        {
            get { return (object)GetValue(PropertyItemsProperty); }
            set { SetValue(PropertyItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PropertyItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyItemsProperty =
            DependencyProperty.Register("PropertyItems", typeof(object), typeof(MyItemsCardItem), new PropertyMetadata(null));



        public Brush Decoration
        {
            get { return (Brush)GetValue(DecorationProperty); }
            set { SetValue(DecorationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Decoration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DecorationProperty =
            DependencyProperty.Register("Decoration", typeof(Brush), typeof(MyItemsCardItem), new PropertyMetadata(new SolidColorBrush(Colors.Gray)));
    }
}
