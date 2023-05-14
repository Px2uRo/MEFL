using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using MEFL.APIData;
using MEFL.AvaControls;
using MEFL.Contract;
using MEFL.PageModelViews;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Tmds.DBus;

namespace MEFL.Views
{
    public partial class MainWindow : Window
    {
        protected override Size ArrangeOverride(Size finalSize)
        {
            if((Dialog.Child as ContentDialog) != null)
            {
                var cd = (Dialog.Child as ContentDialog);
                (cd.Content as IDialogContent).WindowSizeChanged(finalSize);
            }
            return base.ArrangeOverride(finalSize);
        }
        public MainWindow()
        {
            InitializeComponent();
            ShowMainPageBtn.Click += ShowMainPage_Click;
            ShowMainPage_Click(null,null);
            CloseWindow.Click += CloseWindow_Click;
            ShowAddInPageBtn.Click += ShowAddInPageBtn_Click;
            ShowSettingPageBtn.Click += ShowSettingPageBtn_Click;
            ShowDownloadPageBtn.Click += ShowDownloadPageBtn_Click;
            MaxlizeWindow.Click += MaxlizeWindow_Click;
            MinlizeWindow.Click += MinlizeWindow_Click;
            TitelBarGrid.PointerPressed += TitelBarGrid_PointerPressed;
        }

        private void TitelBarGrid_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            BeginMoveDrag(e);
        }

        private void ShowDownloadPageBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ClearPage();
            Page.Children.Add(DownloadPage.UI);
        }

        private void ShowSettingPageBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ClearPage();
            Page.Children.Add(SettingPage.UI);
        }

        private void ShowAddInPageBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ClearPage();
            Page.Children.Add(AddInPage.UI);
        }

        protected override void HandleWindowStateChanged(WindowState state)
        {
            base.HandleWindowStateChanged(state);
        }

        private void MinlizeWindow_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            this.WindowState= WindowState.Minimized;
        }

        private void CloseWindow_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            this.Close();
        }

        private void MaxlizeWindow_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if(WindowState== WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void ShowMainPage_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ClearPage();
            Page.Children.Add(RealMainPage.UI);
        }

        private void ClearPage()
        {
            Page.Children.Clear();
        }

        static Dictionary<Button, EventHandler<RoutedEventArgs>> stupid = new Dictionary<Button,EventHandler<RoutedEventArgs>>();
        public static void AddTempButton(Button button,Control Page)
        {
            if(App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var win = desktop.MainWindow as MainWindow;
                    if (!win.TempBtns.Children.Contains(button))
                    {
                        if (!stupid.ContainsKey(button))
                        {
                            var MyVoid = new EventHandler<RoutedEventArgs>((sender, e) =>
                            {
                                win.ClearPage();
                                win.Page.Children.Add(Page);
                            });
                            stupid.Add(button,MyVoid);
                        }
                        button.Click -= stupid[button];
                        button.Click += stupid[button];
                        win.TempBtns.Children.Add(button);
                    }
                    if (win.TempBtns.Width == 0)
                    {
                        Animations.WidthGo(30.0).RunAsync(win.TempBtns, null);
                        Animations.MarginGo(new(33, 3, 3, 3)).RunAsync(win.Page, null);
                    }
                    win.ClearPage();
                    win.Page.Children.Add(Page);
                });
            }
        }

        public static void RemoveTempButton(Button button)
        {
            if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                var win = desktop.MainWindow as MainWindow;
                if (win.TempBtns.Children.Contains(button))
                {
                    button.Click -= stupid[button];
                    win.TempBtns.Children.Remove(button);
                }
                    if (win.TempBtns.Children.Count==0)
                    {
                        Animations.WidthBack(0.0).RunAsync(win.TempBtns, null);
                        Animations.MarginBack(new(3)).RunAsync(win.Page, null);
                        win.ClearPage();
                        win.Page.Children.Add(RealMainPage.UI);
                        
                    }
                });
            }
        }
    }
}