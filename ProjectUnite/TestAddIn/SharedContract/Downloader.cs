using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Collections.ObjectModel;
using MEFL.Arguments;
using Avalonia.Threading;
using DynamicData;

namespace MEFL.Contract
{
    /// <summary>
    /// 申明 MEFL 下载器，作用是返回下载进程（DownloaderProgress）。下载过程在下载进程进行。
    /// </summary>
    public abstract class MEFLDownloader:MEFLClass
    {
        /// <summary>
        /// 名称
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// 描述
        /// </summary>
        public abstract string Description { get; }
        /// <summary>
        /// 版本
        /// </summary>
        public abstract Version Version { get; }
        /// <summary>
        /// 图标
        /// </summary>
        public abstract object Icon { get; }
        /// <summary>
        /// 创建单文件下载进程
        /// </summary>
        /// <param name="NativeUrl">源 Url</param>
        /// <param name="LoaclPath">本地 Url</param>
        /// <param name="usingLocalFiles">正在使用的本地文件</param>
        /// <param name="sources">下载源</param>
        /// <returns>下载进程</returns>
        public abstract SingleProcess CreateProgress(string NativeUrl, string LoaclPath, DownloadSource[] sources, string[] usingLocalFiles);
        /// <summary>
        /// 创建多文件下载进程
        /// </summary>
        /// <param name="NativeLocalPairs">源文件列表</param>
        /// <param name="sources">下载源</param>
        /// <param name="usingLocalFiles">正在使用的本地文件</param>
        /// <returns>下载进程</returns>
        public abstract SingleProcess CreateProgressFromPair(List<JsonFileInfo> NativeLocalPairs, DownloadSource[] sources,string[] usingLocalFiles);
        /// <summary>
        /// 安装游戏进程
        /// </summary>
        /// <param name="jsonSource"></param>
        /// <param name="dotMCFolder"></param>
        /// <param name="sources"></param>
        /// <param name="args">参数</param>
        /// <param name="usingLocalFiles">正在使用的本地文件</param>
        /// <returns>下载进程</returns>
        public abstract InstallProcess InstallMinecraft(string jsonSource, string dotMCFolder, DownloadSource[] sources, InstallArguments args, string[] usingLocalFiles);
    }
    public class NativeLocalPair
    {
        public event EventHandler<long> ProgressChanged;
        public event EventHandler<bool> IsOverChanged;
        public string NativeUrl;
        public string LocalPath;
        private bool _isOver;

        public bool IsOver
        {
            get { return _isOver; }
            set {
                IsOverChanged?.Invoke(this,value);
                _isOver = value; 
            }
        }

        public long Length;
        private long lastLen = 0;
        private long downloaded = 0;
        public long Downloaded { get => downloaded; set 
            { 
                downloaded= value;
                ProgressChanged?.Invoke(this,downloaded-lastLen);
                lastLen= value;
            } 
        }

        public NativeLocalPair(string nativeUrl, string localPath,long length)
        {
            NativeUrl = nativeUrl;
            LocalPath = localPath;
            Length = length;
            IsOver = false;
        }

        public override string ToString()
        {
            return NativeUrl;
        }
    }
    public class NativeLocalPairsList : ObservableCollection<NativeLocalPair> 
    {
        public event EventHandler<NativeLocalPair> OnItemAdded;

        protected override void InsertItem(int index, NativeLocalPair item)
        {
            item.IsOverChanged += Item_IsOverChanged;
            item.ProgressChanged += Item_ProgressChanged;
            OnItemAdded?.Invoke(this,item);
            base.InsertItem(index, item);
        }

        private void Item_ProgressChanged(object? sender, long e)
        {
            ProgressChanged?.Invoke(this,e);
        }

        private void Item_IsOverChanged(object? sender, bool e)
        {
            IsOverChanged?.Invoke(this,e);
        }

        public event EventHandler<bool>? IsOverChanged;
        public event EventHandler<long>? ProgressChanged;
    }
    public abstract class InstallProcess:MEFLClass,INotifyPropertyChanged
    {
        public void Finish()
        {
            Finished?.Invoke(this,EventArgs.Empty);
        }

        public void Fail()
        {
            Failed?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler<EventArgs>? Finished; 
        
        public event EventHandler<EventArgs>? Failed;
        /// <summary>
        /// 我怕别的下载进程会出现重复情况。所以每个下载进程都要说明它在使用哪些本地文件。
        /// </summary>
        /// <param name="paths">文件</param>
        /// <returns>目前我能不能知道在下载什么文件，如果不能，那我while循环，直到能添加为止</returns>
        public abstract bool GetUsingLocalFiles(out string[] paths);
        #region methods
        public abstract void Retry();
        public abstract void Pause();
        public abstract void Start();
        public abstract void Cancel();
        public abstract void Continue();
        public virtual void Close()
        {
            Dispose();
        }
        public event EventHandler<string> OnLogWriteLine;
        public void LogWriteLine(string content) 
        { 
            OnLogWriteLine?.Invoke(this, content);
        }
        public event EventHandler OnLogClear;
        public void LogClear()
        {
            OnLogClear?.Invoke(this,EventArgs.Empty);
        }
        #endregion

        private string _content;

        public string Content
        {
            get { return _content; }
            set { _content = value;
                ChangeProperty(nameof(Content)); }
        }

        private string _ErrorInfo;

        public string ErrorInfo
        {
            get { return _ErrorInfo; }
            set { _ErrorInfo = value; ChangeProperty(nameof(ErrorInfo)); }
        }

        private int _currectProgressIndex = 0;

        public int CurrectProgressIndex

        {
            get { return _currectProgressIndex; }
            set { _currectProgressIndex = value; ChangeProperty(nameof(CurrectProgressIndex)); }
        }
        private int _totalProgresses = 0;

        public int TotalProgresses
        {
            get { return _totalProgresses; }
            set { _totalProgresses = value; ChangeProperty(nameof(TotalProgresses)); }
        }


        internal void ChangeProperty(string propName)
        {
#if AVALONIA
            if (PropertyChanged != null)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    PropertyChanged.Invoke(this, new(propName));
                });
            }
#elif WPF
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new(propName));
            }
#endif
        }
        private DownloadProgressState _statu;

        public DownloadProgressState Statu
        {
            get { return _statu; }
            set { _statu = value; }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private double _CurrentProgress;

        public double CurrentProgress
        {
            get { return _CurrentProgress; }
            set { _CurrentProgress = value; ChangeProperty(nameof(CurrentProgress)); }
        }



        public InstallArguments Arguments { get; protected set; }
    }

    public abstract class SingleProcess : InstallProcess
    {
        private string _localPath;

        public string LocalPath
        {
            get { return _localPath; }
            set { _localPath = value; }
        }

        private string _nativeUrl;

        public string NativeUrl
        {
            get { return _nativeUrl; }
            set { _nativeUrl = value; }
        }

        private long _downloadedSize;

        public long DownloadedSize
        {
            get { return _downloadedSize; }
            set { _downloadedSize = value; ChangeProperty(nameof(DownloadedSize)); }
        }

        private long _totalSize;

        public long TotalSize
        {
            get { return _totalSize; }
            set { _totalSize = value; ChangeProperty(nameof(TotalSize)); }
        }
    }

    public enum DownloadProgressState
    {
        Downloading=0,
        Paused=1,
        Canceled =2,
        Finished=3,
        Failed=4,
        Stopping=5,
        Pauseing=6,
        Canceling=7,
        RetryingOrContiuning=8
    }
}