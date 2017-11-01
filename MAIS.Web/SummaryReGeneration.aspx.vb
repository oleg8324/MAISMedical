Imports ODMRDDHelperClassLibrary
Imports MAIS.Business.Model
Imports MAIS.Data
Imports MAIS.Data.Objects
Imports MAIS.Business.Services
Imports MAIS.Business.Model.Enums
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Helpers
Imports Microsoft.Reporting.WebForms
Imports System.Security
Imports System.Security.Permissions
Imports MAIS.Business
Imports ODMRDD_NET2
Imports System.IO

Public Class SummaryReGeneration
    Inherits System.Web.UI.Page
    Private Shared _appID As Integer
    Private Shared _sessionId As String
    Private Shared _rnorDD As String
    Private Shared _ApplicationType As String
    Private Shared _SelectedUserRole As Integer
    Private Shared PISvc As IPersonalInformationService
    Private Shared Msvc As IMAISSerivce
    Private Shared SumSvc As ISummaryService
    'Private Shared query1 As Data.Queries.SummaryQueries
    'Private Shared toPermQuery As Data.Queries.MoveTempToPermQueries
    Private Shared SaveToPermSvc As IMoveTempToPermService
    Private Shared uploadSvc As IUploadService
    Private Shared NotSvc As INotationService
    'Private Shared piquery As Data.Queries.PersonalInformationQueries
    'Public Shared pdetails As Objects.PersonalInformationDetails
    'Public Shared conts As List(Of Objects.Contact)
    Public Shared certHist As List(Of Business.Model.Certificate)
    Private Shared wsUDS As UDSWebService.UDSService
    Public Shared welist As List(Of Business.Model.WorkExperienceDetails)
    Public Shared emplist As List(Of Business.Model.EmployerInformationDetails)
    Dim _rntrainer As Integer = RoleLevelCategory.RNTrainer_RLC
    Dim _roles As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SaveToPermSvc = StructureMap.ObjectFactory.GetInstance(Of IMoveTempToPermService)()
        NotSvc = StructureMap.ObjectFactory.GetInstance(Of INotationService)()
        Msvc = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        uploadSvc = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
        SumSvc = StructureMap.ObjectFactory.GetInstance(Of ISummaryService)()
        If SessionHelper.RN_Flg Then
            lblRNLNoOrSSN.Text = "RN License No.:"
            lblDtIssuedOrDOB.Text = "Date of original issuance:"
        Else
            lblRNLNoOrSSN.Text = "Last4SSN:"
            lblDtIssuedOrDOB.Text = "DOB:"
        End If
        If Not IsPostBack Then
            'pnote.Style("display") = "none"
            pnote.Visible = False
            dWE.Style("display") = "none"
            dPrint.Style("display") = "none"
            dEmployers.Style("display") = "none"

            dBtnWE.Style("display") = "none"
            pDocUpload.Style("display") = "none"
            divSkillGrid.Style("display") = "none"
            divCourse.Style("display") = "none"
            dAttestations.Style("display") = "none"
            'If SessionHelper.ApplicationType = "Update Profile" Then
            '    dActions.Style("display") = "none"
            'End If
            'Dim notShowErr As Boolean = False
            Dim rte As ReturnObject(Of Long)
            If Request.QueryString("newwin") <> Nothing Then

                'If Master.UserSessionMatch("At Summary Page Line 655 before the save to Perment tables") = False Then
                'rte = Msvc.SendToErrorLog("At Summary Page Line 655 before the save to Perment tables, session.applicationId=" + SessionHelper.ApplicationID + " Master appid=" + Master.ApplicationID + " session rnddcode=" + SessionHelper.SessionUniqueID + " Master code:" + Master.RNLicenseOrSSN)
                ' Exit Sub
                'End If

                If Request.QueryString("reloadappid") <> Nothing Then

                    AppID = CInt(Request.QueryString("reloadappid"))
                    lblRNDDCode2.Text = Request.QueryString("UserID")
                    lblApplicationID2.Text = AppID
                    SessionID = lblRNDDCode2.Text 'SessionHelper.SessionUniqueID
                    RNorDD = Request.QueryString("RNorDDFlg") 'SessionHelper.RN_Flg
                    sApplicationType = Request.QueryString("ApplicationType")

                    sSelectedUserRole = Request.QueryString("SelectedUserRole")
                    lblName2.Text = Request.QueryString("UserName")
                    'Master.ApplicationID = AppID
                    Dim CertType As New RoleLevelCategory
                    CertType = CInt(Request.QueryString("SelectedUserRole"))

                    lblApptype2.Text = EnumHelper.GetEnumDescription(CertType) & "  " & sApplicationType
                    rte = Msvc.SendToErrorLog("Reload appId is " & AppID)

                Else
                    AppID = SessionHelper.ApplicationID
                End If
                Master.HideProgressBar = True
                Master.HideLink = True
                pError.Visible = False
                dPrint.Style("display") = "inline"
                dViewPrint.Style("display") = "none"
                'dActions.Style("display") = "none"
                dWE.Style("display") = "inline"
                dEmployers.Style("display") = "inline"
                DisableControls(dActions)
                DisableControls(dNavButtons)
            Else
                ' AppID = Master.ApplicationID
                'rte = Msvc.SendToErrorLog("Master appId is " & AppID)
            End If


            'Need to test for application exist
            Dim srvApplData As IApplicationDetailInformationService = StructureMap.ObjectFactory.GetInstance(Of IApplicationDetailInformationService)()
            If srvApplData.GetApplicationInfromationByAppID(AppID).Application_SID = 0 Then
                ProcessUserWithOutAppication()
            Else
                ProcessUserWithApplication()
            End If


            End If
    End Sub

    Private Sub ProcessUserWithOutAppication()
        NotSvc = StructureMap.ObjectFactory.GetInstance(Of INotationService)()
        Msvc = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        uploadSvc = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
        SumSvc = StructureMap.ObjectFactory.GetInstance(Of ISummaryService)()
        Dim notShowErr As Boolean = False

        If RNorDD = False Then
            pDocUpload.Style("display") = "none"
            dBtnWE.Style("display") = "none"
            dWE.Style("display") = "none"
            divSkillGrid.Style("display") = "inline"
        Else
            divSkillGrid.Style("display") = "none"
        End If
        If sApplicationType = "Renewal" Then
            hCertEndDate.Value = Msvc.GetCertificationDate(SessionID, sSelectedUserRole)
        End If


        Dim weSvc As IWorkExperienceService
        Dim empSvs As IEmployerInformationService

        'Dim AppStatuses As List(Of Model.AppStatus)


        hCertTime.Value = SumSvc.GetCertificateTime(sSelectedUserRole, sApplicationType)

        Dim personalSvc As IPersonalInformationService = StructureMap.ObjectFactory.GetInstance(Of IPersonalInformationService)()
        Dim ddpersonel As DDPersonnelDetails = Nothing
        Dim rnInfo As RNInformationDetails = Nothing
        '
        '1. pulling the Personal Data 
        If (RNorDD = False) Then
            ddpersonel = personalSvc.GetDDPersonnelInformationFromPermanent(SessionID)


        Else
            rnInfo = personalSvc.GetRNInfoFromPermanent(SessionID)
        End If
        SetAllDefaultValues(ddpersonel, rnInfo)

        '2. get work Experience  and Employement data
        weSvc = StructureMap.ObjectFactory.GetInstance(Of IWorkExperienceService)()
        welist = weSvc.GetExistingWorkExperience(SessionID)
        rptWE.DataSource = welist
        rptWE.DataBind()

        '3. get Employer Information 
        empSvs = StructureMap.ObjectFactory.GetInstance(Of IEmployerInformationService)()
        emplist = empSvs.GetEmployerInformationWithAddressFromPerm(SessionID, RNorDD)
        rptEmp.DataSource = emplist
        rptEmp.DataBind()

        '4. load course CEU and Skill data. 
        Me.LoadMainCourseData(False)

    End Sub

    Private Sub ProcessUserWithApplication()
        SaveToPermSvc = StructureMap.ObjectFactory.GetInstance(Of IMoveTempToPermService)()
        NotSvc = StructureMap.ObjectFactory.GetInstance(Of INotationService)()
        Msvc = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        uploadSvc = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
        SumSvc = StructureMap.ObjectFactory.GetInstance(Of ISummaryService)()

        Dim notShowErr As Boolean = False
        Dim pageCompletionRules As New Business.Rules.PageCompletionRules(AppID, SessionHelper.ExistingUserRole)
        Dim pl As List(Of PageModel) = PagesHelper.GetPageList()
        pl = PagesHelper.GetPageList()
        For i As Integer = 1 To pl.Count - 1
            Select Case pl(i).PageAddress
                Case "WorkExperience.aspx"
                    dBtnWE.Style("display") = "inline"
                Case "TrainingSkills.aspx"
                    divCourse.Style("display") = "inline"
                Case "Skills.aspx"
                    divSkillGrid.Style("display") = "inline"
                Case "DocumentUpload.aspx"
                    pDocUpload.Style("display") = "inline"
                Case "RNAttestation.aspx"
                    dAttestations.Style("display") = "inline"
            End Select
        Next
        If RNorDD = False Then
            pDocUpload.Style("display") = "none"
            dBtnWE.Style("display") = "none"
            dWE.Style("display") = "none"
            divSkillGrid.Style("display") = "inline"
        Else
            divSkillGrid.Style("display") = "none"
        End If
        If sApplicationType = "Renewal" Then
            hCertEndDate.Value = Msvc.GetCertificationDate(SessionID, sSelectedUserRole)
        End If
        'If NeedCert(SessionHelper.SelectedUserRole, SessionHelper.ApplicationStatus, SessionHelper.ApplicationType) = 0 Then
        If Not IsNothing(Request.QueryString("savedstatus")) Then
            pError.Visible = False
            notShowErr = True
            pnote.Visible = True
            lblNote.Text = "Application saved with status: " & SessionHelper.ApplicationStatus & "</br> Click <a href='UpdateExistingPage.aspx'>HERE</a> if you want to go back to Update Existing page."
            'pnote.Style("display") = "inline"

            'pnote.InnerHtml = "Application saved with status: " & SessionHelper.ApplicationStatus & "</br> Click <a href='UpdateExistingPage.aspx'>HERE</a> if you want to go back to Update Existing page."
        End If

        Dim weSvc As IWorkExperienceService
        Dim empSvs As IEmployerInformationService


        'SessionID = lblRNLicenseLabel2.Text 'SessionHelper.SessionUniqueID
        ' RNorDD = SessionHelper.RN_Flg
        'sApplicationType = SessionHelper.ApplicationType
        'sSelectedUserRole = SessionHelper.SelectedUserRole

        Dim AppStatuses As List(Of Model.AppStatus)
        'PISvc = StructureMap.ObjectFactory.GetInstance(Of IPersonalInformationService)()

        hCertTime.Value = SumSvc.GetCertificateTime(sSelectedUserRole, sApplicationType)

        Dim personalSvc As IPersonalInformationService = StructureMap.ObjectFactory.GetInstance(Of IPersonalInformationService)()
        Dim ddpersonel As DDPersonnelDetails = Nothing
        Dim rnInfo As RNInformationDetails = Nothing

        If (RNorDD = False) Then
            ddpersonel = personalSvc.GetDDPersonnelInformation(AppID)
        Else
            rnInfo = personalSvc.GetRNInformation(AppID)
        End If
        SetAllDefaultValues(ddpersonel, rnInfo)
        ''Skills

        weSvc = StructureMap.ObjectFactory.GetInstance(Of IWorkExperienceService)()
        empSvs = StructureMap.ObjectFactory.GetInstance(Of IEmployerInformationService)()

        welist = weSvc.GetAddedExpFullList(AppID)
        rptWE.DataSource = welist
        rptWE.DataBind()
        emplist = empSvs.GetAddedEmployerFull(AppID)
        rptEmp.DataSource = emplist
        rptEmp.DataBind()
        txtStartDate.Value = Date.Today.ToShortDateString
        Me.LoadMainCourseData(True)

