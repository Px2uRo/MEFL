using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MEFL.Contract;
using System.Collections.Concurrent;
using MEFL.Contract.Helpers;
using System.Diagnostics;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace MEFL.Contract.Web
{

    public class DownloadFile
    {
        private CancellationTokenSource _cancelSource;

        public DownloadFileState State;

        public string ErrorInfo;

        public NativeLocalPair Source;
        public event EventHandler<long> OnBytesAdd;
        public event EventHandler<string> OnDownloadFailed;
        public event EventHandler OnTaskCompleted;

        public DownloadFile(MEFL.Contract.NativeLocalPair source)
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
                    var httpRequest = WebRequest.Create(Source.NativeUrl);
                    httpRequest.Method = "GET";
                    httpRequest.ContentType = "application/x-www-form-urlencoded";

                    httpRequest.Timeout = 30000; // 半分钟。

                    var httpResponse = httpRequest.GetResponse();


                    using (var stream = httpResponse.GetResponseStream())
                    {
                        var parentRoot = Path.GetDirectoryName(Source.LocalPath);
                        FileSystemHelper.CreateFolder(parentRoot);
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

                                        Debug.WriteLine($"{Source.NativeUrl} {fs.Length / 1024}");

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

                    Debug.WriteLine($"下载失败{Source.NativeUrl}，{ex.Message}");
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
            return $"remote uri: {Source.NativeUrl}, local uri: {Source.LocalPath}";
        }

        public T ToObject<T>()
        {
            var jsonSeri = JsonSerializer.CreateDefault();
            using(var fileStream = File.OpenText(Source.LocalPath))
            {
                using (var jsonTextReader = new JsonTextReader(fileStream))
                {
                    return jsonSeri.Deserialize<T>(jsonTextReader);
                }
               
            }
          
        }
    }

 
}
