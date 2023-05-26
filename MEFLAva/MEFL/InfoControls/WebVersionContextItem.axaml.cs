using Avalonia.Controls;
using MEFL.APIData;
using MEFL.Arguments;
using MEFL.AvaControls;
using MEFL.Contract;
using MEFL.PageModelViews;
using System;
using System.Linq;

namespace MEFL.InfoControls
{
    public partial class WebVersionContextItem : UserControl
    {
        public WebVersionContextItem()
        {
            InitializeComponent();
        }

        public WebVersionContextItem(LauncherWebVersionContext context,LauncherWebVersionInfo info) : this()
        {
            this.TexB.Text = context.Name;
            Btn.Height = 50;

            Btn.Click += new((s, e) =>
            {
                var ins = context.Install(info, APIModel.Javas.ToArray(), APIModel.MyFolders[APIModel.SelectedFolderIndex].Path,out var page,out var process);
                if (ins)
                {
                    DownloadingProgressPageModel.ModelView.DownloadingProgresses.Add(process);
                    ContentDialog.Quit();
                }
                else
                {
                    ContentDialog.Quit();
                    page.Solved += Page_Solved;
                    ContentDialog.Show(page);
                }
            });
        }

        private void Page_Solved(object? sender, InstallProcess e)
        {
                if (e != null)
                {
                    DownloadingProgressPageModel.ModelView.DownloadingProgresses.Add(e);
                }
                else
                {
                    WaringDialog.Show("��Ч�����ؽ��̣�ȥ��ϵ��������ߡ�");
                }
            ContentDialog.Quit();
        }

        public WebVersionContextItem(LauncherWebVersionInfo info):this()
        {
            TexB.Text = "ֱ������";
        }

    }
}
