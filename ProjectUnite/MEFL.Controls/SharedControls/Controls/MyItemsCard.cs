using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MEFL.Controls
{
    public class MyItemsCard:ListBox
    {
        public void OverrideOriginalHeight(double height)
        {
            var target = Template.FindName("PART_MY_CARD", this) as MyCard;
            target.OriginalHeight = height;
        }
        static MyItemsCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyItemsCard), new FrameworkPropertyMetadata(typeof(MyItemsCard)));
        }

        #region Propdp
        public object Title
        {
            get
            {

                return (object)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(object), typeof(MyItemsCard), new PropertyMetadata(string.Empty));


        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MyItemsCard), new PropertyMetadata(new CornerRadius(10)));

        public new Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(MyItemsCard), new PropertyMetadata(new Thickness(5)));



        public bool IsAbleToSwap
        {
            get { return (bool)GetValue(IsAbleToSwapProperty); }
            set { SetValue(IsAbleToSwapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAbleToSwap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAbleToSwapProperty =
            DependencyProperty.Register("IsAbleToSwap", typeof(bool), typeof(MyItemsCard), new PropertyMetadata(false));



        public bool IsSwaped
        {
            get { return (bool)GetValue(IsSwapedProperty); }
            set { SetValue(IsSwapedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSwaped.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSwapedProperty =
            DependencyProperty.Register("IsSwaped", typeof(bool), typeof(MyItemsCard), new PropertyMetadata(false));
        #endregion


    }

}
