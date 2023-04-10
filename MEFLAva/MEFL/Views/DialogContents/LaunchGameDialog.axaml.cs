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
                    PB.Value = pmv.Progress * 100d;
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
                        CancelBtn.Content = "明白";
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
                        CancelBtn.Content = "取消";
                        CancelBtn.Click += CancelBtn_Back;
                    }
                }
                else if (e.PropertyName == nameof(pmv.Succeed))
                {
                    if (pmv.Succeed)
                    {
                        //pmv.Process.Start();

                        //TODO 看着写吧
                        ManageProcessesPageModel.ModelView.RunningGames.Add(pmv.Process,null);
                        pmv.Process = null;
                        Quited?.Invoke(this, EventArgs.Empty);
                    }
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
    }
}
