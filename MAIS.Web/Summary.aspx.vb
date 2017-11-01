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

Public Class Summary
    Inherits System.Web.UI.Page
    Private Shared _appID As Integer
    Private Shared _sessionId As String
    Private Shared _rnorDD As String
    Private Shared _ApplicationType As String
    Private Shared _AppTypeId As Integer
    Public Shared _sig As String
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
        'query1 = StructureMap.ObjectFactory.GetInstance(Of Data.Queries.SummaryQueries)()
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
        Dim rnAttService As IRN_AttestationService = StructureMap.ObjectFactory.GetInstance(Of IRN_AttestationService)()
        Dim numberofQuestions As Integer = 1
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
            Dim notShowErr As Boolean = False
            Dim rte As ReturnObject(Of Long)
            If Request.QueryString("newwin") <> Nothing Then

                'If Master.UserSessionMatch("At Summary Page Line 655 before the save to Perment tables") = False Then
                'rte = Msvc.SendToErrorLog("At Summary Page Line 655 before the save to Perment tables, session.applicationId=" + SessionHelper.ApplicationID + " Master appid=" + Master.ApplicationID + " session rnddcode=" + SessionHelper.SessionUniqueID + " Master code:" + Master.RNLicenseOrSSN)
                ' Exit Sub
                'End If

                If Request.QueryString("reloadappid") <> Nothing Then

                    AppID = CInt(Request.QueryString("reloadappid"))

                    SessionHelper.ApplicationID = AppID
                    rte = Msvc.SendToErrorLog("Reload appId is " & AppID)

                Else
                    AppID = SessionHelper.ApplicationID
                End If
                Master.HideProgressBar = True
                pError.Visible = False
                dPrint.Style("display") = "inline"
                dViewPrint.Style("display") = "none"
                'dActions.Style("display") = "none"
                dWE.Style("display") = "inline"
                dEmployers.Style("display") = "inline"
                DisableControls(dActions)
                DisableControls(dNavButtons)
            Else
                AppID = Master.ApplicationID
                rte = Msvc.SendToErrorLog("Master appId is " & AppID)
            End If
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
            If SessionHelper.RN_Flg = False Then
                pDocUpload.Style("display") = "none"
                dBtnWE.Style("display") = "none"
                dWE.Style("display") = "none"
                divSkillGrid.Style("display") = "inline"
            Else
                divSkillGrid.Style("display") = "none"
            End If
            If SessionHelper.ApplicationType = "Renewal" Then
                hCertEndDate.Value = Msvc.GetCertificationDate(SessionHelper.SessionUniqueID, SessionHelper.SelectedUserRole)
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
            Dim a As String = Master.RNLicenseOrSSN
            Dim weSvc As IWorkExperienceService
            Dim empSvs As IEmployerInformationService


            SessionID = Master.RNLicenseOrSSN 'SessionHelper.SessionUniqueID
            RNorDD = SessionHelper.RN_Flg
            sApplicationType = SessionHelper.ApplicationType
            sSelectedUserRole = SessionHelper.SelectedUserRole

            Dim AppStatuses As List(Of Model.AppStatus)
            'PISvc = StructureMap.ObjectFactory.GetInstance(Of IPersonalInformationService)()

            hCertTime.Value = SumSvc.GetCertificateTime(SessionHelper.SelectedUserRole, SessionHelper.ApplicationType)

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


#If DEBUG Then
                'SessionHelper.MAISUserID = 22863
                'SessionHelper.UserRoleForRNDD = 1
                'AppID = 19
