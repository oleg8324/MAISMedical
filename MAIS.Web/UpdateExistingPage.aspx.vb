Imports System.Web.Script.Services
Imports MAIS.Business
Imports MAIS.Business.Helpers
Imports MAIS.Business.Services
Imports MAIS.Business.Model
Imports MAIS.Business.Model.Enums
Imports MAIS.Data
Imports ODMRDDHelperClassLibrary.Utility
Imports System.IO

Public Class UpdateExistingPage
    Inherits System.Web.UI.Page

    Private Shared SumSvc As ISummaryService
    Public Shared LstRoles As String = String.Empty
    Public Shared conts As List(Of Objects.Contact)
    Public Shared previousRowIndex As Integer = -1
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            pnlAction.Visible = False
            lblEmperror.Text = String.Empty
            lnkApplication.Visible = False
            ' lnkAddon.Enabled = True
            lnkInitial.Enabled = True
            ' lnkRenewal.Enabled = True
            ' Master.HideLink = True
            Master.HideProgressBar = True
            If (SessionHelper.RN_Flg) Then
                lblRNLNoOrSSN.Text = "RN Lic. No:"
                lblDtIssuedOrDOB.Text = "Date of Original RN Lic. Issuance:"
            Else
                lblRNLNoOrSSN.Text = "Last 4 SSN:"
                lblDtIssuedOrDOB.Text = "Date of Birth:"
            End If
            If SessionHelper.ApplicationID > 0 Then
                Dim appInfo As IApplicationDetailInformationService = StructureMap.ObjectFactory.GetInstance(Of IApplicationDetailInformationService)()
                Dim appDetails = appInfo.GetApplicationInfromationByAppID(SessionHelper.ApplicationID)
                If appDetails IsNot Nothing Then
                    Dim LogInUserAllowedToProcess As Boolean
                    If ((UserAndRoleHelper.IsUserAdmin = True) OrElse (UserAndRoleHelper.IsUserReadOnly = True)) Then
                        LogInUserAllowedToProcess = True
                    Else
                        LogInUserAllowedToProcess = PagesHelper.LoginUserAllowedtoProcressApplication(MAIS_Helper.GetUserRoleUsingMAIS(MAIS_Helper.GetUserId).RoleSID, appDetails.RoleCategoryLevel_SID)
                    End If


                    lblAppstatus.InnerText = SessionHelper.ApplicationID.ToString() + " Application Status" + " is "
                    Select Case appDetails.ApplicationStatusType_SID
                        Case 1
                            SessionHelper.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.Pending)
                            lblAppstatus.InnerText = lblAppstatus.InnerText + SessionHelper.ApplicationStatus
                            lblAppstatus.Style.Add("color", "red")
                            If (Not LogInUserAllowedToProcess) Then
                                If ((SessionHelper.LoginUsersRNLicense = SessionHelper.SessionUniqueID) And
                                    ((SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Enums.ApplicationType.Renewal)) OrElse (SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Enums.ApplicationType.UpdateProfile)))) Then
                                    LogInUserAllowedToProcess = True
                                End If
                            End If

                            lnkApplication.Visible = LogInUserAllowedToProcess 'True
                            lnkAddon.Enabled = False
                            lnkInitial.Enabled = False
                            lnkRenewal.Enabled = False
                        Case 2
                            SessionHelper.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.DNMR)
                            lblAppstatus.InnerText = lblAppstatus.InnerText + SessionHelper.ApplicationStatus
                            lblAppstatus.Style.Add("color", "black")
                        Case 3
                            SessionHelper.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.MeetsRequirements)
                            lblAppstatus.InnerText = lblAppstatus.InnerText + SessionHelper.ApplicationStatus
                            lblAppstatus.Style.Add("color", "black")
                        Case 4
                            SessionHelper.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.AddedToRegistry)
                            lblAppstatus.InnerText = lblAppstatus.InnerText + SessionHelper.ApplicationStatus
                            lblAppstatus.Style.Add("color", "black")
                        Case 5
                            SessionHelper.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.RemovedFromRegistry)
                            lblAppstatus.InnerText = lblAppstatus.InnerText + SessionHelper.ApplicationStatus
                            lblAppstatus.Style.Add("color", "black")
                        Case 6
                            SessionHelper.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.DODD_Review)
                            lblAppstatus.InnerText = lblAppstatus.InnerText + SessionHelper.ApplicationStatus
                            lblAppstatus.Style.Add("color", "red")
                            lnkApplication.Visible = LogInUserAllowedToProcess 'True
                            lnkAddon.Enabled = False
                            lnkInitial.Enabled = False
                            lnkRenewal.Enabled = False
                        Case 7
                            SessionHelper.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.VoidedApplication)
                            lblAppstatus.InnerText = lblAppstatus.InnerText + SessionHelper.ApplicationStatus
                            lblAppstatus.Style.Add("color", "black")
                        Case 10
                            SessionHelper.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.Certified)
                            lblAppstatus.InnerText = lblAppstatus.InnerText + SessionHelper.ApplicationStatus
                            lblAppstatus.Style.Add("color", "black")
                        Case 11
                            SessionHelper.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.Denied)
                            lblAppstatus.InnerText = lblAppstatus.InnerText + SessionHelper.ApplicationStatus
                            lblAppstatus.Style.Add("color", "red")
                        Case 12
                            SessionHelper.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.IntentToDeny)
                            lblAppstatus.InnerText = lblAppstatus.InnerText + SessionHelper.ApplicationStatus
                            lblAppstatus.Style.Add("color", "red")
                            lnkApplication.Visible = LogInUserAllowedToProcess 'True
                            lnkAddon.Enabled = False
                            lnkInitial.Enabled = False
                            lnkRenewal.Enabled = False
                    End Select

                    lblAppstatus.InnerText = lblAppstatus.InnerText + ", Application type is " + SessionHelper.ApplicationType

                End If
            End If

            Dim personalSvc As IPersonalInformationService = StructureMap.ObjectFactory.GetInstance(Of IPersonalInformationService)()
            Dim ddpersonel As DDPersonnelDetails = Nothing
            Dim rnInfo As RNInformationDetails = Nothing

            Dim searchSvc As ISearchService = StructureMap.ObjectFactory.GetInstance(Of ISearchService)()
            Dim notationFlag As Boolean = searchSvc.GetRRDDAsMoreThanThreeNotation(SessionHelper.RN_Flg, SessionHelper.SessionUniqueID)
            SessionHelper.Notation_Flg = notationFlag

            If (Not String.IsNullOrWhiteSpace(SessionHelper.SessionUniqueID)) Then
                Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
                If maisSvc.GetExistingFlg(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg).ReturnValue Then
                    If (ddpersonel Is Nothing And rnInfo Is Nothing) Then
                        If (SessionHelper.RN_Flg = False) Then
                            ddpersonel = personalSvc.GetDDPersonnelInformationFromPermanent(SessionHelper.SessionUniqueID)
                        Else
                            rnInfo = personalSvc.GetRNInfoFromPermanent(SessionHelper.SessionUniqueID)
                        End If
                        If ddpersonel IsNot Nothing Or rnInfo IsNot Nothing Then
                            SetAllDefaultValues(ddpersonel, rnInfo)
                        End If
                        GetCertificationHistory() 'pull certification history                       
                        GetEmployerInformation() 'Pull employer information
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub SetOnlyPersonnalInformation(personnalInfo As Model.PersonalInformationDetails)
        If personnalInfo IsNot Nothing Then
            lblRNLNoOrSSNtxt.Text = personnalInfo.RNLicenseOrSSN.Trim()
            lblDtIssuedOrDOBtxt.Text = personnalInfo.DOBDateOfIssuance.ToShortDateString().Trim()
            If (lblDtIssuedOrDOBtxt.Text = "12/31/9999") Then
                lblDtIssuedOrDOBtxt.Text = String.Empty
            End If
            If String.IsNullOrWhiteSpace(personnalInfo.Gender) Then
                If (personnalInfo.Gender = "F") Then
                    rdbGender.SelectedValue = "2"
                ElseIf (personnalInfo.Gender = "M") Then
                    rdbGender.SelectedValue = "1"
                End If
            End If
            lblLastName.Text = personnalInfo.LastName.Trim()
            lblFirstName.Text = personnalInfo.FirstName.Trim()
            lblMI.Text = personnalInfo.MiddleName
        End If
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

    Protected Sub lnkApplication_Click(sender As Object, e As EventArgs) Handles lnkApplication.Click
        Response.Redirect("StartPage.aspx")
    End Sub

    Protected Sub lnkInitial_Click(sender As Object, e As EventArgs) Handles lnkInitial.Click
        SessionHelper.ApplicationID = 0
        SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.Initial)
        SessionHelper.ApplicationStatus = String.Empty
        Response.Redirect("StartPage.aspx")
    End Sub

    Protected Sub lnkAddon_Click(sender As Object, e As EventArgs) Handles lnkAddon.Click
        SessionHelper.ApplicationID = 0
        SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.AddOn)
        SessionHelper.ApplicationStatus = String.Empty
        Response.Redirect("StartPage.aspx?Roles=" + LstRoles)
    End Sub

    Protected Sub lnkRenewal_Click(sender As Object, e As EventArgs) Handles lnkRenewal.Click
        SessionHelper.ApplicationID = 0
        SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.Renewal)
        SessionHelper.ApplicationStatus = String.Empty
        Response.Redirect("StartPage.aspx")
    End Sub

    Protected Sub lnkAddSkills_Click(sender As Object, e As EventArgs) Handles lnkAddSkills.Click
        SessionHelper.ApplicationID = 0
        SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.UpdateProfile)
        SessionHelper.ApplicationStatus = String.Empty
        Response.Redirect("Skills.aspx")

    End Sub

    Protected Sub lnkAddUpdateCeus_Click(sender As Object, e As EventArgs) Handles lnkAddUpdateCeus.Click
        SessionHelper.ApplicationID = 0
        SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.UpdateProfile)
        SessionHelper.ApplicationStatus = String.Empty
        Response.Redirect("TrainingSkills.aspx")
    End Sub

    Protected Sub lnkUpdate_Click(sender As Object, e As EventArgs) Handles lnkUpdate.Click
        SessionHelper.ApplicationID = 0
        SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.UpdateProfile)
        SessionHelper.ApplicationStatus = String.Empty
        Response.Redirect("StartPage.aspx")
    End Sub
    Protected Sub lnkContactInfo_Click(sender As Object, e As EventArgs) Handles lnkContactInfo.Click
        SessionHelper.ApplicationID = 0
        Dim contactInfo As String = "Contact"
        SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.UpdateProfile)
        SessionHelper.ApplicationStatus = String.Empty
        Response.Redirect("PersonalInformation.aspx?App=" + contactInfo)
    End Sub

    Protected Sub lnkViewPrintDocument_Click(sender As Object, e As EventArgs) Handles lnkViewPrintDocument.Click
        SessionHelper.ApplicationID = 0
        Dim contactInfo As String = "Contact"
        SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.UpdateProfile)
        SessionHelper.ApplicationStatus = String.Empty
        Response.Redirect("ViewPrintDocments.aspx?ReturnPage=" + Master.CurrentPage)
    End Sub
    Protected Sub lnkCertAdmin_Click(sender As Object, e As EventArgs) Handles lnkCertAdmin.Click
        SessionHelper.ApplicationStatus = String.Empty
        SessionHelper.ApplicationType = String.Empty
        Response.Redirect("certadministration.aspx")
    End Sub
    Protected Sub lnkCertRegerenation_Click(Sender As Object, e As EventArgs) Handles lnkCertRegerenation.Click

        SessionHelper.ApplicationStatus = String.Empty
        SessionHelper.ApplicationType = String.Empty
        Response.Redirect("SummaryReGenerationSearch.aspx")
    End Sub
    Private Sub GetEmployerInformation()
        lblEmperror.Text = String.Empty
        If (Not String.IsNullOrWhiteSpace(SessionHelper.SessionUniqueID)) Then
            Dim empSrv As IEmployerInformationService = StructureMap.ObjectFactory.GetInstance(Of IEmployerInformationService)()
            Dim retemp As List(Of Model.EmployerInformationDetails) = empSrv.GetEmployerInformationFromPerm(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg)
            If retemp.Count > 0 Then
                gvEmpInformation.DataSource = (From p In retemp
                                                Order By p.EmployerType
                                                Select New With
                                                       {
                                                          .EmployerType = p.EmployerType,
                                                          .EmployerName = p.EmployerName,
                                                          .CEOFirstName = p.CEOFirstName.Trim() + " " + p.CEOLastName.Trim(),
                                                          .SupervisorFirstName = p.SupervisorFirstName.Trim() + " " + p.SupervisorLastName.Trim()
                                                           }).ToList()
                gvEmpInformation.DataBind()
            Else
                lblEmperror.Text = "No Employer Information."
            End If
        End If
    End Sub
    Private Sub GetCertificationHistory()
        lblCerterr.Text = String.Empty
        LstRoles = String.Empty
        If (Not String.IsNullOrWhiteSpace(SessionHelper.SessionUniqueID)) Then
            Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
            Dim retcertHis As List(Of Model.Certificate) = maisSvc.GetCertificationHistory(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg)
            If retcertHis.Count > 0 Then
                If retcertHis.Count > 1 Then
                    Dim Lst_rcl = (From r In retcertHis
                                    Order By r.RolePriority
                                    Where r.EndDate >= Today
                                    Select r.Role_Category_Level_Sid Distinct).ToList()
                    For Each rcl In Lst_rcl 'sending this as query string to start page JH
                        LstRoles = LstRoles + rcl.ToString() + ","
                    Next

                End If
                Dim existRole As Integer = 0
                Dim existingRole = (From lst In retcertHis
                                        Order By lst.RolePriority
                                        Select lst).ToList()
                For Each r In existingRole
                    If (r.EndDate >= Today) Then
                        existRole = r.Role_Category_Level_Sid
                        Exit For
                    End If
                Next
                If ((existRole = 0) And (existingRole.Count > 0)) Then
                    For Each rr In existingRole
                        If (rr.EndDate < Today) Then
                            existRole = rr.Role_Category_Level_Sid
                            Exit For
                        End If
                    Next
                End If
                SessionHelper.ExistingUserRole = existRole



                gvCertHistory.DataSource = (From r In retcertHis
                                            Order By r.RolePriority, r.EndDate Descending
                                            Select New With
                                                   {
                                                    .Category = r.Category,
                                                    .Level = r.Level,
                                                    .Role = r.Role,
                                                    .StartDate = r.StartDate.ToShortDateString(),
                                                    .EndDate = If(r.EndDate.ToShortDateString() = "12/31/9999", String.Empty, r.EndDate.ToShortDateString()),
                                                    .Role_Category_Level_Sid = r.Role_Category_Level_Sid,
                                                    .hdRoleLevelCategory = r.Role_Category_Level_Sid,
                                                    .Status = r.Status,
                                                    .Role_RN_DD_Personnel_Xref_Sid = r.Role_RN_DD_Personnel_Xref_Sid
                                                }).ToList()
                gvCertHistory.DataBind()
            Else
                lblCerterr.Text = "No Certification Information."
            End If
        End If
    End Sub
    Private Sub CertLinksEnablingByRole(ByVal certEndDate As DateTime, ByVal RLC_Sid As Integer)
        If UserAndRoleHelper.IsUserAdmin OrElse SessionHelper.MAISLevelUserRole = Enums.RoleLevelCategory.RNMaster_RLC Then
            lnkCertAdmin.Enabled = True
            pnlCertAdminPage.Visible = True
            If (SessionHelper.RN_Flg) Then
                lnkRnlicensechange.Enabled = True
                lblrnExistingNumber.InnerText = SessionHelper.SessionUniqueID
            Else
                lnkRnlicensechange.Enabled = False
            End If
        Else
            lnkCertAdmin.Enabled = False
            pnlCertAdminPage.Visible = False
        End If
   
        Dim lnkInitCross(,) As Short = {{1, 1, 1, 1, 1, 1}, {0, 0, 1, 1, 1, 1}, {0, 0, 0, 0, 1, 1}, {0, 0, 0, 0, 1, 0}, {0, 0, 0, 0, 1, 0}, {0, 0, 0, 0, 0, 0}, {1, 1, 1, 1, 1, 1}, {1, 1, 1, 1, 1, 1}}
        If lnkInitCross((rclToIndex(SessionHelper.MAISLevelUserRole, True)), (rclToIndex(RLC_Sid, False))) = 0 OrElse (lnkInitCross((rclToIndex(SessionHelper.MAISLevelUserRole, True)), (rclToIndex(RLC_Sid, False))) = 2 And (SessionHelper.LoginUsersRNLicense <> SessionHelper.SessionUniqueID)) Then
            lnkInitial.Enabled = False
        End If
        Dim lnkAddCross(,) As Short = {{1, 1, 1, 1, 1, 1}, {0, 2, 1, 1, 1, 1}, {0, 0, 2, 0, 1, 1}, {0, 0, 0, 0, 2, 0}, {0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0}, {1, 1, 1, 1, 1, 1}, {1, 1, 1, 1, 1, 1}}
        If lnkAddCross((rclToIndex(SessionHelper.MAISLevelUserRole, True)), (rclToIndex(RLC_Sid, False))) = 0 OrElse (lnkAddCross((rclToIndex(SessionHelper.MAISLevelUserRole, True)), (rclToIndex(RLC_Sid, False))) = 2 And (SessionHelper.LoginUsersRNLicense <> SessionHelper.SessionUniqueID)) Then
            lnkAddon.Enabled = False
        End If
        Dim lnkRenewCross(,) As Short = {{1, 1, 1, 1, 1, 1}, {0, 2, 0, 0, 0, 1}, {0, 0, 2, 0, 0, 1}, {0, 0, 0, 2, 0, 0}, {0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0}, {1, 1, 1, 1, 1, 1}, {1, 1, 1, 1, 1, 1}}
        If lnkRenewCross((rclToIndex(SessionHelper.MAISLevelUserRole, True)), (rclToIndex(RLC_Sid, False))) = 0 OrElse (lnkRenewCross((rclToIndex(SessionHelper.MAISLevelUserRole, True)), (rclToIndex(RLC_Sid, False))) = 2 And (SessionHelper.LoginUsersRNLicense <> SessionHelper.SessionUniqueID)) Then
            lnkRenewal.Enabled = False
        End If
        Dim lnkUpdateProfileCross(,) As Short = {{1, 1, 1, 1, 1, 1}, {0, 2, 1, 1, 1, 1}, {0, 0, 2, 0, 1, 1}, {0, 0, 0, 2, 1, 0}, {0, 0, 0, 0, 1, 0}, {0, 0, 0, 0, 0, 0}, {1, 1, 1, 1, 1, 1}, {1, 1, 1, 1, 1, 1}}
        If lnkUpdateProfileCross((rclToIndex(SessionHelper.MAISLevelUserRole, True)), (rclToIndex(RLC_Sid, False))) = 0 OrElse (lnkUpdateProfileCross((rclToIndex(SessionHelper.MAISLevelUserRole, True)), (rclToIndex(RLC_Sid, False))) = 2 And (SessionHelper.LoginUsersRNLicense <> SessionHelper.SessionUniqueID)) Then
            lnkUpdate.Enabled = False
        End If
        Dim lnkAddCECross(,) As Short = {{1, 1, 1, 1, 1, 1}, {0, 2, 0, 0, 0, 1}, {0, 0, 2, 0, 0, 1}, {0, 0, 0, 2, 0, 0}, {0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0}, {1, 1, 1, 1, 1, 1}, {1, 1, 1, 1, 1, 1}}
        If lnkAddCECross((rclToIndex(SessionHelper.MAISLevelUserRole, True)), (rclToIndex(RLC_Sid, False))) = 0 OrElse (lnkAddCECross((rclToIndex(SessionHelper.MAISLevelUserRole, True)), (rclToIndex(RLC_Sid, False))) = 2 And (SessionHelper.LoginUsersRNLicense <> SessionHelper.SessionUniqueID)) Then
            lnkAddUpdateCeus.Enabled = False
        End If
        Dim lnkAddSkillsCross(,) As Short = {{0, 0, 0, 0, 0, 1}, {0, 0, 0, 0, 0, 1}, {0, 0, 0, 0, 0, 1}, {0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0}, {1, 1, 1, 1, 1, 1}, {0, 0, 0, 0, 0, 1}}
        If lnkAddSkillsCross((rclToIndex(SessionHelper.MAISLevelUserRole, True)), (rclToIndex(RLC_Sid, False))) = 0 OrElse (lnkAddSkillsCross((rclToIndex(SessionHelper.MAISLevelUserRole, True)), (rclToIndex(RLC_Sid, False))) = 2 And (SessionHelper.LoginUsersRNLicense <> SessionHelper.SessionUniqueID)) Then
            lnkAddSkills.Enabled = False
        End If
        Dim lnkUpdateContsCross(,) As Short = {{1, 1, 1, 1, 1, 1}, {0, 2, 1, 1, 1, 1}, {0, 0, 2, 0, 1, 1}, {0, 0, 0, 2, 1, 0}, {0, 0, 0, 0, 1, 0}, {0, 0, 0, 0, 0, 0}, {1, 1, 1, 1, 1, 1}, {1, 1, 1, 1, 1, 1}}
        If lnkUpdateContsCross((rclToIndex(SessionHelper.MAISLevelUserRole, True)), (rclToIndex(RLC_Sid, False))) = 0 OrElse (lnkUpdateContsCross((rclToIndex(SessionHelper.MAISLevelUserRole, True)), (rclToIndex(RLC_Sid, False))) = 2 And (SessionHelper.LoginUsersRNLicense <> SessionHelper.SessionUniqueID)) Then
            lnkContactInfo.Enabled = False
        End If
        Dim lnkUpdateNotationsCross(,) As Short = {{1, 1, 1, 1, 1, 1}, {0, 0, 1, 1, 0, 1}, {0, 0, 0, 0, 0, 1}, {0, 0, 0, 0, 0, 1}, {0, 0, 0, 0, 0, 1}, {0, 0, 0, 0, 0, 0}, {1, 1, 1, 1, 1, 1}, {1, 1, 1, 1, 1, 1}}
        If lnkUpdateNotationsCross((rclToIndex(SessionHelper.MAISLevelUserRole, True)), (rclToIndex(RLC_Sid, False))) = 0 OrElse (lnkUpdateNotationsCross((rclToIndex(SessionHelper.MAISLevelUserRole, True)), (rclToIndex(RLC_Sid, False))) = 2 And (SessionHelper.LoginUsersRNLicense <> SessionHelper.SessionUniqueID)) Then
            lnkNotation.Enabled = False
        End If
    End Sub
    Private Sub CertificationDateAndLinksEnabling(ByVal certEndDate As DateTime, ByVal RLC_Sid As Integer)
        '120 days for DDpersonnal and 30 days for RN's to renew there certification after expiry
        If (Date.Parse(certEndDate) >= Today) Then
            lnkInitial.Enabled = False
            lnkAddon.Enabled = True
            lnkAddUpdateCeus.Enabled = True
            lnkUpdate.Enabled = True
            If (Today > Date.Parse(certEndDate).AddDays(-180)) Then
                lnkRenewal.Enabled = True
            Else
                lnkRenewal.Enabled = False
            End If
            If (SessionHelper.RN_Flg) Then
                lnkAddSkills.Enabled = False
            Else
                If (Today > Date.Parse(certEndDate).AddDays(-180)) Then
                    lnkAddSkills.Enabled = True
                Else
                    lnkAddSkills.Enabled = False
                End If
            End If
        ElseIf ((Date.Parse(certEndDate) < Today) And (Date.Parse(certEndDate.AddDays(180)) >= Today) And ((UserAndRoleHelper.IsUserAdmin) Or (SessionHelper.MAISLevelUserRole = Enums.RoleLevelCategory.RNMaster_RLC)) And (SessionHelper.RN_Flg)) Then
            lnkInitial.Enabled = True
            lnkAddon.Enabled = False
            lnkRenewal.Enabled = True
            lnkAddUpdateCeus.Enabled = False
            lnkUpdate.Enabled = False
            lnkAddSkills.Enabled = False
        ElseIf ((Date.Parse(certEndDate) < Today) And (Date.Parse(certEndDate.AddDays(90)) >= Today) And Not (SessionHelper.RN_Flg)) Then
            ' lnkInitial.Enabled = False
            lnkAddon.Enabled = False
            ' lnkRenewal.Enabled = True
            lnkAddUpdateCeus.Enabled = False
            lnkUpdate.Enabled = False
            If (Date.Parse(certEndDate.AddDays(60)) >= Today) Then
                lnkAddSkills.Enabled = True
                lnkInitial.Enabled = False
                lnkRenewal.Enabled = True
            Else
                lnkAddSkills.Enabled = False
                lnkInitial.Enabled = True
                lnkRenewal.Enabled = True
            End If
        ElseIf ((Date.Parse(certEndDate) < Today) And (Date.Parse(certEndDate.AddDays(180)) >= Today) And ((UserAndRoleHelper.IsUserAdmin) Or (SessionHelper.MAISLevelUserRole = Enums.RoleLevelCategory.RNMaster_RLC)) And Not (SessionHelper.RN_Flg)) Then
            lnkInitial.Enabled = True
            lnkAddon.Enabled = False
            lnkRenewal.Enabled = True
            lnkAddUpdateCeus.Enabled = False
            lnkUpdate.Enabled = False
        Else
            lnkInitial.Enabled = True
            lnkAddon.Enabled = False
            lnkRenewal.Enabled = False
            lnkAddUpdateCeus.Enabled = False
            lnkUpdate.Enabled = False
            lnkAddSkills.Enabled = False
        End If
        If ((Date.Parse(certEndDate) <= Today) And (RLC_Sid = Enums.RoleLevelCategory.QARN_RLC)) Then
            If (SessionHelper.ExistingUserRole <> Enums.RoleLevelCategory.QARN_RLC) Then
                lnkInitial.Enabled = False
                lnkAddon.Enabled = True
            ElseIf (SessionHelper.ExistingUserRole = Enums.RoleLevelCategory.QARN_RLC) Then
                lnkInitial.Enabled = True
                lnkAddon.Enabled = False
            End If
            'lnkRenewal.Enabled = False
            lnkAddUpdateCeus.Enabled = False
            lnkUpdate.Enabled = False
            lnkAddSkills.Enabled = False
        End If
        If (RLC_Sid = Enums.RoleLevelCategory.QARN_RLC) Then
            lnkAddSkills.Enabled = False
            lnkAddUpdateCeus.Enabled = False
        End If
        If ((SessionHelper.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.Pending)) OrElse
            (SessionHelper.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.DODD_Review))) Then
            lnkInitial.Enabled = False
            lnkAddon.Enabled = False
            lnkRenewal.Enabled = False
        End If
    End Sub

    Private Sub GetEmailNotificationForUnRegister()
        Dim strToEmailAddress As String = Nothing
        If Not String.IsNullOrEmpty(ConfigHelper.ToEmailAddress) Then
            strToEmailAddress = ConfigHelper.ToEmailAddress
        Else
            If (Not String.IsNullOrEmpty(lblHomeAddress.Text)) Then
                strToEmailAddress = lblHomeAddress.Text
            Else
                If (Not String.IsNullOrEmpty(lblWorkAddress.Text)) Then
                    strToEmailAddress = lblWorkAddress.Text
                Else
                    strToEmailAddress = lblCellAddress.Text
                End If
            End If
        End If
        Dim emailSvc As IEmailService = StructureMap.ObjectFactory.GetInstance(Of IEmailService)()
        Dim strBodyMessage As String = "You have been unregistered as a DODD QA RN.<br/><br/>"
        strBodyMessage = strBodyMessage + "Thank you!"
        Dim retObj As ReturnObject(Of Boolean) = emailSvc.SendEmail(strToEmailAddress.Trim(),
                                                                    ConfigHelper.FromEmailAddress,
                                                                    ConfigHelper.EmailSubjectStatus,
                                                                    strBodyMessage, Nothing, String.Empty, String.Empty)
        If retObj.ReturnValue Then
            'lblCerterr.Text = "An email containing your status of this application was sent successfully."
        Else
            If retObj.Messages.Count > 0 Then
                lblCerterr.Text = retObj.MessageStrings.First()
            Else
                lblCerterr.Text = "ERROR: An error has occurred while trying to send an email for QA unregisteration."
            End If
        End If
    End Sub

    Private Sub gvCertHistory_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvCertHistory.RowCommand
        Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Dim gvRow As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)



        If (e.CommandName = "QAUnRegister") Then
            Dim Role_Person_Id As Integer = Convert.ToInt32(e.CommandArgument)

            '  Dim RLC_Sid = gvCertHistory.Rows(gvRow.RowIndex).Cells(6).Text.Trim()
            Dim RLC_Sid As Integer = DirectCast(gvRow.Cells(6).FindControl("hdRoleLevelCategory"), HiddenField).Value
            If ((RLC_Sid > 0) And (Role_Person_Id > 0)) Then

                Dim retObj = maisSvc.UnRegisterQA(RLC_Sid, Role_Person_Id)
                If retObj Then
                    lblCerterr.Text = String.Empty
                    GetEmailNotificationForUnRegister()
                    GetCertificationHistory()
                Else
                    lblCerterr.Text = "Error in Un Registering QA"
                End If
            End If
        End If
        If (e.CommandName = "DesiredOptions") Then
            pnlAction.Visible = True
            Dim str As String = e.CommandArgument
            Dim certEndDate As Date = Convert.ToDateTime(If(String.IsNullOrWhiteSpace(str), "12/31/9999", str))

            If gvCertHistory.Rows.Count > 1 And (Not (previousRowIndex < 0)) Then 'If Not (previousRowIndex < 0) Then
                gvCertHistory.Rows(previousRowIndex).BackColor = Drawing.Color.White
            End If


            gvCertHistory.Rows(gvRow.RowIndex).BackColor = Drawing.Color.PowderBlue
            'gvRow.BackColor = System.Drawing.Color.PowderBlue
            previousRowIndex = gvRow.RowIndex
            Dim roleOne As Integer = DirectCast(gvRow.Cells(6).FindControl("hdRoleLevelCategory"), HiddenField).Value
            If ((SessionHelper.RN_Flg = False) And (roleOne > 0)) Then
                SessionHelper.ExistingUserRole = roleOne
                SessionHelper.SelectedUserRole = roleOne
                If ((SessionHelper.ExistingUserRole = Enums.RoleLevelCategory.DDPersonnel2_RLC) OrElse (SessionHelper.ExistingUserRole = Enums.RoleLevelCategory.DDPersonnel2_RLC)) Then
                    lnkAddon.Visible = False
                    lblAddOn.Visible = False
                Else
                    lnkAddon.Visible = True
                    lblAddOn.Visible = True
                End If
            ElseIf ((SessionHelper.RN_Flg = True) And (roleOne > 0)) Then
                SessionHelper.SelectedUserRole = roleOne
            End If
            CertificationDateAndLinksEnabling(certEndDate, roleOne)
            CertLinksEnablingByRole(certEndDate, roleOne)
            If ((Not String.IsNullOrWhiteSpace(SessionHelper.SessionUniqueID)) And roleOne > 0) Then
                Dim strMsg As String = maisSvc.CheckPreviousRenewal(SessionHelper.SessionUniqueID, roleOne)
                lblMsgRenewal.Text = String.Empty
                If Not (String.IsNullOrWhiteSpace(strMsg)) Then
                    lnkRenewal.Enabled = False
                    If (Date.Parse(certEndDate.AddDays(-180)) <= Today) Then
                        lblMsgRenewal.Text = strMsg
                    End If

                End If
                Dim retFlg As Boolean = maisSvc.CheckRenewalDone(SessionHelper.SessionUniqueID, roleOne, SessionHelper.RN_Flg)
                If retFlg Then
                    lnkRenewal.Enabled = False
                End If
            End If
            Dim strStatus As String = gvRow.Cells(3).Text
            If ((strStatus = "Intent to Revoke") OrElse (strStatus = "Revoked") OrElse (strStatus = "Suspended") OrElse (strStatus = "Voluntary Withdrawal") OrElse (strStatus = "Did Not Meet Requirements")) Then
                If ((UserAndRoleHelper.IsUserAdmin) OrElse (SessionHelper.MAISLevelUserRole = Enums.RoleLevelCategory.RNMaster_RLC)) Then
                    lnkInitial.Enabled = True
                    lnkAddon.Enabled = False
                    lnkRenewal.Enabled = False
                Else
                    lnkInitial.Enabled = False
                    lnkAddon.Enabled = False
                    lnkRenewal.Enabled = False
                End If

                If (strStatus = "Did Not Meet Requirements") Then
                    lnkInitial.Enabled = True
                    lnkAddon.Enabled = False
                    lnkRenewal.Enabled = False
                End If
                'Else
                '    pnlAction.Visible = False
            End If
        End If
    End Sub

    Protected Sub lnkNotation_Click(sender As Object, e As EventArgs) Handles lnkNotation.Click
        Response.Redirect("Notation.aspx?App=" + "Notation")
    End Sub

    Private Sub gvCertHistory_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvCertHistory.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            'Enable QA RN Link to unregister themselves
            Dim EndDate As Date
            Dim StartDate As Date


            If (e.Row.Cells(5).Text = "&nbsp;") Then
                EndDate = "12/31/9999"
            Else
                EndDate = e.Row.Cells(5).Text
            End If
            StartDate = e.Row.Cells(4).Text
            Dim lnkQa As LinkButton = CType(e.Row.FindControl("lnkQAUnregister"), LinkButton)
            Dim RLC_Sid As Integer = CType(e.Row.FindControl("hdRoleLevelCategory"), HiddenField).Value
            If ((RLC_Sid = Enums.RoleLevelCategory.QARN_RLC) And (EndDate > Today) And (StartDate < Today)) Then
                lnkQa.Visible = True
            Else
                lnkQa.Visible = False
            End If
            'visible or not desired actions link
            Dim lnkAct As LinkButton = CType(e.Row.FindControl("lnkAction"), LinkButton)
            If ((Date.Parse(EndDate) >= Today) And (Date.Parse(StartDate) <= Today)) Then
                ' lnkAct.Enabled = True
                If ((SessionHelper.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.Pending)) OrElse
                    (SessionHelper.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.IntentToDeny)) OrElse
                               (SessionHelper.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.DODD_Review))) Then
                    lnkAct.Enabled = False
                    lnkQa.Enabled = False
                Else
                    lnkAct.Enabled = True
                    lnkQa.Enabled = True
                End If
            ElseIf (Date.Parse(EndDate) < Today) Then
                If ((SessionHelper.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.Pending)) OrElse
                    (SessionHelper.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.IntentToDeny)) OrElse
                              (SessionHelper.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.DODD_Review))) Then
                    lnkAct.Enabled = False
                    lnkQa.Enabled = False
                Else
                    lnkAct.Enabled = True
                    lnkQa.Enabled = True
                End If
            Else
                lnkAct.Enabled = False
                lnkQa.Enabled = False
            End If
            If UserAndRoleHelper.IsUserReadOnly Then
                lnkAct.Enabled = False
                lnkQa.Enabled = False
            End If
            Dim lnkActCross(,) As Short = {{1, 1, 1, 1, 1, 1}, {0, 2, 1, 1, 1, 1}, {0, 0, 2, 0, 1, 1}, {0, 0, 0, 2, 1, 1}, {0, 0, 0, 0, 2, 1}, {0, 0, 0, 0, 0, 0}, {1, 1, 1, 1, 1, 1}, {1, 1, 1, 1, 1, 1}}
            If lnkActCross((rclToIndex(SessionHelper.MAISLevelUserRole, True)), (rclToIndex(RLC_Sid, False))) = 0 OrElse (lnkActCross((rclToIndex(SessionHelper.MAISLevelUserRole, True)), (rclToIndex(RLC_Sid, False))) = 2 And (SessionHelper.LoginUsersRNLicense <> SessionHelper.SessionUniqueID)) Then
                lnkAct.Enabled = False
            End If

            'If (SessionHelper.MAISLevelUserRole = Enums.RoleLevelCategory.RNInstructor_RLC And (SessionHelper.ExistingUserRole = Enums.RoleLevelCategory.RNMaster_RLC Or (SessionHelper.ExistingUserRole = Enums.RoleLevelCategory.RNInstructor_RLC And SessionHelper.LoginUsersRNLicense <> SessionHelper.SessionUniqueID))) Then
            '    lnkAct.Enabled = False
            'End If
            'If SessionHelper.LoginUsersRNLicense = SessionHelper.SessionUniqueID Then

        End If
    End Sub
    Protected Function rclToIndex(ByVal rcl As Integer, ByVal forLoggedInPerson As Boolean) As Integer
        Dim retval As Integer = 0
        If forLoggedInPerson Then
            If UserAndRoleHelper.IsUserSecretary Then
                Return 6
            ElseIf UserAndRoleHelper.IsUserAdmin Then
                Return 7
            End If
        End If
        Select Case rcl
            Case 0
                retval = 5
            Case 6
                retval = 0 'master instr
            Case 5
                retval = 1 'instructor
            Case 4
                retval = 2 'trainer
            Case 8
                retval = 3 '17+
            Case 7
                retval = 4 'qa rn
            Case 15, 16, 17
                retval = 5 'ddpers
        End Select
        Return retval
    End Function
    Protected Sub lnkBack_Click(sender As Object, e As EventArgs) Handles lnkBack.Click
        Response.Redirect("Search.aspx")
    End Sub



    Protected Sub lnkRnlicensechange_Click(sender As Object, e As EventArgs) Handles lnkRnlicensechange.Click
        SessionHelper.ApplicationStatus = String.Empty
        SessionHelper.ApplicationType = String.Empty
        If (SessionHelper.RN_Flg) Then
            pnlRNChange.Visible = True
        End If
    End Sub

    Protected Sub btnSaveRN_Click(sender As Object, e As EventArgs) Handles btnSaveRN.Click
        If (Not String.IsNullOrWhiteSpace(txtRnLicenseNumber.Text)) Then
            lblRNChangeErrorMsg.InnerText = String.Empty
            Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
            Dim retObj As String = String.Empty
            retObj = maisSvc.ChangeRNLicenseNumber(Trim(txtRnLicenseNumber.Text), SessionHelper.SessionUniqueID)
            If String.IsNullOrWhiteSpace(retObj) Then
                lblRNChangeErrorMsg.InnerText = "Saved successfully"
                SessionHelper.SessionUniqueID = Trim(txtRnLicenseNumber.Text)
                lblRNChangeErrorMsg.Attributes.Add("style", "color:green")
                Response.Redirect("UpdateExistingPage.aspx")
            Else
                lblRNChangeErrorMsg.InnerText = "Error updating RN License Number, " + retObj
            End If
        Else
            lblRNChangeErrorMsg.InnerText = "Please enter new RN license number"
        End If
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        pnlRNChange.Visible = False
        txtRnLicenseNumber.Text = String.Empty
    End Sub
End Class