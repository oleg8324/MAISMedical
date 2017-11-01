Imports ODMRDDHelperClassLibrary
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data
Imports MAIS.Business.Services
Imports System.String
Imports System.IO
Imports MAIS.Business
Imports Microsoft.Reporting.WebForms
Imports MAIS.Business.Helpers
Imports MAIS.Business.Model.Enums
Imports System.Security
Imports System.Security.Permissions

Public Class CertAdministration
    Inherits System.Web.UI.Page

    Private Shared _nd As NotationDetails
    Private Shared _appID As Integer
    Private Shared NotationSvc As INotationService
    Private Shared PersSvc As IPersonalInformationService
    Private Shared MSvc As IMAISSerivce
    Private Shared CurrentCertRcl As Integer
    Private Shared CurrentCertSDate As Date
    Private Shared CurrentCertRolePersonSid As Integer
    Private Shared CurrentCertSid As Integer
    Private Shared curCert As Business.Model.Certificate
    Dim _roles As String = String.Empty
    'Public Shared usr As ODMRDD_NET2.IUser

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim uploadedFiles As New List(Of Business.Model.DocumentUpload)
        Dim uploadSvc As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
        MSvc = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        PersSvc = StructureMap.ObjectFactory.GetInstance(Of IPersonalInformationService)()
        Dim queryString As String = Request.QueryString("App")
        Master.HideProgressBar = True
        'hStatus.Value = ""
        pnote.Style("display") = "none"
        lblDt.Text = Date.Today.ToShortDateString
        hIsPersonDD.Value = IIf(SessionHelper.RN_Flg = True, "0", "1")
        txtDtOccurance.Value = Date.Today.ToShortDateString
        'ScriptManager.RegisterStartupScript(Me, Me.GetType, "jqscript", "<script language='javascript' src='Scripts/jquery/jquery.1.6.2.min.js'></script><script language='javascript' src='Scripts/jquery/jquery.calendars.picker.pack.js'></script><script language='javascript' src='Scripts/jquery/jquery.calendars.pack.js'></script>", False)
        'ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType, "dtpickscript", "<script language='javascript' src='Scripts/jquery/jquery.calendars.picker.pack.js'></script>", False)
        If Not IsPostBack Then
            Session("NotDocs") = Nothing
            If MSvc.GetExistingFlg(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg).ReturnValue Then
                PEnterNotation.Style("display") = "none"
                chbUnflag.Enabled = False
                rblSelect.SelectedValue = 1
                rblSelect_SelectedIndexChanged(rblSelect, System.EventArgs.Empty)
                Dim certHis As List(Of Business.Model.Certificate) = MSvc.GetCertificationHistory(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg)
                curCert = (From ch In certHis Where ch.Role_Category_Level_Sid = SessionHelper.SelectedUserRole Select ch).FirstOrDefault

                Dim cslist As List(Of Business.Model.CertStatus)
                NotationSvc = StructureMap.ObjectFactory.GetInstance(Of INotationService)()
                If Not IsNothing(curCert) Then
                    'If (Not String.IsNullOrEmpty(lblHomeAddress.Text)) Then
                    '    strToEmailAddress = lblHomeAddress.Text
                    'Else
                    '    If (Not String.IsNullOrEmpty(lblWorkAddress.Text)) Then
                    '        strToEmailAddress = lblWorkAddress.Text
                    '    Else
                    '        strToEmailAddress = lblCellAddress.Text
                    '    End If
                    'End If
                    CurrentCertSDate = curCert.StartDate
                    CurrentCertRcl = curCert.Role_Category_Level_Sid
                    CurrentCertRolePersonSid = curCert.Role_RN_DD_Personnel_Xref_Sid
                    SessionHelper.ApplicationType = curCert.ApplicationType
                    CurrentCertSid = curCert.Certification_Sid

                    lblRole.Text = curCert.Role
                    lblCategory.Text = curCert.Category
                    lblLevel.Text = curCert.Level
                    txtStDate.Value = curCert.StartDate.ToShortDateString
                    txtEDate.Value = curCert.EndDate.ToShortDateString

                    cslist = NotationSvc.GetCertStatuses

                    For Each cm As Business.Model.CertStatus In cslist
                        If curCert.EndDate < Date.Today Then
                            If cm.CertStatusDesc = "Expired" Then
                                ddCertStatus.Items.Add(New ListItem With {.Value = cm.CertStatusSid, .Text = cm.CertStatusDesc, .Selected = IIf(curCert.Status = cm.CertStatusDesc, True, False)})
                            End If
                        End If
                        If curCert.Status = "Intent to Revoke" Then
                            If cm.CertStatusDesc = "Revoked" Or cm.CertStatusDesc = "Certified" Or cm.CertStatusDesc = "Suspended" Or cm.CertStatusDesc = "Voluntary Withdrawal" Or cm.CertStatusDesc = "Did Not Meet Requirements" Then
                                ddCertStatus.Items.Add(New ListItem With {.Value = cm.CertStatusSid, .Text = cm.CertStatusDesc, .Selected = IIf(curCert.Status = cm.CertStatusDesc, True, False)})
                            End If
                        Else
                            If cm.CertStatusDesc = "Intent to Revoke" Or cm.CertStatusDesc = "Certified" Or cm.CertStatusDesc = "Suspended" Or cm.CertStatusDesc = "Voluntary Withdrawal" Or cm.CertStatusDesc = "Did Not Meet Requirements" Or cm.CertStatusDesc = "Denied" Then
                                ddCertStatus.Items.Add(New ListItem With {.Value = cm.CertStatusSid, .Text = cm.CertStatusDesc, .Selected = IIf(curCert.Status = cm.CertStatusDesc, True, False)})
                            End If
                        End If
                    Next
                    'ddCertStatus.DataSource = cslist
                    'ddCertStatus.DataValueField = "CertStatusSid"
                    'ddCertStatus.DataTextField = "CertStatusDesc"
                    'ddCertStatus.DataBind()
                    'For Each li As ListItem In ddCertStatus.Items
                    '    If li.Text = curCert.Status Then
                    '        li.Selected = True
                    '    End If
                    'Next
                Else
                    lblRole.Text = "No Current Certificates to Edit"
                    DisableControls(pnlSearch)
                    DisableControls(PCert)
                    Exit Sub
                    'ddCertStatus.Enabled = False
                    'txtStDate.Style("disabled") = "disabled"
                End If

                'usr = MAIS_Helper.GetUser()
                'txtPerson.Value = usr.LastName & " " & usr.FirstName
                'set hidden field for javascript and saving later
                Dim NTypesList As List(Of Business.Model.NType)
                Dim NReasonsList As List(Of Business.Model.NReason)
                'Dim CStatuses As List(Of Objects.CertStatus)

                If UserAndRoleHelper.IsUserAdmin Then
                    txtPersonTitle.Value = "Admin"
                    txtPerson.Value = "Admin"
                ElseIf UserAndRoleHelper.IsUserSecretary Then
                    txtPersonTitle.Value = "Secretary"
                    txtPerson.Value = "Secretary"
                Else
                    Dim RDet As Business.Model.MAISRNDDRoleDetails = MSvc.GetRoleUsingUserID(SessionHelper.MAISUserID)
                    txtPersonTitle.Value = RDet.RoleName ' query1.GetRole(SessionHelper.MAISLevelUserRole)
                    txtPerson.Value = MSvc.GetRNsName(RDet.RNSID)
                End If

                NTypesList = NotationSvc.GetNotationTypes
                NReasonsList = NotationSvc.GetNotationReasons
                'cklReason.DataSource = NReasonsList
                'cklReason.DataTextField = "NReasonDesc"
                'cklReason.DataValueField = "NReasonSid"
                'cklReason.DataBind()
                'CStatuses = query1.GetCertStatuses()
                'ddlStatus.DataSource = CStatuses
                'ddlStatus.DataTextField = "CertStatusDesc"
                'ddlStatus.DataValueField = "CertStatusSid"
                'ddlStatus.DataBind()
                ddNotationType.DataSource = NTypesList
                ddNotationType.DataValueField = "NTypeSid"
                ddNotationType.DataTextField = "NTypeDesc"
                ddNotationType.DataBind()
                lbHReasons.DataSource = NReasonsList
                lbHReasons.DataTextField = "NReasonDesc"
                lbHReasons.DataValueField = "NReasonSid"
                lbHReasons.DataBind()

                If Not Request.QueryString("appstatus") Is Nothing Then
                    hStatus.Value = Request.QueryString("appstatus")
                    Dim notStat As String = GetNotTypeByStatusType(hStatus.Value)

                    For Each li As ListItem In ddNotationType.Items
                        If li.Text = notStat Then
                            li.Selected = True
                            NotTypeChanged(Me.ddNotationType, System.EventArgs.Empty)
                            pError.Visible = True
                            pError.InnerHtml = "You must enter notation for this status: " & notStat
                        End If
                    Next
                    ddNotationType.Enabled = False
                End If

            Else
                PEnterNotation.Visible = False
                pError.Visible = True
                pError.InnerHtml = "Person doesn't exist. Please finish the application for this person first."
                dNotButtons.Style("display") = "none"
                Exit Sub
            End If
        Else
            ' hPb.Value = "1"
        End If

        grdNotations.DataSource = NotationSvc.GetNotations(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg).ReturnValue
        grdNotations.DataBind()
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
    Private Sub ddcert_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddCertStatus.SelectedIndexChanged
        If ddCertStatus.SelectedItem.Text = "Revoked" Or ddCertStatus.SelectedItem.Text = "Intent to Revoke" Or ddCertStatus.SelectedItem.Text = "Suspended" Then
            PEnterNotation.Style("display") = "inline"
            txtDtOccurance.Value = Date.Today.ToShortDateString
            hNotId.Value = "0"
            ddNotationType.Enabled = True
            lblDt.Text = Date.Today.ToShortDateString
            'txtDtOccurance.Value = ""
            txtUFDate.Value = ""
            chbUnflag.Checked = False
            chbUnflag.Enabled = False
            'ddNotationType.SelectedValue = 0
            For Each it As ListItem In cklReason.Items
                it.Selected = False
            Next
            cklReason.Items.Clear()
            For Each li2 As ListItem In ddNotationType.Items
                li2.Selected = False
            Next

            For Each li As ListItem In ddNotationType.Items
                If li.Text = ddCertStatus.SelectedItem.Text Then
                    li.Selected = True
                    'If ddCertStatus.SelectedItem.Text = "Certified" Then

                    'End If
                    NotTypeChanged(Me.ddNotationType, System.EventArgs.Empty)
                    pError.Visible = True
                    pError.InnerHtml = "You must enter notation for this selection:" & ddCertStatus.SelectedItem.Text
                    Exit For
                End If
            Next
            ddNotationType.Enabled = False
        Else
            If ddCertStatus.SelectedItem.Text = "Select Status" Then
                PEnterNotation.Style("display") = "none"
                Exit Sub
            End If
            PEnterNotation.Style("display") = "inline"
            txtDtOccurance.Value = Date.Today.ToShortDateString
            hNotId.Value = "0"
            ddNotationType.Enabled = True
            lblDt.Text = Date.Today.ToShortDateString
            'txtDtOccurance.Value = ""
            txtUFDate.Value = Date.Today.ToShortDateString
            chbUnflag.Checked = True
            chbUnflag.Enabled = False
            'ddNotationType.SelectedValue = 0
            For Each it As ListItem In cklReason.Items
                it.Selected = False
            Next
            cklReason.Items.Clear()
            For Each li2 As ListItem In ddNotationType.Items
                li2.Selected = False
            Next

            For Each li As ListItem In ddNotationType.Items
                If li.Text = "Other" Then
                    li.Selected = True
                    'If ddCertStatus.SelectedItem.Text = "Certified" Then

                    'End If
                    NotTypeChanged(Me.ddNotationType, System.EventArgs.Empty)
                    pError.Visible = True
                    pError.InnerHtml = "You must enter notation for this selection:" & ddCertStatus.SelectedItem.Text
                    Exit For
                End If
            Next
            ddNotationType.Enabled = False
        End If
    End Sub
    Private Sub rblSelect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblSelect.SelectedIndexChanged
        If (rblSelect.SelectedValue = "1") Then
            Me.pnlTrainingSession.Visible = False
            Me.dvSaveNotationButtons.Visible = True
            pError.Visible = False
            PEnterNotation.Style("display") = "none"
            ddCertStatus.Enabled = True
            txtStDate.Disabled = True
            txtEDate.Disabled = True

        ElseIf (rblSelect.SelectedValue = "2") Then
            Me.pnlTrainingSession.Visible = False
            Me.dvSaveNotationButtons.Visible = True
            pError.Visible = False
            ddCertStatus.Enabled = False
            txtStDate.Disabled = False
            txtEDate.Disabled = False
            PEnterNotation.Style("display") = "inline"
            hNotId.Value = "0"
            txtDtOccurance.Value = Date.Today.ToShortDateString
            ddNotationType.Enabled = True
            lblDt.Text = Date.Today.ToShortDateString
            'txtDtOccurance.Value = ""
            txtUFDate.Value = Date.Today.ToShortDateString
            chbUnflag.Checked = True
            chbUnflag.Enabled = False
            'ddNotationType.SelectedValue = 0
            For Each it As ListItem In cklReason.Items
                it.Selected = False
            Next
            cklReason.Items.Clear()
            For Each li2 As ListItem In ddNotationType.Items
                li2.Selected = False
            Next
            For Each li As ListItem In ddNotationType.Items
                If li.Text = "Certificate Date Change" Then
                    li.Selected = True
                    NotTypeChanged(Me.ddNotationType, System.EventArgs.Empty)
                    pError.Visible = True
                    pError.InnerHtml = "You must enter notation for this selection: Certificate Date Change"
                    Exit For
                End If
            Next
            ddNotationType.Enabled = False
        ElseIf (rblSelect.SelectedValue = "3") Then
            Me.dvSaveNotationButtons.Visible = False
            pError.Visible = False
            PEnterNotation.Style("display") = "none"
            ddCertStatus.Enabled = False
            Me.txtStDate.Disabled = True
            Me.txtEDate.Disabled = True

            Me.pnlTrainingSession.Visible = True
            GetTrainingSession()

        End If
    End Sub

