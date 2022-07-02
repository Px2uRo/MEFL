using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace MEFL.PageModelViews
{
    public class ExtensionPageModelView:PageModelViewBase
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
