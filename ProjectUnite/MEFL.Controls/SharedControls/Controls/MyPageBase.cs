using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MEFL.Controls
{
    public class MyPageBase : UserControl
    {
        static MyPageBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyPageBase), new FrameworkPropertyMetadata(typeof(MyPageBase)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (SideBar == null)
            {
                (Template.FindName("PART_SideBar", this) as ColumnDefinition)
                .Width = new GridLength(0);
                LineBrush= new SolidColorBrush(Colors.Transparent);
            }
        }

        public void Show(MyPageBase From)
        {
            if (From.Tag as String != this.Tag as String)
            {
                From.Hide();
                Show();
            }
        }
        public void Show()
        {
            var ani = new DoubleAnimation()
            {
                Duration = TimeSpan.FromSeconds(0.2),
                From = 0,
                To = 1,
                EasingFunction = new PowerEase()
            };
            BeginAnimation(OpacityProperty, ani);
            ani = null;
            Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            var ani = new DoubleAnimation()
            {
                Duration = TimeSpan.FromSeconds(0.2),
                From = 1,
                To = 0,
                EasingFunction = new PowerEase()
            };
            BeginAnimation(OpacityProperty, ani);
            ani = null;
            Visibility = Visibility.Hidden;
        }

        public Panel SideBar
        {
            get { return (Panel)GetValue(SideBarProperty); }
            set { SetValue(SideBarProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SideBar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SideBarProperty =
            DependencyProperty.Register("SideBar", typeof(Panel), typeof(MyPageBase), new PropertyMetadata(null));



        public Brush LineBrush
        {
            get { return (Brush)GetValue(LineBrushProperty); }
            set { SetValue(LineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineBrushProperty =
            DependencyProperty.Register("LineBrush", typeof(Brush), typeof(MyPageBase), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 0, 0))));


    }
}