#If DEBUG Then
        'SessionHelper.MAISUserID = 22863
        'SessionHelper.UserRoleForRNDD = 1
        'AppID = 19
#End If
        Dim rnAttService As IRN_AttestationService = StructureMap.ObjectFactory.GetInstance(Of IRN_AttestationService)()
        Dim numberofQuestions As Integer = 1

        Dim appInfo As IApplicationDetailInformationService = StructureMap.ObjectFactory.GetInstance(Of IApplicationDetailInformationService)()
        Dim appDetails As New Business.Model.ApplicationInformationDetails
        appDetails = appInfo.GetApplicationInfromationByAppID(AppID)
        Dim AppTypeID As Integer = appDetails.ApplicationType_SID 'DirectCast([Enum].Parse(GetType(Enums.ApplicationType), SessionHelper.ApplicationType), Enums.ApplicationType)
        hRNDateOfIssuance.Value = appInfo.GetRNLicenseIssuenceDateByAppID(AppID)
        hBiggerDate.Value = hRNDateOfIssuance.Value
        hBiggerName.Value = "RN Liscense issuance Date"
        If IsDate(hCourseStartDate.Value) Then
            If CDate(hCourseStartDate.Value) > CDate(hBiggerDate.Value) Then
                hBiggerDate.Value = hCourseStartDate.Value
                hBiggerName.Value = "Session or CEU End Date"
            End If
        End If
        If (hCertEndDate.Value <> "12/12/1999") Then
            If CDate(hCertEndDate.Value) > CDate(hBiggerDate.Value) Then
                hBiggerDate.Value = hCertEndDate.Value
                hBiggerName.Value = "Certification End Date"
            End If
        End If
        Dim Signature = appDetails.Signature
        If Signature Is Nothing OrElse String.IsNullOrWhiteSpace(Signature) Then
        Else
            For Each returnAttestationQuestionModel As MAIS.Business.Model.AttestationQuestions In rnAttService.GetRN_AttestationQuestionForPage(sSelectedUserRole, AppTypeID)
                If Not returnAttestationQuestionModel.AttestationDesc Is Nothing And returnAttestationQuestionModel.AttestationDesc.Length > 0 Then

                    Dim pnlQuestion As New Panel()
                    'pnlQuestion.Controls.Add(lblQuestion)
                    pnlQuestion.ID = "pnl" & numberofQuestions
                    pnlQuestion.GroupingText = "Question " & numberofQuestions
                    pnlQuestion.BackColor = Drawing.Color.White
                    pnlQuestion.Width = "720"
                    Dim offenseControl As RN_AttestationControl = LoadControl("UserControls\RN_AttestationControl.ascx")
                    offenseControl.SetControlParameters(returnAttestationQuestionModel.ApplicationType_Sid, numberofQuestions, returnAttestationQuestionModel.Attestation_SID, returnAttestationQuestionModel.ApplicationType_Sid, returnAttestationQuestionModel.AttestationDesc)
                    offenseControl.Initialize()
                    pnlQuestion.Controls.Add(offenseControl)
                    pAttestationQuestions.Controls.Add(pnlQuestion)
                End If
                numberofQuestions = numberofQuestions + 1
            Next
            Dim maincontextctl As Control = Page.Master.FindControl("maincontent")
            For currentpanelnumber As Integer = 1 To numberofQuestions - 1
                Dim currentpanel As Panel = maincontextctl.FindControl("pnl" & currentpanelnumber)
                For Each c As Control In currentpanel.Controls
                    If TypeOf c Is RN_AttestationControl Then
                        DirectCast(c, RN_AttestationControl).ShowPanel_ReadOnly()
                        'to test if error in master 
                    End If
                Next
            Next
        End If
        If Signature Is Nothing OrElse String.IsNullOrWhiteSpace(Signature) Then
            lblAttestation.Text = "Attestation is not completed. Attestation is Required Before Submitting Application!"
        Else
            If appDetails.IsAdminFlag Then
                lblAttestation.ForeColor = Drawing.Color.Black
                lblAttestation.Text = "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Attestation Completed By <u>Administrator</u>"
            Else
                Dim rndetail As Business.Model.RNInformationDetails = SumSvc.getRNInfo(appDetails.Attestant_SID)
                lblAttestation.ForeColor = Drawing.Color.Black
                lblAttestation.Text = "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Attestation Completed By <u>" & rndetail.FirstName & "&nbsp;" & rndetail.LastName & "&nbsp;" & rndetail.MiddleName & "</u>"
            End If

        End If
        ''lblAttestation.Text = "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Attestation Completed By <u>Jyoti</u>" ' & Master.CertificationApplication.AttestationAcceptanceSignature & "</u>"
        ''If Master.CertificationApplication.AttestationAcceptanceFlag = True Then
        ''lblAttestation.Text = lblAttestation.Text & " And Chose To <u>AGREE</u> to Attestation "
        Dim uploadedFiles As New List(Of Business.Model.DocumentUpload)

        uploadedFiles = uploadSvc.GetUploadedDocuments(AppID) 'AppID
        gvFiles.DataSource = uploadedFiles
        gvFiles.DataBind()

        AppStatuses = SumSvc.GetAppStatuses()
        Dim chk As Boolean = False
        Dim qstat As String = ""
        If Not IsNothing(Request.QueryString("selstat")) Then
            qstat = Request.QueryString("selstat").Replace("-", " ")
            For Each b1 As Model.AppStatus In AppStatuses
                If b1.ASTypeDesc = qstat Then
                    appDetails.ApplicationStatusType_SID = b1.ASTypeSid
                End If
            Next
            pError.Visible = False
            notShowErr = True
            pnote.Visible = True
            lblNote.Text = "Click Save and Continue at the bottom of the page to Save and submit this application."
            lblNote.Focus()
        End If
        For Each a1 As Model.AppStatus In AppStatuses
            If ((a1.ASTypeDesc = "Certified" Or a1.ASTypeDesc = "Intent to Deny" Or a1.ASTypeDesc = "Denied") And (a1.ASTypeSid = appDetails.ApplicationStatusType_SID)) Then
                chk = True
            End If
        Next

        For Each ap As Model.AppStatus In AppStatuses
            If (ap.ASTypeDesc = "Meets Requirements" Or ap.ASTypeDesc = "Did Not Meet Requirements") Then
                ddAppStatus.Items.Add(New ListItem With {.Value = ap.ASTypeSid, .Text = ap.ASTypeDesc, .Enabled = IIf(((appDetails.RoleCategoryLevel_SID = 7) Or UserAndRoleHelper.IsUserSecretary), False, True)})
            ElseIf (ap.ASTypeDesc = "Added To Registry" Or ap.ASTypeDesc = "Removed From Registry") Then
                ddAppStatus.Items.Add(New ListItem With {.Value = ap.ASTypeSid, .Text = ap.ASTypeDesc, .Enabled = IIf(appDetails.RoleCategoryLevel_SID = 7, True, False)})
            Else
                ddAppStatus.Items.Add(New ListItem With {.Value = ap.ASTypeSid, .Text = ap.ASTypeDesc, .Selected = IIf(chk And ap.ASTypeDesc = "DODD Review", True, False), .Enabled = IIf(ap.ASTypeDesc = "Certified" Or ap.ASTypeDesc = "Intent to Deny" Or ap.ASTypeDesc = "Denied", False, True)})
            End If
        Next
        For Each lis As ListItem In ddAppStatus.Items
            If lis.Text = "DODD Review" Then
                lis.Enabled = IIf(((appDetails.RoleCategoryLevel_SID = 7 Or appDetails.RoleCategoryLevel_SID = 8) Or UserAndRoleHelper.IsUserSecretary), False, True)
            End If
        Next
        If Not chk Then
            ddAppStatus.SelectedValue = appDetails.ApplicationStatusType_SID
        End If

        If (ddAppStatus.SelectedItem.Text <> "Pending" And IsNothing(Request.QueryString("selstat"))) Or qstat = "Certified" Or qstat = "Denied" Or qstat = "Intent to Deny" Then
            ddAppStatus.Enabled = False
        End If

        If ((ddAppStatus.SelectedItem.Text = "DODD Review" And (qstat = "" Or qstat <> "DODD Review")) And (SessionHelper.MAISLevelUserRole = Enums.RoleLevelCategory.RNMaster_RLC Or UserAndRoleHelper.IsUserAdmin = True)) Then
            PAdminStatus.Visible = True
            ddAdminStatus.Items.Add(New ListItem With {.Value = "0", .Text = "Select Decision Status"})
            For Each a2 As Model.AppStatus In AppStatuses
                ddAdminStatus.Items.Add(New ListItem With {.Value = a2.ASTypeSid, .Text = a2.ASTypeDesc, .Selected = IIf(chk And a2.ASTypeSid = appDetails.ApplicationStatusType_SID, True, False), .Enabled = IIf(a2.ASTypeDesc = "Certified" Or a2.ASTypeDesc = "Intent to Deny" Or a2.ASTypeDesc = "Denied", True, False)})
            Next

        End If
        If PAdminStatus.Visible = True Then
            For Each admi As ListItem In ddAdminStatus.Items
                If (admi.Text = "Intent to Deny" AndAlso ddAdminStatus.SelectedItem.Text <> "Denied" AndAlso ddAdminStatus.SelectedItem.Text <> "Intent to Deny" AndAlso (qstat = "" OrElse qstat <> "Denied")) Then
                    ddAdminStatus.Items.FindByText("Denied").Enabled = False
                End If
            Next
        End If

        Dim comps As String = ""
        'comps = AllPreviousCompleted(appDetails.ApplicationType_SID)
        'If pageCompletionRules.IsAttestationPageCompletedYes(SessionHelper.SelectedUserRole, appDetails.ApplicationType_SID) = False Then
        '    comps = comps & "</br> Attestant didn't agree to all question on the attestation page"
        'End If

        If comps <> "" Then
            If notShowErr = False Then
                pError.Visible = True
                pError.InnerHtml = comps
            End If
            For Each li As ListItem In ddAppStatus.Items
                If li.Text = "Meets Requirements" Or li.Text = "Added To Registry" Then
                    li.Enabled = False
                End If
            Next
        End If
        Dim attid As Integer = IIf(appDetails.Attestant_SID.HasValue, appDetails.Attestant_SID, -1)
        Dim LoggedInSid As String = ""
        'LoggedInSid = SumSvc.g
        If appDetails.ApplicationType_SID <> 4 Then
            If (UserAndRoleHelper.IsUserSecretary) Then
                For Each li As ListItem In ddAppStatus.Items
                    If li.Text <> "Pending" AndAlso li.Text <> "Voided Application" Then
                        li.Enabled = False
                    Else
                        li.Selected = IIf(li.Text = "Pending", True, False)
                    End If
                Next
                DisableControls(PDates) '(Master.FindControl("mainContent")) '(Page.FindControl("Content3")) 
            End If
            If SessionHelper.Notation_Flg = True Then
                For Each li As ListItem In ddAppStatus.Items
                    If li.Text <> "DODD Review" AndAlso li.Text <> "Voided Application" Then
                        li.Enabled = False
                    Else
                        li.Enabled = True
                        li.Selected = IIf(li.Text = "DODD Review", True, False)
                    End If
                Next
            End If
        Else
            For Each li As ListItem In ddAppStatus.Items
                If li.Text <> "Pending" AndAlso li.Text <> "Voided Application" AndAlso li.Text <> "Meets Requirements" Then
                    li.Enabled = False
                Else
                    li.Selected = IIf(li.Text = "Pending", True, False)
                End If
            Next
            PDates.Style("display") = "none"
            'DisableControls(PDates)
        End If
        If (hCertEndDate.Value <> "12/12/1999" AndAlso (CInt(hCertTime.Value) <> 11 Or CDate(hCertEndDate.Value) >= Date.Today)) Then
            txtStartDate.Value = DateAdd(DateInterval.DayOfYear, 1, CDate(hCertEndDate.Value))
        End If
        txtEndDate.Value = IIf(CalcEndDate(CDate(txtStartDate.Value), hCertTime.Value).ToShortDateString = "12/31/9999", "", CalcEndDate(CDate(txtStartDate.Value), hCertTime.Value).ToShortDateString)
        If ((appDetails.ApplicationStatusType_SID = Enums.ApplicationStatusType.AddedToRegistry Or appDetails.ApplicationStatusType_SID = Enums.ApplicationStatusType.MeetsRequirements Or appDetails.ApplicationStatusType_SID = Enums.ApplicationStatusType.Certified) And Not IsNothing(Session("SDate"))) Then
            txtStartDate.Value = CDate(Session("SDate")).ToShortDateString
            txtEndDate.Value = CDate(Session("EDate")).ToShortDateString
        Else
            Session("SDate") = Nothing
            Session("EDate") = Nothing
        End If

        grdNotationsSum.DataSource = NotSvc.GetNotationsByApp(AppID, RNorDD).ReturnValue
        grdNotationsSum.DataBind()


        If Master.UserSessionMatch = False Then
            dNavButtons.Visible = False
            'Dim message As String = "Test Alert"
            'Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()

            'sb.Append("<script type = 'text/javascript'>")

            'sb.Append("window.onload=function(){")

            'sb.Append("alert('")

            'sb.Append(message)

            'sb.Append("')};")

            'sb.Append("</script>")

            'ClientScript.RegisterStartupScript(Page.GetType(), "ShowAlert", sb.ToString)
        End If

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

    Private Sub SetAllDefaultValues(ByVal ddpersonal As DDPersonnelDetails, ByVal rnInfo As RNInformationDetails)
        If (ddpersonal IsNot Nothing) Then
            lblRNLNoOrSSNtxt.Text = ddpersonal.DODDLast4SSN.Trim()
            lblDtIssuedOrDOBtxt.Text = ddpersonal.DODDDateOfBirth.ToShortDateString().Trim()
            lblLastName.Text = ddpersonal.DODDLastName.Trim()
            lblFirstName.Text = ddpersonal.DODDFirstName.Trim()
            lblMiddleName.Text = ddpersonal.DODDMiddleName.Trim()
            lblAddr1.Text = ddpersonal.DODDHomeAddressLine1.Trim()
            If (Not String.IsNullOrWhiteSpace(ddpersonal.DODDHomeAddressLine2)) Then
                lblAddr2.Text = ddpersonal.DODDHomeAddressLine2.Trim()
            Else
                lblAddr2.Text = String.Empty
            End If

            lblCity.Text = ddpersonal.DODDHomeCity.Trim()
            If (Not String.IsNullOrWhiteSpace(ddpersonal.DODDHomeCity)) Then
                lblCounty.Text = ddpersonal.DODDHomeCounty.Trim()
            Else
                lblCounty.Text = String.Empty
            End If
            lblState.Text = ddpersonal.DODDHomeState.Trim()
            lblZip.Text = ddpersonal.DODDHomeZip.Trim()
            Dim zipPlus As String = String.Empty
            If (Not String.IsNullOrWhiteSpace(ddpersonal.DODDHomeZipPlus)) Then
                zipPlus = ddpersonal.DODDHomeZipPlus.Trim()
                lblZip.Text = "-" + zipPlus
            End If

            If (ddpersonal.DODDGender = "F") Then
                rdbGender.SelectedValue = "1"
            ElseIf (ddpersonal.DODDGender = "M") Then
                rdbGender.SelectedValue = "0"
            End If
            If ddpersonal.Address.Phone IsNot Nothing Then
                For Each ph As Business.Model.PhoneDetails In ddpersonal.Address.Phone
                    If (ph.ContactType = ContactType.Home) Then
                        lblHomePhoneNumber.Text = ph.PhoneNumber.Trim().Substring(0, 3) & "-" & ph.PhoneNumber.Trim().Substring(3, 3) & "-" & ph.PhoneNumber.Trim().Substring(6, 4)
                    ElseIf (ph.ContactType = ContactType.Work) Then
                        lblWorkPhoneNumber.Text = ph.PhoneNumber.Trim().Substring(0, 3) & "-" & ph.PhoneNumber.Trim().Substring(3, 3) & "-" & ph.PhoneNumber.Trim().Substring(6, 4)
                    ElseIf (ph.ContactType = ContactType.CellOther) Then
                        lblCellPhoneNumber.Text = ph.PhoneNumber.Trim().Substring(0, 3) & "-" & ph.PhoneNumber.Trim().Substring(3, 3) & "-" & ph.PhoneNumber.Trim().Substring(6, 4)
                    End If
                Next
            End If

            If ddpersonal.Address.Email IsNot Nothing Then
                For Each ph As Business.Model.EmailAddressDetails In ddpersonal.Address.Email
                    If (ph.ContactType = ContactType.Home) Then
                        lblHomeAddress.Text = ph.EmailAddress.Trim()
                    ElseIf (ph.ContactType = ContactType.Work) Then
                        lblWorkAddress.Text = ph.EmailAddress.Trim()
                    ElseIf (ph.ContactType = ContactType.CellOther) Then
                        lblCellAddress.Text = ph.EmailAddress.Trim()
                    End If
                Next
            End If

        End If
        If (Not (rnInfo Is Nothing)) Then
            lblRNLNoOrSSNtxt.Text = rnInfo.RNLicense.Trim()
            lblDtIssuedOrDOBtxt.Text = rnInfo.DateOforiginalRNLicIssuance.ToShortDateString().Trim()
            lblLastName.Text = rnInfo.LastName.Trim()
            lblFirstName.Text = rnInfo.FirstName.Trim()
            lblMiddleName.Text = rnInfo.MiddleName.Trim()
            lblAddr1.Text = rnInfo.HomeAddressLine1.Trim()
            If (Not String.IsNullOrWhiteSpace(rnInfo.HomeAddressLine2)) Then
                lblAddr2.Text = rnInfo.HomeAddressLine2.Trim()
            Else
                lblAddr2.Text = String.Empty
            End If

            lblCity.Text = rnInfo.HomeCity.Trim()
            If ((Not String.IsNullOrWhiteSpace(rnInfo.HomeCounty))) Then
                lblCounty.Text = rnInfo.HomeCounty.Trim()
            Else
                lblCounty.Text = String.Empty
            End If
            lblState.Text = rnInfo.HomeState.Trim()
            Dim zipPlus1 As String = String.Empty

            lblZip.Text = rnInfo.HomeZip.Trim()
            If (Not String.IsNullOrWhiteSpace(rnInfo.HomeZipPlus)) Then
                zipPlus1 = rnInfo.HomeZipPlus.Trim()
                lblZip.Text = "-" + zipPlus1
            End If
            If (rnInfo.Gender = "F") Then
                rdbGender.SelectedValue = "1"
            ElseIf (rnInfo.Gender = "M") Then
                rdbGender.SelectedValue = "0"
            End If
            For Each ph As Business.Model.PhoneDetails In rnInfo.Address.Phone
                If (ph.ContactType = ContactType.Home) Then
                    lblHomePhoneNumber.Text = ph.PhoneNumber.Trim().Substring(0, 3) & "-" & ph.PhoneNumber.Trim().Substring(3, 3) & "-" & ph.PhoneNumber.Trim().Substring(6, 4)
                ElseIf (ph.ContactType = ContactType.Work) Then
                    lblWorkPhoneNumber.Text = ph.PhoneNumber.Trim().Substring(0, 3) & "-" & ph.PhoneNumber.Trim().Substring(3, 3) & "-" & ph.PhoneNumber.Trim().Substring(6, 4)
                ElseIf (ph.ContactType = ContactType.CellOther) Then
                    lblCellPhoneNumber.Text = ph.PhoneNumber.Trim().Substring(0, 3) & "-" & ph.PhoneNumber.Trim().Substring(3, 3) & "-" & ph.PhoneNumber.Trim().Substring(6, 4)
                End If
            Next
            For Each ph As Business.Model.EmailAddressDetails In rnInfo.Address.Email
                If (ph.ContactType = ContactType.Home) Then
                    lblHomeAddress.Text = ph.EmailAddress.Trim()
                ElseIf (ph.ContactType = ContactType.Work) Then
                    lblWorkAddress.Text = ph.EmailAddress.Trim()
                ElseIf (ph.ContactType = ContactType.CellOther) Then
                    lblCellAddress.Text = ph.EmailAddress.Trim()
                End If
            Next
        End If
    End Sub

    Public Function AllPreviousCompleted(ByVal apptypesid As Integer) As String
        Dim s As String = ""
        Dim whatpage As String = ""
        Dim pageList As List(Of PageModel) = PagesHelper.GetPageList()

        pageList = PagesHelper.GetPageList()
        For i As Integer = 1 To pageList.Count - 1
            Dim currentPageCompleted As Boolean = False
            Dim pageCompletionRules As New Business.Rules.PageCompletionRules(AppID, SessionHelper.ExistingUserRole)
            Select Case pageList(i).PageAddress
                Case "StartPage.aspx"
                    currentPageCompleted = pageCompletionRules.IsStartPageComplete()
                Case "PersonalInformation.aspx"
                    currentPageCompleted = pageCompletionRules.IsPersonalInformationPageComplete()
                Case "EmployerInformation.aspx"
                    currentPageCompleted = pageCompletionRules.IsEmployerInformationPageComplete()
                Case "WorkExperience.aspx"
                    currentPageCompleted = pageCompletionRules.IsWorkExperienceInformationPageComplete()
                Case "TrainingSkills.aspx"
                    currentPageCompleted = pageCompletionRules.IsTrainingPageComplete()
                Case "Skills.aspx"
                    currentPageCompleted = pageCompletionRules.IsSkillsPageComplete()
                Case "DocumentUpload.aspx"
                    currentPageCompleted = pageCompletionRules.IsDocumentUploadPageComplete()
                Case "RNAttestation.aspx"
                    currentPageCompleted = pageCompletionRules.IsAttestationPageComplete()
                Case "Summary.aspx"
                    currentPageCompleted = pageCompletionRules.IsSummaryPageComplete()
                Case "ViewCertificate.aspx"
                    currentPageCompleted = False
                Case "Notation.aspx"
                    currentPageCompleted = False
            End Select
            If Not currentPageCompleted And pageList(i).PageAddress <> "Summary.aspx" And pageList(i).PageAddress <> "ViewCertificate.aspx" And pageList(i).PageAddress <> "Notation.aspx" Then
                If s.Length = 0 Then
                    ' s = pageList(i).PageAddress.Substring(0, pageList(i).PageAddress.Length - 5)
                    s = pageList(i).PageName
                Else
                    s = s & ", " & pageList(i).PageName
                End If

            End If
            If pageList(i).PageAddress = "RNAttestation.aspx" Then
                If pageCompletionRules.IsAttestationPageCompletedYes(SessionHelper.SelectedUserRole, apptypesid) = False Then
                    s = s & "</br> Attestant didn't agree to all question on the attestation page"
                End If
            End If
        Next
        s = IIf(s = "", "", "You cannot select the final status until you complete the following pages: " & s)
        Return s
    End Function

    Protected Sub LoadMainCourseData(ByVal LoadFormAppTable As Boolean)
        Dim CourseService As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()
        Dim CEUDetails As New List(Of Business.Model.CEUDetails)
        Dim CourseDetails As New List(Of Business.Model.CourseDetails)
        If LoadFormAppTable = True Then
            CourseDetails = CourseService.GetApplicationCourseAndSessionsByAppID(AppID)
        Else
            CourseDetails = CourseService.GetCourseAndSessionFromPermByUserID(SessionID)
        End If

        Dim SED As Date = CDate("12/12/1999")
        Dim cED As Date = CDate("12/12/1999")
        If Not IsNothing(CourseDetails) AndAlso CourseDetails.Count() > 0 Then
            Me.grdCourse.DataSource = CourseDetails
            Me.grdCourse.DataBind()
            If CourseDetails.Count > 0 Then
                Me.grdSession.DataSource = CourseDetails(0).SessionDetailList
                Me.grdSession.DataBind()
            End If



            For Each cd As Business.Model.CourseDetails In CourseDetails
                For Each ses As Business.Model.SessionAddressInformation In cd.SessionDetailList
                    If IsDate(ses.Session_End_Date) AndAlso ses.Session_End_Date > SED Then
                        SED = ses.Session_End_Date
                    End If
                Next
            Next
            hCourseStartDate.Value = SED
            'If SED < Date.Today Then
            txtStartDate.Value = CDate(hCourseStartDate.Value).ToShortDateString
            'End If
        End If
        Dim SKService As ISkillPageService = StructureMap.ObjectFactory.GetInstance(Of ISkillPageService)()
        Dim GidDataList As New List(Of Business.Model.SkillVerificationTypeCheckListOnly)
        'Select Case SessionHelper.ApplicationType
        '    Case Enums.ApplicationType.Initial.ToString
        '        GidDataList = SKService.GetSkillVerificationCheckListData(0, SessionHelper.ApplicationID)
        '    Case Else
        GidDataList = SKService.GetSkillVerificationCheckListData(SessionID, AppID)
        'End Select
        Me.grvSkillsData.DataSource = GidDataList
        Me.grvSkillsData.DataBind()
        If Not IsNothing(GidDataList) AndAlso GidDataList.Count > 0 Then
            For Each sver As Business.Model.SkillVerificationTypeCheckListOnly In GidDataList
                If IsDate(sver.Verification_Date) AndAlso sver.Verification_Date > SED Then
                    SED = sver.Verification_Date
                End If
            Next
            hCourseStartDate.Value = SED
            'If SED < Date.Today Then
            txtStartDate.Value = CDate(hCourseStartDate.Value).ToShortDateString
        Else
            divSkillGrid.Visible = False
        End If

        CEUDetails = CourseService.GetAllCEUByUserID(SessionID, CourseService.GetCategoryByRoleCategoryLevelSid(sSelectedUserRole).Category_Type_Sid, AppID) '.GetCEUByUserID(SessionHelper.SessionUniqueID, CourseService.GetCategoryByRoleCategoryLevelSid(SessionHelper.SelectedUserRole).Category_Type_Sid)
        If Not IsNothing(CEUDetails) AndAlso CEUDetails.Count() > 0 Then
            PCeus.Visible = True
            grvCeus.DataSource = CEUDetails
            grvCeus.DataBind()

            For Each c As Business.Model.CEUDetails In CEUDetails
                If IsDate(c.Attended_Date) AndAlso c.Attended_Date > cED Then
                    cED = c.Attended_Date
                End If
            Next
        Else
            PCeus.Visible = False
        End If
        If cED > SED Then
            hCourseStartDate.Value = cED
            txtStartDate.Value = CDate(hCourseStartDate.Value).ToShortDateString
        End If
    End Sub

    Public Function CalcEndDate(ByVal sDate As Date, ByVal ctime As Integer) As Date
        Dim edate As Date = CDate("12/31/9999")
        If ctime = 1 Then
            edate = DateAdd(DateInterval.DayOfYear, -1, DateAdd(DateInterval.Year, 1, sDate))
        ElseIf ctime = 11 Then
            edate = DateAdd(DateInterval.Year, 1, CDate(hCertEndDate.Value))
        ElseIf ctime = 2 Then
            If CDate("8/31/" & sDate.Year) > sDate AndAlso sDate.Year Mod 2 <> 0 Then
                Return CDate("8/31/" & sDate.Year)
            ElseIf ((sDate.Year + 1) Mod 2) <> 0 Then
                Return CDate("8/31/" & (sDate.Year + 1))
            ElseIf ((sDate.Year + 2) Mod 2) <> 0 Then
                Return CDate("8/31/" & (sDate.Year + 2))
            End If
        ElseIf ctime = 12 Then
            If ((sDate.Year + 1) Mod 2) <> 0 Then
                Return CDate("8/31/" & (sDate.Year + 1))
            ElseIf ((sDate.Year + 2) Mod 2) <> 0 Then
                Return CDate("8/31/" & (sDate.Year + 2))
            End If
        End If
        Return edate
    End Function

    Public Sub AppStatChanged()
        ' Do not need this for Print View Only page for Summary. 

        'Dim fstat As String = ""
        'Dim res As New ReturnObject(Of Long)(-1L)
        'fstat = ddAppStatus.SelectedItem.Text

        'If PAdminStatus.Visible = True AndAlso ddAdminStatus.SelectedValue <> "0" Then
        '    fstat = ddAdminStatus.SelectedItem.Text
        'End If
        'If fstat = "Did Not Meet Requirements" Or fstat = "Removed From Registry" Or fstat = "DODD Review" Or fstat = "Denied" Or fstat = "Intent to Deny" Then
        '    ''if no notation exists for this app for this status then
        '    Dim hasNotation As Boolean = False
        '    If SessionHelper.Notation_Flg = True And fstat = "DODD Review" Then
        '        hasNotation = True
        '    End If
        '    Dim nl As New List(Of NotationDetails)
        '    nl = NotSvc.GetNotationsByApp(AppID, RNorDD).ReturnValue
        '    Dim ntype As String = GetNotTypeByStatusType(fstat)

        '    If nl IsNot Nothing Then
        '        For Each no As NotationDetails In nl
        '            If no.NotationType.NTypeDesc = ntype Then
        '                hasNotation = True
        '            End If
        '        Next
        '    End If
        '    If Not hasNotation Then
        '        res = SaveToPermSvc.InsertPersonIfNotExists(AppID, RNorDD)
        '        If res.ReturnValue = 0 Then
        '            For Each mstring As ReturnMessage In res.GeneralMessages
        '                If mstring.Message Like "*niqueCode*" Then
        '                    SessionHelper.SessionUniqueID = mstring.Message.Substring(11)
        '                End If
        '            Next
        '            fstat = Server.HtmlEncode(fstat)
        '            Response.Redirect("Notation.aspx?appstatus=" & fstat)
        '        Else
        '            pError.Visible = True
        '            For Each mstring As ReturnMessage In res.GeneralMessages
        '                If mstring.Message Like "*niqueCode*" Then
        '                    SessionHelper.SessionUniqueID = mstring.Message.Substring(11)
        '                End If
        '            Next
        '            pError.InnerHtml += "An Error occured while saving summary information."
        '        End If
        '    End If
        'End If
    End Sub

    Public Shared Property AppID As Integer
        Private Get
            Return _appID
        End Get
        Set(ByVal value As Integer)
            _appID = value
        End Set
    End Property
    Public Shared Property SessionID As String
        Private Get
            Return _sessionId
        End Get
        Set(ByVal value As String)
            _sessionId = value
        End Set
    End Property
    Public Shared Property RNorDD As String
        Private Get
            Return _rnorDD
        End Get
        Set(ByVal value As String)
            _rnorDD = value
        End Set
    End Property
    Public Shared Property sApplicationType As String
        Get
            Return _ApplicationType
        End Get
        Set(value As String)
            _ApplicationType = value
        End Set
    End Property

    Public Shared Property sSelectedUserRole As Integer
        Get
            Return _SelectedUserRole
        End Get
        Set(value As Integer)
            _SelectedUserRole = value
        End Set
    End Property

    Protected Sub grvCeus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grvCeus.SelectedIndexChanged

    End Sub
End Class