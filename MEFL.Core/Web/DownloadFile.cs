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
        ///<summary>闲置状态。</summary>
        Idle,

        ///<summary>被人工手动取消了。</summary>
        Canceled,

        ///<summary>被暂停了，该状态暂时无效。</summary>
        Paused,

        ///<summary>下载中。</summary>
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

    [Serializable]
    public sealed class DownloadFile
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

        #endregion

        #region props
        public DownloadFileState State
        {
            get; set;
        }
        public string ErrorInfo
        {
            get; set;
        }
        public DownloadURI Source
        {
            get; set;
        }

        public long ContentLength
        {
            get; set;
        }

        ///<summary>下载计数器。</summary>
        public int DownloadCounter
        {
            get; private set;
        }

        ///<summary>文件长度是否正确。</summary>
        [JsonIgnore]
        public bool IsFileLengthCorrect
        {
            get
            {
                return ContentLength == _tempDownloadingLengthForPausedLength;
            }
        }

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

        ///<summary>下载。</summary>
        public async void Download(bool isAsync = true, bool isContinue = false)
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

                    if (isContinue) httpRequest.Headers.Add("Range", $"{_tempDownloadingLengthForPausedLength}-");
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

            if (isAsync == false)
                task.RunSynchronously();
            else
                task.Start();

        }

        ///<summary>将文本内容进行序列化。</summary>
        public T ToObject<T>()
        {
            if (File.Exists(Source.LocalPath)) return default(T);

            using var fileStream = new FileStream(Source.LocalPath, FileMode.Create);
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
            return $"remote uri: {Source.RemoteUri}, local uri: {Source.LocalPath}";
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
                if (downloadFile != null) validList.Add(downloadFile);
                else invalidList.Add(file);
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
