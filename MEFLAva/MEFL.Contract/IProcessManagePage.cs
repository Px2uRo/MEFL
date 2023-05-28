using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEFL.Contract
{
    public interface IProcessManagePage
    {
        public event EventHandler Exited;
        void LauncherQuited();
    }
}
