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
        Try
            Dim skud As TimeSpan = status.Data.sku_generator
            Dim SkuGenMsg As String = "A file which contains most data for every item we stock. This is the file that almost all applications load and is the reason everything takes forever to start up."
            If skud.Minutes > 28 Then
                NoGoodStatus("Sku Generator", States.Down)
                AddControl("Sku Generator", States.Down, InternalPanel, "last updated: " + skud.ToString("%h") + " hour " + skud.ToString("%m") + " mins" + " ago", SkuGenMsg)
            ElseIf skud.Minutes > 14 Then
                NoGoodStatus("Sku Generator", States.Mid)
                AddControl("Sku Generator", States.Mid, InternalPanel, "last updated: " + skud.ToString("%m") + " mins " + skud.ToString("%s") + " secs" + " ago", SkuGenMsg)
            Else
                AddControl("Sku Generator", States.Up, InternalPanel, "last updated: " + skud.ToString("%m") + " mins " + skud.ToString("%s") + " secs" + " ago", SkuGenMsg)
            End If
        Catch ex As Exception
            AddControl("Sku Generator", States.NoHook, InternalPanel, "Unable to work out last update.")
        End Try

        'TRS
        Try
            Dim trs As TimeSpan = status.Data.order_server_trays
            Dim OSTrayMsg As String = "A data file containing a list of all trays and the orders contained within them. This is mostly only used by the Supervisor PC."
            If trs.Minutes > 5 Then
                NoGoodStatus("Order Server", States.Down)
                AddControl("Order Server | Trays", States.Down, InternalPanel, "last updated: " + trs.ToString("%m") + " mins " + trs.ToString("%s") + " secs" + " ago", OSTrayMsg)
            ElseIf trs.Minutes > 2 Then
                NoGoodStatus("Order Server", States.Mid)
                AddControl("Order Server | Trays", States.Mid, InternalPanel, "last updated: " + trs.ToString("%m") + " mins " + trs.ToString("%s") + " secs" + " ago", OSTrayMsg)
            Else
                AddControl("Order Server | Trays", States.Up, InternalPanel, "last updated: " + trs.ToString("%m") + " mins " + trs.ToString("%s") + " secs" + " ago", OSTrayMsg)
            End If
        Catch ex As Exception
            AddControl("Sku Generator", States.NoHook, InternalPanel, "Unable to work out last update.")
        End Try

        'Orddef
        Try
            Dim orddef As TimeSpan = status.Data.order_server_orders
            Dim OSOrdersMsg As String = "A data file containing a slimmed down version of all orders currently in the system. This file should never be more than half an hour old. It is created by Order Server and used by all of the warehouse applications."
            If orddef.Minutes > 30 Then
                NoGoodStatus("Order Server", States.Down)
                AddControl("Order Server | Orders", States.Down, InternalPanel, "last updated: " + orddef.ToString("%m") + " mins " + orddef.ToString("%s") + " secs" + " ago", OSOrdersMsg)
            ElseIf orddef.Minutes > 21 Then
                NoGoodStatus("Order Server", States.Mid)
                AddControl("Order Server | Orders", States.Mid, InternalPanel, "last updated: " + orddef.ToString("%m") + " mins " + orddef.ToString("%s") + " secs" + " ago", OSOrdersMsg)
            Else
                AddControl("Order Server | Orders", States.Up, InternalPanel, "last updated: " + orddef.ToString("%m") + " mins " + orddef.ToString("%s") + " secs" + " ago", OSOrdersMsg)
            End If
        Catch ex As Exception
            AddControl("Sku Generator", States.NoHook, InternalPanel, "Unable to work out last update.")
        End Try

        'Linnworks Export - Processed
        Try
            Dim lipr As TimeSpan = status.Data.linnworks_export_processed
            Dim LinExpProcMsg As String = "A file containing usually around 6 weeks worth of processed orders. They send this file to us every half hour. It's used to calculate sales data and used in various other calculations."
            If lipr.Ticks = -1 Then
                NoGoodStatus("Linnworks Exports", States.Mid)
                AddControl("Linnworks Processed Export", States.Down, InternalPanel, "last updated: " + lipr.ToString("%h") + " hour(s) " + lipr.ToString("%m") + " mins" + " ago, Exported empty", LinExpProcMsg)
            ElseIf lipr.Minutes > 80 Then
                NoGoodStatus("Linnworks Exports", States.Down)
                AddControl("Linnworks Processed Export", States.Down, InternalPanel, "last updated: " + lipr.ToString("%h") + " hour(s) " + lipr.ToString("%m") + " mins" + " ago", LinExpProcMsg)
            ElseIf lipr.Minutes > 40 Then
                NoGoodStatus("Linnworks Exports", States.Mid)
                AddControl("Linnworks Processed Export", States.Mid, InternalPanel, "last updated: " + lipr.ToString("%m") + " minutes ago", LinExpProcMsg)
            Else
                AddControl("Linnworks Processed Export", States.Up, InternalPanel, "last updated: " + lipr.ToString("%m") + " minutes ago", LinExpProcMsg)
            End If
        Catch ex As Exception
            AddControl("Sku Generator", States.NoHook, InternalPanel, "Unable to work out last update.")
        End Try

        'Linnworks Export - Stock
        Try
            Dim list As TimeSpan = status.Data.linnworks_export_inventory
            Dim linexpstockmsg As String = "A file containing all of the stock levels in Linnworks is sent from them to us every half hour. It is then imported into Brian and then the system will have up to date stock levels."
            If list.Ticks = -1 Then
                NoGoodStatus("Linnworks Exports", States.Mid)
                AddControl("Linnworks Stock Export", States.Down, InternalPanel, "last updated: " + list.ToString("%h") + " hour(s) " + list.ToString("%m") + " mins" + " ago, Exported empty", linexpstockmsg)
            ElseIf list.Minutes > 45 Then
                NoGoodStatus("Linnworks Exports", States.Down)
                AddControl("Linnworks Stock Export", States.Down, InternalPanel, "last updated: " + list.ToString("%h") + " hour(s) " + list.ToString("%m") + " mins" + " ago", linexpstockmsg)
            ElseIf list.Minutes > 25 Then
                NoGoodStatus("Linnworks Exports", States.Mid)
                AddControl("Linnworks Stock Export", States.Mid, InternalPanel, "last updated: " + list.ToString("%m") + " minutes ago", linexpstockmsg)
            Else
                AddControl("Linnworks Stock Export", States.Up, InternalPanel, "last updated: " + list.ToString("%m") + " minutes ago", linexpstockmsg)
            End If
        Catch ex As Exception
            AddControl("Sku Generator", States.NoHook, InternalPanel, "Unable to work out last update.")
        End Try

        'MySQL Database
        Try
            If Not status.Data.mysql_database Then
                NoGoodStatus("MySQL Database", States.Down)
                AddControl("MySQL Database", States.Down, InternalPanel, "", "The database hosted on Brian.")
            Else
                AddControl("MySQL Database", States.Up, InternalPanel, "", "The database hosted on Brian.")
            End If
        Catch ex As Exception
            AddControl("Sku Generator", States.NoHook, InternalPanel, "Unable to work out last update.")
        End Try

        'Sales Data
        Try
            Dim sales As TimeSpan = status.Data.salesdata
            Dim salesmsg As String = "DAtabase entries for items, based on the legacy Sales Data system which powers reorder and most of Inventory Control."
            If sales.Ticks = -1 Then
                NoGoodStatus("Sales Data", States.Down)
                AddControl("Sales Data", States.Down, InternalPanel, "last updated: " + sales.ToString("%d") + " day(s) " + sales.ToString("%h") + " hours" + " ago, Exported empty", salesmsg)
            ElseIf sales.Totalhours > 50 Then
                NoGoodStatus("Sales Data", States.Down)
                AddControl("Sales Data", States.Down, InternalPanel, "last updated: " + sales.ToString("%d") + " day(s) " + sales.ToString("%h") + " hours" + " ago", salesmsg)
            ElseIf sales.Totalhours > 26 Then
                NoGoodStatus("Sales Data", States.Mid)
                AddControl("Sales Data", States.Mid, InternalPanel, "last updated: " + sales.ToString("%d") + " day(s) " + sales.ToString("%h") + " hours" + " ago", salesmsg)
            ElseIf sales.Totalhours > 24 Then
                AddControl("Sales Data", States.Up, InternalPanel, "last updated: " + sales.ToString("%d") + " day(s) " + sales.ToString("%h") + " hours" + " ago", salesmsg)
            Else
                AddControl("Sales Data", States.Up, InternalPanel, "last updated: " + sales.ToString("%h") + " hours, " + sales.ToString("%m") + " minutes ago", salesmsg)
            End If
        Catch ex As Exception
            AddControl("Sku Generator", States.NoHook, InternalPanel, "Unable to work out last update.")
        End Try
        'Location Auditing
        Try
            Dim locAudit As TimeSpan = status.Data.locationaudit
            Dim locAuditMsg As String = "Database entries for the stock check and auditing system"
            If locAudit.Ticks = -1 Then
                NoGoodStatus("Location Auditing", States.Down)
                AddControl("Location Auditing", States.Down, InternalPanel, "last updated: "+ locAudit.ToString("%h") + " hours" + " ago, Exported empty", locAuditMsg)
            ElseIf locAudit.Minutes > 180 Then
                NoGoodStatus("Location Auditing", States.Down)
                AddControl("Location Auditing", States.Down, InternalPanel, "last updated: " + locAudit.Minutes.ToString + " minutes ago", locAuditMsg)
            ElseIf locAudit.Minutes > 60 Then
                AddControl("Location Auditing", States.Mid, InternalPanel, "last updated: " + locAudit.Minutes.ToString + " minutes ago", locAuditMsg)
            Else
                AddControl("Location Auditing", States.Up, InternalPanel, "last updated: " + locAudit.Seconds.ToString + " seconds ago", locAuditMsg)
            End If
        Catch ex As Exception
            AddControl("Sku Generator", States.NoHook, InternalPanel, "Unable to work out last update.")
        End Try
        'Backup Data.
        Try
            Dim backup As TimeSpan = status.Data.Backup
            Dim backupmsg As String = "A regular data backup containing data from the X and T drives."
            If backup.Ticks = -1 Then
                NoGoodStatus("Backup Data", States.Down)
                AddControl("Backup Data", States.Down, InternalPanel, "last updated: " + backup.ToString("%d") + " day(s) " + backup.ToString("%h") + " hours" + " ago, Exported empty", backupmsg)
            ElseIf backup.TotalHours > 96 Then
                NoGoodStatus("Backup Data", States.Down)
                AddControl("Backup Data", States.Down, InternalPanel, "last updated: " + backup.ToString("%d") + " day(s) " + backup.ToString("%h") + " hours" + " ago", backupmsg)
            ElseIf backup.TotalHours > 72 Then
                NoGoodStatus("Backup Data", States.Mid)
                AddControl("Backup Data", States.Mid, InternalPanel, "last updated: " + backup.ToString("%d") + " day(s) " + backup.ToString("%h") + " hours" + " ago", backupmsg)
            ElseIf backup.TotalHours > 24 Then
                AddControl("Backup Data", States.Up, InternalPanel, "last updated: " + backup.ToString("%d") + " day(s) " + backup.ToString("%h") + " hours" + " ago", backupmsg)
            Else
                AddControl("Backup Data", States.Up, InternalPanel, "last updated: " + backup.ToString("%h") + " hours, " + backup.ToString("%m") + " minutes ago", backupmsg)
            End If
        Catch ex As Exception
            AddControl("Sku Generator", States.NoHook, InternalPanel, "Unable to work out last update.")
        End Try

    End Sub

    Public Sub CheckExternals()

        Dim LinNetMsg As String = "This is the main Linnworks.net site. Nothing more really."
        If Not status.Data.linnworks_net Then
            NoGoodStatus("Linnworks.net", States.Down)
            AddControl("Linnworks.NET", States.Down, ExternalPanel, "", LinNetMsg)
        Else
            AddControl("Linnworks.NET", States.Up, ExternalPanel, "", LinNetMsg)
        End If
        'API x3
        Dim LinApiMsg As String = "api.linnworks.net is Linnworks\' main API gateway. This server tells us where to send our requests after logging in with Linnworks Authorization. Linnworks Desktop also uses this."
        If Not status.Data.linnworks_api_api Then
            NoGoodStatus("Linnworks API", States.Down)
            AddControl("Linnworks API", States.Down, ExternalPanel, "", LinApiMsg)
        Else
            AddControl("Linnworks API", States.Up, ExternalPanel, "", LinApiMsg)
        End If


    End Sub

    Public Sub CheckServers()
        'WAY WE@RE USING FRAMEWORK ON THE INTERNET WAHOO

        'IAN
        Dim ian As TimeSpan = status.Data.ian
        Dim ianmsg As String = "SQLSERVER looks after a few user profiles, but is mostly in charge of the order system. Ian contains services such as the Order server and Sku Generator, and also holds the majority of office user profiles."
        If ian.Ticks < 0 Then
            NoGoodStatus("SQLSERVER (not responding)", States.Down)
            AddControl("SQLSERVER", States.Down, ServersPanel, "Did not respond within 100ms.", ianmsg)
        ElseIf ian.Milliseconds > 10 Then
            NoGoodStatus("SQLSERVER (slow)", States.Mid)
            AddControl("SQLSERVER", States.Mid, ServersPanel, "Ping time: " + ian.Milliseconds.ToString + "ms.", ianmsg)
        Else
            AddControl("SQLSERVER", States.Up, ServersPanel, "Ping time: " + Math.Round(ian.Milliseconds, 1).ToString + "ms.", ianmsg)
        End If

        'Brian
        Try
            Dim brian As TimeSpan = status.Data.brian
            Dim brianmsg As String = "Brian is the main database server. He is the most powerful server we have and is vital to keeping the office running, looking after any database bound services. Brian has nothing to do with the orders systems (although prepack relies on him to get updated product information)."
            If brian.Ticks < 0 Then
                NoGoodStatus("BRIAN (not responding)", States.Down)
                AddControl("BRIAN", States.Down, ServersPanel, "Did not respond within 100ms.", brianmsg)
            ElseIf brian.Milliseconds > 10 Then
                NoGoodStatus("BRIAN (slow)", States.Mid)
                AddControl("BRIAN", States.Mid, ServersPanel, "Ping time: " + brian.Milliseconds.ToString + "ms.", brianmsg)
            Else
                AddControl("BRIAN", States.Up, ServersPanel, "Ping time: " + Math.Round(brian.Milliseconds, 1).ToString + "ms.", brianmsg)
            End If
        Catch ex As Exception
            AddControl("BRIAN", States.Down, ServersPanel, ex.ToString)
        End Try



        ''Sue
        'Dim sue As TimeSpan = status.Data.sue
        'Dim Suemsg As String = "Sue is primarily a file server, storing all of the application data like analytics and order data, and photos for the listing process. "
        'If sue.Ticks < 0 Then
        '    NoGoodStatus("SUE (not responding)", States.Down)
        '    AddControl("SUE", States.Down, ServersPanel, "Did not respond within 100ms.", Suemsg)
        'ElseIf sue.Milliseconds > 10 Then
        '    NoGoodStatus("SUE (slow)", States.Mid)
        '    AddControl("SUE", States.Mid, ServersPanel, "Ping time: " + sue.Milliseconds.ToString + "ms.", Suemsg)
        'Else
        '    AddControl("SUE", States.Up, ServersPanel, "Ping time: " + Math.Round(sue.Milliseconds, 1).ToString + "ms.", Suemsg)
        'End If

        'Old Server
        Dim old As TimeSpan = status.Data.old_server
        Dim oldmsg As String = "This server looks after the hosting of this page, a few user profiles and also holds the X drive. It also does a couple of other odd bits."
        If old.Ticks < 0 Then
            NoGoodStatus("Old Server (not responding)", States.Down)
            AddControl("Old Server", States.Down, ServersPanel, "Did not respond within 100ms.", oldmsg)
        ElseIf old.Milliseconds > 10 Then
            NoGoodStatus("Old Server (slow)", States.Mid)
            AddControl("Old Server", States.Mid, ServersPanel, "Ping time: " + old.Milliseconds.ToString + "ms.", oldmsg)
        Else
            AddControl("Old Server", States.Up, ServersPanel, "Ping time: " + Math.Round(old.Milliseconds, 1).ToString + "ms.", oldmsg)
        End If

        'vhost-1
        Dim vhost1 As TimeSpan = status.Data.vhost1
        Dim vhost1msg As String = "VHost-1 is a server which runs the large data backups. It also hosts the virtual test server."
        If vhost1.Ticks < 0 Then
            NoGoodStatus("VHost-1 (not responding)", States.Down)
            AddControl("VHost-1 ", States.Down, ServersPanel, "Did not respond within 100ms.", vhost1msg)
        ElseIf vhost1.Milliseconds > 10 Then
            NoGoodStatus("VHost-1 (slow)", States.Mid)
            AddControl("VHost-1", States.Mid, ServersPanel, "Ping time: " + vhost1.Milliseconds.ToString + "ms.", vhost1msg)
        Else
            AddControl("VHost-1", States.Up, ServersPanel, "Ping time: " + Math.Round(vhost1.Milliseconds, 1).ToString + "ms.", vhost1msg)
        End If

        'vhost-2
        Dim vhost2 As TimeSpan = status.Data.VHost2
        Dim vhost2msg As String = "VHost-2 is a server which hosts the software that the PIs are connected to, along with other virtualised software."
        If vhost2.Ticks < 0 Then
            NoGoodStatus("VHost-2 (not responding)", States.Down)
            AddControl("VHost-2 ", States.Down, ServersPanel, "Did not respond within 100ms.", vhost2msg)
        ElseIf vhost2.Milliseconds > 10 Then
            NoGoodStatus("VHost-2 (slow)", States.Mid)
            AddControl("VHost-2", States.Mid, ServersPanel, "Ping time: " + vhost2.Milliseconds.ToString + "ms.", vhost2msg)
        Else
            AddControl("VHost-2", States.Up, ServersPanel, "Ping time: " + Math.Round(vhost2.Milliseconds, 1).ToString + "ms.", vhost2msg)
        End If

        ''testserver
        'Dim TestServer As TimeSpan = status.Data.TestServer
        'Dim TestServermsg As String = "Test server allows IT to test software updates without affecting the main network"
        'If TestServer.Ticks < 0 Then
        '    NoGoodStatus("Test Server (not responding)", States.Down)
        '    AddControl("Test Server ", States.Down, ServersPanel, "Did not respond within 100ms.", TestServermsg)
        'ElseIf TestServer.Milliseconds > 10 Then
        '    NoGoodStatus("Test Server (slow)", States.Mid)
        '    AddControl("Test Server", States.Mid, ServersPanel, "Ping time: " + TestServer.Milliseconds.ToString + "ms.", TestServermsg)
        'Else
        '    AddControl("Test Server", States.Up, ServersPanel, "Ping time: " + Math.Round(TestServer.Milliseconds, 1).ToString + "ms.", TestServermsg)
        'End If

        ''Drop1
        'Dim drop1 As TimeSpan = status.Data.mysql_backup
        'Dim drop1msg As String = "This server stores database backups (Generated at 4AM every morning) and also stores assets for our version of the PPRetail Shop."
        'If drop1.Ticks < 0 Then
        '    NoGoodStatus("MySQL Backup Server (not responding)", States.Down)
        '    AddControl("MySQL Backup Server", States.Down, ServersPanel, "Did not respond within 400ms.", drop1msg)
        'ElseIf drop1.Milliseconds > 100 Then
        '    NoGoodStatus("MySQL Backup Server (slow)", States.Mid)
        '    AddControl("MySQL Backup Server", States.Mid, ServersPanel, "Ping time: " + drop1.Milliseconds.ToString + "ms.", drop1msg)
        'Else
        '    AddControl("MySQL Backup Server", States.Up, ServersPanel, "Ping time: " + Math.Round(drop1.Milliseconds, 1).ToString + "ms.", drop1msg)
        'End If
    End Sub

    Public Sub Jumbotron()
        If Allgood = States.Up Then
            'Jumbotron
            JumboStatus.Text = "Fully Operational"
            JumboSubtext.Text = "Everything appears to be working fine. Never Fear."
            JumbrotronBG.BackColor = Drawing.Color.PaleGreen
        ElseIf Allgood = States.Mid Then
            JumboStatus.Text = "Mild issues"
            JumboSubtext.Text = "Something is responding slowly. There is no need to fear"
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
            JumboSubtext.Text = bads + "are all having issues. See below for more details. Start fearing"
            JumbrotronBG.BackColor = Drawing.Color.OrangeRed
        End If
    End Sub


    Public Enum States
        Down = 0
        Up = 1
        Mid = 2
        NoHook = 4
    End Enum

    Private Sub AddControl(Name As String, State As States, Panel As Panel, Optional Subtext As String = "", Optional message As String = "")
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
        If Not message = "" Then
            Newcontrol.Attributes.Add("onClick", "alert('" + Name + "\r\n\r\n" + message + "');")
            Newcontrol.ToolTip = message.Replace("\", "")
        End If

        Panel.Controls.Add(Newcontrol)

    End Sub

    Protected Sub UpdateTimer_Tick(sender As Object, e As EventArgs) Handles UpdateTimer.Tick

    End Sub
End Class