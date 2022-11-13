using System;

namespace MEFL.Contract
{
    public abstract class DownloadSource:MEFLClass
    {
        public abstract string RuleSource { get; }
        public abstract string ELItem{get;}
        public abstract string GetUri(Arguments.SettingArgs args);
    }
}