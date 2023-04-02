using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace MEFL.Contract
{
    public class Advanced
    {
        [Obsolete("别用，不要用，不是给你用的")]
        public static event EventHandler<NativeLocalPairsList>? AddProcessFromList;
        private static DownloadSource[] selectedSources { get; set; }
        [Obsolete("别用，不要用，不是给你用的")]
        public static void SetSelectedSources(DownloadSource[] value)
        {
            selectedSources = value;
        }
        public static DownloadSource[] GetSelectedSources() => selectedSources;

        public static void CreateDownloadProgressFromList(NativeLocalPairsList list)
        {
            AddProcessFromList.Invoke(null,list);
        }
    }


}
