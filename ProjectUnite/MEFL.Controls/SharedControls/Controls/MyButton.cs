using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MEFL.Controls
{
    public class MyButton : Button
    {
        private DoubleAnimation _dbani;
        private double _speed = 200;
        static MyButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyButton), new FrameworkPropertyMetadata(typeof(MyButton)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.Click += MyButton_Click;
            this.MouseEnter += MyButton_MouseEnter;
            this.MouseLeave += MyButton_MouseLeave;
        }
        private void MyButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _dbani = new DoubleAnimation()
            {
                From = 0.95,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2),
                EasingFunction = new PowerEase()
            };
            (Template.FindName("PART_Background_Rect", this) as Border)
            .BeginAnimation(OpacityProperty, _dbani);
            _dbani = null;
        }

        private void MyButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _dbani = new DoubleAnimation()
            {
                From = 1,
                To = 0.95,
                Duration = TimeSpan.FromSeconds(0.2),
                EasingFunction = new PowerEase()
            };
            (Template.FindName("PART_Background_Rect", this) as Border)
            .BeginAnimation(OpacityProperty, _dbani);
            _dbani = null;
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            _dbani = new DoubleAnimation()
            {
                From = 0,
                To = ActualWidth,
                Duration = TimeSpan.FromSeconds((ActualWidth + 20)/_speed),
                EasingFunction = new PowerEase(),
            };
            (Template.FindName("PART_MyEllipse", this) as EllipseGeometry).Center = Mouse.GetPosition(this);
            (Template.FindName("PART_MyEllipse", this) as EllipseGeometry).BeginAnimation(EllipseGeometry.RadiusXProperty,_dbani);
            _dbani = null;

            _dbani = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds((ActualWidth + 20) / _speed),
                EasingFunction = new PowerEase(),
            };
            (Template.FindName("PART_Path", this) as Path).BeginAnimation(OpacityProperty, _dbani);
            _dbani = null;
        }
        public new SolidColorBrush Background
        {
            get { return (SolidColorBrush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public static readonly new DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(SolidColorBrush), typeof(MyButton), new PropertyMetadata(new SolidColorBrush(Colors.White)));


        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MyButton), new PropertyMetadata(new CornerRadius(5)));



        public new Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderThickness.  This enables animation, styling, binding, etc...
        public static readonly new DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(MyButton), new PropertyMetadata(new Thickness(3)));
    }
}
