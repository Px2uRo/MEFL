using Avalonia;
using Avalonia.Controls;
using MEFL.Contract;
using System;
using System.Collections.Generic;

namespace MEFL.InfoControls
{
    public partial class DownloadsTabItemSubContent : UserControl
    {
        LauncherWebVersionInfoList _list;
        protected override Size MeasureOverride(Size availableSize)
        {
            ItemsGrid.Columns = (int)availableSize.Width / 300;
            if (_list.Count != 0)
            {
                if(ItemsGrid.Columns == 0)
                {
                    ItemsGrid.Columns = 1;
                }
                ItemsGrid.Rows = (int)Math.Ceiling((double)_list.Count / (double)ItemsGrid.Columns);
            }
            return base.MeasureOverride(availableSize);
        }
        public DownloadsTabItemSubContent()
        {
            InitializeComponent();
        }
        public DownloadsTabItemSubContent(LauncherWebVersionInfoList list)
        {
            _list = list;
            InitializeComponent();
            this.TitleTB.Text = list.Title;
            ItemsGrid.Columns = 1;
            ItemsGrid.Rows = list.Count / ItemsGrid.Columns;
            foreach (var item in list)
            {
                this.ItemsGrid.Children.Add(new DownloadItemButton(item));
            }
        }
    }
}
