using Avalonia;
using Avalonia.Controls;

namespace MEFL.Views
{
    public partial class AddInPage : UserControl
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            var res = base.MeasureOverride(availableSize);

            PART_UNIFORM_GRID.Columns = (int)availableSize.Height / 290;
            PART_UNIFORM_GRID.Rows = (int)availableSize.Width / 80;
            return res;
        }
        internal static AddInPage UI= new AddInPage();

        public AddInPage()
        {
            InitializeComponent();
        }
    }
}
