using MEFL.Contract;
using MEFL.Pages;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MEFL.PageModelViews
{
    public class DownloadPageModelView : PageModelViewBase
    {
        public static DownloadPage UI { get; set; }
        public bool IsRefreshing { get => DownloadRefresher.IsRefreshing; set {DownloadRefresher.IsRefreshing = value; Invoke(nameof(IsRefreshing)); } }
        private bool _hasErrors;

        public bool HasErrors
        {
            get { return _hasErrors; }
            set { _hasErrors = value;
                Invoke(nameof(HasErrors));
            }
        }

        private string _errorDescription;
        public string ErrorDescription { get =>_errorDescription; set
            {
                _errorDescription = value;
                Invoke(nameof(ErrorDescription));
            } 
        }
        public List<LauncherWebVersionInfo> ItemSource { get; set; }
        public static DownloadPageModelView ModelView = new DownloadPageModelView();
        public DownloadPageModelView()
        {
            
        }
    }
}
