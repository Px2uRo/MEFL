﻿#pragma checksum "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B49BDACC38A7D8AF78DCB8D9CFD48D3B2B2E6C3E"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using MEFL.Controls;
using MEFL.PageModelViews;
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


namespace MEFL.Pages {
    
    
    /// <summary>
    /// SettingPage
    /// </summary>
    public partial class SettingPage : MEFL.Controls.MyPageBase, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 17 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MEFL.Controls.ChangePageContentButton DefalutChangeButton;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MEFL.Controls.MyComboBox JavaCB;
        
        #line default
        #line hidden
        
        
        #line 318 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MEFL.Controls.MyItemsCard MyC;
        
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
            System.Uri resourceLocater = new System.Uri("/MEFL;component/pages/settingpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
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
            this.DefalutChangeButton = ((MEFL.Controls.ChangePageContentButton)(target));
            
            #line 17 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
            this.DefalutChangeButton.Checked += new System.Windows.RoutedEventHandler(this.ChangePageContentButton_Checked);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 18 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
            ((MEFL.Controls.ChangePageContentButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.ChangePageContentButton_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 19 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
            ((MEFL.Controls.ChangePageContentButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.ChangePageContentButton_Checked);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 20 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
            ((MEFL.Controls.ChangePageContentButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.ChangePageContentButton_Checked);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 21 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
            ((MEFL.Controls.ChangePageContentButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.ChangePageContentButton_Checked);
            
            #line default
            #line hidden
            return;
            case 6:
            this.JavaCB = ((MEFL.Controls.MyComboBox)(target));
            return;
            case 7:
            
            #line 91 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
            ((MEFL.Controls.MyButton)(target)).Click += new System.Windows.RoutedEventHandler(this.SearchJava);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 92 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
            ((MEFL.Controls.MyButton)(target)).Click += new System.Windows.RoutedEventHandler(this.AddNewJava);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 102 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
            ((MEFL.Controls.MyButton)(target)).Click += new System.Windows.RoutedEventHandler(this.ReSetJVMArgs);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 132 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
            ((MEFL.Controls.MyButton)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenWebSite);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 149 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
            ((MEFL.Controls.MyButton)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenWebSite);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 167 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
            ((MEFL.Controls.MyButton)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenWebSite);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 180 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
            ((MEFL.Controls.MyButton)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenWebSite);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 195 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
            ((MEFL.Controls.MyButton)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenWebSite);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 211 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
            ((MEFL.Controls.MyButton)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenWebSite);
            
            #line default
            #line hidden
            return;
            case 18:
            this.MyC = ((MEFL.Controls.MyItemsCard)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 16:
            
            #line 225 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.ChangeDownloader);
            
            #line default
            #line hidden
            break;
            case 17:
            
            #line 283 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
            ((System.Windows.Controls.ComboBox)(target)).Loaded += new System.Windows.RoutedEventHandler(this.CBLoaded);
            
            #line default
            #line hidden
            
            #line 283 "..\..\..\..\..\SharedMVVMWPF\Pages\SettingPage.xaml"
            ((System.Windows.Controls.ComboBox)(target)).SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.SelectionChanged);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

