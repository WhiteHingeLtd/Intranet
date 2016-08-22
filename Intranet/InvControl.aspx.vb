Imports WHLClasses
Public Class InvControl
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim skus As SkuCollection = Application("SkuCollection")
        Dim Table As New DataTable
        Table.Columns.Add("colSku", ("").GetType)
        Table.Columns.Add("colTitle", ("").GetType)
        Table.Columns.Add("colRetail", (0.01).GetType)
        If Searchbox.Text.Length > 2 Then
            skus = skus.AdvancedSearch(Searchbox.Text,,, MatchAllCheck.Checked)
        Else
            Searchbox.Text = ""
        End If

        For Each item As WhlSKU In skus

            Table.Rows.Add(item.SKU, item.Title.Invoice, item.Price.Retail)

        Next

        SkuGrid.DataSource = Table
        SkuGrid.DataBind()
    End Sub

End Class