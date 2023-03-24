using MEFL.Arguments;
using System;
using System.ComponentModel;
using System.Security.Policy;

namespace MEFL.Contract
{
    public abstract class DownloadSource:MEFLClass
    {
        public string ELItem { get; set; }
        public string RuleSourceName { get; set; }
        public virtual string GetUri(string parama) => Uri;
        public string Uri { get; set; }
        public override string ToString()
        {
            if (RuleSourceName != null)
            {
                return RuleSourceName;
            }
            else
            {
                return base.ToString();
            }
        }
    }
}