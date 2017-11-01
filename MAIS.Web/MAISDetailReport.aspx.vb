Imports MAIS.Business.Services
Imports MAIS.Business.Model
Imports MAIS.Business.Model.Enums

Public Class MAISDetailReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.HideLink = True
        Master.HideProgressBar = True
        SessionHelper.Notation_Flg = False
        Master.HideProgressBar = True
        If Not IsPostBack Then
            If (SessionHelper.SessionUniqueID.Contains("RN")) Then
                lblRNLNoOrSSN.Text = "RN Lic. No:"
                lblDtIssuedOrDOB.Text = "Date of Original RN Lic. Issuance:"
            Else
                lblRNLNoOrSSN.Text = "Last 4 SSN:"
                lblDtIssuedOrDOB.Text = "Date of Birth:"
            End If
            Dim personalSvc As IPersonalInformationService = StructureMap.ObjectFactory.GetInstance(Of IPersonalInformationService)()
            Dim ddpersonel As DDPersonnelDetails = Nothing
            Dim rnInfo As RNInformationDetails = Nothing

            Dim searchSvc As ISearchService = StructureMap.ObjectFactory.GetInstance(Of ISearchService)()
            Dim notationFlag As Boolean = searchSvc.GetRRDDAsMoreThanThreeNotation(If(SessionHelper.SessionUniqueID.Contains("RN"), True, False), SessionHelper.SessionUniqueID)
            SessionHelper.Notation_Flg = notationFlag
            If (Not String.IsNullOrWhiteSpace(SessionHelper.SessionUniqueID)) Then
                Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
                If maisSvc.GetExistingFlg(SessionHelper.SessionUniqueID, If(SessionHelper.SessionUniqueID.Contains("RN"), True, False)).ReturnValue Then
                    If (ddpersonel Is Nothing And rnInfo Is Nothing) Then
                        If (If(SessionHelper.SessionUniqueID.Contains("RN"), True, False) = False) Then
                            ddpersonel = personalSvc.GetDDPersonnelInformationFromPermanent(SessionHelper.SessionUniqueID)
                            GetALLSkills()
                            pnlSkills.Visible = True
                        Else
                            rnInfo = personalSvc.GetRNInfoFromPermanent(SessionHelper.SessionUniqueID)
                            pnlSkills.Visible = False
                        End If
                        If ddpersonel IsNot Nothing Or rnInfo IsNot Nothing Then
                            SetAllDefaultValues(ddpersonel, rnInfo)
                        End If
                        GetAllHistory() 'pull certification history 
                        GetALLCourses()
                        GetCEUSRenewal()
                        LoadEmployerList()
                        GetAllNotaions()
                        LoadSupervisorList()
                        'GetEmployerInformation() 'Pull employer information
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub GetALLSkills()
        Dim reportSVC As IMAISReportService = StructureMap.ObjectFactory.GetInstance(Of IMAISReportService)()
        Dim retList As List(Of DDSkills_Info) = reportSVC.GetDDPersonnelSkillsByUniqueID(SessionHelper.SessionUniqueID)
        If (retList.Count > 0) Then
            grdSkills.DataSource = (From ss In retList
                                            Order By ss.Category_Desc, ss.Skill_Desc, ss.Skill_CheckList_Desc
                                            Select New With {                                                   
                                                    .Category_Desc = ss.Category_Desc,
                                                    .Skill_Desc = ss.Skill_Desc,
                                                    .Skill_CheckList_Desc = ss.Skill_CheckList_Desc,
                                                    .Verification_Date = ss.Verification_Date,
                                                    .Verified_Person_Name = ss.Verified_Person_Name,
                                                    .Verified_Person_Title = ss.Verified_Person_Title
                                                }).ToList()
            grdSkills.DataBind()
        Else
            grdSkills.DataSource = Nothing
            grdSkills.DataBind()
        End If
    End Sub
    Private Sub GetCEUSRenewal()
        Dim reportSVC As IMAISReportService = StructureMap.ObjectFactory.GetInstance(Of IMAISReportService)()
        Dim retList As List(Of CEUDetails) = reportSVC.GetDDRNCEUSRenewal(SessionHelper.SessionUniqueID)
        If (retList.Count > 0) Then
            grdCEUSRenewal.DataSource = retList
            grdCEUSRenewal.DataBind()
        Else
            grdCEUSRenewal.DataSource = Nothing
            grdCEUSRenewal.DataBind()
        End If
    End Sub
    Private Sub LoadSupervisorList()
        Dim reportSVC As IMAISReportService = StructureMap.ObjectFactory.GetInstance(Of IMAISReportService)()
        Dim retList As List(Of SupervisorDetails) = reportSVC.GetSupervisorListByUniqueID(SessionHelper.SessionUniqueID)
        If (retList.Count > 0) Then
            grdSupervisorList.DataSource = (From ss In retList
                                            Order By ss.supFirstName
                                            Select New With {
                                                    .supFirstName = ss.supFirstName,
                                                    .supLastName = ss.supLastName,
                                                    .EmailAddress = ss.EmailAddress,
                                                    .supEndDate = ss.supEndDate.ToShortDateString(),
                                                    .supStartDate = ss.supStartDate.ToShortDateString(),
                                                    .PhoneNumber = ss.PhoneNumber
                                                }).ToList()
            grdSupervisorList.DataBind()
        Else
            grdSupervisorList.DataSource = Nothing
            grdSupervisorList.DataBind()
        End If
    End Sub
    Private Sub LoadEmployerList()
        Dim reportSVC As IMAISReportService = StructureMap.ObjectFactory.GetInstance(Of IMAISReportService)()

        Dim retList As List(Of EmployerDetails) = reportSVC.GetEmployersListByUniqueID(SessionHelper.SessionUniqueID)
        If retList.Count > 0 Then
            grdEmployerList.DataSource = (From ee In retList
                                          Order By ee.EmployerName
                                          Select New With {
                                                .CEOFirstName = ee.CEOFirstName,
                                                .CEOLastName = ee.CEOLastName,
                                                .EmailAddress = ee.EmailAddress,
                                                .EmpEndDate = ee.EmpEndDate.ToShortDateString(),
                                                .EmployerName = ee.EmployerName,
                                                .EmpStartDate = ee.EmpStartDate.ToShortDateString(),
                                                .IdentificationValue = ee.IdentificationValue,
                                                .PhoneNumber = ee.PhoneNumber
                                              }).ToList()
            grdEmployerList.DataBind()

        Else
            grdEmployerList.DataSource = Nothing
            grdEmployerList.DataBind()

        End If
    End Sub
    Private Sub GetALLCourses()
        Dim reportSVC As IMAISReportService = StructureMap.ObjectFactory.GetInstance(Of IMAISReportService)()
        Dim retList As List(Of Course_Info) = reportSVC.GetAllCoursesByUniqueID(SessionHelper.SessionUniqueID)
        If retList.Count > 0 Then
            gvCourse.DataSource = (From cou In retList
                                Order By cou.Course_Sid
                                Select New With {
                                    .Trainer_Name = cou.Trainer_Name,
                                    .Course_Number = cou.Course_Number,
                                    .Session_Start_Date = cou.Session_Start_Date.ToShortDateString(),
                                    .Session_End_Date = cou.Session_End_Date.ToShortDateString(),
                                    .Session_CEUs = cou.Session_CEUs
                                    }).ToList()
            gvCourse.DataBind()          
        Else
            gvCourse.DataSource = Nothing
            gvCourse.DataBind()

        End If
    End Sub
    Private Sub GetAllNotaions()
        Dim notSvc As INotationService = StructureMap.ObjectFactory.GetInstance(Of INotationService)()
        Dim ret = notSvc.GetNotations(SessionHelper.SessionUniqueID, If(SessionHelper.SessionUniqueID.Contains("RN"), True, False)).ReturnValue
        If ret.Count > 0 Then
            grdNotations.DataSource = ret
            grdNotations.DataBind()
        Else
            grdNotations.DataSource = Nothing
            grdNotations.DataBind()
        End If       
    End Sub
    Private Sub GetAllHistory()
        Dim maisSvc As IMAISReportService = StructureMap.ObjectFactory.GetInstance(Of IMAISReportService)()
        Dim certHist As List(Of Cert_Info) = maisSvc.GetCertificationHistory(SessionHelper.SessionUniqueID)
        If certHist.Count > 0 Then
            lblErrorMsg.Text = String.Empty
            gvCertification.DataSource = (From cer In certHist
                                             Order By cer.Certification_Sid
                                             Select New With {
                                                 .Certification_Type = cer.Certification_Type,
                                                 .Certification_Status = cer.Certification_Status,
                                                 .Certification_Start_Date = cer.Certification_Start_Date.ToShortDateString(),
                                                 .Certification_End_Date = cer.Certification_End_Date.ToShortDateString(),
                                                 .Attestant_Name = cer.Attestant_Name,
                                                 .RenewalCount = cer.RenewalCount
                                                 }).ToList()
            gvCertification.DataBind()
        Else
            lblErrorMsg.Text = "No certification records found"
            gvCertification.DataSource = Nothing
            gvCertification.DataBind()
        End If

    End Sub
    Protected Sub lnkBack_Click(sender As Object, e As EventArgs) Handles lnkBack.Click
        Response.Redirect("MAIS_Reports.aspx")
    End Sub
    Private Sub SetAllDefaultValues(ByVal ddpersonal As DDPersonnelDetails, ByVal rnInfo As RNInformationDetails)
        If (ddpersonal IsNot Nothing) Then
            lblRNLNoOrSSNtxt.Text = ddpersonal.DODDLast4SSN.Trim()
            lblDtIssuedOrDOBtxt.Text = ddpersonal.DODDDateOfBirth.ToShortDateString().Trim()
            If (lblDtIssuedOrDOBtxt.Text = "12/31/9999") Then
                lblDtIssuedOrDOBtxt.Text = String.Empty
            End If
            lblLastName.Text = ddpersonal.DODDLastName.Trim()
            lblFirstName.Text = ddpersonal.DODDFirstName.Trim()
            lblMI.Text = Trim(ddpersonal.DODDMiddleName)
            lblAddr1.Text = Trim(ddpersonal.DODDHomeAddressLine1)
            If (Not String.IsNullOrWhiteSpace(ddpersonal.DODDHomeAddressLine2)) Then
                lblAddr2.Text = ddpersonal.DODDHomeAddressLine2.Trim()
            Else
                lblAddr2.Text = String.Empty
            End If

            lblCity.Text = Trim(ddpersonal.DODDHomeCity)
            If (Not String.IsNullOrWhiteSpace(ddpersonal.DODDHomeCity)) Then
                lblCounty.Text = ddpersonal.DODDHomeCounty.Trim()
            Else
                lblCounty.Text = String.Empty
            End If
            lblState.Text = Trim(ddpersonal.DODDHomeState)
            lblZip.Text = Trim(ddpersonal.DODDHomeZip)
            Dim zipPlus As String = String.Empty
            If (Not String.IsNullOrWhiteSpace(ddpersonal.DODDHomeZipPlus)) Then
                zipPlus = ddpersonal.DODDHomeZipPlus.Trim()
                lblZip.Text = "-" + zipPlus
            End If

            If Not (String.IsNullOrWhiteSpace(ddpersonal.DODDGender)) Then
                If (ddpersonal.DODDGender = "F") Then
                    rdbGender.SelectedValue = "2"
                ElseIf (ddpersonal.DODDGender = "M") Then
                    rdbGender.SelectedValue = "1"
                End If
            End If

            If ddpersonal.Address.Phone IsNot Nothing Then
                For Each ph As PhoneDetails In ddpersonal.Address.Phone
                    If (ph.ContactType = ContactType.Home) Then
                        lblHomePhoneNumber.Text = ph.PhoneNumber.Trim()
                    ElseIf (ph.ContactType = ContactType.Work) Then
                        lblWorkPhoneNumber.Text = ph.PhoneNumber.Trim()
                    ElseIf (ph.ContactType = ContactType.CellOther) Then
                        lblCellPhoneNumber.Text = ph.PhoneNumber.Trim()
                    End If
                Next
            End If
            If ddpersonal.Address.Email IsNot Nothing Then
                For Each ph As EmailAddressDetails In ddpersonal.Address.Email
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
            If (lblDtIssuedOrDOBtxt.Text = "12/31/9999") Then
                lblDtIssuedOrDOBtxt.Text = String.Empty
            End If
            lblLastName.Text = rnInfo.LastName.Trim()
            lblFirstName.Text = rnInfo.FirstName.Trim()
            lblMI.Text = Trim(rnInfo.MiddleName)
            lblAddr1.Text = Trim(rnInfo.HomeAddressLine1)
            If (Not String.IsNullOrWhiteSpace(rnInfo.HomeAddressLine2)) Then
                lblAddr2.Text = rnInfo.HomeAddressLine2.Trim()
            Else
                lblAddr2.Text = String.Empty
            End If

            lblCity.Text = Trim(rnInfo.HomeCity)
            If ((Not String.IsNullOrWhiteSpace(rnInfo.HomeCounty))) Then
                lblCounty.Text = rnInfo.HomeCounty.Trim()
            Else
                lblCounty.Text = String.Empty
            End If
            lblState.Text = Trim(rnInfo.HomeState)
            Dim zipPlus1 As String = String.Empty

            lblZip.Text = Trim(rnInfo.HomeZip)
            If (Not String.IsNullOrWhiteSpace(rnInfo.HomeZipPlus)) Then
                zipPlus1 = rnInfo.HomeZipPlus.Trim()
                lblZip.Text = "-" + zipPlus1
            End If
            If Not (String.IsNullOrWhiteSpace(rnInfo.Gender)) Then
                If (rnInfo.Gender = "F") Then
                    rdbGender.SelectedValue = "2"
                ElseIf (rnInfo.Gender = "M") Then
                    rdbGender.SelectedValue = "1"
                End If
            End If
            If rnInfo.Address.Phone IsNot Nothing Then
                For Each ph As PhoneDetails In rnInfo.Address.Phone
                    If (ph.ContactType = ContactType.Home) Then
                        lblHomePhoneNumber.Text = ph.PhoneNumber.Trim()
                    ElseIf (ph.ContactType = ContactType.Work) Then
                        lblWorkPhoneNumber.Text = ph.PhoneNumber.Trim()
                    ElseIf (ph.ContactType = ContactType.CellOther) Then
                        lblCellPhoneNumber.Text = ph.PhoneNumber.Trim()
                    End If
                Next
            End If
            If rnInfo.Address.Email IsNot Nothing Then
                For Each ph As EmailAddressDetails In rnInfo.Address.Email
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
    End Sub

    Private Sub grdEmployerList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdEmployerList.PageIndexChanging
        grdEmployerList.PageIndex = e.NewPageIndex
        LoadEmployerList()
    End Sub

    Private Sub grdSupervisorList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdSupervisorList.PageIndexChanging
        grdSupervisorList.PageIndex = e.NewPageIndex
        LoadSupervisorList()
    End Sub
End Class