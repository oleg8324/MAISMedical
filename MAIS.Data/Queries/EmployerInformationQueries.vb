Imports ODMRDDHelperClassLibrary.Utility
Imports System.Configuration
Imports System.Data.Objects
Imports System.Data.Entity.Validation
Imports MAIS.Data.Objects

Namespace Queries
    Public Interface IEmployerInformationQueries
        Inherits IQueriesBase
        Function GetRecentlyAddedEmplyeInfo(ByVal applicationID As Integer) As List(Of EmployerInformationDetails)
        Function GetDataRecentlyAddedEmplyerInfo(ByVal employerID As Integer) As EmployerInformationDetails
        Function GetDataHistoryAddedEmplyerInfo(ByVal employerID As Integer, ByVal UniqueCode As String, ByVal RN_Flg As Boolean) As EmployerInformationDetails
        Function SaveEmployerInformation(ByVal employerInfo As Objects.EmployerInformationDetails, ByVal applicationID As Integer, ByVal employerId As Integer, ByVal flag As Boolean, ByVal supflag As Boolean) As ReturnObject(Of Long)
        Function GetAddedEmpIDs(ByVal applicationID As Integer) As List(Of Integer)
        Function DeleteEmployerInfo(ByVal employerId As Integer, ByVal applicationId As Integer) As Boolean
        Function GetEmployerPageComplete(applicationId As Integer, ByVal RNDDUniqueCode As String) As Integer
        Function GetEmployerInformationFromPerm(ByVal UniqueCode As String, ByVal RN_Flg As Boolean) As List(Of EmployerInformationDetails)
        Function GetEmployerInformationWithAddressFromPerm(ByVal UniqueCode As String, ByVal RN_Flg As Boolean) As List(Of EmployerInformationDetails)
    End Interface
    Public Class EmployerInformationQueries
        Inherits QueriesBase
        Implements IEmployerInformationQueries
        Public Function GetEmployerInformationFromPerm(ByVal UniqueCode As String, ByVal RN_Flg As Boolean) As List(Of EmployerInformationDetails) Implements IEmployerInformationQueries.GetEmployerInformationFromPerm
            Dim empInfo As New List(Of EmployerInformationDetails)
            Try
                Using context As New MAISContext
                    If RN_Flg Then
                        empInfo = (From r In context.RNs
                                   Join rp In context.RN_DD_Person_Type_Xref On r.RN_Sid Equals rp.RN_DDPersonnel_Sid
                                   Join ep In context.Employer_RN_DD_Person_Type_Xref On ep.RN_DD_Person_Type_Xref_Sid Equals rp.RN_DD_Person_Type_Xref_Sid
                                   Join e In context.Employers On e.Employer_Sid Equals ep.Employer_Sid
                                   Join ety In context.Employer_Type On ep.Employer_Type_Sid Equals ety.Employer_Type_Sid
                                   Where r.RNLicense_Number = UniqueCode AndAlso rp.Person_Type_Sid = 1
                                   Select New Objects.EmployerInformationDetails() With {
                                       .EmployerName = e.Employer_Name,
                                       .EmployerEndDate = ep.Employment_End_Date,
                                       .EmployerStartDate = ep.Employment_Start_Date,
                                       .CEOFirstName = ep.CEO_First_Name,
                                       .CEOMiddleName = ep.CEO_Middle_Name,
                                       .CEOLastName = ep.CEO_Last_Name,
                                       .EmployerTypeID = ep.Employer_Type_Sid,
                                       .EmployerType = ety.Employer_Desc,
                                       .DODDProviderContractNumber = ep.Provider_Contract_Number,
                                       .IdentitficationValue = e.Identification_Value
                                       }).Distinct.ToList()
                    Else
                        empInfo = (From dd In context.DDPersonnels
                                   Join rp In context.RN_DD_Person_Type_Xref On dd.DDPersonnel_Sid Equals rp.RN_DDPersonnel_Sid
                                   Join ep In context.Employer_RN_DD_Person_Type_Xref On ep.RN_DD_Person_Type_Xref_Sid Equals rp.RN_DD_Person_Type_Xref_Sid
                                   Join e In context.Employers On e.Employer_Sid Equals ep.Employer_Sid
                                   Join ety In context.Employer_Type On ep.Employer_Type_Sid Equals ety.Employer_Type_Sid
                                   Where dd.DDPersonnel_Code = UniqueCode AndAlso rp.Person_Type_Sid = 2
                                   Select New Objects.EmployerInformationDetails() With {
                                       .EmployerName = e.Employer_Name,
                                       .EmployerEndDate = ep.Employment_End_Date,
                                       .EmployerStartDate = ep.Employment_Start_Date,
                                       .CEOFirstName = ep.CEO_First_Name,
                                       .CEOMiddleName = ep.CEO_Middle_Name,
                                       .CEOLastName = ep.CEO_Last_Name,
                                       .EmployerTypeID = ep.Employer_Type_Sid,
                                       .EmployerType = ety.Employer_Desc,
                                       .DODDProviderContractNumber = ep.Provider_Contract_Number,
                                       .IdentitficationValue = e.Identification_Value
                                       }).Distinct.ToList()
                    End If
                    For Each employerDetails As Objects.EmployerInformationDetails In empInfo
                        employerDetails.SupervisorFirstName = (From emp In context.Employer_RN_DD_Person_Type_Xref
                                                               Join e In context.Employers On e.Employer_Sid Equals emp.Employer_Sid
                                                               Where e.Employer_Name = employerDetails.EmployerName And e.Identification_Value = employerDetails.IdentitficationValue _
                                                               Order By emp.Last_Update_Date Descending Select emp.Supervisor_First_Name).FirstOrDefault()
                        employerDetails.SupervisorLastName = (From emp In context.Employer_RN_DD_Person_Type_Xref
                                                              Join e In context.Employers On e.Employer_Sid Equals emp.Employer_Sid
                                                              Where e.Employer_Name = employerDetails.EmployerName And e.Identification_Value = employerDetails.IdentitficationValue _
                                                              Order By emp.Last_Update_Date Descending Select emp.Supervisor_Last_Name).FirstOrDefault()
                        employerDetails.EmployerID = (From emp In context.Employer_RN_DD_Person_Type_Xref
                                                      Join e In context.Employers On e.Employer_Sid Equals emp.Employer_Sid
                                                      Where e.Employer_Name = employerDetails.EmployerName And e.Identification_Value = employerDetails.IdentitficationValue _
                                                      Order By emp.Last_Update_Date Descending Select e.Employer_Sid).FirstOrDefault()

                    Next
                End Using
            Catch ex As Exception
                Me.LogError("Error Getting employer information from permanent", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error Getting employer information from permanent.", True, False))
                Throw
            End Try
            Return empInfo
        End Function
        Public Function GetRecentlyAddedEmplyeInfo(ByVal applicationID As Integer) As List(Of EmployerInformationDetails) Implements IEmployerInformationQueries.GetRecentlyAddedEmplyeInfo
            Dim recentlyAddedEmployer As New List(Of Objects.EmployerInformationDetails)
            Using context As New MAISContext()
                Try
                    recentlyAddedEmployer = (From emp In context.Application_Employer _
                                             Where emp.Application_Sid = applicationID Order By emp.Last_Update_Date Descending
                                             Select New Objects.EmployerInformationDetails() With {
                                                 .CEOFirstName = emp.CEO_First_Name,
                                                 .CEOLastName = emp.CEO_Last_Name,
                                                 .CEOMiddleName = emp.CEO_Middle_Name,
                                                 .EmployerName = emp.Employer_Name,
                                                 .ApplicationSID = emp.Application_Sid,
                                                 .IdentitficationValue = emp.Identification_Value,
                                                  .DODDProviderContractNumber = emp.Provider_Contract_Number,
                                                 .Pending_Information_Flg = emp.Pending_Information_Flg
                                                 }).Distinct.ToList()
                    For Each employerDetails As Objects.EmployerInformationDetails In recentlyAddedEmployer
                        employerDetails.SupervisorFirstName = (From emp In context.Application_Employer Where emp.Employer_Name = employerDetails.EmployerName And emp.Identification_Value = employerDetails.IdentitficationValue _
                                                             And emp.Application_Sid = employerDetails.ApplicationSID Order By emp.Last_Update_Date Descending Select emp.Supervisor_First_Name).FirstOrDefault()
                        employerDetails.SupervisorLastName = (From emp In context.Application_Employer Where emp.Employer_Name = employerDetails.EmployerName And emp.Identification_Value = employerDetails.IdentitficationValue _
                                                             And emp.Application_Sid = employerDetails.ApplicationSID Order By emp.Last_Update_Date Descending Select emp.Supervisor_Last_Name).FirstOrDefault()
                        employerDetails.CurrentSupervisor = (From emp In context.Application_Employer Where emp.Employer_Name = employerDetails.EmployerName And emp.Identification_Value = employerDetails.IdentitficationValue _
                                                             And emp.Application_Sid = employerDetails.ApplicationSID Order By emp.Last_Update_Date Descending Select emp.Supervisor_End_date).FirstOrDefault()
                        employerDetails.CurrentWorkLocation = (From emp In context.Application_Employer
                                                            Join workLocation In context.Application_Employer_Address_Xref On workLocation.Application_Employer_Sid Equals emp.Application_Employer_Sid
                                                            Where emp.Employer_Name = employerDetails.EmployerName And emp.Identification_Value = employerDetails.IdentitficationValue _
                                                             And emp.Application_Sid = employerDetails.ApplicationSID And workLocation.Address_Type_Sid = 4
                                                            Order By workLocation.Last_Update_Date Descending Select workLocation.Agency_Work_Location_End_date).FirstOrDefault()
                        employerDetails.EmployerID = (From emp In context.Application_Employer Where emp.Employer_Name = employerDetails.EmployerName And emp.Identification_Value = employerDetails.IdentitficationValue _
                                                             And emp.Application_Sid = employerDetails.ApplicationSID Order By emp.Last_Update_Date Descending Select emp.Application_Employer_Sid).FirstOrDefault()

                    Next
                Catch ex As Exception
                    Me.LogError("Error Getting recent employer Info", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting recent employer Info.", True, False))
                    Throw
                End Try
            End Using
            Return recentlyAddedEmployer
        End Function
        Public Function GetAddedEmpIDs(ByVal applicationID As Integer) As List(Of Integer) Implements IEmployerInformationQueries.GetAddedEmpIDs
            Dim ids As New List(Of Integer)
            Using context As New MAISContext()
                Try
                    ids = (From emp In context.Application_Employer _
                                             Where emp.Application_Sid = applicationID AndAlso emp.Pending_Information_Flg = False
                                             Select emp.Application_Employer_Sid).ToList()

                Catch ex As Exception
                    Me.LogError("Error Getting recent employer Info", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting recent employer Info.", True, False))
                    Throw
                End Try
            End Using
            Return ids
        End Function
        Public Function GetDataRecentlyAddedEmplyerInfo(employerID As Integer) As EmployerInformationDetails Implements IEmployerInformationQueries.GetDataRecentlyAddedEmplyerInfo
            Dim employerData As New Objects.EmployerInformationDetails
            Dim agencyLocationAddress As New Objects.AddressControlDetails
            Dim workLocationAddress As New Objects.AddressControlDetails
            Dim SupervisorLocationAddress As New Objects.AddressControlDetails
            Using context As New MAISContext()
                Try
                    employerData = (From emp In context.Application_Employer _
                                    Where emp.Application_Employer_Sid = employerID Order By emp.Last_Update_Date Descending
                                    Select New Objects.EmployerInformationDetails() With {
                                        .AgencyPersonalAddressSame = emp.Personal_Mailing_Address_Different_Flg,
                                        .AgencyWorkAddressSame = emp.Work_Address_Same_As_Agency_Flg,
                                        .CEOFirstName = emp.CEO_First_Name,
                                        .CEOLastName = emp.CEO_Last_Name,
                                        .CEOMiddleName = emp.CEO_Middle_Name,
                                        .DODDProviderContractNumber = emp.Provider_Contract_Number,
                                        .EmployerEndDate = emp.Employment_End_Date,
                                        .EmployerStartDate = emp.Employment_Start_Date,
                                        .EmployerID = emp.Application_Employer_Sid,
                                        .EmployerName = emp.Employer_Name,
                                        .EmployerTypeID = emp.Employer_Type_Sid,
                                        .EmployerIdentificationTypeID = emp.Identification_Type_Sid,
                                        .EmployerTaxID = emp.Identification_Value,
                                        .SupervisorFirstName = emp.Supervisor_First_Name,
                                        .SupervisorLastName = emp.Supervisor_Last_Name,
                            .StartDate = emp.Supervisor_Start_date,
                            .EndDate = emp.Supervisor_End_date
                                        }).FirstOrDefault()
                    agencyLocationAddress = (From addrReference In context.Application_Employer_Address_Xref
                    Where addrReference.Application_Employer_Sid = employerID And addrReference.Active_Flg = True And addrReference.Address_Type_Sid = 3 Order By addrReference.Last_Update_Date Descending
                    Select New Objects.AddressControlDetails() With {
                        .AddressSID = addrReference.Address_Sid,
                        .AddressType = addrReference.Address_Type_Sid
                        }).FirstOrDefault()
                    If (agencyLocationAddress IsNot Nothing) Then
                        If (agencyLocationAddress.AddressSID > 0) Then
                            Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", agencyLocationAddress.AddressSID)
                            Dim address As List(Of Address_Lookup_And_Insert_Result) = context.Address_Lookup_And_Insert(parameter, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).ToList()
                            agencyLocationAddress.AddressLine1 = address(0).Address_Line1
                            agencyLocationAddress.AddressLine2 = address(0).Address_Line2
                            agencyLocationAddress.City = address(0).City
                            agencyLocationAddress.County = address(0).County_Desc
                            agencyLocationAddress.State = address(0).State_Abbr
                            agencyLocationAddress.Zip = address(0).Zip
                            agencyLocationAddress.City = address(0).City
                            agencyLocationAddress.CountyID = address(0).CountyID
                            agencyLocationAddress.StateID = address(0).StateID
                        End If
                    End If
                    If (agencyLocationAddress IsNot Nothing) Then
                        agencyLocationAddress.Phone = (From phone In context.Phone_Number _
                            Join phoneRef In context.Application_Employer_Phone_Xref On phoneRef.Phone_Sid Equals phone.Phone_Number_SID
                            Where phoneRef.Active_Flg = True And phoneRef.Contact_Type_Sid = 4 And phoneRef.Application_Employer_Sid = employerID
                            Select phone.Phone_Number1).FirstOrDefault()
                        agencyLocationAddress.Email = (From phone In context.Email1 _
                            Join phoneRef In context.Application_Employer_Email_Xref On phoneRef.Email_Sid Equals phone.Email_SID
                            Where phoneRef.Active_Flg = True And phoneRef.Contact_Type_Sid = 4 And phoneRef.Application_Employer_Sid = employerID
                            Select phone.Email_Address).FirstOrDefault()
                    End If
                    workLocationAddress = (From addr In context.Application_Employer_Address_Xref
                    Where addr.Application_Employer_Sid = employerID And addr.Active_Flg = True And addr.Address_Type_Sid = 4 Order By addr.Last_Update_Date Descending
                    Select New Objects.AddressControlDetails() With {
                        .AddressSID = addr.Address_Sid,
                    .StartDate = addr.Agency_Work_Location_Start_date,
                    .EndDate = addr.Agency_Work_Location_End_date,
                        .AddressType = addr.Address_Type_Sid
                        }).FirstOrDefault()
                    If (workLocationAddress IsNot Nothing) Then
                        If (workLocationAddress.AddressSID > 0) Then
                            Dim workparameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", workLocationAddress.AddressSID)
                            Dim address As List(Of Address_Lookup_And_Insert_Result) = context.Address_Lookup_And_Insert(workparameter, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).ToList()
                            workLocationAddress.AddressLine1 = address(0).Address_Line1
                            workLocationAddress.AddressLine2 = address(0).Address_Line2
                            workLocationAddress.City = address(0).City
                            workLocationAddress.County = address(0).County_Desc
                            workLocationAddress.State = address(0).State_Abbr
                            workLocationAddress.Zip = address(0).Zip
                            workLocationAddress.City = address(0).City
                            workLocationAddress.CountyID = address(0).CountyID
                            workLocationAddress.StateID = address(0).StateID
                        End If
                    End If
                    If (workLocationAddress IsNot Nothing) Then
                        workLocationAddress.Phone = (From phone In context.Phone_Number _
                            Join phoneRef In context.Application_Employer_Phone_Xref On phoneRef.Phone_Sid Equals phone.Phone_Number_SID
                            Where phoneRef.Active_Flg = True And phoneRef.Contact_Type_Sid = 5 And phoneRef.Application_Employer_Sid = employerID
                            Select phone.Phone_Number1).FirstOrDefault()
                        workLocationAddress.Email = (From phone In context.Email1 _
                            Join phoneRef In context.Application_Employer_Email_Xref On phoneRef.Email_Sid Equals phone.Email_SID
                            Where phoneRef.Active_Flg = True And phoneRef.Contact_Type_Sid = 5 And phoneRef.Application_Employer_Sid = employerID
                            Select phone.Email_Address).FirstOrDefault()
                    End If
                    SupervisorLocationAddress.Phone = (From phone In context.Phone_Number _
                        Join phoneRef In context.Application_Employer_Phone_Xref On phoneRef.Phone_Sid Equals phone.Phone_Number_SID
                        Where phoneRef.Active_Flg = True And phoneRef.Contact_Type_Sid = 7 And phoneRef.Application_Employer_Sid = employerID
                        Select phone.Phone_Number1).FirstOrDefault()
                    SupervisorLocationAddress.Email = (From phone In context.Email1 _
                        Join phoneRef In context.Application_Employer_Email_Xref On phoneRef.Email_Sid Equals phone.Email_SID
                        Where phoneRef.Active_Flg = True And phoneRef.Contact_Type_Sid = 7 And phoneRef.Application_Employer_Sid = employerID
                        Select phone.Email_Address).FirstOrDefault()
                    employerData.AgencyLocationAddress = agencyLocationAddress
                    employerData.WorkAgencyLocationAddress = workLocationAddress
                    employerData.SupervisorPhoneEmail = SupervisorLocationAddress
                Catch ex As Exception
                    Me.LogError("Error Getting recent data employer Info", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting recent data employer Info.", True, False))
                    Throw
                End Try
            End Using
            Return employerData
        End Function
        Public Function SaveEmployerInformation(ByVal employerInfo As Objects.EmployerInformationDetails, ByVal applicationID As Integer, ByVal employerId As Integer, ByVal flag As Boolean, ByVal supflag As Boolean) As ReturnObject(Of Long) Implements IEmployerInformationQueries.SaveEmployerInformation
            Dim retval As New ReturnObject(Of Long)(-1L)
            Dim Pendeing_Information_flg = True
            Using context As New MAISContext()
                Try
                    If (employerInfo IsNot Nothing) Then
                        If ((applicationID > 0) And
                           (Not String.IsNullOrWhiteSpace(employerInfo.CEOFirstName)) And
                           (Not String.IsNullOrWhiteSpace(employerInfo.CEOLastName)) And
                            (Not String.IsNullOrWhiteSpace(employerInfo.EmployerName)) And
                            (If(employerInfo.EmployerStartDate = "12/31/9999", False, True)) And
                            (Not String.IsNullOrWhiteSpace(employerInfo.EmployerTaxID)) And
                            (If(employerInfo.StartDate = "12/31/9999", False, True)) And
                            (Not String.IsNullOrWhiteSpace(employerInfo.SupervisorFirstName)) And
                             (Not String.IsNullOrWhiteSpace(employerInfo.SupervisorLastName)) And
                             (Not String.IsNullOrWhiteSpace(employerInfo.AgencyLocationAddress.AddressLine1)) And
                               (Not String.IsNullOrWhiteSpace(employerInfo.AgencyLocationAddress.City)) And
                               (Not String.IsNullOrWhiteSpace(employerInfo.AgencyLocationAddress.County)) And
                               (Not String.IsNullOrWhiteSpace(employerInfo.AgencyLocationAddress.Zip)) And
                            (Not String.IsNullOrWhiteSpace(employerInfo.WorkAgencyLocationAddress.AddressLine1)) And
                             (Not String.IsNullOrWhiteSpace(employerInfo.WorkAgencyLocationAddress.City)) And
                             (Not String.IsNullOrWhiteSpace(employerInfo.WorkAgencyLocationAddress.County)) And
                             (Not String.IsNullOrWhiteSpace(employerInfo.WorkAgencyLocationAddress.Zip)) And
                            (Not String.IsNullOrWhiteSpace(employerInfo.AgencyLocationAddress.Phone)) And
                            (Not String.IsNullOrWhiteSpace(employerInfo.AgencyLocationAddress.Email)) And
                            (Not String.IsNullOrWhiteSpace(employerInfo.WorkAgencyLocationAddress.Phone)) And
                            (Not String.IsNullOrWhiteSpace(employerInfo.WorkAgencyLocationAddress.Email)) And
                            (Not String.IsNullOrWhiteSpace(employerInfo.SupervisorPhoneEmail.Phone)) And
                            (Not String.IsNullOrWhiteSpace(employerInfo.SupervisorPhoneEmail.Email)) And
                            (If(employerInfo.WorkAgencyLocationAddress.StartDate = "12/31/9999", False, True))) Then

                            Pendeing_Information_flg = False 'Employer information is complete JH Aug 29th

                        End If


                        Dim _emp As Application_Employer = Nothing
                        _emp = (From emp In context.Application_Employer
                                Where emp.Application_Sid = applicationID And emp.Employer_Name = employerInfo.EmployerName _
                                And emp.Identification_Value = employerInfo.EmployerTaxID And emp.Supervisor_First_Name = employerInfo.SupervisorFirstName _
                                And emp.Supervisor_Last_Name = employerInfo.SupervisorLastName And emp.Supervisor_End_date = employerInfo.EndDate And emp.Supervisor_Start_date = employerInfo.StartDate And emp.Pending_Information_Flg = False Order By emp.Last_Update_Date Descending
                                Select emp).FirstOrDefault()
                        If (_emp IsNot Nothing And employerId = 0) Then
                            retval.AddErrorMessage("Cannot save this employer as this employer details already exists in database. Please add new employer details")
                            Return retval
                        ElseIf (_emp IsNot Nothing And supflag = False And flag = False And employerId = 0) Then
                            retval.AddErrorMessage("Cannot save this employer as this employer details already exists in database. Please add new employer details")
                            retval.ReturnValue = employerId
                            Return retval
                        Else
                            Dim _employer As Application_Employer = Nothing
                            Dim _insertEmployer As New Application_Employer
                            If (employerId > 0 And supflag = False) Then
                                _employer = (From emp In context.Application_Employer Where emp.Application_Sid = applicationID And emp.Application_Employer_Sid = employerId _
                                             Select emp).FirstOrDefault
                                _employer.CEO_First_Name = employerInfo.CEOFirstName
                                _employer.CEO_Last_Name = employerInfo.CEOLastName
                                _employer.CEO_Middle_Name = employerInfo.CEOMiddleName
                                _employer.Employer_Name = employerInfo.EmployerName
                                _employer.Employer_Type_Sid = employerInfo.EmployerTypeID
                                If (employerInfo.EmployerEndDate = Convert.ToDateTime("12/31/9999")) Then
                                    _employer.Active_Flg = True
                                Else
                                    _employer.Active_Flg = False
                                End If
                                _employer.Employment_End_Date = employerInfo.EmployerEndDate
                                _employer.Employment_Start_Date = employerInfo.EmployerStartDate
                                _employer.Identification_Type_Sid = employerInfo.EmployerIdentificationTypeID
                                _employer.Identification_Value = employerInfo.EmployerTaxID
                                _employer.Last_Update_By = Me.UserID
                                _employer.Last_Update_Date = DateTime.Now
                                _employer.Provider_Contract_Number = employerInfo.DODDProviderContractNumber
                                _employer.Supervisor_First_Name = employerInfo.SupervisorFirstName
                                _employer.Supervisor_Last_Name = employerInfo.SupervisorLastName
                                _employer.Personal_Mailing_Address_Different_Flg = employerInfo.AgencyPersonalAddressSame
                                _employer.Work_Address_Same_As_Agency_Flg = employerInfo.AgencyWorkAddressSame
                                _employer.Supervisor_Start_date = employerInfo.StartDate
                                _employer.Supervisor_End_date = employerInfo.EndDate
                                _employer.Pending_Information_Flg = Pendeing_Information_flg
                            Else
                                If (employerInfo.EmployerEndDate = Convert.ToDateTime("12/31/9999")) Then
                                    _insertEmployer.Active_Flg = True
                                Else
                                    _insertEmployer.Active_Flg = False
                                End If
                                _insertEmployer.CEO_First_Name = employerInfo.CEOFirstName
                                _insertEmployer.CEO_Last_Name = employerInfo.CEOLastName
                                _insertEmployer.CEO_Middle_Name = employerInfo.CEOMiddleName
                                _insertEmployer.Employer_Name = employerInfo.EmployerName
                                _insertEmployer.Employer_Type_Sid = employerInfo.EmployerTypeID
                                _insertEmployer.Employment_End_Date = employerInfo.EmployerEndDate
                                _insertEmployer.Employment_Start_Date = employerInfo.EmployerStartDate
                                _insertEmployer.Identification_Type_Sid = employerInfo.EmployerIdentificationTypeID
                                _insertEmployer.Identification_Value = employerInfo.EmployerTaxID
                                _insertEmployer.Last_Update_By = Me.UserID
                                _insertEmployer.Last_Update_Date = DateTime.Now
                                _insertEmployer.Provider_Contract_Number = employerInfo.DODDProviderContractNumber
                                _insertEmployer.Supervisor_First_Name = employerInfo.SupervisorFirstName
                                _insertEmployer.Supervisor_Last_Name = employerInfo.SupervisorLastName
                                _insertEmployer.Personal_Mailing_Address_Different_Flg = employerInfo.AgencyPersonalAddressSame
                                _insertEmployer.Work_Address_Same_As_Agency_Flg = employerInfo.AgencyWorkAddressSame
                                _insertEmployer.Create_By = Me.UserID
                                _insertEmployer.Create_Date = DateTime.Now
                                _insertEmployer.Supervisor_Start_date = employerInfo.StartDate
                                _insertEmployer.Supervisor_End_date = employerInfo.EndDate
                                _insertEmployer.Application_Sid = applicationID
                                _insertEmployer.Pending_Information_Flg = Pendeing_Information_flg
                                context.Application_Employer.Add(_insertEmployer)
                                'context.SaveChanges()
                            End If
                            If ((Not String.IsNullOrWhiteSpace(employerInfo.AgencyLocationAddress.AddressLine1)) And
                               (Not String.IsNullOrWhiteSpace(employerInfo.AgencyLocationAddress.City)) And
                               (Not String.IsNullOrWhiteSpace(employerInfo.AgencyLocationAddress.County)) And
                               (Not String.IsNullOrWhiteSpace(employerInfo.AgencyLocationAddress.Zip))) Then
                                Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", GetType(Integer))
                                Dim _address As List(Of Address_Lookup_And_Insert_Result) = context.Address_Lookup_And_Insert(parameter, String.Empty, employerInfo.AgencyLocationAddress.AddressLine1, employerInfo.AgencyLocationAddress.AddressLine2, String.Empty, Convert.ToInt32(employerInfo.AgencyLocationAddress.County) _
                                                                                                                              , Convert.ToInt32(employerInfo.AgencyLocationAddress.State), employerInfo.AgencyLocationAddress.Zip + employerInfo.AgencyLocationAddress.ZipPlus, employerInfo.AgencyLocationAddress.City, 0).ToList()
                                Dim _addressRef As Application_Employer_Address_Xref = Nothing
                                _addressRef = (From addRef In context.Application_Employer_Address_Xref Where addRef.Application_Employer_Sid = employerId And addRef.Active_Flg = True _
                                               And addRef.Address_Type_Sid = 3 Select addRef).FirstOrDefault()
                                If (_addressRef Is Nothing) Then
                                    Dim _reference As New Application_Employer_Address_Xref
                                    _reference.Active_Flg = True
                                    _reference.Address_Type_Sid = employerInfo.AgencyLocationAddress.AddressType
                                    If (_employer Is Nothing) Then
                                        _reference.Application_Employer = _insertEmployer
                                    Else
                                        _reference.Application_Employer = _employer
                                    End If
                                    _reference.Address_Sid = parameter.Value
                                    _reference.Application_Sid = applicationID
                                    _reference.Create_By = Me.UserID
                                    _reference.Create_Date = DateTime.Now
                                    _reference.Last_Update_By = Me.UserID
                                    _reference.Last_Update_Date = DateTime.Now
                                    _reference.Agency_Work_Location_Start_date = employerInfo.AgencyLocationAddress.StartDate
                                    _reference.Agency_Work_Location_End_date = employerInfo.AgencyLocationAddress.EndDate
                                    context.Application_Employer_Address_Xref.Add(_reference)
                                Else
                                    _addressRef.Active_Flg = True
                                    If (_employer Is Nothing) Then
                                        _addressRef.Application_Employer = _insertEmployer
                                    Else
                                        _addressRef.Application_Employer = _employer
                                    End If
                                    _addressRef.Address_Type_Sid = employerInfo.AgencyLocationAddress.AddressType
                                    _addressRef.Address_Sid = parameter.Value
                                    _addressRef.Application_Sid = applicationID
                                    _addressRef.Last_Update_By = Me.UserID
                                    _addressRef.Last_Update_Date = DateTime.Now
                                    _addressRef.Agency_Work_Location_Start_date = employerInfo.AgencyLocationAddress.StartDate
                                    _addressRef.Agency_Work_Location_End_date = employerInfo.AgencyLocationAddress.EndDate
                                End If
                            End If
                            context.SaveChanges()

                            If (Not String.IsNullOrEmpty(employerInfo.AgencyLocationAddress.Phone)) Then
                                Dim phoneParameter As System.Data.Objects.ObjectParameter = New ObjectParameter("PhoneSID", GetType(Integer))
                                Dim _phoneaddress As List(Of Phone_Number_Lookup_And_Insert_Result) = context.Phone_Number_Lookup_And_Insert(phoneParameter, employerInfo.AgencyLocationAddress.Phone).ToList()
                                Dim _phoneRef As Application_Employer_Phone_Xref = Nothing
                                _phoneRef = (From addRef In context.Application_Employer_Phone_Xref Where addRef.Application_Employer_Sid = employerId And addRef.Active_Flg = True _
                                                 And addRef.Contact_Type_Sid = 4 Select addRef).FirstOrDefault()
                                If (_phoneRef Is Nothing) Then
                                    Dim _reference As New Application_Employer_Phone_Xref
                                    _reference.Application_Sid = applicationID
                                    _reference.Phone_Sid = phoneParameter.Value
                                    If (_employer Is Nothing) Then
                                        _reference.Application_Employer = _insertEmployer
                                    Else
                                        _reference.Application_Employer = _employer
                                    End If
                                    _reference.Contact_Type_Sid = employerInfo.AgencyLocationAddress.ContactType
                                    _reference.Active_Flg = True
                                    _reference.Create_Date = DateTime.Now
                                    _reference.Last_Update_By = Me.UserID
                                    _reference.Create_By = Me.UserID
                                    _reference.Last_Update_Date = DateTime.Now
                                    context.Application_Employer_Phone_Xref.Add(_reference)
                                Else
                                    _phoneRef.Application_Sid = applicationID
                                    If (_employer Is Nothing) Then
                                        _phoneRef.Application_Employer = _insertEmployer
                                    Else
                                        _phoneRef.Application_Employer = _employer
                                    End If
                                    _phoneRef.Phone_Sid = phoneParameter.Value
                                    _phoneRef.Contact_Type_Sid = employerInfo.AgencyLocationAddress.ContactType
                                    _phoneRef.Active_Flg = True
                                    _phoneRef.Last_Update_By = Me.UserID
                                    _phoneRef.Last_Update_Date = DateTime.Now

                                End If
                                context.SaveChanges()
                            End If

                            If (Not String.IsNullOrEmpty(employerInfo.AgencyLocationAddress.Email)) Then
                                Dim emailParameter As System.Data.Objects.ObjectParameter = New ObjectParameter("EmailSID", GetType(Integer))
                                Dim _emailaddress As List(Of Email_Lookup_And_Insert_Result) = context.Email_Lookup_And_Insert(emailParameter, employerInfo.AgencyLocationAddress.Email).ToList()
                                Dim _emailRef As Application_Employer_Email_Xref = Nothing
                                _emailRef = (From addRef In context.Application_Employer_Email_Xref Where addRef.Application_Employer_Sid = employerId And addRef.Active_Flg = True _
                                                 And addRef.Contact_Type_Sid = 4 Select addRef).FirstOrDefault()
                                If (_emailRef Is Nothing) Then
                                    Dim _reference As New Application_Employer_Email_Xref
                                    _reference.Application_Sid = applicationID
                                    If (_employer Is Nothing) Then
                                        _reference.Application_Employer = _insertEmployer
                                    Else
                                        _reference.Application_Employer = _employer
                                    End If
                                    _reference.Email_Sid = emailParameter.Value
                                    _reference.Contact_Type_Sid = employerInfo.AgencyLocationAddress.ContactType
                                    _reference.Active_Flg = True
                                    _reference.Create_Date = DateTime.Now
                                    _reference.Last_Update_By = Me.UserID
                                    _reference.Create_By = Me.UserID
                                    _reference.Last_Update_Date = DateTime.Now
                                    context.Application_Employer_Email_Xref.Add(_reference)
                                Else
                                    _emailRef.Application_Sid = applicationID
                                    If (_employer Is Nothing) Then
                                        _emailRef.Application_Employer = _insertEmployer
                                    Else
                                        _emailRef.Application_Employer = _employer
                                    End If
                                    _emailRef.Email_Sid = emailParameter.Value
                                    _emailRef.Contact_Type_Sid = employerInfo.AgencyLocationAddress.ContactType
                                    _emailRef.Active_Flg = True
                                    _emailRef.Last_Update_By = Me.UserID
                                    _emailRef.Last_Update_Date = DateTime.Now
                                End If
                                context.SaveChanges()
                            End If


                            If ((Not String.IsNullOrWhiteSpace(employerInfo.WorkAgencyLocationAddress.AddressLine1)) And
                             (Not String.IsNullOrWhiteSpace(employerInfo.WorkAgencyLocationAddress.City)) And
                             (Not String.IsNullOrWhiteSpace(employerInfo.WorkAgencyLocationAddress.County)) And
                             (Not String.IsNullOrWhiteSpace(employerInfo.WorkAgencyLocationAddress.Zip))) Then
                                Dim workparameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", GetType(Integer))
                                Dim _workaddress As List(Of Address_Lookup_And_Insert_Result) = context.Address_Lookup_And_Insert(workparameter, String.Empty, employerInfo.WorkAgencyLocationAddress.AddressLine1, employerInfo.WorkAgencyLocationAddress.AddressLine2, String.Empty, Convert.ToInt32(employerInfo.WorkAgencyLocationAddress.County) _
                                                                                                                              , Convert.ToInt32(employerInfo.WorkAgencyLocationAddress.State), employerInfo.WorkAgencyLocationAddress.Zip + employerInfo.WorkAgencyLocationAddress.ZipPlus, employerInfo.WorkAgencyLocationAddress.City, 0).ToList()

                                Dim _workempAddress As Application_Employer_Address_Xref = Nothing
                                _workempAddress = (From addRef In context.Application_Employer_Address_Xref
                                        Where addRef.Application_Employer_Sid = employerId _
                                        And addRef.Agency_Work_Location_End_date = employerInfo.WorkAgencyLocationAddress.EndDate And addRef.Agency_Work_Location_Start_date = employerInfo.WorkAgencyLocationAddress.StartDate _
                                                       And addRef.Address_Type_Sid = 4 Order By addRef.Last_Update_Date Descending
                                        Select addRef).FirstOrDefault()
                                'Dim _workaddressRef As Application_Employer_Address_Xref = Nothing
                                '_workaddressRef = (From addRef In context.Application_Employer_Address_Xref Where addRef.Application_Employer_Sid = employerId _
                                '                       And addRef.Address_Type_Sid = 4 Order By addRef.Last_Update_Date Descending Select addRef).FirstOrDefault()
                                If (_workempAddress IsNot Nothing And employerId = 0 And flag = False) Then
                                    retval.AddErrorMessage("Cannot save this employer as this work details already exists in database. Please add new employer details or change the work details")
                                    Return retval
                                    'ElseIf (_workaddressRef IsNot Nothing) Then
                                    '    retval.AddErrorMessage("Cannot save this employer as this work details already exists in database. Please add new employer details or change the work details")
                                    '    Return retval
                                End If
                                If (_workempAddress Is Nothing Or flag) Then
                                    Dim _reference As New Application_Employer_Address_Xref
                                    _reference.Active_Flg = True
                                    _reference.Address_Type_Sid = employerInfo.WorkAgencyLocationAddress.AddressType
                                    _reference.Address_Sid = workparameter.Value
                                    _reference.Application_Sid = applicationID
                                    If (_employer Is Nothing) Then
                                        _reference.Application_Employer = _insertEmployer
                                    Else
                                        _reference.Application_Employer = _employer
                                    End If
                                    _reference.Create_By = Me.UserID
                                    _reference.Create_Date = DateTime.Now
                                    _reference.Last_Update_By = Me.UserID
                                    _reference.Last_Update_Date = DateTime.Now
                                    _reference.Agency_Work_Location_Start_date = employerInfo.WorkAgencyLocationAddress.StartDate
                                    _reference.Agency_Work_Location_End_date = employerInfo.WorkAgencyLocationAddress.EndDate
                                    context.Application_Employer_Address_Xref.Add(_reference)
                                Else
                                    _workempAddress.Active_Flg = True
                                    _workempAddress.Address_Type_Sid = employerInfo.WorkAgencyLocationAddress.AddressType
                                    _workempAddress.Address_Sid = workparameter.Value
                                    _workempAddress.Application_Sid = applicationID
                                    If (_employer Is Nothing) Then
                                        _workempAddress.Application_Employer = _insertEmployer
                                    Else
                                        _workempAddress.Application_Employer = _employer
                                    End If
                                    _workempAddress.Last_Update_By = Me.UserID
                                    _workempAddress.Last_Update_Date = DateTime.Now
                                    _workempAddress.Agency_Work_Location_Start_date = employerInfo.WorkAgencyLocationAddress.StartDate
                                    _workempAddress.Agency_Work_Location_End_date = employerInfo.WorkAgencyLocationAddress.EndDate
                                End If
                                context.SaveChanges()

                            End If


                            If (Not String.IsNullOrEmpty(employerInfo.WorkAgencyLocationAddress.Phone)) Then
                                Dim phoneAgencyParameter As System.Data.Objects.ObjectParameter = New ObjectParameter("PhoneSID", GetType(Integer))
                                Dim _phoneagencyaddress As List(Of Phone_Number_Lookup_And_Insert_Result) = context.Phone_Number_Lookup_And_Insert(phoneAgencyParameter, employerInfo.WorkAgencyLocationAddress.Phone).ToList()
                                Dim _phoneRef As Application_Employer_Phone_Xref = Nothing
                                _phoneRef = (From addRef In context.Application_Employer_Phone_Xref Where addRef.Application_Employer_Sid = employerId _
                                                    And addRef.Contact_Type_Sid = 5 Select addRef).FirstOrDefault()
                                If (_phoneRef Is Nothing) Then
                                    Dim _reference As New Application_Employer_Phone_Xref
                                    _reference.Application_Sid = applicationID
                                    If (_employer Is Nothing) Then
                                        _reference.Application_Employer = _insertEmployer
                                    Else
                                        _reference.Application_Employer = _employer
                                    End If
                                    _reference.Phone_Sid = phoneAgencyParameter.Value
                                    _reference.Contact_Type_Sid = employerInfo.WorkAgencyLocationAddress.ContactType
                                    _reference.Active_Flg = True
                                    _reference.Create_Date = DateTime.Now
                                    _reference.Last_Update_By = Me.UserID
                                    _reference.Create_By = Me.UserID
                                    _reference.Last_Update_Date = DateTime.Now
                                    context.Application_Employer_Phone_Xref.Add(_reference)
                                Else
                                    _phoneRef.Application_Sid = applicationID
                                    If (_employer Is Nothing) Then
                                        _phoneRef.Application_Employer = _insertEmployer
                                    Else
                                        _phoneRef.Application_Employer = _employer
                                    End If
                                    _phoneRef.Phone_Sid = phoneAgencyParameter.Value
                                    _phoneRef.Contact_Type_Sid = employerInfo.WorkAgencyLocationAddress.ContactType
                                    _phoneRef.Active_Flg = True
                                    _phoneRef.Last_Update_By = Me.UserID
                                    _phoneRef.Last_Update_Date = DateTime.Now
                                End If
                                context.SaveChanges()
                            End If

                            If (Not String.IsNullOrEmpty(employerInfo.WorkAgencyLocationAddress.Email)) Then
                                Dim emailAgencyParameter As System.Data.Objects.ObjectParameter = New ObjectParameter("EmailSID", GetType(Integer))
                                Dim _emailagencyaddress As List(Of Email_Lookup_And_Insert_Result) = context.Email_Lookup_And_Insert(emailAgencyParameter, employerInfo.WorkAgencyLocationAddress.Email).ToList()
                                Dim _emailRef As Application_Employer_Email_Xref = Nothing
                                _emailRef = (From addRef In context.Application_Employer_Email_Xref Where addRef.Application_Employer_Sid = employerId _
                                            And addRef.Contact_Type_Sid = 5 Select addRef).FirstOrDefault()
                                If (_emailRef Is Nothing) Then
                                    Dim _reference As New Application_Employer_Email_Xref
                                    _reference.Application_Sid = applicationID
                                    If (_employer Is Nothing) Then
                                        _reference.Application_Employer = _insertEmployer
                                    Else
                                        _reference.Application_Employer = _employer
                                    End If
                                    _reference.Email_Sid = emailAgencyParameter.Value
                                    _reference.Contact_Type_Sid = employerInfo.WorkAgencyLocationAddress.ContactType
                                    _reference.Active_Flg = True
                                    _reference.Create_Date = DateTime.Now
                                    _reference.Last_Update_By = Me.UserID
                                    _reference.Create_By = Me.UserID
                                    _reference.Last_Update_Date = DateTime.Now
                                    context.Application_Employer_Email_Xref.Add(_reference)
                                Else
                                    _emailRef.Application_Sid = applicationID
                                    If (_employer Is Nothing) Then
                                        _emailRef.Application_Employer = _insertEmployer
                                    Else
                                        _emailRef.Application_Employer = _employer
                                    End If
                                    _emailRef.Email_Sid = emailAgencyParameter.Value
                                    _emailRef.Contact_Type_Sid = employerInfo.WorkAgencyLocationAddress.ContactType
                                    _emailRef.Active_Flg = True
                                    _emailRef.Last_Update_By = Me.UserID
                                    _emailRef.Last_Update_Date = DateTime.Now
                                End If
                                context.SaveChanges()
                            End If

                            If (Not String.IsNullOrEmpty(employerInfo.SupervisorPhoneEmail.Phone)) Then
                                Dim phoneSupervisorParameter As System.Data.Objects.ObjectParameter = New ObjectParameter("PhoneSID", GetType(Integer))
                                Dim _phoneSupervisoraddress As List(Of Phone_Number_Lookup_And_Insert_Result) = context.Phone_Number_Lookup_And_Insert(phoneSupervisorParameter, employerInfo.SupervisorPhoneEmail.Phone).ToList()
                                Dim _phoneRef As Application_Employer_Phone_Xref = Nothing
                                _phoneRef = (From addRef In context.Application_Employer_Phone_Xref Where addRef.Application_Employer_Sid = employerId _
                                                And addRef.Contact_Type_Sid = 7 Select addRef).FirstOrDefault()
                                If (_phoneRef Is Nothing) Then
                                    Dim _reference As New Application_Employer_Phone_Xref
                                    _reference.Application_Sid = applicationID
                                    If (_employer Is Nothing) Then
                                        _reference.Application_Employer = _insertEmployer
                                    Else
                                        _reference.Application_Employer = _employer
                                    End If
                                    _reference.Phone_Sid = phoneSupervisorParameter.Value
                                    _reference.Contact_Type_Sid = employerInfo.SupervisorPhoneEmail.ContactType
                                    _reference.Active_Flg = True
                                    _reference.Create_Date = DateTime.Now
                                    _reference.Last_Update_By = Me.UserID
                                    _reference.Create_By = Me.UserID
                                    _reference.Last_Update_Date = DateTime.Now
                                    context.Application_Employer_Phone_Xref.Add(_reference)
                                Else
                                    _phoneRef.Application_Sid = applicationID
                                    If (_employer Is Nothing) Then
                                        _phoneRef.Application_Employer = _insertEmployer
                                    Else
                                        _phoneRef.Application_Employer = _employer
                                    End If
                                    _phoneRef.Phone_Sid = phoneSupervisorParameter.Value
                                    _phoneRef.Contact_Type_Sid = employerInfo.SupervisorPhoneEmail.ContactType
                                    _phoneRef.Active_Flg = True
                                    _phoneRef.Last_Update_By = Me.UserID
                                    _phoneRef.Last_Update_Date = DateTime.Now
                                End If
                                context.SaveChanges()
                            End If

                            If (Not String.IsNullOrEmpty(employerInfo.SupervisorPhoneEmail.Email)) Then
                                Dim emailSupervisorParameter As System.Data.Objects.ObjectParameter = New ObjectParameter("EmailSID", GetType(Integer))
                                Dim _emailsupervisoraddress As List(Of Email_Lookup_And_Insert_Result) = context.Email_Lookup_And_Insert(emailSupervisorParameter, employerInfo.SupervisorPhoneEmail.Email).ToList()
                                Dim _emailRef As Application_Employer_Email_Xref = Nothing
                                _emailRef = (From addRef In context.Application_Employer_Email_Xref Where addRef.Contact_Type_Sid = 7 And addRef.Application_Employer_Sid = employerId Select addRef).FirstOrDefault()
                                If (_emailRef Is Nothing) Then
                                    Dim _reference As New Application_Employer_Email_Xref
                                    _reference.Application_Sid = applicationID
                                    If (_employer Is Nothing) Then
                                        _reference.Application_Employer = _insertEmployer
                                    Else
                                        _reference.Application_Employer = _employer
                                    End If
                                    _reference.Email_Sid = emailSupervisorParameter.Value
                                    _reference.Contact_Type_Sid = employerInfo.SupervisorPhoneEmail.ContactType
                                    _reference.Active_Flg = True
                                    _reference.Create_Date = DateTime.Now
                                    _reference.Last_Update_By = Me.UserID
                                    _reference.Create_By = Me.UserID
                                    _reference.Last_Update_Date = DateTime.Now
                                    context.Application_Employer_Email_Xref.Add(_reference)
                                Else
                                    If (_employer Is Nothing) Then
                                        _emailRef.Application_Employer = _insertEmployer
                                    Else
                                        _emailRef.Application_Employer = _employer
                                    End If
                                    _emailRef.Email_Sid = emailSupervisorParameter.Value
                                    _emailRef.Contact_Type_Sid = employerInfo.SupervisorPhoneEmail.ContactType
                                    _emailRef.Active_Flg = True
                                    _emailRef.Last_Update_By = Me.UserID
                                    _emailRef.Application_Sid = applicationID
                                    _emailRef.Last_Update_Date = DateTime.Now
                                End If
                                context.SaveChanges()
                            End If
                            context.SaveChanges()
                            If (_insertEmployer.Application_Employer_Sid > 0) Then
                                retval.ReturnValue = _insertEmployer.Application_Employer_Sid
                            Else
                                retval.ReturnValue = employerId
                            End If
                            Dim variable As Date = Convert.ToDateTime("12/31/9999")
                            Dim _count As Integer = (From emp In context.Application_Employer
                                                     Where emp.Application_Sid = applicationID And emp.Employment_End_Date = variable Select emp).Count()
                            Dim _supervisorcount As Integer = (From emp In context.Application_Employer
                                                     Where emp.Application_Sid = applicationID And emp.Supervisor_End_date = variable Select emp).Count()
                            Dim _workcount As Integer = (From emp In context.Application_Employer
                                                         Join addRef In context.Application_Employer_Address_Xref On addRef.Application_Employer_Sid Equals emp.Application_Employer_Sid
                                                     Where emp.Application_Sid = applicationID And addRef.Agency_Work_Location_End_date = variable And addRef.Address_Type_Sid = 4 Select emp).Count()
                            If (_count = 0 Or _supervisorcount = 0 Or _workcount = 0) Then
                                retval.AddErrorMessage("Data is successfully saved but Please enter an active employer or active supervisor or active work location")
                            End If
                        End If
                    End If
                Catch ex As DbEntityValidationException
                    Dim strResult As String = ex.Message
                    'For Each eve As Object In ex.EntityValidationErrors
                    '    Dim s As String = eve.Entry.Enity.GetType().Name
                    '    Dim s1 As String = eve.Entry.State
                    '    For Each ve As Object In eve.ValidationErrors
                    '        Dim s2 As String = ve.PropertyName
                    '        Dim s3 As String = ve.ErrorMessage
                    '    Next
                    'Next
                    Me.LogError("Error Getting saving employer Info", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting saving employer Info.", True, False))
                    retval.AddErrorMessage(ex.Message)
                Catch ex As Exception
                    Me.LogError("Error Getting saving employer Info", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting saving employer Info.", True, False))
                    retval.AddErrorMessage(ex.Message)
                End Try
            End Using
            Return retval
        End Function
        Public Function DeleteEmployerInfo(ByVal employerId As Integer, ByVal applicationId As Integer) As Boolean Implements IEmployerInformationQueries.DeleteEmployerInfo
            Dim flag As Boolean = False
            Using context As New MAISContext()
                Try
                    If (employerId > 0 And applicationId > 0) Then
                        'Remove Phone Xrefs
                        Dim _maRefPhoneEmployer As List(Of Application_Employer_Phone_Xref)
                        _maRefPhoneEmployer = (From ph In context.Application_Employer_Phone_Xref
                                                  Where ph.Application_Sid = applicationId And ph.Application_Employer_Sid = employerId Select ph).ToList()
                        If (_maRefPhoneEmployer.Count > 0) Then
                            For Each phExists In _maRefPhoneEmployer
                                context.Application_Employer_Phone_Xref.Remove(phExists)
                            Next
                        End If
                        'Remove Email Xrefs
                        Dim _maRefEmailEmployer As List(Of Application_Employer_Email_Xref)
                        _maRefEmailEmployer = (From em In context.Application_Employer_Email_Xref
                                                  Where em.Application_Sid = applicationId And em.Application_Employer_Sid = employerId Select em).ToList()
                        If (_maRefEmailEmployer.Count > 0) Then
                            For Each emExists In _maRefEmailEmployer
                                context.Application_Employer_Email_Xref.Remove(emExists)
                            Next
                        End If
                        'Remove Address Xrefs
                        Dim _maRefAddressEmployer As List(Of Application_Employer_Address_Xref)
                        _maRefAddressEmployer = (From addr In context.Application_Employer_Address_Xref
                                                    Where addr.Application_Sid = applicationId And addr.Application_Employer_Sid = employerId Select addr).ToList()
                        If (_maRefAddressEmployer.Count > 0) Then
                            For Each addrExists In _maRefAddressEmployer
                                context.Application_Employer_Address_Xref.Remove(addrExists)
                            Next
                        End If
                        'Remove Employer
                        Dim _maEmployer = (From emp In context.Application_Employer
                                             Where emp.Application_Sid = applicationId And emp.Application_Employer_Sid = employerId Select emp).ToList()
                        If (_maEmployer.Count > 0) Then
                            For Each empExists In _maEmployer
                                context.Application_Employer.Remove(empExists)
                            Next
                        End If
                        context.SaveChanges()
                        flag = True
                    End If
                Catch ex As Exception
                    Me.LogError("Error Getting when deleting employer Info", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting when deleting application Info.", True, False))
                    Throw
                End Try
            End Using
            Return flag
        End Function
        Public Function GetEmployerPageComplete(applicationId As Integer, ByVal RNDDUniqueCode As String) As Integer Implements IEmployerInformationQueries.GetEmployerPageComplete
            Dim exists As Integer = 0
            Dim supervisorexists As Integer = 0
            Dim workexists As Integer = 0
            Using context As New MAISContext()
                Try
                    Dim _permRNcount As Integer = 0
                    Dim _permDDcount As Integer = 0
                    Dim variable As Date = Convert.ToDateTime("12/31/9999")
                    Dim _count As Integer = (From emp In context.Application_Employer
                                             Where emp.Application_Sid = applicationId And emp.Employment_End_Date = variable And emp.Pending_Information_Flg = False Select emp).Count()
                    If (Not String.IsNullOrEmpty(RNDDUniqueCode)) Then
                        If (RNDDUniqueCode.Contains("RN")) Then
                            _permRNcount = (From emp In context.Employer_RN_DD_Person_Type_Xref
                                                         Join rnType In context.RN_DD_Person_Type_Xref On rnType.RN_DD_Person_Type_Xref_Sid Equals emp.RN_DD_Person_Type_Xref_Sid
                                                         Join rn In context.RNs On rn.RN_Sid Equals rnType.RN_DDPersonnel_Sid
                                                     Where rn.RNLicense_Number = RNDDUniqueCode And emp.Employment_End_Date = variable Select emp).Count()
                        Else
                            _permDDcount = (From emp In context.Employer_RN_DD_Person_Type_Xref
                                                         Join rnType In context.RN_DD_Person_Type_Xref On rnType.RN_DD_Person_Type_Xref_Sid Equals emp.RN_DD_Person_Type_Xref_Sid
                                                         Join rn In context.DDPersonnels On rn.DDPersonnel_Sid Equals rnType.RN_DDPersonnel_Sid
                                                     Where rn.DDPersonnel_Code = RNDDUniqueCode And emp.Employment_End_Date = variable Select emp).Count()
                        End If
                    End If
                    If (_count > 0 Or _permRNcount > 0 Or _permDDcount > 0) Then
                        exists = 1
                    End If
                    Dim _permSuperRNDDcount As Integer = 0
                    Dim _supercount As Integer = 0
                    Dim listIdentityValuePerm As New List(Of String)
                    If (applicationId > 0) Then
                        Dim listIdentityValueTemp As List(Of String) = (From emp In context.Application_Employer
                                                     Where emp.Application_Sid = applicationId And emp.Pending_Information_Flg = False Select emp.Identification_Value).Distinct.ToList()
                        For Each identity As String In listIdentityValueTemp
                            Dim supervisorEndDate As DateTime = (From emp In context.Application_Employer Where emp.Identification_Value = identity _
                                        And emp.Application_Sid = applicationId And emp.Pending_Information_Flg = False Order By emp.Last_Update_Date Descending Select emp.Supervisor_End_date).FirstOrDefault()
                            If (supervisorEndDate = variable) Then
                                _supercount = 1
                                Exit For
                            End If
                        Next
                    End If
                    If (Not String.IsNullOrEmpty(RNDDUniqueCode)) Then
                        If (RNDDUniqueCode.Contains("RN")) Then
                            listIdentityValuePerm = (From emp In context.Employer_RN_DD_Person_Type_Xref
                                                         Join rnType In context.RN_DD_Person_Type_Xref On rnType.RN_DD_Person_Type_Xref_Sid Equals emp.RN_DD_Person_Type_Xref_Sid
                                                         Join rn In context.RNs On rn.RN_Sid Equals rnType.RN_DDPersonnel_Sid
                                                         Join e In context.Employers On emp.Employer_Sid Equals e.Employer_Sid
                                                     Where rn.RNLicense_Number = RNDDUniqueCode Select e.Identification_Value).Distinct.ToList()
                        Else
                            listIdentityValuePerm = (From emp In context.Employer_RN_DD_Person_Type_Xref
                                                         Join rnType In context.RN_DD_Person_Type_Xref On rnType.RN_DD_Person_Type_Xref_Sid Equals emp.RN_DD_Person_Type_Xref_Sid
                                                         Join rn In context.DDPersonnels On rn.DDPersonnel_Sid Equals rnType.RN_DDPersonnel_Sid
                                                         Join e In context.Employers On emp.Employer_Sid Equals e.Employer_Sid
                                                     Where rn.DDPersonnel_Code = RNDDUniqueCode Select e.Identification_Value).Distinct.ToList()
                        End If
                        For Each identity As String In listIdentityValuePerm
                            Dim supervisorEndDate As DateTime = (From emp In context.Employers
                                                                 Join e In context.Employer_RN_DD_Person_Type_Xref On e.Employer_Sid Equals emp.Employer_Sid
                                                                 Where emp.Identification_Value = identity Order By e.Last_Update_Date Descending Select e.Supervisor_End_date).FirstOrDefault()
                            If (supervisorEndDate = variable) Then
                                _permSuperRNDDcount = 1
                                Exit For
                            End If
                        Next
                    End If
                    If (_supercount > 0 Or _permSuperRNDDcount > 0) Then
                        supervisorexists = 1
                    End If
                    Dim _permworkRNDDcount As Integer = 0
                    Dim _workcount As Integer = 0
                    If (applicationId > 0) Then
                        Dim identityValueTemp As List(Of String) = (From emp In context.Application_Employer
                                                     Where emp.Application_Sid = applicationId And emp.Pending_Information_Flg = False Select emp.Identification_Value).Distinct.ToList()
                        For Each identity As String In identityValueTemp
                            Dim workLocationEndDate As DateTime = (From emp In context.Application_Employer
                                                                 Join empRef In context.Application_Employer_Address_Xref On emp.Application_Employer_Sid Equals empRef.Application_Employer_Sid
                                                                 Where emp.Identification_Value = identity And empRef.Address_Type_Sid = 4 And emp.Application_Sid = applicationId _
                                        And emp.Application_Sid = applicationId And emp.Pending_Information_Flg = False Order By empRef.Last_Update_Date Descending Select empRef.Agency_Work_Location_End_date).FirstOrDefault()
                            If (workLocationEndDate = variable) Then
                                _workcount = 1
                                Exit For
                            End If
                        Next
                    End If
                    If (Not String.IsNullOrEmpty(RNDDUniqueCode)) Then
                        If (RNDDUniqueCode.Contains("RN")) Then
                            listIdentityValuePerm = (From emp In context.Employer_RN_DD_Person_Type_Xref
                                                         Join rnType In context.RN_DD_Person_Type_Xref On rnType.RN_DD_Person_Type_Xref_Sid Equals emp.RN_DD_Person_Type_Xref_Sid
                                                         Join rn In context.RNs On rn.RN_Sid Equals rnType.RN_DDPersonnel_Sid
                                                         Join e In context.Employers On emp.Employer_Sid Equals e.Employer_Sid
                                                     Where rn.RNLicense_Number = RNDDUniqueCode Select e.Identification_Value).Distinct.ToList()
                        Else
                            listIdentityValuePerm = (From emp In context.Employer_RN_DD_Person_Type_Xref
                                                         Join rnType In context.RN_DD_Person_Type_Xref On rnType.RN_DD_Person_Type_Xref_Sid Equals emp.RN_DD_Person_Type_Xref_Sid
                                                         Join rn In context.DDPersonnels On rn.DDPersonnel_Sid Equals rnType.RN_DDPersonnel_Sid
                                                         Join e In context.Employers On emp.Employer_Sid Equals e.Employer_Sid
                                                     Where rn.DDPersonnel_Code = RNDDUniqueCode Select e.Identification_Value).Distinct.ToList()
                        End If
                        For Each identity As String In listIdentityValuePerm
                            Dim supervisorWorkEndDate As DateTime = (From emp In context.Employers
                                                                 Join empAddRef In context.Employer_RN_DD_Person_Type_Address_Xref On empAddRef.Employer_Sid Equals emp.Employer_Sid
                                                                 Where emp.Identification_Value = identity Order By empAddRef.Last_Update_Date Descending Select empAddRef.Agency_Work_Location_End_Date).FirstOrDefault()
                            If (supervisorWorkEndDate = variable) Then
                                _permworkRNDDcount = 1
                                Exit For
                            End If
                        Next
                    End If
                    If (_workcount > 0 Or _permworkRNDDcount > 0) Then
                        workexists = 1
                    End If
                    If (workexists <> 1 Or supervisorexists <> 1 Or exists <> 1) Then
                        exists = 0
                    Else
                        exists = 1
                    End If
                Catch ex As Exception
                    Me.LogError("Error Getting in employer information for page completion rule.", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting in employer information for page completion rule.", True, False))
                    Throw
                End Try
            End Using
            Return exists
        End Function
        Public Function GetDataHistoryAddedEmplyerInfo(employerID As Integer, ByVal UniqueCode As String, ByVal RN_Flg As Boolean) As EmployerInformationDetails Implements IEmployerInformationQueries.GetDataHistoryAddedEmplyerInfo
            Dim employerData As New Objects.EmployerInformationDetails
            Dim agencyLocationAddress As New Objects.AddressControlDetails
            Dim workLocationAddress As New Objects.AddressControlDetails
            Dim SupervisorLocationAddress As New Objects.AddressControlDetails
            Using context As New MAISContext()
                Try
                    If RN_Flg Then
                        employerData = (From emp In context.Employers _
                                        Join ep In context.Employer_RN_DD_Person_Type_Xref On ep.Employer_Sid Equals emp.Employer_Sid
                                        Join ety In context.Employer_Type On ep.Employer_Type_Sid Equals ety.Employer_Type_Sid
                                        Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals ep.RN_DD_Person_Type_Xref_Sid
                                        Join rn In context.RNs On rn.RN_Sid Equals rnRef.RN_DDPersonnel_Sid
                                        Where emp.Employer_Sid = employerID And rn.RNLicense_Number = UniqueCode Order By emp.Last_Update_Date Descending
                                        Select New Objects.EmployerInformationDetails() With {
                                            .AgencyPersonalAddressSame = ep.Personal_Mailing_Address_Different_Flg,
                                            .AgencyWorkAddressSame = ep.Work_Address_Same_As_Agency_Flg,
                                            .CEOFirstName = ep.CEO_First_Name,
                                            .CEOLastName = ep.CEO_Last_Name,
                                            .CEOMiddleName = ep.CEO_Middle_Name,
                                            .DODDProviderContractNumber = ep.Provider_Contract_Number,
                                            .EmployerEndDate = ep.Employment_End_Date,
                                            .EmployerStartDate = ep.Employment_Start_Date,
                                            .SupervisorFirstName = ep.Supervisor_First_Name,
                                            .SupervisorLastName = ep.Supervisor_Last_Name,
                                            .EmployerID = emp.Employer_Sid,
                                            .EmployerName = emp.Employer_Name,
                                            .EmployerTypeID = ety.Employer_Type_Sid,
                                            .EmployerIdentificationTypeID = emp.Identification_Type_Sid,
                                            .StartDate = ep.Supervisor_Start_date,
                                            .EndDate = ep.Supervisor_End_date,
                        .EmployerTaxID = emp.Identification_Value
                                            }).FirstOrDefault()
                        agencyLocationAddress = (From addrReference In context.Employer_RN_DD_Person_Type_Address_Xref
                        Join ep In context.Employers On addrReference.Employer_Sid Equals ep.Employer_Sid
                        Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals addrReference.RN_DD_Person_Type_Xref_Sid
                        Join rn In context.RNs On rn.RN_Sid Equals rnRef.RN_DDPersonnel_Sid
                        Where ep.Employer_Sid = employerID And addrReference.Active_Flg = True And addrReference.Address_Type_Sid = 3 _
                        And rn.RNLicense_Number = UniqueCode
                        Select New Objects.AddressControlDetails() With {
                            .AddressType = addrReference.Address_Type_Sid,
                            .AddressSID = addrReference.Address_Sid
                            }).FirstOrDefault()
                        If (agencyLocationAddress IsNot Nothing) Then
                            If (agencyLocationAddress.AddressSID > 0) Then
                                Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", agencyLocationAddress.AddressSID)
                                Dim address As List(Of Address_Lookup_And_Insert_Result) = context.Address_Lookup_And_Insert(parameter, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).ToList()
                                agencyLocationAddress.AddressLine1 = address(0).Address_Line1
                                agencyLocationAddress.AddressLine2 = address(0).Address_Line2
                                agencyLocationAddress.City = address(0).City
                                agencyLocationAddress.County = address(0).County_Desc
                                agencyLocationAddress.State = address(0).State_Abbr
                                agencyLocationAddress.Zip = address(0).Zip
                                agencyLocationAddress.City = address(0).City
                                agencyLocationAddress.CountyID = address(0).CountyID
                                agencyLocationAddress.StateID = address(0).StateID
                            End If
                        End If
                        If (agencyLocationAddress IsNot Nothing) Then
                            agencyLocationAddress.Phone = (From phone In context.Phone_Number _
                                Join phoneRef In context.Employer_RN_DD_Person_Type_Phone_Xref On phoneRef.Phone_Sid Equals phone.Phone_Number_SID
                                Join ep In context.Employers On phoneRef.Employer_Sid Equals ep.Employer_Sid
                                 Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals phoneRef.RN_DD_Person_Type_Xref_Sid
                        Join rn In context.RNs On rn.RN_Sid Equals rnRef.RN_DDPersonnel_Sid
                                Where phoneRef.Active_Flg = True And phoneRef.Contact_Type_Sid = 4 And ep.Employer_Sid = employerID And rn.RNLicense_Number = UniqueCode
                                Select phone.Phone_Number1).FirstOrDefault()
                            agencyLocationAddress.Email = (From phone In context.Email1 _
                                Join phoneRef In context.Employer_RN_DD_Person_Type_Email_Xref On phoneRef.Email_Sid Equals phone.Email_SID
                                Join ep In context.Employers On phoneRef.Employer_Sid Equals ep.Employer_Sid
                                 Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals phoneRef.RN_DD_Person_Type_Xref_Sid
                        Join rn In context.RNs On rn.RN_Sid Equals rnRef.RN_DDPersonnel_Sid
                                Where phoneRef.Active_Flg = True And phoneRef.Contact_Type_Sid = 4 And ep.Employer_Sid = employerID And rn.RNLicense_Number = UniqueCode
                                Select phone.Email_Address).FirstOrDefault()
                        End If
                        workLocationAddress = (From addrReference In context.Employer_RN_DD_Person_Type_Address_Xref
                        Join ep In context.Employers On addrReference.Employer_Sid Equals ep.Employer_Sid
                        Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals addrReference.RN_DD_Person_Type_Xref_Sid
                        Join rn In context.RNs On rn.RN_Sid Equals rnRef.RN_DDPersonnel_Sid
                        Where ep.Employer_Sid = employerID And addrReference.Active_Flg = True And addrReference.Address_Type_Sid = 4 And rn.RNLicense_Number = UniqueCode Order By addrReference.Last_Update_Date Descending
                        Select New Objects.AddressControlDetails() With {
                            .AddressType = addrReference.Address_Type_Sid,
                            .StartDate = addrReference.Agency_Work_Location_Start_Date,
                            .EndDate = addrReference.Agency_Work_Location_End_Date,
                            .AddressSID = addrReference.Address_Sid
                            }).FirstOrDefault()
                        If (workLocationAddress IsNot Nothing) Then
                            If (workLocationAddress.AddressSID > 0) Then
                                Dim workparameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", workLocationAddress.AddressSID)
                                Dim address As List(Of Address_Lookup_And_Insert_Result) = context.Address_Lookup_And_Insert(workparameter, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).ToList()
                                workLocationAddress.AddressLine1 = address(0).Address_Line1
                                workLocationAddress.AddressLine2 = address(0).Address_Line2
                                workLocationAddress.City = address(0).City
                                workLocationAddress.County = address(0).County_Desc
                                workLocationAddress.State = address(0).State_Abbr
                                workLocationAddress.Zip = address(0).Zip
                                workLocationAddress.City = address(0).City
                                workLocationAddress.CountyID = address(0).CountyID
                                workLocationAddress.StateID = address(0).StateID
                            End If
                        End If
                        If (workLocationAddress IsNot Nothing) Then
                            workLocationAddress.Phone = (From phone In context.Phone_Number _
                                Join phoneRef In context.Employer_RN_DD_Person_Type_Phone_Xref On phoneRef.Phone_Sid Equals phone.Phone_Number_SID
                                Join ep In context.Employers On phoneRef.Employer_Sid Equals ep.Employer_Sid
                                Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals phoneRef.RN_DD_Person_Type_Xref_Sid
                                Join rn In context.RNs On rn.RN_Sid Equals rnRef.RN_DDPersonnel_Sid
                                Where phoneRef.Active_Flg = True And phoneRef.Contact_Type_Sid = 5 And ep.Employer_Sid = employerID And rn.RNLicense_Number = UniqueCode
                                Select phone.Phone_Number1).FirstOrDefault()
                            workLocationAddress.Email = (From phone In context.Email1 _
                                Join phoneRef In context.Employer_RN_DD_Person_Type_Email_Xref On phoneRef.Email_Sid Equals phone.Email_SID
                                Join ep In context.Employers On phoneRef.Employer_Sid Equals ep.Employer_Sid
                                Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals phoneRef.RN_DD_Person_Type_Xref_Sid
                                Join rn In context.RNs On rn.RN_Sid Equals rnRef.RN_DDPersonnel_Sid
                                Where phoneRef.Active_Flg = True And phoneRef.Contact_Type_Sid = 5 And ep.Employer_Sid = employerID And rn.RNLicense_Number = UniqueCode
                                Select phone.Email_Address).FirstOrDefault()
                        End If
                        SupervisorLocationAddress.Phone = (From phone In context.Phone_Number _
                            Join phoneRef In context.Employer_RN_DD_Person_Type_Phone_Xref On phoneRef.Phone_Sid Equals phone.Phone_Number_SID
                            Join ep In context.Employers On phoneRef.Employer_Sid Equals ep.Employer_Sid
                            Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals phoneRef.RN_DD_Person_Type_Xref_Sid
                            Join rn In context.RNs On rn.RN_Sid Equals rnRef.RN_DDPersonnel_Sid
                            Where phoneRef.Active_Flg = True And phoneRef.Contact_Type_Sid = 7 And ep.Employer_Sid = employerID And rn.RNLicense_Number = UniqueCode
                            Select phone.Phone_Number1).FirstOrDefault()
                        SupervisorLocationAddress.Email = (From phone In context.Email1 _
                            Join phoneRef In context.Employer_RN_DD_Person_Type_Email_Xref On phoneRef.Email_Sid Equals phone.Email_SID
                            Join ep In context.Employers On phoneRef.Employer_Sid Equals ep.Employer_Sid
                            Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals phoneRef.RN_DD_Person_Type_Xref_Sid
                            Join rn In context.RNs On rn.RN_Sid Equals rnRef.RN_DDPersonnel_Sid
                            Where phoneRef.Active_Flg = True And phoneRef.Contact_Type_Sid = 7 And ep.Employer_Sid = employerID And rn.RNLicense_Number = UniqueCode
                            Select phone.Email_Address).FirstOrDefault()
                        employerData.AgencyLocationAddress = agencyLocationAddress
                        employerData.WorkAgencyLocationAddress = workLocationAddress
                        employerData.SupervisorPhoneEmail = SupervisorLocationAddress
                    Else
                        employerData = (From emp In context.Employers _
                                        Join ep In context.Employer_RN_DD_Person_Type_Xref On ep.Employer_Sid Equals emp.Employer_Sid
                                        Join ety In context.Employer_Type On ep.Employer_Type_Sid Equals ety.Employer_Type_Sid
                                        Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals ep.RN_DD_Person_Type_Xref_Sid
                                        Join rn In context.DDPersonnels On rn.DDPersonnel_Sid Equals rnRef.RN_DDPersonnel_Sid
                                        Where emp.Employer_Sid = employerID And rn.DDPersonnel_Code = UniqueCode
                                        Select New Objects.EmployerInformationDetails() With {
                                            .AgencyPersonalAddressSame = ep.Personal_Mailing_Address_Different_Flg,
                                            .AgencyWorkAddressSame = ep.Work_Address_Same_As_Agency_Flg,
                                            .CEOFirstName = ep.CEO_First_Name,
                                            .CEOLastName = ep.CEO_Last_Name,
                                            .CEOMiddleName = ep.CEO_Middle_Name,
                                            .DODDProviderContractNumber = ep.Provider_Contract_Number,
                                            .EmployerEndDate = ep.Employment_End_Date,
                                            .EmployerStartDate = ep.Employment_Start_Date,
                                            .SupervisorFirstName = ep.Supervisor_First_Name,
                                            .SupervisorLastName = ep.Supervisor_Last_Name,
                                            .EmployerID = emp.Employer_Sid,
                                            .EmployerName = emp.Employer_Name,
                                            .EmployerTypeID = ety.Employer_Type_Sid,
                                            .EmployerIdentificationTypeID = emp.Identification_Type_Sid,
                                            .StartDate = ep.Supervisor_Start_date,
                                            .EndDate = ep.Supervisor_End_date,
                                            .EmployerTaxID = emp.Identification_Value
                                            }).FirstOrDefault()
                        agencyLocationAddress = (From addrReference In context.Employer_RN_DD_Person_Type_Address_Xref
                        Join ep In context.Employers On addrReference.Employer_Sid Equals ep.Employer_Sid
                        Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals addrReference.RN_DD_Person_Type_Xref_Sid
                        Join rn In context.DDPersonnels On rn.DDPersonnel_Sid Equals rnRef.RN_DDPersonnel_Sid
                        Where ep.Employer_Sid = employerID And addrReference.Active_Flg = True And addrReference.Address_Type_Sid = 3 _
                        And rn.DDPersonnel_Code = UniqueCode
                        Select New Objects.AddressControlDetails() With {
                            .AddressType = addrReference.Address_Type_Sid,
                            .AddressSID = addrReference.Address_Sid
                            }).FirstOrDefault()
                        If (agencyLocationAddress IsNot Nothing) Then
                            If (agencyLocationAddress.AddressSID > 0) Then
                                Dim parameter1 As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", agencyLocationAddress.AddressSID)
                                Dim address1 As List(Of Address_Lookup_And_Insert_Result) = context.Address_Lookup_And_Insert(parameter1, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).ToList()
                                agencyLocationAddress.AddressLine1 = address1(0).Address_Line1
                                agencyLocationAddress.AddressLine2 = address1(0).Address_Line2
                                agencyLocationAddress.City = address1(0).City
                                agencyLocationAddress.County = address1(0).County_Desc
                                agencyLocationAddress.State = address1(0).State_Abbr
                                agencyLocationAddress.Zip = address1(0).Zip
                                agencyLocationAddress.City = address1(0).City
                                agencyLocationAddress.CountyID = address1(0).CountyID
                                agencyLocationAddress.StateID = address1(0).StateID
                            End If
                        End If
                        If (agencyLocationAddress IsNot Nothing) Then
                            agencyLocationAddress.Phone = (From phone In context.Phone_Number _
                                Join phoneRef In context.Employer_RN_DD_Person_Type_Phone_Xref On phoneRef.Phone_Sid Equals phone.Phone_Number_SID _
                                Join ep In context.Employers On phoneRef.Employer_Sid Equals ep.Employer_Sid
                                 Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals phoneRef.RN_DD_Person_Type_Xref_Sid
                        Join rn In context.DDPersonnels On rn.DDPersonnel_Sid Equals rnRef.RN_DDPersonnel_Sid
                                Where phoneRef.Active_Flg = True And phoneRef.Contact_Type_Sid = 4 And ep.Employer_Sid = employerID And rn.DDPersonnel_Code = UniqueCode
                                Select phone.Phone_Number1).FirstOrDefault()
                            agencyLocationAddress.Email = (From phone In context.Email1 _
                                Join phoneRef In context.Employer_RN_DD_Person_Type_Email_Xref On phoneRef.Email_Sid Equals phone.Email_SID
                                Join ep In context.Employers On phoneRef.Employer_Sid Equals ep.Employer_Sid
                                 Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals phoneRef.RN_DD_Person_Type_Xref_Sid
                        Join rn In context.DDPersonnels On rn.DDPersonnel_Sid Equals rnRef.RN_DDPersonnel_Sid
                                Where phoneRef.Active_Flg = True And phoneRef.Contact_Type_Sid = 4 And ep.Employer_Sid = employerID And rn.DDPersonnel_Code = UniqueCode
                                Select phone.Email_Address).FirstOrDefault()
                        End If
                        workLocationAddress = (From addrReference In context.Employer_RN_DD_Person_Type_Address_Xref
                        Join ep In context.Employers On addrReference.Employer_Sid Equals ep.Employer_Sid
                        Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals addrReference.RN_DD_Person_Type_Xref_Sid
                        Join rn In context.DDPersonnels On rn.DDPersonnel_Sid Equals rnRef.RN_DDPersonnel_Sid
                        Where ep.Employer_Sid = employerID And addrReference.Active_Flg = True And addrReference.Address_Type_Sid = 4 And rn.DDPersonnel_Code = UniqueCode
                        Select New Objects.AddressControlDetails() With {
                            .AddressType = addrReference.Address_Type_Sid,
                            .StartDate = addrReference.Agency_Work_Location_Start_Date,
                            .EndDate = addrReference.Agency_Work_Location_End_Date,
                            .AddressSID = addrReference.Address_Sid
                            }).FirstOrDefault()
                        If (agencyLocationAddress IsNot Nothing) Then
                            If (workLocationAddress.AddressSID > 0) Then
                                Dim workparameter1 As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", workLocationAddress.AddressSID)
                                Dim address1 As List(Of Address_Lookup_And_Insert_Result) = context.Address_Lookup_And_Insert(workparameter1, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).ToList()
                                workLocationAddress.AddressLine1 = address1(0).Address_Line1
                                workLocationAddress.AddressLine2 = address1(0).Address_Line2
                                workLocationAddress.City = address1(0).City
                                workLocationAddress.County = address1(0).County_Desc
                                workLocationAddress.State = address1(0).State_Abbr
                                workLocationAddress.Zip = address1(0).Zip
                                workLocationAddress.City = address1(0).City
                                workLocationAddress.CountyID = address1(0).CountyID
                                workLocationAddress.StateID = address1(0).StateID
                            End If
                        End If
                        If (workLocationAddress IsNot Nothing) Then
                            workLocationAddress.Phone = (From phone In context.Phone_Number _
                                Join phoneRef In context.Employer_RN_DD_Person_Type_Phone_Xref On phoneRef.Phone_Sid Equals phone.Phone_Number_SID
                                Join ep In context.Employers On phoneRef.Employer_Sid Equals ep.Employer_Sid
                                Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals phoneRef.RN_DD_Person_Type_Xref_Sid
                                Join rn In context.DDPersonnels On rn.DDPersonnel_Sid Equals rnRef.RN_DDPersonnel_Sid
                                Where phoneRef.Active_Flg = True And phoneRef.Contact_Type_Sid = 5 And ep.Employer_Sid = employerID And rn.DDPersonnel_Code = UniqueCode
                                Select phone.Phone_Number1).FirstOrDefault()
                            workLocationAddress.Email = (From phone In context.Email1 _
                                Join phoneRef In context.Employer_RN_DD_Person_Type_Email_Xref On phoneRef.Email_Sid Equals phone.Email_SID
                                Join ep In context.Employers On phoneRef.Employer_Sid Equals ep.Employer_Sid
                                Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals phoneRef.RN_DD_Person_Type_Xref_Sid
                                Join rn In context.DDPersonnels On rn.DDPersonnel_Sid Equals rnRef.RN_DDPersonnel_Sid
                                Where phoneRef.Active_Flg = True And phoneRef.Contact_Type_Sid = 5 And ep.Employer_Sid = employerID And rn.DDPersonnel_Code = UniqueCode
                                Select phone.Email_Address).FirstOrDefault()
                        End If
                        SupervisorLocationAddress.Phone = (From phone In context.Phone_Number _
                            Join phoneRef In context.Employer_RN_DD_Person_Type_Phone_Xref On phoneRef.Phone_Sid Equals phone.Phone_Number_SID
                            Join ep In context.Employers On phoneRef.Employer_Sid Equals ep.Employer_Sid
                            Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals phoneRef.RN_DD_Person_Type_Xref_Sid
                            Join rn In context.DDPersonnels On rn.DDPersonnel_Sid Equals rnRef.RN_DDPersonnel_Sid
                            Where phoneRef.Active_Flg = True And phoneRef.Contact_Type_Sid = 7 And ep.Employer_Sid = employerID And rn.DDPersonnel_Code = UniqueCode
                            Select phone.Phone_Number1).FirstOrDefault()
                        SupervisorLocationAddress.Email = (From phone In context.Email1 _
                            Join phoneRef In context.Employer_RN_DD_Person_Type_Email_Xref On phoneRef.Email_Sid Equals phone.Email_SID
                            Join ep In context.Employers On phoneRef.Employer_Sid Equals ep.Employer_Sid
                            Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals phoneRef.RN_DD_Person_Type_Xref_Sid
                            Join rn In context.DDPersonnels On rn.DDPersonnel_Sid Equals rnRef.RN_DDPersonnel_Sid
                            Where phoneRef.Active_Flg = True And phoneRef.Contact_Type_Sid = 7 And ep.Employer_Sid = employerID And rn.DDPersonnel_Code = UniqueCode
                            Select phone.Email_Address).FirstOrDefault()
                        employerData.AgencyLocationAddress = agencyLocationAddress
                        employerData.WorkAgencyLocationAddress = workLocationAddress
                        employerData.SupervisorPhoneEmail = SupervisorLocationAddress
                    End If
                Catch ex As Exception
                    Me.LogError("Error Getting details of approved employer Info", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting details of approved employer Info.", True, False))
                    Throw
                End Try
            End Using
            Return employerData
        End Function

        Public Function GetEmployerInformationWithAddressFromPerm(UniqueCode As String, RN_Flg As Boolean) As List(Of EmployerInformationDetails) Implements IEmployerInformationQueries.GetEmployerInformationWithAddressFromPerm
            Dim empInfo As New List(Of EmployerInformationDetails)
            Dim empInfoReturn As New List(Of EmployerInformationDetails)
            Try
                Using context As New MAISContext
                    If RN_Flg Then
                        empInfo = (From r In context.RNs
                                   Join rp In context.RN_DD_Person_Type_Xref On r.RN_Sid Equals rp.RN_DDPersonnel_Sid
                                   Join ep In context.Employer_RN_DD_Person_Type_Xref On ep.RN_DD_Person_Type_Xref_Sid Equals rp.RN_DD_Person_Type_Xref_Sid
                                   Join e In context.Employers On e.Employer_Sid Equals ep.Employer_Sid
                                   Join ety In context.Employer_Type On ep.Employer_Type_Sid Equals ety.Employer_Type_Sid
                                   Where r.RNLicense_Number = UniqueCode AndAlso rp.Person_Type_Sid = 1
                                   Select New Objects.EmployerInformationDetails() With {
                                       .EmployerID = e.Employer_Sid
                                       }).Distinct.ToList()
                    Else
                        empInfo = (From dd In context.DDPersonnels
                                   Join rp In context.RN_DD_Person_Type_Xref On dd.DDPersonnel_Sid Equals rp.RN_DDPersonnel_Sid
                                   Join ep In context.Employer_RN_DD_Person_Type_Xref On ep.RN_DD_Person_Type_Xref_Sid Equals rp.RN_DD_Person_Type_Xref_Sid
                                   Join e In context.Employers On e.Employer_Sid Equals ep.Employer_Sid
                                   Join ety In context.Employer_Type On ep.Employer_Type_Sid Equals ety.Employer_Type_Sid
                                   Where dd.DDPersonnel_Code = UniqueCode AndAlso rp.Person_Type_Sid = 2
                                   Select New Objects.EmployerInformationDetails() With {
                                       .EmployerID = e.Employer_Sid
                                       }).Distinct.ToList()
                    End If
                    For Each employerDetails As Objects.EmployerInformationDetails In empInfo
                        empInfoReturn.Add(GetDataHistoryAddedEmplyerInfo(employerDetails.EmployerID, UniqueCode, RN_Flg))

                    Next
                End Using
            Catch ex As Exception
                Me.LogError("Error Getting employer information from permanent", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error Getting employer information from permanent.", True, False))
                Throw
            End Try
            Return empInfoReturn
        End Function
    End Class
End Namespace
