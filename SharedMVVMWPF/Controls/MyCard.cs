using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MEFL.Controls
{
    public class MyCard : UserControl
    {
        #region 你是个屁的 Methods
        DoubleAnimation OpacityAni;
        DoubleAnimation dbani;
        private double OriginalHeight { get; set; }
        private double Time;
        private IEasingFunction Ease;
        private DoubleAnimation dbaniIcon;
        #endregion
        static MyCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyCard), new FrameworkPropertyMetadata(typeof(MyCard)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            this.MouseEnter += MyCard_MouseEnter;
            this.MouseLeave += MyCard_MouseLeave;
            if (IsAbleToSwap == false)
            {
                (this.Template.FindName("PART_CheckBox_Icon", this) as Path).Visibility = Visibility.Hidden;
            }
            else
            {
                if(IsSwaped == false)
                {
                    (Template.FindName("PART_CheckBox_Icon_Rotate", this) as RotateTransform).Angle = 180;
                    (this.Template.FindName("PART_CheckBox", this) as System.Windows.Shapes.Rectangle).MouseDown += SwapBox_Swap;
                }
                else
                {
                    (this.Template.FindName("PART_CheckBox", this) as System.Windows.Shapes.Rectangle).MouseDown += SwapBox_UnSwap;
                    this.Loaded += MyCard_Initialized;
                }
            }
            Ease = new PowerEase();
        }

        private void MyCard_Initialized(object? sender, EventArgs e)
        {
            OriginalHeight = this.ActualHeight;
            this.Height = 40;
        }

        private void SwapBox_UnSwap(object sender, RoutedEventArgs e)
        {
            Time = OriginalHeight / 1000;
            dbani = new DoubleAnimation();
            dbani.Duration = new Duration(TimeSpan.FromSeconds(Time));
            dbani.From = 40;
            dbani.To = OriginalHeight;
            dbani.EasingFunction = Ease;
            dbaniIcon = new DoubleAnimation();
            dbaniIcon.Duration = new Duration(TimeSpan.FromSeconds(Time));
            dbaniIcon.From = 360;
            dbaniIcon.To = 180;
            this.BeginAnimation(HeightProperty, dbani);
            (Template.FindName("PART_CheckBox_Icon_Rotate",this) as RotateTransform).BeginAnimation(RotateTransform.AngleProperty,dbaniIcon);
            (sender as System.Windows.Shapes.Rectangle).MouseDown += SwapBox_Swap;
            (sender as System.Windows.Shapes.Rectangle).MouseDown -= SwapBox_UnSwap;  
        }

        private void SwapBox_Swap(object sender, RoutedEventArgs e)
        {
            if(OriginalHeight == 0)
            {
                OriginalHeight = this.ActualHeight;
            }
            Time = this.ActualHeight / 1000;
            dbani = new DoubleAnimation();
            dbani.Duration = new Duration(TimeSpan.FromSeconds(Time));
            dbani.From = Height;
            dbani.To = 40;
            dbani.EasingFunction = Ease;
            dbaniIcon = new DoubleAnimation();
            dbaniIcon.Duration = new Duration(TimeSpan.FromSeconds(Time));
            dbaniIcon.To = 360;
            dbaniIcon.From = 180;
            (Template.FindName("PART_CheckBox_Icon_Rotate", this) as RotateTransform).BeginAnimation(RotateTransform.AngleProperty, dbaniIcon);
            this.BeginAnimation(HeightProperty, dbani);
            (sender as System.Windows.Shapes.Rectangle).MouseDown += SwapBox_UnSwap;
            (sender as System.Windows.Shapes.Rectangle).MouseDown -= SwapBox_Swap;
        }

        private void MyCard_MouseLeave(object sender, MouseEventArgs e)
        {
            OpacityAni = new DoubleAnimation();
            OpacityAni.To = this.Opacity - 0.1;
            OpacityAni.From = this.Opacity;
            OpacityAni.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            this.BeginAnimation(OpacityProperty, OpacityAni);
        }

        //进入事件。
        private void MyCard_MouseEnter(object sender, MouseEventArgs e)
        {
            OpacityAni = new DoubleAnimation();
            OpacityAni.From = this.Opacity;
            OpacityAni.To = this.Opacity + 0.1;
            OpacityAni.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            this.BeginAnimation(OpacityProperty, OpacityAni);
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
            DependencyProperty.Register("Title", typeof(object), typeof(MyCard), new PropertyMetadata(string.Empty));


        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MyCard), new PropertyMetadata(new CornerRadius(10)));

        public new Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(MyCard), new PropertyMetadata(new Thickness(5)));



        public bool IsAbleToSwap
        {
            get { return (bool)GetValue(IsAbleToSwapProperty); }
            set { SetValue(IsAbleToSwapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAbleToSwap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAbleToSwapProperty =
            DependencyProperty.Register("IsAbleToSwap", typeof(bool), typeof(MyCard), new PropertyMetadata(false));



        public bool IsSwaped
        {
            get { return (bool)GetValue(IsSwapedProperty); }
            set { SetValue(IsSwapedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSwaped.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSwapedProperty =
            DependencyProperty.Register("IsSwaped", typeof(bool), typeof(MyCard), new PropertyMetadata(false));



        #endregion
    }
}
