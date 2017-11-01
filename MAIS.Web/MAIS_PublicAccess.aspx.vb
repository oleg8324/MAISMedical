Public Class MAIS_PublicAccess
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub rblSelect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblSelect.SelectedIndexChanged

        If (rblSelect.SelectedValue = "1") Then
            rblSelect.SelectedIndex = -1
            Response.Redirect("MAIS_PASearch.aspx")

        ElseIf (rblSelect.SelectedValue = "2") Then
            rblSelect.SelectedIndex = -1
            Dim oldmapub As String = "https://ma.prodapps.dodd.ohio.gov/ma_oamrp.asp"
            If ConfigurationManager.AppSettings("OLDMAPUBLIC") IsNot Nothing Then
                oldmapub = ConfigurationManager.AppSettings("OLDMAPUBLIC")
            End If
            Response.Redirect(oldmapub)
        End If
    End Sub
End Class