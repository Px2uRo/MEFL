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
        }
    }
}
