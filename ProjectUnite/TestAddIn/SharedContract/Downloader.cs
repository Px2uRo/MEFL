using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace MEFL.Contract
{
    public abstract class MEFLDownloader:MEFLClass
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract Version Version { get; }
        public abstract object Icon { get; }
        public abstract DownloadProgress CreateProgress(string NativeUrl, string LoaclPath, DownloadSource[] sources,string dotMCFolder);
        public abstract DownloadProgress CreateProgress(List<NativeLocalPair> NativeLocalPairs, DownloadSource[] sources, string dotMCFolder);
    }
    public class NativeLocalPair
    {
        public string NativeUrl;
        public string LoaclPath;

        public NativeLocalPair(string nativeUrl, string loaclPath)
        {
            NativeUrl = nativeUrl;
            LoaclPath = loaclPath;
        }
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
        #endregion
        private string _ErrorInfo;

        public string ErrorInfo
        {
            get { return _ErrorInfo; }
            set { _ErrorInfo = value; ChangeProperty(nameof(ErrorInfo)); }
        }


        public List<NativeLocalPair> NativeLocalPairs;
        private string _currectFile;

        public string CurrectFile
        {
            get { return _currectFile; }
            set { _currectFile = value; ChangeProperty(nameof(CurrectFile)); }
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

        private int _totalCount;

        public int TotalCount
        {
            get { return _totalCount; }
            set { _totalCount = value; ChangeProperty(nameof(TotalCount)); }
        }

        private int _downloadedItems;

        public int DownloadedItems
        {
            get { return _downloadedItems; }
            set { _downloadedItems = value; ChangeProperty(nameof(DownloadedItems)); }
        }
        private int _downloadedSize;

        public int DownloadedSize
        {
            get { return _downloadedSize; }
            set { _downloadedSize = value; ChangeProperty(nameof(DownloadedSize)); }
        }
        private int _totalSize;

        public int TotalSize
        {
            get { return _totalSize; }
            set { _totalSize = value; ChangeProperty(nameof(TotalSize)); }
        }

        public DownloadProgress()
        {
            State = DownloadProgressState.Canceled;
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