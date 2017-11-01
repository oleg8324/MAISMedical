Imports ODMRDD_NET2
Imports System.DirectoryServices
Imports MAIS.Business
Imports MAIS.Business.Helpers
Imports MAIS.Business.Model.Enums
Imports MAIS.Business.Services

Partial Class MasterPage
    Inherits System.Web.UI.MasterPage

    Public Const sDisplayWidth As String = "850px"
    Private _currentPage As String = ""
    Private _EmailOfAgency As String
    Private _PhoneOfAgency As String
    Private _SecondaryEmail As String
    Private _hasErrors As Boolean = False
    Public Property HasErrors As Boolean
        Get
            Return _hasErrors
        End Get
        Private Set(value As Boolean)
            _hasErrors = value
        End Set
    End Property

    Public Sub New()
        _currentPage = System.IO.Path.GetFileName(HttpContext.Current.Request.FilePath)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub
    'Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
    '    progBar.PageName = Me.CurrentPage
    'End Sub
    Private Sub lnkCertifyRegister_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCertifyRegister.ServerClick
        Response.Redirect("Search.aspx")
    End Sub
    Private Sub lnkRNMapping_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRNMapping.ServerClick
        Response.Redirect("RNMappingUnmapping.aspx")
    End Sub
    Private Sub lnkSecretary_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSecretary.ServerClick
        Response.Redirect("SecretaryAssociation.aspx")
    End Sub
    'Private Sub lnkCertificateModification_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCertificateModification.ServerClick
    '    Response.Redirect("AdminEditCourse.aspx")
    'End Sub
    'Private Sub lnkEditCourse_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditCourse.ServerClick
    '    Response.Redirect("AdminEditCourse.aspx")
    'End Sub
    Private Sub lnkReports_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkReports.ServerClick
        Response.Redirect("MAIS_Reports.aspx")
    End Sub
    Private Sub lnkResource_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkResource.ServerClick
        Response.Redirect("Resources.aspx")
    End Sub
    Private Sub lnkViewPrintDocuments_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkViewPrintDocuments.ServerClick
        Response.Redirect("ViewPrintDocments.aspx")
    End Sub
    Public Sub lnkManageCourses_ServierClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkManageCourses.ServerClick
        Response.Redirect("ManageCourses.aspx")
    End Sub
    Public Sub lnkApplicationHistoryReport_ServierClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkApplicationHistoryReport.ServerClick
        Response.Redirect("ApplicationHistoryReport.aspx")
    End Sub

    Public Sub lnkUpdateMyProfile_ServierClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkUpdateMyProfile.ServerClick
        SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.UpdateProfile)
        SessionHelper.ApplicationStatus = String.Empty
        Response.Redirect("StartPage.aspx")
    End Sub
    Private Sub lnkMessages_ServerClick(sender As Object, e As EventArgs) Handles lnkMessages.ServerClick
        SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.UpdateProfile)
        SessionHelper.ApplicationStatus = String.Empty
        Response.Redirect("maisDODD_Message.aspx")

    End Sub
    Public Sub lnkOldMASystem_ServierClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkOldMASystem.ServerClick
        Dim oldMA As String = ConfigHelper.OldMA & "?" & "GUID=" & HttpContext.Current.Session("GUID")
        Dim redirect As String = "<script type='text/javascript'>window.open('" & oldMA & "','_blank','height=750,width=1100,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes')</script>"
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "OpenWindow", redirect, False)
    End Sub
    Public Sub lknManageCourseAdminClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkManageCourseAdmin.ServerClick
        SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.UpdateProfile)
        SessionHelper.ApplicationStatus = String.Empty
        Response.Redirect("ManageCourseAdmin.aspx")
    End Sub
    Public ReadOnly Property CurrentPage As String
        Get
            Return _currentPage
        End Get
    End Property
    Private Sub VerifyAccess()
        Try
            If UserAndRoleHelper.CurrentUser Is Nothing AndAlso MAIS_Helper.GetUser() Is Nothing Then
                If Trim(CStr(Request.QueryString("GUID"))) <> String.Empty Then
                    MAIS_Helper.SetGUID(Trim(CStr(Request.QueryString("GUID"))))
                Else
                    MAIS_Helper.LogOut()
                End If
            End If

            If UserAndRoleHelper.CurrentUser Is Nothing Then
                MAIS_Helper.LogOut()
            Else
                If CurrentPage.ToLower().Equals("mais_home.aspx") Then
                    SessionHelper.MAISUserID = UserAndRoleHelper.CurrentUser.UserID
                End If

            End If
        Catch iex As InvalidOperationException
            Response.Redirect("MAISLogout.aspx?reason=" & iex.Message)
        Catch ex As Exception
            SetError("MasterPage.init(): " & ex.Message, True)
        End Try

    End Sub

