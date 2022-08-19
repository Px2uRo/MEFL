
using MEFL.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace MEFL.PageModelViews 
{

    public class RenameAFolderModelView : PageModelViewBase
    {
        public string SelectedPath
        {
            get { return RenameAFolderModel.SelectedPath; }
            set { RenameAFolderModel.SelectedPath = value; Invoke("SelectedPath"); }
        }

        public string Name
        {
            get { return RenameAFolderModel.Name; }
            set { RenameAFolderModel.Name = value; }
        }



        public ICommand ApplyCommand
        {
            get { return RenameAFolderModel.ApplyCommand; }
            set { RenameAFolderModel.ApplyCommand = value; }
        }



        public RenameAFolderModelView()
        {
            RenameAFolderModel.SelectedPath = null;
        }
    }

    public static class RenameAFolderModel
    {
        public static string SelectedPath { get; set; }
        public static string Name { get; set; }
        public static ICommand ApplyCommand { get;set; }

        static RenameAFolderModel()
        {
            ApplyCommand = new ApplyCommand();
        }
    }

    public class ApplyCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            MEFL.APIData.APIModel.MyFolders.Add(new MEFLFolderInfo(RenameAFolderModel.SelectedPath, RenameAFolderModel.Name));
            (App.Current.Resources["RMPMV"] as RealMainPageModelView).Invoke("MyFolders");
            (App.Current.Resources["RMPMV"] as RealMainPageModelView).SelectedFolderIndex = (App.Current.Resources["RMPMV"] as RealMainPageModelView).MyFolders.Count - 1;
            MyPageBase From = null;
            foreach (MyPageBase item in (App.Current.Resources["MainPage"] as Grid).Children)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    From = item;
                }
            }
            foreach (MyPageBase item in FindControl.FromTag("RealMainPage", (App.Current.Resources["MainPage"] as Grid)))
            {
                item.Show(From);
            }
            RegManager.Write("Folders", JsonConvert.SerializeObject(APIData.APIModel.MyFolders));
        }
    }
}
