using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace MEFL.PageModelViews
{
    public class ExtensionPageModelView:PageModelViewBase
    {
        public static Hosting[] Hostings { get; set; }

        static ExtensionPageModelView()
        {
            Hostings = Hosting.LoadAll();
        }

        public ExtensionPageModelView()
        {

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
