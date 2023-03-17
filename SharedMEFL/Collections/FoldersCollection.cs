using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace MEFL
{
    internal class FoldersCollection:ObservableCollection<MEFLFolderInfo>
    {
        protected override void InsertItem(int index, MEFLFolderInfo item)
        {
            if (item.Path==Path.Combine(Environment.CurrentDirectory,".minecraft")) 
            {
                base.InsertItem(index, item);
            }
            else 
            {
                base.InsertItem(index, item);
                RegManager.Write("Folders",JsonConvert.SerializeObject(this));
            }
        }
    }
}
