using CLAddIn.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.CLAddIn
{
    public static class ModCache
    {
        public static InfoCache[] Caches;
        static ModCache()
        {
            Caches = JsonConvert.DeserializeObject<InfoCache[]>(Resources.ModCache);
        }
    }

    public class InfoCache
    {
        public string majorName { get; set; }
        public string minorName { get; set; }
        public string simplifiedName { get; set; }
        public string[] slugs { get; set; }
        public int mcModIndex { get; set; }
        public string mCBBSIndex { get; set; }
    }
}
