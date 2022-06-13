using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.Contract
{
    public interface IPages
    {
        
    }
#if WPF
    public class PageButtonPair
    {
        public object Icon { get; set; }
        public object SideBar { get; set; }
        public object Page { get; set; }
    }
#endif
}
