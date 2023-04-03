using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;
#if WPF
using System.Windows.Controls;
using MEFL.Controls;
using System.Windows.Data;
#elif AVALONIA
using Avalonia.Controls;
#endif

namespace MEFL.PageModelViews 
{
#if WPF
    public class PickUpAFolderPageModelView : PageModelViewBase
    {

        public int Columns
        {
            get { return PickUpAFolderPageModel.Columns; }
            set {
                if (value < 1)
                {
                    PickUpAFolderPageModel.Columns = 1;
                }
                else
                {
                    PickUpAFolderPageModel.Columns = value;
                }
                Invoke("Columns"); }
        }
        public string Curret
        {
            get { return PickUpAFolderPageModel.Curret; }
            set
            {
                PickUpAFolderPageModel.Curret = value;
                Invoke("Curret");
                if (value == "根目录")
                {
                    //TODO i18N
                    PickUpAFolderPageModel.Curret = "根目录";
                    Items = new List<DirectoryInfo>();
                    foreach (var item in Drives)
                    {
                        Items.Add(new DirectoryInfo(item));
                    }
                    Invoke("Items");
                    ExceptionInfo = String.Empty;
                    }
                else
                {
                    if (Directory.Exists(value))
                    {
                        try
                        {
                            PickUpAFolderPageModel.Curret = value;
                            Items = new List<DirectoryInfo>();
                            foreach (var item in Directory.GetDirectories(value))
                            {
                                Items.Add(new DirectoryInfo(item));
                            }
                            Selected = new DirectoryInfo(value);
                            ExceptionInfo = String.Empty;
                        }
                        catch (Exception ex)
                        {
                            Selected = null;
                            ExceptionInfo = ex.Message;
                        }
                    }
                }
                Invoke("Items");
                Invoke("Curret");
                Invoke("ExceptionInfo");

            }
        }
        public List<DirectoryInfo> Items
    {
        get { return PickUpAFolderPageModel.Items; }
        set
        {
            PickUpAFolderPageModel.Items = value;
            Invoke("Items");
        }
    }
        public string[] Drives
        {
        get { return PickUpAFolderPageModel.Drives; }
        }
        private string _ExceptionInfo;
        public string ExceptionInfo
        {
            get { return _ExceptionInfo; }
            set { _ExceptionInfo = value;
                Invoke("ExceptionInfo");
            }
        }
        public DirectoryInfo Selected
        {
            get { return PickUpAFolderPageModel.Selected; }
            set { PickUpAFolderPageModel.Selected = value; Invoke("Selected");Invoke("SelectItemCommand"); }
        }

        public ICommand RenameFolderCommand
        {
            get { return PickUpAFolderPageModel.RenameFolderCommand; }
            set { PickUpAFolderPageModel.RenameFolderCommand = value; }
        }



        public PickUpAFolderPageModelView()
        {
            PickUpAFolderPageModel.Selected = null;
        }
    }

    public static class PickUpAFolderPageModel
    {
        public static List<DirectoryInfo> Items { get; set; }
        public static ICommand RenameFolderCommand { get; set; }
        public static string[] Drives { get => Environment.GetLogicalDrives(); }
        public static string Curret { get; set; }
        public static DirectoryInfo Selected { get; set; }
        public static int Columns { get; set; }
        static PickUpAFolderPageModel()
        {
            //TODO 加载注册表
            Items = new List<DirectoryInfo>();
            RenameFolderCommand = new RenameFolderCommand();
            Columns = 1;
        }
    }

    internal class RenameFolderCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            for (int i = 0; i < (App.Current.Resources["MainPage"] as Grid).Children.Count; i++)
            {
                if (((App.Current.Resources["MainPage"] as Grid).Children[i] as MyPageBase).Tag == "PickUP"
                )
                {
                    (App.Current.Resources["MainPage"] as Grid).Children.RemoveAt(i);
                }
            }
            (App.Current.Resources["MainPage"] as Grid).Children.Add(new SpecialPages.RenameFolderPage() { Tag = "RenameFolder", Visibility = Visibility.Hidden,SelectedPath=PickUpAFolderPageModel.Selected.FullName });
            foreach (MyPageBase item in FindControl.FromTag("RenameFolder", (App.Current.Resources["MainPage"] as Grid)))
            {
                item.Show();
            }
        }
    }

    public class AuthPathRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
        //TODO i18N
        var isValid = false;
        var errorText = "不存在的文件夹";
        if ((value as String)!="根目录"&&(value as String)!="不存在的文件夹")
        {
            if (Directory.Exists(value as String))
            {
                isValid = true;
            }
        }
        else
        {
            isValid = true;
        }
        return new ValidationResult(isValid, errorText);
        }
    }

    public class ExceptionInfoToVisbility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((value as String) == String.Empty||value==null)
            {
                return Visibility.Hidden;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NullToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
#endif
}