#Region "ErrorMessaging"
    Public Sub SetError(ByVal ErrorText As String, Optional ByVal isWarning As Boolean = False)
        Me.HideError()
        If Not Trim(ErrorText) = String.Empty Then
            If Not Trim(Me.div_MessagesContent.InnerHtml) = String.Empty Then
                Me.div_MessagesContent.InnerHtml += "<br/>" + "&nbsp" + ErrorText
            Else
                Me.div_MessagesContent.InnerHtml = "&nbsp" + ErrorText
            End If

            If isWarning Then
                Me.div_MessagesContent.Attributes.Remove("class")
                Me.div_MessagesContent.Attributes.Add("class", "alert")
            End If

            Me.ShowError()
        End If
    End Sub

    Public Sub ShowError()
        Me.Error_Message.Style("DISPLAY") = "block"
    End Sub

    Public Sub HideError()
        Me.div_MessagesContent.InnerHtml = String.Empty
        Me.Error_Message.Style("DISPLAY") = "none"
    End Sub

    Public Sub CheckForErrorMessages(ByVal errorMessage As System.Collections.Generic.List(Of ODMRDDHelperClassLibrary.Utility.ReturnMessage))

        If errorMessage IsNot Nothing Then
            If errorMessage.Count > 0 Then
                For Each message In errorMessage
                    If message.IsError Then
                        SetError(message.ToString(), False)
                        HasErrors = True
                    ElseIf message.IsWarning Then
                        SetError(message.ToString(), True)
                    Else
                        SetError(message.ToString(), False)
                    End If
                Next
                errorMessage.Clear()
            End If
        End If

    End Sub
