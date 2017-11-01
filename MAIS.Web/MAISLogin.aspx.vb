Imports MAIS.Business.Model
Imports MAIS.Business.Services
Imports ODMRDDHelperClassLibrary.Utility

Public Class MAISLogin
    Inherits System.Web.UI.Page
    Private _usermapping As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtFirstname.Value = UserAndRoleHelper.CurrentUser.FirstName
        txtLastname.Value = UserAndRoleHelper.CurrentUser.LastName
        txtFirstname.Disabled = True
        txtLastname.Disabled = True
        If Not IsPostBack Then
            Master.HideLink = True
            Master.HideProgressBar = True
            Master.HideOtherLink = True
            txtRNLicense.Value = String.Empty
            If (UserAndRoleHelper.CurrentUser.UserID > 0) Then
                '  Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
                'If (UserAndRoleHelper.IsUserSecretary) Then
                '    pnLogin.Visible = False
                '    Dim flag As Boolean = maisSvc.CheckSecetaryMapping(UserAndRoleHelper.CurrentUser.UserID)
                '    If (flag) Then
                '        Response.Redirect("mais_home.aspx")
                '    Else
                '        lblError.Text = "You are not associated with any RN, Please ask respective RN to map you to the system."
                '        lblError.Style.Add("font-weight", "Bold")
                '    End If
                'Else
                If (UserAndRoleHelper.IsUserRN) Then
                    pnLogin.Visible = True
                    txtRNLicense.Disabled = False
                    btnSignIn.Enabled = True
                End If
                'End If
            End If
        End If
    End Sub

    Protected Sub btnSignIn_Click(sender As Object, e As EventArgs) Handles btnSignIn.Click
        If (Not String.IsNullOrEmpty(txtRNLicense.Value)) Then
            Dim userDetails As New UserLoginSearch
            userDetails.FirstName = UserAndRoleHelper.CurrentUser.FirstName
            userDetails.LastName = UserAndRoleHelper.CurrentUser.LastName
            userDetails.RNLicenseNumber = txtRNLicense.Value
            Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
            Dim retval As New ReturnObject(Of Long)(-1L)
            retval = maisSvc.SaveUserRNMappingData(userDetails)
            If (retval.Errors.Count = 0) Then
                SessionHelper.LoginUsersRNLicense = retval.Messages(0).Message
                Response.Redirect("mais_home.aspx")
            Else
                lblError.Text = retval.Errors(0).Message
            End If
        Else
            lblError.Text = "Please enter the mandatory field RN License Number."
        End If
    End Sub
    Public Property UserMappingID As Integer
        Get
            Return _usermapping
        End Get
        Set(ByVal value As Integer)
            _usermapping = value
        End Set
    End Property
End Class