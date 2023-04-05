using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using MEFL.APIData;
using MEFL.PageModelViews;
using System;

namespace MEFL.Views
{
    public partial class MainWindow : Window
    {
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
    }

    
}