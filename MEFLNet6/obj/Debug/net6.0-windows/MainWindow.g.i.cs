﻿#pragma checksum "..\..\..\..\SharedMVVMWPF\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "D8576A611815F67BE026901442DE40F40FE96655"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using MEFL;
using MEFL.Controls;
using MEFL.Pages;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace MEFL {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 92 "..\..\..\..\SharedMVVMWPF\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border MinWindowButton;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\..\..\SharedMVVMWPF\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border MaxWindowIcon;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\..\..\SharedMVVMWPF\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border WinWindowIcon;
        
        #line default
        #line hidden
        
        
        #line 125 "..\..\..\..\SharedMVVMWPF\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border ChangeSizeButton;
        
        #line default
        #line hidden
        
        
        #line 133 "..\..\..\..\SharedMVVMWPF\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border CloseWindowsButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MEFL;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\SharedMVVMWPF\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 11 "..\..\..\..\SharedMVVMWPF\MainWindow.xaml"
            ((MEFL.MainWindow)(target)).StateChanged += new System.EventHandler(this.Window_StateChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 59 "..\..\..\..\SharedMVVMWPF\MainWindow.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseMove += new System.Windows.Input.MouseEventHandler(this.Grid_MouseMove);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 78 "..\..\..\..\SharedMVVMWPF\MainWindow.xaml"
            ((System.Windows.Controls.ContentPresenter)(target)).Initialized += new System.EventHandler(this.Ini);
            
            #line default
            #line hidden
            return;
            case 4:
            this.MinWindowButton = ((System.Windows.Controls.Border)(target));
            
            #line 91 "..\..\..\..\SharedMVVMWPF\MainWindow.xaml"
            this.MinWindowButton.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Border_MouseDown);
            
            #line default
            #line hidden
            
            #line 93 "..\..\..\..\SharedMVVMWPF\MainWindow.xaml"
            this.MinWindowButton.MouseEnter += new System.Windows.Input.MouseEventHandler(this.MinWindowButton_MouseEnter);
            
            #line default
            #line hidden
            
            #line 94 "..\..\..\..\SharedMVVMWPF\MainWindow.xaml"
            this.MinWindowButton.MouseLeave += new System.Windows.Input.MouseEventHandler(this.MinWindowButton_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 5:
            this.MaxWindowIcon = ((System.Windows.Controls.Border)(target));
            return;
            case 6:
            this.WinWindowIcon = ((System.Windows.Controls.Border)(target));
            return;
            case 7:
            this.ChangeSizeButton = ((System.Windows.Controls.Border)(target));
            
            #line 124 "..\..\..\..\SharedMVVMWPF\MainWindow.xaml"
            this.ChangeSizeButton.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Border_MouseDown);
            
            #line default
            #line hidden
            
            #line 126 "..\..\..\..\SharedMVVMWPF\MainWindow.xaml"
            this.ChangeSizeButton.MouseEnter += new System.Windows.Input.MouseEventHandler(this.MinWindowButton_MouseEnter);
            
            #line default
            #line hidden
            
            #line 127 "..\..\..\..\SharedMVVMWPF\MainWindow.xaml"
            this.ChangeSizeButton.MouseLeave += new System.Windows.Input.MouseEventHandler(this.MinWindowButton_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 8:
            this.CloseWindowsButton = ((System.Windows.Controls.Border)(target));
            
            #line 132 "..\..\..\..\SharedMVVMWPF\MainWindow.xaml"
            this.CloseWindowsButton.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Border_MouseDown);
            
            #line default
            #line hidden
            
            #line 134 "..\..\..\..\SharedMVVMWPF\MainWindow.xaml"
            this.CloseWindowsButton.MouseEnter += new System.Windows.Input.MouseEventHandler(this.MinWindowButton_MouseEnter);
            
            #line default
            #line hidden
            
            #line 135 "..\..\..\..\SharedMVVMWPF\MainWindow.xaml"
            this.CloseWindowsButton.MouseLeave += new System.Windows.Input.MouseEventHandler(this.MinWindowButton_MouseLeave);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

