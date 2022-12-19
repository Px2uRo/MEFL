using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MEFL.APIData
{
    internal class DSList: ObservableCollection<DownloadSource>
    {
        public DownloadSource Selected { get; set; }
        protected override void RemoveItem(int index)
        {
            this[index].Dispose();
            base.RemoveItem(index);
        }
    }
    internal class DownloadSourceCollection:Dictionary<string,DSList>
    {
        private int _ChangedCount;

        public int ChangedCount
        {
            get { return _ChangedCount; }
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
            if (!this.ContainsKey(item.ELItem))
            {
                this.Add(item.ELItem, new DSList());
            }
            this[item.ELItem].Add(item);
        }

        public void RemoveItem(DownloadSource item)
        {
            _ChangedCount = 0;
            this[item.ELItem].Remove(item);
            if (this[item.ELItem].Count == 0)
            {
                GC.SuppressFinalize(item.ELItem);
                this.Remove(item.ELItem);
                _ChangedCount = _ChangedCount - 1;
            }
            GC.SuppressFinalize(item);
        }
    }
}