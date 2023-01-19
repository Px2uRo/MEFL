using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MEFL.Core.Web
{
    public enum DownloadFileState
    {
        ///<summary>完全没有进入下载。</summary>
        Ready,

        ///<summary>下载中</summary>
        Downloading,

        ///<summary>超时。</summary>
        DownloadOutTime,

        ///<summary>下载失败。</summary>
        DownloadFailed,

        ///<summary>下载成功。</summary>
        DownloadSucessed,

        ///<summary>下载虽然成功，但是本地文件找不到了。可能被删掉了。</summary>
        DownloadSuceessedButLocalFileMissed,
    }

    public class DownloadFile
    {
        private CancellationTokenSource _cancelSource;

        public DownloadFileState State;

        public string ErrorInfo;

        public DownloadURI Source;
        public event EventHandler<long> OnBytesAdd;
        public event EventHandler<string> OnDownloadFailed;
        public event EventHandler OnTaskCompleted;

        public DownloadFile(DownloadURI source)
        {
            _cancelSource = new CancellationTokenSource();
            Source = source;
            State = DownloadFileState.Ready;
        }

        public virtual async void Download()
        {
            State = DownloadFileState.Downloading;
            var task = new Task(() =>
            {

                try
                {
                    var httpRequest = WebRequest.Create(Source.RemoteUri);
                    httpRequest.Method = "GET";
                    httpRequest.ContentType = "application/x-www-form-urlencoded";

                    httpRequest.Timeout = 30000; // 半分钟。

                    var httpResponse = httpRequest.GetResponse();


                    using (var stream = httpResponse.GetResponseStream())
                    {
                        var parentRoot = Path.GetDirectoryName(Source.LocalPath);
                        Helpers.FileSystemHelper.CreateFolder(parentRoot);
                        using (FileStream fs = new FileStream(Source.LocalPath, FileMode.Create))
                        {
                            var cancelToken = new CancellationTokenSource();
                            Task.Run(() =>
                            {
                                long lastLeng = 0;
                                while (cancelToken.IsCancellationRequested == false)
                                {
                                    Thread.Sleep(2500);
                                    if (cancelToken.IsCancellationRequested == false && fs.CanWrite)
                                    {

                                        Debug.WriteLine($"{Source.RemoteUri} {fs.Length / 1024}");

                                        OnBytesAdd?.Invoke(this, fs.Length - lastLeng);
                                    }
                                }
                            });
                            stream.CopyTo(fs);
                            cancelToken.Cancel();
                        }
                    }

                    OnDownloadSuccessed();
                    State = DownloadFileState.DownloadSucessed;
                    OnTaskCompleted?.Invoke(this, EventArgs.Empty);


                }
                catch (Exception ex)
                {

                    Debug.WriteLine($"下载失败{Source.RemoteUri}，{ex.Message}");
                    State = DownloadFileState.DownloadFailed;
                    ErrorInfo = ex.Message;
                    OnDownloadFailed?.Invoke(this, ErrorInfo);
                    OnTaskCompleted?.Invoke(this, EventArgs.Empty);
                }

            });
            task.Start();

        }

        protected virtual void OnDownloadSuccessed()
        {

        }

        public override string ToString()
        {
            return $"remote uri: {Source.RemoteUri}, local uri: {Source.LocalPath}";
        }

        public T ToObject<T>()
        {
            if (File.Exists(Source.LocalPath)) return default(T);

            using (var fileStream = new FileStream(Source.LocalPath, FileMode.Create))
            {
                var obj = JsonSerializer.Deserialize<T>(fileStream);
                return obj;
            }
        }
    }
}
