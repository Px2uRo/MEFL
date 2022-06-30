using System;
using System.Collections.Generic;
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
            (Template.FindName("PART_Popup", this) as Popup).Opened += MyComboBox_Initialized;
            this.SelectionChanged += MyComboBox_SelectionChanged;
        }

        private void MyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //object a;
            //a = (sender as MyComboBox).SelectedItem;
            //(sender as MyComboBox).Items[(sender as MyComboBox).SelectedIndex] = a;
            //(Template.FindName("PART_CP", this) as ContentControl).Content = a;
        }

        private void MyComboBox_Initialized(object? sender, EventArgs e)
        {
            if (OrignalHeight == 0.0)
            {
                this.OrignalHeight = (Template.FindName("PART_Popup_Border", this) as Border).ActualHeight;
                (Template.FindName("PART_Popup_Border", this) as Border).Height = 0;
            }
        }

        private void MyComboBox_DropDownClosed(object? sender, EventArgs e)
        {
            (Template.FindName("PART_Popup_Border", this) as Border)
    .BeginAnimation(HeightProperty, new DoubleAnimation()
    {
        To = 0,
        From = OrignalHeight,
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
            (Template.FindName("PART_Popup_Border", this) as Border)
    .BeginAnimation(HeightProperty, new DoubleAnimation()
    {
        From = 0,
        To = OrignalHeight,
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



        public double OrignalHeight
        {
            get { return (double)GetValue(OrignalHeightProperty); }
            set { SetValue(OrignalHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrignalHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrignalHeightProperty =
            DependencyProperty.Register("OrignalHeight", typeof(double), typeof(MyComboBox), new PropertyMetadata(0.0));



        #endregion
    }

    public class BoolToCR : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
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
    public class BoolTotTH : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
            {
                return new Thickness(1,1,1,0);
            }
            else
            {
                return new Thickness(1);
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
