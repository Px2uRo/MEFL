using Avalonia;
using Avalonia.Controls;
using MEFL.APIData;
using MEFL.AvaControls;
using MEFL.Contract;
using MEFL.PageModelViews;
using System;
using System.ComponentModel;
using Tmds.DBus;

namespace MEFL.Views.MainPageTool
{
    public partial class SimpleDownloaderTool : UserControl,IMainPageTool
    {
        internal static SimpleDownloaderTool UI = new SimpleDownloaderTool();
        private bool _isAbleToExit = true;

        public bool IsAbleToExit 
        {
            get { return _isAbleToExit; }
            set { _isAbleToExit = value; PropertyChanging.Invoke(this,new(nameof(IsAbleToExit))); }
        }
        private string _title = "单文件下载测试";

        public string Title
        {
            get { return _title; }
            set { _title = value; PropertyChanging.Invoke(this, new(nameof(Title))); }
        }

        public SimpleDownloaderTool()
        {
            InitializeComponent();
            this.OnPositionChanged += SimpleDownloaderTool_OnPositionChanged;
            LocalTB.Text = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),"downloads");
            ExcuteBtn.Click += ExcuteBtn_Click;
        }

        private void ExcuteBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Callers.DownloaderCaller.CallSingle(NativeTB.Text,System.IO.Path.Combine(LocalTB.Text,System.IO.Path.GetFileName(NativeTB.Text)));
        }

        private void SimpleDownloaderTool_OnPositionChanged(object? sender, Point e)
        {
            APIModel.SettingConfig.SimpleDownloaderPosition = new System.Drawing.Point((int)e.X, (int)e.Y);
        }

        public void ChangePosition(Point position)
        {
            OnPositionChanged.Invoke(this,position);
        }

        public void Remove()
        {
            Removed.Invoke(this,EventArgs.Empty);
            APIModel.SettingConfig.ShowSimpleDownloaderTool= false;
            SettingPageModel.ModelView.Invoke("ShowDownloader");
        }

        public MainPageToolStyle GetStyle()
        {
            return MainPageToolStyle.AlwaysShowBorder;
        }

        public event EventHandler<EventArgs> Removed;
        public event EventHandler<EventArgs> Hidden;
        public event EventHandler<Point> OnPositionChanged;
        public event PropertyChangingEventHandler? PropertyChanging;
    }
}
