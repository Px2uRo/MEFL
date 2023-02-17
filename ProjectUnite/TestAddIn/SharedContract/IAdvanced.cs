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
        private static MEFLDownloader selectedDownloader { get; set; }
        [Obsolete("别用，不要用，不是给你用的")]
        public static void SetSelectedDownloader(MEFLDownloader value)
        {
            selectedDownloader= value;
        }
        public static MEFLDownloader GetSelectedDownloader()
        {
            return selectedDownloader;
        }
    }


}
