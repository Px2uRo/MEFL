using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MEFL.APIData;
using System.Diagnostics;

namespace MEFL
{
    public static class Debugger
    {
        private static string _LogFile { get; set; }
        public static void Logger(string Log, string InfoType)
        {
            try
            {
                if (File.Exists(_LogFile) != true)
                {
                    File.Create(_LogFile).Close();
                }
                using (StreamWriter sw = new StreamWriter(_LogFile))
                {
                    sw.WriteLine($"[{System.DateTime.Now.ToString("T")}][{InfoType}]{Log}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        public static void Logger(string Log)
        {
            Logger(Log,"MEFL");
        }
        static Debugger()
        {
            _LogFile = Path.Combine(Environment.CurrentDirectory,$"MEFL\\Log{(APIModel.SettingConfig.LogIndex+1).ToString()}.txt");
            APIModel.SettingConfig.LogIndex += 1;
            if(APIModel.SettingConfig.LogIndex > 4)
            {
                APIModel.SettingConfig.LogIndex = 0;
            }
            APIModel.SettingConfig.Update();
        }
    }
}