#Region "ChangeSession"
    Private Sub GetTrainingSession()
        Dim srvMAIS As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Dim srvTraining As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()

        'Get the all Session with from the Current Cert. ID
        Dim CourseInfo As New CourseDetails
        CourseInfo = srvMAIS.GetCourseInformationByCertificationID(CurrentCertSid)


        Dim CurrentSession As New Model.PersonCourseSession
        Dim ReplaceSession As New List(Of Model.SessionCourseInfoDetails)
        Dim SessionDetail As New List(Of Model.SessionCourseInfoDetails)
        Dim repacelist As New List(Of Model.SessionCourseInfoDetails)

        If CourseInfo IsNot Nothing Then
            'get the Current Session With the current Cert ID.
            CurrentSession = srvMAIS.GetCurrnetSessionWithCertificationID(CurrentCertSid)


            If CourseInfo.SessionDetailList IsNot Nothing Then
                SessionDetail = srvMAIS.GetSessionCourseInfoDetailsBySesssionID(CurrentSession.SessionSid)
            End If

            ReplaceSession = srvTraining.GetCourseSessionByRN_Sid(CourseInfo.RN_Sid, CourseInfo.Role_Calegory_Level_Sid)

            repacelist = (From s In ReplaceSession
                          Where s.CourseNumber = CourseInfo.OBNApprovalNumber And s.EndDate < CDate(Me.txtStDate.Value) And s.Session_sID <> SessionDetail(0).Session_sID
                          Select s).ToList
        End If

        gvSearchData.DataSource = SessionDetail
        gvSearchData.DataBind()
        Me.gvSessionReplace.DataSource = repacelist
        Me.gvSessionReplace.DataBind()





    End Sub
