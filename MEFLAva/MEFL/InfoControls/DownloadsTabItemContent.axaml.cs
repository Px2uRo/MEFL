using Avalonia.Controls;
using MEFL.Contract;
using System;
using System.Collections.Generic;

namespace MEFL.InfoControls
{
    public partial class DownloadsTabItemContent : UserControl
    {
        private List<LauncherWebVersionInfoList> _contents;
        private bool _showInfo = true;

        public bool ShowInfo
        {
            get { return _showInfo = true; }
            set { _showInfo = value; }
        }

        public DownloadsTabItemContent()
        {
            InitializeComponent();
        }

        public DownloadsTabItemContent(List<LauncherWebVersionInfoList> contents) :this()
        {
            _contents= contents;
            ReFresh(contents);
        }

        internal void ReFresh(List<LauncherWebVersionInfoList> contents)
        {
            StackP.Children.Clear();
            if(contents!=_contents) 
            { 
                GC.SuppressFinalize(_contents);
                _contents= null;
            }
            _contents = contents;
            foreach (var item in contents)
            {
                StackP.Children.Add(new DownloadsTabItemSubContent(item));
            }
        }
    }
}
