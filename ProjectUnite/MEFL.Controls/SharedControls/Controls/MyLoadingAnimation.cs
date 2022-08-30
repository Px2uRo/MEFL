using MEFL.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MEFL.Controls
{
    public class MyLoadingAnimation:UserControl
    {
        static DoubleAnimationUsingKeyFrames _DbAni;
        static MyLoadingAnimation()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyLoadingAnimation), new FrameworkPropertyMetadata(typeof(MyLoadingAnimation)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_DbAni == null)
            {
                _DbAni = new DoubleAnimationUsingKeyFrames();
                var KeyFrames = new DoubleKeyFrameCollection();

                KeyFrames.Add(new EasingDoubleKeyFrame(0));
                KeyFrames.Add(new EasingDoubleKeyFrame(1));
                _DbAni.KeyFrames = KeyFrames;
                _DbAni.Duration = new Duration(TimeSpan.FromSeconds(2));
                _DbAni.RepeatBehavior = RepeatBehavior.Forever;
            }
            (Template.FindName("PART_Rectangle",this) as Rectangle).BeginAnimation(OpacityProperty,_DbAni);
        }


        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(MyLoadingAnimation), new PropertyMetadata(new SolidColorBrush(Colors.Black)));


    }
}
