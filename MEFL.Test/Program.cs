using MEFL.Core.Helpers;
using MEFL.Core.Web;

namespace MEFL.Test;

internal class Program
{
    static void Main(string[] args)
    {

        var downloadfile = DownloadFile.LoadTest();
        downloadfile.OnTaskCompleted += Downloadfile_OnTaskCompleted;
        var startTime = DateTime.Now;
        downloadfile.Download(true);
        downloadfile.WaitDownload();
        Console.WriteLine($"耗时：{DateTime.Now - startTime}");
        //downloadfile.SaveTempCache(Environment.CurrentDirectory);

 
        Console.ReadLine();
    }

    private static void Downloadfile_OnTaskCompleted(object? sender, EventArgs e)
    {
        var file = sender as DownloadFile;
        FileHelper.StartProcessAndSelectFile(file.Source.LocalPath);
    }
}
