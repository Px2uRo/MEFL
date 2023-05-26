using Avalonia;
using Avalonia.Controls;
using MEFL.APIData;
using MEFL.Contract;
using MEFL.InfoControls;
using ReactiveUI;
using System;
using System.Linq;

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
        }

        public event EventHandler<EventArgs> Quited;

        public void WindowSizeChanged(Size size)
        {

        }
    }
}
