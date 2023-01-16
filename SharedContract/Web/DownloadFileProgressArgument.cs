using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.Contract.Web
{

    public class DownloadFileProgressArgument : EventArgs
    {
        public readonly int TotalFileCount;
        public readonly int CompletedFileCount;

        public DownloadFileProgressArgument(int totalFileCount, int completedFileCount)
        {
            TotalFileCount = totalFileCount;
            CompletedFileCount = completedFileCount;
        }

        public override string ToString()
        {
            return $"{CompletedFileCount}/{TotalFileCount}";
        }
    }

}
