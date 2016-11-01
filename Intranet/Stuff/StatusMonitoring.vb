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
            sku_generator = CheckFileAge("\\SUE\DFSRoot\AppData\Collections\Skudata.skuc")
            order_server_trays = CheckFileAge("\\SUE\DFSRoot\AppData\Trays\.trs")
            order_server_orders = CheckFileAge("\\SUE\DFSRoot\AppData\Orders\.orddef")
            linnworks_export_processed = CheckFileAge("\\WIN-NOHLS1H9ER8\Data Storage\Linncloud\processed.csv")
            linnworks_export_inventory = CheckFileAge("\\WIN-NOHLS1H9ER8\Data Storage\Linncloud\inventory.csv")
            mysql_database = WHLClasses.MySQL.TestConn.ToString.StartsWith("Connection to ")
            salesdata = (Now - (Date.ParseExact(WHLClasses.SelectData("SELECT Shortsku FROM whldata.salesdata ORDER BY ShortSku DESC LIMIT 1 ;")(0)(0).ToString, "yyyy-MM-dd HH:mm:ss", Globalization.CultureInfo.InvariantCulture)))


            'Servers
            ian = PingServerTime("IAN")
            brian = PingServerTime("BRIAN")
            sue = PingServerTime("SUE")
            old_server = PingServerTime("SERVER")
            mysql_backup = PingServerTime("drop1.drops.ad.whitehinge.com", 400)

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

        'Servers
        Public ian As TimeSpan
        Public brian As TimeSpan
        Public sue As TimeSpan
        Public old_server As TimeSpan
        Public mysql_backup As TimeSpan
    End Class


End Class
