using Microsoft.VisualBasic.CompilerServices;
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
        #region Methods
        ColorAnimation ani;
        DoubleAnimation dbani;
        private double OriginalHeight;
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
            (this.Template.FindName("PART_CheckBox", this) as System.Windows.Shapes.Rectangle).MouseDown += CheckBox_Checked;
            Ease = new PowerEase();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            dbani = new DoubleAnimation();
            dbani.Duration = new Duration(TimeSpan.FromSeconds(Time));
            dbani.From = 40;
            dbani.To = OriginalHeight;
            dbani.EasingFunction = Ease;
            dbaniIcon = new DoubleAnimation();
            dbaniIcon.Duration = new Duration(TimeSpan.FromSeconds(Time));
            dbaniIcon.From = 0;
            dbaniIcon.To = 180;
            this.BeginAnimation(HeightProperty, dbani);
            (Template.FindName("PART_CheckBox_Icon_Rotate",this) as RotateTransform).BeginAnimation(RotateTransform.AngleProperty,dbaniIcon);
            (sender as System.Windows.Shapes.Rectangle).MouseDown += CheckBox_Checked;
            (sender as System.Windows.Shapes.Rectangle).MouseDown -= CheckBox_Unchecked;  
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Time = this.ActualHeight / 1000;
            dbani = new DoubleAnimation();
            dbani.Duration = new Duration(TimeSpan.FromSeconds(Time));
            OriginalHeight = Height;
            dbani.From = Height;
            dbani.To = 40;
            dbani.EasingFunction = Ease;
            dbaniIcon = new DoubleAnimation();
            dbaniIcon.Duration = new Duration(TimeSpan.FromSeconds(Time));
            dbaniIcon.To = 0;
            dbaniIcon.From = 180;
            (Template.FindName("PART_CheckBox_Icon_Rotate", this) as RotateTransform).BeginAnimation(RotateTransform.AngleProperty, dbaniIcon);
            this.BeginAnimation(HeightProperty, dbani);
            (sender as System.Windows.Shapes.Rectangle).MouseDown += CheckBox_Unchecked;
            (sender as System.Windows.Shapes.Rectangle).MouseDown -= CheckBox_Checked;
        }

        private void MyCard_MouseLeave(object sender, MouseEventArgs e)
        {
            ani = new ColorAnimation();
            ani.To = Color.FromArgb(10, 102, 153, 253);
            ani.From = Color.FromArgb(50, 102, 153, 253);
            ani.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            SolidColorBrush brush = new SolidColorBrush();
            brush.BeginAnimation(SolidColorBrush.ColorProperty, ani);
            this.Background = brush;
        }

        private void MyCard_MouseEnter(object sender, MouseEventArgs e)
        {
            ani = new ColorAnimation();
            ani.From = Color.FromArgb(10,102,153,253);
            ani.To = Color.FromArgb(50, 102, 153, 253);
            ani.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            SolidColorBrush brush = new SolidColorBrush();
            brush.BeginAnimation(SolidColorBrush.ColorProperty, ani);
            this.Background = brush;
        }

        public object Title
        {
            get { 

                return (object)GetValue(TitleProperty); 
            }
            set {
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



        public Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(MyCard), new PropertyMetadata(new Thickness(5)));



        public SolidColorBrush Background
        {
            get { return (SolidColorBrush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(SolidColorBrush), typeof(MyCard), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(00,00,00,00))));




    }
}
