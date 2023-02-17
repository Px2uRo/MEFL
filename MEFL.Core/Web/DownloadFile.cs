using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MEFL.Core.Web
{
    ///<summary>文件下载状态。</summary>
    public enum DownloadFileState
    {
        ///<summary>文件下载任务处于闲置状态。</summary>
        Idle,

        ///<summary>文件下载任务被取消了。</summary>
        Canceled,

        ///<summary>下载中。</summary>
        Downloading,

        ///<summary>下载失败。</summary>
        DownloadFailed,

        ///<summary>下载成功。</summary>
        DownloadSucessed,

    }

    public struct ContentRange
    {
        public long Offset;
        public long Length;
    }

    ///<summary>文件下载模块，一个实例对应一个下载任务。</summary>
    [Serializable]
    public class DownloadFile
    {
        #region events

        public event EventHandler<long> OnBytesAdd;
        public event EventHandler<string> OnDownloadFailed;
        public event EventHandler OnTaskCompleted;

        #endregion

        #region fields

        [NonSerialized]
        private CancellationTokenSource _cancelSource;

        ///<summary>文件当前下载的长度位置，如果要实现恢复下载，可以从此处进行设置。是临时数据。</summary>
        private long _tempDownloadingLengthForPausedLength;

        ///<summary>下载缓存。</summary>
        private byte[] _buffer;

        #endregion

        #region props

        ///<summary>下载状态。</summary>
        public DownloadFileState State
        {
            get; private set;
        }

        ///<summary>错误信息。</summary>
        public string ErrorInfo
        {
            get; private set;
        }

        ///<summary>文件下载和本地存放的来源。</summary>
        public DownloadURI Source
        {
            get; private set;
        }

        ///<summary>文件长度。</summary>
        public long ContentLength
        {
            get; private set;
        }

        ///<summary>下载计数器。</summary>
        public int DownloadCounter
        {
            get; private set;
        }

        ///<summary>文件长度是否正确。</summary>
        [JsonIgnore]
        public bool IsFileLengthCorrect => ContentLength == _tempDownloadingLengthForPausedLength;

        #endregion

        #region ctors

        public DownloadFile(DownloadURI source)
        {
            _cancelSource = new CancellationTokenSource();
            _tempDownloadingLengthForPausedLength = 0;
            Source = source;
            State = DownloadFileState.Idle;
            DownloadCounter = 0;
        }
        #endregion

        #region methods

        public async Task Download(bool isContinue = false)
        {
            try
            {
                var httpRequest = WebRequest.Create(Source.RemoteUri);
                httpRequest.Method = "GET";
                httpRequest.ContentType = "application/x-www-form-urlencoded";

                httpRequest.Timeout = 30000; // 半分钟。
                var httpResponse = httpRequest.GetResponse();

                ContentLength = httpResponse.ContentLength;
                
                if(ContentLength > 1024 * 1024)
                {
                    await DownloadCombineParts(ContentLength);
                }
                else
                {
                    await DownloadSingle(false);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        ///<summary>下载切片。</summary>
        public async Task DownloadCombineParts(long contentLength)
        {
            await DownloadCombineParts(contentLength, 1024 * 1024); // 1MB区域
        }

        ///<summary>下载多项。</summary>
        private async Task DownloadCombineParts(long contentLength, long partSize)
        {

            var task = new Task(() =>
            {
                State = DownloadFileState.Downloading;
                var buffer = new byte[contentLength];
                var partCount = contentLength / partSize + contentLength % partSize == 0 ? 0 : 1; // 是否为整除，如果不是整除则补上余数段。
                var hasRest = contentLength % partSize != 0;

                var parts = new byte[partSize];
                var ranges = new ContentRange[partCount];
                for (int i = 0; i < partSize; i++) { ranges[i] = new ContentRange() { Offset = i * partSize, Length = partSize }; }
                if (hasRest) ranges[partSize - 1].Length = contentLength - partCount * partSize + 1;

                var po = new ParallelOptions();
                po.CancellationToken = _cancelSource.Token;
                try
                {
                    Parallel.ForEach(ranges, po, r => DownloadToBuffer(buffer, r.Offset, r.Length));
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

                File.WriteAllBytes(Source.LocalPath, buffer);
            });
            task.Start();
            await task;

        }

        ///<summary>下载到缓存。</summary>
        private void DownloadToBuffer(byte[] buffer, long rangeStart, long rangeLen)
        {
            HttpHelper.GetContentByPart(buffer, Source.RemoteUri, rangeStart, rangeStart + rangeLen - 1);
        }

        ///<summary>下载任务。</summary>
        public Task DownloadSingle(bool isContinue = false)
        {
            State = DownloadFileState.Downloading;
            DownloadCounter++;
            var task = new Task(async () =>
            {
                try
                {
                    var httpRequest = WebRequest.Create(Source.RemoteUri);
                    httpRequest.Method = "GET";
                    httpRequest.ContentType = "application/x-www-form-urlencoded";

                    // 设置断点续传的信息。
                    if (isContinue)
                    {
                        httpRequest.Headers.Add("Range", $"{_tempDownloadingLengthForPausedLength}-");
                    }

                    httpRequest.Timeout = 30000; // 半分钟。
                    var httpResponse = httpRequest.GetResponse();

                    ContentLength = httpResponse.ContentLength;

                    using (var stream = httpResponse.GetResponseStream())
                    {
                        var parentRoot = Path.GetDirectoryName(Source.LocalPath);
                        Helpers.FileHelper.CreateFolder(parentRoot);
                        if (isContinue)
                        {
                            using (FileStream fs = new FileStream(Source.LocalPath, FileMode.Append))
                            {
                                fs.Seek(_tempDownloadingLengthForPausedLength, SeekOrigin.Begin);
                                await stream.CopyToAsync(fs, _cancelSource.Token);
                                _tempDownloadingLengthForPausedLength = fs.Length;
                            }
                        }
                        else
                        {
                            using (FileStream fs = new FileStream(Source.LocalPath, FileMode.Create))
                            {
                                await stream.CopyToAsync(fs, _cancelSource.Token);
                                _tempDownloadingLengthForPausedLength = fs.Length;
                            }
                        }

                    }

                    State = DownloadFileState.DownloadSucessed;
                    OnTaskCompleted?.Invoke(this, EventArgs.Empty);


                }
                catch (TaskCanceledException cancelex)
                {
                    Debug.WriteLine($"下载取消");
                    State = DownloadFileState.Canceled;
                    OnDownloadFailed?.Invoke(this, ErrorInfo);
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


            return task;
        }

        public void WaitDownload()
        {
            while (State == DownloadFileState.Downloading)
            {
                Thread.Sleep(250);
            }
        }

        ///<summary>将文本内容进行序列化。</summary>
        public T ToObject<T>()
        {
            if (File.Exists(Source.LocalPath) == false)
                return default(T);

            using var fileStream = new FileStream(Source.LocalPath, FileMode.Open);
            var obj = JsonSerializer.Deserialize<T>(fileStream);
            return obj;
        }

        ///<summary>取消任务。会保存上一次的下载数据信息。</summary>
        public void Cancel()
        {
            _cancelSource.Cancel();
            State = DownloadFileState.Canceled;
        }

        ///<summary>保存文件状态。</summary>
        public void SaveTempCache(string cacheDirPath)
        {
            // 本地文件会保存成这种样子。  
            var guid = Source.LocalPath;
            guid = guid.Replace(':', '_');
            guid = guid.Replace('/', '_');
            guid = guid.Replace('\\', '_');
            guid = guid.Replace(' ', '_');
            guid = guid.Replace('.', '_');
            guid = guid.ToUpper();
            guid = guid + ".cache";

            var path = Path.Combine(cacheDirPath, guid);
            var file = new FileInfo(path);
            using var stream = file.OpenWrite();
            JsonSerializer.Serialize(stream, this);
        }

        ///<summary>载入当前文件的长度。</summary>
        private long LoadCurrentFileLength()
        {
            var file = new FileInfo(Source.LocalPath);
            var len = file.Length;
            _tempDownloadingLengthForPausedLength = len;
            return file.Length;
        }

        #endregion

        #region overrides
        public override string ToString()
        {
            return $"[DownloadFile] remote uri: {Source.RemoteUri}, local uri: {Source.LocalPath}, content len: {ContentLength}";
        }
        #endregion

        #region static

        ///<summary>读取下载信息。</summary>
        public static DownloadFile LoadTempCache(string path)
        {
            DownloadFile downloadFile = null;
            using (var stream = File.OpenRead(path))
            {
                downloadFile = JsonSerializer.Deserialize<DownloadFile>(stream);
            }
            downloadFile.LoadCurrentFileLength();
            return downloadFile;
        }

        ///<summary>读取缓存文件夹的所有数据。</summary>
        public static DownloadFile[] LoadTempCaches(string path, out string[] invalidPaths)
        {
            var validList = new List<DownloadFile>();
            var invalidList = new List<string>();
            foreach (var file in Directory.GetFiles(path, "*.cache"))
            {
                var downloadFile = LoadTempCache(file);
                if (downloadFile != null)
                    validList.Add(downloadFile);
                else
                    invalidList.Add(file);
            }
            invalidPaths = invalidList.ToArray();
            return validList.ToArray();
        }

#if DEBUG
        public static DownloadFile LoadTest()
        {
            var targetPath = Path.Combine(Environment.CurrentDirectory, "client.jar");
            return new DownloadFile(new DownloadURI("https://piston-data.mojang.com/v1/objects/977727ec9ab8b4631e5c12839f064092f17663f8/client.jar", targetPath));
        }
#endif


        #endregion

    }
}
