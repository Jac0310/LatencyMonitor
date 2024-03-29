﻿#pragma checksum "..\..\FailOverControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "C6BF82BB68EAEC56A8ECEECCED4DCBFC5DD71C0C624AEB2BC6F4538E53014CA1"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace LatencyMonitor {
    
    
    /// <summary>
    /// FailOverControl
    /// </summary>
    public partial class FailOverControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\FailOverControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Sender;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\FailOverControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Stop;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\FailOverControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Rate;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\FailOverControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Close;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\FailOverControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid foGrid;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\FailOverControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid laGrid;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\FailOverControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabItem fgmap;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\FailOverControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image map;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\FailOverControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image OverallSeverity;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\FailOverControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid alertsGrid;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/LatencyMonitor;component/failovercontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\FailOverControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Sender = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\FailOverControl.xaml"
            this.Sender.Click += new System.Windows.RoutedEventHandler(this.startWriting);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Stop = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\FailOverControl.xaml"
            this.Stop.Click += new System.Windows.RoutedEventHandler(this.stopWriting);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Rate = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.Close = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\FailOverControl.xaml"
            this.Close.Click += new System.Windows.RoutedEventHandler(this.onClose);
            
            #line default
            #line hidden
            return;
            case 5:
            this.foGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 6:
            this.laGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 7:
            this.fgmap = ((System.Windows.Controls.TabItem)(target));
            return;
            case 8:
            this.map = ((System.Windows.Controls.Image)(target));
            return;
            case 9:
            this.OverallSeverity = ((System.Windows.Controls.Image)(target));
            return;
            case 10:
            this.alertsGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

