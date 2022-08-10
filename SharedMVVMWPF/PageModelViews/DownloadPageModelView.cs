using MEFL.Contract;
using MEFL.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MEFL.PageModelViews
{
    public class DownloadPageModelView:PageModelViewBase
    {
        public bool Refreshing
        {
            get { return DownloadPageModel.Refreshing; }
            set { DownloadPageModel.Refreshing = value; Invoke(nameof(Refreshing)); }
        }

        private string tmpFolderPath { get
            {
                if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "MEFL\\Temp")))
                {
                    Directory.CreateDirectory(Path.Combine(Path.Combine(Environment.CurrentDirectory, "MEFL\\Temp")));
                }
                return Path.Combine(Path.Combine(Environment.CurrentDirectory, "MEFL\\Temp"));
            }
        }
        private List<DownloadPageItemPair> _KnownPairs;

        public List<DownloadPageItemPair> KnownPairs
        {
            get { return _KnownPairs; }
            set { _KnownPairs = value; }
        }
        Thread RefreshThread;
        private void ChangePageContentButton_Checked(object sender, RoutedEventArgs e)
        {
            HasErrors = false;
            try
            {
                var btn = sender as ChangePageContentButton;
                foreach (var item in KnownPairs)
                {
                    if (item.Tag.ToString() == btn.Tag.ToString())
                    {
                        if (RefreshThread != null)
                        {
                            if (RefreshThread.ThreadState == ThreadState.Running)
                            {
                                RefreshThread.Interrupt();
                                RefreshThread = new Thread(() =>
                                {
                                    try
                                    {
                                        item.InvokeRefreshEvent(item, tmpFolderPath);
                                        Refreshing = false;
                                        Items = item.Items;
                                    }
                                    catch (Exception ex)
                                    {
                                        Refreshing = false;
                                        HasErrors = true;
                                        ErrorText = ex.Message;
                                    }
                                });
                                RefreshThread.Start();
                            }
                            else
                            {
                                RefreshThread = new Thread(() =>
                                {
                                    try
                                    {
                                        item.InvokeRefreshEvent(item, tmpFolderPath);
                                        Refreshing = false;
                                        Items = item.Items;
                                    }
                                    catch (Exception ex)
                                    {
                                        Refreshing = false;
                                        HasErrors = true;
                                        ErrorText = ex.Message;
                                    }
                                });
                                RefreshThread.Start();
                            }
                        }
                        else
                        {
                            RefreshThread = new Thread(() =>
                            {
                                try
                                {
                                    item.InvokeRefreshEvent(item, tmpFolderPath);
                                    Refreshing = false;
                                    Items = item.Items;
                                }
                                catch (Exception ex)
                                {
                                    Refreshing = false;
                                    HasErrors = true;
                                    ErrorText = ex.Message;
                                }
                            });
                            RefreshThread.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                    HasErrors = true;
                    ErrorText = ex.Message;
            }
        }

        private StackPanel _ChangePageContentButtons;

        public StackPanel ChangePageContentButtons
        {
            get { return _ChangePageContentButtons; }
            set { _ChangePageContentButtons = value; Invoke(nameof(ChangePageContentButtons)); }
        }

        private List<LauncherWebVersionInfo> _Items;

        public List<LauncherWebVersionInfo> Items
        {
            get { return _Items; }
            set { _Items = value; Invoke(nameof(Items)); }
        }
        private bool _hasErrors;

        public bool HasErrors
        {
            get { return _hasErrors; }
            set { _hasErrors = value; Invoke(nameof(HasErrors)); }
        }

        private string _ErrorText;

        public string ErrorText
        {
            get { return _ErrorText; }
            set { _ErrorText = value;Invoke(nameof(ErrorText)); }
        }



        public static DownloadPageModelView ModelView { get => DownloadPageModel.ModelView; set { DownloadPageModel.ModelView = value; } }
        public DownloadPageModelView()
        {
            _Items = new List<LauncherWebVersionInfo>();
            _ChangePageContentButtons =new StackPanel();
            _KnownPairs = new List<DownloadPageItemPair>();
            DownloadPageItemPair RealseTypePair = new DownloadPageItemPair("Realse", null, "RealsePage");
            RealseTypePair.RefreshEvent += RealseTypePair_RefreshEvent;
            _KnownPairs.Add(RealseTypePair);
            DownloadPageItemPair SnapsortTypePair = new DownloadPageItemPair("Snapsort", null, "SnapsortPage");
            SnapsortTypePair.RefreshEvent += SnapsortTypePair_RefreshEvent;
            _KnownPairs.Add(SnapsortTypePair);
            var e1 = new ChangePageContentButton() { Content = "正式版", Tag = "RealsePage" };
            e1.Checked += ChangePageContentButton_Checked;
            _ChangePageContentButtons.Children.Add(e1);
            var e2 = new ChangePageContentButton() { Content = "预览版", Tag = "SnapsortPage" };
            e2.Checked += ChangePageContentButton_Checked;
            _ChangePageContentButtons.Children.Add(e2);
            Invoke("ChangePageContentButtons");
            e1.IsChecked = true;
            DownloadPageModel.Refreshing= true;
            //RefreshThread = new Thread(() =>
            //{
            //    try
            //    {
            //        RealseTypePair_RefreshEvent(RealseTypePair, tmpFolderPath);
            //        Items = RealseTypePair.Items;
            //        DownloadPageModel.Refreshing = false;
            //    }
            //    catch (Exception ex)
            //    {
            //        _hasErrors = true;
            //        _ErrorText = ex.Message;
            //        DownloadPageModel.Refreshing = false;
            //    }
            //});
            //RefreshThread.Start();
        }

        private void SnapsortTypePair_RefreshEvent(object sender, string tmpFolderPath)
        {
            var itms = new List<LauncherWebVersionInfo>();
            if (!IVEGOT)
            {
                var websrm = HttpWebRequest.Create("https://piston-meta.mojang.com/mc/game/version_manifest.json").GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(websrm);
                FileStream fs;
                if (File.Exists($"{tmpFolderPath}\\RecordedVersion.json"))
                {
                    fs = new FileStream($"{tmpFolderPath}\\RecordedVersion.json", FileMode.Create);
                }
                else
                {
                    File.Delete($"{tmpFolderPath}\\RecordedVersion.json");
                    fs = new FileStream($"{tmpFolderPath}\\RecordedVersion.json", FileMode.Open);
                }
                byte[] bArr = new byte[1024];
                int size = websrm.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    fs.Write(bArr, 0, size);
                    size = websrm.Read(bArr, 0, (int)bArr.Length);
                }
                fs.Close();
                sr.Close();
                websrm.Close();
                IVEGOT = true;
            }
            jOb = FastLoadJson.Load($"{tmpFolderPath}\\RecordedVersion.json");
            foreach (var item in jOb["versions"])
            {
                if (item["type"].ToString() == "snapshot")
                {
                    itms.Add(new LauncherWebVersionInfo() { Url = item["url"].ToString(), Id = item["id"].ToString(), Type = item["type"].ToString() });
                    (sender as DownloadPageItemPair).Items = itms;
                }
            }
        }

        static bool IVEGOT = false;
        static JObject jOb;

        private void RealseTypePair_RefreshEvent(object sender, string tmpFolderPath)
        {
            var itms = new List<LauncherWebVersionInfo>();
            if (!IVEGOT)
            {
                var websrm = HttpWebRequest.Create("https://piston-meta.mojang.com/mc/game/version_manifest.json").GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(websrm);
                FileStream fs;
                if (File.Exists($"{tmpFolderPath}\\RecordedVersion.json"))
                {
                    fs = new FileStream($"{tmpFolderPath}\\RecordedVersion.json", FileMode.Create);
                }
                else
                {
                    File.Delete($"{tmpFolderPath}\\RecordedVersion.json");
                    fs = new FileStream($"{tmpFolderPath}\\RecordedVersion.json", FileMode.Open);
                }
                byte[] bArr = new byte[1024];
                int size = websrm.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    fs.Write(bArr, 0, size);
                    size = websrm.Read(bArr, 0, (int)bArr.Length);
                }
                fs.Close();
                sr.Close();
                websrm.Close();
                IVEGOT = true;
            }
            jOb = FastLoadJson.Load($"{tmpFolderPath}\\RecordedVersion.json");
            foreach (var item in jOb["versions"])
            {
                if (item["type"].ToString() == "release")
                {
                    itms.Add(new LauncherWebVersionInfo() { Url = item["url"].ToString(), Id = item["id"].ToString(), Type = item["type"].ToString() });
                    (sender as DownloadPageItemPair).Items = itms;
                }
            }
        }
    }

    public class BoolToVisiblility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(parameter == null)
            {
                parameter = string.Empty;
            }
            if (parameter.ToString() == "ContentBox")
            {
                if((!(bool)value)&& DownloadPageModel.Refreshing==false)
                {
                    return Visibility.Visible;
                }
                else if ((!(bool)value) && DownloadPageModel.Refreshing == true)
                {
                    return Visibility.Hidden;
                }
                else if (!(bool)value)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Hidden;
                }
            }
            else if (parameter.ToString() == "RefreshingBox")
            {
                if (!(bool)value)
                {
                    return Visibility.Hidden;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
            else
            {
                if (!(bool)value)
                {
                    return Visibility.Hidden;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public static class DownloadPageModel
    {
        public static bool Refreshing;
        public static DownloadPageModelView ModelView { get; set; }
        static DownloadPageModel()
        {
            ModelView = new DownloadPageModelView();
        }
    }
}
