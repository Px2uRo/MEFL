using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using MEFL.Contract;
using System;

namespace MEFL.AvaControls
{
    public partial class MainPageToolWindow : UserControl
    {
        public MainPageToolWindow()
        {
            InitializeComponent();
        }
        Size a = new Size(0,0);
        protected override Size ArrangeOverride(Size finalSize)
        {
            a = base.ArrangeOverride(finalSize);
            return a;
        }
        public MainPageToolWindow(IMainPageTool tool) : this()
        {
            this.ContentBox.Content = tool;
            DataContext = tool;
            Title.PointerMoved += Title_PointerMoved;
            Title.PointerLeave += Title_PointerLeave;
            Remove.PointerPressed += Remove_PointerPressed;
        }

        private void Remove_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var tool = this.ContentBox.Content as IMainPageTool;
            tool.Remove();
        }

        private void Title_PointerLeave(object? sender, PointerEventArgs e)
        {
            if(ContentBox.Content!=null)
            {
                var c = ContentBox.Content as IMainPageTool;
                c.ChangePosition(new(GetValue(Canvas.LeftProperty), GetValue(Canvas.TopProperty)));
            }
        }

        private void Title_PointerMoved(object? sender, PointerEventArgs e)
        {
            if (e.InputModifiers == InputModifiers.LeftMouseButton)
            {
                var p = e.GetPointerPoint(this.Parent);
                var ld = p.Position.X - DesiredSize.Width / 2;
                var rd = p.Position.Y - (sender as Grid).DesiredSize.Height / 2;
                if ((ld >= 0&&rd>=0)&& (ld + a.Width) <= this.Parent.Width&& (rd + a.Height) <= this.Parent.Height)
                {
                    SetValue(Canvas.LeftProperty, ld);
                    SetValue(Canvas.TopProperty, rd);
                }
            }
        }
    }
}
