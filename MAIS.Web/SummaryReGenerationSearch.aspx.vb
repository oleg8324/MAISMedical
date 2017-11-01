Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Helpers
Imports Microsoft.Reporting.WebForms
Imports System.Security
Imports System.Security.Permissions
Imports MAIS.Business
Imports MAIS.Business.Services
Imports ODMRDD_NET2
Imports System.IO
Imports MAIS.Business.Model.Enums
Imports MAIS.Business.Model

Public Class SummaryReGenerationSearch
    Inherits System.Web.UI.Page
    Private Shared MSvc As IMAISSerivce
    Private Shared CurrentCertRcl As Integer
    Private Shared CurrentCertSDate As Date
    Private Shared CurrentCertRolePersonSid As Integer
    Private Shared CurrentCertSid As Integer
    Private Shared CurrentCertApplicationType As String
    Private Shared CurrentCertApplicationID As Integer
    Private Shared CurrentRNorDDFlg As Boolean
    Dim _roles As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MSvc = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Master.HideProgressBar = True
        If Not IsPostBack Then
            LoadPersonnalData()
            LoadApplication()
        End If
    End Sub


    Private Sub LoadPersonnalData()
        Dim personalSvc As IPersonalInformationService = StructureMap.ObjectFactory.GetInstance(Of IPersonalInformationService)()
        Dim ddpersonel As DDPersonnelDetails = Nothing
        Dim rnInfo As RNInformationDetails = Nothing

        If (SessionHelper.RN_Flg = False) Then
            ddpersonel = personalSvc.GetDDPersonnelInformationFromPermanent(Master.RNLicenseOrSSN)
            'ddpersonel = personalSvc.GetDDPersonnelInformation(AppID)
        Else
            rnInfo = personalSvc.GetRNInfoFromPermanent(Master.RNLicenseOrSSN)
            'rnInfo = personalSvc.GetRNInformation(AppID)
        End If
        SetAllDefaultValues(ddpersonel, rnInfo)

    End Sub

    Private Sub gvCertHistory_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvCertHistory.RowCommand


        Select Case e.CommandName
            Case "RecreateSummary"
                Dim ep As Int16
                ep = e.CommandArgument
                AppToPdfAndDB(ep, Master.RNLicenseOrSSN.ToString)

            Case "RecreateCertificate"

                Dim ep As Integer
                ep = e.CommandArgument
                GenerateCertificate(ep)

        End Select

    End Sub

    Private Sub SetAllDefaultValues(ByVal ddpersonal As DDPersonnelDetails, ByVal rnInfo As RNInformationDetails)
        If (ddpersonal IsNot Nothing) Then
            lblRNLNoOrSSNtxt.Text = ddpersonal.DODDLast4SSN.Trim()
            lblDtIssuedOrDOBtxt.Text = ddpersonal.DODDDateOfBirth.ToShortDateString().Trim()
            lblLastName.Text = ddpersonal.DODDLastName.Trim()
            lblFirstName.Text = ddpersonal.DODDFirstName.Trim()
            If Not (ddpersonal.DODDMiddleName Is Nothing) Then
                lblMiddleName.Text = ddpersonal.DODDMiddleName.Trim()
            End If

            If (Not String.IsNullOrEmpty(ddpersonal.DODDHomeAddressLine1)) Then
                lblAddr1.Text = ddpersonal.DODDHomeAddressLine1.Trim()
            Else
                lblAddr1.Text = String.Empty
            End If


            If (Not String.IsNullOrWhiteSpace(ddpersonal.DODDHomeAddressLine2)) Then
                lblAddr2.Text = ddpersonal.DODDHomeAddressLine2.Trim()
            Else
                lblAddr2.Text = String.Empty
            End If

            If (Not String.IsNullOrWhiteSpace(ddpersonal.DODDHomeCity)) Then
                lblCity.Text = ddpersonal.DODDHomeCity.Trim()
            Else
                lblCity.Text = String.Empty
            End If


            If (Not String.IsNullOrWhiteSpace(ddpersonal.DODDHomeCity)) Then
                lblCounty.Text = ddpersonal.DODDHomeCounty.Trim()
            Else
                lblCounty.Text = String.Empty
            End If

            If (Not String.IsNullOrWhiteSpace(ddpersonal.DODDHomeState)) Then
                lblState.Text = ddpersonal.DODDHomeState.Trim()
            Else
                lblState.Text = String.Empty
            End If

            If (Not String.IsNullOrWhiteSpace(ddpersonal.DODDHomeZip)) Then
                lblZip.Text = ddpersonal.DODDHomeZip.Trim()
            Else
                lblZip.Text = String.Empty
            End If


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
            'If ddpersonal.Address.Phone IsNot Nothing Then
            '    For Each ph As Business.Model.PhoneDetails In ddpersonal.Address.Phone
            '        If (ph.ContactType = ContactType.Home) Then
            '            lblHomePhoneNumber.Text = ph.PhoneNumber.Trim().Substring(0, 3) & "-" & ph.PhoneNumber.Trim().Substring(3, 3) & "-" & ph.PhoneNumber.Trim().Substring(6, 4)
            '        ElseIf (ph.ContactType = ContactType.Work) Then
            '            lblWorkPhoneNumber.Text = ph.PhoneNumber.Trim().Substring(0, 3) & "-" & ph.PhoneNumber.Trim().Substring(3, 3) & "-" & ph.PhoneNumber.Trim().Substring(6, 4)
            '        ElseIf (ph.ContactType = ContactType.CellOther) Then
            '            lblCellPhoneNumber.Text = ph.PhoneNumber.Trim().Substring(0, 3) & "-" & ph.PhoneNumber.Trim().Substring(3, 3) & "-" & ph.PhoneNumber.Trim().Substring(6, 4)
            '        End If
            '    Next
            'End If

            'If ddpersonal.Address.Email IsNot Nothing Then
            '    For Each ph As Business.Model.EmailAddressDetails In ddpersonal.Address.Email
            '        If (ph.ContactType = ContactType.Home) Then
            '            lblHomeAddress.Text = ph.EmailAddress.Trim()
            '        ElseIf (ph.ContactType = ContactType.Work) Then
            '            lblWorkAddress.Text = ph.EmailAddress.Trim()
            '        ElseIf (ph.ContactType = ContactType.CellOther) Then
            '            lblCellAddress.Text = ph.EmailAddress.Trim()
            '        End If
            '    Next
            'End If

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
            'For Each ph As Business.Model.PhoneDetails In rnInfo.Address.Phone
            '    If (ph.ContactType = ContactType.Home) Then
            '        lblHomePhoneNumber.Text = ph.PhoneNumber.Trim().Substring(0, 3) & "-" & ph.PhoneNumber.Trim().Substring(3, 3) & "-" & ph.PhoneNumber.Trim().Substring(6, 4)
            '    ElseIf (ph.ContactType = ContactType.Work) Then
            '        lblWorkPhoneNumber.Text = ph.PhoneNumber.Trim().Substring(0, 3) & "-" & ph.PhoneNumber.Trim().Substring(3, 3) & "-" & ph.PhoneNumber.Trim().Substring(6, 4)
            '    ElseIf (ph.ContactType = ContactType.CellOther) Then
            '        lblCellPhoneNumber.Text = ph.PhoneNumber.Trim().Substring(0, 3) & "-" & ph.PhoneNumber.Trim().Substring(3, 3) & "-" & ph.PhoneNumber.Trim().Substring(6, 4)
            '    End If
            'Next
            'For Each ph As Business.Model.EmailAddressDetails In rnInfo.Address.Email
            '    If (ph.ContactType = ContactType.Home) Then
            '        lblHomeAddress.Text = ph.EmailAddress.Trim()
            '    ElseIf (ph.ContactType = ContactType.Work) Then
            '        lblWorkAddress.Text = ph.EmailAddress.Trim()
            '    ElseIf (ph.ContactType = ContactType.CellOther) Then
            '        lblCellAddress.Text = ph.EmailAddress.Trim()
            '    End If
            'Next
        End If
    End Sub

    Private Sub LoadApplication()
        Dim certHis As List(Of Business.Model.Certificate) = MSvc.GetCertificationHistory(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg)
        Dim curCert As Business.Model.Certificate = (From ch In certHis Where ch.Role_Category_Level_Sid = SessionHelper.SelectedUserRole Select ch).FirstOrDefault ' And ch.EndDate > Date.Today

        CurrentCertSDate = curCert.StartDate
        CurrentCertRcl = curCert.Role_Category_Level_Sid
        CurrentCertRolePersonSid = curCert.Role_RN_DD_Personnel_Xref_Sid
        CurrentCertApplicationType = curCert.ApplicationType
        CurrentCertSid = curCert.Certification_Sid
        CurrentCertApplicationID = curCert.Application_Sid
        CurrentRNorDDFlg = SessionHelper.RN_Flg
        If CurrentCertApplicationID = 0 Then
            Me.lblPanelMessage.Text = "This Certification was imported from Old MA. Can not recreate Summary or Certification."
        Else
            Me.lblPanelMessage.Text = String.Empty

            Me.gvCertHistory.DataSource = (From r In certHis _
                               Where r.Certification_Sid = curCert.Certification_Sid
                               Select New With
                                              {
                                               .Category = r.Category,
                                               .Application_Sid = r.Application_Sid,
                                               .Certification_Sid = r.Certification_Sid,
                                               .Level = r.Level,
                                               .Role = r.Role,
                                               .StartDate = r.StartDate.ToShortDateString(),
                                               .EndDate = If(r.EndDate.ToShortDateString() = "12/31/9999", String.Empty, r.EndDate.ToShortDateString()),
                                               .Role_Category_Level_Sid = r.Role_Category_Level_Sid,
                                               .hdRoleLevelCategory = r.Role_Category_Level_Sid,
                                               .Status = r.Status,
                                               .Role_RN_DD_Personnel_Xref_Sid = r.Role_RN_DD_Personnel_Xref_Sid,
                                               .CertAllowed = IIf({6, 7}.Contains(r.Role_Category_Level_Sid), False, True)}).ToList


            Me.gvCertHistory.DataBind()
        End If


    End Sub

    Private Function GenerateCertificate(ByVal mAppID As Integer) As Long
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
        If ((CurrentCertRcl = 4 Or CurrentCertRcl = RoleLevelCategory.Bed17_RLC) And (CurrentCertApplicationType <> EnumHelper.GetEnumDescription(ApplicationType.Renewal))) Then
            Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfoUsingCertMod(Master.RNLicenseOrSSN, CurrentCertApplicationType, CurrentCertRcl, CurrentCertSid))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", mAppID)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", CurrentCertApplicationType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", Master.RNLicenseOrSSN)
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/RNTrainerCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
        End If
        If ((CurrentCertRcl = RoleLevelCategory.RNInstructor_RLC) And (CurrentCertApplicationType <> EnumHelper.GetEnumDescription(ApplicationType.Renewal))) Then
            Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfoUsingCertMod(Master.RNLicenseOrSSN, CurrentCertApplicationType, CurrentCertRcl, CurrentCertSid))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", mAppID)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", CurrentCertApplicationType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", Master.RNLicenseOrSSN)
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/RNInstructorCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
        End If
        If ((CurrentCertRcl = RoleLevelCategory.RNInstructor_RLC) And (CurrentCertApplicationType = EnumHelper.GetEnumDescription(ApplicationType.Renewal))) Then
            Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfoUsingCertMod(Master.RNLicenseOrSSN, CurrentCertApplicationType, CurrentCertRcl, CurrentCertSid))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", mAppID)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", CurrentCertApplicationType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", Master.RNLicenseOrSSN)
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/RNInstructorRenewalCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
        End If
        If ((CurrentCertRcl = 4 Or CurrentCertRcl = RoleLevelCategory.Bed17_RLC) And CurrentCertApplicationType = EnumHelper.GetEnumDescription(ApplicationType.Renewal)) Then
            Dim params(3) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateInfoUsingCertMod(Master.RNLicenseOrSSN, CurrentCertApplicationType, CurrentCertRcl, CurrentCertSid))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", mAppID)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", CurrentCertApplicationType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("RNLicenseNumber", Master.RNLicenseOrSSN)
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/RNTrainerRenewalCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
        End If
        If ((CurrentCertRcl = RoleLevelCategory.DDPersonnel_RLC Or CurrentCertRcl = RoleLevelCategory.DDPersonnel2_RLC Or CurrentCertRcl = RoleLevelCategory.DDPersonnel3_RLC) And CurrentCertApplicationType <> EnumHelper.GetEnumDescription(ApplicationType.Renewal)) Then
            Dim params(4) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateDDInfoUsingCertMod(Master.RNLicenseOrSSN, CurrentCertApplicationType, CurrentCertRcl, CurrentCertSid))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", mAppID)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", CurrentCertApplicationType)
            params(2) = New Microsoft.Reporting.WebForms.ReportParameter("Role", _roles)
            params(3) = New Microsoft.Reporting.WebForms.ReportParameter("DDCategoryCode", Master.RNLicenseOrSSN)
            params(4) = New Microsoft.Reporting.WebForms.ReportParameter("TodayDate", DateTime.Today.ToShortDateString())
            rvCertificate.LocalReport.Refresh()
            rvCertificate.LocalReport.ReportPath = "Reports/DDCategoryInitialCertificate.rdlc"
            rvCertificate.LocalReport.SetParameters(params)
            rvCertificate.LocalReport.DataSources.Add(reportds)
            rvCertificate.LocalReport.Refresh()
        End If
        If ((CurrentCertRcl = RoleLevelCategory.DDPersonnel_RLC Or CurrentCertRcl = RoleLevelCategory.DDPersonnel2_RLC Or CurrentCertRcl = RoleLevelCategory.DDPersonnel3_RLC) And CurrentCertApplicationType = EnumHelper.GetEnumDescription(ApplicationType.Renewal)) Then
            Dim params(4) As Microsoft.Reporting.WebForms.ReportParameter

            rvCertificate.Reset()

            Dim reportds As New ReportDataSource("dsCertificate", certificate.GetCertificateDDInfoUsingCertMod(Master.RNLicenseOrSSN, CurrentCertApplicationType, CurrentCertRcl, CurrentCertSid))

            params(0) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationID", mAppID)
            params(1) = New Microsoft.Reporting.WebForms.ReportParameter("ApplicationType", CurrentCertApplicationType)
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
        'Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
        'Dim uploadDocumentNumber As Long = uploadService.InsertUploadedDocumentIntoImageStore(myBytes)
        'If uploadDocumentNumber > 0 Then
        '    Dim uploadedDoc As New Business.Model.DocumentUpload()

        '    'Generate model for database information
        '    With uploadedDoc
        '        .ImageSID = uploadDocumentNumber
        '        .DocumentName = "Reissue" & "_" & SessionHelper.SessionUniqueID & "_" & SessionHelper.ApplicationType & "_" & Convert.ToString(DateTime.Now) & ".pdf"
        '        .DocumentType = "9"
        '    End With
        '    ' Save file info to database
        '    uploadService.InsertUploadedDocument(uploadedDoc, mAppID) 'sessionhelper.applicationId should be used

        'End If

        ' for testing popup window to view Cert. 
        Response.AddHeader("content-disposition", "attachment;filename= TestCert" + CStr(CurrentCertSid) + ".pdf")
        Response.ContentType = "application/octectstream"
        Response.BinaryWrite(myBytes)
        Response.End()
        '------------------
        rvCertificate.LocalReport.Refresh()
        rvCertificate.Reset()
        Return 1 ' uploadDocumentNumber
    End Function

    Public Sub AppToPdfAndDB(ByVal ProcessAppID As Integer, ByVal UserID As String)
        Dim sw As New System.IO.StringWriter()
        Dim hw As New HtmlTextWriter(sw)
        Dim strError As String = String.Empty

        'HttpContext.Current.Server.Execute(("Summary.aspx?newwin=yes&reloadappid=" & ProcessAppID), hw, False)
        HttpContext.Current.Server.Execute(("SummaryReGeneration.aspx?newwin=yes&reloadappid=" & ProcessAppID & "&UserID=" & UserID & "&ApplicationType=" & CurrentCertApplicationType & "&SelectedUserRole=" & CurrentCertRcl & "&UserName=" & SessionHelper.Name & "&RNorDDFlg=" & CurrentRNorDDFlg.ToString), hw, False)

        Dim urlBase As String = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority

        Dim docSvc As IDocumentService = StructureMap.ObjectFactory.GetInstance(Of IDocumentService)()
        Dim currentUser As IUser = UserAndRoleHelper.CurrentUser()
        Dim pdfBytes As Byte() = docSvc.CreateApplicationPDFDocument(sw.ToString(),
                                                                     urlBase,
                                                                     String.Format("Submitted by {0} ({1}) on {2}", currentUser.UserName, currentUser.UserCode, Date.Now.ToString()))

        'If pdfBytes IsNot Nothing AndAlso pdfBytes.Count() > 0 Then
        '    Dim sumImg As Long = uploadSvc.InsertUploadedDocumentIntoImageStore(pdfBytes)
        '    If sumImg > 0 Then
        '        Dim uploadedDoc As New Business.Model.DocumentUpload()

        '        'Generate model for database information
        '        With uploadedDoc
        '            .ImageSID = sumImg
        '            .DocumentName = Format(DateTime.Now, "MMddmm") + CStr(ProcessAppID) + "_ApplicationSummary.pdf"
        '            .DocumentType = "7"
        '        End With
        '        ' Save file info to database
        '        uploadSvc.InsertUploadedDocument(uploadedDoc, ProcessAppID) 'AppID should be used
        '    End If
        'End If

        ' for testing popup window to view Cert. 
        Response.AddHeader("content-disposition", "attachment;filename= TestSummary" + CStr(CurrentCertSid) + CStr(ProcessAppID) + Today.ToString & ".pdf")
        Response.ContentType = "application/octectstream"
        Response.BinaryWrite(pdfBytes)
        Response.End()
    End Sub
End Class