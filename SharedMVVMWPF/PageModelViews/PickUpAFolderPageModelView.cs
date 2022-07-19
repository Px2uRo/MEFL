using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace MEFL.PageModelViews {

public class PickUpAFolderPageModelView : PageModelViewBase
{

    public string Curret
    {
        get { return PickUpAFolderPageModel.Curret; }
        set
        {
            if (value == "根目录")
            {
                //TODO i18N
                PickUpAFolderPageModel.Curret = "根目录";
                    Items = new List<DirectoryInfo>();
                foreach (var item in Drives)
                {
                    Items.Add(new DirectoryInfo(item));
                }
                Invoke("Drives");
            }
            else
            {
                    PickUpAFolderPageModel.Curret = value;
                    Items = new List<DirectoryInfo>();
                    foreach (var item in Directory.GetDirectories(value))
                    {
                        Items.Add(new DirectoryInfo(item));
                    }
                    
            }
                Invoke("Items");
                Invoke("Curret");
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

}

public static class PickUpAFolderPageModel
{
    public static List<DirectoryInfo> Items { get; set; }
    public static string[] Drives { get => Environment.GetLogicalDrives(); }
    public static string Curret { get; set; }
    static PickUpAFolderPageModel()
    {
        //TODO 加载注册表
        Items = new List<DirectoryInfo>();
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
}
