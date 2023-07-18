using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MEFL.Callers
{
    public static class Debugger
    {
        public delegate void DebuggerDele(string fileName,string str);
        public static event DebuggerDele DebugerEvent;
        public static void WriteLine(string typeName,object str) 
        {
            DebugerEvent?.Invoke(typeName,str.ToString());
        }
    }
}