#End Region
    Public Function GetNotTypeByStatusType(ByVal s As String) As String
        Dim nStat As String = ""
        If s = "Removed From Registry" Then
            nStat = "Unregistered"
        Else
            nStat = s
        End If
        Return nStat
    End Function
    Public Sub upPreRender()
    End Sub
    Private Sub SaveToUDS(ByVal reissuedcert As Boolean)
        Dim NotDocsHold = CType(Session("NotDocs"), List(Of Business.Model.DocumentUpload))
        Dim strError As String = String.Empty

        If NotDocsHold IsNot Nothing Or reissuedcert = True Then
            Dim maisSerivce As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
            Dim UplSvc As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
            Dim rnmod As Business.Model.RNInformationDetails = Nothing
            Dim ddmod As Business.Model.DDPersonnelDetails = Nothing
            If SessionHelper.RN_Flg Then
                'rnmod = PersSvc.GetRNInfoFromPermanent(SessionHelper.SessionUniqueID)
            Else
                ddmod = PersSvc.GetDDPersonnelInformationFromPermanent(SessionHelper.SessionUniqueID)
            End If
            Dim RnNoOrSSN As String = ""
            Dim dob As Date = Date.Today
            If Not IsNothing(ddmod) Then
                RnNoOrSSN = ddmod.DODDLast4SSN
                dob = ddmod.DODDDateOfBirth
            Else
                RnNoOrSSN = SessionHelper.SessionUniqueID
            End If
            'RnNoOrSSN = IIf(IsNothing(ddmod), SessionHelper.SessionUniqueID, ddmod.DODDLast4SSN)
            Dim UserName As String = "N/A"
            Dim UserSid As String
            ' If Me.RequiredFieldValidator1.Enabled = False Then ' the control will not post back to the server if Enable is False. Need to set the RN From the Hiden field
            UserName = maisSerivce.GetApplicantNameByCode(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg) 'getApplicant Name maisSerivce.GetRNsName(hdfRNID.Value)
            UserSid = maisSerivce.GetApplicantXrefSidByCode(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg) 'getApplicant Name maisSerivce.GetRNsName(hdfRNID.Value)
            'Else
            '    UserName = "" 'getApplicant Name maisSerivce.GetRNsName(hdfRNID.Value) maisSerivce.GetRNsName(ddlRnNames.SelectedValue)
            '    userSid = "" 'getApplicant Name maisSerivce.GetRNsName(hdfRNID.Value) maisSerivce.GetRNsLicenseNumber(ddlRnNames.SelectedValue).Replace("RN", "")

            'End If
            UserName = UserName.Replace(" ", ".")
            Dim wsUDS As New UDSWebService.UDSService() With {.Url = ConfigHelper.UDSServiceEndpoint}

            Dim repositoryName As String = "UDS - MA"

            Dim wsDataSet As New DataSet 'wsUDS.GetGenericIndexes(repositoryName)

            If Not IsNothing(SessionHelper.ApplicationID) Then
                AppID = SessionHelper.ApplicationID
            Else
                AppID = 0
            End If
            Dim xml As String = "<Table> <Index> <Label> Course Number </Label> <Value> " + CStr(hNotId.Value) + "</Value> </Index>" _
                                + "<Index> <Label> Application ID </Label> <Value> " + CStr(AppID) + " </Value> </Index>" _
                                + "<Index> <Label> Application Type </Label> <Value> Other </Value> </Index>" _
                                + "<Index> <Label> DD Personnel Code </Label> <Value> " + IIf(SessionHelper.RN_Flg, "", SessionHelper.SessionUniqueID) + " </Value> </Index>" _
                                + "<Index> <Label> NAME </Label> <Value> " + UserName + " </Value> </Index>" _
                                + "<Index> <Label> RN Lics or 4 SSN </Label> <Value> " + RnNoOrSSN + " </Value> </Index>" _
                                + "<Index> <Label> DOB </Label> <Value> " + String.Format("{0}/{1}/{2}", dob.Year.ToString.PadLeft(4, "0"), dob.Month.ToString.PadLeft(2, "0"), dob.Day.ToString.PadLeft(2, "0")) + " </Value> </Index>" _
                                + "<Index> <Label> Personnel Type </Label> <Value> " + IIf(SessionHelper.RN_Flg, "RN", "DDPERS") + " </Value> </Index>" _
                                + "<Index> <Label> CERT STATUS </Label> <Value>  </Value> </Index>" _
                                + "</Table>"

            Dim Render = New System.IO.StringReader(xml)
            wsDataSet.ReadXml(Render)

            ' rows.FirstOrDefault(Function(r) r("label") = "Course Number")("Value") = Me.txtCourseNumberRnNew.Text
            Dim Document_Int As Integer = 0
            If reissuedcert = True Then
                Dim certdoc As Model.DocumentUpload
                certdoc = UplSvc.GetUploadedReissuedCert(SessionHelper.SessionUniqueID)
                If Not IsNothing(certdoc) Then
                    Dim byteArrayForUDS As Byte() = UplSvc.GetUploadedDocumentByImageSID(certdoc.ImageSID)
                    Dim splitDocName As String() = certdoc.DocumentName.Split("_")
                    Dim DocumentName As String = splitDocName(0) & "_" & splitDocName(1) & "_" & splitDocName(2) & "_" & DateTime.Now.ToString("yyyyMMddmm") & ".pdf"

                    Dim result As UDSWebService.Result = wsUDS.SaveToUDS(repositoryName, _
                                                                         byteArrayForUDS, _
                                                                         UserName, _
                                                                         DocumentName, _
                                                                         "MACERTS", _
                                                                         "MA CERTIFICATE", _
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
                            UplSvc.MarkDocumentSavedUDS(certdoc.ImageSID)
                    End Select
                End If
            Else
                For Each s In NotDocsHold
                    Document_Int += 1
                    Dim byteArrayForUDS As Byte() = s.DocumentImage
                    Dim DocumentName As String = hNotId.Value + "_" + Document_Int.ToString + "_" + s.DocumentName

                    Dim result As UDSWebService.Result = wsUDS.SaveToUDS(repositoryName, _
                                                                         byteArrayForUDS, _
                                                                         UserName, _
                                                                         DocumentName, _
                                                                         "NOTATION", _
                                                                         "NOTATION DOCUMENT", _
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
                            'blnSaveFileToUDS = True
                    End Select
                Next
            End If

        End If
    End Sub
    Private Function getNotationDocs(ByVal notid As String) As List(Of MAIS.Web.UDSWebService.IndexedDocument)
        'Dim RetVal As New List(Of MAIS.Web.UDSWebService.IndexedDocument)
        Dim wsUDS As New UDSWebService.UDSService() With {.Url = ConfigHelper.UDSServiceEndpoint}
        Dim SearchDS As New DataSet

        Dim repositoryName As String = "UDS - MA"

        Dim xml As String = "<Table> <Index> <Label> Course Number </Label> <Value> " + notid + "</Value> </Index>" _
                           + "</Table>"
        Dim Render = New System.IO.StringReader(xml)
        SearchDS.ReadXml(Render)

        Dim randomNum = (New Random()).Next(0, 999999999).ToString.PadLeft(10, "0")

        Dim Int As Integer?

        Dim ListDoc = wsUDS.SearchUDS(repositoryName, Int, Int, "", "", "", "", "NOTATION", "", "", "", SearchDS)
        For Each e In ListDoc
            Dim udsUrl As String = e.DownloadURL.Replace("download.aspx", "madownload.aspx")
            e.DownloadURL = udsUrl + "&UID=" & randomNum & MAIS_Helper.GetUserId & randomNum
        Next
        Return ListDoc.ToList

    End Function

    Public Sub Upload_Click() Handles btnUpload.ServerClick
        Try
            Dim strValid As Boolean = True
            Dim uploadedFiles As New List(Of Business.Model.DocumentUpload)
            'Dim rulesUpload As New Rules.RequiredDocumentRules
            'Dim strErrors As List(Of String) = rulesUpload.ValidatePage(cblUploadRequirements.SelectedIndex, uplCWUploadFile.HasFile,
            '                                                            uplCWUploadFile.PostedFile.ContentLength)
            If Not UploadHelper.IsAcceptableFileType(uplNotationFile.FileName) Then
                strValid = False
                'File is of an unacceptable filetype
                'valsum1.lblErrorLabel.Text += "Please select a file that is in one of the following formats: "
                'Display list of acceptable extensions
                'For Each extension As String In UploadHelper.AcceptableExtensions
                '    lblErrorLabel.Text += extension & " "
                'Next
            End If

            If strValid = True Then
                'File is acceptable for upload
                Dim bytesForFileToSaveToDatabase(uplNotationFile.PostedFile.InputStream.Length - 1) As Byte
                Dim NotDocsHold As New List(Of Business.Model.DocumentUpload)
                If Session("NotDocs") IsNot Nothing Then
                    NotDocsHold = CType(Session("NotDocs"), List(Of Business.Model.DocumentUpload))
                End If
                uplNotationFile.PostedFile.InputStream.Read(bytesForFileToSaveToDatabase, 0, bytesForFileToSaveToDatabase.Length)

                If bytesForFileToSaveToDatabase.Length > 0 Then
                    'Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
                    'Dim uploadDocumentNumber As Long = uploadService.InsertUploadedDocumentIntoImageStore(bytesForFileToSaveToDatabase)

                    'If uploadDocumentNumber > 0 Then
                    Dim uploadedDoc As New Business.Model.DocumentUpload With {
                        .ImageSID = NotDocsHold.Count + 1,
                        .DocumentTypeID = 3,
                        .AppNotId = hNotId.Value,
                        .DocumentName = uplNotationFile.FileName,
                        .UploadDate = Date.Today,
                        .DocumentImage = bytesForFileToSaveToDatabase,
                        .DocumentType = "Notation Document",
                        .FolderName = "Notations",
                        .SourcePage = "Notation.aspx"
                    }
                    NotDocsHold.Add(uploadedDoc)
                    Session("NotDocs") = NotDocsHold
                    gvFiles.DataSource = NotDocsHold
                    gvFiles.DataBind()
                    ' Save file info to database
                    'UploadService.InsertUploadedDocument(uploadedDoc, SessionHelper.ApplicationID) 'sessionhelper.applicationId should be used
                    ''End If
                    ''lblErrorLabel.Text = uplCWUploadFile.PostedFile.FileName & " has been uploaded"
                    ''lblErrorLabel.ForeColor = Drawing.Color.Black
                    'If Not hNotId.Value = "0" Then
                    '    uploadedFiles = UploadService.GetDocumentsByNotation(hNotId.Value) 'SessionHelper.ApplicationID
                    '    gvFiles.DataSource = uploadedFiles
                    '    gvFiles.DataBind()
                    'End If
                End If
            End If
        Catch ex As Exception
            'lblErrorLabel.Text = "Error Uploading File"
            'lblErrorLabel.ForeColor = Drawing.Color.Red
        End Try
    End Sub
    Protected Sub gvFiles_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs) Handles gvFiles.RowDeleting
        'If Master.CertificationApplication.NumaraFootPrintsID > 0 Then
        '    lblErrorLabel.Text = "Files cannot be deleted once the application is submitted."
        '    lblErrorLabel.ForeColor = Drawing.Color.Red
        'Else
        Dim NotDocsHold = CType(Session("NotDocs"), List(Of Business.Model.DocumentUpload))
        NotDocsHold.Remove(NotDocsHold(e.RowIndex))
        Session("NotDocs") = NotDocsHold
        gvFiles.DataSource = NotDocsHold
        gvFiles.DataBind()
        ''If Session("NotDocs") IsNot Nothing Then
        ''    NotDocsHold = CType(Session("NotDocs"), List(Of Business.Model.DocumentUpload))
        ''End If
        'Dim idToDel As Integer = e.Keys.Values(0) '  gvFiles.DataKeys(e.Keys.NewSelectedIndex).Values(0)
        ''Dim filenameToDelete As String = gridViewUploadedFiles.Rows(e.RowIndex).Cells.Item(2).Text
        'Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()

        ''Delete database entry for file
        'uploadService.DeleteDocumentByStoreSid(idToDel)
        'Dim uploadedFiles As New List(Of Business.Model.DocumentUpload)
        'uploadedFiles = uploadService.GetDocumentsByNotation(hNotId.Value) 'SessionHelper.ApplicationID
        'Me.Master.CheckForErrorMessages(uploadService.Messages)
        'lblErrorLabel.Text = filenameToDelete & " has been deleted"
        'lblErrorLabel.ForeColor = Drawing.Color.Black
        'End If
    End Sub
    Public Sub SaveClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.ServerClick
        'Dim res As New ReturnObject(Of Long)(-1L)
        'Dim certificate As New CertificateService()
        'Dim certFicateDetails As New List(Of Model.CertificationDetails)
        'If (rblSelect.SelectedValue = "1") Then
        '    Dim nl As New List(Of NotationDetails)
        '    nl = NotationSvc.GetNotations(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg).ReturnValue
        '    Dim hasNotation As Boolean = False
        '    For Each no As NotationDetails In nl
        '        If no.NotationType.NTypeDesc = ddCertStatus.SelectedItem.Text Then
        '            hasNotation = True
        '        End If
        '    Next

        '    res = MSvc.SetCertStatusAndDates(CurrentCertRcl, CurrentCertRolePersonSid, ddCertStatus.SelectedValue)
        'ElseIf (rblSelect.SelectedValue = "2") Then
        '    res = MSvc.SetCertStatusAndDates(CurrentCertRcl, CurrentCertRolePersonSid, ddCertStatus.SelectedValue, txtStDate.Value, txtEDate.Value)
        '    If res.ReturnValue = 0 Then
        '        For Each mstring As ReturnMessage In res.GeneralMessages
        '            If (mstring.Message.Contains("Certification Changed")) Then
        '                Dim uploadedDocumentSID As Long = GenerateCertificate()
        '                GenerateEmailForUpdateProfile(uploadedDocumentSID)
        '            End If
        '        Next
        '    End If
        'End If
        ''SaveUDS()
    End Sub
    Private Sub GenerateEmailForUpdateProfile(ByVal imageSID As Long)
        Dim retVal As String = String.Empty
        Dim strToEmailAddress As String = Nothing
        Dim strCCEmailAddress As String = Nothing
        strCCEmailAddress = ConfigHelper.CCEmailAddress
        If Not String.IsNullOrEmpty(ConfigHelper.ToEmailAddress) Then
            strToEmailAddress = ConfigHelper.ToEmailAddress
        Else
            Dim ddpersonel As DDPersonnelDetails = Nothing
            Dim rnInfo As RNInformationDetails = Nothing
            Dim EmailHome As String = Nothing, EmailWork As String = Nothing, EmailOther As String = Nothing
            If (SessionHelper.RN_Flg = False) Then
                ddpersonel = PersSvc.GetDDPersonnelInformationFromPermanent(SessionHelper.SessionUniqueID)
                If ddpersonel.Address.Email IsNot Nothing Then
                    For Each ph As EmailAddressDetails In ddpersonel.Address.Email
                        If (ph.ContactType = ContactType.Home) Then
                            EmailHome = ph.EmailAddress.Trim()
                        ElseIf (ph.ContactType = ContactType.Work) Then
                            EmailWork = ph.EmailAddress.Trim()
                        ElseIf (ph.ContactType = ContactType.CellOther) Then
                            EmailOther = ph.EmailAddress.Trim()
                        End If
                    Next
                End If
            Else
                rnInfo = PersSvc.GetRNInfoFromPermanent(SessionHelper.SessionUniqueID)
                If rnInfo.Address.Email IsNot Nothing Then
                    For Each ph As EmailAddressDetails In rnInfo.Address.Email
                        If (ph.ContactType = ContactType.Home) Then
                            EmailHome = ph.EmailAddress.Trim()
                        ElseIf (ph.ContactType = ContactType.Work) Then
                            EmailWork = ph.EmailAddress.Trim()
                        ElseIf (ph.ContactType = ContactType.CellOther) Then
                            EmailOther = ph.EmailAddress.Trim()
                        End If
                    Next
                End If
            End If
            If (Not String.IsNullOrEmpty(EmailHome)) Then
                strToEmailAddress = EmailHome
            Else
                If (Not String.IsNullOrEmpty(EmailWork)) Then
                    strToEmailAddress = EmailWork
                Else
                    strToEmailAddress = EmailOther
                End If
            End If
        End If
        Dim emailSvc As IEmailService = StructureMap.ObjectFactory.GetInstance(Of IEmailService)()
        Dim uploadedFiles As New Business.Model.DocumentUpload
        Dim uploadSvc As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()

        uploadedFiles = uploadSvc.GetUploadedDocumentsByImageSID(imageSID)
        Dim ms As New MemoryStream
        Dim imageStore() As Byte = uploadSvc.GetUploadedDocumentByImageSID(uploadedFiles.ImageSID)
        ms.Write(imageStore, 0, imageStore.Length)
        ms.Position = 0
        Dim strBodyMessage As String = String.Empty
        strBodyMessage = "You are certification dates are change<br/><br/>"
        strBodyMessage = strBodyMessage + "Please check<br/><br/>"
        strBodyMessage = strBodyMessage + "For further information feel free to contact your RN Trainer " + UserAndRoleHelper.CurrentUser.Email +
            " or DODD if you have any questions or concerns about this certification or safe performance of medication administration and health related activities.<br/><br/>"
        strBodyMessage = strBodyMessage + "Thank you"
        Dim retObj As ReturnObject(Of Boolean) = emailSvc.SendEmail(strToEmailAddress.Trim(),
                                                                    ConfigHelper.FromEmailAddress,
                                                                    ConfigHelper.EmailSubjectStatus,
                                                                    strBodyMessage, ms, uploadedFiles.DocumentName, strCCEmailAddress.Trim())

        If retObj.ReturnValue Then
            retVal = "An email containing your status of this application was sent successfully."
        Else
            If retObj.Messages.Count > 0 Then
                retVal = retObj.MessageStrings.First()
            Else
                retVal = "ERROR: An error has occurred while trying to send an email for QA registeration."
            End If
        End If
        'Next
        pnote.Style("display") = "inline"
        pnote.InnerHtml = retVal
    End Sub
    Private Function GenerateCertificate() As Long
        Select Case CurrentCertRcl
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
        Dim certificate As ICertificateService = StructureMap.ObjectFactory.GetInstance(Of ICertificateService)()
        'Dim certificate As New CertificateService()
        'StructureMap.ObjectFactory.GetInstance(Of ICertificateService)()
        rvCertificate.ProcessingMode = ProcessingMode.Local
        Dim permissions As PermissionSet = New PermissionSet(PermissionState.Unrestricted)
        rvCertificate.LocalReport.SetBasePermissionsForSandboxAppDomain(permissions)
        If ((CurrentCertRcl = 4 Or CurrentCertRcl = RoleLevelCategory.Bed17_RLC) And (SessionHelper.ApplicationType <> EnumHelper.GetEnumDescription(ApplicationType.Renewal))) Then
            Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfoUsingCertMod(SessionHelper.SessionUniqueID, SessionHelper.ApplicationType, CurrentCertRcl, CurrentCertSid))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", 0)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", SessionHelper.ApplicationType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", SessionHelper.SessionUniqueID)
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/RNTrainerCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
        End If
        If ((CurrentCertRcl = RoleLevelCategory.RNInstructor_RLC) And (SessionHelper.ApplicationType <> EnumHelper.GetEnumDescription(ApplicationType.Renewal))) Then
            Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfoUsingCertMod(SessionHelper.SessionUniqueID, SessionHelper.ApplicationType, CurrentCertRcl, CurrentCertSid))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", 0)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", SessionHelper.ApplicationType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", SessionHelper.SessionUniqueID)
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/RNInstructorCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
        End If
        If ((CurrentCertRcl = RoleLevelCategory.RNInstructor_RLC) And (SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.Renewal))) Then
            Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfoUsingCertMod(SessionHelper.SessionUniqueID, SessionHelper.ApplicationType, CurrentCertRcl, CurrentCertSid))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", 0)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", SessionHelper.ApplicationType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", SessionHelper.SessionUniqueID)
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/RNInstructorRenewalCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
        End If
        If ((CurrentCertRcl = 4 Or CurrentCertRcl = RoleLevelCategory.Bed17_RLC) And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.Renewal)) Then
            Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfoUsingCertMod(SessionHelper.SessionUniqueID, SessionHelper.ApplicationType, CurrentCertRcl, CurrentCertSid))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", 0)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", SessionHelper.ApplicationType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", SessionHelper.SessionUniqueID)
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/RNTrainerRenewalCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
        End If
        If ((CurrentCertRcl = RoleLevelCategory.DDPersonnel_RLC Or CurrentCertRcl = RoleLevelCategory.DDPersonnel2_RLC Or CurrentCertRcl = RoleLevelCategory.DDPersonnel3_RLC) And SessionHelper.ApplicationType <> EnumHelper.GetEnumDescription(ApplicationType.Renewal)) Then
            Dim params(4) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateDDInfoUsingCertMod(SessionHelper.SessionUniqueID, SessionHelper.ApplicationType, CurrentCertRcl, CurrentCertSid))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", 0)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", SessionHelper.ApplicationType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("DDCategoryCode", SessionHelper.SessionUniqueID)
            params(4) = New Microsoft.Reporting.WebForms.ReportParameter("TodayDate", DateTime.Today.ToShortDateString())
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/DDCategoryInitialCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
        End If
        If ((CurrentCertRcl = RoleLevelCategory.DDPersonnel_RLC Or CurrentCertRcl = RoleLevelCategory.DDPersonnel2_RLC Or CurrentCertRcl = RoleLevelCategory.DDPersonnel3_RLC) And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.Renewal)) Then
            Dim params(4) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateDDInfoUsingCertMod(SessionHelper.SessionUniqueID, SessionHelper.ApplicationType, CurrentCertRcl, CurrentCertSid))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", 0)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", SessionHelper.ApplicationType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("DDCode", SessionHelper.SessionUniqueID)
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
                .DocumentName = "Reissue" & "_" & SessionHelper.SessionUniqueID & "_" & SessionHelper.ApplicationType & "_" & Convert.ToString(DateTime.Now) & ".pdf"
                .DocumentType = "9"
            End With
            ' Save file info to database
            uploadService.InsertUploadedDocument(uploadedDoc, 0) 'sessionhelper.applicationId should be used

        End If
        rvCertificate.LocalReport.Refresh()
        rvCertificate.Reset()
        Return uploadDocumentNumber
    End Function
    Public Sub NotTypeChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For Each it As ListItem In cklReason.Items
            it.Selected = False
        Next
        cklReason.Items.Clear()
        Dim i = 0
        Dim dnmr As Boolean = IIf(ddNotationType.SelectedItem.Text = "Did Not Meet Requirements", True, False)
        If dnmr Then
            For Each l1 As ListItem In lbHReasons.Items
                If l1.Text = "DODD" Then
                    Exit For
                End If
                i = i + 1
                cklReason.Items.Add(l1)
            Next
        Else
            For Val As Integer = 5 To lbHReasons.Items.Count - 1
                cklReason.Items.Add(lbHReasons.Items(Val))
            Next
        End If
        For Each it As ListItem In cklReason.Items
            it.Selected = False
        Next
    End Sub
    Public Sub btnAddClicked(ByVal sender As Object, ByVal e As System.EventArgs)
        PEnterNotation.Style("display") = "inline"
        hNotId.Value = "0"
        ddNotationType.Enabled = True
        lblDt.Text = Date.Today.ToShortDateString
        Session("NotDocs") = Nothing
        gvFiles.DataBind()
        gvUdsFiles.DataSource = Nothing
        gvUdsFiles.DataBind()
        txtDtOccurance.Disabled = False

        txtDtOccurance.Value = ""
        txtUFDate.Value = ""
        chbUnflag.Checked = False
        chbUnflag.Enabled = False
        ddNotationType.SelectedValue = 0
        For Each li As ListItem In ddNotationType.Items
            If li.Text = "Certificate Date Change" Then
                li.Enabled = False
            End If
        Next
        cklReason.Enabled = True
        For Each it As ListItem In cklReason.Items
            it.Selected = False
        Next
        cklReason.Items.Clear()

    End Sub
    Protected Sub ServerValidate(source As Object, args As ServerValidateEventArgs)
        args.IsValid = False
        For Each li As ListItem In cklReason.Items
            If li.Selected = True Then
                args.IsValid = True
            End If
        Next

    End Sub
    Public Sub SaveNotation() Handles btnSaveNotation.ServerClick
        Dim res As Long
        Dim nd As New NotationDetails
        Page.Validate()
        If (Page.IsValid) Then
            If hNotId.Value = "0" Then
                Dim NotDocsHold2 = CType(Session("NotDocs"), List(Of Business.Model.DocumentUpload))
                If IsNothing(NotDocsHold2) Then
                    pError.Visible = True
                    pError.InnerHtml = "You must upload a document with this notation."
                    divSpinner.Style("display") = "none"
                    Exit Sub
                End If
                'If PEnterNotation.Style("display") = "none" Then
                '    pError.Style("display") = "inline"
                '    pError.InnerHtml = "You must enter a notation."
                'End If
                If IsDate(txtStDate.Value) Then
                    If CDate(txtStDate.Value) > Date.Today Then
                        pError.Visible = True
                        pError.InnerHtml = "Start Date cannot be greater than today"
                        Exit Sub
                    End If
                Else
                    pError.Visible = True
                    pError.InnerHtml = "Please enter a valid start date"
                    Exit Sub
                End If
                If IsDate(txtEDate.Value) Then
                    If CDate(txtEDate.Value) < CDate(txtStDate.Value) Then
                        pError.Visible = True
                        pError.InnerHtml = "End Date cannot be less than Start Date"
                        Exit Sub
                    End If
                Else
                    pError.Visible = True
                    pError.InnerHtml = "Please enter a valid start date"
                    Exit Sub
                End If
                '''''''''''''''''''''''''''''
                Dim result As New ReturnObject(Of Long)(-1L)
                'Dim certificate As New CertificateService()
                'Dim certFicateDetails As New List(Of Model.CertificationDetails)
                If (rblSelect.SelectedValue = "1") Then
                    result = MSvc.SetCertStatusAndDates(SessionHelper.RN_Flg, CurrentCertRolePersonSid, ddCertStatus.SelectedValue)
                ElseIf (rblSelect.SelectedValue = "2") Then

                    result = MSvc.SetCertStatusAndDates(SessionHelper.RN_Flg, CurrentCertRolePersonSid, ddCertStatus.Items.FindByText(curCert.Status).Value, txtStDate.Value, txtEDate.Value)

                End If
                If result.ReturnValue = 0 Then
                    For Each mstring As ReturnMessage In result.GeneralMessages
                        If (mstring.Message.Contains("Certification Changed")) Then
                            Dim uploadedDocumentSID As Long


                            If ddCertStatus.SelectedItem.Text <> "Registered" Then
                                uploadedDocumentSID = GenerateCertificate()
                                GenerateEmailForUpdateProfile(uploadedDocumentSID)
                                SaveToUDS(True)
                            Else
                                uploadedDocumentSID = -1
                                GenerateEmailForUpdateProfile(uploadedDocumentSID)
                            End If




                        End If
                    Next
                    pError.Visible = False
                Else
                    pError.Visible = True
                    pError.InnerHtml = "Error occured </br>"
                    For Each mstring As ReturnMessage In result.GeneralMessages
                        If (mstring.Message.Contains("rror")) Then
                            pError.InnerHtml += mstring.Message & "</br>"
                        End If
                    Next
                    divSpinner.Style("display") = "none"
                    Exit Sub
                End If
                ''''''''''''''
                nd.NotationDate = Date.Today
                nd.OccurenceDate = Convert.ToDateTime(txtDtOccurance.Value)
                nd.PersonEnteringNotation = txtPerson.Value
                nd.PersonTitle = txtPersonTitle.Value
                nd.NotationType = New NType With {.NTypeSid = ddNotationType.SelectedValue}
                If Not IsNothing(SessionHelper.ApplicationID) And SessionHelper.ApplicationID > 0 Then
                    nd.AppId = SessionHelper.ApplicationID
                End If
                Dim nrlist As New List(Of NReason)
                For Each it As ListItem In cklReason.Items
                    If it.Selected Then
                        nrlist.Add(New NReason With {.NReasonSid = CInt(it.Value)})
                    End If
                Next
                nd.NotationReasons = nrlist
                If IsDate(txtUFDate.Value.ToString) Then
                    nd.UnflaggedDate = txtUFDate.Value.ToString
                End If
                '_nd = New NotationDetails
                '_nd.NotationDate = DateTime.Now
                '_nd.OccurenceDate = Convert.ToDateTime(txtDtOccurance.Value)
                '_nd.PersonEnteringNotation = txtPerson.Value
                '_nd.PersonTitle = txtPersonTitle.Value
                ''Add reasons later
                ''_nd.NotationType = ddReasonNotation.SelectedItem.Text
                res = NotationSvc.SaveNotation(nd, SessionHelper.SessionUniqueID, SessionHelper.RN_Flg).ReturnValue
                hNotId.Value = res
                SaveToUDS(False)
                divSpinner.Style("display") = "none"
                Session("NotDocs") = Nothing
                'If hStatus.Value <> "" Then
                '    'NotationSvc.SetAppStatus(SessionHelper.ApplicationID, hStatus.Value)
                '    'SessionHelper.ApplicationStatus = hStatus.Value
                '    'set app status to the right status, and sessionhelper appstatus too
                '    Response.Redirect("Summary.aspx?selstat=" & hStatus.Value.Replace(" ", "-"))
                'End If
                gvFiles.DataBind()
                '''''''''''''''''
                hNotId.Value = "0"
            Else
                nd = NotationSvc.GetNotationByNotID(CInt(hNotId.Value)).ReturnValue
                If chbUnflag.Checked = True Then
                    nd.UnflaggedDate = Date.Today
                End If
                nd.AllReasons = ""
                nd.OccurenceDate = Convert.ToDateTime(txtDtOccurance.Value)
                Dim nrlist As New List(Of NReason)
                For Each it As ListItem In cklReason.Items
                    If it.Selected Then
                        nrlist.Add(New NReason With {.NReasonSid = CInt(it.Value)})
                    End If
                Next
                nd.NotationReasons = nrlist
                res = NotationSvc.UpdateNotation(nd, CInt(hNotId.Value)).ReturnValue
                SaveToUDS(False)
                Session("NotDocs") = Nothing
                gvFiles.DataBind()
                hNotId.Value = "0"
            End If
        End If
        divSpinner.Style("display") = "none"
        'pError.Visible = False
        pnote.Style("display") = "inline"
        pnote.InnerHtml = "Saved Successfuly."
        'ValidationSummary1.
        'errors PEnterNotation.Style("display") = "inline"
        grdNotations.DataSource = NotationSvc.GetNotations(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg).ReturnValue
        grdNotations.DataBind()
        PEnterNotation.Style("display") = "none"
        'res = query1.SaveNotation(no, 11, 2).ReturnValue
    End Sub

    Protected Sub EditCmd(sender As Object, e As GridViewCommandEventArgs)
        Dim aid As Integer = CInt(grdNotations.DataKeys(Convert.ToInt32(e.CommandArgument)).Value.ToString())
        'Dim query2 As Data.Queries.NotationQueries = Nothing
        Dim uploadedFiles As New List(Of Business.Model.DocumentUpload)
        Dim uploadSvc As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
        Dim n As Business.Model.NotationDetails = Nothing ' Business.Model.NotationDetails = Nothing
        'query2 = StructureMap.ObjectFactory.GetInstance(Of Data.Queries.NotationQueries)()
        Session("NotDocs") = Nothing
        gvFiles.DataBind()
        n = NotationSvc.GetNotationByNotID(aid).ReturnValue
        ddNotationType.SelectedValue = n.NotationType.NTypeSid
        lblDt.Text = n.NotationDate.ToShortDateString()
        txtDtOccurance.Value = n.OccurenceDate.ToShortDateString()
        txtPerson.Value = n.PersonEnteringNotation.ToString()
        If IsDate(n.UnflaggedDate) Then
            txtUFDate.Value = n.UnflaggedDate
            chbUnflag.Checked = True
            chbUnflag.Enabled = False
            'chbUnflag.Attributes.Clear()
            'chbUnflag.Attributes.Add("disabled", "true")
        Else
            txtUFDate.Value = ""
            chbUnflag.Checked = False
            If (SessionHelper.MAISLevelUserRole = Enums.RoleLevelCategory.RNMaster_RLC Or UserAndRoleHelper.IsUserAdmin = True) Then
                chbUnflag.Enabled = True
            End If
            'chbUnflag.Attributes.Clear()
        End If
        PEnterNotation.Style("display") = "inline"
        'txtPersonTitle.Value = IIf(IsNullOrEmpty(n.PersonTitle), "", n.PersonTitle.ToString)
        NotTypeChanged(ddNotationType, System.EventArgs.Empty)
        For Each it As ListItem In cklReason.Items
            it.Selected = False
        Next
        For Each r As Business.Model.NReason In n.NotationReasons
            cklReason.Items.FindByValue(r.NReasonSid).Selected = True
        Next
        If (SessionHelper.MAISLevelUserRole = Enums.RoleLevelCategory.RNMaster_RLC Or UserAndRoleHelper.IsUserAdmin = True) Then
            cklReason.Enabled = True
        Else
            cklReason.Enabled = False
        End If
        hNotId.Value = aid
        ddNotationType.Enabled = False
        'txtDtOccurance.Disabled = True

        If hNotId.Value = "0" Then
        Else
            If (SessionHelper.MAISLevelUserRole = Enums.RoleLevelCategory.RNMaster_RLC Or UserAndRoleHelper.IsUserAdmin = True) Then
                Dim udslist As New List(Of MAIS.Web.UDSWebService.IndexedDocument)
                udslist = getNotationDocs(hNotId.Value) ' As List(Of MAIS.Web.UDSWebService.IndexedDocument)
                gvUdsFiles.DataSource = udslist
                gvUdsFiles.DataBind()
                'Debug.Print(udslist(0).DownloadURL)
                'uploadedFiles = uploadSvc.GetDocumentsByNotation(hNotId.Value) 'SessionHelper.ApplicationID
                'gvFiles.DataSource = uploadedFiles
                'gvFiles.DataBind()
            End If
        End If

    End Sub

    Public Function GetColumnIndexByHeaderText(g As GridView, ct As String)
        Dim cell As TableCell
        For i As Integer = 0 To g.HeaderRow.Cells.Count
            cell = g.HeaderRow.Cells(i)
            If cell.Text.ToString() = ct Then
                Return i
            End If
        Next
        Return -1
    End Function
    Protected Sub gvFiles_SelectedIndexChanging(ByVal sender As Object, ByVal e As GridViewSelectEventArgs) Handles gvFiles.SelectedIndexChanging
        'Transmit copy of selected file to user for preview
        'Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
        Dim byteArrayForFileToView As Byte() '= uploadService.GetUploadedDocumentByImageSID(gvFiles.DataKeys(e.NewSelectedIndex).Values(0))
        Dim NotDocsHold = CType(Session("NotDocs"), List(Of Business.Model.DocumentUpload))
        byteArrayForFileToView = NotDocsHold(e.NewSelectedIndex).DocumentImage
        'Clear all content output from buffer stream
        Response.Clear()
        Dim filenameCell As Integer = 2

        If gvFiles.AutoGenerateDeleteButton = False Then
            filenameCell = 1
        End If

        'Add HTTP header to output stream to specify a default filename, and file length
        Response.AddHeader("Content-Disposition", "attachment; filename=" & _
            NotDocsHold(e.NewSelectedIndex).DocumentName)
        Response.AddHeader("Content-Length", byteArrayForFileToView.Length.ToString())
        'Set the HTTP MIME type for output stream
        Response.ContentType = "application/octet-stream"
        'Output the data to the client
        If byteArrayForFileToView.Length > 0 Then
            'Dim ext = System.IO.Path.GetExtension(gvFiles.Rows(e.NewSelectedIndex).Cells(filenameCell).Text.ToString)
            'If ext = ".html" Then
            '    Response.Redirect(PagesHelper.AttestationPrintPage)
            'Else
            Response.BinaryWrite(byteArrayForFileToView)
            'End If
            Response.End()
        End If
    End Sub
    Public Shared Property AppID As Integer
        Private Get
            Return _appID
        End Get
        Set(ByVal value As Integer)
            _appID = value
        End Set
    End Property

    Protected Sub gvSessionReplace_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvSessionReplace.SelectedIndexChanged
        Dim Index As Integer = CType(sender, GridView).SelectedIndex
        If Index > -1 Then
            Me.bntSaveSession.Enabled = True
        Else
            Me.bntSaveSession.Enabled = False
        End If
    End Sub

    Protected Sub bntSaveSession_Click(sender As Object, e As EventArgs) Handles bntSaveSession.Click
        Dim sid As Integer
        sid = Me.gvSessionReplace.SelectedDataKey.Value

        Dim srvMAIS As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        If srvMAIS.UpdateSessionCourseInfoSession(sid, CurrentCertRolePersonSid, CurrentCertSid) = True Then
            GetTrainingSession()
            pError.Visible = True
            pError.InnerHtml = "The session was save. "
        Else
            pError.Visible = True
            pError.InnerHtml = "The session did not save. Please try again later."
        End If
    End Sub
End Class