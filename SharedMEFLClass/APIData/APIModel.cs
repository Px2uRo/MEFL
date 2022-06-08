using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.APIData
{
    public static class APIModel
    {
        public static SettingConfig SettingConfig { get; set; }
        static APIModel()
        {
            SettingConfig = MEFL.APIData.SettingConfig.Load();
        }
    }
}
