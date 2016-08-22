Imports WHLClasses
Public Class Status
    Inherits System.Web.UI.Page

    Dim status As StatusMonitoring
    Dim Allgood As States = States.Up
    Dim NotGood As New List(Of String)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            status = Application("Status")
        Catch ex As Exception
            Application("Status") = New StatusMonitoring
            status = Application("Status")
        End Try



        'Externals
        CheckExternals()

        'Servers
        CheckServers()


        'Internals
        InternalFiles()

        Jumbotron()

        TimeLabel.Text = Now.ToString
    End Sub


    Public Sub NoGoodStatus(Source As String, NewState As States)
        If NewState < Allgood Then
            Allgood = NewState
        End If
        NotGood.Add(Source)
    End Sub



    Public Sub InternalFiles()
        'SkuData
        Dim skud As TimeSpan = Status.Data.sku_generator
        If skud.Minutes > 90 Then
            NoGoodStatus("Sku Generator", States.Down)
            AddControl("Sku Generator", States.Down, InternalPanel, "last updated: " + skud.ToString("%h") + " hour " + skud.ToString("%m") + " mins" + " ago")
        ElseIf skud.Minutes > 40 Then
            NoGoodStatus("Sku Generator", States.Mid)
            AddControl("Sku Generator", States.Mid, InternalPanel, "last updated: " + skud.ToString("%m") + " mins " + skud.ToString("%s") + " secs" + " ago")
        Else
            AddControl("Sku Generator", States.Up, InternalPanel, "last updated: " + skud.ToString("%m") + " mins " + skud.ToString("%s") + " secs" + " ago")
        End If

        'TRS
        Dim trs As TimeSpan = Status.Data.order_server_trays
        If trs.Minutes > 5 Then
            NoGoodStatus("Order Server", States.Down)
            AddControl("Order Server | Trays", States.Down, InternalPanel, "last updated: " + trs.ToString("%m") + " mins " + trs.ToString("%s") + " secs" + " ago")
        ElseIf trs.Minutes > 2 Then
            NoGoodStatus("Order Server", States.Mid)
            AddControl("Order Server | Trays", States.Mid, InternalPanel, "last updated: " + trs.ToString("%m") + " mins " + trs.ToString("%s") + " secs" + " ago")
        Else
            AddControl("Order Server | Trays", States.Up, InternalPanel, "last updated: " + trs.ToString("%m") + " mins " + trs.ToString("%s") + " secs" + " ago")
        End If

        'Orddef
        Dim orddef As TimeSpan = Status.Data.order_server_orders
        If orddef.Minutes > 30 Then
            NoGoodStatus("Order Server", States.Down)
            AddControl("Order Server | Orders", States.Down, InternalPanel, "last updated: " + orddef.ToString("%m") + " mins " + orddef.ToString("%s") + " secs" + " ago")
        ElseIf orddef.Minutes > 15 Then
            NoGoodStatus("Order Server", States.Mid)
            AddControl("Order Server | Orders", States.Mid, InternalPanel, "last updated: " + orddef.ToString("%m") + " mins " + orddef.ToString("%s") + " secs" + " ago")
        Else
            AddControl("Order Server | Orders", States.Up, InternalPanel, "last updated: " + orddef.ToString("%m") + " mins " + orddef.ToString("%s") + " secs" + " ago")
        End If

        'Linnworks Export - Processed
        Dim lipr As TimeSpan = Status.Data.linnworks_export_processed
        If lipr.Minutes > 60 Then
            NoGoodStatus("Linnworks Exports", States.Down)
            AddControl("Linnworks Processed Export", States.Down, InternalPanel, "last updated: " + lipr.ToString("%h") + " hour(s) " + lipr.ToString("%m") + " mins" + " ago")
        ElseIf lipr.Minutes > 30 Then
            NoGoodStatus("Linnworks Exports", States.Mid)
            AddControl("Linnworks Processed Export", States.Mid, InternalPanel, "last updated: " + lipr.ToString("%m") + " minutes ago")
        Else
            AddControl("Linnworks Processed Export", States.Up, InternalPanel, "last updated: " + lipr.ToString("%m") + " minutes ago")
        End If

        'Linnworks Export - Stock
        Dim list As TimeSpan = Status.Data.linnworks_export_inventory
        If list.Minutes > 60 Then
            NoGoodStatus("Linnworks Exports", States.Down)
            AddControl("Linnworks Stock Export", States.Down, InternalPanel, "last updated: " + list.ToString("%h") + " hour(s) " + list.ToString("%m") + " mins" + " ago")
        ElseIf list.Minutes > 30 Then
            NoGoodStatus("Linnworks Exports", States.Mid)
            AddControl("Linnworks Stock Export", States.Mid, InternalPanel, "last updated: " + list.ToString("%m") + " minutes ago")
        Else
            AddControl("Linnworks Stock Export", States.Up, InternalPanel, "last updated: " + list.ToString("%m") + " minutes ago")
        End If

        'MySQL Database
        If Not status.Data.mysql_database Then
            NoGoodStatus("MySQL Database", States.Down)
            AddControl("MySQL Database", States.Down, InternalPanel)
        Else
            AddControl("MySQL Database", States.Up, InternalPanel)
        End If

    End Sub

    Public Sub CheckExternals()
        'eBay.co.uk
        If Not status.Data.ebay Then
            NoGoodStatus("Ebay.co.uk", States.Down)
            AddControl("Ebay UK", States.Down, ExternalPanel)
        Else
            AddControl("Ebay UK", States.Up, ExternalPanel)
        End If
        'eBay API
        'If Not status.Data.ebay_api Then
        '    NoGoodStatus("Ebay API", States.Down)
        '    AddControl("Ebay API", States.Down, ExternalPanel)
        'Else
        '    AddControl("Ebay API", States.Up, ExternalPanel)
        'End If

        'Amazon
        If Not status.Data.amazon Then
            NoGoodStatus("Amazon UK", States.Down)
            AddControl("Amazon UK", States.Down, ExternalPanel)
        Else
            AddControl("Amazon UK", States.Up, ExternalPanel)
        End If
        'Amazon API
        If Not status.Data.amazon_productapi Then
            NoGoodStatus("Amazon API", States.Down)
            AddControl("Amazon API", States.Down, ExternalPanel)
        Else
            AddControl("Amazon API", States.Up, ExternalPanel)
        End If

        'LINNWORKS
        ''Com
        'If Not status.Data.linnworks_com Then
        '    NoGoodStatus("Linnworks.com", States.Down)
        '    AddControl("Linnworks", States.Down, ExternalPanel)
        'Else
        '    AddControl("Linnworks", States.Up, ExternalPanel)
        'End If
        'Net
        If Not status.Data.linnworks_net Then
            NoGoodStatus("Linnworks.net", States.Down)
            AddControl("Linnworks.NET", States.Down, ExternalPanel)
        Else
            AddControl("Linnworks.NET", States.Up, ExternalPanel)
        End If
        'API x3
        If Not status.Data.linnworks_api_api Then
            NoGoodStatus("Linnworks API", States.Down)
            AddControl("Linnworks API", States.Down, ExternalPanel)
        Else
            AddControl("Linnworks API", States.Up, ExternalPanel)
        End If
        If Not status.Data.linnworks_api_eu1 Then
            NoGoodStatus("Linnworks API - EU", States.Down)
            AddControl("Linnworks API EU", States.Down, ExternalPanel)
        Else
            AddControl("Linnworks API EU", States.Up, ExternalPanel)
        End If
        If Not status.Data.linnworks_api_apps Then
            NoGoodStatus("Linnworks API - Apps", States.Down)
            AddControl("Linnworks API Apps", States.Down, ExternalPanel)
        Else
            AddControl("Linnworks API Apps", States.Up, ExternalPanel)
        End If

    End Sub

    Public Sub CheckServers()
        'WAY WE@RE USING FRAMEWORK ON THE INTERNET WAHOO

        'IAN
        Dim ian As TimeSpan = Status.Data.ian
        If ian.Ticks < 0 Then
            NoGoodStatus("IAN (not responding)", States.Down)
            AddControl("IAN", States.Down, ServersPanel, "Did not respond within 100ms.")
        ElseIf ian.Milliseconds > 10 Then
            NoGoodStatus("IAN (slow)", States.Mid)
            AddControl("IAN", States.Mid, ServersPanel, "Ping time: " + ian.Milliseconds.ToString + "ms.")
        Else
            AddControl("IAN", States.Up, ServersPanel, "Ping time: " + Math.Round(ian.Milliseconds, 1).ToString + "ms.")
        End If

        'Brian
        Try
            Dim brian As TimeSpan = Status.Data.brian
            If brian.Ticks < 0 Then
                NoGoodStatus("BRIAN (not responding)", States.Down)
                AddControl("BRIAN", States.Down, ServersPanel, "Did not respond within 100ms.")
            ElseIf brian.Milliseconds > 10 Then
                NoGoodStatus("BRIAN (slow)", States.Mid)
                AddControl("BRIAN", States.Mid, ServersPanel, "Ping time: " + brian.Milliseconds.ToString + "ms.")
            Else
                AddControl("BRIAN", States.Up, ServersPanel, "Ping time: " + Math.Round(brian.Milliseconds, 1).ToString + "ms.")
            End If
        Catch ex As Exception
            AddControl("BRIAN", States.Down, ServersPanel, ex.ToString)
        End Try



        'Sue
        Dim sue As TimeSpan = Status.Data.sue
        If sue.Ticks < 0 Then
            NoGoodStatus("SUE (not responding)", States.Down)
            AddControl("SUE", States.Down, ServersPanel, "Did not respond within 100ms.")
        ElseIf sue.Milliseconds > 10 Then
            NoGoodStatus("SUE (slow)", States.Mid)
            AddControl("SUE", States.Mid, ServersPanel, "Ping time: " + sue.Milliseconds.ToString + "ms.")
        Else
            AddControl("SUE", States.Up, ServersPanel, "Ping time: " + Math.Round(sue.Milliseconds, 1).ToString + "ms.")
        End If

        'Old Server
        Dim old As TimeSpan = Status.Data.old_server
        If old.Ticks < 0 Then
            NoGoodStatus("Old Server (not responding)", States.Down)
            AddControl("Old Server", States.Down, ServersPanel, "Did not respond within 100ms.")
        ElseIf old.Milliseconds > 10 Then
            NoGoodStatus("Old Server (slow)", States.Mid)
            AddControl("Old Server", States.Mid, ServersPanel, "Ping time: " + old.Milliseconds.ToString + "ms.")
        Else
            AddControl("Old Server", States.Up, ServersPanel, "Ping time: " + Math.Round(old.Milliseconds, 1).ToString + "ms.")
        End If

        'Drop1
        Dim drop1 As TimeSpan = Status.Data.mysql_backup
        If drop1.Ticks < 0 Then
            NoGoodStatus("MySQL Backup Server (not responding)", States.Down)
            AddControl("MySQL Backup Server", States.Down, ServersPanel, "Did not respond within 400ms.")
        ElseIf drop1.Milliseconds > 100 Then
            NoGoodStatus("MySQL Backup Server (slow)", States.Mid)
            AddControl("MySQL Backup Server", States.Mid, ServersPanel, "Ping time: " + drop1.Milliseconds.ToString + "ms.")
        Else
            AddControl("MySQL Backup Server", States.Up, ServersPanel, "Ping time: " + Math.Round(drop1.Milliseconds, 1).ToString + "ms.")
        End If
    End Sub

    Public Sub Jumbotron()
        If Allgood = States.Up Then
            'Jumbotron
            JumboStatus.Text = "Fully Operational"
            JumboSubtext.Text = "Everything appears to be working fine."
            JumbrotronBG.BackColor = Drawing.Color.PaleGreen
        ElseIf Allgood = States.Mid Then
            JumboStatus.Text = "Mild issues"
            JumboSubtext.Text = "Something is responding slowly."
            JumbrotronBG.BackColor = Drawing.Color.PaleGoldenrod
        Else
            'Compile list of bad ones.
            Dim bads As String = ""
            For Each item As String In NotGood
                bads += item + ", "
            Next
            bads += "#"
            bads = bads.Replace(", #", " ")
            'Jumbotron
            JumboStatus.Text = "Limited Availability"
            JumboSubtext.Text = bads + "are all having issues. See below for more details."
            JumbrotronBG.BackColor = Drawing.Color.OrangeRed
        End If
    End Sub


    Public Enum States
        Down = 0
        Up = 1
        Mid = 2
        NoHook = 4
    End Enum

    Private Sub AddControl(Name As String, State As States, Panel As Panel, Optional Subtext As String = "")
        Dim Newcontrol As New Label

        Newcontrol.CssClass = "StatusControl"
        If State = States.Down Then
            Newcontrol.BackColor = Drawing.Color.FromArgb(255, 200, 200)
        ElseIf State = States.Up Then
            Newcontrol.BackColor = Drawing.Color.FromArgb(200, 255, 200)
        ElseIf State = States.NoHook Then
            Newcontrol.BackColor = Drawing.Color.FromArgb(238, 238, 238)
        Else
            Newcontrol.BackColor = Drawing.Color.FromArgb(255, 230, 200)
        End If
        If Not Subtext = "" Then
            'Create MainText
            Dim main As New Label
            main.Text = Name
            Newcontrol.Controls.Add(main)

            'Create Subtext
            Dim scs As New Label
            scs.Text = Subtext
            scs.CssClass = "StatusControlSubtext"
            Newcontrol.Controls.Add(scs)
        Else
            Newcontrol.Text = Name
        End If

        Panel.Controls.Add(Newcontrol)

    End Sub

    Protected Sub UpdateTimer_Tick(sender As Object, e As EventArgs) Handles UpdateTimer.Tick

    End Sub
End Class