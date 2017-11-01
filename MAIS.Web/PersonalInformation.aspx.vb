Imports System.Web.Script.Services
Imports MAIS.Business.Model
Imports MAIS.Business.Services
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business
Imports MAIS.Business.Model.Enums
Imports ODMRDDHelperClassLibrary
Imports MAIS.Business.Helpers

Public Class PersonalInformation
    Inherits System.Web.UI.Page
    Private Shared _maisApp As MAIS.Business.Model.MAISApplicationDetails
    Private Shared _appID As Integer
    Private Shared _sessionId As String
    Private Shared _rnorDD As Boolean
    Private Shared savedData As Boolean = False
    Private Shared queryObject As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            queryObject = Request.QueryString("App")
            addrBar1.LoadStates()
            addrBar1.LoadCounties()
            Dim a As String = Master.RNLicenseOrSSN
            addrBar1.HideEmailAddress = True
            addrBar1.HideLabelEmail = True
            addrBar1.HidePhoneNumber = True
            addrBar1.MandatoryEmailAddress = True
            addrBar1.MandatoryPhone = True
            addrBar1.HideLabelPhone = True
            If (SessionHelper.RN_Flg) Then
                lblLicenseNoLast4SSN.InnerText = "RN Lic. No:"
                lblDOBRNLicIssuance.InnerText = "Date of Original RN Lic. Issuance:"
            Else
                lblLicenseNoLast4SSN.InnerText = "Last 4 SSN:"
                lblDOBRNLicIssuance.InnerText = "Date of Birth:"
            End If

            divNote.FindControl("pNote18").Visible = False
            If SessionHelper.RN_Flg Then
                If SessionHelper.BrandNew Then
                    divNote.FindControl("pNote18").Visible = True
                Else
                    If Not String.IsNullOrEmpty(SessionHelper.SessionUniqueID) Then
                        Dim MSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
                        Dim curCert As Business.Model.Certificate = (From c In MSvc.GetCertificationHistory(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg) Where c.EndDate >= Date.Today Select c).FirstOrDefault
                        If IsNothing(curCert) Then
                            divNote.FindControl("pNote18").Visible = True
                        End If
                    End If
                End If
            ElseIf (SessionHelper.RN_Flg And SessionHelper.BrandNew = False) Then
                divNote.FindControl("pNote18").Visible = False
            End If
            If (queryObject = "Contact") Then
                Master.HideProgressBar = True
                Dim personalSvc As IPersonalInformationService = StructureMap.ObjectFactory.GetInstance(Of IPersonalInformationService)()
                Dim ddpersonel As DDPersonnelDetails = Nothing
                Dim rnInfo As RNInformationDetails = Nothing
                If (SessionHelper.RN_Flg = False) Then
                    ddpersonel = personalSvc.GetDDPersonnelInformationFromPermanent(SessionHelper.SessionUniqueID)
                Else
                    rnInfo = personalSvc.GetRNInfoFromPermanent(SessionHelper.SessionUniqueID)
                End If
                SetAllDefaultValues(ddpersonel, rnInfo)
                lnkBack.Visible = True
                hdNew.Value = "Old"
                txtDOBRNLicIssuance.Disabled = True
                txtFirstName.Disabled = True
                txtLastname.Disabled = True
                txtLicenseNoLast4SSN.Disabled = True
                txtMiddleName.Disabled = True
                btnSaveAndContinue.Visible = False
            Else
                btnSaveAndContinue.Visible = True
                Master.HideProgressBar = False
                lnkBack.Visible = False
                txtDOBRNLicIssuance.Disabled = False
                txtFirstName.Disabled = False
                txtLastname.Disabled = False
                txtLicenseNoLast4SSN.Disabled = False
                txtMiddleName.Disabled = False
                hdNew.Value = "New"
                hdRNFlag.Value = SessionHelper.RN_Flg
                hdRole.Value = SessionHelper.SelectedUserRole
                hdStartpage.Value = SessionHelper.BrandNew
                Dim personalSvc As IPersonalInformationService = StructureMap.ObjectFactory.GetInstance(Of IPersonalInformationService)()
                Dim ddpersonel As DDPersonnelDetails = Nothing
                Dim rnInfo As RNInformationDetails = Nothing
                If (SessionHelper.ApplicationID > 0) Then

                    If (SessionHelper.RN_Flg = False) Then
                        ddpersonel = personalSvc.GetDDPersonnelInformation(SessionHelper.ApplicationID)
                    Else
                        rnInfo = personalSvc.GetRNInformation(SessionHelper.ApplicationID)
                    End If
                End If
                If (ddpersonel Is Nothing And rnInfo Is Nothing) Then
                    If Not (String.IsNullOrWhiteSpace(SessionHelper.SessionUniqueID)) Then
                        If (SessionHelper.RN_Flg = False) Then
                            ddpersonel = personalSvc.GetDDPersonnelInformationFromPermanent(SessionHelper.SessionUniqueID)
                        Else
                            rnInfo = personalSvc.GetRNInfoFromPermanent(SessionHelper.SessionUniqueID)
                        End If
                    End If
                    If (ddpersonel Is Nothing And rnInfo Is Nothing) Then
                        hdNew.Value = "New"
                    Else
                        hdNew.Value = "Old"
                    End If
                Else
                    hdNew.Value = "Old"
                End If
                SetAllDefaultValues(ddpersonel, rnInfo)
            End If           
            'SessionHelper.ApplicationID = app
        End If
        If ((SessionHelper.RN_Flg) AndAlso (SessionHelper.ApplicationType <> EnumHelper.GetEnumDescription(ApplicationType.Initial))) Then
            txtLicenseNoLast4SSN.Disabled = True
        End If
    End Sub
    Private Sub EnableDisableControls(ByVal ddpersonal As DDPersonnelDetails, ByVal rnInfo As RNInformationDetails)
        Dim userRoleInMAIS As Integer = SessionHelper.MAISLevelUserRole
        'Dim userRoleForRNDD As Integer = SessionHelper.SelectedUserRole
        'If (Not (UserAndRoleHelper.IsUserAdmin) And userRoleForRNDD = userRoleInMAIS) Then
        '    txtLicenseNoLast4SSN.Disabled = True
        '    txtDOBRNLicIssuance.Disabled = True
        '    rdbGender.Enabled = False
        'End If
        If (UserAndRoleHelper.IsUserRN) Then
            If (userRoleInMAIS = RoleLevelCategory.RNTrainer_RLC Or userRoleInMAIS = RoleLevelCategory.QARN_RLC Or userRoleInMAIS = RoleLevelCategory.Bed17_RLC Or userRoleInMAIS = RoleLevelCategory.RNInstructor_RLC) Then
                If (rnInfo IsNot Nothing And SessionHelper.RN_Flg) Then
                    If (Not String.IsNullOrEmpty(rnInfo.RNLicense) And (rnInfo.DateOforiginalRNLicIssuance <> Convert.ToDateTime("12/31/9999"))) Then
                        txtLicenseNoLast4SSN.Disabled = True
                        txtDOBRNLicIssuance.Disabled = True
                        rdbGender.Enabled = False
                    Else
                        txtLicenseNoLast4SSN.Disabled = False
                        txtDOBRNLicIssuance.Disabled = False
                        rdbGender.Enabled = True
                    End If
                Else
                    rdbGender.Enabled = True
                    If (SessionHelper.RN_Flg = False And ddpersonal IsNot Nothing) Then
                        If (Not String.IsNullOrEmpty(ddpersonal.DODDLast4SSN) And (ddpersonal.DODDDateOfBirth <> Convert.ToDateTime("12/31/9999"))) Then
                            txtLicenseNoLast4SSN.Disabled = True
                            txtDOBRNLicIssuance.Disabled = True
                            'rdbGender.Enabled = False
                        Else
                            txtLicenseNoLast4SSN.Disabled = False
                            txtDOBRNLicIssuance.Disabled = False
                            'rdbGender.Enabled = True
                        End If
                    End If
                End If
            End If
        End If
        If (UserAndRoleHelper.IsUserAdmin Or userRoleInMAIS = RoleLevelCategory.RNMaster_RLC Or SessionHelper.MyUpdate_Profile) Then
            txtLicenseNoLast4SSN.Disabled = False
            txtDOBRNLicIssuance.Disabled = False
            rdbGender.Enabled = True
            txtLastname.Disabled = False
            txtFirstName.Disabled = False
            txtMiddleName.Disabled = False
            txtHomePhoneNumber.Disabled = False
            txtWorkPhoneNumber.Disabled = False
            txtCellPhoneNumber.Disabled = False
            txtHomeAddress.Disabled = False
            txtWorkAddress.Disabled = False
            txtCellAddress.Disabled = False
            addrBar1.HideAddress1 = True
            addrBar1.HideAddress2 = True
            addrBar1.HideCity = True
            addrBar1.EnableState = False
            addrBar1.HideCounty = False
            addrBar1.HideZip = True
            addrBar1.HideZipPlus = True
        End If
    End Sub

    Private Sub SetAllDefaultValues(ByVal ddpersonal As DDPersonnelDetails, ByVal rnInfo As RNInformationDetails)
        EnableDisableControls(ddpersonal, rnInfo)
        If (Not (ddpersonal Is Nothing)) Then
            txtLicenseNoLast4SSN.Value = ddpersonal.DODDLast4SSN.Trim()
            If (ddpersonal.DODDDateOfBirth.ToShortDateString() = "12/31/9999") Then
                txtDOBRNLicIssuance.Value = String.Empty
            Else
                txtDOBRNLicIssuance.Value = ddpersonal.DODDDateOfBirth.ToShortDateString().Trim()
            End If
            txtLastname.Value = ddpersonal.DODDLastName.Trim()
            txtFirstName.Value = ddpersonal.DODDFirstName.Trim()
            txtMiddleName.Value = Trim(ddpersonal.DODDMiddleName)
            addrBar1.AddressLine1 = Trim(ddpersonal.DODDHomeAddressLine1)
            If (Not String.IsNullOrEmpty(ddpersonal.DODDHomeAddressLine2)) Then
                addrBar1.AddressLine2 = ddpersonal.DODDHomeAddressLine2.Trim()
            Else
                addrBar1.AddressLine2 = String.Empty
            End If
            addrBar1.City = Trim(ddpersonal.DODDHomeCity)
            If (Not String.IsNullOrEmpty(ddpersonal.DODDHomeCounty)) Then
                'addrBar1.County = ddpersonal.DODDHomeCounty.Trim()
                addrBar1.CountyID = ddpersonal.DODDHomeCountyID
            Else
                'addrBar1.County = "--- County Selection ---"
                addrBar1.CountyID = -1
            End If
            addrBar1.StateID = ddpersonal.DODDHomeStateID
            'addrBar1.State = ddpersonal.DODDHomeState.Trim()
            addrBar1.Zip = Trim(ddpersonal.Address.Zip)
            If (String.IsNullOrEmpty(ddpersonal.Address.ZipPlus)) Then
                ddpersonal.DODDHomeZipPlus = String.Empty
            Else
                addrBar1.ZipPlus = ddpersonal.Address.ZipPlus.Trim()
            End If
            If (ddpersonal.DODDGender = "F") Then
                rdbGender.SelectedValue = "1"
            ElseIf (ddpersonal.DODDGender = "M") Then
                rdbGender.SelectedValue = "0"
            End If
            If (ddpersonal.Address.Phone IsNot Nothing) Then
                For Each ph As PhoneDetails In ddpersonal.Address.Phone
                    If (ph.ContactType = ContactType.Home) Then
                        txtHomePhoneNumber.Value = ph.PhoneNumber.Trim()
                    ElseIf (ph.ContactType = ContactType.Work) Then
                        txtWorkPhoneNumber.Value = ph.PhoneNumber.Trim()
                    ElseIf (ph.ContactType = ContactType.CellOther) Then
                        txtCellPhoneNumber.Value = ph.PhoneNumber.Trim()
                    End If
                Next
            End If
            If (ddpersonal.Address.Email IsNot Nothing) Then
                For Each ph As EmailAddressDetails In ddpersonal.Address.Email
                    If (ph.ContactType = ContactType.Home) Then
                        txtHomeAddress.Value = ph.EmailAddress.Trim()
                    ElseIf (ph.ContactType = ContactType.Work) Then
                        txtWorkAddress.Value = ph.EmailAddress.Trim()
                    ElseIf (ph.ContactType = ContactType.CellOther) Then
                        txtCellAddress.Value = ph.EmailAddress.Trim()
                    End If
                Next
            End If
        End If
        If (Not (rnInfo Is Nothing)) Then
            txtLicenseNoLast4SSN.Value = rnInfo.RNLicense.Trim()
            If (rnInfo.DateOforiginalRNLicIssuance.ToShortDateString() = "12/31/9999") Then
                txtDOBRNLicIssuance.Value = String.Empty
            Else
                txtDOBRNLicIssuance.Value = rnInfo.DateOforiginalRNLicIssuance.ToShortDateString().Trim()
            End If
            txtLastname.Value = rnInfo.LastName.Trim()
            txtFirstName.Value = rnInfo.FirstName.Trim()
            txtMiddleName.Value = Trim(rnInfo.MiddleName)
            addrBar1.AddressLine1 = Trim(rnInfo.HomeAddressLine1)
            If (Not String.IsNullOrEmpty(rnInfo.HomeAddressLine2)) Then
                addrBar1.AddressLine2 = rnInfo.HomeAddressLine2.Trim()
            Else
                addrBar1.AddressLine2 = String.Empty
            End If
            addrBar1.City = Trim(rnInfo.HomeCity)
            If ((Not String.IsNullOrEmpty(rnInfo.HomeCounty))) Then
                'addrBar1.County = rnInfo.HomeCounty.Trim()
                addrBar1.CountyID = rnInfo.HomeCountyID
            Else
                'addrBar1.County = "--- County Selection ---"
                addrBar1.CountyID = -1
            End If
            addrBar1.StateID = rnInfo.HomeStateID
            'addrBar1.State = rnInfo.HomeState.Trim()
            addrBar1.Zip = Trim(rnInfo.Address.Zip)
            If (String.IsNullOrEmpty(rnInfo.Address.ZipPlus)) Then
                addrBar1.ZipPlus = String.Empty
            Else
                addrBar1.ZipPlus = rnInfo.Address.ZipPlus.Trim()
            End If
            If (rnInfo.Gender = "F") Then
                rdbGender.SelectedValue = "1"
            ElseIf (rnInfo.Gender = "M") Then
                rdbGender.SelectedValue = "0"
            End If
            If (rnInfo.Address.Phone IsNot Nothing) Then
                For Each ph As PhoneDetails In rnInfo.Address.Phone
                    If (ph.ContactType = ContactType.Home) Then
                        txtHomePhoneNumber.Value = ph.PhoneNumber.Trim()
                    ElseIf (ph.ContactType = ContactType.Work) Then
                        txtWorkPhoneNumber.Value = ph.PhoneNumber.Trim()
                    ElseIf (ph.ContactType = ContactType.CellOther) Then
                        txtCellPhoneNumber.Value = ph.PhoneNumber.Trim()
                    End If
                Next
            End If
            If (rnInfo.Address.Email IsNot Nothing) Then
                For Each ph As EmailAddressDetails In rnInfo.Address.Email
                    If (ph.ContactType = ContactType.Home) Then
                        txtHomeAddress.Value = ph.EmailAddress.Trim()
                    ElseIf (ph.ContactType = ContactType.Work) Then
                        txtWorkAddress.Value = ph.EmailAddress.Trim()
                    ElseIf (ph.ContactType = ContactType.CellOther) Then
                        txtCellAddress.Value = ph.EmailAddress.Trim()
                    End If
                Next
            End If
        End If
    End Sub
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function SavePersonalInformation(ByVal savePersonalInfoVars As Dictionary(Of String, String)) As Object
        savedData = True
        Dim retObj As New ReturnObject(Of Long)(-1L)
        Dim jsonOutput As Object = DBNull.Value
        If (SessionHelper.SelectedUserRole = Enums.RoleLevelCategory.QARN_RLC And SessionHelper.LoginUsersRNLicense = savePersonalInfoVars("LicenseLast4SSN") And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(ApplicationType.Initial)) Then
            retObj.AddErrorMessage("You cannot do initial application for your account")
            jsonOutput = New With {.bolVal = retObj.Errors(0)}
        Else
            Dim strArr() As String
            Dim _personalInfo As New PersonalInformationDetails
            Dim _listphone As New List(Of PhoneDetails)
            Dim _listEmail As New List(Of EmailAddressDetails)
            _personalInfo.DOBDateOfIssuance = Convert.ToDateTime(savePersonalInfoVars("DOBRNLicIssuance"))
            _personalInfo.FirstName = savePersonalInfoVars("FirstName")
            If (savePersonalInfoVars("Gender") = 0) Then
                _personalInfo.Gender = "M"
            Else
                _personalInfo.Gender = "F"
            End If
            _personalInfo.LastName = savePersonalInfoVars("LastName")
            _personalInfo.MiddleName = savePersonalInfoVars("MiddleName")
            _personalInfo.RNLicenseOrSSN = savePersonalInfoVars("LicenseLast4SSN")
            If Not (SessionHelper.RN_Flg) And Not (String.IsNullOrWhiteSpace(SessionHelper.SessionUniqueID)) Then
                _personalInfo.DDPersonnelCode = SessionHelper.SessionUniqueID
            End If
            If (savePersonalInfoVars.ContainsKey("HomePhoneNumber")) Then
                If (Not String.IsNullOrEmpty(savePersonalInfoVars("HomePhoneNumber"))) Then
                    Dim homePhone As String = String.Empty
                    If (savePersonalInfoVars("HomePhoneNumber").Contains("-")) Then
                        homePhone = savePersonalInfoVars("HomePhoneNumber").Replace("-", "")
                    Else
                        homePhone = savePersonalInfoVars("HomePhoneNumber")
                    End If
                    Dim _phone As New PhoneDetails
                    _phone.PhoneNumber = homePhone
                    _phone.ContactType = ContactType.Home
                    _listphone.Add(_phone)
                End If
            End If

            If (savePersonalInfoVars.ContainsKey("WorkPhoneNumber")) Then
                If (Not String.IsNullOrEmpty(savePersonalInfoVars("WorkPhoneNumber"))) Then
                    Dim workPhone As String = String.Empty
                    If (savePersonalInfoVars("WorkPhoneNumber").Contains("-")) Then
                        workPhone = savePersonalInfoVars("WorkPhoneNumber").Replace("-", "")
                    Else
                        workPhone = savePersonalInfoVars("WorkPhoneNumber")
                    End If
                    Dim _workphone As New PhoneDetails
                    _workphone.PhoneNumber = workPhone
                    _workphone.ContactType = ContactType.Work
                    _listphone.Add(_workphone)
                End If
            End If

            If (savePersonalInfoVars.ContainsKey("CellPhoneNumber")) Then
                If (Not String.IsNullOrEmpty(savePersonalInfoVars("CellPhoneNumber"))) Then
                    Dim cellPhone As String = String.Empty
                    If (savePersonalInfoVars("CellPhoneNumber").Contains("-")) Then
                        cellPhone = savePersonalInfoVars("CellPhoneNumber").Replace("-", "")
                    Else
                        cellPhone = savePersonalInfoVars("CellPhoneNumber")
                    End If
                    Dim _cellphone As New PhoneDetails
                    _cellphone.PhoneNumber = cellPhone
                    _cellphone.ContactType = ContactType.CellOther
                    _listphone.Add(_cellphone)
                End If
            End If

            If (savePersonalInfoVars.ContainsKey("HomeAddress")) Then
                If (Not String.IsNullOrEmpty(savePersonalInfoVars("HomeAddress"))) Then
                    Dim _email As New EmailAddressDetails
                    _email.EmailAddress = savePersonalInfoVars("HomeAddress")
                    _email.ContactType = ContactType.Home
                    _listEmail.Add(_email)
                End If
            End If

            If (savePersonalInfoVars.ContainsKey("WorkAddress")) Then
                If (Not String.IsNullOrEmpty(savePersonalInfoVars("WorkAddress"))) Then
                    Dim _workemail As New EmailAddressDetails
                    _workemail.EmailAddress = savePersonalInfoVars("WorkAddress")
                    _workemail.ContactType = ContactType.Work
                    _listEmail.Add(_workemail)
                End If
            End If

            If (savePersonalInfoVars.ContainsKey("CellAddress")) Then
                If (Not String.IsNullOrEmpty(savePersonalInfoVars("CellAddress"))) Then
                    Dim _cellemail As New EmailAddressDetails
                    _cellemail.EmailAddress = savePersonalInfoVars("CellAddress")
                    _cellemail.ContactType = ContactType.CellOther
                    _listEmail.Add(_cellemail)
                End If
            End If
            If SessionHelper.RN_Flg Then

            End If
            _personalInfo.Phone = _listphone
            _personalInfo.Email = _listEmail

            Dim address As String = savePersonalInfoVars("UserControlAddress")
            strArr = address.Split("*")
            _personalInfo.AddressLine1 = strArr(0)
            _personalInfo.AddressLine2 = strArr(1)
            _personalInfo.City = strArr(2)
            _personalInfo.State = strArr(3)
            _personalInfo.Zip = strArr(4)
            _personalInfo.ZipPlus = strArr(5)
            _personalInfo.County = strArr(6)

            Dim personalSvc As IPersonalInformationService = StructureMap.ObjectFactory.GetInstance(Of IPersonalInformationService)()
            retObj = personalSvc.SavePersonalInformation(_personalInfo, SessionHelper.ApplicationID, SessionHelper.RN_Flg, SessionHelper.Name, SessionHelper.BrandNew, SessionHelper.SessionUniqueID, UserAndRoleHelper.IsUserAdmin, queryObject)
            If (retObj.Messages.Count > 0) Then
                jsonOutput = New With {.bolVal = retObj.Errors(0)}
            Else
                SessionHelper.BrandNew = False
                SessionHelper.Name = _personalInfo.LastName + "," + _personalInfo.FirstName + _personalInfo.MiddleName
                If (SessionHelper.RN_Flg) Then
                    SessionHelper.SessionUniqueID = _personalInfo.RNLicenseOrSSN
                Else
                    If (String.IsNullOrEmpty(SessionHelper.SessionUniqueID)) Then
                        SessionHelper.SessionUniqueID = String.Empty
                    End If
                End If
            End If
        End If
        Return jsonOutput
    End Function
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function DeleteAppInfo() As Object
        Dim personalSvc As IPersonalInformationService = StructureMap.ObjectFactory.GetInstance(Of IPersonalInformationService)()
        Dim flag As Boolean = personalSvc.DeleteTheDataApplication(SessionHelper.ApplicationID)
        Dim json As Object = Nothing
        json = flag
        SessionHelper.ApplicationID = 0
        SessionHelper.SelectedUserRole = 0
        SessionHelper.Name = String.Empty
        SessionHelper.SessionUniqueID = String.Empty
        Return json
    End Function
    Protected Sub lnkBack_Click(sender As Object, e As EventArgs) Handles lnkBack.Click
        Response.Redirect("UpdateExistingPage.aspx")
    End Sub
End Class