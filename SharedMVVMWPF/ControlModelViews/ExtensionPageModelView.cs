using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MEFL.ControlModelViews
{
    public class ExtensionPageModelView:PageModelView
    {
        public Hosting[] Hostings { get; set; }

        public ExtensionPageModelView()
        {
            Hostings = Hosting.LoadAll();

            var Items = MEFL.APIData.APIModel.AddInConfigs;


            foreach (var item in Hostings)
            {
                for (int i=0;i< MEFL.APIData.APIModel.AddInConfigs.Count;i++)
                {
                    if (item.Guid != MEFL.APIData.APIModel.AddInConfigs[i].Guid)
                    {
                        Items.Add(new APIData.AddInConfig() { Guid = item.Guid, IsOpen = false });
                    }
                }
            }
        }
    }
}