#End If
           

            Dim appInfo As IApplicationDetailInformationService = StructureMap.ObjectFactory.GetInstance(Of IApplicationDetailInformationService)()
            Dim appDetails As Business.Model.ApplicationInformationDetails
            appDetails = appInfo.GetApplicationInfromationByAppID(AppID)
            Dim AppTypeID As Integer = appDetails.ApplicationType_SID 'DirectCast([Enum].Parse(GetType(Enums.ApplicationType), SessionHelper.ApplicationType), Enums.ApplicationType)
            _AppTypeId = appDetails.ApplicationType_SID
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
            _sig = appDetails.Signature
            
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
            comps = AllPreviousCompleted(appDetails.ApplicationType_SID)
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
            Dim attLicNo As String = IIf(attid > 0, Msvc.GetRNsLicenseNumber(attid), "-1")
            'Dim LoggedInSid As String = ""
            'LoggedInSid = SumSvc.g
            If appDetails.ApplicationType_SID <> 4 Then
                If (UserAndRoleHelper.IsUserSecretary Or (SessionHelper.LoginUsersRNLicense <> attLicNo And (UserAndRoleHelper.IsUserAdmin = False) And SessionHelper.MAISLevelUserRole <> Enums.RoleLevelCategory.RNMaster_RLC)) Then
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
                        If UserAndRoleHelper.IsUserSecretary = False And comps = "" Then
                            li.Enabled = True 'IIf(li.Text = "Meets Requirements", True, False)
                        End If
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

        End If

        Me.LoadMainCourseData()
        If _sig Is Nothing OrElse String.IsNullOrWhiteSpace(_sig) Then
        Else
            For Each returnAttestationQuestionModel As MAIS.Business.Model.AttestationQuestions In rnAttService.GetRN_AttestationQuestionForPage(SessionHelper.SelectedUserRole, _AppTypeId)
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

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
    End Sub
    Protected Sub gvFiles_SelectedIndexChanging(ByVal sender As Object, ByVal e As GridViewSelectEventArgs) Handles gvFiles.SelectedIndexChanging
        'Transmit copy of selected file to user for preview
        Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
        Dim byteArrayForFileToView As Byte() = uploadService.GetUploadedDocumentByImageSID(gvFiles.DataKeys(e.NewSelectedIndex).Values(0))

        'Clear all content output from buffer stream
        Response.Clear()

        Dim filenameCell As Integer = 2

        If gvFiles.AutoGenerateDeleteButton = False Then
            filenameCell = 1
        End If

        'Add HTTP header to output stream to specify a default filename, and file length
        Response.AddHeader("Content-Disposition", "attachment; filename=" & _
            gvFiles.Rows(e.NewSelectedIndex).Cells.Item(filenameCell).Text.ToString)

        Response.AddHeader("Content-Length", byteArrayForFileToView.Length.ToString())

        'Set the HTTP MIME type for output stream
        Response.ContentType = "application/octet-stream"

        'Output the data to the client
        If byteArrayForFileToView.Length > 0 Then
            Dim ext = System.IO.Path.GetExtension(gvFiles.Rows(e.NewSelectedIndex).Cells(filenameCell).Text.ToString)

            'If ext = ".html" Then
            '    Response.Redirect(PagesHelper.AttestationPrintPage)
            'Else
            Response.BinaryWrite(byteArrayForFileToView)
            'End If

            Response.End()
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

    Protected Sub LoadMainCourseData()
        Dim CourseService As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()
        Dim CEUDetails As New List(Of Business.Model.CEUDetails)
        Dim CourseDetails As New List(Of Business.Model.CourseDetails)
        CourseDetails = CourseService.GetApplicationCourseAndSessionsByAppID(AppID)
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
        GidDataList = SKService.GetSkillVerificationCheckListData(SessionHelper.SessionUniqueID, SessionHelper.ApplicationID)
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

        CEUDetails = CourseService.GetAllCEUByUserID(SessionHelper.SessionUniqueID, CourseService.GetCategoryByRoleCategoryLevelSid(SessionHelper.SelectedUserRole).Category_Type_Sid, AppID) '.GetCEUByUserID(SessionHelper.SessionUniqueID, CourseService.GetCategoryByRoleCategoryLevelSid(SessionHelper.SelectedUserRole).Category_Type_Sid)
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
            If CDate("8/31/" & sDate.Year) > sDate AndAlso CDate("8/31/" & sDate.Year) > Date.Today AndAlso sDate.Year Mod 2 <> 0 Then
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
    Protected Sub btnSaveContinue_click() Handles btnSaveContinue.ServerClick
        Dim rte As ReturnObject(Of Long)
        'SessionHelper.ApplicationID = 500  ' for testing if session is changing

        If Master.UserSessionMatch("At Summary Page Line 720 before the save to Perment tables") = False Then
            rte = Msvc.SendToErrorLog("At Summary Page Line 720 before the save to Perment tables, session.applicationId=" + CStr(SessionHelper.ApplicationID) + " Master appid=" + CStr(Master.ApplicationID) + " session rnddcode=" + SessionHelper.SessionUniqueID + " Master code:" + Master.RNLicenseOrSSN)
            SessionHelper.ClearSessionHelpValues()

            Exit Sub
        End If



        Dim res As ReturnObject(Of Long)
        Dim EndDate As Date
        Dim fstat As String = ""
        fstat = ddAppStatus.SelectedItem.Text
        EndDate = Me.CalcEndDate(CDate(txtStartDate.Value), CInt(hCertTime.Value))
        Dim NotMoveAllToPerm As Boolean = False

        If PAdminStatus.Visible = True AndAlso ddAdminStatus.SelectedValue <> "0" Then
            fstat = ddAdminStatus.SelectedItem.Text
            res = SaveToPermSvc.SaveTempToPerm(AppID, RNorDD, SessionHelper.SelectedUserRole, CDate(txtStartDate.Value), EndDate, ddAppStatus.SelectedItem.Text, ddAdminStatus.SelectedItem.Text)
        Else
            res = SaveToPermSvc.SaveTempToPerm(AppID, RNorDD, SessionHelper.SelectedUserRole, CDate(txtStartDate.Value), EndDate, ddAppStatus.SelectedItem.Text, "")
        End If
        If SessionHelper.ApplicationType = "Update Profile" Then
            fstat = "Meets Requirements"
        End If

        If (fstat = "Pending" OrElse fstat = "DODD Review" OrElse fstat = "Select Decision Status" OrElse fstat = "Intent to Deny" OrElse fstat = "Voided Application") Then
            NotMoveAllToPerm = True
        End If
        If res.ReturnValue = 0 Then
            Session("SDate") = CDate(txtStartDate.Value)
            Session("EDate") = EndDate
            Session("AppStat") = fstat
            If fstat = "Voided Application" Or fstat = "Did Not Meet Requirements" Or fstat = "Removed From Registry" Or fstat = "Denied" Then
                pError.Visible = False
            End If

            SessionHelper.ApplicationStatus = fstat
            For Each mstring As ReturnMessage In res.GeneralMessages
                If mstring.Message Like "*niqueCode*" Then
                    SessionHelper.SessionUniqueID = mstring.Message.Substring(11)
                    Master.RNLicenseOrSSN = SessionHelper.SessionUniqueID
                End If
            Next
            If Master.UserSessionMatch("At Summary Page Line 691 after the SessionUniqueID Loop.") = False Then
                rte = Msvc.SendToErrorLog("At Summary Page Line 691 after the SessionUniqueID Loop." + SessionHelper.ApplicationID + " Master appid=" + Master.ApplicationID + " session rnddcode=" + SessionHelper.SessionUniqueID + " Master code:" + Master.RNLicenseOrSSN)
                Exit Sub
            End If
            Session("AppStat") = fstat
            'If fstat <> "Meets Requirements" AndAlso fstat <> "Added To Registry" AndAlso fstat <> "Voided Application" AndAlso fstat <> "Pending" Then
            '    Response.Redirect("Notation.aspx")
            'Else
            '    Response.Redirect(PagesHelper.GetNextPage(Master.CurrentPage))
            'End If
            divSpinner.Style("display") = "none"
        Else
            pError.Visible = True
            For Each mstring As ReturnMessage In res.GeneralMessages
                If mstring.Message Like "*niqueCode*" Then
                    'SessionHelper.SessionUniqueID = mstring.Message.Substring(11)
                ElseIf mstring.Message Like "*overlap*" Then
                    pError.InnerHtml = mstring.Message & "</br>"
                End If
            Next
            pError.InnerHtml += "An Error occured while saving summary information."
            divSpinner.Style("display") = "none"
            Exit Sub
            'For Each m As ReturnMessage In res.Errors
            '    pError.InnerHtml += m.Message & "</br>"
            'Next
        End If

        Dim t As Integer = 1
        If res.ReturnValue = 0 Then

            Dim MycontactEmail As String = String.Empty
            If (Not String.IsNullOrWhiteSpace(lblHomeAddress.Text.Trim())) Then
                MycontactEmail = lblHomeAddress.Text.Trim()
            ElseIf (Not String.IsNullOrWhiteSpace(lblWorkAddress.Text.Trim())) Then
                MycontactEmail = lblWorkAddress.Text.Trim()
            Else
                MycontactEmail = lblCellAddress.Text.Trim()
            End If

            If Master.UserSessionMatch("At Summary Page Line 804 before the processing Summary page and Cert") = False Then
                rte = Msvc.SendToErrorLog("At Summary Page Line 804 before the save to Perment tables, session.applicationId=" + CStr(SessionHelper.ApplicationID) + " Master appid=" + CStr(Master.ApplicationID) + " session rnddcode=" + SessionHelper.SessionUniqueID + " Master code:" + Master.RNLicenseOrSSN)
                SessionHelper.ClearSessionHelpValues()

                Exit Sub
            Else
                CreateSummary_Cert_SaveToUDS(AppID, sApplicationType, SessionID, sSelectedUserRole, RNorDD, res, MycontactEmail, lblDtIssuedOrDOBtxt.Text, lblRNLNoOrSSNtxt.Text)

            End If


            'Code to check to request for New Scecurity account creation for Initial RN application (Brand new who are not exisitng in the system
            If (ConfigHelper.CreateUserAccountAccess And (Not String.IsNullOrWhiteSpace(Trim(txtEndDate.Value)))) Then 'check to see if new security portal is up
                Dim retMsg As String = String.Empty
                Dim contactEmail As String = String.Empty
                If (Not String.IsNullOrWhiteSpace(lblHomeAddress.Text.Trim())) Then
                    contactEmail = lblHomeAddress.Text.Trim()
                ElseIf (Not String.IsNullOrWhiteSpace(lblWorkAddress.Text.Trim())) Then
                    contactEmail = lblWorkAddress.Text.Trim()
                Else
                    contactEmail = lblCellAddress.Text.Trim()
                End If
                If ((RNorDD = True) And (SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Enums.ApplicationType.Initial)) And
                    (Not String.IsNullOrWhiteSpace(SessionID)) And (SessionID.Contains("RN"))) Then

                    ' If ((Not String.IsNullOrWhiteSpace(SessionHelper.SessionUniqueID)) And (SessionHelper.SessionUniqueID.Contains("RN"))) Then
                    'Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
                    'Dim retcertHis As Model.Certificate = maisSvc.GetCertificationHistory(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg).FirstOrDefault()
                    'If ((retcertHis IsNot Nothing) And (retcertHis.EndDate > Today) And (retcertHis.Status = EnumHelper.GetEnumDescription(Enums.CertificationStatusType.Certified))) Then

                    '    Dim userSVC As New UserRequestService.AEGIS_WS
                    '    retMsg = userSVC.CreateNewUser_Nurse(lblFirstName.Text, lblLastName.Text, contactEmail, ConfigHelper.BUCode, lblRNLNoOrSSNtxt.Text, CDate(Trim(txtEndDate.Value)), True)
                    '    If retMsg.Contains("error") Then
                    '        lblErrMsgSecurity.Text = "Error in web service to create security account for New RN,Please contact Adminstrator"
                    '    Else
                    '        lblErrMsgSecurity.Text = "Check this email address for access credentials in to the portal security :" + contactEmail
                    '    End If
                    'End If
                    If (SessionHelper.SelectedUserRole = Enums.RoleLevelCategory.QARN_RLC) Then
                        txtEndDate.Value = CalcEndDate(Today, 2) ' 2 value is for RN initial , 1 or 11 for DD, 12 is for RN renewal
                    End If
                    If ((CDate(Trim(txtEndDate.Value)) > Today) And
                        ((ddAdminStatus.SelectedValue = EnumHelper.GetEnumDescription(Enums.CertificationStatusType.Certified)) OrElse
                         (ddAdminStatus.SelectedValue = EnumHelper.GetEnumDescription(Enums.CertificationStatusType.Registered)) OrElse
                         (ddAppStatus.SelectedValue = EnumHelper.GetEnumDescription(Enums.ApplicationStatusType.Certified)) OrElse
                         (ddAppStatus.SelectedValue = EnumHelper.GetEnumDescription(Enums.ApplicationStatusType.AddedToRegistry)) OrElse
                         (ddAppStatus.SelectedValue = EnumHelper.GetEnumDescription(Enums.ApplicationStatusType.MeetsRequirements)))) Then

                        Dim userSVC As New UserRequestService.AEGIS_WS
                        retMsg = userSVC.CreateNewUser_Nurse(lblFirstName.Text, lblLastName.Text, contactEmail, ConfigHelper.BUCode, lblRNLNoOrSSNtxt.Text, CDate(Trim(txtEndDate.Value)), True)
                        If retMsg.Contains("error") Then
                            lblErrMsgSecurity.Text = "Error in web service to create security account for New RN,Please contact Adminstrator"
                        Else
                            lblErrMsgSecurity.Text = "Check this email address for access credentials in to the portal security :" + contactEmail
                        End If
                    End If
                ElseIf ((RNorDD = True) And (SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Enums.ApplicationType.Renewal)) And
                        (Not String.IsNullOrWhiteSpace(SessionID)) And (SessionID.Contains("RN"))) Then
                    'Need to do a renewing security access
                    If ((CDate(Trim(txtEndDate.Value)) > Today) And
                       ((ddAdminStatus.SelectedValue = EnumHelper.GetEnumDescription(Enums.CertificationStatusType.Certified)) OrElse
                        (ddAdminStatus.SelectedValue = EnumHelper.GetEnumDescription(Enums.CertificationStatusType.Registered)) OrElse
                        (ddAppStatus.SelectedValue = EnumHelper.GetEnumDescription(Enums.ApplicationStatusType.Certified)) OrElse
                        (ddAppStatus.SelectedValue = EnumHelper.GetEnumDescription(Enums.ApplicationStatusType.AddedToRegistry)) OrElse
                        (ddAppStatus.SelectedValue = EnumHelper.GetEnumDescription(Enums.ApplicationStatusType.MeetsRequirements)))) Then
                        Dim userSVC As New UserRequestService.AEGIS_WS
                        retMsg = userSVC.UpdateNurse(lblRNLNoOrSSNtxt.Text, CDate(Trim(txtEndDate.Value)), True)
                        If retMsg.Contains("error") Then
                            lblErrMsgSecurity.Text = "Error in web service to create security account for renewing RN,Please contact Adminstrator"
                        Else
                            lblErrMsgSecurity.Text = "Check this email address for access credentials in to the portal security :" + contactEmail
                        End If
                    End If
                End If
            End If


            'If (SessionHelper.ApplicationType <> EnumHelper.GetEnumDescription(ApplicationType.UpdateProfile)) Then
            '    If (ddAppStatus.SelectedItem IsNot Nothing) Then
            '        If (ddAppStatus.SelectedItem.Text = EnumHelper.GetEnumDescription(ApplicationStatusType.MeetsRequirements)) Then
            '            If (SessionHelper.SelectedUserRole <> Enums.RoleLevelCategory.RNMaster_RLC And SessionHelper.SelectedUserRole <> Enums.RoleLevelCategory.QARN_RLC) Then
            '                GenerateCertificate()
            '            End If
            '        End If
            '    Else
            '        If (ddAdminStatus.SelectedItem IsNot Nothing) Then
            '            If ((ddAdminStatus.SelectedItem.Text = EnumHelper.GetEnumDescription(ApplicationStatusType.MeetsRequirements)) Or (ddAdminStatus.SelectedItem.Text = EnumHelper.GetEnumDescription(ApplicationStatusType.Certified))) Then
            '                If (SessionHelper.SelectedUserRole <> Enums.RoleLevelCategory.RNMaster_RLC And SessionHelper.SelectedUserRole <> Enums.RoleLevelCategory.QARN_RLC) Then
            '                    GenerateCertificate()
            '                End If
            '            End If
            '        End If
            '    End If
            '    If (ddAdminStatus.SelectedItem IsNot Nothing) Then
            '        If ((ddAdminStatus.SelectedItem.Text = EnumHelper.GetEnumDescription(ApplicationStatusType.MeetsRequirements)) Or (ddAdminStatus.SelectedItem.Text = EnumHelper.GetEnumDescription(ApplicationStatusType.Certified))) Then
            '            If (SessionHelper.SelectedUserRole <> Enums.RoleLevelCategory.RNMaster_RLC And SessionHelper.SelectedUserRole <> Enums.RoleLevelCategory.QARN_RLC) Then
            '                GenerateCertificate()
            '            End If
            '        End If
            '    End If
            'Else
            '    Dim certificate As ICertificateService = StructureMap.ObjectFactory.GetInstance(Of ICertificateService)()  'New CertificateService()
            '    For Each mstring As ReturnMessage In res.GeneralMessages
            '        If (mstring.Message.Contains("Name Changed")) Then
            '            Dim certFicateDetails As New List(Of Model.CertificationDetails)
            '            certFicateDetails = certificate.GetCertificateDetailsInfo(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg)
            '            For Each certDetails As Model.CertificationDetails In certFicateDetails
            '                GetAllCertificates(certDetails)
            '            Next
            '        End If
            '    Next
            'End If
            'If NotMoveAllToPerm = False Then
            '    If Master.UserSessionMatch("At Summary Page Line 869") = False Then
            '        rte = Msvc.SendToErrorLog("At Summary Page Line 869 Session AppicationID " + SessionHelper.ApplicationID + " Master appid=" + Master.ApplicationID + " session rnddcode=" + SessionHelper.SessionUniqueID + " Master code:" + Master.RNLicenseOrSSN)
            '        Exit Sub
            '    End If
            '    AppToPdfAndDB()

            '    If (SessionHelper.ApplicationType <> EnumHelper.GetEnumDescription(ApplicationType.UpdateProfile)) Then
            '        If (ddAppStatus.SelectedItem IsNot Nothing) Then
            '            If (ddAppStatus.SelectedItem.Text <> EnumHelper.GetEnumDescription(ApplicationStatusType.DODD_Review)) Then
            '                If GenerateEmailNotification(ddAppStatus.SelectedItem.Text) = -1 Then
            '                    'Exit Sub
            '                End If
            '            Else
            '                If (ddAdminStatus.SelectedItem IsNot Nothing) Then
            '                    If GenerateEmailNotification(ddAdminStatus.SelectedItem.Text) = -1 Then
            '                        'Exit Sub
            '                    End If
            '                End If
            '            End If
            '        End If
            '    Else
            '        For Each mstring As ReturnMessage In res.GeneralMessages
            '            If (mstring.Message.Contains("Name Changed")) Then
            '                If GenerateEmailForUpdateProfile() = -1 Then
            '                    'Exit Sub
            '                End If
            '            End If
            '        Next
            '    End If
            '    If Master.UserSessionMatch("At Summary Page Line 844 all application procedures from Line 721 to 845") = False Then
            '        rte = Msvc.SendToErrorLog("At Summary Page Line 844 all application procedures from Line 721 to 845" + SessionHelper.ApplicationID + " Master appid=" + Master.ApplicationID + " session rnddcode=" + SessionHelper.SessionUniqueID + " Master code:" + Master.RNLicenseOrSSN)
            '        Exit Sub
            '    End If
            '    'AppToPdfAndDB()
            '    wsUDS = New UDSWebService.UDSService() With {.Url = ConfigHelper.UDSServiceEndpoint}


            '    If Master.UserSessionMatch("At Summary Page Line 850 after the SaveToUDS code call.") = False Then
            '        rte = Msvc.SendToErrorLog("At Summary Page Line 850 after the SaveToUDS code call." + SessionHelper.ApplicationID + " Master appid=" + Master.ApplicationID + " session rnddcode=" + SessionHelper.SessionUniqueID + " Master code:" + Master.RNLicenseOrSSN)
            '        Exit Sub
            '    End If
            '    SaveToUDS(AppID)
            '    If Master.UserSessionMatch("At Summary Page Line 852 after the AppToPDFAndUDS code call") = False Then
            '        rte = Msvc.SendToErrorLog("At Summary Page Line 852 after the AppToPDFAndUDS code call" + SessionHelper.ApplicationID + " Master appid=" + Master.ApplicationID + " session rnddcode=" + SessionHelper.SessionUniqueID + " Master code:" + Master.RNLicenseOrSSN)
            '        Exit Sub
            '    End If
            'End If

        End If

        divSpinner.Style("display") = "none"
        Dim ndc As Integer = NeedCert(SessionHelper.SelectedUserRole, fstat, SessionHelper.ApplicationType)
        If (ndc = 2 Or ndc = 0) Then
            Response.Redirect("Summary.aspx?savedstatus=" & fstat)
        Else
            Response.Redirect(PagesHelper.GetNextPage(Master.CurrentPage))
        End If

    End Sub

    Protected Sub CreateSummary_Cert_SaveToUDS(ByVal MyAppID As Integer, ByVal MyAppType As String, ByVal MySessionUniqueID As String, ByVal MyUserRole As Integer, ByVal MyRn_FLg As Boolean, ByVal res As ReturnObject(Of Long), ByVal MyContextEmail As String, ByVal MyIssuedofDoBtxt As String, ByVal RNorSSNumberTxt As String)

        '1. Create the summary page 
        AppToPdfAndDB(MyAppID, MySessionUniqueID, MyAppType, MyUserRole, MyRn_FLg)
        Msvc.SendToErrorLog("ApptoPdfandDatabase passing in AppID=" & MyAppID & " session.applicationId=" + CStr(SessionHelper.ApplicationID) + " Master appid=" + CStr(Master.ApplicationID) + " session rnddcode=" + SessionHelper.SessionUniqueID + " Master Page RNLable " + Master.RNLicenseOrSSN)

        '2. Create the Certification for the application



        If (MyAppType <> EnumHelper.GetEnumDescription(ApplicationType.UpdateProfile)) Then
            If (ddAppStatus.SelectedItem IsNot Nothing) Then
                If (ddAppStatus.SelectedItem.Text = EnumHelper.GetEnumDescription(ApplicationStatusType.MeetsRequirements)) Then
                    If (MyUserRole <> Enums.RoleLevelCategory.RNMaster_RLC And MyUserRole <> Enums.RoleLevelCategory.QARN_RLC) Then
                        'GenerateCertificate()
                        GenerateCertificate(MyAppID, MyUserRole, MyAppType, MySessionUniqueID)
                    End If
                End If
            Else
                If (ddAdminStatus.SelectedItem IsNot Nothing) Then
                    If ((ddAdminStatus.SelectedItem.Text = EnumHelper.GetEnumDescription(ApplicationStatusType.MeetsRequirements)) Or (ddAdminStatus.SelectedItem.Text = EnumHelper.GetEnumDescription(ApplicationStatusType.Certified))) Then
                        If (MyUserRole <> Enums.RoleLevelCategory.RNMaster_RLC And MyUserRole <> Enums.RoleLevelCategory.QARN_RLC) Then
                            'GenerateCertificate()
                            GenerateCertificate(MyAppID, MyUserRole, MyAppType, MySessionUniqueID)
                        End If
                    End If
                End If
            End If
            If (ddAdminStatus.SelectedItem IsNot Nothing) Then
                If ((ddAdminStatus.SelectedItem.Text = EnumHelper.GetEnumDescription(ApplicationStatusType.MeetsRequirements)) Or (ddAdminStatus.SelectedItem.Text = EnumHelper.GetEnumDescription(ApplicationStatusType.Certified))) Then
                    If (MyUserRole <> Enums.RoleLevelCategory.RNMaster_RLC And MyUserRole <> Enums.RoleLevelCategory.QARN_RLC) Then
                        'GenerateCertificate()
                        GenerateCertificate(MyAppID, MyUserRole, MyAppType, MySessionUniqueID)
                    End If
                End If
            End If
        Else
            Dim certificate As ICertificateService = StructureMap.ObjectFactory.GetInstance(Of ICertificateService)()  'New CertificateService()
            For Each mstring As ReturnMessage In res.GeneralMessages
                If (mstring.Message.Contains("Name Changed")) Then
                    Dim certFicateDetails As New List(Of Model.CertificationDetails)
                    certFicateDetails = certificate.GetCertificateDetailsInfo(MySessionUniqueID, MyRn_FLg)
                    For Each certDetails As Model.CertificationDetails In certFicateDetails
                        GetAllCertificates(certDetails, MyAppID, MySessionUniqueID)
                    Next
                End If
            Next
        End If
        Msvc.SendToErrorLog("After Certificate created: session.applicationId=" + CStr(SessionHelper.ApplicationID) + " Master appid=" + CStr(Master.ApplicationID) + " session rnddcode=" + SessionHelper.SessionUniqueID + " Master Page RNLable " + Master.RNLicenseOrSSN)

        '3. Save to UDS 
        wsUDS = New UDSWebService.UDSService() With {.Url = ConfigHelper.UDSServiceEndpoint}
        SaveToUDS(MyAppID, MyAppType, MySessionUniqueID, MyUserRole, MyRn_FLg, MyIssuedofDoBtxt, RNorSSNumberTxt)

        Msvc.SendToErrorLog("After SaveToUDS: passing in AppID=" & MyAppID & " session.applicationId=" + CStr(SessionHelper.ApplicationID) + " Master appid=" + CStr(Master.ApplicationID) + " session rnddcode=" + SessionHelper.SessionUniqueID + " Master Page RNLable " + Master.RNLicenseOrSSN)


        '4 Send email
        If (MyAppType <> EnumHelper.GetEnumDescription(ApplicationType.UpdateProfile)) Then
            If (ddAppStatus.SelectedItem IsNot Nothing) Then
                If (ddAppStatus.SelectedItem.Text <> EnumHelper.GetEnumDescription(ApplicationStatusType.DODD_Review)) Then
                    If GenerateEmailNotification(ddAppStatus.SelectedItem.Text, MyAppID, MyContextEmail) = -1 Then
                        'Exit Sub
                    End If
                Else
                    If (ddAdminStatus.SelectedItem IsNot Nothing) Then
                        If GenerateEmailNotification(ddAdminStatus.SelectedItem.Text, MyAppID, MyContextEmail) = -1 Then
                            'Exit Sub
                        End If
                    End If
                End If
            End If
        Else
            For Each mstring As ReturnMessage In res.GeneralMessages
                If (mstring.Message.Contains("Name Changed")) Then
                    If GenerateEmailForUpdateProfile(MyAppID, MyContextEmail) = -1 Then
                        'Exit Sub
                    End If
                End If
            Next
        End If

    End Sub

    Private Function GenerateEmailForUpdateProfile(ByVal MyAppID As Integer, ByVal MyContextMail As String) As Integer
        Dim retVal As String = String.Empty
        Dim strToEmailAddress As String = Nothing
        Dim strCCEmailAddress As String = Nothing
        strCCEmailAddress = ConfigHelper.CCSummaryEmailAddress
        strToEmailAddress = MyContextMail
        'If Not String.IsNullOrEmpty(ConfigHelper.ToEmailAddress) Then
        '    strToEmailAddress = ConfigHelper.ToEmailAddress
        'Else
        '    If (Not String.IsNullOrEmpty(lblHomeAddress.Text)) Then
        '        strToEmailAddress = lblHomeAddress.Text
        '    Else
        '        If (Not String.IsNullOrEmpty(lblWorkAddress.Text)) Then
        '            strToEmailAddress = lblWorkAddress.Text
        '        Else
        '            strToEmailAddress = lblCellAddress.Text
        '        End If
        '    End If
        'End If
        Dim emailSvc As IEmailService = StructureMap.ObjectFactory.GetInstance(Of IEmailService)()
        Dim uploadedFiles As New List(Of Business.Model.DocumentUpload)
        Dim uploadSvc As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()

        uploadedFiles = uploadSvc.GetUploadedDocuments(MyAppID) 'AppID
        uploadedFiles = uploadedFiles.FindAll(Function(up) (up.DocumentTypeID = 9 OrElse up.DocumentName.Contains("ApplicationSummary")))
        For Each df As Business.Model.DocumentUpload In uploadedFiles
            df.DocumentImage = uploadSvc.GetUploadedDocumentByImageSID(df.ImageSID)
        Next

        'attachmentFiles.Add(uploadedFiles(0).DocumentName)
        Dim strBodyMessage As String = String.Empty
        strBodyMessage = "<b>Congratulations!</b><br/><br/>"
        strBodyMessage = strBodyMessage + "Your  information has been successfully entered into  the DODD Medication Administration Information System (MAIS).<br/><br/>"
        strBodyMessage = strBodyMessage + "If you earned a certification it is attached to this e-mail.<br/><br/>"
        strBodyMessage = strBodyMessage + "<b><u>Please Note:</u></b><br/><br/>"
        strBodyMessage = strBodyMessage + "<b><u>All DD Personnel Certifications</u></b> expire 1 year from the date of certification. " +
                                            "DD Personnel and employers should always confirm current DODD certification is active any time the Personnel is assigned to administer medications. " +
                                            "Public access to certification dates and status is available at any time on the DODD Home Page.<br/><br/>"
        strBodyMessage = strBodyMessage + "<b><u>All RN Certifications and Registrations</u></b> <b>expire every 2 years on Aug. 31st of the odd numbered years.</b> " +
                                            "You can enter applicable CEUs to your record at any time, you must update your personal and employer contact information within 30 days of any changes and notify DODD immediately of any license restrictions. " +
                                            "You will need to log into the MAIS system to enter your renewal request starting 6 months prior to expiration. No late renewals will be accepted.<br/><br/>"
        strBodyMessage = strBodyMessage + "You should feel free to contact your RN Trainer " + UserAndRoleHelper.CurrentUser.Email + " or DODD if you have any questions or concerns about this certification or safe performance of medication administration and health related activities.<br/><br/>"
        strBodyMessage = strBodyMessage + "Thank you"
        Dim retObj As ReturnObject(Of Boolean) = emailSvc.SendEmail1(strToEmailAddress.Trim(),
                                                                    ConfigHelper.FromEmailAddress,
                                                                    ConfigHelper.EmailSubjectStatus,
                                                                    strBodyMessage, uploadedFiles, strCCEmailAddress.Trim())

        If retObj.ReturnValue Then
            retVal = "An email containing your status of this application was sent successfully."
        Else
            If retObj.Messages.Count > 0 Then
                retVal = retObj.Messages.FirstOrDefault().Message
            Else
                retVal = "ERROR: An error has occurred while trying to send an email for QA registeration."
            End If
            pError.Visible = True
            pError.InnerHtml += ("</br>" & retVal)
            Return -1
        End If
        Return 0
        'pnote.Visible = True
        'lblNote.Text = retVal
        'pnote.Style("display") = "inline"
        'pnote.InnerHtml = retVal
    End Function
    Private Function GenerateEmailNotification(ByVal status As String, ByVal MyAppID As Integer, ByVal MyContextMail As String) As Integer
        Dim retVal As String = String.Empty
        Dim strToEmailAddress As String = Nothing
        Dim strCCEmailAddress As String = Nothing
        strCCEmailAddress = ConfigHelper.CCSummaryEmailAddress
        strToEmailAddress = MyContextMail
        'If Not String.IsNullOrEmpty(ConfigHelper.ToEmailAddress) Then
        '    strToEmailAddress = ConfigHelper.ToEmailAddress
        'Else
        '    If (Not String.IsNullOrEmpty(lblHomeAddress.Text)) Then
        '        strToEmailAddress = lblHomeAddress.Text
        '    Else
        '        If (Not String.IsNullOrEmpty(lblWorkAddress.Text)) Then
        '            strToEmailAddress = lblWorkAddress.Text
        '        Else
        '            strToEmailAddress = lblCellAddress.Text
        '        End If
        '    End If
        'End If
        Dim emailSvc As IEmailService = StructureMap.ObjectFactory.GetInstance(Of IEmailService)()
        Dim uploadedFiles As New List(Of Business.Model.DocumentUpload)
        Dim uploadSvc As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()

        uploadedFiles = uploadSvc.GetUploadedDocuments(MyAppID) 'AppID
        uploadedFiles = uploadedFiles.FindAll(Function(up) (up.DocumentTypeID = 9 OrElse up.DocumentName.Contains("ApplicationSummary")))
        For Each df As Business.Model.DocumentUpload In uploadedFiles
            df.DocumentImage = uploadSvc.GetUploadedDocumentByImageSID(df.ImageSID)
        Next

        If (status = EnumHelper.GetEnumDescription(ApplicationStatusType.AddedToRegistry)) Then
            Dim strBodyMessage As String = "You have been registered as a DODD QA RN.<br/><br/>"
            strBodyMessage = strBodyMessage + "Thank you!"
            Dim retObj As ReturnObject(Of Boolean) = emailSvc.SendEmail1(strToEmailAddress.Trim(),
                                                                        ConfigHelper.FromEmailAddress,
                                                                        ConfigHelper.EmailSubjectStatus,
                                                                        strBodyMessage, uploadedFiles, strCCEmailAddress.Trim())

            If retObj.ReturnValue Then
                retVal = "An email containing your status of this application was sent successfully."
            Else
                If retObj.Messages.Count > 0 Then
                    retVal = retObj.MessageStrings.First()
                Else
                    retVal = "ERROR: An error has occurred while trying to send an email for QA registeration."
                End If
                pError.Visible = True
                pError.InnerHtml += ("</br>" & retVal)
                Return -1
            End If
        End If
        If (status = EnumHelper.GetEnumDescription(ApplicationStatusType.RemovedFromRegistry)) Then
            Dim strBodyMessage As String = "You have been unregistered as a DODD QA RN.<br/><br/>"
            strBodyMessage = strBodyMessage + "Thank you!"
            Dim retObj As ReturnObject(Of Boolean) = emailSvc.SendEmail1(strToEmailAddress.Trim(),
                                                                        ConfigHelper.FromEmailAddress,
                                                                        ConfigHelper.EmailSubjectStatus,
                                                                        strBodyMessage, uploadedFiles, strCCEmailAddress.Trim())

            If retObj.ReturnValue Then
                retVal = "An email containing your status of this application was sent successfully."
            Else
                If retObj.Messages.Count > 0 Then
                    retVal = retObj.MessageStrings.First()
                Else
                    retVal = "ERROR: An error has occurred while trying to send an email for QA unregisteration."
                End If
                pError.Visible = True
                pError.InnerHtml += ("</br>" & retVal)
                Return -1
            End If
        End If
        If (status = EnumHelper.GetEnumDescription(ApplicationStatusType.DNMR)) Then
            Dim strBodyMessage As String = String.Empty
            strBodyMessage = "You did not meet requirements for this application.<br/><br/>"
            strBodyMessage = strBodyMessage + "Please contact RN Trainer " + UserAndRoleHelper.CurrentUser.UserName + " at the following email address: " + UserAndRoleHelper.CurrentUser.Email _
                                        + " for additional information regarding Medication Administration training.<br/><br/>"
            strBodyMessage = strBodyMessage + "Thank you!"
            Dim retObj As ReturnObject(Of Boolean) = emailSvc.SendEmail1(strToEmailAddress.Trim(),
                                                                        ConfigHelper.FromEmailAddress,
                                                                        ConfigHelper.EmailSubjectStatus,
                                                                        strBodyMessage, uploadedFiles, strCCEmailAddress.Trim())

            If retObj.ReturnValue Then
                retVal = "An email containing your status of this application was sent successfully."
            Else
                If retObj.Messages.Count > 0 Then
                    retVal = retObj.MessageStrings.First()
                Else
                    retVal = "ERROR: An error has occurred while trying to send an email for QA registeration."
                End If
                pError.Visible = True
                pError.InnerHtml += ("</br>" & retVal)
                Return -1
            End If
        End If
        If (status = EnumHelper.GetEnumDescription(ApplicationStatusType.Denied)) Then
            Dim strBodyMessage As String = String.Empty
            strBodyMessage = "You are denied for this training.<br/><br/>"
            strBodyMessage = strBodyMessage + "Please contact RN Trainer " + UserAndRoleHelper.CurrentUser.UserName + " at the following email address: " + UserAndRoleHelper.CurrentUser.Email _
                                        + " for additional information regarding Medication Administration training.<br/><br/>"
            strBodyMessage = strBodyMessage + "Thank you!"
            Dim retObj As ReturnObject(Of Boolean) = emailSvc.SendEmail1(strToEmailAddress.Trim(),
                                                                        ConfigHelper.FromEmailAddress,
                                                                        ConfigHelper.EmailSubjectStatus,
                                                                        strBodyMessage, uploadedFiles, strCCEmailAddress.Trim())

            If retObj.ReturnValue Then
                retVal = "An email containing your status of this application was sent successfully."
            Else
                If retObj.Messages.Count > 0 Then
                    retVal = retObj.MessageStrings.First()
                Else
                    retVal = "ERROR: An error has occurred while trying to send an email for QA registeration."
                End If
                pError.Visible = True
                pError.InnerHtml += ("</br>" & retVal)
                Return -1
            End If
        End If
        If (status = EnumHelper.GetEnumDescription(ApplicationStatusType.MeetsRequirements) Or status = EnumHelper.GetEnumDescription(ApplicationStatusType.Certified)) Then
            'Dim attachmentFiles As New List(Of String)()

            'attachmentFiles.Add(uploadedFiles(0).DocumentName)
            Dim strBodyMessage As String = String.Empty
            strBodyMessage = "<b>Congratulations!</b><br/><br/>"
            strBodyMessage = strBodyMessage + "input application ID " & CStr(MyAppID) & " Master page Application ID : " & CStr(Master.ApplicationID) & " Session Application ID: " & CStr(SessionHelper.ApplicationID)
            strBodyMessage = strBodyMessage + "Your  information has been successfully entered into  the DODD Medication Administration Information System (MAIS).<br/><br/>"
            strBodyMessage = strBodyMessage + "If you earned a certification it is attached to this e-mail.<br/><br/>"
            strBodyMessage = strBodyMessage + "<b><u>Please Note:</u></b><br/><br/>"
            strBodyMessage = strBodyMessage + "<b><u>All DD Personnel Certifications</u></b> expire 1 year from the date of certification. " +
                                                "DD Personnel and employers should always confirm current DODD certification is active any time the Personnel is assigned to administer medications. " +
                                                "Public access to certification dates and status is available at any time on the DODD Home Page.<br/><br/>"
            strBodyMessage = strBodyMessage + "<b><u>All RN Certifications and Registrations</u></b> <b>expire every 2 years on Aug. 31st of the odd numbered years.</b> " +
                                                "You can enter applicable CEUs to your record at any time, you must update your personal and employer contact information within 30 days of any changes and notify DODD immediately of any license restrictions. " +
                                                "You will need to log into the MAIS system to enter your renewal request starting 6 months prior to expiration. No late renewals will be accepted.<br/><br/>"
            strBodyMessage = strBodyMessage + "You should feel free to contact your RN Trainer " + UserAndRoleHelper.CurrentUser.Email + " or DODD if you have any questions or concerns about this certification or safe performance of medication administration and health related activities.<br/><br/>"
            strBodyMessage = strBodyMessage + "Thank you"
            Dim retObj As ReturnObject(Of Boolean) = emailSvc.SendEmail1(strToEmailAddress.Trim(),
                                                                        ConfigHelper.FromEmailAddress,
                                                                        ConfigHelper.EmailSubjectStatus,
                                                                        strBodyMessage, uploadedFiles, strCCEmailAddress.Trim())

            If retObj.ReturnValue Then
                retVal = "An email containing your status of this application was sent successfully."
                Dim summaryService As ISummaryService = StructureMap.ObjectFactory.GetInstance(Of ISummaryService)()
                Dim flag As Boolean = summaryService.GetEmailDateInHistory(AppID)
                If (flag = False) Then
                    retVal = "An error occurred while update application history for email"
                End If
            Else
                If retObj.Messages.Count > 0 Then
                    retVal = retObj.MessageStrings.First()
                Else
                    retVal = "ERROR: An error has occurred while trying to send an email for QA registeration."
                End If
                pError.Visible = True
                pError.InnerHtml += ("</br>" & retVal)
                Return -1
            End If
        End If
        Return 0
        'pnote.Visible = True
        'lblNote.Text = retVal
        'pnote.Style("display") = "inline"
        'pnote.InnerHtml = retVal
    End Function

    Private Sub GenerateCertificate(ByVal MyAppID As Integer, ByVal myUserRole As Integer, ByVal MyAppType As String, ByVal MySessionUniqueID As String)
        Select Case myUserRole
            Case 4
                _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.RNTrainer_RLC)
            Case 5
                _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.RNInstructor_RLC)
            Case 6
                _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.RNMaster_RLC)
            Case 7
                _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.QARN_RLC)
            Case 8
                _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.Bed17_RLC)
            Case 15
                _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.DDPersonnel_RLC)
            Case 16
                _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.DDPersonnel2_RLC)
            Case 17
                _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.DDPersonnel3_RLC)
        End Select

        Dim certificate As ICertificateService = StructureMap.ObjectFactory.GetInstance(Of ICertificateService)() 'New CertificateService()
        'StructureMap.ObjectFactory.GetInstance(Of ICertificateService)()
        rvCertificate.ProcessingMode = ProcessingMode.Local
        Dim permissions As PermissionSet = New PermissionSet(PermissionState.Unrestricted)
        rvCertificate.LocalReport.SetBasePermissionsForSandboxAppDomain(permissions)
        If ((myUserRole = _rntrainer Or myUserRole = RoleLevelCategory.Bed17_RLC) And (MyAppType <> EnumHelper.GetEnumDescription(ApplicationType.Renewal))) Then
            Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfo(MySessionUniqueID, MyAppType, myUserRole))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", MyAppID)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", MyAppType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", MySessionUniqueID)
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/RNTrainerCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
        End If
        If ((myUserRole = RoleLevelCategory.RNInstructor_RLC) And (MyAppType <> EnumHelper.GetEnumDescription(ApplicationType.Renewal))) Then
            Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfo(MySessionUniqueID, MyAppType, myUserRole))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", MyAppID)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", MyAppType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", MySessionUniqueID)
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/RNInstructorCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
        End If
        If ((myUserRole = RoleLevelCategory.RNInstructor_RLC) And (MyAppType = EnumHelper.GetEnumDescription(ApplicationType.Renewal))) Then
            Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfo(MySessionUniqueID, MyAppType, myUserRole))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", MyAppID)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", MyAppType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", MySessionUniqueID)
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/RNInstructorRenewalCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
        End If
        If ((myUserRole = _rntrainer Or myUserRole = RoleLevelCategory.Bed17_RLC) And MyAppType = EnumHelper.GetEnumDescription(ApplicationType.Renewal)) Then
            Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfo(MySessionUniqueID, MyAppType, myUserRole))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", MyAppID)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", MyAppType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", MySessionUniqueID)
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/RNTrainerRenewalCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
        End If
        If ((myUserRole = RoleLevelCategory.DDPersonnel_RLC Or myUserRole = RoleLevelCategory.DDPersonnel2_RLC Or myUserRole = RoleLevelCategory.DDPersonnel3_RLC) And MyAppType <> EnumHelper.GetEnumDescription(ApplicationType.Renewal)) Then
            Dim params(4) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetDDCertificateInfo(MySessionUniqueID, MyAppType, myUserRole))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", MyAppID)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", MyAppType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("DDCategoryCode", MySessionUniqueID)
            params(4) = New Microsoft.Reporting.WebForms.ReportParameter("TodayDate", DateTime.Today.ToShortDateString())
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/DDCategoryInitialCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
        End If
        If ((myUserRole = RoleLevelCategory.DDPersonnel_RLC Or myUserRole = RoleLevelCategory.DDPersonnel2_RLC Or myUserRole = RoleLevelCategory.DDPersonnel3_RLC) And MyAppType = EnumHelper.GetEnumDescription(ApplicationType.Renewal)) Then
            Dim params(4) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetDDCertificateInfo(MySessionUniqueID, MyAppType, myUserRole))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", MyAppID)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", MyAppType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("DDCode", MySessionUniqueID)
            params(4) = New Microsoft.Reporting.WebForms.ReportParameter("TodayDate", DateTime.Today.ToShortDateString())
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/DDCategoryRenewalCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
        End If
        Dim warnings As Warning() = Nothing
        Dim streamids As String() = Nothing
        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing
        Dim myBytes As Byte()

        myBytes = rvCertificate.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)
        Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
        Dim uploadDocumentNumber As Long = uploadService.InsertUploadedDocumentIntoImageStore(myBytes)
        If uploadDocumentNumber > 0 Then
            Dim uploadedDoc As New Business.Model.DocumentUpload()

            'Generate model for database information
            With uploadedDoc
                .ImageSID = uploadDocumentNumber
                .DocumentName = MyAppID & "_" & MyAppType & "_" & DateString & ".pdf"
                .DocumentType = "9"
            End With
            ' Save file info to database
            uploadService.InsertUploadedDocument(uploadedDoc, MyAppID) 'AppID should be used

        End If
        rvCertificate.LocalReport.Refresh()
        rvCertificate.Reset()
    End Sub

    Private Sub GetAllCertificates(ByVal certDetails As Model.CertificationDetails, ByVal MyAppID As Integer, ByVal MySessionUniqueID As String)
        Dim certificate As ICertificateService = StructureMap.ObjectFactory.GetInstance(Of ICertificateService)() 'New CertificateService()
        Select Case certDetails.RoleLevelCategoryID
            Case 4
                _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.RNTrainer_RLC)
            Case 5
                _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.RNInstructor_RLC)
            Case 6
                _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.RNMaster_RLC)
            Case 7
                _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.QARN_RLC)
            Case 8
                _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.Bed17_RLC)
            Case 15
                _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.DDPersonnel_RLC)
            Case 16
                _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.DDPersonnel2_RLC)
            Case 17
                _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.DDPersonnel3_RLC)
        End Select
        rvCertificate.ProcessingMode = ProcessingMode.Local
        Dim permissions As PermissionSet = New PermissionSet(PermissionState.Unrestricted)
        rvCertificate.LocalReport.SetBasePermissionsForSandboxAppDomain(permissions)
        If ((certDetails.RoleLevelCategoryID = _rntrainer Or certDetails.RoleLevelCategoryID = RoleLevelCategory.Bed17_RLC) And (certDetails.ApplicationType <> EnumHelper.GetEnumDescription(ApplicationType.Renewal))) Then
            Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfo(SessionID, certDetails.ApplicationType, certDetails.RoleLevelCategoryID))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", MyAppID)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", certDetails.ApplicationType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", MySessionUniqueID)
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/RNTrainerCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
            Dim warnings As Warning() = Nothing
            Dim streamids As String() = Nothing
            Dim mimeType As String = Nothing
            Dim encoding As String = Nothing
            Dim extension As String = Nothing
            Dim myBytes As Byte()

            myBytes = rvCertificate.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)
            Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
            Dim uploadDocumentNumber As Long = uploadService.InsertUploadedDocumentIntoImageStore(myBytes)
            If uploadDocumentNumber > 0 Then
                Dim uploadedDoc As New Business.Model.DocumentUpload()

                'Generate model for database information
                With uploadedDoc
                    .ImageSID = uploadDocumentNumber
                    .DocumentName = MyAppID & "_" & certDetails.ApplicationType & "_" & _roles & "_" & DateString & ".pdf"
                    .DocumentType = "9"
                End With
                ' Save file info to database
                uploadService.InsertUploadedDocument(uploadedDoc, MyAppID) 'AppID should be used

            End If
            rvCertificate.LocalReport.Refresh()
            rvCertificate.Reset()
        End If
        If ((certDetails.RoleLevelCategoryID = RoleLevelCategory.RNInstructor_RLC) And (certDetails.ApplicationType <> EnumHelper.GetEnumDescription(ApplicationType.Renewal))) Then
            Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfo(MySessionUniqueID, certDetails.ApplicationType, certDetails.RoleLevelCategoryID))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", MyAppID)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", certDetails.ApplicationType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", MySessionUniqueID)
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/RNInstructorCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
            Dim warnings As Warning() = Nothing
            Dim streamids As String() = Nothing
            Dim mimeType As String = Nothing
            Dim encoding As String = Nothing
            Dim extension As String = Nothing
            Dim myBytes As Byte()

            myBytes = rvCertificate.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)
            Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
            Dim uploadDocumentNumber As Long = uploadService.InsertUploadedDocumentIntoImageStore(myBytes)
            If uploadDocumentNumber > 0 Then
                Dim uploadedDoc As New Business.Model.DocumentUpload()

                'Generate model for database information
                With uploadedDoc
                    .ImageSID = uploadDocumentNumber
                    .DocumentName = MyAppID & "_" & certDetails.ApplicationType & "_" & _roles & "_" & DateString & ".pdf"
                    .DocumentType = "9"
                End With
                ' Save file info to database
                uploadService.InsertUploadedDocument(uploadedDoc, MyAppID) 'AppID should be used

            End If
            rvCertificate.LocalReport.Refresh()
            rvCertificate.Reset()
        End If
        If ((certDetails.RoleLevelCategoryID = RoleLevelCategory.RNInstructor_RLC) And (certDetails.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.Renewal))) Then
            Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfo(MySessionUniqueID, certDetails.ApplicationType, certDetails.RoleLevelCategoryID))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", MyAppID)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", certDetails.ApplicationType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", MySessionUniqueID)
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/RNInstructorRenewalCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
            Dim warnings As Warning() = Nothing
            Dim streamids As String() = Nothing
            Dim mimeType As String = Nothing
            Dim encoding As String = Nothing
            Dim extension As String = Nothing
            Dim myBytes As Byte()

            myBytes = rvCertificate.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)
            Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
            Dim uploadDocumentNumber As Long = uploadService.InsertUploadedDocumentIntoImageStore(myBytes)
            If uploadDocumentNumber > 0 Then
                Dim uploadedDoc As New Business.Model.DocumentUpload()

                'Generate model for database information
                With uploadedDoc
                    .ImageSID = uploadDocumentNumber
                    .DocumentName = AppID & "_" & certDetails.ApplicationType & "_" & _roles & "_" & DateString & ".pdf"
                    .DocumentType = "9"
                End With
                ' Save file info to database
                uploadService.InsertUploadedDocument(uploadedDoc, MyAppID) 'AppID should be used

            End If
            rvCertificate.LocalReport.Refresh()
            rvCertificate.Reset()
        End If
        If ((certDetails.RoleLevelCategoryID = _rntrainer Or certDetails.RoleLevelCategoryID = RoleLevelCategory.Bed17_RLC) And certDetails.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.Renewal)) Then
            Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfo(MySessionUniqueID, certDetails.ApplicationType, certDetails.RoleLevelCategoryID))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", MyAppID)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", certDetails.ApplicationType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", MySessionUniqueID)
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/RNTrainerRenewalCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
            Dim warnings As Warning() = Nothing
            Dim streamids As String() = Nothing
            Dim mimeType As String = Nothing
            Dim encoding As String = Nothing
            Dim extension As String = Nothing
            Dim myBytes As Byte()

            myBytes = rvCertificate.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)
            Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
            Dim uploadDocumentNumber As Long = uploadService.InsertUploadedDocumentIntoImageStore(myBytes)
            If uploadDocumentNumber > 0 Then
                Dim uploadedDoc As New Business.Model.DocumentUpload()

                'Generate model for database information
                With uploadedDoc
                    .ImageSID = uploadDocumentNumber
                    .DocumentName = AppID & "_" & certDetails.ApplicationType & "_" & _roles & "_" & DateString & ".pdf"
                    .DocumentType = "9"
                End With
                ' Save file info to database
                uploadService.InsertUploadedDocument(uploadedDoc, MyAppID) 'AppID should be used

            End If
            rvCertificate.LocalReport.Refresh()
            rvCertificate.Reset()
        End If
        If ((certDetails.RoleLevelCategoryID = 15 Or certDetails.RoleLevelCategoryID = 16 Or certDetails.RoleLevelCategoryID = 17) And certDetails.ApplicationType <> EnumHelper.GetEnumDescription(ApplicationType.Renewal)) Then
            Dim params(4) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetDDCertificateInfo(MySessionUniqueID, certDetails.ApplicationType, certDetails.RoleLevelCategoryID))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", MyAppID)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", certDetails.ApplicationType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("DDCategoryCode", MySessionUniqueID)
            params(4) = New Microsoft.Reporting.WebForms.ReportParameter("TodayDate", DateTime.Today.ToShortDateString())
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/DDCategoryInitialCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
            Dim warnings As Warning() = Nothing
            Dim streamids As String() = Nothing
            Dim mimeType As String = Nothing
            Dim encoding As String = Nothing
            Dim extension As String = Nothing
            Dim myBytes As Byte()

            myBytes = rvCertificate.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)
            Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
            Dim uploadDocumentNumber As Long = uploadService.InsertUploadedDocumentIntoImageStore(myBytes)
            If uploadDocumentNumber > 0 Then
                Dim uploadedDoc As New Business.Model.DocumentUpload()

                'Generate model for database information
                With uploadedDoc
                    .ImageSID = uploadDocumentNumber
                    .DocumentName = AppID & "_" & certDetails.ApplicationType & "_" & _roles & "_" & DateString & ".pdf"
                    .DocumentType = "9"
                End With
                ' Save file info to database
                uploadService.InsertUploadedDocument(uploadedDoc, MyAppID) 'AppID should be used

            End If
            rvCertificate.LocalReport.Refresh()
            rvCertificate.Reset()
        End If
        If ((certDetails.RoleLevelCategoryID = 15 Or certDetails.RoleLevelCategoryID = 16 Or certDetails.RoleLevelCategoryID = 17) And certDetails.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.Renewal)) Then
            Dim params(4) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetDDCertificateInfo(MySessionUniqueID, certDetails.ApplicationType, certDetails.RoleLevelCategoryID))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", MyAppID)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", certDetails.ApplicationType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("DDCode", MySessionUniqueID)
            params(4) = New Microsoft.Reporting.WebForms.ReportParameter("TodayDate", DateTime.Today.ToShortDateString())
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/DDCategoryRenewalCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
            Dim warnings As Warning() = Nothing
            Dim streamids As String() = Nothing
            Dim mimeType As String = Nothing
            Dim encoding As String = Nothing
            Dim extension As String = Nothing
            Dim myBytes As Byte()

            myBytes = rvCertificate.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)
            Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
            Dim uploadDocumentNumber As Long = uploadService.InsertUploadedDocumentIntoImageStore(myBytes)
            If uploadDocumentNumber > 0 Then
                Dim uploadedDoc As New Business.Model.DocumentUpload()

                'Generate model for database information
                With uploadedDoc
                    .ImageSID = uploadDocumentNumber
                    .DocumentName = MyAppID & "_" & certDetails.ApplicationType & "_" & _roles & "_" & DateString & ".pdf"
                    .DocumentType = "9"
                End With
                ' Save file info to database
                uploadService.InsertUploadedDocument(uploadedDoc, MyAppID) 'AppID should be used

            End If
            rvCertificate.LocalReport.Refresh()
            rvCertificate.Reset()
        End If
    End Sub

    'Private Sub GenerateCertificate()
    '    Select Case SessionHelper.SelectedUserRole
    '        Case 4
    '            _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.RNTrainer_RLC)
    '        Case 5
    '            _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.RNInstructor_RLC)
    '        Case 6
    '            _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.RNMaster_RLC)
    '        Case 7
    '            _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.QARN_RLC)
    '        Case 8
    '            _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.Bed17_RLC)
    '        Case 15
    '            _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.DDPersonnel_RLC)
    '        Case 16
    '            _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.DDPersonnel2_RLC)
    '        Case 17
    '            _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.DDPersonnel3_RLC)
    '    End Select

    '    Dim certificate As ICertificateService = StructureMap.ObjectFactory.GetInstance(Of ICertificateService)() 'New CertificateService()
    '    'StructureMap.ObjectFactory.GetInstance(Of ICertificateService)()
    '    rvCertificate.ProcessingMode = ProcessingMode.Local
    '    Dim permissions As PermissionSet = New PermissionSet(PermissionState.Unrestricted)
    '    rvCertificate.LocalReport.SetBasePermissionsForSandboxAppDomain(permissions)
    '    If ((SessionHelper.SelectedUserRole = _rntrainer Or SessionHelper.SelectedUserRole = RoleLevelCategory.Bed17_RLC) And (SessionHelper.ApplicationType <> EnumHelper.GetEnumDescription(ApplicationType.Renewal))) Then
    '        Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

    '        rvCertificate.Reset()

    '        Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfo(SessionID, SessionHelper.ApplicationType, SessionHelper.SelectedUserRole))

    '        params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", AppID)
    '        params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", SessionHelper.ApplicationType)
    '        params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
    '        params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", SessionID)
    '        rvCertificate.LocalReport.Refresh()
    '        rvCertificate.LocalReport.ReportPath = "Reports/RNTrainerCertificate.rdlc"
    '        rvCertificate.LocalReport.SetParameters(params)
    '        rvCertificate.LocalReport.DataSources.Add(reportds)
    '        rvCertificate.LocalReport.Refresh()
    '    End If
    '    If ((SessionHelper.SelectedUserRole = RoleLevelCategory.RNInstructor_RLC) And (SessionHelper.ApplicationType <> EnumHelper.GetEnumDescription(ApplicationType.Renewal))) Then
    '        Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

    '        rvCertificate.Reset()

    '        Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfo(SessionID, SessionHelper.ApplicationType, SessionHelper.SelectedUserRole))

    '        params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", AppID)
    '        params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", SessionHelper.ApplicationType)
    '        params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
    '        params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", SessionID)
    '        rvCertificate.LocalReport.Refresh()
    '        rvCertificate.LocalReport.ReportPath = "Reports/RNInstructorCertificate.rdlc"
    '        rvCertificate.LocalReport.SetParameters(params)
    '        rvCertificate.LocalReport.DataSources.Add(reportds)
    '        rvCertificate.LocalReport.Refresh()
    '    End If
    '    If ((SessionHelper.SelectedUserRole = RoleLevelCategory.RNInstructor_RLC) And (SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.Renewal))) Then
    '        Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

    '        rvCertificate.Reset()

    '        Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfo(SessionID, SessionHelper.ApplicationType, SessionHelper.SelectedUserRole))

    '        params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", AppID)
    '        params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", SessionHelper.ApplicationType)
    '        params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
    '        params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", SessionID)
    '        rvCertificate.LocalReport.Refresh()
    '        rvCertificate.LocalReport.ReportPath = "Reports/RNInstructorRenewalCertificate.rdlc"
    '        rvCertificate.LocalReport.SetParameters(params)
    '        rvCertificate.LocalReport.DataSources.Add(reportds)
    '        rvCertificate.LocalReport.Refresh()
    '    End If
    '    If ((SessionHelper.SelectedUserRole = _rntrainer Or SessionHelper.SelectedUserRole = RoleLevelCategory.Bed17_RLC) And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.Renewal)) Then
    '        Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

    '        rvCertificate.Reset()

    '        Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfo(SessionID, SessionHelper.ApplicationType, SessionHelper.SelectedUserRole))

    '        params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", AppID)
    '        params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", SessionHelper.ApplicationType)
    '        params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
    '        params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", SessionID)
    '        rvCertificate.LocalReport.Refresh()
    '        rvCertificate.LocalReport.ReportPath = "Reports/RNTrainerRenewalCertificate.rdlc"
    '        rvCertificate.LocalReport.SetParameters(params)
    '        rvCertificate.LocalReport.DataSources.Add(reportds)
    '        rvCertificate.LocalReport.Refresh()
    '    End If
    '    If ((SessionHelper.SelectedUserRole = RoleLevelCategory.DDPersonnel_RLC Or SessionHelper.SelectedUserRole = RoleLevelCategory.DDPersonnel2_RLC Or SessionHelper.SelectedUserRole = RoleLevelCategory.DDPersonnel3_RLC) And SessionHelper.ApplicationType <> EnumHelper.GetEnumDescription(ApplicationType.Renewal)) Then
    '        Dim params(4) As Microsoft.Reporting.WebForms.ReportParameter

    '        rvCertificate.Reset()

    '        Dim reportds As New ReportDataSource("dsCertificate", certificate.GetDDCertificateInfo(SessionID, SessionHelper.ApplicationType, SessionHelper.SelectedUserRole))

    '        params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", AppID)
    '        params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", SessionHelper.ApplicationType)
    '        params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
    '        params(3) = New Microsoft.Reporting.WebForms.ReportParameter("DDCategoryCode", SessionID)
    '        params(4) = New Microsoft.Reporting.WebForms.ReportParameter("TodayDate", DateTime.Today.ToShortDateString())
    '        rvCertificate.LocalReport.Refresh()
    '        rvCertificate.LocalReport.ReportPath = "Reports/DDCategoryInitialCertificate.rdlc"
    '        rvCertificate.LocalReport.SetParameters(params)
    '        rvCertificate.LocalReport.DataSources.Add(reportds)
    '        rvCertificate.LocalReport.Refresh()
    '    End If
    '    If ((SessionHelper.SelectedUserRole = RoleLevelCategory.DDPersonnel_RLC Or SessionHelper.SelectedUserRole = RoleLevelCategory.DDPersonnel2_RLC Or SessionHelper.SelectedUserRole = RoleLevelCategory.DDPersonnel3_RLC) And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.Renewal)) Then
    '        Dim params(4) As Microsoft.Reporting.WebForms.ReportParameter

    '        rvCertificate.Reset()

    '        Dim reportds As New ReportDataSource("dsCertificate", certificate.GetDDCertificateInfo(SessionID, SessionHelper.ApplicationType, SessionHelper.SelectedUserRole))

    '        params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", AppID)
    '        params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", SessionHelper.ApplicationType)
    '        params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
    '        params(3) = New Microsoft.Reporting.WebForms.ReportParameter("DDCode", SessionID)
    '        params(4) = New Microsoft.Reporting.WebForms.ReportParameter("TodayDate", DateTime.Today.ToShortDateString())
    '        rvCertificate.LocalReport.Refresh()
    '        rvCertificate.LocalReport.ReportPath = "Reports/DDCategoryRenewalCertificate.rdlc"
    '        rvCertificate.LocalReport.SetParameters(params)
    '        rvCertificate.LocalReport.DataSources.Add(reportds)
    '        rvCertificate.LocalReport.Refresh()
    '    End If
    '    Dim warnings As Warning() = Nothing
    '    Dim streamids As String() = Nothing
    '    Dim mimeType As String = Nothing
    '    Dim encoding As String = Nothing
    '    Dim extension As String = Nothing
    '    Dim myBytes As Byte()

    '    myBytes = rvCertificate.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)
    '    Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
    '    Dim uploadDocumentNumber As Long = uploadService.InsertUploadedDocumentIntoImageStore(myBytes)
    '    If uploadDocumentNumber > 0 Then
    '        Dim uploadedDoc As New Business.Model.DocumentUpload()

    '        'Generate model for database information
    '        With uploadedDoc
    '            .ImageSID = uploadDocumentNumber
    '            .DocumentName = AppID & "_" & SessionHelper.ApplicationType & "_" & DateString & ".pdf"
    '            .DocumentType = "9"
    '        End With
    '        ' Save file info to database
    '        uploadService.InsertUploadedDocument(uploadedDoc, AppID) 'AppID should be used

    '    End If
    '    rvCertificate.LocalReport.Refresh()
    '    rvCertificate.Reset()
    'End Sub

    'Private Sub GetAllCertificates(ByVal certDetails As Model.CertificationDetails)
    '    Dim certificate As ICertificateService = StructureMap.ObjectFactory.GetInstance(Of ICertificateService)() 'New CertificateService()
    '    Select Case certDetails.RoleLevelCategoryID
    '        Case 4
    '            _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.RNTrainer_RLC)
    '        Case 5
    '            _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.RNInstructor_RLC)
    '        Case 6
    '            _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.RNMaster_RLC)
    '        Case 7
    '            _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.QARN_RLC)
    '        Case 8
    '            _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.Bed17_RLC)
    '        Case 15
    '            _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.DDPersonnel_RLC)
    '        Case 16
    '            _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.DDPersonnel2_RLC)
    '        Case 17
    '            _roles = EnumHelper.GetEnumDescription(RoleLevelCategory.DDPersonnel3_RLC)
    '    End Select
    '    rvCertificate.ProcessingMode = ProcessingMode.Local
    '    Dim permissions As PermissionSet = New PermissionSet(PermissionState.Unrestricted)
    '    rvCertificate.LocalReport.SetBasePermissionsForSandboxAppDomain(permissions)
    '    If ((certDetails.RoleLevelCategoryID = _rntrainer Or certDetails.RoleLevelCategoryID = RoleLevelCategory.Bed17_RLC) And (certDetails.ApplicationType <> EnumHelper.GetEnumDescription(ApplicationType.Renewal))) Then
    '        Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

    '        rvCertificate.Reset()

    '        Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfo(SessionID, certDetails.ApplicationType, certDetails.RoleLevelCategoryID))

    '        params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", AppID)
    '        params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", certDetails.ApplicationType)
    '        params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
    '        params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", SessionID)
    '        rvCertificate.LocalReport.Refresh()
    '        rvCertificate.LocalReport.ReportPath = "Reports/RNTrainerCertificate.rdlc"
    '        rvCertificate.LocalReport.SetParameters(params)
    '        rvCertificate.LocalReport.DataSources.Add(reportds)
    '        rvCertificate.LocalReport.Refresh()
    '        Dim warnings As Warning() = Nothing
    '        Dim streamids As String() = Nothing
    '        Dim mimeType As String = Nothing
    '        Dim encoding As String = Nothing
    '        Dim extension As String = Nothing
    '        Dim myBytes As Byte()

    '        myBytes = rvCertificate.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)
    '        Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
    '        Dim uploadDocumentNumber As Long = uploadService.InsertUploadedDocumentIntoImageStore(myBytes)
    '        If uploadDocumentNumber > 0 Then
    '            Dim uploadedDoc As New Business.Model.DocumentUpload()

    '            'Generate model for database information
    '            With uploadedDoc
    '                .ImageSID = uploadDocumentNumber
    '                .DocumentName = AppID & "_" & certDetails.ApplicationType & "_" & _roles & "_" & DateString & ".pdf"
    '                .DocumentType = "9"
    '            End With
    '            ' Save file info to database
    '            uploadService.InsertUploadedDocument(uploadedDoc, AppID) 'AppID should be used

    '        End If
    '        rvCertificate.LocalReport.Refresh()
    '        rvCertificate.Reset()
    '    End If
    '    If ((certDetails.RoleLevelCategoryID = RoleLevelCategory.RNInstructor_RLC) And (certDetails.ApplicationType <> EnumHelper.GetEnumDescription(ApplicationType.Renewal))) Then
    '        Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

    '        rvCertificate.Reset()

    '        Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfo(SessionID, certDetails.ApplicationType, certDetails.RoleLevelCategoryID))

    '        params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", AppID)
    '        params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", certDetails.ApplicationType)
    '        params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
    '        params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", SessionID)
    '        rvCertificate.LocalReport.Refresh()
    '        rvCertificate.LocalReport.ReportPath = "Reports/RNInstructorCertificate.rdlc"
    '        rvCertificate.LocalReport.SetParameters(params)
    '        rvCertificate.LocalReport.DataSources.Add(reportds)
    '        rvCertificate.LocalReport.Refresh()
    '        Dim warnings As Warning() = Nothing
    '        Dim streamids As String() = Nothing
    '        Dim mimeType As String = Nothing
    '        Dim encoding As String = Nothing
    '        Dim extension As String = Nothing
    '        Dim myBytes As Byte()

    '        myBytes = rvCertificate.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)
    '        Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
    '        Dim uploadDocumentNumber As Long = uploadService.InsertUploadedDocumentIntoImageStore(myBytes)
    '        If uploadDocumentNumber > 0 Then
    '            Dim uploadedDoc As New Business.Model.DocumentUpload()

    '            'Generate model for database information
    '            With uploadedDoc
    '                .ImageSID = uploadDocumentNumber
    '                .DocumentName = AppID & "_" & certDetails.ApplicationType & "_" & _roles & "_" & DateString & ".pdf"
    '                .DocumentType = "9"
    '            End With
    '            ' Save file info to database
    '            uploadService.InsertUploadedDocument(uploadedDoc, AppID) 'AppID should be used

    '        End If
    '        rvCertificate.LocalReport.Refresh()
    '        rvCertificate.Reset()
    '    End If
    '    If ((certDetails.RoleLevelCategoryID = RoleLevelCategory.RNInstructor_RLC) And (certDetails.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.Renewal))) Then
    '        Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

    '        rvCertificate.Reset()

    '        Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfo(SessionID, certDetails.ApplicationType, certDetails.RoleLevelCategoryID))

    '        params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", AppID)
    '        params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", certDetails.ApplicationType)
    '        params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
    '        params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", SessionID)
    '        rvCertificate.LocalReport.Refresh()
    '        rvCertificate.LocalReport.ReportPath = "Reports/RNInstructorRenewalCertificate.rdlc"
    '        rvCertificate.LocalReport.SetParameters(params)
    '        rvCertificate.LocalReport.DataSources.Add(reportds)
    '        rvCertificate.LocalReport.Refresh()
    '        Dim warnings As Warning() = Nothing
    '        Dim streamids As String() = Nothing
    '        Dim mimeType As String = Nothing
    '        Dim encoding As String = Nothing
    '        Dim extension As String = Nothing
    '        Dim myBytes As Byte()

    '        myBytes = rvCertificate.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)
    '        Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
    '        Dim uploadDocumentNumber As Long = uploadService.InsertUploadedDocumentIntoImageStore(myBytes)
    '        If uploadDocumentNumber > 0 Then
    '            Dim uploadedDoc As New Business.Model.DocumentUpload()

    '            'Generate model for database information
    '            With uploadedDoc
    '                .ImageSID = uploadDocumentNumber
    '                .DocumentName = AppID & "_" & certDetails.ApplicationType & "_" & _roles & "_" & DateString & ".pdf"
    '                .DocumentType = "9"
    '            End With
    '            ' Save file info to database
    '            uploadService.InsertUploadedDocument(uploadedDoc, AppID) 'AppID should be used

    '        End If
    '        rvCertificate.LocalReport.Refresh()
    '        rvCertificate.Reset()
    '    End If
    '    If ((certDetails.RoleLevelCategoryID = _rntrainer Or certDetails.RoleLevelCategoryID = RoleLevelCategory.Bed17_RLC) And certDetails.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.Renewal)) Then
    '        Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

    '        rvCertificate.Reset()

    '        Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfo(SessionID, certDetails.ApplicationType, certDetails.RoleLevelCategoryID))

    '        params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", AppID)
    '        params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", certDetails.ApplicationType)
    '        params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
    '        params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", SessionID)
    '        rvCertificate.LocalReport.Refresh()
    '        rvCertificate.LocalReport.ReportPath = "Reports/RNTrainerRenewalCertificate.rdlc"
    '        rvCertificate.LocalReport.SetParameters(params)
    '        rvCertificate.LocalReport.DataSources.Add(reportds)
    '        rvCertificate.LocalReport.Refresh()
    '        Dim warnings As Warning() = Nothing
    '        Dim streamids As String() = Nothing
    '        Dim mimeType As String = Nothing
    '        Dim encoding As String = Nothing
    '        Dim extension As String = Nothing
    '        Dim myBytes As Byte()

    '        myBytes = rvCertificate.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)
    '        Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
    '        Dim uploadDocumentNumber As Long = uploadService.InsertUploadedDocumentIntoImageStore(myBytes)
    '        If uploadDocumentNumber > 0 Then
    '            Dim uploadedDoc As New Business.Model.DocumentUpload()

    '            'Generate model for database information
    '            With uploadedDoc
    '                .ImageSID = uploadDocumentNumber
    '                .DocumentName = AppID & "_" & certDetails.ApplicationType & "_" & _roles & "_" & DateString & ".pdf"
    '                .DocumentType = "9"
    '            End With
    '            ' Save file info to database
    '            uploadService.InsertUploadedDocument(uploadedDoc, AppID) 'AppID should be used

    '        End If
    '        rvCertificate.LocalReport.Refresh()
    '        rvCertificate.Reset()
    '    End If
    '    If ((certDetails.RoleLevelCategoryID = 15 Or certDetails.RoleLevelCategoryID = 16 Or certDetails.RoleLevelCategoryID = 17) And certDetails.ApplicationType <> EnumHelper.GetEnumDescription(ApplicationType.Renewal)) Then
    '        Dim params(4) As Microsoft.Reporting.WebForms.ReportParameter

    '        rvCertificate.Reset()

    '        Dim reportds As New ReportDataSource("dsCertificate", certificate.GetDDCertificateInfo(SessionID, certDetails.ApplicationType, certDetails.RoleLevelCategoryID))

    '        params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", AppID)
    '        params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", certDetails.ApplicationType)
    '        params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
    '        params(3) = New Microsoft.Reporting.WebForms.ReportParameter("DDCategoryCode", SessionID)
    '        params(4) = New Microsoft.Reporting.WebForms.ReportParameter("TodayDate", DateTime.Today.ToShortDateString())
    '        rvCertificate.LocalReport.Refresh()
    '        rvCertificate.LocalReport.ReportPath = "Reports/DDCategoryInitialCertificate.rdlc"
    '        rvCertificate.LocalReport.SetParameters(params)
    '        rvCertificate.LocalReport.DataSources.Add(reportds)
    '        rvCertificate.LocalReport.Refresh()
    '        Dim warnings As Warning() = Nothing
    '        Dim streamids As String() = Nothing
    '        Dim mimeType As String = Nothing
    '        Dim encoding As String = Nothing
    '        Dim extension As String = Nothing
    '        Dim myBytes As Byte()

    '        myBytes = rvCertificate.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)
    '        Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
    '        Dim uploadDocumentNumber As Long = uploadService.InsertUploadedDocumentIntoImageStore(myBytes)
    '        If uploadDocumentNumber > 0 Then
    '            Dim uploadedDoc As New Business.Model.DocumentUpload()

    '            'Generate model for database information
    '            With uploadedDoc
    '                .ImageSID = uploadDocumentNumber
    '                .DocumentName = AppID & "_" & certDetails.ApplicationType & "_" & _roles & "_" & DateString & ".pdf"
    '                .DocumentType = "9"
    '            End With
    '            ' Save file info to database
    '            uploadService.InsertUploadedDocument(uploadedDoc, AppID) 'AppID should be used

    '        End If
    '        rvCertificate.LocalReport.Refresh()
    '        rvCertificate.Reset()
    '    End If
    '    If ((certDetails.RoleLevelCategoryID = 15 Or certDetails.RoleLevelCategoryID = 16 Or certDetails.RoleLevelCategoryID = 17) And certDetails.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.Renewal)) Then
    '        Dim params(4) As Microsoft.Reporting.WebForms.ReportParameter

    '        rvCertificate.Reset()

    '        Dim reportds As New ReportDataSource("dsCertificate", certificate.GetDDCertificateInfo(SessionHelper.SessionUniqueID, certDetails.ApplicationType, certDetails.RoleLevelCategoryID))

    '        params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", AppID)
    '        params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", certDetails.ApplicationType)
    '        params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
    '        params(3) = New Microsoft.Reporting.WebForms.ReportParameter("DDCode", SessionHelper.SessionUniqueID)
    '        params(4) = New Microsoft.Reporting.WebForms.ReportParameter("TodayDate", DateTime.Today.ToShortDateString())
    '        rvCertificate.LocalReport.Refresh()
    '        rvCertificate.LocalReport.ReportPath = "Reports/DDCategoryRenewalCertificate.rdlc"
    '        rvCertificate.LocalReport.SetParameters(params)
    '        rvCertificate.LocalReport.DataSources.Add(reportds)
    '        rvCertificate.LocalReport.Refresh()
    '        Dim warnings As Warning() = Nothing
    '        Dim streamids As String() = Nothing
    '        Dim mimeType As String = Nothing
    '        Dim encoding As String = Nothing
    '        Dim extension As String = Nothing
    '        Dim myBytes As Byte()

    '        myBytes = rvCertificate.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)
    '        Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
    '        Dim uploadDocumentNumber As Long = uploadService.InsertUploadedDocumentIntoImageStore(myBytes)
    '        If uploadDocumentNumber > 0 Then
    '            Dim uploadedDoc As New Business.Model.DocumentUpload()

    '            'Generate model for database information
    '            With uploadedDoc
    '                .ImageSID = uploadDocumentNumber
    '                .DocumentName = AppID & "_" & certDetails.ApplicationType & "_" & _roles & "_" & DateString & ".pdf"
    '                .DocumentType = "9"
    '            End With
    '            ' Save file info to database
    '            uploadService.InsertUploadedDocument(uploadedDoc, AppID) 'AppID should be used

    '        End If
    '        rvCertificate.LocalReport.Refresh()
    '        rvCertificate.Reset()
    '    End If
    'End Sub
    Public Sub AppStatChanged()
        Dim fstat As String = ""
        Dim res As New ReturnObject(Of Long)(-1L)
        fstat = ddAppStatus.SelectedItem.Text

        If PAdminStatus.Visible = True AndAlso ddAdminStatus.SelectedValue <> "0" Then
            fstat = ddAdminStatus.SelectedItem.Text
        End If
        If fstat = "Did Not Meet Requirements" Or fstat = "Removed From Registry" Or fstat = "DODD Review" Or fstat = "Denied" Or fstat = "Intent to Deny" Then
            ''if no notation exists for this app for this status then
            Dim hasNotation As Boolean = False
            If SessionHelper.Notation_Flg = True And fstat = "DODD Review" Then
                hasNotation = True
            End If
            Dim nl As New List(Of NotationDetails)
            nl = NotSvc.GetNotationsByApp(AppID, RNorDD).ReturnValue
            Dim ntype As String = GetNotTypeByStatusType(fstat)

            If nl IsNot Nothing Then
                For Each no As NotationDetails In nl
                    If no.NotationType.NTypeDesc = ntype Then
                        hasNotation = True
                    End If
                Next
            End If
            If Not hasNotation Then
                fstat = Server.HtmlEncode(fstat)
                Response.Redirect("Notation.aspx?appstatus=" & fstat)

                'res = SaveToPermSvc.InsertPersonIfNotExists(AppID, RNorDD)
                'If res.ReturnValue = 0 Then
                '    For Each mstring As ReturnMessage In res.GeneralMessages
                '        If mstring.Message Like "*niqueCode*" Then
                '            SessionHelper.SessionUniqueID = mstring.Message.Substring(11)
                '        End If
                '    Next
                '    fstat = Server.HtmlEncode(fstat)
                '    Response.Redirect("Notation.aspx?appstatus=" & fstat)
                'Else
                '    pError.Visible = True
                '    For Each mstring As ReturnMessage In res.GeneralMessages
                '        If mstring.Message Like "*niqueCode*" Then
                '            SessionHelper.SessionUniqueID = mstring.Message.Substring(11)
                '        End If
                '    Next
                '    pError.InnerHtml += "An Error occured while saving summary information."
                'End If
            End If
        End If
    End Sub
    Public Function NeedCert(ByVal rcl As Integer, ByVal status As String, ByVal apptype As String) As Integer
        Dim rdet As Business.Model.RoleCategoryLevelDetails = Msvc.GetRoleCategoryLevelInfoByRoleCategoryLevelSid(rcl)
        If status = "Meets Requirements" Or status = "Added To Registry" Or status = "Certified" Then
            If rdet.Role_Name = "RN Master" Or rdet.Role_Name = "QA RN" Then
                Return 0 'no cert-final
            Else
                Return 1 'cert-final
            End If
        Else
            If status = "DODD Review" Or status = "Intent to Deny" Or status = "Pending" Then
                Return 2 'no cert-not final
            Else
                Return 0
            End If
        End If
        Return True
    End Function
    Public Function GetNotTypeByStatusType(ByVal s As String) As String
        Dim nStat As String = ""
        If s = "Removed From Registry" Then
            nStat = "Unregistered"
        Else
            nStat = s
        End If
        Return nStat
    End Function
    Public Sub AppToPdfAndDB(ByVal ProcessAppID As Integer, ByVal UserID As String, ByVal CurrentCertApplicationType As String, ByVal CurrentCertRcl As Integer, ByVal CurrentRNorDDFlg As Boolean)
        Dim sw As New System.IO.StringWriter()
        Dim hw As New HtmlTextWriter(sw)

        Dim strError As String = String.Empty

        'HttpContext.Current.Server.Execute(("Summary.aspx?newwin=yes&reloadappid=" & ProcessAppID), hw, False)
        'HttpContext.Current.Server.Execute(("SummaryPrintView.aspx?newwin=yes&reloadappid=" & ProcessAppID), hw, False)
        HttpContext.Current.Server.Execute(("SummaryReGeneration.aspx?newwin=yes&reloadappid=" & ProcessAppID & "&UserID=" & UserID & "&ApplicationType=" & CurrentCertApplicationType & "&SelectedUserRole=" & CurrentCertRcl & "&UserName=" & SessionHelper.Name & "&RNorDDFlg=" & CurrentRNorDDFlg.ToString), hw, False)

        Dim urlBase As String = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority

        Dim docSvc As IDocumentService = StructureMap.ObjectFactory.GetInstance(Of IDocumentService)()
        Dim currentUser As IUser = UserAndRoleHelper.CurrentUser()
        Dim pdfBytes As Byte() = docSvc.CreateApplicationPDFDocument(sw.ToString(),
                                                                     urlBase,
                                                                     String.Format("Submitted by {0} ({1}) on {2}", currentUser.UserName, currentUser.UserCode, Date.Now.ToString()))

        If pdfBytes IsNot Nothing AndAlso pdfBytes.Count() > 0 Then
            Dim sumImg As Long = uploadSvc.InsertUploadedDocumentIntoImageStore(pdfBytes)
            If sumImg > 0 Then
                Dim uploadedDoc As New Business.Model.DocumentUpload()

                'Generate model for database information
                With uploadedDoc
                    .ImageSID = sumImg
                    .DocumentName = Format(DateTime.Now, "MMddmm") + CStr(ProcessAppID) + "_ApplicationSummary.pdf"
                    .DocumentType = "7"
                End With
                ' Save file info to database
                uploadSvc.InsertUploadedDocument(uploadedDoc, ProcessAppID) 'AppID should be used
            End If
            'Dim UserName As String = "N/A"
            'Dim UserSid As String
            'UserName = Msvc.GetApplicantNameByCode(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg) 'getApplicant Name maisSerivce.GetRNsName(hdfRNID.Value)
            'UserSid = Msvc.GetApplicantXrefSidByCode(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg) 'getApplicant Name maisSerivce.GetRNsName(hdfRNID.Value)

            'UserName = UserName.Replace(" ", ".")
            ''Dim wsUDS As New UDSWebService.UDSService() With {.Url = ConfigHelper.UDSServiceEndpoint} 'move this line to calling method outside
            'Dim repositoryName As String = "UDS - MA"
            'Dim wsDataSet As New DataSet 'wsUDS.GetGenericIndexes(repositoryName)
            'Dim dob As Date = Date.Today
            'If IsDate(lblDtIssuedOrDOBtxt.Text) Then
            '    dob = CDate(lblDtIssuedOrDOBtxt.Text)
            '    Dim sd As String = Format(dob, "MMddYYYY")
            'End If
            'Dim xml As String = "<Table> <Index> <Label> Course Number </Label> <Value>  </Value> </Index>" _
            '                    + "<Index> <Label> Application ID </Label> <Value> " + CStr(AppID) + " </Value> </Index>" _
            '                    + "<Index> <Label> Application Type </Label> <Value> " + getAppTypeUds(SessionHelper.ApplicationType, SessionHelper.SelectedUserRole) + " </Value> </Index>" _
            '                    + "<Index> <Label> DD Personnel Code </Label> <Value> " + IIf(SessionHelper.RN_Flg, "", SessionHelper.SessionUniqueID) + " </Value> </Index>" _
            '                    + "<Index> <Label> NAME </Label> <Value> " + UserName + " </Value> </Index>" _
            '                    + "<Index> <Label> RN Lics or 4 SSN </Label> <Value> " + lblRNLNoOrSSNtxt.Text.Trim + " </Value> </Index>" _
            '                    + "<Index> <Label> DOB </Label> <Value> " + String.Format("{0}/{1}/{2}", dob.Year.ToString.PadLeft(4, "0"), dob.Month.ToString.PadLeft(2, "0"), dob.Day.ToString.PadLeft(2, "0")) + " </Value> </Index>" _
            '                    + "<Index> <Label> Personnel Type </Label> <Value> " + IIf(SessionHelper.RN_Flg, "RN", "DDPERS") + " </Value> </Index>" _
            '                    + "<Index> <Label> CERT STATUS </Label> <Value>  </Value> </Index>" _
            '                    + "</Table>"
            'Dim Render = New System.IO.StringReader(xml)
            'wsDataSet.ReadXml(Render)
            '' Format(DateTime.Now,"nnmmdd")
            'Dim DocumentName As String = Format(DateTime.Now, "MMddmm") + CStr(AppID) + "_ApplicationSummary.pdf"

            'Dim result As UDSWebService.Result = wsUDS.SaveToUDS(repositoryName, _
            '                                                     pdfBytes, _
            '                                                     UserName, _
            '                                                     DocumentName, _
            '                                                     "APPSUM", _
            '                                                     "APPLICATION SUMMARY", _
            '                                                     Today, _
            '                                                     String.Empty, _
            '                                                     wsDataSet)
            'Select Case result
            '    'Case UDSWebService.Result.AlreadyExists
            '    '   strError = "UDSWebService.Result.AlreadyExists"
            '    Case UDSWebService.Result.EmptyFile
            '        strError = "UDSWebService.Result.EmptyFile"
            '        'blnSaveFileToUDS = False
            '    Case UDSWebService.Result.InError
            '        strError = "UDSWebService.Result.InError"
            '        'blnSaveFileToUDS = False
            '    Case UDSWebService.Result.Invalid
            '        strError = "UDSWebService.Result.Invalid"
            '        'blnSaveFileToUDS = False
            '    Case UDSWebService.Result.Success
            '        'upload service mark as saved to UDS
            'End Select
        End If
    End Sub

    Private Sub SaveToUDS(ByVal Myappid As Integer, ByVal MyAppType As String, ByVal MySessionUniqueID As String, ByVal MyUserRole As Integer, ByVal MyRn_FLg As Boolean, ByVal myIssuedOrDOBtxt As String, ByVal RNorSSNumber As String)
        Dim uploadedFiles As New List(Of Business.Model.DocumentUpload)
        Dim uploadSvc As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()

        uploadedFiles = uploadSvc.GetUploadedDocumentsNotInUDS(Myappid)
        Dim strError As String = String.Empty

        If uploadedFiles IsNot Nothing Then
            'Dim maisSerivce As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
            Dim UserName As String = "N/A"
            Dim UserSid As String
            Dim dtype As String = ""
            Dim ddesc As String = ""
            UserName = Trim(lblFirstName.Text) & "." & Trim(lblLastName.Text) 'Msvc.GetApplicantNameByCode(MySessionUniqueID, MyRn_FLg) 'getApplicant Name maisSerivce.GetRNsName(hdfRNID.Value)
            UserSid = Msvc.GetApplicantXrefSidByCode(MySessionUniqueID, MyRn_FLg) 'getApplicant Name maisSerivce.GetRNsName(hdfRNID.Value)
            UserSid = IIf(IsNothing(UserSid), "0", UserSid)


            ' If Me.RequiredFieldValidator1.Enabled = False Then ' the control will not post back to the server if Enable is False. Need to set the RN From the Hiden field
            ' UserName = Msvc.GetApplicantNameByCode(MySessionUniqueID, MyRn_FLg) 'getApplicant Name maisSerivce.GetRNsName(hdfRNID.Value)
            ' UserSid = Msvc.GetApplicantXrefSidByCode(MySessionUniqueID, MyRn_FLg) 'getApplicant Name maisSerivce.GetRNsName(hdfRNID.Value)
            'Else
            '    UserName = "" 'getApplicant Name maisSerivce.GetRNsName(hdfRNID.Value) maisSerivce.GetRNsName(ddlRnNames.SelectedValue)
            '    userSid = "" 'getApplicant Name maisSerivce.GetRNsName(hdfRNID.Value) maisSerivce.GetRNsLicenseNumber(ddlRnNames.SelectedValue).Replace("RN", "")
            'End If
            ' UserName = UserName.Replace(" ", ".")
            'Dim wsUDS As New UDSWebService.UDSService() With {.Url = ConfigHelper.UDSServiceEndpoint} 'move this line to calling method outside
            Dim repositoryName As String = "UDS - MA"
            Dim wsDataSet As New DataSet 'wsUDS.GetGenericIndexes(repositoryName)
            Dim dob As Date = Date.Today
            If IsDate(myIssuedOrDOBtxt) Then
                dob = CDate(myIssuedOrDOBtxt)
            End If
            Dim xml As String = "<Table> <Index> <Label> Course Number </Label> <Value>  </Value> </Index>" _
                                + "<Index> <Label> Application ID </Label> <Value> " + CStr(Myappid) + " </Value> </Index>" _
                                + "<Index> <Label> Application Type </Label> <Value> " + getAppTypeUds(MyAppType, MyUserRole) + " </Value> </Index>" _
                                + "<Index> <Label> DD Personnel Code </Label> <Value> " + IIf(MyRn_FLg, "", MySessionUniqueID) + " </Value> </Index>" _
                                + "<Index> <Label> NAME </Label> <Value> " + UserName + " </Value> </Index>" _
                                + "<Index> <Label> RN Lics or 4 SSN </Label> <Value> " + RNorSSNumber.Trim + " </Value> </Index>" _
                                + "<Index> <Label> DOB </Label> <Value> " + String.Format("{0}/{1}/{2}", dob.Year.ToString.PadLeft(4, "0"), dob.Month.ToString.PadLeft(2, "0"), dob.Day.ToString.PadLeft(2, "0")) + " </Value> </Index>" _
                                + "<Index> <Label> Personnel Type </Label> <Value> " + IIf(MyRn_FLg, "RN", "DDPERS") + " </Value> </Index>" _
                                + "<Index> <Label> CERT STATUS </Label> <Value>  </Value> </Index>" _
                                + "</Table>"
            Dim Render = New System.IO.StringReader(xml)
            wsDataSet.ReadXml(Render)

            Dim Document_Int As Integer = 0
            For Each s In uploadedFiles
                If s.DocumentType = "Certificate" Then
                    dtype = "MACERTS"
                    ddesc = "MA CERTIFICATE"
                Else
                    dtype = "APPDOC"
                    ddesc = "APPLICATION"
                End If
                Document_Int += 1
                Dim byteArrayForUDS As Byte() = uploadSvc.GetUploadedDocumentByImageSID(s.ImageSID)
                Dim DocumentName As String = ""
                If s.DocumentName.Contains("ApplicationSummary") Then
                    DocumentName = Document_Int.ToString + "_" + s.DocumentName
                    dtype = "APPSUM"
                    ddesc = "APPLICATION SUMMARY"
                Else
                    DocumentName = Format(DateTime.Now, "MMddmm") + CStr(Myappid) + "_" + Document_Int.ToString + "_" + s.DocumentName
                End If

                Dim result As UDSWebService.Result = wsUDS.SaveToUDS(repositoryName, _
                                                                     byteArrayForUDS, _
                                                                     UserName, _
                                                                     DocumentName, _
                                                                     dtype, _
                                                                     ddesc, _
                                                                     Today, _
                                                                     String.Empty, _
                                                                     wsDataSet)
                Select Case result
                    'Case UDSWebService.Result.AlreadyExists
                    '   strError = "UDSWebService.Result.AlreadyExists"
                    Case UDSWebService.Result.EmptyFile
                        strError = "UDSWebService.Result.EmptyFile"
                        'blnSaveFileToUDS = False
                    Case UDSWebService.Result.InError
                        strError = "UDSWebService.Result.InError"
                        'blnSaveFileToUDS = False
                    Case UDSWebService.Result.Invalid
                        strError = "UDSWebService.Result.Invalid"
                        'blnSaveFileToUDS = False
                    Case UDSWebService.Result.Success
                        uploadSvc.MarkDocumentSavedUDS(s.ImageSID)
                        'upload service mark as saved to UDS
                        'blnSaveFileToUDS = True
                End Select
            Next

        End If
    End Sub
    Public Function getAppTypeUds(ByVal at As String, ByVal rcl As Integer) As String
        Dim rcldet As Model.RoleCategoryLevelDetails = Msvc.GetRoleCategoryLevelInfoByRoleCategoryLevelSid(rcl)
        Dim udstype As String = ""
        If at = "AddOn" Then
            at = "ADD-ON"
        End If
        Select Case rcldet.Category_Type_Name
            Case "Cat - I"
                udstype = "DD CATEGORY - 1 " & at.ToUpper
            Case "Cat - II"
                udstype = "DD CATEGORY - 2 " & at.ToUpper
            Case "Cat - III"
                udstype = "DD CATEGORY - 3 " & at.ToUpper
            Case "Cat - IV"
                udstype = "17+ BED - " & at.ToUpper
            Case "Cat - V"
                udstype = "RN MASTER " & at.ToUpper
            Case "Cat - VI"
                udstype = "RN TRAINER " & at.ToUpper
            Case "Cat - VII"
                udstype = "RN INSTRUCTOR " & at.ToUpper
            Case "Cat - VIII"
                udstype = "QA RN - " & at.ToUpper
        End Select
        Return udstype
    End Function
    Protected Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.ServerClick
        Response.Redirect(PagesHelper.GetPreviousPage(Master.CurrentPage))
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