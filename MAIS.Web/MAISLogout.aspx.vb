Public Class MAISLogout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'TODO: 
        Dim msg As String = Request.QueryString("reason")
        If msg <> "" Then
            lblMsg.Text = msg
        Else

        End If
        If (UserAndRoleHelper.IsUserSecretary) Then
            lblMsg.Text = "You are not associated with any RN, Please ask respective RN to map you to the system."
        ElseIf (UserAndRoleHelper.IsUserRN) Then
            lblMsg.Text = "You don't have permission in MAIS application. Please contact security Administration"
        End If   

    End Sub

End Class