using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MEFL.Contract.Web
{
    ///<summary>配置清单文件。</summary>
    public class ManifestFile : DownloadFile
    {
        private string _gameRoot;

        public List<NativeLocalPair> NativeLocalPairs { get; private set; }//= new List<NativeLocalPair>();

        ///<summary>将本地的下载条目项载入空的清单内。</summary>
        public ManifestFile(IEnumerable<NativeLocalPair> localContent, string gameRoot) : base(null)
        {
            State = DownloadFileState.DownloadSuceessedButLocalFileMissed;
            NativeLocalPairs = new List<NativeLocalPair>(localContent);
            _gameRoot = gameRoot;
        }

        ///<summary>远端下载配置清单。</summary>
        public ManifestFile(NativeLocalPair remoteSource, string gameRoot) : base(remoteSource)
        {
            NativeLocalPairs = new List<NativeLocalPair>();
            _gameRoot = gameRoot;
        }

        protected override void OnDownloadSuccessed()
        {
            try
            {
                //var manifestLocalPath = Source.LocalPath;
                var obj = ToObject<MEFL.Contract.Models.ManifestStruct.Rootobject>();
                var pairs = GetNewSource(obj);
                NativeLocalPairs = new List<NativeLocalPair>();
                foreach (var pair in pairs)
                {
                    var localPath = Path.Combine(_gameRoot, pair.LocalPath);
                    NativeLocalPairs.Add(new NativeLocalPair(pair.NativeUrl, localPath));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            base.OnDownloadSuccessed();
        }

        private IEnumerable<NativeLocalPair> GetNewSource(Models.ManifestStruct.Rootobject rootobject)
        {
            var list = new List<NativeLocalPair>();
            foreach (var lib in rootobject.libraries)
            {
                var _isAllow = true;
                if (lib.rules == null)
                {
                    _isAllow = true;
                }
                else
                {
                    foreach (var rule in lib.rules)
                    {
                        var name = rule.os?.name;
                        if (name.Contains("windows", StringComparison.OrdinalIgnoreCase))
                        {
                            _isAllow = true;
                        }
                        else
                        {
                            _isAllow = false;
                        }
                    }

                }


                if (_isAllow)
                {
                    var artifact = lib.downloads?.artifact;
                    var resultPath = lib.downloads?.artifact?.path;
                    if (artifact == null) continue;
                    if (string.IsNullOrWhiteSpace(resultPath)) continue;
                    list.Add(new NativeLocalPair(artifact.url, artifact.path));
                }

            }
            return list; ;
        }

    }
}
