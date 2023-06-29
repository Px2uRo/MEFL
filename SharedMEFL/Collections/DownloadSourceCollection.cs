using MEFL.Callers;
using MEFL.Contract;
using MEFL.PageModelViews;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace MEFL.APIData
{
    public class DSList: ObservableCollection<DownloadSource>
    {
        private DownloadSource _selected;
        public DownloadSource Selected
        {
            get => _selected; set
            {
                _selected = value;
                OnPropertyChanged(new("Selected"));
                DownloaderCaller.Set(APIModel.DownloadSources.Selected);
                RegUpdate();
            }
        }

        private void RegUpdate()
        {
            var txt = APIModel.DownloadSources.Selected.ToJson();
            RegManager.Write("DownSources", txt);
        }

        protected override void RemoveItem(int index)
        {
            this[index].Dispose();
            base.RemoveItem(index);
        }
    }
    public class DownloadSourceCollection:Dictionary<string,DSList>
    {
        internal IEnumerable<DownloadSource> ShowAll()
        {
            var lst = new List<DownloadSource>();
            foreach (var keys in this.Values)
            {
                foreach (var vlu in keys)
                {
                    lst.Add(vlu);
                }
            }
            return lst;
        }
        private int _ChangedCount;

        public int ChangedCount
        {
            get {return _ChangedCount; }
        }

        public DownloadSource[] Selected
        {
            get
            {
                var ret = new List<DownloadSource>();
                foreach (var item in this)
                {
                    ret.Add(item.Value.Selected);
                }
                return ret.ToArray();
            }
        }

        public void AddItem(DownloadSource item)
        {
            SettingPageModel.ModelView.Invoke("DownSources");
            if (!this.ContainsKey(item.ELItem))
            {
                this.Add(item.ELItem, new DSList());
            }
            this[item.ELItem].Add(item);
        }

        public void RemoveItem(DownloadSource item)
        {
            SettingPageModel.ModelView.Invoke("DownSources");
            _ChangedCount = 0;
            this[item.ELItem].Remove(item);
            if (this[item.ELItem].Count == 0)
            {
                GC.SuppressFinalize(item.ELItem);
                this.Remove(item.ELItem);
                _ChangedCount = _ChangedCount - 1;
            }
            GC.SuppressFinalize(item);
            item.Dispose();
        }
    }
}