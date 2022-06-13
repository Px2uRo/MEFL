using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.ControlModelViews
{
    public static class MainModelViews
    {
        public static MyCardModelView MyCardModelView { get; set; }
        static MainModelViews()
        {
            MyCardModelView = new MyCardModelView();
        }
    }
}
