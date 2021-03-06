﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace My
    
    <Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.8.0.0"),  _
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Partial Friend NotInheritable Class MySettings
        Inherits Global.System.Configuration.ApplicationSettingsBase
        
        Private Shared defaultInstance As MySettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New MySettings()),MySettings)
        
#Region "My.Settings Auto-Save Functionality"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(sender As Global.System.Object, e As Global.System.EventArgs)
        If My.Application.SaveMySettingsOnExit Then
            My.Settings.Save()
        End If
    End Sub
#End If
#End Region
        
        Public Shared ReadOnly Property [Default]() As MySettings
            Get
                
#If _MyType = "WindowsForms" Then
               If Not addedHandler Then
                    SyncLock addedHandlerLockObject
                        If Not addedHandler Then
                            AddHandler My.Application.Shutdown, AddressOf AutoSaveSettings
                            addedHandler = True
                        End If
                    End SyncLock
                End If
#End If
                Return defaultInstance
            End Get
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("AI01")>  _
        Public Property sqlServer() As String
            Get
                Return CType(Me("sqlServer"),String)
            End Get
            Set
                Me("sqlServer") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("ROBOTICS_OPTICS_DEV")>  _
        Public Property sqlDBName() As String
            Get
                Return CType(Me("sqlDBName"),String)
            End Get
            Set
                Me("sqlDBName") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property sqlUsername() As String
            Get
                Return CType(Me("sqlUsername"),String)
            End Get
            Set
                Me("sqlUsername") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property sqlPassword() As String
            Get
                Return CType(Me("sqlPassword"),String)
            End Get
            Set
                Me("sqlPassword") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("192.168.1.126")>  _
        Public Property XFP_PrinterAddr() As String
            Get
                Return CType(Me("XFP_PrinterAddr"),String)
            End Get
            Set
                Me("XFP_PrinterAddr") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("192.168.1.127")>  _
        Public Property SFP_PrinterAddr() As String
            Get
                Return CType(Me("SFP_PrinterAddr"),String)
            End Get
            Set
                Me("SFP_PrinterAddr") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("192.168.1.128")>  _
        Public Property Clamshell_PrinterAddr() As String
            Get
                Return CType(Me("Clamshell_PrinterAddr"),String)
            End Get
            Set
                Me("Clamshell_PrinterAddr") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("192.168.1.129")>  _
        Public Property ShippingA_PrinterAddr() As String
            Get
                Return CType(Me("ShippingA_PrinterAddr"),String)
            End Get
            Set
                Me("ShippingA_PrinterAddr") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("192.168.1.238")>  _
        Public Property ShippingM_PrinterAddr() As String
            Get
                Return CType(Me("ShippingM_PrinterAddr"),String)
            End Get
            Set
                Me("ShippingM_PrinterAddr") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property XFP_Horizontal() As Integer
            Get
                Return CType(Me("XFP_Horizontal"),Integer)
            End Get
            Set
                Me("XFP_Horizontal") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property XFP_Vertical() As Integer
            Get
                Return CType(Me("XFP_Vertical"),Integer)
            End Get
            Set
                Me("XFP_Vertical") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property XFP_Size() As Integer
            Get
                Return CType(Me("XFP_Size"),Integer)
            End Get
            Set
                Me("XFP_Size") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property XFP_Rotation() As Integer
            Get
                Return CType(Me("XFP_Rotation"),Integer)
            End Get
            Set
                Me("XFP_Rotation") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property SFP_Horizontal() As Decimal
            Get
                Return CType(Me("SFP_Horizontal"),Decimal)
            End Get
            Set
                Me("SFP_Horizontal") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property SFP_Vertical() As Decimal
            Get
                Return CType(Me("SFP_Vertical"),Decimal)
            End Get
            Set
                Me("SFP_Vertical") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property SFP_Size() As Decimal
            Get
                Return CType(Me("SFP_Size"),Decimal)
            End Get
            Set
                Me("SFP_Size") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property SFP_Rotation() As Integer
            Get
                Return CType(Me("SFP_Rotation"),Integer)
            End Get
            Set
                Me("SFP_Rotation") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property Clamshell_Vertical() As Integer
            Get
                Return CType(Me("Clamshell_Vertical"),Integer)
            End Get
            Set
                Me("Clamshell_Vertical") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property Clamshell_Horizontal() As Integer
            Get
                Return CType(Me("Clamshell_Horizontal"),Integer)
            End Get
            Set
                Me("Clamshell_Horizontal") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property Clamshell_Size() As Integer
            Get
                Return CType(Me("Clamshell_Size"),Integer)
            End Get
            Set
                Me("Clamshell_Size") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property Clamshell_Rotation() As Integer
            Get
                Return CType(Me("Clamshell_Rotation"),Integer)
            End Get
            Set
                Me("Clamshell_Rotation") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property ShippingA_Vertical() As Integer
            Get
                Return CType(Me("ShippingA_Vertical"),Integer)
            End Get
            Set
                Me("ShippingA_Vertical") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property ShippingA_Horizontal() As Integer
            Get
                Return CType(Me("ShippingA_Horizontal"),Integer)
            End Get
            Set
                Me("ShippingA_Horizontal") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property ShippingA_Size() As Integer
            Get
                Return CType(Me("ShippingA_Size"),Integer)
            End Get
            Set
                Me("ShippingA_Size") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property ShippingA_Rotation() As Integer
            Get
                Return CType(Me("ShippingA_Rotation"),Integer)
            End Get
            Set
                Me("ShippingA_Rotation") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property ShippingM_Vertical() As Integer
            Get
                Return CType(Me("ShippingM_Vertical"),Integer)
            End Get
            Set
                Me("ShippingM_Vertical") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property ShippingM_Horizontal() As Integer
            Get
                Return CType(Me("ShippingM_Horizontal"),Integer)
            End Get
            Set
                Me("ShippingM_Horizontal") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property ShippingM_Size() As Integer
            Get
                Return CType(Me("ShippingM_Size"),Integer)
            End Get
            Set
                Me("ShippingM_Size") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property ShippingM_Rotation() As Integer
            Get
                Return CType(Me("ShippingM_Rotation"),Integer)
            End Get
            Set
                Me("ShippingM_Rotation") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property ConnStr() As String
            Get
                Return CType(Me("ConnStr"),String)
            End Get
            Set
                Me("ConnStr") = value
            End Set
        End Property
    End Class
End Namespace

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.PrintLabel.My.MySettings
            Get
                Return Global.PrintLabel.My.MySettings.Default
            End Get
        End Property
    End Module
End Namespace
