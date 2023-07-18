using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using MEFL.APIData;
using MEFL.Contract;
using MEFL.PageModelViews;
using System;
using System.Threading;

namespace MEFL.Views.DialogContents
{
    public partial class LaunchGameDialog : UserControl,IDialogContent
    {
        public static LaunchGameDialog UI =new LaunchGameDialog();
        static ProcessModelView PMV;

        public event EventHandler<EventArgs> Quited;

        public LaunchGameDialog()
        {
            InitializeComponent();
            PMV = new ProcessModelView();
            PMV.PropertyChanged += PMV_PropertyChanged;
            this.DataContext = PMV;
        }

        private void PMV_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var pmv = sender as ProcessModelView;
            Dispatcher.UIThread.InvokeAsync(new Action(() =>
            {
                if (e.PropertyName == nameof(pmv.Progress))
                {
                    PB.Value = pmv.Progress;
                    ValueString.Text = pmv.Progress.ToString() + "%";
                }
                else if (e.PropertyName == nameof(pmv.Failed))
                {
                    if (pmv.Failed)
                    {
                        PB.IsVisible = false;
                        HintTB.Text = pmv.ErrorInfo;
                        HintTB.IsVisible = true;
                        FailedHint.IsVisible= true;
                        LaunchingHint.IsVisible= false;
                        CancelBtn.Click -= CancelBtn_Back;
                        CancelBtn.Content = "Ã÷°×";
                        CancelBtn.Click += CancelBtn_Cancel;
                    }
                    else
                    {
                        PB.Value = 0;
                        PB.IsVisible = true;
                        HintTB.IsVisible = false;
                        FailedHint.IsVisible = false;
                        LaunchingHint.IsVisible = true;
                        CancelBtn.Click -= CancelBtn_Cancel;
                        CancelBtn.Content = "È¡Ïû";
                        CancelBtn.Click += CancelBtn_Back;
                    }
                }
                else if (e.PropertyName == nameof(pmv.Succeed))
                {
                    if (pmv.Succeed)
                    {
                        var page = pmv.Game.GetManageProcessPage(pmv.Process, APIModel.SettingArgs);
                        ManageProcessesPageModel.ModelView.RunningGames.AddItem(pmv.Process,page,pmv.Game);
                        pmv.Process = null;
                        pmv.Game = null;
                        Quited?.Invoke(this, EventArgs.Empty);
                    }
                }
                else if(e.PropertyName == nameof(pmv.Statu))
                {
                    DetailTB.Text = pmv.Statu;
                }
            }));
        }

        private void CancelBtn_Back(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Quited?.Invoke(this, EventArgs.Empty);
        }

        private void CancelBtn_Cancel(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Quited?.Invoke(this, EventArgs.Empty);
        }

        public void ReLoad()
        {
            PMV.Game = APIModel.CurretGame;
            PMV.BuildProcess();
        }

        public void WindowSizeChanged(Size size)
        {

        }
    }
}
