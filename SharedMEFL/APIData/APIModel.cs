using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.APIData
{
    public static class APIModel
    {
        public static SettingConfig SettingConfig { get; set; }
        public static List<AddInConfig> AddInConfigs { get; set; }
        public static void RemoveTheSameItem()
        {
            for (int i = 0; i < AddInConfigs.Count; i++)
            {
                if(i != 0)
                {
                    if (AddInConfigs[i] == AddInConfigs[(i - 1)])
                    {
                        AddInConfigs.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
        static APIModel()
        {
            SettingConfig = MEFL.APIData.SettingConfig.Load();
            AddInConfigs = MEFL.APIData.AddInConfig.GetAll();
            RemoveTheSameItem();
        }
    }
}
