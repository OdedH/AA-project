﻿#pragma checksum "..\..\AddFeedback.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0EAF4D5BEF706753D14874C1BDF342AB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
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


namespace WpfApplication1 {
    
    
    /// <summary>
    /// AddFeedback
    /// </summary>
    public partial class AddFeedback : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\AddFeedback.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox combo_document;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\AddFeedback.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox result_author;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\AddFeedback.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox combo_feedback;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\AddFeedback.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBoxItem feedback_correct;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\AddFeedback.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBoxItem feedback_incorrect;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\AddFeedback.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkbox_add_author;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\AddFeedback.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbl_correct_author;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\AddFeedback.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox combo_correct_author;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\AddFeedback.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Cancel;
        
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
            System.Uri resourceLocater = new System.Uri("/AuthorDetector;component/addfeedback.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\AddFeedback.xaml"
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
            this.combo_document = ((System.Windows.Controls.ComboBox)(target));
            
            #line 8 "..\..\AddFeedback.xaml"
            this.combo_document.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.combo_document_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.result_author = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.combo_feedback = ((System.Windows.Controls.ComboBox)(target));
            
            #line 14 "..\..\AddFeedback.xaml"
            this.combo_feedback.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ComboBoxFeedback_Changed);
            
            #line default
            #line hidden
            return;
            case 4:
            this.feedback_correct = ((System.Windows.Controls.ComboBoxItem)(target));
            return;
            case 5:
            this.feedback_incorrect = ((System.Windows.Controls.ComboBoxItem)(target));
            return;
            case 6:
            this.checkbox_add_author = ((System.Windows.Controls.CheckBox)(target));
            
            #line 19 "..\..\AddFeedback.xaml"
            this.checkbox_add_author.Checked += new System.Windows.RoutedEventHandler(this.checkbox_add_author_Checked);
            
            #line default
            #line hidden
            
            #line 20 "..\..\AddFeedback.xaml"
            this.checkbox_add_author.Unchecked += new System.Windows.RoutedEventHandler(this.checkbox_add_author_Unchecked);
            
            #line default
            #line hidden
            return;
            case 7:
            this.lbl_correct_author = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.combo_correct_author = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 9:
            
            #line 23 "..\..\AddFeedback.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btn_Save_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btn_Cancel = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\AddFeedback.xaml"
            this.btn_Cancel.Click += new System.Windows.RoutedEventHandler(this.btn_Cancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
