using MEFL.CLAddIn.WebVersion;
using MEFL.Contract;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MEFL.CLAddIn
{
    internal class DownloadPagePair : DownloadPageItemPair
    {
        public DownloadPagePair() : base("正式版", new(),  "realse")
        {
            WebRefreshEvent += Pair_WebRefreshEvent;
            ListRefreshEvent += Pair_ListRefreshEvent;
        }
        string ResponString;
        JObject jOb;
        const string website = "https://launchermeta.mojang.com/mc/game/version_manifest.json";
        static WebRequest req;
        private List<LauncherWebVersionInfoList> Refresh(DownloadPageItemPair pair, string tmpFolderPath)
        {
            while (string.IsNullOrEmpty(ResponString))
            {
                if (pair.HasError)
                {
                    return new();
                }
            }
            if (pair.Tag == "realse")
            {
                if (Contents.Count <= 0)
                {
                    foreach (var item in jOb["versions"])
                    {
                        if (item["type"].ToString() == "release")
                        {
                            if (Version.TryParse(item["id"].ToString(), out var version))
                            {
                                var Tag = $"{version.Major.ToString()}.{version.Minor.ToString()}";
                                var list = Contents.Where(a => a.Title == Tag).ToList();
                                if (list.Count == 0)
                                {
                                    var nc = new LauncherWebVersionInfoList(Tag);
                                    nc.Add(new NormalWebVersion() { Id = item["id"].ToString(), Type = item["type"].ToString(), Url = item["url"].ToString(), ReleaseTime = item["releaseTime"].ToString() });
                                    Contents.Add(nc);
                                }
                                else
                                {
                                    list[0].Add(new NormalWebVersion() { Id = item["id"].ToString(), Type = item["type"].ToString(), Url = item["url"].ToString(), ReleaseTime = item["releaseTime"].ToString() });
                                }
                            }
                            else
                            {
                                Contents[0].Add(new NormalWebVersion() { Id = item["id"].ToString(), Type = item["type"].ToString(), Url = item["url"].ToString(), ReleaseTime = item["releaseTime"].ToString() });
                            }
                        }
                    }
                }
            }
            return Contents;
        }
        private void Pair_WebRefreshEvent(object sender, string tmpFolderPath)
        {
            var pair = sender as DownloadPageItemPair;
            try
            {
                req = HttpWebRequest.Create(website);
                req.Method = "GET";
                using (WebResponse wr = req.GetResponse())
                {
                    var strm1 = wr.GetResponseStream();
                    var strm2 = new StreamReader(strm1);
                    ResponString = strm2.ReadToEnd();
                    strm1.Close();
                    strm1.Dispose();
                    strm2.Close();
                    strm2.Dispose();
                    jOb = JObject.Parse(ResponString);
                }
                req.Abort();
            }
            catch (Exception ex)
            {
                pair.HasError = true;
                pair.ErrorDescription = ex.Message;
            }
        }
        private void Pair_ListRefreshEvent(object sender, string tmpFolderPath)
        {
            var pair = (sender as DownloadPageItemPair);
            pair.Contents = Refresh(pair, tmpFolderPath);
            pair.RefreshCompete();
        }
    }
}
