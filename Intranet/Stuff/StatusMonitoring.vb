Imports System.IO
Imports WHLClasses.MiscFunctions
Public Class StatusMonitoring

    Public Sub New()

    End Sub

    Private _Data As New StatusObject

    Public ReadOnly Property Data As StatusObject
        Get
            If (Now - _Data.LastCheck).Seconds > 6 Then
                RefreshData()
            End If
            Return _Data
        End Get
    End Property

    'Refersh Datas
    Public Sub RefreshData()
        _Data = New StatusObject
    End Sub


    Public Class StatusObject
        Public Sub New()
            'Internals 
            sku_generator = CheckFileAge("\\VHOST-1\DFSRoot\AppData\Collections\Sku Collection 1.skus")
            order_server_trays = CheckFileAge("\\VHOST-1\DFSRoot\AppData\Trays\.trs")
            order_server_orders = CheckFileAge("\\VHOST-1\DFSRoot\AppData\Orders\.orddef")
            Try
                linnworks_export_processed = CheckFileAge("\\WIN-NOHLS1H9ER8\Data Storage\Linncloud\processed.csv")
            Catch ex As Exception
                linnworks_export_processed = TimeSpan.MaxValue
            End Try
            Try
                linnworks_export_inventory = CheckFileAge("\\WIN-NOHLS1H9ER8\Data Storage\Linncloud\inventory.csv")
            Catch ex As Exception
                linnworks_export_inventory = TimeSpan.MaxValue
            End Try
            
            mysql_database = WHLClasses.MySQL.TestConn.ToString.StartsWith("Connection to ")
            salesdata = (Now - (Date.ParseExact(WHLClasses.MySQL.SelectData("SELECT Shortsku FROM whldata.salesdata ORDER BY ShortSku DESC LIMIT 1 ;")(0)(0).ToString, "yyyy-MM-dd HH:mm:ss", Globalization.CultureInfo.InvariantCulture)))
            locationaudit = (Now - DirectCast(WHLClasses.MySQL.SelectData("SELECT CAST(DateOfEvent as datetime) from whldata.locationaudit order by auditID desc limit 1;")(0)(0),DateTime))
            'Backup file
            Backup = CheckFileAge((New DirectoryInfo("\\vhost-1\E\T and X\")).GetFiles().OrderByDescending(Function(f As FileInfo) f.LastWriteTime).First().FullName)


            'Servers
            ian = PingServerTime("IAN")
            brian = PingServerTime("BRIAN")
            ' RIP SUE sue = PingServerTime("SUE")
            old_server = PingServerTime("SERVER")
            mysql_backup = PingServerTime("drop1.drops.ad.whitehinge.com", 400)
            vhost1 = PingServerTime("VHost-1")
            VHost2 = PingServerTime("Vhost-2")
            TestServer = PingServerTime("TestServer")

            'Externals
            linnworks_api_ext = GetServerHTTPStatus("ext.linnworks.net", 400)
            linnworks_api_apps = GetServerHTTPStatus("apps.linnworks.net", 400)
            linnworks_api_api = GetServerHTTPStatus("api.linnworks.net", 400)
            linnworks_net = GetServerHTTPStatus("www.linnworks.net", 400)
            linnworks_com = GetServerHTTPStatus("www.linnworks.com", 400)

            amazon = GetServerHTTPStatus("www.amazon.co.uk", 400)
            amazon_productapi = GetServerHTTPStatus("webservices.amazon.com", 400)
            ebay = GetServerHTTPStatus("www.ebay.co.uk", 600)
            'ebay_api = GetServerHTTPStatus("developer.ebay.com", 600)
            'End
            LastCheck = Now
        End Sub

        Public LastCheck As DateTime



        'Externals
        Public ebay As Boolean
        Public ebay_api As Boolean
        Public amazon As Boolean
        Public amazon_productapi As Boolean
        Public despatchbay As Boolean
        Public linnworks_net As Boolean
        Public linnworks_com As Boolean
        Public linnworks_api_ext As Boolean
        Public linnworks_api_apps As Boolean
        Public linnworks_api_api As Boolean

        'Internals
        Public sku_generator As TimeSpan
        Public order_server_trays As TimeSpan
        Public order_server_orders As TimeSpan
        Public linnworks_export_processed As TimeSpan
        Public linnworks_export_inventory As TimeSpan
        Public mysql_database As Boolean
        Public salesdata As TimeSpan
        Public locationaudit as TimeSpan
        Public Backup As TimeSpan

        'Servers
        Public ian As TimeSpan
        Public brian As TimeSpan
        Public sue As TimeSpan
        Public old_server As TimeSpan
        Public mysql_backup As TimeSpan
        Public vhost1 As TimeSpan
        Public VHost2 As TimeSpan
        Public TestServer As TimeSpan
        Public Build As TimeSpan

    End Class


End Class
