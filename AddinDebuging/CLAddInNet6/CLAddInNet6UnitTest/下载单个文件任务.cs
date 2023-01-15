using MEFL.Contract.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace CLAddInNet6UnitTest
{
    [TestClass]
    public class 下载单个文件任务
    {
        [TestMethod]
        public void 下载()
        {
            var source = @"https://libraries.minecraft.net/it/unimi/dsi/fastutil/8.5.9/fastutil-8.5.9.jar";
            var targetName = Path.GetFileName(source);
            var target = Path.Combine(Environment.CurrentDirectory, targetName);
            var downloadFile = new DownloadFile( new MEFL.Contract.NativeLocalPair(source, target) );

            downloadFile.Download();
            while (downloadFile.State != DownloadFileState.DownloadFailed && downloadFile.State != DownloadFileState.DownloadSucessed)
            {
               
            }
            Assert.IsTrue(downloadFile.State == DownloadFileState.DownloadSucessed, "文件下载出现异常。");
        }
    }
}