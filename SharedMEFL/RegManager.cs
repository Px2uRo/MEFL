using MEFL.RegManagers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL
{
    public static class RegManager
    {
        private static BaseRegManager _instance;

        static RegManager()
        {

#if WINDOWS
            _instance = new WindowsRegManager();
#else
            throw new NotImplementedException();  
#endif

        }

        public static void Write(string key, string value)
        {
            _instance.Write(key, value, false);
        }

        public static void Write(string key, string value, bool forceWrite)
        {
            _instance.Write(key, value, forceWrite);
        }

        public static string Read(string key)
        {
            return _instance.Read(key); 
        }

    }
}
