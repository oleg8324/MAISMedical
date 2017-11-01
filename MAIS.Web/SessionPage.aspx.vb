Public Class SessionPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblSessionID.Text = Me.Session.SessionID
    End Sub

End Class