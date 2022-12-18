using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MEFL.APIData
{
    public class DownloadSourceCollection:Dictionary<string,List<DownloadSource>>
    {
        public void AddItem(DownloadSource item)
        {
            if (!this.ContainsKey(item.ELItem))
            {
                this.Add(item.ELItem, new List<DownloadSource>());
            }
            this[item.ELItem].Add(item);
        }

        public void RemoveItem(DownloadSource item)
        {
            this[item.ELItem].Remove(item);
            if (this[item.ELItem].Count == 0)
            {
                GC.SuppressFinalize(item.ELItem);
                this.Remove(item.ELItem);
            }
            GC.SuppressFinalize(item);
        }
    }
}