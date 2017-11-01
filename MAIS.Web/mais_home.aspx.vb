Imports System.Web.Script.Services
Imports MAIS.Business
Imports MAIS.Business.Helpers
Imports MAIS.Business.Services
Imports MAIS.Business.Model
Imports MAIS.Business.Model.Enums
Imports MAIS.Data
Imports ODMRDDHelperClassLibrary.Utility

Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.HideLink = True
        Master.HideProgressBar = True
        Master.HideNotationLink = True
        SessionHelper.Notation_Flg = False
        'Master.HideUpdateProfileLink = True
        If Not IsPostBack Then
            Dim roles As MAIS.Business.Model.MAISRNDDRoleDetails = Nothing
            Dim queryString = String.Empty

            If Request.QueryString("GUID") IsNot Nothing AndAlso Trim(CStr(Request.QueryString("GUID"))) <> String.Empty Then
                queryString &= "GUID=" & Trim(CStr(Request.QueryString("GUID")))
            End If

            If Request.QueryString("ShowHeaders") IsNot Nothing Then
                If (Not String.IsNullOrEmpty(queryString)) Then
                    queryString &= "&"
                End If

                queryString &= "showheaders=" & Request.QueryString("ShowHeaders")
            End If
            If (UserAndRoleHelper.IsUserRN) Then
                roles = MAIS_Helper.GetUserRoleUsingMAIS(UserAndRoleHelper.CurrentUser.UserID)
                If (roles Is Nothing) Then
                    If Not String.IsNullOrEmpty(queryString) Then
                        Response.Redirect(PagesHelper.LogoutPage + "?" + queryString, False)
                    Else
                        Response.Redirect(PagesHelper.LogoutPage, False)
                    End If
                Else
                    SessionHelper.MAISLevelUserRole = roles.RoleSID
                End If
                Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
                Dim exists As Integer = maisSvc.CheckTheMandatoryFields(SessionHelper.LoginUsersRNLicense)
                If (exists = 1) Then
                    Master.HideUpdateProfileLink = True
                    Master.HideCertificateLink = False
                    SessionHelper.SelectedUserRole = 0
                    SessionHelper.ApplicationID = 0
                    SessionHelper.ApplicationType = String.Empty
                    SessionHelper.ExistingUserRole = 0
                    SessionHelper.SessionUniqueID = String.Empty
                    SessionHelper.RN_Flg = False
                    SessionHelper.Name = String.Empty
                    SessionHelper.ApplicationStatus = String.Empty
                    SessionHelper.MyUpdate_Profile = False
                Else
                    Master.HideUpdateProfileLink = False
                    Master.HideCertificateLink = True
                    SessionHelper.SelectedUserRole = SessionHelper.MAISLevelUserRole
                    SessionHelper.ApplicationID = maisSvc.GetAppIDByRNLicenseNumber(SessionHelper.LoginUsersRNLicense)
                    SessionHelper.SessionUniqueID = SessionHelper.LoginUsersRNLicense
                    SessionHelper.RN_Flg = UserAndRoleHelper.IsUserRN
                    SessionHelper.ExistingUserRole = SessionHelper.MAISLevelUserRole
                    SessionHelper.Name = UserAndRoleHelper.CurrentUser.UserName
                    SessionHelper.MyUpdate_Profile = True
                End If
            End If
            SetCertAlerts()
            'SetCertExprationTotals()
            SetDODDMessageAlerts()

        End If
    End Sub

    Private Sub SetCertAlerts()
        If UserAndRoleHelper.IsUserRN Then
            Dim srvMais As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
            Dim CertInfo As List(Of Model.Certificate) = srvMais.GetCertificationHistory(SessionHelper.LoginUsersRNLicense, UserAndRoleHelper.IsUserRN)

            Me.rpRNCert.DataSource = CertInfo
            Me.rpRNCert.DataBind()
        End If


    End Sub
    Private Sub SetCertExprationTotals()

        Dim srvMais As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Dim CertTotals As New List(Of Model.CertificateExpirationTotals)

        Select Case True
            Case UserAndRoleHelper.IsUserAdmin
                CertTotals = srvMais.GetCertificateExpirationTotals(RoleLevelCategory.RNMaster_RLC)
                Me.rptCertExpCount.DataSource = CertTotals
                Me.rptCertExpCount.DataBind()
            Case UserAndRoleHelper.IsUserRN
                CertTotals = srvMais.GetCertificateExpirationTotals(SessionHelper.MAISLevelUserRole)
                Me.rptCertExpCount.DataSource = CertTotals
                Me.rptCertExpCount.DataBind()
        End Select



    End Sub
    Private Sub SetDODDMessageAlerts(Optional ByVal LoadArchived As Boolean = False)
        Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Dim RoleList As String = String.Empty
        Dim LoadGrid As Boolean = False

        Select Case True
            Case UserAndRoleHelper.IsUserSecretary
                RoleList = Mais_Roles.Secretary
                LoadGrid = True
            Case UserAndRoleHelper.IsUserAdmin
                RoleList = Mais_Roles.Admin
                LoadGrid = True

            Case UserAndRoleHelper.IsUserRN
                Dim retcertHis As List(Of Model.Certificate) = maisSvc.GetCertificationHistory(SessionHelper.LoginUsersRNLicense, True) '("RN778899", True)
                If retcertHis.Count > 0 Then



                    For Each r In retcertHis
                        Dim em As Enums.RoleLevelCategory
                        em = r.Role_Category_Level_Sid

                        Dim D = EnumHelper.GetEnumDescription(em)
                        Dim rl As Enums.Mais_Roles
                        Select Case D
                            Case Enums.Mais_Roles.RNTrainer.ToString
                                rl = Mais_Roles.RNTrainer
                            Case Enums.Mais_Roles.RNInstructor.ToString
                                rl = Mais_Roles.RNInstructor
                            Case Enums.Mais_Roles.QARN.ToString
                                rl = Mais_Roles.QARN
                            Case "17Bed"
                                rl = Mais_Roles.Bed17
                            Case Enums.Mais_Roles.RNMaster.ToString
                                rl = Mais_Roles.RNMaster
                        End Select
                        If RoleList.Length > 0 Then
                            RoleList = RoleList & "," & rl
                        Else
                            RoleList = rl
                        End If
                    Next
                    LoadGrid = True
                End If


        End Select

        Dim maisMessageService As IDODDMessagePageService = StructureMap.ObjectFactory.GetInstance(Of IDODDMessagePageService)()
        Dim messageList As New List(Of Model.DODDMessageInfo)
        If LoadGrid Then

            messageList = maisMessageService.GetMessageDataByUserRoles(RoleList, UserAndRoleHelper.CurrentUser.UserID)

            Me.rptDODDMessage.DataSource = messageList
            Me.rptDODDMessage.DataBind()

        End If
        If LoadArchived = True Then
            Dim AcrhivedMessage As New List(Of Model.DODDMessageInfo)
            AcrhivedMessage = maisMessageService.GetMessageDataArchivedDataByUserRolesAndPersionID(RoleList, UserAndRoleHelper.CurrentUser.UserID)
            Me.rptArchivedMessage.DataSource = AcrhivedMessage
            Me.rptArchivedMessage.DataBind()

        End If
    End Sub

    Protected Sub lkbSubjectMessage_Click(sender As Object, e As EventArgs)
        Dim MessageID As Integer = CType(sender, LinkButton).CommandArgument



        Response.Redirect("maisDODD_Message.aspx?MessageID=" & MessageID)
    End Sub

    Protected Sub lkbArchivedMessage_Click(sender As Object, e As EventArgs) Handles lkbArchivedMessage.Click
        Me.pnlArchivedMessages.Visible = True
        Me.SetDODDMessageAlerts(True)
    End Sub
End Class