using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MEFL.ControlModelViews
{
    public class ExtensionPageModelView:PageModelViews
    {
        public ObservableCollection<Hosting> Hostings { get; set; }

        public ExtensionPageModelView()
        {
            Hostings = Hosting.LoadAll();
        }
    }
}
