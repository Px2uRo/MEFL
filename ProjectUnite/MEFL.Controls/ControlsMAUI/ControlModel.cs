using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEFL.Controls
{
    public static class ControlModel
    {
        public static double TimeMultiple { get; set; }
        static ControlModel()
        {
            TimeMultiple = 1;
        }
    }
}
