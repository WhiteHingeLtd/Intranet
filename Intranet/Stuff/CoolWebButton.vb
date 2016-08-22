Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls


<DefaultProperty("Text"), ToolboxData("<{0}:CoolWebButton runat=server></{0}:CoolWebButton>")> _
Public Class CoolWebButton
    Inherits WebControl

    <Bindable(True), Category("Appearance"), DefaultValue(""), Localizable(True)> Property Text() As String
        Get
            Dim s As String = CStr(ViewState("Text"))
            If s Is Nothing Then
                Return String.Empty
            Else
                Return s
            End If
        End Get

        Set(ByVal Value As String)
            ViewState("Text") = Value
        End Set
    End Property

    Public Sub New()
        CssClass = "CoolButton"
        ClientIDMode = ClientIDMode.Static
    End Sub

    Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
        Dim style As String = ""
        If (Not _BPTop.Length = "0") Or (Not _BPLeft.Length = "0") Then
            Me.Style.Add("background-position", _BPLeft + "px " + _BPTop + "px")
        End If
        Dim tag As String = "<span id=""" + Me.ID + """ class=""CoolButton"">" + Text + "_Text</span>"
        writer.Write(Text)
    End Sub

    Property BP_Top() As String
        Get
            Return _BPTop
        End Get
        Set(value As String)
            _BPTop = value

        End Set
    End Property

    Dim _BPTop As String = "0"
    Dim _BPLeft As String = "0"

    Property BP_Left() As String
        Get
            Return _BPLeft
        End Get
        Set(value As String)
            _BPLeft = value
        End Set
    End Property

    Dim _OnClick As String = ""
    Property ClickAction As String
        Get
            Return _OnClick
        End Get
        Set(value As String)
            _OnClick = value
        End Set
    End Property
End Class
