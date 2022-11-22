﻿using MEFL.Contract;
using MEFL.Pages;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MEFL.PageModelViews
{
    public class DownloadPageModelView : PageModelViewBase
    {
        public static DownloadPage UI { get; set; }
        public bool IsRefreshing { get => WebListRefresher.IsRefreshing; set {WebListRefresher.IsRefreshing = value; Invoke(nameof(IsRefreshing)); } }
        private bool _hasErrors;

        public bool HasErrors
        {
            get { return _hasErrors; }
            set { _hasErrors = value;
                Invoke(nameof(HasErrors));
            }
        }
        private string _errorStatu;

        public string ErrorStatu
        {
            get { return _errorStatu; }
            set { _errorStatu = value;Invoke(nameof(ErrorStatu)); }
        }

        private string _errorDescription;
        public string ErrorDescription { get =>_errorDescription; set
            {
                _errorDescription = value;
                Invoke(nameof(ErrorDescription));
            } 
        }
        public List<LauncherWebVersionInfoList> ItemSource { get; set; }
        public static DownloadPageModelView ModelView = new DownloadPageModelView();
        public DownloadPageModelView()
        {
            ItemSource = new();
            ErrorDescription = "没有要显示的项目";
        }
    }
}
