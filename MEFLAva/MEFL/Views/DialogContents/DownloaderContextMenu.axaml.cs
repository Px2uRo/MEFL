using Avalonia;
using Avalonia.Controls;
using MEFL.APIData;
using MEFL.Contract;
using MEFL.InfoControls;
using ReactiveUI;
using System;
using System.Linq;
using System.Net.Quic;

namespace MEFL.Views.DialogContents
{
    public partial class DownloaderContextMenu : UserControl,IDialogContent
    {
        static DownloaderContextMenu _ui = new();

        internal static DownloaderContextMenu GetUI(LauncherWebVersionInfo baseInfo) 
        {
            _ui.MainItem.Child = new WebVersionContextItem(baseInfo);
            try
            {
                foreach (var h in APIModel.Hostings)
                {
                    if (h.IsOpen)
                    {
                        if(h.Permissions.UseDownloadPageAPI)
                        {
                            var dmc = APIModel.MyFolders[APIModel.SelectedFolderIndex].Path;
                            foreach (var c in h.Download.GetDataCotexts(baseInfo,APIModel.Javas.ToArray(),dmc))
                            {
                                var child = new WebVersionContextItem(c,baseInfo);
                                _ui.Context.Children.Add(child);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return _ui;
        }
        public DownloaderContextMenu()
        {
            InitializeComponent();
            Quited += DownloaderContextMenu_Quited;
            Cls.Click += Cls_Click;
        }

        private void Cls_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Quited?.Invoke(this,null);
        }

        private void DownloaderContextMenu_Quited(object? sender, EventArgs e)
        {
            for (int i = 0; i < Context.Children.Count; i++)
            {
                var item = Context.Children[i] as WebVersionContextItem;
                item.Btn.Content = null;
                Context.Children.RemoveAt(i);
                i--;
            }
            Context.Children.Clear();
        }

        public event EventHandler<EventArgs> Quited;

        public void WindowSizeChanged(Size size)
        {

        }
    }
}
