using MEFL.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEFL.Callers
{
    public static class AccountCaller
    {
        public static event EventHandler<AccountBase>  RemoveAccountEvent;

        public static void Remove(AccountBase account)
        {
            RemoveAccountEvent?.Invoke(null,account);
        }
    }
    public static class DownloaderCaller
    {
        public static DownloadSource[] sources = new DownloadSource[0];
        public static string[] usingLocalFiles = new string[0];
        public static event EventHandler LoadDownloader;
        public static MEFLDownloader SelectedDownloader { get; set; }
        public static event EventHandler<DownloadSingleArgs> SingleCallerEvent;
        public static void CallSingle(string native,string local) 
        {
            SingleCallerEvent?.Invoke(null,new DownloadSingleArgs(native,local));
        }

        public static SingleProcess CallSingleProcess(string native, string local)
        {
            LoadDownloader?.Invoke(null,null);
            return SelectedDownloader.CreateProgress(native,local,sources,usingLocalFiles);
        }
    }

    public static class GamesCaller
    {
        public static event EventHandler LoadGames;
        public static GameInfoBase _selected;
        public static GameInfoBase GetSelected() {
            LoadGames?.Invoke(null,EventArgs.Empty);
            return _selected;
        }
    }

    public class DownloadSingleArgs:EventArgs
    {
        public string Native { get; private set; }
        public string Local { get; set; }
        public DownloadSingleArgs(string native, string local)
        {
            Native = native;
            Local = local;
        }
    }
}
