using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using MEFL.Callers;
using MEFL.Contract;
using System;
using System.Diagnostics;
using System.Drawing;
using Size = Avalonia.Size;
using System.Drawing.Imaging;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace CLAddIn.Views
{
    public partial class ImageViewerDialog : UserControl,IDialogContent
    {
        private static ImageViewerDialog UI = new ImageViewerDialog();
        private string filePath;
        private Bitmap img;
        public ImageViewerDialog()
        {
            InitializeComponent();
        }

        public static void Show(string filePath,Bitmap img)
        {
            UI.filePath= filePath;
            UI.BigImage.Source= img;
            UI.img = img;
            DialogCaller.Show(UI);
        }

        public event EventHandler<EventArgs> Quited;

        public void WindowSizeChanged(Size size)
        {
            Scl.Height= size.Height -10;
            BigImage.Width= size.Width - 40;
        }

        private void CopyTo(object? sender, RoutedEventArgs e)
        {
            Clowd.Clipboard.ClipboardAvalonia.SetImage(img);
        }

        private void ShowInExplorer(object? sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", $"/select,\"{filePath}\"");
        }

        private void Exit(object? sender, RoutedEventArgs e)
        {
            Quited.Invoke(this, e);
        }

    }
}
