using System.ComponentModel;
using System;
using System.Collections.Generic;

namespace MEFL.Contract
{
    public abstract class MEFLDownloader:MEFLClass
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract Version Version { get; }
        public abstract object Icon { get; }
        public abstract DownloadProgress CreateProgress(string NativeUrl, string LoaclPath, DownloadSource[] Sources);
        public abstract DownloadProgress CreateProgress(Dictionary<string,string> NativeLocalPairs, DownloadSource[] Sources);
    }

    public abstract class DownloadProgress:MEFLClass,INotifyPropertyChanged
    {
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
        public DownloaderStatu Statu { get; set; }
        public virtual void Pause()
        {
            Statu = DownloaderStatu.Paused;
        }
        public virtual void Start()
        {
            Statu = DownloaderStatu.Downloading;
        }
        public virtual void Cancel()
        {
            Statu = DownloaderStatu.Canceled;
        }
        public virtual void Close()
        {
            Dispose();
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
            Statu = DownloaderStatu.Canceled;
        }
    }

    public enum DownloaderStatu
    {
        Downloading=0,
        Paused=1,
        Canceled =2,
        Finished=3
    }
}