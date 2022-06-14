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
    public class ChangePageContentButton : RadioButton
    {
        //  <Rectangle Width="5" Height="35"
        //   Fill="{TemplateBinding SideBarBrush}" 
        //   Name="SideRect"/>

        static ChangePageContentButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChangePageContentButton), new FrameworkPropertyMetadata(typeof(ChangePageContentButton)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.IsChecked!=true)
            {
                (Template.FindName("SideRect", this) as Rectangle)
                    .Opacity = 0;
            }
            this.Checked += ChangePageContentButton_Checked;
            this.MouseEnter += ChangePageContentButton_MouseEnter;
            this.MouseLeave += ChangePageContentButton_MouseLeave;
        }

        private void ChangePageContentButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (Template.FindName("BackgroundRect", this) as Rectangle)
                .BeginAnimation(OpacityProperty,
                new DoubleAnimation()
                {
                    From=0.2,
                    To=0,
                    EasingFunction=new PowerEase(),
                    Duration=TimeSpan.FromSeconds(0.2)
                });
        }

        private void ChangePageContentButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (Template.FindName("BackgroundRect", this) as Rectangle)
                .BeginAnimation(OpacityProperty,
                new DoubleAnimation()
                {
                    From = 0,
                    To = 0.2,
                    EasingFunction = new PowerEase(),
                    Duration = TimeSpan.FromSeconds(0.2)
                });
        }

        public void ChangePageContentButton_Checked(object sender, RoutedEventArgs e)
        {
            int FromIndex = -1;
            var Panel = ((sender as ChangePageContentButton).Parent as StackPanel).Children;
            for (int i = 0; i < Panel.Count; i++)
            {
                //(Panel[i] as ChangePageContentButton)
                if ((Template.FindName("SideRect", (Panel[i] as ChangePageContentButton)) as Rectangle).Opacity == 1)
                {
                    FromIndex = i;
                }
            }
            if(FromIndex != -1)
            {
                //(Template.FindName("SideRect", (Panel[FromIndex] as ChangePageContentButton)) as Rectangle);

                foreach (ChangePageContentButton item in Panel)
                {
                    (item.Template.FindName("SideRect", item) as Rectangle)
                .Opacity = 0;
                }
                #region UselessCode
                //for (int i = 0; i < Time; i++)
                //{
                //    if (FromIndex >= ToIndex)
                //    {
                //        var AtrualHeight = (Panel[FromIndex - i] as ChangePageContentButton).ActualHeight;

                //        ((Panel[FromIndex - i] as ChangePageContentButton)
                //            .Template.FindName
                //            ("SideRect",
                //            (Panel[FromIndex - i] as ChangePageContentButton)) as Rectangle)
                //            .Opacity = 1;
                //        ((Panel[FromIndex - i] as ChangePageContentButton)
                //            .Template.FindName
                //            ("SideRect",
                //            (Panel[FromIndex - i] as ChangePageContentButton)) as Rectangle)
                //            .VerticalAlignment = VerticalAlignment.Top;

                //        (Panel[FromIndex - i] as ChangePageContentButton)
                //            .Height = AtrualHeight;

                //        var newani = new DoubleAnimation()
                //        {
                //            Duration = TimeSpan.FromSeconds(AtrualHeight / 20.0),
                //            From = AtrualHeight,
                //            To = 0,
                //            EasingFunction = new PowerEase()
                //        };
                //        ((Panel[FromIndex - i] as ChangePageContentButton)
                //            .Template.FindName
                //            ("SideRect",
                //            (Panel[FromIndex - i] as ChangePageContentButton)) as Rectangle)
                //            .BeginAnimation(HeightProperty, newani
                //            );

                //        ((Panel[FromIndex - i] as ChangePageContentButton)
                //            .Template.FindName
                //            ("SideRect",
                //            (Panel[FromIndex - i] as ChangePageContentButton)) as Rectangle)
                //            .Opacity = 0;
                //        //MessageBox.Show($"{(FromIndex - i).ToString()},{((Panel[FromIndex - i] as ChangePageContentButton).ActualHeight).ToString()}");
                //    }
                //    else if (FromIndex < ToIndex)
                //    {
                //        var AtrualHeight = (Panel[FromIndex + i] as ChangePageContentButton).ActualHeight;
                //        ((Panel[FromIndex + i] as ChangePageContentButton)
                //            .Template.FindName
                //            ("SideRect",
                //            (Panel[FromIndex + i] as ChangePageContentButton)) as Rectangle)
                //            .VerticalAlignment = VerticalAlignment.Bottom;
                //        //TODO Animation
                //        ((Panel[FromIndex + i] as ChangePageContentButton)
                //            .Template.FindName
                //            ("SideRect",
                //            (Panel[FromIndex + i] as ChangePageContentButton)) as Rectangle)
                //            .Height = AtrualHeight;

                //        ((Panel[FromIndex + i] as ChangePageContentButton)
                //            .Template.FindName
                //            ("SideRect",
                //            (Panel[FromIndex + i] as ChangePageContentButton)) as Rectangle)
                //            .Opacity = 0;
                //        //MessageBox.Show($"{(FromIndex + i).ToString()},{((Panel[FromIndex + i] as ChangePageContentButton).ActualHeight).ToString()}");
                //    }
                //}
                #endregion
                        ((Panel[FromIndex] as ChangePageContentButton)
                            .Template.FindName
                            ("SideRect",
                            (Panel[FromIndex] as ChangePageContentButton)) as Rectangle)
                                .BeginAnimation(OpacityProperty, new DoubleAnimation()
                                {
                                    From = 1,
                                    To = 0,
                                    Duration = TimeSpan.FromSeconds(0.2),
                                    EasingFunction = new PowerEase()
                                });

            }
            (Template.FindName("SideRect", this) as Rectangle)
                .BeginAnimation(OpacityProperty, new DoubleAnimation()
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.2),
                    EasingFunction = new PowerEase()
                });
            Panel = null;
        }

        public Brush SideBarBrush
        {
            get { return (Brush)GetValue(SideBarBrushProperty); }
            set { SetValue(SideBarBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SideBarBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SideBarBrushProperty =
            DependencyProperty.Register("SideBarBrush", typeof(Brush), typeof(ChangePageContentButton), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0,0,0))));


    }
}
