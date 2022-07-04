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
            this.Loaded += MyComboBox_Loaded;
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            if (Template != null)
            {
                (Template.FindName("PART_Popup_Border_Contents", this) as StackPanel).Children.Clear();
                for (int i = 0; i < Items.Count; i++)
                {
                    var combo = new MyComboBoxItem() { Content = Items[i], Index = i };
                    combo.MouseDown += Combo_MouseDown;
                    (Template.FindName("PART_Popup_Border_Contents", this) as StackPanel)
                    .Children.Add(combo);
                }
            }
        }

        private void Combo_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.SelectedIndex = (sender as MyComboBoxItem).Index;
        }

        private void MyComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            (Template.FindName("PART_Popup_Border_Contents", this) as StackPanel).Children.Clear();
            for (int i = 0; i < Items.Count; i++)
            {
                var combo = new MyComboBoxItem() { Content = Items[i], Index = i };
                combo.MouseDown += Combo_MouseDown;
                (Template.FindName("PART_Popup_Border_Contents", this) as StackPanel)
                .Children.Add(combo);
            }
        }

        private void MyComboBox_DropDownClosed(object? sender, EventArgs e)
        {
            (Template.FindName("PART_Popup_Border", this) as Border)
    .BeginAnimation(HeightProperty, new DoubleAnimation()
    {
        To = 0,
        From = 80,
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
        To = 80,
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
