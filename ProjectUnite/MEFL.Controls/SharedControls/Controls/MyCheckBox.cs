using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MEFL.Controls
{
    public class MyCheckBox:ToggleButton
    {
        private IEasingFunction _Ease;
        private SolidColorBrush _Brush1;
        private SolidColorBrush _Brush2;
        private SolidColorBrush _Brush3;
        static MyCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyCheckBox), new FrameworkPropertyMetadata(typeof(MyCheckBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.Checked += MySwitchButton_Checked;
            this.Unchecked += MySwitchButton_Unchecked;
            if (IsChecked == true)
            {
                (Template.FindName("PART_Circle", this) as Ellipse).BeginAnimation(MarginProperty, new ThicknessAnimation()
                {
                    From = new Thickness(2, 0, 0, 0),
                    To = new Thickness(11, 0, 0, 0),
                    Duration = TimeSpan.FromSeconds(0.2),
                    EasingFunction = new PowerEase()
                });
                _Brush1 = new SolidColorBrush();
                _Brush1.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation()
                {
                    Duration = new Duration(TimeSpan.FromSeconds(0.2)),
                    EasingFunction = _Ease,
                    From = Colors.White,
                    To = (this.Background as SolidColorBrush).Color
                });
                (Template.FindName("PART_Border", this) as Border).Background = _Brush1;
                _Brush2 = new SolidColorBrush();
                _Brush2.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation()
                {
                    Duration = new Duration(TimeSpan.FromSeconds(0.2)),
                    EasingFunction = _Ease,
                    To = (this.Background as SolidColorBrush).Color,
                    From = Colors.Black
                });
                (Template.FindName("PART_Border", this) as Border).BorderBrush = _Brush2;
                _Brush3 = new SolidColorBrush();
                _Brush3.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation()
                {
                    Duration = new Duration(TimeSpan.FromSeconds(0.2)),
                    EasingFunction = _Ease,
                    To = Colors.White,
                    From = Colors.Black
                });
                (Template.FindName("PART_Circle", this) as Ellipse).Fill = _Brush3;
            }
        }

        private void MySwitchButton_Unchecked(object sender, RoutedEventArgs e)
        {
            _Ease = new PowerEase();
            (Template.FindName("PART_Circle", this) as Ellipse).BeginAnimation(MarginProperty, new ThicknessAnimation()
            {
                From=new Thickness(11,0,0,0),
                To=new Thickness(2,0,0,0),
                Duration=TimeSpan.FromSeconds(0.2),
                EasingFunction=_Ease
            });
            _Brush1 = new SolidColorBrush();
            _Brush1.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.2)),
                EasingFunction = _Ease,
                To = Colors.White,
                From = (this.Background as SolidColorBrush).Color
            });
            (Template.FindName("PART_Border", this) as Border).Background=_Brush1;
            _Brush2 = new SolidColorBrush();
            _Brush2.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.2)),
                EasingFunction = _Ease,
                From = (this.Background as SolidColorBrush).Color,
                To = Colors.Black
            });
            (Template.FindName("PART_Border", this) as Border).BorderBrush = _Brush2;
            _Brush3 = new SolidColorBrush();
            _Brush3.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.2)),
                EasingFunction = _Ease,
                From = Colors.White,
                To = Colors.Black
            });
            (Template.FindName("PART_Circle", this) as Ellipse).Fill = _Brush3;
        }

        private void MySwitchButton_Checked(object sender, RoutedEventArgs e)
        {
            (Template.FindName("PART_Circle", this) as Ellipse).BeginAnimation(MarginProperty, new ThicknessAnimation()
            {
                From = new Thickness(2, 0, 0, 0),
                To = new Thickness(11, 0, 0, 0),
                Duration = TimeSpan.FromSeconds(0.2),
                EasingFunction = new PowerEase()
            });
            _Brush1 = new SolidColorBrush();
            _Brush1.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.2)),
                EasingFunction = _Ease,
                From = Colors.White,
                To = (this.Background as SolidColorBrush).Color
            });
            (Template.FindName("PART_Border", this) as Border).Background = _Brush1;
            _Brush2 = new SolidColorBrush();
            _Brush2.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.2)),
                EasingFunction = _Ease,
                To = (this.Background as SolidColorBrush).Color,
                From = Colors.Black
            });
            (Template.FindName("PART_Border", this) as Border).BorderBrush = _Brush2;
            _Brush3 = new SolidColorBrush();
            _Brush3.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.2)),
                EasingFunction = _Ease,
                To = Colors.White,
                From = Colors.Black
            });
            (Template.FindName("PART_Circle", this) as Ellipse).Fill = _Brush3;
        }



        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(MyCheckBox), new PropertyMetadata(new SolidColorBrush(Colors.White)));


    }
}
