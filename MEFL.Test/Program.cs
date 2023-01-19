using MEFL.Core.Helpers;
using MEFL.Core.Web;

namespace MEFL.Test;

internal class Program
{
    static void Main(string[] args)
    {

        var downloadfile = DownloadFile.LoadTest();
        downloadfile.OnTaskCompleted += Downloadfile_OnTaskCompleted;
        downloadfile.Download(true);
        Thread.Sleep(1000);
        downloadfile.Cancel();

        downloadfile.SaveTempCache(Environment.CurrentDirectory);

        Console.ReadLine();
    }

    private static void Downloadfile_OnTaskCompleted(object? sender, EventArgs e)
    {
        var file = sender as DownloadFile;
        FileHelper.StartProcessAndSelectFile(file.Source.LocalPath);
    }
}
