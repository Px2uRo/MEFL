using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MEFL.Contract
{
#if CONTRACT&&WPF
    public class BaseInfo
    {
        public object NameTitle { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Guid { get; set; }
        public Version Version { get; set; }
        public object Icon { get; set; }
        public Uri PulisherUri { get; set; }
        public Uri ExtensionUri { get; set; }
    }
#endif
}