#End Region
    Public Function getAppType(ByVal at As String, ByVal rcl As Integer) As String
        'Dim rcldet As Model.RoleCategoryLevelDetails = Msvc.GetRoleCategoryLevelInfoByRoleCategoryLevelSid(rcl)
        Dim udstype As String = ""
        If at = "AddOn" Then
            at = "ADD-ON"
        End If
        Select Case rcl
            Case RoleLevelCategory.DDPersonnel_RLC
                udstype = EnumHelper.GetEnumDescription(RoleLevelCategory.DDPersonnel_RLC) & " " & at.ToUpper
            Case RoleLevelCategory.DDPersonnel2_RLC
                udstype = EnumHelper.GetEnumDescription(RoleLevelCategory.DDPersonnel2_RLC) & " " & at.ToUpper
            Case RoleLevelCategory.DDPersonnel3_RLC
                udstype = EnumHelper.GetEnumDescription(RoleLevelCategory.DDPersonnel3_RLC) & " " & at.ToUpper
            Case RoleLevelCategory.Bed17_RLC
                udstype = EnumHelper.GetEnumDescription(RoleLevelCategory.Bed17_RLC) & " " & at.ToUpper
            Case RoleLevelCategory.RNMaster_RLC
                udstype = EnumHelper.GetEnumDescription(RoleLevelCategory.RNMaster_RLC) & " " & at.ToUpper
            Case RoleLevelCategory.RNTrainer_RLC
                udstype = EnumHelper.GetEnumDescription(RoleLevelCategory.RNTrainer_RLC) & " " & at.ToUpper
            Case RoleLevelCategory.RNInstructor_RLC
                udstype = EnumHelper.GetEnumDescription(RoleLevelCategory.RNInstructor_RLC) & " " & at.ToUpper
            Case RoleLevelCategory.QARN_RLC
                udstype = EnumHelper.GetEnumDescription(RoleLevelCategory.QARN_RLC) & " " & at.ToUpper
        End Select
        Return udstype
    End Function

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            '        'If Not IsPostBack Then

            Me.HideError()
            HideUpdateProfileLink = True
            'Dim ABC = MAIS_Helper.GetUser()
            'lblName.Text = ABC.UserID.ToString
            Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
            Dim exists As Integer = maisSvc.CheckTheMandatoryFields(SessionHelper.LoginUsersRNLicense)
            If (UserAndRoleHelper.IsUserRN) Then
                If (exists = 0) Then
                    HideUpdateProfileLink = False
                    HideCertificateLink = True
                    SessionHelper.MyUpdate_Profile = True
                Else
                    HideUpdateProfileLink = True
                    HideCertificateLink = False
                    SessionHelper.MyUpdate_Profile = False
                End If
            End If
            lblApplicationID.Text = SessionHelper.ApplicationID.ToString()

            If (SessionHelper.SessionUniqueID IsNot Nothing) Then
                lblRNDDCode.Text = SessionHelper.SessionUniqueID
            Else
                lblRNDDCode.Text = String.Empty
            End If
            If (SessionHelper.Name IsNot Nothing) Then
                lblName.Text = SessionHelper.Name
                If (SessionHelper.Notation_Flg) Then
                    lblName.ForeColor = Drawing.Color.Red
                Else
                    lblName.ForeColor = Drawing.Color.Green
                End If
            Else
                lblName.Text = String.Empty
            End If
            If (MAIS_Helper.GetUser IsNot Nothing) Then
                Dim user As ODMRDD_NET2.IUser = MAIS_Helper.GetUser
                lblLoginUser.Text = user.UserName
            Else
                lblLoginUser.Text = String.Empty
            End If

            VerifyAccess()
            If (UserAndRoleHelper.IsUserAdmin = False And SessionHelper.MAISLevelUserRole <> RoleLevelCategory.RNMaster_RLC) Then
                menu1.Visible = False
                lnkRNMapping.Visible = False
            Else
                menu1.Visible = True
                lnkRNMapping.Visible = True
            End If
            If (SessionHelper.MAISLevelUserRole = Model.Enums.RoleLevelCategory.RNMaster_RLC) Then
                lnkRNMapping.Visible = True
            End If
            If (UserAndRoleHelper.IsUserAdmin Or UserAndRoleHelper.IsUserRN) Then
                lnkSecretary.Visible = True
            Else
                lnkSecretary.Visible = False
            End If
            Dim portalFunctions As New PortalFunctions(Me.Page)
            If portalFunctions.CheckForPortal() = True Then

            End If
        Catch ex As Exception
            SetError("MasterPage.init(): " & ex.Message, True)
        End Try
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        progBar.PageName = Me.CurrentPage
        Dim appStatus As String = SessionHelper.ApplicationStatus
        If (SessionHelper.ApplicationType IsNot Nothing AndAlso SessionHelper.SelectedUserRole > 0) Then
            lblApptype.Text = getAppType(SessionHelper.ApplicationType, SessionHelper.SelectedUserRole)
        End If
        If (lblApptype.Text = "" OrElse Me.CurrentPage = "UpdateExistingPage.aspx") Then
            lblApptypeLabel.Visible = False
            lblApptype.Visible = False
        Else
            lblApptypeLabel.Visible = True
            lblApptype.Visible = True
        End If

        If (Me.CurrentPage <> "Search.aspx" And Me.CurrentPage <> "UpdateExistingPage.aspx" And Me.CurrentPage <> "mais_home.aspx" And Me.CurrentPage <> "Notation.aspx" And Me.CurrentPage <> "ManageCourses.aspx" _
            And Me.CurrentPage <> "ViewCertificate.aspx" And Me.CurrentPage <> "SecretaryAssociation.aspx" And Me.CurrentPage <> "RNMappingUnmapping.aspx" And Me.CurrentPage <> "ViewPrintDocments.aspx" _
            And Me.CurrentPage <> "MAIS_Reports.aspx" And Me.CurrentPage <> "ApplicationHistoryReport.aspx") Then
            If (Not String.IsNullOrEmpty(SessionHelper.ApplicationStatus)) Then
                If (appStatus <> EnumHelper.GetEnumDescription(ApplicationStatusType.Pending) And appStatus <> EnumHelper.GetEnumDescription(ApplicationStatusType.DODD_Review) _
                    And appStatus <> EnumHelper.GetEnumDescription(ApplicationStatusType.IntentToDeny)) Then
                    DisableControls(mainContent)
                End If
                Select Case ConfigurationManager.AppSettings("ENVIRONMENTTYPE")
                    Case "DEV", "QA", "UAT"
                        'If lblLoginUser.Text.Contains("Steve Nicosia") Then
                        Me.divTestAppID.Visible = True
                        'End If
                    Case Else
                        Me.divTestAppID.Visible = False
                End Select
            End If
            If (UserAndRoleHelper.IsUserReadOnly) Then
                DisableControls(mainContent)
            End If
        Else
            If (Me.CurrentPage = "Notation.aspx" AndAlso UserAndRoleHelper.IsUserReadOnly) Then
                DisableControls(mainContent)
            End If
            If (Me.CurrentPage = "Summary.aspx" AndAlso UserAndRoleHelper.IsUserSecretary) Then
                DisableControls(mainContent)
            End If
            If (Me.CurrentPage = "Notation.aspx" AndAlso SessionHelper.LoginUsersRNLicense = SessionHelper.SessionUniqueID AndAlso (String.IsNullOrEmpty(SessionHelper.SessionUniqueID) = False)) Then
                DisableControls(mainContent)
            End If
        End If

        Me.HasErrors = False
        If (SessionHelper.Notation_Flg) Then
            HideNotationLink = False
        Else
            HideNotationLink = True
        End If
        If (UserAndRoleHelper.IsUserAdmin = False And SessionHelper.MAISLevelUserRole <> RoleLevelCategory.RNMaster_RLC) Then
            menu1.Visible = False
            lnkRNMapping.Visible = False
        Else
            menu1.Visible = True
            lnkRNMapping.Visible = True
        End If

        TestIfApplictionIDChanged()
    End Sub

    Private Sub TestIfApplictionIDChanged()
        If (Me.CurrentPage <> "Search.aspx" And Me.CurrentPage <> "UpdateExistingPage.aspx" And Me.CurrentPage <> "mais_home.aspx" And Me.CurrentPage <> "Notation.aspx" And Me.CurrentPage <> "ManageCourses.aspx" _
        And Me.CurrentPage <> "ViewCertificate.aspx" And Me.CurrentPage <> "SecretaryAssociation.aspx" And Me.CurrentPage <> "RNMappingUnmapping.aspx" And Me.CurrentPage <> "ViewPrintDocments.aspx" _
        And Me.CurrentPage <> "MAIS_Reports.aspx" And Me.CurrentPage <> "ApplicationHistoryReport.aspx" And Me.CurrentPage <> "SummaryReGeneration.aspx") Then
            'If UserSessionMatch = False Then
            Dim misServ As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
            Dim TestValue As Boolean

            TestValue = misServ.UserSessionMatch(SessionHelper.ApplicationID, RNLicenseOrSSN, ApplicationID)
            If TestValue = False Then

                Dim srvApp As IApplicationDetailInformationService = StructureMap.ObjectFactory.GetInstance(Of IApplicationDetailInformationService)()
                Dim appdata = srvApp.GetApplicationInfromationByAppID(ApplicationID)
                SessionHelper.ApplicationID = appdata.Application_SID
                SessionHelper.SessionUniqueID = appdata.RNDDUnique_Code

                Response.Redirect(CurrentPage)

                'Dim Path = System.IO.Path.GetFullPath(HttpContext.Current.Request.FilePath)
                'Dim message As String = "The application ID " & ApplicationID & " does not match with the RNLicense# or DDPersonnelCode. Please stop and return to search page."
                'Dim AlertMessage As String = "An error has accord in the system. You will be returned to the Search page. \n \n Admin: \n on Page change to " + Me.CurrentPage.ToString
                'SetError(message, False)

                'Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()

                'sb.Append("<script type = 'text/javascript'>")

                'sb.Append("window.onload=function(){")

                'sb.Append("alert('")

                'sb.Append(AlertMessage)

                'sb.Append("'); window.location='Search.aspx' };")

                'sb.Append("</script>")

                'Page.ClientScript.RegisterStartupScript(Page.GetType(), "ShowAlert", sb.ToString)


                'Response.Redirect("Search.aspx")
            End If


        End If
        'End If
    End Sub

    Public Sub DisableControls(ByVal c As Control)

        If (TypeOf c Is TextBox) OrElse (TypeOf c Is RadioButton) OrElse (TypeOf c Is RadioButtonList) OrElse (TypeOf c Is CheckBox) OrElse (TypeOf c Is Button) OrElse (TypeOf c Is GridView) OrElse (TypeOf c Is CheckBoxList) _
            OrElse (TypeOf c Is DropDownList) OrElse (TypeOf c Is FileUpload) Then
            CType(c, WebControl).Enabled = False
        End If

        If (TypeOf c Is HtmlInputControl) OrElse (TypeOf c Is HtmlInputButton) OrElse (TypeOf c Is HtmlInputHidden) Then
            CType(c, HtmlControl).Disabled = True
        End If
        For Each child As Control In c.Controls
            DisableControls(child)
        Next
    End Sub

    Public Property HideLink As Boolean
        Get
            Return Not pnlNew.Visible()
        End Get
        Set(ByVal value As Boolean)
            pnlNew.Visible = Not value
        End Set
    End Property
    Public Property HideNotationLink As Boolean
        Get
            Return Not lblNotation.Visible()
        End Get
        Set(ByVal value As Boolean)
            lblNotation.Visible = Not value
        End Set
    End Property
    Public Property HideOtherLink As Boolean
        Get
            Return Not pnlOther.Visible()
        End Get
        Set(ByVal value As Boolean)
            pnlOther.Visible = Not value
        End Set
    End Property
    Public Property ApplicationID As String
        Get
            Return lblApplicationID.Text
        End Get
        Set(ByVal value As String)
            lblApplicationID.Text = value
        End Set
    End Property


    Public Property HideProgressBar As Boolean
        Get
            Return Not pnlProgressBar.Visible
        End Get
        Set(ByVal value As Boolean)
            pnlProgressBar.Visible = Not value
        End Set
    End Property
    Public Property HideCertificateLink As Boolean
        Get
            Return Not menu2.Visible
        End Get
        Set(ByVal value As Boolean)
            menu2.Visible = Not value
        End Set
    End Property
    Public Property HideUpdateProfileLink As Boolean
        Get
            Return Not ulProfile.Visible
        End Get
        Set(ByVal value As Boolean)
            ulProfile.Visible = Not value
        End Set
    End Property
    Public Property RNLicenseOrSSN As String
        Get
            Return lblRNDDCode.Text
        End Get
        Set(ByVal value As String)
            lblRNDDCode.Text = value
        End Set
    End Property

    Protected Sub lbtnHome_Click(sender As Object, e As EventArgs) Handles lbtnHome.Click
        Response.Redirect("mais_home.aspx")
    End Sub


    Public ReadOnly Property UserSessionMatch(Optional ByVal extraMessage As String = "") As Boolean
        Get
            Dim misServ As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
            Dim TestValue As Boolean
            'Dim ShowAlert As Boolean = False

            TestValue = misServ.UserSessionMatch(SessionHelper.ApplicationID, RNLicenseOrSSN, ApplicationID)
            If TestValue = False Then
                'Dim srvApp As IApplicationDetailInformationService = StructureMap.ObjectFactory.GetInstance(Of IApplicationDetailInformationService)()
                'Dim appdata = srvApp.GetApplicationInfromationByAppID(ApplicationID)
                'SessionHelper.ApplicationID = appdata.Application_SID
                'SessionHelper.SessionUniqueID = appdata.RNDDUnique_Code

                'Response.Redirect(CurrentPage)

                'System.IO.Path.GetFileName(HttpContext.Current.Request.FilePath)
                Dim Path = System.IO.Path.GetFullPath(HttpContext.Current.Request.FilePath)
                Dim message As String = "The application ID " & ApplicationID & " does not match with the RNLicense# or DDPersonnelCode. Please stop and return to search page."
                SetError(message, False)

                Dim AlertMessage As String
                If String.IsNullOrWhiteSpace(extraMessage) = False Then
                    AlertMessage = "An error has accord in the system. \n You will be returned to the Search page. \n \n Addmin: \n" + extraMessage
                Else
                    AlertMessage = "An error has accord in the system. You will be returned to the Search page. "
                End If
                Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()

                sb.Append("<script type = 'text/javascript'>")

                sb.Append("window.onload=function(){")

                sb.Append("alert('")

                sb.Append(AlertMessage)

                sb.Append("');  window.location='Search.aspx' };")

                sb.Append("</script>")

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ShowAlert", sb.ToString)
                'Response.Redirect("Search.aspx")
                Return TestValue
            Else
                SetError("", False)
                Return TestValue
            End If
        End Get

    End Property



    Protected Sub bntChangeAppID_Click(sender As Object, e As EventArgs) Handles bntChangeAppID.Click
        If txtAppIDChange.Text <> String.Empty Then
            If IsNumeric(txtAppIDChange.Text) Then
                SessionHelper.ApplicationID = CInt(txtAppIDChange.Text)
            End If
        End If
    End Sub
End Class

