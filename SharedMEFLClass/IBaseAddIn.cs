using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL
{
    public interface IBaseAddIn
    {
        BaseInfo BaseInfo();
        Permissions Permissions();
    }
}
