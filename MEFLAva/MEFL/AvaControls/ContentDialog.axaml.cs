using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using MEFL.Contract;
using MEFL.Views;
using System;

namespace MEFL.AvaControls
{
    public partial class ContentDialog : UserControl
    {
        public ContentDialog()
        {
            InitializeComponent();
        }
        public static ContentDialog cd = new ContentDialog();
        public static void Show(IDialogContent dialogContent)
        {
            dialogContent.Quited -= DialogContent_Quited;
            dialogContent.Quited += DialogContent_Quited;
            if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mv = desktop.MainWindow as MainWindow;
                mv.Page.IsEnabled= false;
                mv.LeftButtons.IsEnabled= false;
                cd.Content = null;
                cd.Content = dialogContent;
                cd.Background = new SolidColorBrush(Colors.Black);
                mv.Dialog.Child = null;
                mv.Dialog.Child = cd;
                mv.DialogBackGround.IsVisible = true;
            }
        }

        private static void DialogContent_Quited(object? sender, EventArgs e)
        {
            Quit();
        }

        internal static void Quit()
        {
            if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mv = desktop.MainWindow as MainWindow;
                mv.Page.IsEnabled = true;
                mv.LeftButtons.IsEnabled = true;
                mv.DialogBackGround.IsVisible = false;
            }
        }
    }
}
