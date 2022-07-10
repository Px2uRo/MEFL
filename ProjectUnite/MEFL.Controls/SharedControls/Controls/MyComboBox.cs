using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MEFL.Controls
{
    public class MyComboBox:ComboBox
    {
        static MyComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyComboBox), new FrameworkPropertyMetadata(typeof(MyComboBox)));
        }

        public override void OnApplyTemplate()
        {
            (Template.FindName("PART_Border", this) as Border).MouseDown += MyComboBox_MouseDown;
            this.DropDownOpened += MyComboBox_DropDownOpened;
            this.DropDownClosed += MyComboBox_DropDownClosed;
            base.OnApplyTemplate();
        }
        private void MyComboBox_DropDownClosed(object? sender, EventArgs e)
        {
            (Template.FindName("PART_Popup", this) as Popup)
    .BeginAnimation(HeightProperty, new DoubleAnimation()
    {
        To = 0,
        From = ComboHeight,
        Duration = TimeSpan.FromSeconds(0.2),
        EasingFunction = new PowerEase()
    });
            (Template.FindName("RTF", this) as RotateTransform)
    .BeginAnimation(RotateTransform.AngleProperty, new DoubleAnimation()
    {
        From = 180,
        To = 360,
        Duration = TimeSpan.FromSeconds(0.2),
        EasingFunction = new PowerEase()
    });

        }
        private void MyComboBox_DropDownOpened(object? sender, EventArgs e)
        {
            (Template.FindName("PART_Popup", this) as Popup)
    .BeginAnimation(HeightProperty, new DoubleAnimation()
    {
        From = 0,
        To = ComboHeight,
        Duration = TimeSpan.FromSeconds(0.2),
        EasingFunction = new PowerEase()
    });
            (Template.FindName("RTF", this) as RotateTransform)
                .BeginAnimation(RotateTransform.AngleProperty, new DoubleAnimation()
                {
                    From=0,
                    To=180,
                    Duration = TimeSpan.FromSeconds(0.2),
                    EasingFunction = new PowerEase()
                });

        }
        private void MyComboBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.IsDropDownOpen)
            {
                IsDropDownOpen = false;
            }
            else
            {
                IsDropDownOpen = true;
            }
        }

        #region Propdps



        public double ComboHeight
        {
            get { return (double)GetValue(ComboHeightProperty); }
            set { SetValue(ComboHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ComboHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ComboHeightProperty =
            DependencyProperty.Register("ComboHeight", typeof(double), typeof(MyComboBox), new PropertyMetadata(75.0));


        public new Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public static readonly new DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(MyComboBox), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));



        public Brush MyItemColor
        {
            get { return (Brush)GetValue(MyItemColorProperty); }
            set { SetValue(MyItemColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyItemColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyItemColorProperty =
            DependencyProperty.Register("MyItemColor", typeof(Brush), typeof(MyComboBox), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        #endregion
    }

    public class BoolToCR : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == false)
            {
                return new CornerRadius(2, 2, 0, 0);
            }
            else
            {
                return new CornerRadius(2);
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
