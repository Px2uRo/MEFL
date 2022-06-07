using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.Contract
{
#if CONTRACT
    public interface IBaseAddIn
    {
        BaseInfo BaseInfo();
        Permissions Permissions();
    }
#endif
}
