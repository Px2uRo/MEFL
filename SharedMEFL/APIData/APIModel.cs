using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.APIData
{
    public static class APIModel
    {
        public static SettingConfig SettingConfig { get; set; }
        public static List<AddInConfig> AddInConfigs { get; set; }
        public static void RemoveAddInsTheSameItem()
        {
            for (int i = 0; i < AddInConfigs.Count; i++)
            {
                if(i != 0)
                {
                    if (AddInConfigs[i].Guid == AddInConfigs[(i - 1)].Guid)
                    {
                        AddInConfigs.RemoveAt(i);
                        i--;
                    }
                }
            }
            AddInConfig.Update(AddInConfigs);
        }
        static APIModel()
        {
            SettingConfig = MEFL.APIData.SettingConfig.Load();
            AddInConfigs = MEFL.APIData.AddInConfig.GetAll();
            RemoveAddInsTheSameItem();
        }
    }
}
