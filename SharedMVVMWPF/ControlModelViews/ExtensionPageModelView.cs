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
                foreach (var itm in MEFL.APIData.APIModel.AddInConfigs)
                {
                    if (item.Guid != itm.Guid)
                    {
                        Items.Add(new APIData.AddInConfig() { Guid = item.Guid, IsOpen = false });
                    }
                }
            }
        }
    }
}
