﻿#pragma checksum "..\..\..\..\SharedMVVMWPF\Pages\ExtensionPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "BE30EAD1C449ACD8A5639E1CE81D184A4605945D"
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
    /// ExtensionPage
    /// </summary>
    public partial class ExtensionPage : MEFL.Controls.MyPageBase, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\..\SharedMVVMWPF\Pages\ExtensionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MEFL.Controls.ChangePageContentButton DefalutChangeButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.7.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MEFL;component/pages/extensionpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\SharedMVVMWPF\Pages\ExtensionPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.7.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.DefalutChangeButton = ((MEFL.Controls.ChangePageContentButton)(target));
            
            #line 17 "..\..\..\..\SharedMVVMWPF\Pages\ExtensionPage.xaml"
            this.DefalutChangeButton.Checked += new System.Windows.RoutedEventHandler(this.ChangePageContentButton_Checked);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 18 "..\..\..\..\SharedMVVMWPF\Pages\ExtensionPage.xaml"
            ((MEFL.Controls.ChangePageContentButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.ChangePageContentButton_Checked);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

