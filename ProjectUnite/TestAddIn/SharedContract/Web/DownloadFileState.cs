using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.Contract.Web
{
    public enum DownloadFileState
    {
        ///<summary>完全没有进入下载。</summary>
        Ready,

        ///<summary>下载中</summary>
        Downloading,

        ///<summary>超时。</summary>
        DownloadOutTime,

        ///<summary>下载失败。</summary>
        DownloadFailed,

        ///<summary>下载成功。</summary>
        DownloadSucessed,

        ///<summary>下载虽然成功，但是本地文件找不到了。可能被删掉了。</summary>
        DownloadSuceessedButLocalFileMissed,
    }

}
