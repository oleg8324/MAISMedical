Public Class Resources
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.HideLink = True
        Master.HideProgressBar = True
    End Sub

End Class