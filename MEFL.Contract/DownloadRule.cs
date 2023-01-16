using MEFL.Arguments;
using System;
using System.ComponentModel;

namespace MEFL.Contract
{
    public class DownloadSource:MEFLClass
    {
        public string ELItem { get; set; }
        public string RuleSourceName { get; set; }
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