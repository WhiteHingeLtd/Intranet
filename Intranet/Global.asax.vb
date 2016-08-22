Imports WHLClasses

Imports System.Web.Optimization

Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(sender As Object, e As EventArgs)
        ' Fires when the application is started
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)

        'Create the first Status
        Application("Status") = New StatusMonitoring

        'Load the fucking skucollection.

        Dim loader As New GenericDataController
        Application("SkuCollection") = loader.SmartSkuCollLoad(False, "\\SUE\DFSRoot\AppData\Collections\")
    End Sub


End Class