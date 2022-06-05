using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MEFL
{
    public class BaseInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Guid { get; set; }
        public Version Version { get; set; }
        public object Icon { get; set; }
    }
}
