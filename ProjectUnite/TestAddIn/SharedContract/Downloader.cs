using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Collections.ObjectModel;
using MEFL.Arguments;

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
        /// <param name="sources">下载源</param>
        /// <returns>下载进程</returns>
        /// <exception cref="NotImplementedException">如果你没有重写就抛出这个异常</exception>
        public virtual DownloadProgress CreateProgress(string NativeUrl, string LoaclPath, DownloadSource[] sources) 
        { 
            throw new NotImplementedException();
        }
        /// <summary>
        /// 创建多文件下载进程
        /// </summary>
        /// <param name="NativeLocalPairs">源文件列表</param>
        /// <param name="sources">下载源</param>
        /// <returns>下载进程</returns>
        public virtual DownloadProgress CreateProgress(NativeLocalPairsList NativeLocalPairs, DownloadSource[] sources) 
        { 
            throw new NotImplementedException();
        }
        /// <summary>
        /// 安装游戏进程
        /// </summary>
        /// <param name="jsonSource"></param>
        /// <param name="dotMCFolder"></param>
        /// <param name="sources"></param>
        /// <param name="args">参数</param>
        /// <returns>下载进程</returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual DownloadProgress InstallMinecraft(string jsonSource,  string dotMCFolder, DownloadSource[] sources,InstallArguments args)
        {
            throw new NotImplementedException();
        }
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
    public abstract class DownloadProgress:MEFLClass,INotifyPropertyChanged
    {
        #region methods
        public virtual void Retry()
        {
            State = DownloadProgressState.Downloading;
        }

        public virtual void Pause()
        {
            State = DownloadProgressState.Paused;
        }
        public virtual void Start()
        {
            State = DownloadProgressState.Downloading;
        }
        public virtual void Cancel()
        {
            State = DownloadProgressState.Canceled;
        }
        public virtual void Continue()
        {
            State = DownloadProgressState.Downloading;
        }
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
        public NativeLocalPairsList TaskItems;

        private string _ErrorInfo;

        public string ErrorInfo
        {
            get { return _ErrorInfo; }
            set { _ErrorInfo = value; ChangeProperty(nameof(ErrorInfo)); }
        }

        private string _currectFile;

        public string CurrectProgress

        {
            get { return _currectFile; }
            set { _currectFile = value; ChangeProperty(nameof(CurrectProgress)); }
        }


        void ChangeProperty(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new(propName));
            }
        }
        private DownloadProgressState _statu;

        public DownloadProgressState State
        {
            get { return _statu; }
            set { _statu = value; ChangeProperty(nameof(State)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private long _totalCount;
        public long TotalCount => _totalCount;

        private long _downloadedItems;
        public long DownloadedItems=> _downloadedItems;

        private long _downloadedSize;
        public long DownloadedSize => _downloadedSize;

        private long _totalSize;
        public long TotalSize => _totalSize;

        public InstallArguments Arguments { get; }

        public DownloadProgress()
        {
            TaskItems = new();
            TaskItems.OnItemAdded += TaskItems_OnItemAdded;
            TaskItems.IsOverChanged += TaskItems_IsOverChanged;
            TaskItems.ProgressChanged += TaskItems_ProgressChanged;
            State = DownloadProgressState.Canceled;
        }

        public DownloadProgress(NativeLocalPairsList items)
        {
            TaskItems = items;
            TaskItems.OnItemAdded += TaskItems_OnItemAdded;
            TaskItems.IsOverChanged += TaskItems_IsOverChanged;
            TaskItems.ProgressChanged += TaskItems_ProgressChanged;
            State = DownloadProgressState.Canceled;
        }

        protected DownloadProgress(InstallArguments args):this()
        {
            Arguments = args;
        }

        private void TaskItems_ProgressChanged(object? sender, long e)
        {
            _downloadedSize += e;
            PropertyChanged.Invoke(this, new(nameof(DownloadedSize)));
        }

        private void TaskItems_IsOverChanged(object? sender, bool e)
        {
            _downloadedItems++;
            PropertyChanged.Invoke(this, new(nameof(DownloadedItems)));
        }

        private void TaskItems_OnItemAdded(object? sender, NativeLocalPair e)
        {
            _totalSize += e.Length;
            _totalCount++;
            PropertyChanged.Invoke(this,new(nameof(TotalSize)));
            PropertyChanged.Invoke(this, new(nameof(TotalCount)));
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