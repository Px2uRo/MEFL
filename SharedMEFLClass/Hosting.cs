using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using MEFL;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MEFL
{
    public class Hosting
    {
        [Import(AllowRecomposition = true)]
        public IBaseAddIn BaseAddIn;

        [Import(AllowRecomposition = true)]
        public ISettingPage SettingPage;

        public Exception ExceptionInfo;

        public ObservableCollection<Hosting> LoadAll()
        {
            ObservableCollection<Hosting> h;
            string path = System.IO.Path.Combine( 
                Environment.CurrentDirectory,"AddIns");
            if (System.IO.Directory.Exists(path)!=true)
            {
                System.IO.Directory.CreateDirectory(path);
            }
            try
            {
                foreach (var item in new DirectoryCatalog(path))
                {
                    Debug.WriteLine(item);
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionInfo = ex;
                return null;
            }
        }
    }
}
