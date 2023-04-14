using Avalonia.Controls;
using MEFL.PageModelViews;

namespace MEFL.Views
{
    public partial class DownloadProcessPage : UserControl
    {
        public static DownloadProcessPage UI = new();
        public DownloadProcessPage()
        {
            InitializeComponent();
            DataContext = DownloadingProgressPageModel.ModelView;
        }


    }
}
