using CoreLaunching.JsonTemplates;
using MEFL.Contract.Helpers;
using MEFL.Contract.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLAddInNet6UnitTest
{
    [TestClass]
    public class 下载任务清单
    {
        [TestMethod]
        public void 全部任务()
        {
            var source = @"https://piston-meta.mojang.com/v1/packages/6607feafdb2f96baad9314f207277730421a8e76/1.19.3.json";
            var targetName = Path.GetFileName(source);
            var target = Path.Combine(Environment.CurrentDirectory, targetName);
            var resultFile = DownloadTest(source, target);

            foreach (var item in resultFile.NativeLocalPairs)
            {
                Debug.WriteLine(item);
            }

            //ManifestToObject(resultFile);
        }

        [TestMethod]
        public void 反序列化测试()
        {
            var source = @"https://piston-meta.mojang.com/v1/packages/6607feafdb2f96baad9314f207277730421a8e76/1.19.3.json";
            var targetName = Path.GetFileName(source);
            var target = Path.Combine(Environment.CurrentDirectory, targetName);
            var resultFile = DownloadTest(source, target);
            var downloadFile = new DownloadFile(new MEFL.Contract.NativeLocalPair(source, target));

            //ManifestToObject(downloadFile);
        }


        public ManifestFile DownloadTest(string source, string target)
        {
            var downloadFile = new ManifestFile(new MEFL.Contract.NativeLocalPair(source, target));
            downloadFile.Download();
            while (downloadFile.State == DownloadFileState.Downloading) { }
            return downloadFile;
        }

        public void ManifestToObject(DownloadFile downloadFile)
        {
            var obj = downloadFile.ToObject<MEFL.Contract.Models.ManifestStruct.Rootobject>();
            foreach (var item in GetNewSource(obj))
            {
                Debug.WriteLine(item);
            }

            //FileSystemHelper.StartProcessAndSelectFile(downloadFile.Source.LoaclPath);
        }

        private IEnumerable<string> GetNewSource(MEFL.Contract.Models.ManifestStruct.Rootobject rootobject)
        {
            foreach (var lib in rootobject.libraries)
            {
                if (lib.rules == null) continue;
                var _isAllow = true;
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

                if (_isAllow)
                {
                    var resultPath = lib.downloads?.artifact?.path;
                    if (string.IsNullOrWhiteSpace(resultPath)) continue;
                    yield return resultPath;
                }

            }

        }


    }
}
