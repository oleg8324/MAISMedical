Imports System.Web.Script.Services
Imports MAIS.Business.Services
Imports MAIS.Business
Imports MAIS.Business.Model
Imports MAIS.Business.Model.Enums
Imports MAIS.Business.Helpers

Public Class StartPage
    Inherits System.Web.UI.Page
    Dim _progress As New ProgressBar
    Dim _rntrainerRoles As Integer = RoleLevelCategory.RNTrainer_RLC
    Dim _rnInstructorRoles As Integer = RoleLevelCategory.RNInstructor_RLC
    Dim _rnMasterRoles As Integer = RoleLevelCategory.RNMaster_RLC
    Dim _rnQARoles As Integer = RoleLevelCategory.QARN_RLC
    Dim _rnICFRoles As Integer = RoleLevelCategory.Bed17_RLC
    Dim _rnDODDRoles As Integer = RoleLevelCategory.DDPersonnel_RLC
    Dim _rnDODDRolesCat2 As Integer = RoleLevelCategory.DDPersonnel2_RLC
    Dim _rnDODDRolesCat3 As Integer = RoleLevelCategory.DDPersonnel3_RLC
    Dim _rnSuperVisorRoles As Integer = 14
    Dim certificationDetails As New CertificationEligibleInfo
    Dim querystring As Object = Nothing
    Dim queryObject As String = Nothing
    Private Shared _rnorDD As Boolean
    Private Shared _appType As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim dynTable As String = String.Empty
            lblError.InnerText = ""
            RNorDD = SessionHelper.RN_Flg
            Dim startPageSvc As IStartPageService = StructureMap.ObjectFactory.GetInstance(Of IStartPageService)()
            hdRNorDDRole.Value = SessionHelper.ExistingUserRole '4
            hdUserRoleinMais.Value = SessionHelper.MAISLevelUserRole '7
            querystring = Request.QueryString("Roles")
            queryObject = Request.QueryString("Create")
            If (queryObject = "New") Then
                SessionHelper.BrandNew = True
            Else
                SessionHelper.BrandNew = False
            End If
            If (SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn) And querystring IsNot Nothing) Then
                If (querystring.ToString().Length > 1) Then
                    querystring = querystring.ToString().Substring(0, querystring.ToString().Length - 1)
                End If
            End If
            If (Not String.IsNullOrEmpty(SessionHelper.ApplicationType)) Then
                'If (SessionHelper.ApplicationType <> EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn)) Then
                startPage.Disabled = False
                rdbAddOn.Enabled = True
                btnSaveAndContinue.Disabled = False
                If (Convert.ToInt32(SessionHelper.ApplicationID) > 0) Then
                    hdApplicationID.Value = SessionHelper.ApplicationID.ToString()
                    GetApplicationInformation(SessionHelper.ApplicationID, SessionHelper.ExistingUserRole)
                    DefaultGridOnPageLoad(SessionHelper.ApplicationType, startPageSvc)
                Else
                    hdApplicationID.Value = ""
                    DefaultGridOnPageLoad(SessionHelper.ApplicationType, startPageSvc)
                    If (Convert.ToInt32(SessionHelper.ExistingUserRole) <> 0 And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn)) Then
                        EnableTheControls(SessionHelper.ExistingUserRole, SessionHelper.ApplicationType)
                    ElseIf (Convert.ToInt32(SessionHelper.SelectedUserRole) <> 0 And SessionHelper.ApplicationType <> EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn)) Then
                        EnableTheControls(SessionHelper.SelectedUserRole, SessionHelper.ApplicationType)
                    Else
                        pnlInitial.Visible = True
                        lblInitial.Visible = True
                        SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.Initial)
                        'DefaultGridOnPageLoad(SessionHelper.ApplicationType, startPageSvc)
                        If (RNorDD) Then
                            hdRNorDDRole.Value = SessionHelper.MAISLevelUserRole '_rntrainerRoles
                            If (SessionHelper.MAISLevelUserRole = _rntrainerRoles) Then
                                rdbInitial.Items(1).Enabled = True
                            ElseIf (SessionHelper.MAISLevelUserRole = _rnQARoles) Then
                                rdbInitial.Items(1).Enabled = True
                            ElseIf (SessionHelper.MAISLevelUserRole = _rnICFRoles) Then
                                rdbInitial.Items(1).Enabled = True
                            ElseIf (SessionHelper.MAISLevelUserRole = _rnInstructorRoles Or SessionHelper.MAISLevelUserRole = _rnMasterRoles Or UserAndRoleHelper.IsUserAdmin) Then
                                rdbInitial.Items(0).Enabled = True
                                rdbInitial.Items(1).Enabled = True
                                rdbInitial.Items(2).Enabled = True
                            ElseIf (UserAndRoleHelper.IsUserSecretary) Then
                                rdbInitial.Items(0).Enabled = True
                                rdbInitial.Items(1).Enabled = True
                            End If
                            'rdbInitial.Items(4).Enabled = True
                        Else
                            hdRNorDDRole.Value = _rnDODDRoles
                            rdbInitial.Items(3).Enabled = True
                        End If
                    End If
                End If
                'Else
                'If (SessionHelper.RN_Flg) Then
                '    If (SessionHelper.LoginUsersRNLicense = SessionHelper.SessionUniqueID And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn)) Then
                '        startPage.Disabled = True
                '        lblError.Attributes.Add("style", "color:red")
                '        lblError.InnerText = "You cannot do add on application to yourself"
                '        rdbAddOn.Enabled = False
                '        btnSaveAndContinue.Disabled = True
                '    End If
                'End If
            Else
                pnlInitial.Visible = True
                lblInitial.Visible = True
                SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.Initial)
                DefaultGridOnPageLoad(SessionHelper.ApplicationType, startPageSvc)
                If (RNorDD) Then
                    hdRNorDDRole.Value = SessionHelper.MAISLevelUserRole '_rntrainerRoles
                    If (SessionHelper.MAISLevelUserRole = _rntrainerRoles) Then
                        rdbInitial.Items(1).Enabled = True
                    ElseIf (SessionHelper.MAISLevelUserRole = _rnQARoles) Then
                        rdbInitial.Items(1).Enabled = True
                    ElseIf (SessionHelper.MAISLevelUserRole = _rnICFRoles) Then
                        rdbInitial.Items(1).Enabled = True
                    ElseIf (SessionHelper.MAISLevelUserRole = _rnInstructorRoles Or SessionHelper.MAISLevelUserRole = _rnMasterRoles Or UserAndRoleHelper.IsUserAdmin) Then
                        rdbInitial.Items(0).Enabled = True
                        rdbInitial.Items(1).Enabled = True
                        rdbInitial.Items(2).Enabled = True
                    ElseIf (UserAndRoleHelper.IsUserSecretary) Then
                        rdbInitial.Items(0).Enabled = True
                        rdbInitial.Items(1).Enabled = True
                    End If
                    'rdbInitial.Items(4).Enabled = True
                Else
                    hdRNorDDRole.Value = _rnDODDRoles
                    rdbInitial.Items(3).Enabled = True
                End If
            End If
        End If
    End Sub
    Private Sub GetApplicationInformation(ByVal applicationID As Integer, ByVal userRoleID As Integer)
        Dim details As New MAISApplicationDetails
        Dim startPageSvc As IStartPageService = StructureMap.ObjectFactory.GetInstance(Of IStartPageService)()
        details = startPageSvc.GetApplicationInformation(applicationID)
        EnableAndSelectControls(details, SessionHelper.ExistingUserRole)
        SessionHelper.SelectedUserRole = details.RoleCategoryLevelID
        If (SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn)) Then
            hdRNorDDRole.Value = SessionHelper.ExistingUserRole
            'hdRNorDDRole.Value = SessionHelper.SelectedUserRole
        Else
            hdRNorDDRole.Value = SessionHelper.SelectedUserRole
        End If
    End Sub
    Public Sub EnableAndSelectControls(ByVal details As MAISApplicationDetails, ByVal userRoleID As Integer)
        If (details.ApplicationTypeID = Model.Enums.ApplicationType.Initial) Then
            pnlInitial.Visible = True
            Select Case details.RoleCategoryLevelID.ToString()
                Case rdbInitial.Items(0).Value
                    rdbInitial.Items(0).Enabled = True
                    rdbInitial.Items(0).Selected = True
                Case rdbInitial.Items(1).Value
                    rdbInitial.Items(1).Enabled = True
                    rdbInitial.Items(1).Selected = True
                Case rdbInitial.Items(2).Value
                    rdbInitial.Items(2).Enabled = True
                    rdbInitial.Items(2).Selected = True
                Case rdbInitial.Items(3).Value
                    rdbInitial.Items(3).Enabled = True
                    rdbInitial.Items(3).Selected = True
            End Select
        End If
        If (details.ApplicationTypeID = Model.Enums.ApplicationType.Renewal) Then
            pnlRenewal.Visible = True
            Select Case details.RoleCategoryLevelID.ToString()
                Case rdbRenewal.Items(0).Value
                    rdbRenewal.Items(0).Enabled = True
                    rdbRenewal.Items(0).Selected = True
                Case rdbRenewal.Items(1).Value
                    rdbRenewal.Items(1).Enabled = True
                    rdbRenewal.Items(1).Selected = True
                Case rdbRenewal.Items(2).Value
                    rdbRenewal.Items(2).Enabled = True
                    rdbRenewal.Items(2).Selected = True
                Case rdbRenewal.Items(3).Value
                    rdbRenewal.Items(3).Enabled = True
                    rdbRenewal.Items(3).Selected = True
                Case rdbRenewal.Items(4).Value
                    rdbRenewal.Items(4).Enabled = True
                    rdbRenewal.Items(4).Selected = True
                Case rdbRenewal.Items(5).Value
                    rdbRenewal.Items(5).Enabled = True
                    rdbRenewal.Items(5).Selected = True
                Case rdbRenewal.Items(6).Value
                    rdbRenewal.Items(6).Enabled = True
                    rdbRenewal.Items(6).Selected = True
            End Select
        End If
        If (details.ApplicationTypeID = Model.Enums.ApplicationType.AddOn) Then
            pnlAddOn.Visible = True
            Select Case details.RoleCategoryLevelID.ToString()
                Case rdbAddOn.Items(0).Value
                    rdbAddOn.Items(0).Enabled = True
                    rdbAddOn.Items(0).Selected = True
                Case rdbAddOn.Items(1).Value
                    rdbAddOn.Items(1).Enabled = True
                    rdbAddOn.Items(1).Selected = True
                Case rdbAddOn.Items(2).Value
                    rdbAddOn.Items(2).Enabled = True
                    rdbAddOn.Items(2).Selected = True
                Case rdbAddOn.Items(3).Value
                    rdbAddOn.Items(3).Enabled = True
                    rdbAddOn.Items(3).Selected = True
                Case rdbAddOn.Items(4).Value
                    rdbAddOn.Items(4).Enabled = True
                    rdbAddOn.Items(4).Selected = True
                Case _rnDODDRolesCat2
                    rdbAddOn.Items(5).Enabled = True
                    rdbAddOn.Items(5).Selected = True
                Case rdbAddOn.Items(6).Value
                    rdbAddOn.Items(6).Enabled = True
                    rdbAddOn.Items(6).Selected = True
            End Select
        End If
        If (details.ApplicationTypeID = Model.Enums.ApplicationType.UpdateProfile) Then
            pnlUpdate.Visible = True
            rdbUpdate.Items(0).Enabled = True
            rdbUpdate.Items(0).Selected = True
        End If
    End Sub
    Private Sub DefaultGridOnPageLoad(ByVal applicationType As String, ByVal startPagesvc As IStartPageService)
        If (applicationType <> EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn)) Then
            If (hdRNorDDRole.Value <> "0") Then
                certificationDetails = startPagesvc.GetCertificationDetails(Convert.ToInt32(hdRNorDDRole.Value), Convert.ToInt32(hdRNorDDRole.Value), applicationType)
            Else
                If (RNorDD) Then
                    certificationDetails = startPagesvc.GetCertificationDetails(_rntrainerRoles, _rntrainerRoles, applicationType)
                Else
                    certificationDetails = startPagesvc.GetCertificationDetails(_rnDODDRoles, _rnDODDRoles, applicationType)
                End If
            End If
        End If
        If (applicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn)) Then
            'If (SessionHelper.SelectedUserRole = 0) Then
            Select Case Convert.ToInt32(hdRNorDDRole.Value)
                Case _rntrainerRoles
                    certificationDetails = startPagesvc.GetCertificationDetails(Convert.ToInt32(hdRNorDDRole.Value), RoleLevelCategory.RNInstructor_RLC, applicationType)
                Case _rnDODDRoles
                    certificationDetails = startPagesvc.GetCertificationDetails(Convert.ToInt32(hdRNorDDRole.Value), RoleLevelCategory.DDPersonnel2_RLC, applicationType)
                Case _rnDODDRolesCat2
                    certificationDetails = startPagesvc.GetCertificationDetails(RoleLevelCategory.DDPersonnel_RLC, RoleLevelCategory.DDPersonnel2_RLC, applicationType)
                Case _rnDODDRolesCat3
                    certificationDetails = startPagesvc.GetCertificationDetails(RoleLevelCategory.DDPersonnel_RLC, RoleLevelCategory.DDPersonnel3_RLC, applicationType)
                Case _rnICFRoles
                    certificationDetails = startPagesvc.GetCertificationDetails(Convert.ToInt32(hdRNorDDRole.Value), RoleLevelCategory.RNTrainer_RLC, applicationType)
                Case _rnQARoles
                    certificationDetails = startPagesvc.GetCertificationDetails(Convert.ToInt32(hdRNorDDRole.Value), RoleLevelCategory.RNTrainer_RLC, applicationType)
                Case _rnInstructorRoles
                    certificationDetails = startPagesvc.GetCertificationDetails(Convert.ToInt32(hdRNorDDRole.Value), RoleLevelCategory.RNMaster_RLC, applicationType)
                Case _rnMasterRoles
                    certificationDetails = startPagesvc.GetCertificationDetails(Convert.ToInt32(hdRNorDDRole.Value), RoleLevelCategory.QARN_RLC, applicationType)
            End Select
            'Else
            'certificationDetails = startPagesvc.GetCertificationDetails(Convert.ToInt32(hdRNorDDRole.Value), SessionHelper.SelectedUserRole, applicationType)
            'End If
        End If
        If (Not String.IsNullOrEmpty(certificationDetails.CertificationType)) Then
            startPage.Disabled = False
            Dim row As HtmlTableRow = New HtmlTableRow()
            Dim cell As HtmlTableCell = New HtmlTableCell()
            cell.InnerText = certificationDetails.CertificationType
            row.Cells.Add(cell)
            Dim cellApp As HtmlTableCell = New HtmlTableCell()
            cellApp.InnerText = applicationType
            row.Cells.Add(cellApp)
            If (Not String.IsNullOrEmpty(certificationDetails.InitialCertificationReq)) Then
                Dim cell1 As HtmlTableCell = New HtmlTableCell()
                cell1.InnerText = certificationDetails.InitialCertificationReq
                row.Cells.Add(cell1)
            End If
            If (Not String.IsNullOrEmpty(certificationDetails.RenewalCertificationReq)) Then
                Dim cell2 As HtmlTableCell = New HtmlTableCell()
                cell2.InnerText = certificationDetails.RenewalCertificationReq
                row.Cells.Add(cell2)
            End If
            If (Not String.IsNullOrEmpty(certificationDetails.AddOnCertificationReq)) Then
                Dim cell3 As HtmlTableCell = New HtmlTableCell()
                cell3.InnerText = certificationDetails.AddOnCertificationReq
                row.Cells.Add(cell3)
            End If
            startPage.Rows.Add(row)
        Else
            startPage.Disabled = True
        End If
    End Sub
    Private Sub EnableTheControls(ByVal role As Integer, ByVal applicationType As String)
        If (applicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.Initial)) Then
            pnlInitial.Visible = True
            lblInitial.Visible = True
            If (UserAndRoleHelper.IsUserSecretary = False) Then
                Select Case role
                    Case _rntrainerRoles
                        rdbInitial.Items(0).Enabled = True
                    Case _rnQARoles
                        rdbInitial.Items(1).Enabled = True
                    Case _rnICFRoles
                        rdbInitial.Items(2).Enabled = True
                    Case _rnDODDRoles
                        rdbInitial.Items(3).Enabled = True
                    Case _rnDODDRolesCat2
                        rdbInitial.Items(3).Enabled = True
                    Case _rnDODDRolesCat3
                        rdbInitial.Items(3).Enabled = True
                        'Case _rnSuperVisorRoles
                        '    rdbInitial.Items(4).Enabled = True
                        'Case _rnInstructorRoles
                        '    rdbInitial.Items(0).Enabled = True
                        '    rdbInitial.Items(1).Enabled = True
                        '    rdbInitial.Items(2).Enabled = True
                        'Case _rnMasterRoles 'rn master too
                        '    rdbInitial.Items(0).Enabled = True
                        '    rdbInitial.Items(1).Enabled = True
                        '    rdbInitial.Items(2).Enabled = True

                End Select
            Else
                rdbInitial.Items(0).Enabled = True
                rdbInitial.Items(1).Enabled = True
            End If
        End If
        If (applicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.Renewal)) Then
            pnlRenewal.Visible = True
            lblRenewal.Visible = True
            Select Case role
                Case _rntrainerRoles
                    rdbRenewal.Items(0).Enabled = True
                Case _rnInstructorRoles
                    If (UserAndRoleHelper.IsUserAdmin Or SessionHelper.MAISLevelUserRole = RoleLevelCategory.RNMaster_RLC) Then
                        rdbRenewal.Items(1).Enabled = True
                    End If
                Case _rnQARoles
                    rdbRenewal.Items(2).Enabled = True
                Case _rnICFRoles
                    rdbRenewal.Items(3).Enabled = True
                Case _rnDODDRoles
                    rdbRenewal.Items(4).Enabled = True
                    'rdbRenewal.Items(4).Enabled = True
                    'rdbRenewal.Items(5).Enabled = True
                Case _rnDODDRolesCat2
                    rdbRenewal.Items(5).Enabled = True
                Case _rnDODDRolesCat3
                    rdbRenewal.Items(6).Enabled = True
                    'Case _rnInstructorRoles
                    '    rdbInitial.Items(0).Enabled = True
                    '    rdbInitial.Items(1).Enabled = True
                    '    rdbInitial.Items(2).Enabled = True
                Case _rnMasterRoles 'rn master too
                    If (UserAndRoleHelper.IsUserAdmin) Then
                        rdbRenewal.Items(7).Enabled = True
                        'rdbInitial.Items(1).Enabled = True
                        'rdbInitial.Items(2).Enabled = True
                    End If
            End Select
        End If
        If (applicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn)) Then
            pnlAddOn.Visible = True
            lblAddOn.Visible = True
            If (String.IsNullOrEmpty(querystring)) Then
                Select Case role
                    Case _rntrainerRoles
                        rdbAddOn.Items(3).Enabled = True
                        If (SessionHelper.MAISLevelUserRole = _rnMasterRoles Or UserAndRoleHelper.IsUserAdmin Or SessionHelper.MAISLevelUserRole = _rnInstructorRoles) Then
                            rdbAddOn.Items(4).Enabled = True
                        End If
                        If (SessionHelper.MAISLevelUserRole = _rnMasterRoles Or UserAndRoleHelper.IsUserAdmin) Then
                            rdbAddOn.Items(1).Enabled = True
                        End If
                    Case _rnInstructorRoles
                        rdbAddOn.Items(3).Enabled = True
                        rdbAddOn.Items(4).Enabled = True
                        If (UserAndRoleHelper.IsUserAdmin Or SessionHelper.MAISLevelUserRole = _rnMasterRoles) Then
                            rdbAddOn.Items(2).Enabled = True
                        End If
                    Case _rnMasterRoles
                        rdbAddOn.Items(3).Enabled = True
                        rdbAddOn.Items(4).Enabled = True
                    Case _rnQARoles
                        If (SessionHelper.MAISLevelUserRole = _rnMasterRoles Or SessionHelper.MAISLevelUserRole = _rnInstructorRoles Or UserAndRoleHelper.IsUserAdmin) Then
                            rdbAddOn.Items(0).Enabled = True
                            rdbAddOn.Items(4).Enabled = True
                        ElseIf (SessionHelper.MAISLevelUserRole = _rntrainerRoles Or SessionHelper.MAISLevelUserRole = _rnICFRoles) Then
                            rdbAddOn.Items(4).Enabled = True
                        End If
                    Case _rnICFRoles
                        If (SessionHelper.MAISLevelUserRole = _rnMasterRoles Or SessionHelper.MAISLevelUserRole = _rnInstructorRoles Or UserAndRoleHelper.IsUserAdmin) Then
                            rdbAddOn.Items(0).Enabled = True
                        End If
                        rdbAddOn.Items(3).Enabled = True
                    Case _rnDODDRoles
                        rdbAddOn.Items(5).Enabled = True
                        rdbAddOn.Items(6).Enabled = True
                    Case _rnDODDRolesCat2
                        rdbAddOn.Items(6).Enabled = True
                End Select
            Else
                Select Case role
                    Case _rntrainerRoles
                        If ((SessionHelper.MAISLevelUserRole = _rnMasterRoles Or UserAndRoleHelper.IsUserAdmin) And Not querystring.ToString().Contains(_rnInstructorRoles)) Then
                            rdbAddOn.Items(1).Enabled = True
                        End If
                        If (Not querystring.ToString().Contains(_rnQARoles)) Then
                            rdbAddOn.Items(3).Enabled = True
                        End If
                        If (Not querystring.ToString().Contains(_rnICFRoles)) Then
                            rdbAddOn.Items(4).Enabled = True
                        End If
                    Case _rnInstructorRoles
                        If (Not querystring.ToString().Contains(_rnMasterRoles) Or (UserAndRoleHelper.IsUserAdmin)) Then
                            rdbAddOn.Items(2).Enabled = True
                        End If
                        If (Not querystring.ToString().Contains(_rnQARoles)) Then
                            rdbAddOn.Items(3).Enabled = True
                        End If
                        If (Not querystring.ToString().Contains(_rnICFRoles)) Then
                            rdbAddOn.Items(4).Enabled = True
                        End If
                    Case _rnMasterRoles
                        If (Not querystring.ToString().Contains(_rnQARoles)) Then
                            rdbAddOn.Items(3).Enabled = True
                        End If
                        If (Not querystring.ToString().Contains(_rnICFRoles)) Then
                            rdbAddOn.Items(4).Enabled = True
                        End If
                    Case _rnQARoles
                        If (Not querystring.ToString().Contains(_rntrainerRoles) And (SessionHelper.MAISLevelUserRole = _rnMasterRoles Or SessionHelper.MAISLevelUserRole = _rnInstructorRoles Or UserAndRoleHelper.IsUserAdmin)) Then
                            rdbAddOn.Items(0).Enabled = True
                            rdbAddOn.Items(4).Enabled = True
                        End If
                        If (Not querystring.ToString().Contains(_rnICFRoles) And (SessionHelper.MAISLevelUserRole = _rntrainerRoles Or SessionHelper.MAISLevelUserRole = _rnICFRoles)) Then
                            rdbAddOn.Items(4).Enabled = True
                        End If
                    Case _rnICFRoles
                        If (Not querystring.ToString().Contains(_rntrainerRoles) And (SessionHelper.MAISLevelUserRole = _rnMasterRoles Or SessionHelper.MAISLevelUserRole = _rnInstructorRoles Or UserAndRoleHelper.IsUserAdmin)) Then
                            rdbAddOn.Items(0).Enabled = True
                        End If
                        If (Not querystring.ToString().Contains(_rnQARoles)) Then
                            rdbAddOn.Items(3).Enabled = True
                        End If
                    Case _rnDODDRoles
                        If (Not querystring.ToString().Contains(_rnDODDRolesCat2)) Then
                            rdbAddOn.Items(5).Enabled = True
                        End If
                        If (Not querystring.ToString().Contains(_rnDODDRolesCat3)) Then
                            rdbAddOn.Items(6).Enabled = True
                        End If
                    Case _rnDODDRolesCat2
                        If (Not querystring.ToString().Contains(_rnDODDRolesCat3)) Then
                            rdbAddOn.Items(6).Enabled = True
                        End If
                End Select
            End If
        End If
        If (applicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.UpdateProfile)) Then
            pnlUpdate.Visible = True
            pnlUpdate.Visible = True
            rdbUpdate.Items(0).Enabled = True
        End If
    End Sub
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function GetCertificationDetails(ByVal getCertInfo As Dictionary(Of String, String)) As Object
        Dim jsonOutput As Object = DBNull.Value
        Dim certificationDetails As New CertificationEligibleInfo
        Dim applicationType As String = SessionHelper.ApplicationType
        Dim currentRole As Integer = 0
        Dim role As String = getCertInfo("role")
        Dim originalRole As Integer = 0
        'originalRole = 
        If (getCertInfo.ContainsKey("currentRoleInitial")) Then
            If (applicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.Initial)) Then
                Dim initialRole As Integer = getCertInfo("currentRoleInitial")
                currentRole = Convert.ToInt32(initialRole)
                role = currentRole
            End If
        End If
        If (getCertInfo.ContainsKey("currentRoleAddOn")) Then
            Dim addOnRole As Integer = getCertInfo("currentRoleAddOn")
            currentRole = Convert.ToInt32(addOnRole)
        End If
        If (getCertInfo.ContainsKey("currentRoleRenewal")) Then
            If (applicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.Renewal)) Then
                Dim renewalRole As Integer = getCertInfo("currentRoleRenewal")
                currentRole = Convert.ToInt32(renewalRole)
                role = currentRole
            End If
        End If
        Dim startPageSvc As IStartPageService = StructureMap.ObjectFactory.GetInstance(Of IStartPageService)()
        certificationDetails = startPageSvc.GetCertificationDetails(role, currentRole, applicationType)
        Dim inititalCert As String = String.Empty
        Dim RenewalCert As String = String.Empty
        Dim AddonCert As String = String.Empty
        If (Not String.IsNullOrEmpty(certificationDetails.InitialCertificationReq)) Then
            inititalCert = certificationDetails.InitialCertificationReq.Trim()
        End If
        If (Not String.IsNullOrEmpty(certificationDetails.RenewalCertificationReq)) Then
            RenewalCert = certificationDetails.RenewalCertificationReq.Trim()
        End If
        If (Not String.IsNullOrEmpty(certificationDetails.AddOnCertificationReq)) Then
            AddonCert = certificationDetails.AddOnCertificationReq.Trim()
        End If
        If (certificationDetails IsNot Nothing) Then
            jsonOutput = New With {
                .certificateType = certificationDetails.CertificationType,
                .initialCertDescription = inititalCert,
                .renewalCertDescription = RenewalCert,
                .addonCertDescription = AddonCert,
                .applicationType = applicationType.Trim()
                }
        End If
        Return jsonOutput
    End Function
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Sub SaveStartPageInformation(ByVal saveInfo As Dictionary(Of String, String))
        Dim _maisInfo As New MAISApplicationDetails
        If (Not String.IsNullOrEmpty(saveInfo("ApplicationID"))) Then
            _maisInfo.ApplicationID = saveInfo("ApplicationID")
        End If
        _maisInfo.ApplicationStatusTypeID = ApplicationStatusType.Pending
        _maisInfo.DDPersonnelRNID = SessionHelper.SessionUniqueID
        If (SessionHelper.ApplicationType <> "Update Profile") Then
            _maisInfo.ApplicationTypeID = DirectCast([Enum].Parse(GetType(ApplicationType), SessionHelper.ApplicationType), ApplicationType) '[Enum].GetValues(SessionHelper.ApplicationType ApplicationType.Initial
        Else
            _maisInfo.ApplicationTypeID = 4
        End If
        _maisInfo.RNFlag = SessionHelper.RN_Flg
        If (SessionHelper.ApplicationType <> EnumHelper.GetEnumDescription(ApplicationType.UpdateProfile)) Then
            If (saveInfo.ContainsKey("currentRoleInitial")) Then
                If (Not String.IsNullOrEmpty(saveInfo("currentRoleInitial"))) Then
                    _maisInfo.RoleCategoryLevelID = Convert.ToInt32(saveInfo("currentRoleInitial"))
                End If
            ElseIf (saveInfo.ContainsKey("currentRoleAddOn")) Then
                _maisInfo.RoleCategoryLevelID = Convert.ToInt32(saveInfo("currentRoleAddOn"))
            ElseIf (saveInfo.ContainsKey("currentRoleRenewal")) Then
                _maisInfo.RoleCategoryLevelID = Convert.ToInt32(saveInfo("currentRoleRenewal"))
            End If
        Else
            _maisInfo.RoleCategoryLevelID = SessionHelper.ExistingUserRole
        End If
        Dim startSvc As IStartPageService = StructureMap.ObjectFactory.GetInstance(Of IStartPageService)()
        Dim retObj As Integer = startSvc.SaveAppInfo(_maisInfo)
        If (SessionHelper.ApplicationID = 0) Then
            SessionHelper.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.Pending)
        End If
        SessionHelper.ApplicationID = retObj
        If (SessionHelper.ApplicationID > 0) Then
            SessionHelper.SelectedUserRole = _maisInfo.RoleCategoryLevelID
        End If
    End Sub
    Public Shared Property RNorDD As Boolean
        Private Get
            Return _rnorDD
        End Get
        Set(ByVal value As Boolean)
            _rnorDD = value
        End Set
    End Property
    Public Shared Property Application_Type As String
        Private Get
            Return _appType
        End Get
        Set(ByVal value As String)
            _appType = value
        End Set
    End Property
End Class