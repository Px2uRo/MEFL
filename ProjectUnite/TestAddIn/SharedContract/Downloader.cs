using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using MEFL.Contract.Properties;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Threading;
using MEFL.Contract.Helpers;

namespace MEFL.Contract
{

    public abstract class MEFLDownloader : MEFLClass
    {
        ///<summary>���ơ�</summary>
        public abstract string Name { get; }

        ///<summary>������</summary>
        public abstract string Description { get; }

        ///<summary>�汾��</summary>
        public abstract Version Version { get; }

        ///<summary>��Ϊ������ͼ�꣬����Icon�������κ�ֵ���򵥵�������һ���ַ���������ContentPresenter��ʾ��</summary>
        public abstract object Icon { get; }

        ///<summary>��ʼ�������̡�</summary>
        public abstract DownloadProgress CreateProgress(string NativeUrl, string LoaclPath, IEnumerable<DownloadSource> sources, string dotMCFolder);

        ///<summary>��ʼ�������̡�</summary>
        public abstract DownloadProgress CreateProgress(NativeLocalPair nativeLocalPair, IEnumerable<DownloadSource> sources, string doyMCFolder);

        ///<summary>��ʼ�������̡�</summary>
        public abstract DownloadProgress CreateProgress(IEnumerable<NativeLocalPair> NativeLocalPairs, IEnumerable<DownloadSource> sources, string dotMCFolder);
    }


    ///<summary>�ļ����嵥��</summary>
    [Obsolete("ûʲôƨ��", true)]
    public class PackageManifest
    {
        public event EventHandler<double> OnDownloadProgressChanged;
        public event EventHandler<bool> OnDownloadProgressCompleted;
        public event EventHandler<Exception> OnDownloadFailed;

        #region fields

        private CancellationTokenSource _cancelToken;//= new CancellationTokenSource();

        #endregion


        public string NativeUrl { get; private set; }
        public string LocalPath { get; private set; }

        


        ///<summary>�����嵥�������</summary>
        public IEnumerable<NativeLocalPair> Items { get; set; }

        public PackageManifest(string nativeUrl, string localPath)
        {
            NativeUrl = nativeUrl;
            LocalPath = localPath;
            if (string.IsNullOrWhiteSpace(nativeUrl) || string.IsNullOrWhiteSpace(localPath))
                throw new ArgumentNullException();
        }

        public void Download()
        {

        }

        ///<summary>�����嵥���ݣ���װ�ص�Items�����嵥����Ŀ�б��С�</summary>
        private async void DownloadCore(string nativeUri, string resultPath)
        {
            _cancelToken = new CancellationTokenSource();

            var task = new Task(() =>
            {

                try
                {
                    var httpRequest = WebRequest.Create(NativeUrl);
                    httpRequest.Method = "GET";
                    httpRequest.ContentType = "application/x-www-form-urlencoded";

                    httpRequest.Timeout = 30000; // ����ӡ�

                    var httpResponse = httpRequest.GetResponse();

                    using (var stream = httpResponse.GetResponseStream())
                    {
                        var parentRoot = Path.GetDirectoryName(LocalPath);
                        FileSystemHelper.CreateFolder(parentRoot);
                        using (FileStream fs = new FileStream(LocalPath, FileMode.Create))
                        {

                            Task.Run(() =>
                            {
                                long lastLeng = 0;
                                while (_cancelToken.IsCancellationRequested == false)
                                {
                                    Thread.Sleep(2500);
                                    if (_cancelToken.IsCancellationRequested == false && fs.CanWrite)
                                    {
#if DEBUG
                                        Debug.WriteLine($"{NativeUrl} {fs.Length / 1024}");
#endif
                                        OnDownloadProgressChanged?.Invoke(this, fs.Length - lastLeng);
                                    }
                                }
                            });
                            stream.CopyTo(fs);
                            _cancelToken.Cancel();
                        }
                    }

                    OnDownloadProgressCompleted?.Invoke(this, true);

                }
                catch (Exception ex)
                {

                    Debug.WriteLine($"����ʧ��{NativeUrl}��{ex.Message}");

                    OnDownloadFailed?.Invoke(this, ex);
                    OnDownloadProgressCompleted?.Invoke(this, false);
                }

            });
            task.Start();

        }

    }

   
    public abstract class DownloadProgress : MEFLClass, INotifyPropertyChanged
    {
        #region events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region props
        public List<NativeLocalPair> NativeLocalPairs { get; protected set; }

        private string _errorInfo;
        public string ErrorInfo { get { return _errorInfo; } set { _errorInfo = value; ChangeProperty(nameof(ErrorInfo)); } }

        private string _currectFile;
        public string CurrectFile { get { return _currectFile; } set { _currectFile = value; ChangeProperty(nameof(CurrectFile)); } }

        private DownloadProgressState _statu;
        public DownloadProgressState State { get { return _statu; } set { _statu = value; ChangeProperty(nameof(State)); } }

        private long _totalCount;
        public long TotalCount { get { return _totalCount; } set { _totalCount = value; ChangeProperty(nameof(TotalCount)); } }

        private long _downloadedItems;
        public long DownloadedItems { get { return _downloadedItems; } set { _downloadedItems = value; ChangeProperty(nameof(DownloadedItems)); } }

        private long _downloadedSize;
        public long DownloadedSize { get { return _downloadedSize; } set { _downloadedSize = value; ChangeProperty(nameof(DownloadedSize)); } }

        private long _totalSize;
        public long TotalSize { get { return _totalSize; } set { _totalSize = value; ChangeProperty(nameof(TotalSize)); } }

        #endregion

        #region ctors
        public DownloadProgress()
        {
            State = DownloadProgressState.Canceled;
        }
        #endregion

        #region methods
        private void ChangeProperty(string propName) { if (PropertyChanged != null) { PropertyChanged.Invoke(this, new(propName)); } }

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
            OnLogClear?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }

    public enum DownloadProgressState
    {
        Downloading = 0,
        Paused = 1,
        Canceled = 2,
        Finished = 3,
        Failed = 4,
        Stopping = 5,
        Pauseing = 6,
        Canceling = 7,
        RetryingOrContiuning = 8
    }
}