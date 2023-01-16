using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MEFL.Contract.Web
{
    public class DownloadFilePool
    {
        ///<summary>最大线程数。</summary>
        public int MAX_THREAD_COUNT = 64;


        public event EventHandler OnDownloadAllCompleted;
        public event EventHandler<DownloadFileProgressArgument> OnProgressUpdated;
        public event EventHandler<DownloadFile> OnDownloadFailed;
        public event EventHandler<long> OnBytesAdd;

        private ConcurrentQueue<DownloadFile> _readyQueue;
        private ConcurrentQueue<DownloadFile> _completedQueue;
        private string _targetDir;
        private int _totalCount;

        public DownloadFilePool(IEnumerable<NativeLocalPair> pairs)
        {

            _targetDir = AppDomain.CurrentDomain.BaseDirectory;

            _readyQueue = new ConcurrentQueue<DownloadFile>();
            foreach (var item in pairs)
            {
                var newFile = new DownloadFile(item);
                newFile.Source.LocalPath = newFile.Source.LocalPath.Replace("${请Ctrl+H替换}", _targetDir);
                newFile.OnBytesAdd += NewFile_OnBytesAdd;
                newFile.OnDownloadFailed += NewFile_OnDownloadFailed;
                newFile.OnTaskCompleted += NewFile_OnTaskCompleted;
                _readyQueue.Enqueue(newFile);
            }
            _totalCount = _readyQueue.Count;
            _completedQueue = new ConcurrentQueue<DownloadFile>();
        }

        private void NewFile_OnDownloadFailed(object? sender, string e)
        {
            OnDownloadFailed?.Invoke(this, (DownloadFile)sender);
        }

        private void NewFile_OnBytesAdd(object? sender, long e)
        {
            OnBytesAdd?.Invoke(this, e);
        }

        public void StartDownload()
        {
            for (int i = 0; i < MAX_THREAD_COUNT; i++)
            {
                ContinueDownload();
            }
        }


        private void NewFile_OnTaskCompleted(object? sender, EventArgs e)
        {
            _completedQueue.Enqueue((DownloadFile)sender);
            if (((DownloadFile)sender).State == DownloadFileState.DownloadFailed)
            {
                OnDownloadFailed?.Invoke(this, (DownloadFile)sender);
            }
            ContinueDownload();
            OnProgressUpdated?.Invoke(this, new DownloadFileProgressArgument(_totalCount, _completedQueue.Count));
        }

        private void ContinueDownload()
        {
            if (_completedQueue.Count == _totalCount) OnDownloadAllCompleted?.Invoke(this, EventArgs.Empty);
            if (_readyQueue.TryDequeue(out var unreadyFile))
            {
                unreadyFile.Download();
            }
        }


    }


}
