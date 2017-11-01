Imports System.Web.Script.Services
Imports ODMRDDHelperClassLibrary
Imports ODMRDDHelperClassLibrary.ODMRDDServiceProvider
Imports MAIS.Business.Services
Imports MAIS.Business.Model
Imports MAIS.Business.Model.Enums
Imports ODMRDDHelperClassLibrary.Utility

Public Class EmployerInformation
    Inherits System.Web.UI.Page
    Private Shared flag1 As Boolean = False
    Private Shared superflag As Boolean = True
    Private Shared emprecentID As Integer = 0    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If SessionHelper.ApplicationID > 0 Then
                divError.Visible = False
                addrBar.LoadStates()
                addrBar.LoadCounties()
                addrBar1.LoadStates()
                addrBar1.LoadCounties()
                pnlDD.Visible = True
                pnlEmployer.Visible = True
                pnlEmp.Visible = True
                pnlProgressBar.Visible = True
                Panel1.Visible = True
                pnlRecent.Visible = True
                pnlHistroy.Visible = True
                pnlFooterNav.Visible = True
                Panel2.Visible = True
                If SessionHelper.RN_Flg = False Then
                    rblSelect.Items(0).Enabled = True
                    rblSelect.Items(1).Enabled = True
                    rblSelect.Items(2).Enabled = True
                    rblSelect.Items(3).Enabled = False
                    rblSelect.Items(4).Enabled = False
                Else
                    rblSelect.Items(0).Enabled = False
                    rblSelect.Items(1).Enabled = True
                    rblSelect.Items(2).Enabled = True
                    rblSelect.Items(3).Enabled = True
                    rblSelect.Items(4).Enabled = True
                    ''pnlEmployer.Visible = False
                    'If (SessionHelper.SelectedUserRole = Enums.RoleLevelCategory.QARN_RLC) Then
                    '    rblSelect.Items(0).Enabled = False
                    '    rblSelect.Items(1).Enabled = True
                    '    rblSelect.Items(2).Enabled = False
                    '    rblSelect.Items(3).Enabled = False
                    'Else
                    '    rblSelect.Items(0).Enabled = False
                    '    rblSelect.Items(1).Enabled = True
                    '    rblSelect.Items(2).Enabled = True
                    '    rblSelect.Items(3).Enabled = True
                    'End If
                End If
                GetRecentlyAddedInfo()
                GetEmployer()
            Else
                lblAppError.InnerText = "There exists no applications to start with employer's page. Please navigate to start page."
                divError.Visible = True
                SessionHelper.ApplicationID = 0
                SessionHelper.SelectedUserRole = 0
                SessionHelper.Name = String.Empty
                pnlDD.Visible = False
                pnlEmployer.Visible = False
                pnlEmp.Visible = False
                pnlProgressBar.Visible = False
                Panel1.Visible = False
                pnlRecent.Visible = False
                pnlHistroy.Visible = False
                pnlFooterNav.Visible = False
                Panel2.Visible = False
            End If
        End If
    End Sub
    Private Sub GetRecentlyAddedInfo()
        Dim employerSvc As IEmployerInformationService = StructureMap.ObjectFactory.GetInstance(Of IEmployerInformationService)()
        Dim listEmployer As New List(Of EmployerInformationDetails)
        listEmployer = employerSvc.GetRecentlyAddedEmplyerInfo(SessionHelper.ApplicationID)
        If (listEmployer.Count > 0) Then
            ViewState("Pending_Employer_Info_Flg") = (From lst In listEmployer
                                                      Where lst.Pending_Information_Flg = True
                                                      Select lst.Pending_Information_Flg).FirstOrDefault()
            DisplayRecentlyAddedResults(grdRecent, listEmployer, pnlRecent)
            If (ViewState("Pending_Employer_Info_Flg") = True) Then
                btnSave.Enabled = False
                btnSaveAdditional.Enabled = False
                rblSelect.Enabled = False
            Else
                btnSave.Enabled = True
                btnSaveAdditional.Enabled = True
                rblSelect.Enabled = True
            End If
        Else
            ViewState("Pending_Employer_Info_Flg") = False
            btnSave.Enabled = True
            btnSaveAdditional.Enabled = True
            rblSelect.Enabled = True
            grdRecent.DataSource = Nothing
            grdRecent.DataBind()
        End If

        'grdRecent.Columns(7).Visible = False
        'grdRecent.Columns(8).Visible = False
    End Sub
    Private Sub GetEmployer()
        Dim employerSvc As IEmployerInformationService = StructureMap.ObjectFactory.GetInstance(Of IEmployerInformationService)()
        Dim listEmployer As New List(Of EmployerInformationDetails)
        listEmployer = employerSvc.GetEmployerInformationFromPerm(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg)
        If (listEmployer.Count > 0) Then
            DisplayResults(grdViewHistory, listEmployer, pnlHistroy)
        End If
    End Sub
    Private Sub DisplayRecentlyAddedResults(ByVal displayGrid As GridView, ByVal results As IEnumerable(Of EmployerInformationDetails), ByVal panels As Panel)
        If results.Count = 0 Then
            panels.Visible = False
            displayGrid.DataSource = Nothing
            displayGrid.DataBind()
        Else
            panels.Visible = True
            displayGrid.DataSource = (From p In results
                                      Order By p.EmployerName
                                      Select New With
                                             {
                                                 .EmployerID = p.EmployerID.ToString(),
                                                 .EmployerName = p.EmployerName.Trim(),
                                                 .CEOFirstName = p.CEOFirstName.Trim(),
                                                 .CEOLastName = p.CEOLastName.Trim(),
                                                 .SupervisorFirstName = p.SupervisorFirstName.Trim(),
                                                 .SupervisorLastName = p.SupervisorLastName.Trim(),
                                                 .SuperVisorEndDate = p.CurrentSupervisor.ToShortDateString(),
                                                 .WorkLocationEndDate = p.CurrentWorkLocation.ToShortDateString(),
                                                 .DODDProviderContractNumber = Trim(p.DODDProviderContractNumber),
                                                 .IdentitficationValue = Trim(p.IdentitficationValue),
                                                 .Pending_Information_Flg = p.Pending_Information_Flg
                                          }).ToList()
            displayGrid.DataBind()
        End If
    End Sub
    Private Sub DisplayResults(ByVal displayGrid As GridView, ByVal results As IEnumerable(Of EmployerInformationDetails), ByVal panels As Panel)
        If results.Count = 0 Then
            panels.Visible = False
        Else
            panels.Visible = True
            displayGrid.DataSource = (From p In results
                                      Order By p.EmployerName
                                      Select New With
                                             {
                                                 .EmployerID = p.EmployerID.ToString(),
                                                 .EmployerName = p.EmployerName.Trim(),
                                                 .CEOFirstName = p.CEOFirstName.Trim(),
                                                 .CEOLastName = p.CEOLastName.Trim(),
                                                 .SupervisorFirstName = p.SupervisorFirstName.Trim(),
                                                 .SupervisorLastName = p.SupervisorLastName.Trim(),
                                                 .SuperVisorEndDate = p.CurrentSupervisor.ToShortDateString(),
                                                 .WorkLocationEndDate = p.CurrentWorkLocation.ToShortDateString(),
                                                 .DODDProviderContractNumber = Trim(p.DODDProviderContractNumber),
                                                 .IdentitficationValue = Trim(p.IdentitficationValue)
                                          }).ToList()
            displayGrid.DataBind()
        End If
    End Sub
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function GetAgencyInformation(ByVal getAgencyInfo As Dictionary(Of String, String)) As Collection
        Dim provider As New Utility.ReturnObject(Of List(Of ODMRDDServiceProvider.IODMRDDServiceProvider))
        Dim json As New Collection
        Dim independent As String = "ANY"
        Dim contractNumber As String = getAgencyInfo("ContractNumber")
        'Dim providerName As String = getAgencyInfo("EmployerName")
        If (getAgencyInfo("Independent") = 3) Then
            independent = "I"
        ElseIf (getAgencyInfo("Independent") = 4) Then
            independent = "A"
        End If
        Dim _providerService As ServiceProviderService = New ODMRDDServiceProvider.ServiceProviderService(ConfigHelper.GetProviderConnectionString)
        provider = _providerService.GetServiceProviders(contractNumber, "", "", "", independent)
        For Each prov As IODMRDDServiceProvider In provider.ReturnValue
            Dim jsonObject As Object
            jsonObject = New With {
            .ProviderName = prov.ProviderName,
            .Name = prov.FirstName + " " + prov.LastName,
            .ContractNumber = prov.ContractNumber
        }
            json.Add(jsonObject)
        Next
        Return json
    End Function
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function GetEmployerInformation(ByVal empInfo As Dictionary(Of String, String)) As Object
        Dim provider As New Utility.ReturnObject(Of ODMRDDServiceProvider.IODMRDDServiceProvider)
        Dim jsonOutput As New Object
        Dim countyCode As String = String.Empty
        Dim contractNumber As String = empInfo("ContractNumber")
        Dim _providerService As ServiceProviderService = New ODMRDDServiceProvider.ServiceProviderService(ConfigHelper.GetProviderConnectionString)
        provider = _providerService.GetServiceProviderByContractNumberAndName(contractNumber)
        If (provider.ReturnValue IsNot Nothing) Then
            Dim zip = String.Empty
            If ((provider.ReturnValue.ZipPlus4Cd IsNot Nothing) And (Not String.IsNullOrWhiteSpace(provider.ReturnValue.ZipPlus4Cd))) Then
                zip = provider.ReturnValue.ZipPlus4Cd.Trim()
            End If
            Dim phone = String.Empty
            If ((provider.ReturnValue.PhoneNumber IsNot Nothing) And (Not String.IsNullOrWhiteSpace(provider.ReturnValue.PhoneNumber))) Then
                phone = provider.ReturnValue.PhoneNumber.Trim()
            End If
            Dim email = String.Empty
            If ((provider.ReturnValue.EmailAddress IsNot Nothing) And (Not String.IsNullOrWhiteSpace(provider.ReturnValue.EmailAddress))) Then
                email = provider.ReturnValue.EmailAddress.Trim()
            End If
            Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
            Dim countyID As Integer = -1
            Dim stateID As Integer = 35
            If (provider.ReturnValue.County <> 0) Then
                Dim lstCountyBoards As New ReturnObject(Of String)
                Dim cs As CountyBoardService = New CountyBoardService(ConfigHelper.GetOIDDBConnectionString)
                lstCountyBoards = cs.GetCountyBoardAliasById(provider.ReturnValue.County, "mastr")
                countyCode = lstCountyBoards.ReturnValue
                countyID = maisSvc.GetCountyIDByCodes(countyCode)
            End If
            Dim FirstName = String.Empty
            Dim LastName = String.Empty
            Dim EntityName = String.Empty
            If ((provider.ReturnValue.ProviderName Is Nothing) OrElse (String.IsNullOrWhiteSpace(Trim(provider.ReturnValue.ProviderName)))) Then
                EntityName = "Unknown"
            Else
                EntityName = Trim(provider.ReturnValue.ProviderName)
            End If
            If ((provider.ReturnValue.LastName Is Nothing) OrElse (String.IsNullOrWhiteSpace(provider.ReturnValue.LastName.Trim()))) Then
                LastName = "Unknown"
            Else
                LastName = provider.ReturnValue.LastName.Trim()
            End If
            If ((provider.ReturnValue.FirstName Is Nothing) OrElse (String.IsNullOrWhiteSpace(provider.ReturnValue.FirstName.Trim()))) Then
                FirstName = "Unknown"
            Else
                FirstName = provider.ReturnValue.FirstName.Trim()
            End If
            Dim addrln2 = String.Empty
            If ((provider.ReturnValue.AddrLn2 IsNot Nothing) And (Not String.IsNullOrWhiteSpace(provider.ReturnValue.AddrLn2))) Then
                addrln2 = provider.ReturnValue.AddrLn2.Trim()
            End If
            stateID = maisSvc.GetStateIDByStates(provider.ReturnValue.StateNm.Trim())
            jsonOutput = New With {
            .EntityName = EntityName,
            .TaxID = provider.ReturnValue.TaxID.Trim(),
            .CertificateStartDate = provider.ReturnValue.CertStartDate.ToShortDateString().Trim(),
            .CertificationEndDate = provider.ReturnValue.CertEndDate.ToShortDateString().Trim(),
            .CertficationStatus = provider.ReturnValue.CertStatus.Trim(),
            .LastName = LastName,
            .Firstname = FirstName,
            .AddressLine1 = provider.ReturnValue.AddrLn1.Trim(),
            .AddressLine2 = addrln2,
            .City = provider.ReturnValue.CityNm.Trim(),
            .State = stateID,
            .Zip = provider.ReturnValue.ZipCd.ToString().Trim(),
            .ZipPlus = zip,
            .County = countyID,
            .PhoneNumber = phone,
            .EmailAddress = email
                }
        End If
        Return jsonOutput
    End Function
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function GetRNInformation() As Object
        Dim personalSvc As IPersonalInformationService = StructureMap.ObjectFactory.GetInstance(Of IPersonalInformationService)()
        Dim rnInfo As RNInformationDetails = Nothing
        Dim county As String = String.Empty
        Dim jSon As New Object
        Dim phone As String = String.Empty
        Dim mailAddress As String = String.Empty
        Dim AddressLine1 As String
        Dim AddressLine2 As String
        Dim City As String
        Dim State As String
        Dim Zip As String
        Dim ZipPlus As String

        rnInfo = personalSvc.GetRNInformation(SessionHelper.ApplicationID)
        If ((Trim(rnInfo.HomeAddressLine1) Is Nothing) OrElse (String.IsNullOrWhiteSpace(Trim(rnInfo.HomeAddressLine1)))) Then
            AddressLine1 = String.Empty
        Else
            AddressLine1 = Trim(rnInfo.HomeAddressLine1)
        End If

        If ((Trim(rnInfo.HomeAddressLine2) Is Nothing) OrElse (String.IsNullOrWhiteSpace(Trim(rnInfo.HomeAddressLine2)))) Then
            AddressLine2 = String.Empty
        Else
            AddressLine2 = Trim(rnInfo.HomeAddressLine2)
        End If
        If ((Trim(rnInfo.HomeCity) Is Nothing) OrElse (String.IsNullOrWhiteSpace(Trim(rnInfo.HomeCity)))) Then
            City = String.Empty
        Else
            City = Trim(rnInfo.HomeCity)
        End If
        If ((Trim(rnInfo.HomeZip) Is Nothing) OrElse (String.IsNullOrWhiteSpace(Trim(rnInfo.HomeZip)))) Then
            Zip = String.Empty
        Else
            Zip = Trim(rnInfo.HomeZip)
        End If
        If ((Trim(rnInfo.HomeZipPlus) Is Nothing) OrElse (String.IsNullOrWhiteSpace(Trim(rnInfo.HomeZipPlus)))) Then
            ZipPlus = String.Empty
        Else
            ZipPlus = Trim(rnInfo.HomeZipPlus)
        End If
        'If (rnInfo.HomeState > 0) Then
        '    StateID = 35
        'Else
        '    StateID = Trim(rnInfo.HomeStateID)
        'End If
        If ((String.IsNullOrWhiteSpace(rnInfo.HomeState)) OrElse (Trim(rnInfo.HomeState) Is Nothing)) Then
            State = 35
        Else
            State = rnInfo.HomeStateID
        End If
        For Each ph As PhoneDetails In rnInfo.Address.Phone
            If (ph.ContactType = ContactType.Home) Then
                phone = ph.PhoneNumber
            End If
        Next
        For Each email As EmailAddressDetails In rnInfo.Address.Email
            If (email.ContactType = ContactType.Home) Then
                mailAddress = email.EmailAddress
            End If
        Next
        If ((String.IsNullOrWhiteSpace(rnInfo.HomeCounty)) OrElse (Trim(rnInfo.HomeCounty) Is Nothing)) Then
            'county = "--- County Selection ---"
            county = -1
        Else
            county = rnInfo.HomeCountyID
        End If

        jSon = New With
                     {
                         .RNLicense = rnInfo.RNLicense.Trim(),
                         .CEOFirstName = rnInfo.FirstName.Trim(),
                         .CEOLastName = rnInfo.LastName.Trim(),
                         .CEOMiddleName = rnInfo.MiddleName.Trim(),
                         .AddressLine1 = AddressLine1,
                         .AddressLine2 = AddressLine2,
                         .City = City,
                         .County = county,
                         .State = State,
                         .Zip = Zip,
                         .ZipPlus = ZipPlus,
                        .HomePhone = phone.Trim(),
                         .HomeEmail = mailAddress.Trim(),
                         .EmployerName = rnInfo.FirstName.Trim() + " " + rnInfo.LastName.Trim()
                     }
        Return jSon
    End Function
    <WebMethod()>
   <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function SaveEmployerInformation(ByVal saveInfo As Dictionary(Of String, String)) As Object
        emprecentID = 0
        Dim json As Object = DBNull.Value
        Dim provider As String = String.Empty
        Dim employerID As Integer = Convert.ToInt32(saveInfo("EmployerId"))
        Dim employerInfo As New EmployerInformationDetails
        employerInfo.CEOFirstName = saveInfo("CEOFirstName")
        employerInfo.CEOLastName = saveInfo("CEOLastName")
        employerInfo.CEOMiddleName = saveInfo("CEOMiddleName")
        If (saveInfo.ContainsKey("ContractNumber")) Then
            provider = saveInfo("ContractNumber")
        End If
        employerInfo.DODDProviderContractNumber = provider
        If (Not String.IsNullOrWhiteSpace(saveInfo("EmployerStartDate"))) Then
            employerInfo.EmployerStartDate = Convert.ToDateTime(saveInfo("EmployerStartDate"))
        Else
            employerInfo.EmployerStartDate = "12/31/9999"
        End If
        If (Not String.IsNullOrWhiteSpace(saveInfo("EmployerEndDate"))) Then
            employerInfo.EmployerEndDate = Convert.ToDateTime(saveInfo("EmployerEndDate"))
        Else
            employerInfo.EmployerEndDate = "12/31/9999"
        End If

        employerInfo.EmployerName = saveInfo("EmployerName")
        employerInfo.EmployerTaxID = saveInfo("IdentificationValue")
        employerInfo.EmployerTypeID = Convert.ToInt32(saveInfo("EmployerType"))

        If ((employerInfo.EmployerTypeID = 5) And (String.IsNullOrWhiteSpace(employerInfo.EmployerTaxID))) Then
            employerInfo.EmployerTaxID = "NoProvider"
        End If


        If (Not String.IsNullOrWhiteSpace(saveInfo("SupervisorStartDate"))) Then
            employerInfo.StartDate = Convert.ToDateTime(saveInfo("SupervisorStartDate"))
        Else
            employerInfo.StartDate = "12/31/9999"
        End If

        If (Not String.IsNullOrWhiteSpace(saveInfo("SupervisorEndDate"))) Then
            employerInfo.EndDate = Convert.ToDateTime(saveInfo("SupervisorEndDate"))
        Else
            employerInfo.EndDate = "12/31/9999"
        End If
        If (Not String.IsNullOrWhiteSpace(saveInfo("IdentificationType"))) Then
            If (saveInfo("IdentificationType").IndexOf("Provider", StringComparison.OrdinalIgnoreCase) > -1) Then
                employerInfo.EmployerIdentificationTypeID = IdentificationType.Provider
            ElseIf (saveInfo("IdentificationType").IndexOf("Tax", StringComparison.OrdinalIgnoreCase) > -1) Then
                employerInfo.EmployerIdentificationTypeID = IdentificationType.TaxID
            ElseIf (saveInfo("IdentificationType").IndexOf("RN", StringComparison.OrdinalIgnoreCase) > -1) Then
                employerInfo.EmployerIdentificationTypeID = IdentificationType.RNLicense
            End If
        End If

        employerInfo.SupervisorFirstName = saveInfo("SupervisorFirstName")
        employerInfo.SupervisorLastName = saveInfo("SupervisorlastName")
        employerInfo.AgencyPersonalAddressSame = saveInfo("PersonalCheckbox")
        employerInfo.AgencyWorkAddressSame = saveInfo("AgencyWorkSame")
        Dim agencyAddress As New AddressControlDetails
        agencyAddress.AddressLine1 = saveInfo("AgencyAddressLine1")
        agencyAddress.AddressLine2 = saveInfo("AgencyAddressLine2")
        agencyAddress.City = saveInfo("AgencyCity")
        agencyAddress.State = saveInfo("AgencyState")
        agencyAddress.Zip = saveInfo("AgencyZip")
        agencyAddress.ZipPlus = saveInfo("AgencyZipPlus")
        If (saveInfo("AgencyCounty") <> -1) Then
            agencyAddress.County = saveInfo("AgencyCounty")
        Else
            agencyAddress.County = String.Empty
        End If
        If (Not String.IsNullOrEmpty(saveInfo("AgencyPhone"))) Then
            agencyAddress.Phone = saveInfo("AgencyPhone").Replace("-", "")
            agencyAddress.ContactType = ContactType.AgencyCEO
        End If
        If (Not String.IsNullOrEmpty(saveInfo("AgencyEmail"))) Then
            agencyAddress.Email = saveInfo("AgencyEmail")
            agencyAddress.ContactType = ContactType.AgencyCEO
        End If
        agencyAddress.AddressType = AddressType.Agency
        agencyAddress.StartDate = DateTime.Now()
        agencyAddress.EndDate = Convert.ToDateTime("12/31/9999")
        employerInfo.AgencyLocationAddress = agencyAddress
        Dim workAddress As New AddressControlDetails
        workAddress.AddressLine1 = saveInfo("WorkAddressLine1")
        workAddress.AddressLine2 = saveInfo("WorkAddressLine2")
        workAddress.City = saveInfo("WorkCity")
        workAddress.State = saveInfo("WorkState")
        workAddress.Zip = saveInfo("WorkZip")
        workAddress.ZipPlus = saveInfo("WorkZipPlus")
        If (saveInfo("WorkCounty") <> -1) Then
            workAddress.County = saveInfo("WorkCounty")
        Else
            workAddress.County = String.Empty
        End If
        If (Not String.IsNullOrEmpty(saveInfo("WorkPhone"))) Then
            workAddress.Phone = saveInfo("WorkPhone").Replace("-", "")
            workAddress.ContactType = ContactType.WorkLocation
        End If
        If (Not String.IsNullOrEmpty(saveInfo("WorkEmail"))) Then
            workAddress.Email = saveInfo("WorkEmail")
            workAddress.ContactType = ContactType.WorkLocation
        End If
        workAddress.AddressType = AddressType.WorkLocation
        If (Not String.IsNullOrWhiteSpace(saveInfo("WorkLocationStartDate"))) Then
            workAddress.StartDate = Convert.ToDateTime(saveInfo("WorkLocationStartDate"))
        Else
            workAddress.StartDate = "12/31/9999"
        End If
        If (Not String.IsNullOrWhiteSpace(saveInfo("WorkLocationEndDate"))) Then
            workAddress.EndDate = Convert.ToDateTime(saveInfo("WorkLocationEndDate"))
        Else
            workAddress.EndDate = "12/31/9999"
        End If

        employerInfo.WorkAgencyLocationAddress = workAddress
        Dim supervisorDetails As New AddressControlDetails
        'supervisorDetails.StartDate = Convert.ToDateTime(saveInfo("SupervisorStartDate"))
        'supervisorDetails.EndDate = Convert.ToDateTime(saveInfo("SupervisorEndDate"))
        If (saveInfo.ContainsKey("SupervisorPhoneNumber")) Then
            If (Not String.IsNullOrEmpty(saveInfo("SupervisorPhoneNumber"))) Then
                supervisorDetails.Phone = saveInfo("SupervisorPhoneNumber").Replace("-", "")
                'supervisorDetails.Phone = saveInfo("SupervisorPhoneNumber")
                supervisorDetails.ContactType = ContactType.Supervisor
            End If
        End If

        If (saveInfo.ContainsKey("SupervisorEmail")) Then
            If (Not String.IsNullOrEmpty(saveInfo("SupervisorEmail"))) Then
                supervisorDetails.Email = saveInfo("SupervisorEmail")
                supervisorDetails.ContactType = ContactType.Supervisor
            End If
        End If
        employerInfo.SupervisorPhoneEmail = supervisorDetails
        Dim employerSvc As IEmployerInformationService = StructureMap.ObjectFactory.GetInstance(Of IEmployerInformationService)()
        Dim retObj As ReturnObject(Of Long) = employerSvc.SaveEmployerInformation(employerInfo, SessionHelper.ApplicationID, employerID, flag1, superflag)
        If (employerID = 0 Or superflag) Then
            emprecentID = retObj.ReturnValue
        Else
            emprecentID = employerID
        End If
        If (retObj.Messages.Count > 0) Then
            json = New With {
                .bolVal = retObj.Errors(0),
            .retVal = retObj.ReturnValue
            }
        Else
            json = New With {
               .retVal = retObj.ReturnValue
           }
        End If
        flag1 = False
        superflag = False
        Return json
    End Function
    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        GetRecentlyAddedInfo()
        If (emprecentID > 0) Then
            hdEmployerID.Value = emprecentID
            PopulateRecent(emprecentID)
        End If
    End Sub
    Private Sub PopulateRecent(ByVal employerId As Integer)
        Dim emp As New EmployerInformationDetails
        Dim employerSvc As IEmployerInformationService = StructureMap.ObjectFactory.GetInstance(Of IEmployerInformationService)()
        emp = employerSvc.GetDataRecentlyAddedEmplyerInfo(employerId)
        'txtEmployerAgencyInformation.Value = emp.EmployerName.Trim()
        If (Not String.IsNullOrEmpty(emp.DODDProviderContractNumber)) Then
            txtDODDProvider.Value = emp.DODDProviderContractNumber.Trim()
        Else
            txtDODDProvider.Value = String.Empty
        End If
        txtEmployerName.Value = emp.EmployerName.Trim()
        txtEmployerTaxID.Value = emp.EmployerTaxID.Trim()
        If (emp.EmployerStartDate.ToShortDateString() = "12/31/9999") Then
            txtEmploymentStartDate.Value = String.Empty
        Else
            txtEmploymentStartDate.Value = emp.EmployerStartDate.ToShortDateString()
        End If

        If (emp.EmployerEndDate.ToShortDateString() = "12/31/9999") Then
            txtEmploymentEndDate.Value = String.Empty
        Else
            txtEmploymentEndDate.Value = emp.EmployerEndDate.ToShortDateString()
        End If
        txtCEOLastName.Value = emp.CEOLastName.Trim()
        txtCEOFristName.Value = emp.CEOFirstName.Trim()
        txtCEOMiddleName.Value = emp.CEOMiddleName.Trim()
        txtSupervisorLastName.Value = emp.SupervisorLastName.Trim()
        txtSupervisorFirstName.Value = emp.SupervisorFirstName.Trim()

        If (emp.EndDate.ToShortDateString() = "12/31/9999") Then
            txtSuperVisorEndDate.Value = String.Empty
            txtSuperVisorEndDate.Disabled = False
        Else
            txtSuperVisorEndDate.Value = emp.EndDate.ToShortDateString()
            txtSuperVisorEndDate.Disabled = True
        End If
        If (emp.StartDate.ToShortDateString() = "12/31/9999") Then
            txtSuperVisorStartDate.Value = String.Empty
            txtSuperVisorStartDate.Disabled = False
        Else
            txtSuperVisorStartDate.Value = emp.StartDate.ToShortDateString()
            txtSuperVisorStartDate.Disabled = True
        End If

        If (emp.AgencyPersonalAddressSame) Then
            chkAgencyAddress.Checked = True
        Else
            chkAgencyAddress.Checked = False
        End If
        If (emp.AgencyWorkAddressSame) Then
            chkAgency.Checked = True
        Else
            chkAgency.Checked = False
        End If
        If (emp.AgencyLocationAddress IsNot Nothing And emp.AgencyLocationAddress.AddressLine1 IsNot Nothing) Then
            addrBar.AddressLine1 = emp.AgencyLocationAddress.AddressLine1.Trim()
            addrBar.AddressLine2 = emp.AgencyLocationAddress.AddressLine2.Trim()
            addrBar.City = emp.AgencyLocationAddress.City.Trim()
            If (String.IsNullOrEmpty(emp.AgencyLocationAddress.County.Trim())) Then
                'addrBar.County = "--- County Selection ---"
                addrBar.CountyID = -1
            Else
                addrBar.CountyID = emp.AgencyLocationAddress.CountyID
                'addrBar.County = emp.AgencyLocationAddress.County.Trim()
            End If
            addrBar.StateID = emp.AgencyLocationAddress.StateID
            'addrBar.State = emp.AgencyLocationAddress.State.Trim()
            addrBar.Zip = emp.AgencyLocationAddress.Zip.Trim()
            addrBar.ZipPlus = Trim(emp.AgencyLocationAddress.ZipPlus)
            If (Not String.IsNullOrEmpty(emp.AgencyLocationAddress.Phone)) Then
                addrBar.PhoneNumber = emp.AgencyLocationAddress.Phone.Trim()
            Else
                addrBar.PhoneNumber = String.Empty
            End If
            If (Not String.IsNullOrEmpty(emp.AgencyLocationAddress.Email)) Then
                addrBar.Email = emp.AgencyLocationAddress.Email.Trim()
            Else
                addrBar.Email = String.Empty
            End If
        Else
            addrBar.AddressLine1 = String.Empty
            addrBar.AddressLine2 = String.Empty
            addrBar.City = String.Empty
            addrBar.CountyID = -1
            addrBar.StateID = 35
            'addrBar.County = "--- County Selection ---"
            'addrBar.State = "OH"
            addrBar.Zip = String.Empty
            addrBar.ZipPlus = String.Empty
            addrBar.PhoneNumber = String.Empty
            addrBar.Email = String.Empty
        End If
        If (emp.WorkAgencyLocationAddress IsNot Nothing And emp.WorkAgencyLocationAddress.AddressLine1 IsNot Nothing) Then
            addrBar1.AddressLine1 = emp.WorkAgencyLocationAddress.AddressLine1.Trim()
            addrBar1.AddressLine2 = emp.WorkAgencyLocationAddress.AddressLine2.Trim()
            addrBar1.City = emp.WorkAgencyLocationAddress.City.Trim()
            If (String.IsNullOrEmpty(emp.WorkAgencyLocationAddress.County.Trim())) Then
                'addrBar1.County = "--- County Selection ---"
                addrBar1.CountyID = -1
            Else
                addrBar1.CountyID = emp.WorkAgencyLocationAddress.CountyID
                'addrBar1.County = emp.WorkAgencyLocationAddress.County.Trim()
            End If
            addrBar1.StateID = emp.WorkAgencyLocationAddress.StateID
            'addrBar1.State = emp.WorkAgencyLocationAddress.State.Trim()
            addrBar1.Zip = emp.WorkAgencyLocationAddress.Zip.Trim()
            addrBar1.ZipPlus = Trim(emp.WorkAgencyLocationAddress.ZipPlus)
            If (emp.WorkAgencyLocationAddress.EndDate.ToShortDateString() = "12/31/9999") Then
                txtWorkLocationEndDate.Value = String.Empty
                txtWorkLocationEndDate.Disabled = False
            Else
                txtWorkLocationEndDate.Value = emp.WorkAgencyLocationAddress.EndDate.ToShortDateString()
                txtWorkLocationEndDate.Disabled = True
            End If
            If (emp.WorkAgencyLocationAddress.StartDate.ToShortDateString() = "12/31/9999") Then
                txtWorkLocationStartDate.Value = String.Empty
                txtWorkLocationStartDate.Disabled = False
            Else
                txtWorkLocationStartDate.Value = emp.WorkAgencyLocationAddress.StartDate.ToShortDateString()
                txtWorkLocationStartDate.Disabled = True
            End If

            If (Not String.IsNullOrEmpty(emp.WorkAgencyLocationAddress.Phone)) Then
                addrBar1.PhoneNumber = emp.WorkAgencyLocationAddress.Phone.Trim()
            Else
                addrBar1.PhoneNumber = String.Empty
            End If
            If (Not String.IsNullOrEmpty(emp.WorkAgencyLocationAddress.Email)) Then
                addrBar1.Email = emp.WorkAgencyLocationAddress.Email.Trim()
            Else
                addrBar1.Email = String.Empty
            End If
        Else
            addrBar1.AddressLine1 = String.Empty
            addrBar1.AddressLine2 = String.Empty
            addrBar1.City = String.Empty
            'addrBar1.County = "--- County Selection ---"
            addrBar1.CountyID = -1
            addrBar1.StateID = 35
            'addrBar1.State = "OH"
            addrBar1.Zip = String.Empty
            addrBar1.ZipPlus = String.Empty
            addrBar1.PhoneNumber = String.Empty
            addrBar1.Email = String.Empty
            txtWorkLocationEndDate.Value = String.Empty
            txtWorkLocationStartDate.Value = String.Empty
        End If
        If (Not String.IsNullOrEmpty(emp.SupervisorPhoneEmail.Phone)) Then
            txtSuperVisorPhoneNumber.Value = emp.SupervisorPhoneEmail.Phone.Trim()
        Else
            txtSuperVisorPhoneNumber.Value = String.Empty
        End If
        If (Not String.IsNullOrEmpty(emp.SupervisorPhoneEmail.Email)) Then
            txtSuperVisorEmail.Value = emp.SupervisorPhoneEmail.Email.Trim()
        Else
            txtSuperVisorEmail.Value = String.Empty
        End If
        If SessionHelper.RN_Flg = False Then
            rblSelect.Items(0).Enabled = True
            rblSelect.Items(1).Enabled = True
            rblSelect.Items(2).Enabled = True
            rblSelect.Items(3).Enabled = False
            rblSelect.Items(4).Enabled = False
        Else
            rblSelect.Items(0).Enabled = False
            rblSelect.Items(1).Enabled = True
            rblSelect.Items(2).Enabled = True
            rblSelect.Items(3).Enabled = True
            rblSelect.Items(4).Enabled = True
            'If (SessionHelper.SelectedUserRole = Enums.RoleLevelCategory.QARN_RLC) Then
            '    rblSelect.Items(0).Enabled = False
            '    rblSelect.Items(1).Enabled = True
            '    rblSelect.Items(2).Enabled = False
            '    rblSelect.Items(3).Enabled = False
            'Else
            '    rblSelect.Items(0).Enabled = False
            '    rblSelect.Items(1).Enabled = True
            '    rblSelect.Items(2).Enabled = True
            '    rblSelect.Items(3).Enabled = True
            'End If
        End If
        addrBar1.HideAddress1 = True
        addrBar1.HideAddress2 = True
        addrBar1.HideCity = True
        addrBar1.EnableState = False
        addrBar1.HideCounty = False
        addrBar1.HideZip = True
        addrBar1.HideZipPlus = True
        addrBar1.DisablePhone = True
        addrBar1.DisableEmail = True
        chkAgency.Disabled = False
        txtEmploymentEndDate.Disabled = False
        txtEmploymentStartDate.Disabled = False
        txtSuperVisorEmail.Disabled = False
        txtSupervisorFirstName.Disabled = False
        txtSupervisorLastName.Disabled = False
        txtSuperVisorPhoneNumber.Disabled = False
        If (emp.EmployerTypeID = 1) Then
            rblSelect.Items(3).Selected = True
            rblSelect.Items(1).Selected = False
            rblSelect.Items(0).Selected = False
            rblSelect.Items(2).Selected = False
            rblSelect.Items(4).Selected = False
            chkAgencyAddress.Disabled = False
            pnlEmployer.Enabled = False
            txtDODDProvider.Disabled = True
            btnSearch.Disabled = True
            addrBar.HideAddress1 = True
            addrBar.HideAddress2 = True
            addrBar.HideCity = True
            addrBar.EnableState = False
            addrBar.HideCounty = False
            addrBar.HideZip = True
            addrBar.HideZipPlus = True
            addrBar.DisablePhone = True
            addrBar.DisableEmail = True
            txtEmployerName.Disabled = True
            txtEmployerTaxID.Disabled = True
            txtCEOLastName.Disabled = True
            txtCEOFristName.Disabled = True
            txtCEOMiddleName.Disabled = True
            txtCertStartDate.Disabled = True
            txtCertEndDate.Disabled = True
            txtCertStatus.Disabled = True
        ElseIf (emp.EmployerTypeID = 2) Then
            rblSelect.Items(4).Selected = True
            rblSelect.Items(2).Selected = False
            rblSelect.Items(1).Selected = False
            rblSelect.Items(0).Selected = False
            rblSelect.Items(3).Selected = False
            pnlEmployer.Enabled = False
            txtDODDProvider.Disabled = True
            btnSearch.Disabled = True
            addrBar.HideAddress1 = True
            addrBar.HideAddress2 = True
            addrBar.HideCity = True
            addrBar.EnableState = False
            addrBar.HideCounty = False
            addrBar.HideZip = True
            addrBar.HideZipPlus = True
            addrBar.DisablePhone = True
            addrBar.DisableEmail = True
            chkAgencyAddress.Disabled = False
            txtEmployerName.Disabled = False
            txtEmployerTaxID.Disabled = False
            txtCEOLastName.Disabled = False
            txtCEOFristName.Disabled = False
            txtCEOMiddleName.Disabled = False
            txtCertStartDate.Disabled = True
            txtCertEndDate.Disabled = True
            txtCertStatus.Disabled = True
        ElseIf (emp.EmployerTypeID = 3) Then
            rblSelect.Items(0).Selected = True
            rblSelect.Items(3).Selected = False
            rblSelect.Items(2).Selected = False
            rblSelect.Items(1).Selected = False
            rblSelect.Items(4).Selected = False
            chkAgencyAddress.Disabled = True
            pnlEmployer.Enabled = True
            txtDODDProvider.Disabled = True
            btnSearch.Disabled = True
            addrBar.HideAddress1 = False
            addrBar.HideAddress2 = False
            addrBar.HideCity = False
            addrBar.EnableState = True
            addrBar.HideCounty = True
            addrBar.HideZip = False
            addrBar.HideZipPlus = False
            addrBar.DisablePhone = False
            addrBar.DisableEmail = False
            txtEmployerName.Disabled = True
            txtEmployerTaxID.Disabled = True
            txtCEOLastName.Disabled = True
            txtCEOFristName.Disabled = True
            txtCEOMiddleName.Disabled = True
            txtCertStartDate.Disabled = True
            txtCertEndDate.Disabled = True
            txtCertStatus.Disabled = True
        ElseIf (emp.EmployerTypeID = 4) Then
            rblSelect.Items(1).Selected = True
            rblSelect.Items(0).Selected = False
            rblSelect.Items(3).Selected = False
            rblSelect.Items(2).Selected = False
            rblSelect.Items(4).Selected = False
            chkAgencyAddress.Disabled = True
            pnlEmployer.Enabled = True
            txtDODDProvider.Disabled = True
            btnSearch.Disabled = True
            addrBar.HideAddress1 = False
            addrBar.HideAddress2 = False
            addrBar.HideCity = False
            addrBar.EnableState = True
            addrBar.HideCounty = True
            addrBar.HideZip = False
            addrBar.HideZipPlus = False
            addrBar.DisablePhone = False
            addrBar.DisableEmail = False
            txtEmployerName.Disabled = True
            txtEmployerTaxID.Disabled = True
            txtCEOLastName.Disabled = True
            txtCEOFristName.Disabled = True
            txtCEOMiddleName.Disabled = True
            txtCertStartDate.Disabled = True
            txtCertEndDate.Disabled = True
            txtCertStatus.Disabled = True
        ElseIf (emp.EmployerTypeID = 5) Then
            rblSelect.Items(2).Selected = True
            rblSelect.Items(0).Selected = False
            rblSelect.Items(3).Selected = False
            rblSelect.Items(1).Selected = False
            rblSelect.Items(4).Selected = False
            chkAgencyAddress.Disabled = True
            pnlEmployer.Enabled = False
            txtDODDProvider.Disabled = False
            btnSearch.Disabled = False
            addrBar.HideAddress1 = True
            addrBar.HideAddress2 = True
            addrBar.HideCity = True
            addrBar.EnableState = False
            addrBar.HideCounty = False
            addrBar.HideZip = True
            addrBar.HideZipPlus = True
            addrBar.DisablePhone = True
            addrBar.DisableEmail = True
            txtEmployerName.Disabled = False
            txtEmployerTaxID.Disabled = False
            txtCEOLastName.Disabled = False
            txtCEOFristName.Disabled = False
            txtCEOMiddleName.Disabled = False
            txtCertStartDate.Disabled = False
            txtCertEndDate.Disabled = False
            txtCertStatus.Disabled = False
        End If
        If (emp.EmployerIdentificationTypeID = 1) Then
            tdTaxID.InnerText = "Provider#:"
        ElseIf (emp.EmployerIdentificationTypeID = 2) Then
            tdTaxID.InnerText = "Provider#:"
        ElseIf (emp.EmployerIdentificationTypeID = 3) Then
            tdTaxID.InnerText = "RN License#:"
        ElseIf (emp.EmployerIdentificationTypeID = 4) Then
            tdTaxID.InnerText = "Provider#:"
        End If
    End Sub
    Private Sub PopulateTheFields(ByVal employerId As Integer, ByVal type As String)
        If (type = "recent") Then
            PopulateRecent(employerId)
        Else
            PopulateHistory(employerId)
        End If
    End Sub
    Private Sub PopulateHistory(ByVal employerId As String)
        Dim emp As New EmployerInformationDetails
        Dim employerSvc As IEmployerInformationService = StructureMap.ObjectFactory.GetInstance(Of IEmployerInformationService)()
        emp = employerSvc.GetDataHistoryAddedEmplyerInfo(Convert.ToInt32(employerId), SessionHelper.SessionUniqueID, SessionHelper.RN_Flg)
        If (Not String.IsNullOrEmpty(emp.DODDProviderContractNumber)) Then
            txtDODDProvider.Value = emp.DODDProviderContractNumber.Trim()
        Else
            txtDODDProvider.Value = String.Empty
        End If
        txtEmployerName.Value = emp.EmployerName.Trim()
        txtEmployerTaxID.Value = emp.EmployerTaxID.Trim()
        If (emp.EmployerStartDate.ToShortDateString() = "12/31/9999") Then
            txtEmploymentStartDate.Value = String.Empty
        Else
            txtEmploymentStartDate.Value = emp.EmployerStartDate.ToShortDateString()
        End If
        If (emp.EmployerEndDate.ToShortDateString() = "12/31/9999") Then
            txtEmploymentEndDate.Value = String.Empty
        Else
            txtEmploymentEndDate.Value = emp.EmployerEndDate.ToShortDateString()
        End If
        txtCEOLastName.Value = emp.CEOLastName.Trim()
        txtCEOFristName.Value = emp.CEOFirstName.Trim()
        txtCEOMiddleName.Value = emp.CEOMiddleName.Trim()
        txtSupervisorLastName.Value = emp.SupervisorLastName.Trim()
        txtSupervisorFirstName.Value = emp.SupervisorFirstName.Trim()
        If (emp.EndDate.ToShortDateString() = "12/31/9999") Then
            txtSuperVisorEndDate.Value = String.Empty
        Else
            txtSuperVisorEndDate.Value = emp.EndDate.ToShortDateString()
        End If
        If (emp.StartDate.ToShortDateString() = "12/31/9999") Then
            txtSuperVisorStartDate.Value = String.Empty
        Else
            txtSuperVisorStartDate.Value = emp.StartDate.ToShortDateString()
        End If
        If (emp.AgencyPersonalAddressSame) Then
            chkAgencyAddress.Checked = True
        Else
            chkAgencyAddress.Checked = False
        End If
        If (emp.AgencyWorkAddressSame) Then
            chkAgency.Checked = True
        Else
            chkAgency.Checked = False
        End If
        If (emp.AgencyLocationAddress IsNot Nothing And emp.AgencyLocationAddress.AddressLine1 IsNot Nothing) Then
            addrBar.AddressLine1 = emp.AgencyLocationAddress.AddressLine1.Trim()
            addrBar.AddressLine2 = emp.AgencyLocationAddress.AddressLine2.Trim()
            addrBar.City = emp.AgencyLocationAddress.City.Trim()
            If (String.IsNullOrEmpty(emp.AgencyLocationAddress.County.Trim())) Then
                'addrBar.County = "--- County Selection ---"
                addrBar.CountyID = -1
            Else
                addrBar.CountyID = emp.AgencyLocationAddress.CountyID
                'addrBar.County = emp.AgencyLocationAddress.County.Trim()
            End If
            'addrBar.State = emp.AgencyLocationAddress.State.Trim()
            addrBar.StateID = emp.AgencyLocationAddress.StateID
            addrBar.Zip = emp.AgencyLocationAddress.Zip.Trim()
            addrBar.ZipPlus = Trim(emp.AgencyLocationAddress.ZipPlus)
            If (Not String.IsNullOrEmpty(emp.AgencyLocationAddress.Phone)) Then
                addrBar.PhoneNumber = emp.AgencyLocationAddress.Phone.Trim()
            Else
                addrBar.PhoneNumber = String.Empty
            End If
            If (Not String.IsNullOrEmpty(emp.AgencyLocationAddress.Email)) Then
                addrBar.Email = emp.AgencyLocationAddress.Email.Trim()
            Else
                addrBar.Email = String.Empty
            End If
        Else
            addrBar.AddressLine1 = String.Empty
            addrBar.AddressLine2 = String.Empty
            addrBar.City = String.Empty
            'addrBar.County = "--- County Selection ---"
            'addrBar.State = "OH"
            addrBar.Zip = String.Empty
            addrBar.StateID = 35
            addrBar.CountyID = -1
            addrBar.ZipPlus = String.Empty
            addrBar.PhoneNumber = String.Empty
            addrBar.Email = String.Empty
        End If
        If (emp.WorkAgencyLocationAddress IsNot Nothing And emp.WorkAgencyLocationAddress.AddressLine1 IsNot Nothing) Then
            addrBar1.AddressLine1 = emp.WorkAgencyLocationAddress.AddressLine1.Trim()
            addrBar1.AddressLine2 = emp.WorkAgencyLocationAddress.AddressLine2.Trim()
            addrBar1.City = emp.WorkAgencyLocationAddress.City.Trim()
            If (String.IsNullOrEmpty(emp.WorkAgencyLocationAddress.County.Trim())) Then
                'addrBar1.County = "--- County Selection ---"
                addrBar1.CountyID = -1
            Else
                'addrBar1.County = emp.WorkAgencyLocationAddress.County.Trim()
                addrBar1.CountyID = emp.WorkAgencyLocationAddress.CountyID
            End If
            'addrBar1.State = emp.WorkAgencyLocationAddress.State.Trim()
            addrBar1.StateID = emp.WorkAgencyLocationAddress.StateID
            addrBar1.Zip = emp.WorkAgencyLocationAddress.Zip.Trim()
            addrBar1.ZipPlus = Trim(emp.WorkAgencyLocationAddress.ZipPlus)
            If (Not String.IsNullOrEmpty(emp.WorkAgencyLocationAddress.Phone)) Then
                addrBar1.PhoneNumber = emp.WorkAgencyLocationAddress.Phone.Trim()
            Else
                addrBar1.PhoneNumber = String.Empty
            End If
            If (Not String.IsNullOrEmpty(emp.WorkAgencyLocationAddress.Email)) Then
                addrBar1.Email = emp.WorkAgencyLocationAddress.Email.Trim()
            Else
                addrBar1.Email = String.Empty
            End If
            If (emp.WorkAgencyLocationAddress.EndDate.ToShortDateString() = "12/31/9999") Then
                txtWorkLocationEndDate.Value = String.Empty
            Else
                txtWorkLocationEndDate.Value = emp.WorkAgencyLocationAddress.EndDate.ToShortDateString()
            End If
            If (emp.WorkAgencyLocationAddress.StartDate.ToShortDateString() = "12/31/9999") Then
                txtWorkLocationStartDate.Value = String.Empty
            Else
                txtWorkLocationStartDate.Value = emp.WorkAgencyLocationAddress.StartDate.ToShortDateString()
            End If
        Else
            addrBar1.AddressLine1 = String.Empty
            addrBar1.AddressLine2 = String.Empty
            addrBar1.City = String.Empty
            'addrBar1.County = "--- County Selection ---"
            'addrBar1.State = "OH"
            addrBar1.Zip = String.Empty
            addrBar1.ZipPlus = String.Empty
            addrBar1.PhoneNumber = String.Empty
            addrBar1.Email = String.Empty
            txtWorkLocationEndDate.Value = String.Empty
            txtWorkLocationStartDate.Value = String.Empty
        End If
        If (Not String.IsNullOrEmpty(emp.SupervisorPhoneEmail.Phone)) Then
            txtSuperVisorPhoneNumber.Value = emp.SupervisorPhoneEmail.Phone.Trim()
        Else
            txtSuperVisorPhoneNumber.Value = String.Empty
        End If
        If (Not String.IsNullOrEmpty(emp.SupervisorPhoneEmail.Email)) Then
            txtSuperVisorEmail.Value = emp.SupervisorPhoneEmail.Email.Trim()
        Else
            txtSuperVisorEmail.Value = String.Empty
        End If

        rblSelect.Items(0).Enabled = False
        rblSelect.Items(1).Enabled = False
        rblSelect.Items(2).Enabled = False
        rblSelect.Items(3).Enabled = False
        rblSelect.Items(4).Enabled = False
        txtDODDProvider.Disabled = True
        btnSearch.Disabled = True
        txtEmployerName.Disabled = True
        txtEmploymentEndDate.Disabled = True
        txtEmploymentStartDate.Disabled = True
        txtEmployerTaxID.Disabled = True
        txtCEOLastName.Disabled = True
        txtCEOFristName.Disabled = True
        txtCEOMiddleName.Disabled = True
        txtCertStartDate.Disabled = True
        txtCertEndDate.Disabled = True
        txtCertStatus.Disabled = True
        txtSuperVisorEmail.Disabled = True
        txtSupervisorFirstName.Disabled = True
        txtSupervisorLastName.Disabled = True
        txtSuperVisorPhoneNumber.Disabled = True
        txtSuperVisorEndDate.Disabled = True
        txtSuperVisorStartDate.Disabled = True
        txtWorkLocationEndDate.Disabled = True
        txtWorkLocationStartDate.Disabled = True
        chkAgencyAddress.Disabled = True
        chkAgency.Disabled = True
        addrBar.HideAddress1 = False
        addrBar.HideAddress2 = False
        addrBar.HideCity = False
        addrBar.EnableState = True
        addrBar.HideCounty = True
        addrBar.HideZip = False
        addrBar.HideZipPlus = False
        addrBar.DisablePhone = False
        addrBar.DisableEmail = False
        addrBar1.HideAddress1 = False
        addrBar1.HideAddress2 = False
        addrBar1.HideCity = False
        addrBar1.EnableState = True
        addrBar1.HideCounty = True
        addrBar1.HideZip = False
        addrBar1.HideZipPlus = False
        addrBar1.DisablePhone = False
        addrBar1.DisableEmail = False
        If (emp.EmployerTypeID = 1) Then
            rblSelect.Items(3).Selected = True
            rblSelect.Items(1).Selected = False
            rblSelect.Items(0).Selected = False
            rblSelect.Items(4).Selected = False
            rblSelect.Items(2).Selected = False
        ElseIf (emp.EmployerTypeID = 2) Then
            rblSelect.Items(4).Selected = True
            rblSelect.Items(2).Selected = False
            rblSelect.Items(1).Selected = False
            rblSelect.Items(0).Selected = False
            rblSelect.Items(3).Selected = False
        ElseIf (emp.EmployerTypeID = 3) Then
            rblSelect.Items(0).Selected = True
            rblSelect.Items(3).Selected = False
            rblSelect.Items(2).Selected = False
            rblSelect.Items(1).Selected = False
            rblSelect.Items(4).Selected = False
        ElseIf (emp.EmployerTypeID = 4) Then
            rblSelect.Items(1).Selected = True
            rblSelect.Items(0).Selected = False
            rblSelect.Items(3).Selected = False
            rblSelect.Items(2).Selected = False
            rblSelect.Items(4).Selected = False
        ElseIf (emp.EmployerTypeID = 5) Then
            rblSelect.Items(2).Selected = True
            rblSelect.Items(0).Selected = False
            rblSelect.Items(3).Selected = False
            rblSelect.Items(1).Selected = False
            rblSelect.Items(4).Selected = False
        End If
        If (emp.EmployerIdentificationTypeID = 1) Then
            tdTaxID.InnerText = "Provider#:"
        ElseIf (emp.EmployerIdentificationTypeID = 2) Then
            tdTaxID.InnerText = "Provider#:"
        ElseIf (emp.EmployerIdentificationTypeID = 3) Then
            tdTaxID.InnerText = "RN License#:"
        ElseIf (emp.EmployerIdentificationTypeID = 4) Then
            tdTaxID.InnerText = "Provider#:"
        End If
    End Sub
    Protected Sub grdRecent_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdRecent.SelectedIndexChanged
        Dim row As GridViewRow = grdRecent.SelectedRow
        Dim employerId As Integer = Convert.ToInt32(grdRecent.DataKeys(Convert.ToInt32(row.RowIndex)).Value)
        ' Dim employerId As String = row.Cells(6).Text
        hdEmployerID.Value = employerId
        PopulateTheFields(employerId, "recent")
    End Sub
    Protected Sub grdViewHistory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdViewHistory.SelectedIndexChanged
        Dim row As GridViewRow = grdViewHistory.SelectedRow
        Dim employerId As Integer = Convert.ToInt32(grdViewHistory.DataKeys(Convert.ToInt32(row.RowIndex)).Value)
        ' Dim employerId As String = row.Cells(6).Text
        PopulateTheFields(employerId, "history")
    End Sub
    Private Sub ClearDataFromEmp()
        txtDODDProvider.Value = String.Empty
        txtEmployerName.Value = String.Empty
        txtEmployerTaxID.Value = String.Empty
        txtEmploymentStartDate.Value = String.Empty
        txtEmploymentEndDate.Value = String.Empty
        txtCEOLastName.Value = String.Empty
        txtCEOFristName.Value = String.Empty
        txtCEOMiddleName.Value = String.Empty
        txtSupervisorLastName.Value = String.Empty
        txtSupervisorFirstName.Value = String.Empty
        txtSuperVisorEndDate.Value = String.Empty
        txtSuperVisorStartDate.Value = String.Empty
        chkAgencyAddress.Checked = False
        chkAgency.Checked = False
        addrBar1.AddressLine1 = String.Empty
        addrBar1.AddressLine2 = String.Empty
        addrBar1.City = String.Empty
        addrBar1.CountyID = -1
        addrBar1.StateID = 35
        'addrBar1.County = "--- County Selection ---"
        'addrBar1.State = "OH"
        addrBar1.Zip = String.Empty
        addrBar1.ZipPlus = String.Empty
        addrBar1.PhoneNumber = String.Empty
        addrBar1.Email = String.Empty
        addrBar.AddressLine1 = String.Empty
        addrBar.AddressLine2 = String.Empty
        addrBar.City = String.Empty
        addrBar.CountyID = -1
        addrBar.StateID = 35
        'addrBar.County = "--- County Selection ---"
        'addrBar.State = "OH"
        addrBar.Zip = String.Empty
        addrBar.ZipPlus = String.Empty
        addrBar.PhoneNumber = String.Empty
        txtWorkLocationEndDate.Value = String.Empty
        txtWorkLocationStartDate.Value = String.Empty
        addrBar.Email = String.Empty
        If (rblSelect.Items(3).Selected) Then 'Self Employer
            'rblSelect.Items(2).Selected = True
            'rblSelect.Items(1).Selected = False
            'rblSelect.Items(0).Selected = False
            'rblSelect.Items(3).Selected = False
            chkAgencyAddress.Disabled = False
            pnlEmployer.Enabled = False
            'txtEmployerAgencyInformation.Disabled = True
            txtDODDProvider.Disabled = True
            btnSearch.Disabled = True
            addrBar.HideAddress1 = True
            addrBar.HideAddress2 = True
            addrBar.HideCity = True
            addrBar.EnableState = False
            addrBar.HideCounty = False
            addrBar.HideZip = True
            addrBar.HideZipPlus = True
            addrBar.DisablePhone = True
            addrBar.DisableEmail = True
            txtEmployerName.Disabled = True
            txtEmployerTaxID.Disabled = True
            txtCEOLastName.Disabled = True
            txtCEOFristName.Disabled = True
            txtCEOMiddleName.Disabled = True
            txtCertStartDate.Disabled = True
            txtCertEndDate.Disabled = True
            txtCertStatus.Disabled = True

        ElseIf (rblSelect.Items(4).Selected) Then 'Other employer
            pnlEmployer.Enabled = False
            'txtEmployerAgencyInformation.Disabled = True
            txtDODDProvider.Disabled = True
            btnSearch.Disabled = True
            addrBar.HideAddress1 = True
            addrBar.HideAddress2 = True
            addrBar.HideCity = True
            addrBar.EnableState = False
            addrBar.HideCounty = False
            addrBar.HideZip = True
            addrBar.HideZipPlus = True
            addrBar.DisablePhone = True
            addrBar.DisableEmail = True
            chkAgencyAddress.Disabled = False
            txtEmployerName.Disabled = False
            txtEmployerTaxID.Disabled = False
            txtCEOLastName.Disabled = False
            txtCEOFristName.Disabled = False
            txtCEOMiddleName.Disabled = False
            txtCertStartDate.Disabled = True
            txtCertEndDate.Disabled = True
            txtCertStatus.Disabled = True
        ElseIf (rblSelect.Items(1).Selected) Then 'Agency provider
            chkAgencyAddress.Disabled = True
            pnlEmployer.Enabled = True
            'txtEmployerAgencyInformation.Disabled = False
            txtDODDProvider.Disabled = True
            btnSearch.Disabled = True
            addrBar.HideAddress1 = False
            addrBar.HideAddress2 = False
            addrBar.HideCity = False
            addrBar.EnableState = True
            addrBar.HideCounty = True
            addrBar.HideZip = False
            addrBar.HideZipPlus = False
            addrBar.DisablePhone = False
            addrBar.DisableEmail = False
            txtEmployerName.Disabled = True
            txtEmployerTaxID.Disabled = True
            txtCEOLastName.Disabled = True
            txtCEOFristName.Disabled = True
            txtCEOMiddleName.Disabled = True
            txtCertStartDate.Disabled = True
            txtCertEndDate.Disabled = True
            txtCertStatus.Disabled = True
        ElseIf (rblSelect.Items(0).Selected) Then 'Independent provider
            chkAgencyAddress.Disabled = True
            pnlEmployer.Enabled = True
            'txtEmployerAgencyInformation.Disabled = False
            txtDODDProvider.Disabled = True
            btnSearch.Disabled = True
            addrBar.HideAddress1 = False
            addrBar.HideAddress2 = False
            addrBar.HideCity = False
            addrBar.EnableState = True
            addrBar.HideCounty = True
            addrBar.HideZip = False
            addrBar.HideZipPlus = False
            addrBar.DisablePhone = False
            addrBar.DisableEmail = False
            txtEmployerName.Disabled = True
            txtEmployerTaxID.Disabled = True
            txtCEOLastName.Disabled = True
            txtCEOFristName.Disabled = True
            txtCEOMiddleName.Disabled = True
            txtCertStartDate.Disabled = True
            txtCertEndDate.Disabled = True
            txtCertStatus.Disabled = True
        ElseIf (rblSelect.Items(2).Selected) Then 'ICF EMployer type
            chkAgencyAddress.Disabled = True
            pnlEmployer.Enabled = True
            'txtEmployerAgencyInformation.Disabled = False
            txtDODDProvider.Disabled = True
            btnSearch.Disabled = True
            addrBar.HideAddress1 = False
            addrBar.HideAddress2 = False
            addrBar.HideCity = False
            addrBar.EnableState = True
            addrBar.HideCounty = True
            addrBar.HideZip = False
            addrBar.HideZipPlus = False
            addrBar.DisablePhone = False
            addrBar.DisableEmail = False
            txtEmployerName.Disabled = True
            txtEmployerTaxID.Disabled = True
            txtCEOLastName.Disabled = True
            txtCEOFristName.Disabled = True
            txtCEOMiddleName.Disabled = True
            txtCertStartDate.Disabled = True
            txtCertEndDate.Disabled = True
            txtCertStatus.Disabled = True
        End If
        rblSelect.Items(2).Selected = False
        rblSelect.Items(1).Selected = False
        rblSelect.Items(0).Selected = False
        rblSelect.Items(3).Selected = False
        rblSelect.Items(4).Selected = False
    End Sub
    Private Sub grdRecent_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdRecent.RowDeleting
        'flag1 = True
        pnlDD.Enabled = True
        rblSelect.Enabled = True
        Dim empId As Integer = Convert.ToInt32(grdRecent.DataKeys(Convert.ToInt32(e.RowIndex)).Value)      
        'Dim identityValue As String = grdRecent.Rows(e.RowIndex).Cells(10).Text
        ' Dim employerName As String = grdRecent.Rows(e.RowIndex).Cells(0).Text
        Dim employerSvc As IEmployerInformationService = StructureMap.ObjectFactory.GetInstance(Of IEmployerInformationService)()
        Dim flag As Boolean = employerSvc.DeleteEmployerInfo(empId, SessionHelper.ApplicationID)
        GetRecentlyAddedInfo()
        ClearDataFromEmp()
        btnSave.Enabled = True
        btnSaveAdditional.Enabled = True

    End Sub

    Private Sub grdRecent_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdRecent.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lnkAct As LinkButton = CType(e.Row.FindControl("lnkAction"), LinkButton)
            Dim lnkWorkAct As LinkButton = CType(e.Row.FindControl("lnkWorkAction"), LinkButton)
            Dim lnkSelect As LinkButton = CType(e.Row.FindControl("lnkBtnSelect"), LinkButton)
            Dim lnkUpdate As LinkButton = CType(e.Row.FindControl("lnkUpdateInfo"), LinkButton)

            If (ViewState("Pending_Employer_Info_Flg") = True) Then
                lnkAct.Enabled = False
                lnkWorkAct.Enabled = False
                lnkSelect.Enabled = False
            Else
                lnkSelect.Enabled = True
                If (e.Row.Cells(6).Text <> "12/31/9999") Then
                    lnkAct.Enabled = True
                Else
                    lnkAct.Enabled = False
                End If
                If (e.Row.Cells(7).Text <> "12/31/9999") Then
                    lnkWorkAct.Enabled = True
                Else
                    lnkWorkAct.Enabled = False
                End If
            End If

            If (e.Row.DataItem.Pending_Information_Flg = True) Then
                lnkUpdate.Enabled = True
            Else
                lnkUpdate.Enabled = False
                lnkUpdate.Text = String.Empty
            End If
        End If
    End Sub
    Private Sub grdRecent_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdRecent.RowCommand
      
        If (e.CommandName = "DesiredSuperVisorOptions") Then
            'If (addrBar1.AddressLine1 <> String.Empty) Then     
            Dim gvRow As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            Dim employerId As Integer = Convert.ToInt32(grdRecent.DataKeys(Convert.ToInt32(gvRow.RowIndex)).Value)

            ' Dim employerId As String = grdRecent.Rows(gvRow.RowIndex).Cells(6).Text.Trim()
            hdEmployerID.Value = employerId
            PopulateTheFields(employerId, "recent")
            'End If
            txtSupervisorLastName.Value = String.Empty
            txtSupervisorFirstName.Value = String.Empty
            txtSuperVisorPhoneNumber.Value = String.Empty
            txtSuperVisorEmail.Value = String.Empty
            txtSuperVisorStartDate.Value = String.Empty
            txtSuperVisorStartDate.Disabled = False
            txtSuperVisorEndDate.Value = String.Empty
            txtSuperVisorEndDate.Disabled = False
            'hdEmployerID.Value = 0
            superflag = True
            flag1 = False
        End If
        If (e.CommandName = "DesiredWorkLocationOptions") Then
          
            'If (txtSupervisorFirstName.Value <> String.Empty) Then          
            ' Dim employerId As String = grdRecent.Rows(gvRow.RowIndex).Cells(6).Text.Trim()
            Dim gvRow As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            Dim employerId As Integer = Convert.ToInt32(grdRecent.DataKeys(Convert.ToInt32(gvRow.RowIndex)).Value)

            hdEmployerID.Value = employerId
            PopulateTheFields(employerId, "recent")
            'End If
            addrBar1.AddressLine1 = String.Empty
            addrBar1.AddressLine2 = String.Empty
            addrBar1.City = String.Empty
            addrBar1.StateID = 35
            addrBar1.CountyID = -1
            'addrBar1.State = "OH"
            'addrBar1.County = "--- County Selection ---"
            addrBar1.Zip = String.Empty
            addrBar1.ZipPlus = String.Empty
            addrBar1.PhoneNumber = String.Empty
            addrBar1.Email = String.Empty
            txtWorkLocationStartDate.Value = String.Empty
            txtWorkLocationStartDate.Disabled = False
            txtWorkLocationEndDate.Value = String.Empty
            txtWorkLocationEndDate.Disabled = False
            chkAgency.Checked = False
            flag1 = True
            superflag = False
            'hdEmployerID.Value = 0
        End If
        If (e.CommandName = "PendingEmployer") Then
            Dim gvRow As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            Dim employerId As Integer = Convert.ToInt32(grdRecent.DataKeys(Convert.ToInt32(gvRow.RowIndex)).Value)

            hdEmployerID.Value = employerId
            btnSave.Enabled = True
            PopulateRecent(employerId)
            superflag = False
            flag1 = False
        End If
    End Sub
    Protected Sub btnSaveAdditional_Click(sender As Object, e As EventArgs) Handles btnSaveAdditional.Click
        GetRecentlyAddedInfo()
    End Sub
    Protected Sub btnContinue_Click(sender As Object, e As EventArgs) Handles btnSaveAndContinue.Click
        Response.Redirect(PagesHelper.GetNextPage(Master.CurrentPage))
    End Sub
    'Protected Sub lnkBtnRemove_Click(sender As Object, e As EventArgs)
    '    Dim linkButton As LinkButton = DirectCast(sender, LinkButton)
    '    Dim row As GridViewRow = linkButton.Parent.Parent
    '    Dim empId As Integer = row.Cells(6).Text
    '    Dim employerSvc As IEmployerInformationService = StructureMap.ObjectFactory.GetInstance(Of IEmployerInformationService)()
    '    Dim flag As Boolean = employerSvc.DeleteEmployerInfo(empId, SessionHelper.ApplicationID)
    '    GetRecentlyAddedInfo()
    'End Sub
End Class