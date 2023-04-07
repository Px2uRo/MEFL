using Avalonia.Controls;
using MEFL.APIData;
using MEFL.Contract;
using System;
using System.Linq;

namespace MEFL.Views.DialogContents
{
    public partial class AddAccountPart1 : UserControl,IDialogContent
    {
        public static AddAccountPart1 UI = new AddAccountPart1();
        public AddAccountPart1()
        {
            InitializeComponent();
        }

        public void Reload()
        {
            try
            {
                var loaded = APIModel.Hostings.Where((h) => h.Loaded&&h.IsOpen).ToArray();
                var abled = loaded.Where((h) => h.Permissions.UseAccountAPI == true).ToArray();
                foreach (var item in abled)
                {
                    foreach (var pages in item.Account.GetSingUpPage(APIModel.SettingArgs))
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public event EventHandler<EventArgs> Quited;
    }
}
