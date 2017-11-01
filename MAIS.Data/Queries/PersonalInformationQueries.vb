Imports ODMRDDHelperClassLibrary.Utility
Imports System.Configuration
Imports System.Data.Objects
Imports System.Data.Entity.Validation


Namespace Queries
    Public Interface IPersonalInformationQueries
        Inherits IQueriesBase

        Function SavePersonalInformation(ByVal personalResult As Objects.PersonalInformationDetails, ByVal appId As Integer, ByVal rnOrDD As Boolean, ByVal name As String, ByVal brandNew As Boolean, ByVal uniqueCode As String, ByVal adminPerm As Boolean, ByVal queryString As String) As ReturnObject(Of Long)
        Function GetDDPersonnelInformation(ByVal applicationID As Integer) As Objects.PersonalInformationDetails
        Function GetRNInformation(ByVal applicationID As Integer) As Objects.PersonalInformationDetails
        Function GetRNInformationFromPermanent(ByVal rnLicenseNumber As String) As Objects.PersonalInformationDetails
        Function GetDDPersonnelInformationFromPermanent(ByVal ddpersonnelCode As String) As Objects.PersonalInformationDetails
        Function DeleteTheDataApplication(ByVal applicationID As Integer) As Boolean
        Function GetPersonalPageComplete(ByVal applicationID As Integer) As Integer
        Function GetPartialPersonalPageComplete(ByVal applicationID As Integer) As Integer
        'Function GetOnlyPersonnalInformation(UniqueCode As String, RN_Flg As Boolean) As Objects.PersonalInformationDetails
    End Interface
    Public Class PersonalInformationQueries
        Inherits QueriesBase
        Implements IPersonalInformationQueries
        'Public Function GetOnlyPersonnalInformation(UniqueCode As String, RN_Flg As Boolean) As Objects.PersonalInformationDetails Implements IPersonalInformationQueries.GetOnlyPersonnalInformation
        '    Dim RNDDPersonalInfo As Objects.PersonalInformationDetails = Nothing
        '    Try
        '        Using context As New MAISContext
        '            If RN_Flg Then
        '                RNDDPersonalInfo = (From r In context.RNs
        '                               Where r.RNLicense_Number = UniqueCode
        '                               Select New Objects.PersonalInformationDetails() With {
        '                                        .FirstName = r.First_Name,
        '                                        .LastName = r.Last_Name,
        '                                        .MiddleName = r.Middle_Name,
        '                                        .Gender = r.Gender,
        '                                        .DOBDateOfIssuance = r.Date_Of_Original_Issuance,
        '                                        .RNLicenseOrSSN = r.RNLicense_Number
        '                                        }).FirstOrDefault()
        '            Else
        '                RNDDPersonalInfo = (From dd In context.DDPersonnels
        '                                Where dd.DDPersonnel_Code = UniqueCode
        '                                Select New Objects.PersonalInformationDetails() With {
        '                                         .FirstName = dd.First_Name,
        '                                         .LastName = dd.Last_Name,
        '                                         .MiddleName = Trim(dd.Middle_Name),
        '                                         .Gender = Trim(dd.Gender),
        '                                         .DOBDateOfIssuance = dd.DOB,
        '                                         .RNLicenseOrSSN = dd.SSN
        '                                         }).FirstOrDefault()
        '            End If
        '        End Using
        '    Catch ex As Exception
        '        Me.LogError("Error fetching only personnal information", ex)
        '        Me._messages.Add(New ReturnMessage("Error fetching only personnal information", True, False))
        '        Throw
        '    End Try
        '    Return RNDDPersonalInfo
        'End Function
        Public Function GetDDPersonnelInformation(ByVal applicationID As Integer) As Objects.PersonalInformationDetails Implements IPersonalInformationQueries.GetDDPersonnelInformation
            Dim DDPersonalInfo As Objects.PersonalInformationDetails = Nothing
            Dim ListPhone As New List(Of Objects.PhoneDetails)
            Dim ListEmail As New List(Of Objects.EmailAddressDetails)
            Using context As New MAISContext()
                Try
                    '    DDPersonalInfo = s_compQuery2.Invoke(context, SSN).SingleOrDefault()
                    DDPersonalInfo = (From dd In context.DDPersonnel_Application _
                            Where dd.Application_Sid = applicationID
                            Select New Objects.PersonalInformationDetails() With {
                                .FirstName = dd.First_Name,
                                .LastName = dd.Last_Name,
                                .Gender = dd.Gender,
                                .DOBDateOfIssuance = dd.DOB,
                                .RNLicenseOrSSN = dd.SSN,
                                .MiddleName = dd.Middle_Name,
                                .ApplicationSID = dd.Application_Sid
                            }).FirstOrDefault()
                    If (DDPersonalInfo IsNot Nothing) Then
                        DDPersonalInfo.AddressSID = (From addr In context.Application_Address_Xref
                                                     Where addr.Application_Sid = applicationID And addr.Active_Flg = True And addr.Address_Type_Sid = 2
                                                     Select addr.Address_Sid).FirstOrDefault()
                        If (DDPersonalInfo.AddressSID > 0) Then
                            Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", DDPersonalInfo.AddressSID)
                            Dim address As List(Of Address_Lookup_And_Insert_Result) = context.Address_Lookup_And_Insert(parameter, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).ToList()
                            DDPersonalInfo.AddressLine1 = address(0).Address_Line1
                            DDPersonalInfo.AddressLine2 = address(0).Address_Line2
                            DDPersonalInfo.County = address(0).County_Desc
                            DDPersonalInfo.State = address(0).State_Abbr
                            DDPersonalInfo.Zip = address(0).Zip
                            DDPersonalInfo.City = address(0).City
                            DDPersonalInfo.CountyID = address(0).CountyID
                            DDPersonalInfo.StateID = address(0).StateID
                        Else
                            DDPersonalInfo.AddressLine1 = String.Empty
                            DDPersonalInfo.AddressLine2 = String.Empty
                            DDPersonalInfo.County = String.Empty
                            DDPersonalInfo.State = String.Empty
                            DDPersonalInfo.Zip = String.Empty
                            DDPersonalInfo.City = String.Empty
                            DDPersonalInfo.CountyID = 0
                            DDPersonalInfo.StateID = 0
                        End If
                    End If

                    ListEmail = (From EmailRef In context.Application_Email_Xref
                                 Join dd In context.DDPersonnel_Application On dd.Application_Sid Equals EmailRef.Application_Sid
                                 Where dd.Application_Sid = applicationID And EmailRef.Active_Flg = True
                                 Order By EmailRef.Last_Update_Date Descending
                                 Select New Objects.EmailAddressDetails() With {
                                    .ContactType = EmailRef.Contact_Type_Sid,
                                    .EmailAddressSID = EmailRef.Email_Sid
                                     }).ToList()

                    For Each Email As Objects.EmailAddressDetails In ListEmail
                        Dim emailparameter As System.Data.Objects.ObjectParameter = New ObjectParameter("EmailSID", Email.EmailAddressSID)
                        Dim emailaddress As List(Of Email_Lookup_And_Insert_Result) = context.Email_Lookup_And_Insert(emailparameter, String.Empty).ToList()
                        Email.EmailAddress = emailaddress(0).Email_Address
                    Next
                    ListPhone = (From PhoneRef In context.Application_Phone_Xref
                                 Join dd In context.DDPersonnel_Application On dd.Application_Sid Equals PhoneRef.Application_Sid
                                 Where dd.Application_Sid = applicationID And PhoneRef.Active_Flg = True
                    Order By PhoneRef.Last_Update_Date Descending
                                 Select New Objects.PhoneDetails() With {
                                    .ContactType = PhoneRef.Contact_Type_Sid,
                                    .PhoneSID = PhoneRef.Phone_Sid
                                     }).ToList()
                    For Each phone As Objects.PhoneDetails In ListPhone
                        Dim phoneparameter As System.Data.Objects.ObjectParameter = New ObjectParameter("PhoneSID", phone.PhoneSID)
                        Dim phoneaddress As List(Of Phone_Number_Lookup_And_Insert_Result) = context.Phone_Number_Lookup_And_Insert(phoneparameter, String.Empty).ToList()
                        phone.PhoneNumber = phoneaddress(0).Phone_Number
                    Next
                    If (DDPersonalInfo IsNot Nothing) Then
                        If (ListPhone.Count > 0) Then
                            DDPersonalInfo.Phone = ListPhone
                        End If
                        If (ListEmail.Count > 0) Then
                            DDPersonalInfo.Email = ListEmail
                        End If
                    End If
                    'context.Connection.Close()
                Catch ex As Exception
                    Me.LogError("Error Getting DDPersonnel Info", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting DDPersonnel Info.", True, False))
                    Throw
                End Try
            End Using
            Return DDPersonalInfo
        End Function
        Public Function GetDDPersonnelInformationFromPermanent(ByVal ddpersonnelCode As String) As Objects.PersonalInformationDetails Implements IPersonalInformationQueries.GetDDPersonnelInformationFromPermanent
            Dim DDPersonalInfo As Objects.PersonalInformationDetails = Nothing
            Dim ListPhone As New List(Of Objects.PhoneDetails)
            Dim ListEmail As New List(Of Objects.EmailAddressDetails)
            Using context As New MAISContext()
                Try
                    '    DDPersonalInfo = s_compQuery2.Invoke(context, SSN).SingleOrDefault()
                    DDPersonalInfo = (From dd In context.DDPersonnels
                                      Join ddRef In context.RN_DD_Person_Type_Xref On dd.DDPersonnel_Sid Equals ddRef.RN_DDPersonnel_Sid
                            Where dd.DDPersonnel_Code = ddpersonnelCode
                            Select New Objects.PersonalInformationDetails() With {
                                .FirstName = dd.First_Name,
                                .LastName = dd.Last_Name,
                                .Gender = dd.Gender,
                                .DOBDateOfIssuance = dd.DOB,
                                .RNLicenseOrSSN = dd.SSN,
                                .MiddleName = dd.Middle_Name,
                                      .ApplicationSID = dd.DDPersonnel_Sid,
                                .DDPersonnelCode = dd.DDPersonnel_Code
                                       }).FirstOrDefault()
                    If (DDPersonalInfo IsNot Nothing) Then
                        DDPersonalInfo.AddressSID = (From dd In context.DDPersonnels _
                                       Join ddRef In context.RN_DD_Person_Type_Xref On dd.DDPersonnel_Sid Equals ddRef.RN_DDPersonnel_Sid
                                       Join addRef In context.RN_DD_Person_Type_Address_Xref On addRef.RN_DD_Person_Type_Xref_Sid Equals ddRef.RN_DD_Person_Type_Xref_Sid
                                    Where dd.DDPersonnel_Code = ddpersonnelCode And addRef.Address_Type_Sid = 2 Select addRef.Address_Sid).FirstOrDefault()
                        If (DDPersonalInfo.AddressSID > 0) Then
                            Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", DDPersonalInfo.AddressSID)
                            Dim address As List(Of Address_Lookup_And_Insert_Result) = context.Address_Lookup_And_Insert(parameter, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).ToList()
                            DDPersonalInfo.AddressLine1 = address(0).Address_Line1
                            DDPersonalInfo.AddressLine2 = address(0).Address_Line2
                            DDPersonalInfo.City = address(0).City
                            DDPersonalInfo.County = address(0).County_Desc
                            DDPersonalInfo.State = address(0).State_Abbr
                            DDPersonalInfo.Zip = address(0).Zip
                            DDPersonalInfo.City = address(0).City
                            DDPersonalInfo.CountyID = address(0).CountyID
                            DDPersonalInfo.StateID = address(0).StateID
                            DDPersonalInfo.AddressSID = parameter.Value
                        End If
                    End If
                    ListEmail = (From EmailRef In context.RN_DD_Person_Type_Email_Xref
                                 Join dd In context.RN_DD_Person_Type_Xref On dd.RN_DD_Person_Type_Xref_Sid Equals EmailRef.RN_DD_Person_Type_Xref_Sid
                                 Join dp In context.DDPersonnels On dp.DDPersonnel_Sid Equals dd.RN_DDPersonnel_Sid
                                 Where dp.DDPersonnel_Code = ddpersonnelCode And EmailRef.Active_Flg = True
                                 Order By EmailRef.Last_Update_By Descending
                                 Select New Objects.EmailAddressDetails() With {
                                    .ContactType = EmailRef.Contact_Type_Sid,
                                    .EmailAddressSID = EmailRef.Email_Sid
                                     }).ToList()
                    For Each Email As Objects.EmailAddressDetails In ListEmail
                        Dim emailparameter As System.Data.Objects.ObjectParameter = New ObjectParameter("EmailSID", Email.EmailAddressSID)
                        Dim emailaddress As List(Of Email_Lookup_And_Insert_Result) = context.Email_Lookup_And_Insert(emailparameter, String.Empty).ToList()
                        Email.EmailAddress = emailaddress(0).Email_Address
                    Next
                    ListPhone = (From PhoneRef In context.RN_DD_Person_Type_Phone_Xref
                                 Join dd In context.RN_DD_Person_Type_Xref On dd.RN_DD_Person_Type_Xref_Sid Equals PhoneRef.RN_DD_Person_Type_Xref_Sid
                                 Join dp In context.DDPersonnels On dp.DDPersonnel_Sid Equals dd.RN_DDPersonnel_Sid
                                 Where dp.DDPersonnel_Code = ddpersonnelCode And PhoneRef.Active_Flg = True
                                 Order By PhoneRef.Last_Update_Date Descending
                                 Select New Objects.PhoneDetails() With {
                                    .ContactType = PhoneRef.Contact_Type_Sid,
                                    .PhoneSID = PhoneRef.Phone_Sid
                                     }).ToList()
                    For Each phone As Objects.PhoneDetails In ListPhone
                        Dim phoneparameter As System.Data.Objects.ObjectParameter = New ObjectParameter("PhoneSID", phone.PhoneSID)
                        Dim phoneaddress As List(Of Phone_Number_Lookup_And_Insert_Result) = context.Phone_Number_Lookup_And_Insert(phoneparameter, String.Empty).ToList()
                        phone.PhoneNumber = phoneaddress(0).Phone_Number
                    Next
                    If (DDPersonalInfo IsNot Nothing) Then
                        If (ListPhone.Count > 0) Then
                            DDPersonalInfo.Phone = ListPhone
                        End If
                        If (ListEmail.Count > 0) Then
                            DDPersonalInfo.Email = ListEmail
                        End If
                    End If
                    'context.Connection.Close()
                Catch ex As Exception
                    Me.LogError("Error Getting DDPersonnel Info from permanent", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting DDPersonnel Info. from permanent", True, False))
                    Throw
                End Try
            End Using
            Return DDPersonalInfo
        End Function
        Public Function GetRNInformation(ByVal applicationID As Integer) As Objects.PersonalInformationDetails Implements IPersonalInformationQueries.GetRNInformation
            Dim DDPersonalInfo As Objects.PersonalInformationDetails = Nothing
            Dim ListPhone As New List(Of Objects.PhoneDetails)
            Dim ListEmail As New List(Of Objects.EmailAddressDetails)
            Using context As New MAISContext()
                Try
                    'context.Connection.Open()
                    DDPersonalInfo = (From rr In context.RN_Application _
                            Where rr.Application_Sid = applicationID
                            Select New Objects.PersonalInformationDetails() With {
                                .FirstName = rr.First_Name,
                                .LastName = rr.Last_Name,
                                .Gender = rr.Gender,
                                .DOBDateOfIssuance = rr.Date_Of_Original_Issuance,
                                .RNLicenseOrSSN = rr.RNLicense_Number,
                                .MiddleName = rr.Middle_Name
                             }).FirstOrDefault()
                    If (DDPersonalInfo IsNot Nothing) Then
                        DDPersonalInfo.AddressSID = (From addr In context.Application_Address_Xref
                                                   Where addr.Application_Sid = applicationID And addr.Active_Flg = True And addr.Address_Type_Sid = 2
                                                   Select addr.Address_Sid).FirstOrDefault()
                        If (DDPersonalInfo.AddressSID > 0) Then
                            Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", DDPersonalInfo.AddressSID)
                            Dim address As List(Of Address_Lookup_And_Insert_Result) = context.Address_Lookup_And_Insert(parameter, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).ToList()
                            DDPersonalInfo.AddressLine1 = address(0).Address_Line1
                            DDPersonalInfo.AddressLine2 = address(0).Address_Line2
                            DDPersonalInfo.County = address(0).County_Desc
                            DDPersonalInfo.CountyID = address(0).CountyID
                            DDPersonalInfo.State = address(0).State_Abbr
                            DDPersonalInfo.StateID = address(0).StateID
                            DDPersonalInfo.Zip = address(0).Zip
                            DDPersonalInfo.City = address(0).City
                        Else
                            DDPersonalInfo.AddressLine1 = String.Empty
                            DDPersonalInfo.AddressLine2 = String.Empty
                            DDPersonalInfo.County = String.Empty
                            DDPersonalInfo.CountyID = 0
                            DDPersonalInfo.State = String.Empty
                            DDPersonalInfo.StateID = 0
                            DDPersonalInfo.Zip = String.Empty
                            DDPersonalInfo.City = String.Empty
                        End If
                    End If

                    ListEmail = (From EmailRef In context.Application_Email_Xref
                                 Join dd In context.RN_Application On dd.Application_Sid Equals EmailRef.Application_Sid
                                 Where dd.Application_Sid = applicationID And EmailRef.Active_Flg = True
                                 Order By EmailRef.Last_Update_Date Descending
                                 Select New Objects.EmailAddressDetails() With {
                                    .ContactType = EmailRef.Contact_Type_Sid,
                                    .EmailAddressSID = EmailRef.Email_Sid
                                     }).ToList()
                    For Each Email As Objects.EmailAddressDetails In ListEmail
                        Dim emailparameter As System.Data.Objects.ObjectParameter = New ObjectParameter("EmailSID", Email.EmailAddressSID)
                        Dim emailaddress As List(Of Email_Lookup_And_Insert_Result) = context.Email_Lookup_And_Insert(emailparameter, String.Empty).ToList()
                        Email.EmailAddress = emailaddress(0).Email_Address
                    Next
                    ListPhone = (From PhoneRef In context.Application_Phone_Xref
                                 Join dd In context.RN_Application On dd.Application_Sid Equals PhoneRef.Application_Sid
                                 Where dd.Application_Sid = applicationID And PhoneRef.Active_Flg = True
                                 Order By PhoneRef.Last_Update_Date Descending
                                 Select New Objects.PhoneDetails() With {
                                    .ContactType = PhoneRef.Contact_Type_Sid,
                                    .PhoneSID = PhoneRef.Phone_Sid
                                     }).ToList()
                    For Each phone As Objects.PhoneDetails In ListPhone
                        Dim phoneparameter As System.Data.Objects.ObjectParameter = New ObjectParameter("PhoneSID", phone.PhoneSID)
                        Dim phoneaddress As List(Of Phone_Number_Lookup_And_Insert_Result) = context.Phone_Number_Lookup_And_Insert(phoneparameter, String.Empty).ToList()
                        phone.PhoneNumber = phoneaddress(0).Phone_Number
                    Next
                    If (DDPersonalInfo IsNot Nothing) Then
                        If (ListPhone.Count > 0) Then
                            DDPersonalInfo.Phone = ListPhone
                        End If
                        If (ListEmail.Count > 0) Then
                            DDPersonalInfo.Email = ListEmail
                        End If
                    End If
                Catch ex As Exception
                    Me.LogError("Error Getting RN Info", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting RN Info.", True, False))
                    Throw
                End Try
            End Using
            Return DDPersonalInfo
        End Function
        Public Function GetRNInformationFromPermanent(rnLicenseNumber As String) As Objects.PersonalInformationDetails Implements IPersonalInformationQueries.GetRNInformationFromPermanent
            Dim DDPersonalInfo As Objects.PersonalInformationDetails = Nothing
            Dim ListPhone As New List(Of Objects.PhoneDetails)
            Dim ListEmail As New List(Of Objects.EmailAddressDetails)
            Using context As New MAISContext()
                Try
                    'context.Connection.Open()
                    DDPersonalInfo = (From dd In context.RNs _
                                       Join ddRef In context.RN_DD_Person_Type_Xref On dd.RN_Sid Equals ddRef.RN_DDPersonnel_Sid
                                    Where dd.RNLicense_Number = rnLicenseNumber
                                Select New Objects.PersonalInformationDetails() With {
                                    .FirstName = dd.First_Name,
                                    .LastName = dd.Last_Name,
                                    .Gender = dd.Gender,
                                    .DOBDateOfIssuance = dd.Date_Of_Original_Issuance,
                                    .RNLicenseOrSSN = dd.RNLicense_Number,
                                    .MiddleName = dd.Middle_Name,
                                           .ApplicationSID = dd.RN_Sid
                                           }).FirstOrDefault()
                    If (DDPersonalInfo IsNot Nothing) Then
                        DDPersonalInfo.AddressSID = (From dd In context.RNs _
                                       Join ddRef In context.RN_DD_Person_Type_Xref On dd.RN_Sid Equals ddRef.RN_DDPersonnel_Sid
                                       Join addRef In context.RN_DD_Person_Type_Address_Xref On addRef.RN_DD_Person_Type_Xref_Sid Equals ddRef.RN_DD_Person_Type_Xref_Sid
                                    Where dd.RNLicense_Number = rnLicenseNumber And addRef.Address_Type_Sid = 2 Select addRef.Address_Sid).FirstOrDefault()
                        If (DDPersonalInfo.AddressSID > 0) Then
                            Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", DDPersonalInfo.AddressSID)
                            Dim address As List(Of Address_Lookup_And_Insert_Result) = context.Address_Lookup_And_Insert(parameter, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).ToList()
                            DDPersonalInfo.AddressLine1 = address(0).Address_Line1
                            DDPersonalInfo.AddressLine2 = address(0).Address_Line2
                            DDPersonalInfo.County = address(0).County_Desc
                            DDPersonalInfo.CountyID = address(0).CountyID
                            DDPersonalInfo.State = address(0).State_Abbr
                            DDPersonalInfo.StateID = address(0).StateID
                            DDPersonalInfo.Zip = address(0).Zip
                            DDPersonalInfo.City = address(0).City
                            DDPersonalInfo.AddressSID = parameter.Value
                        End If
                    End If
                    'Else
                    '    DDPersonalInfo.AddressLine1 = String.Empty
                    '    DDPersonalInfo.AddressLine2 = String.Empty
                    '    DDPersonalInfo.Zip = String.Empty
                    '    DDPersonalInfo.ZipPlus = String.Empty
                    '    DDPersonalInfo.City = String.Empty
                    '    DDPersonalInfo.County = String.Empty
                    '    DDPersonalInfo.State = String.Empty
                    '    DDPersonalInfo.AddressSID = 0
                    'End If

                    ListEmail = (From EmailRef In context.RN_DD_Person_Type_Email_Xref
                                  Join dd In context.RN_DD_Person_Type_Xref On dd.RN_DD_Person_Type_Xref_Sid Equals EmailRef.RN_DD_Person_Type_Xref_Sid
                                 Join rn In context.RNs On rn.RN_Sid Equals dd.RN_DDPersonnel_Sid
                                 Where rn.RNLicense_Number = rnLicenseNumber And EmailRef.Active_Flg = True _
                                 Order By EmailRef.Last_Update_Date Descending
                                 Select New Objects.EmailAddressDetails() With {
                                    .ContactType = EmailRef.Contact_Type_Sid,
                                    .EmailAddressSID = EmailRef.Email_Sid
                                     }).ToList()
                    For Each Email As Objects.EmailAddressDetails In ListEmail
                        Dim emailparameter As System.Data.Objects.ObjectParameter = New ObjectParameter("EmailSID", Email.EmailAddressSID)
                        Dim emailaddress As List(Of Email_Lookup_And_Insert_Result) = context.Email_Lookup_And_Insert(emailparameter, String.Empty).ToList()
                        Email.EmailAddress = emailaddress(0).Email_Address
                    Next
                    ListPhone = (From PhoneRef In context.RN_DD_Person_Type_Phone_Xref
                                 Join dd In context.RN_DD_Person_Type_Xref On dd.RN_DD_Person_Type_Xref_Sid Equals PhoneRef.RN_DD_Person_Type_Xref_Sid
                                 Join rn In context.RNs On rn.RN_Sid Equals dd.RN_DDPersonnel_Sid
                                 Where rn.RNLicense_Number = rnLicenseNumber And PhoneRef.Active_Flg = True
                                 Order By PhoneRef.Last_Update_Date Descending
                                 Select New Objects.PhoneDetails() With {
                                    .ContactType = PhoneRef.Contact_Type_Sid,
                                    .PhoneSID = PhoneRef.Phone_Sid
                                     }).ToList()
                    For Each phone As Objects.PhoneDetails In ListPhone
                        Dim phoneparameter As System.Data.Objects.ObjectParameter = New ObjectParameter("PhoneSID", phone.PhoneSID)
                        Dim phoneaddress As List(Of Phone_Number_Lookup_And_Insert_Result) = context.Phone_Number_Lookup_And_Insert(phoneparameter, String.Empty).ToList()
                        phone.PhoneNumber = phoneaddress(0).Phone_Number
                    Next
                    If (DDPersonalInfo IsNot Nothing) Then
                        If (ListPhone.Count > 0) Then
                            DDPersonalInfo.Phone = ListPhone
                        End If
                        If (ListEmail.Count > 0) Then
                            DDPersonalInfo.Email = ListEmail
                        End If
                    End If
                Catch ex As Exception
                    Me.LogError("Error Getting RN Info from permanent", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting RN Info. from permanent", True, False))
                    Throw
                End Try
            End Using
            Return DDPersonalInfo
        End Function
        Public Function SavePersonalInformation(ByVal personalResult As Objects.PersonalInformationDetails, ByVal appId As Integer, ByVal rnOrDD As Boolean, ByVal name As String, ByVal brandNew As Boolean, ByVal uniqueCode As String _
                                                , ByVal adminPerm As Boolean, ByVal queryString As String) As ReturnObject(Of Long) Implements IPersonalInformationQueries.SavePersonalInformation
            Dim retval As New ReturnObject(Of Long)(-1L)
            Using context As New MAISContext()
                Try
                    If (String.IsNullOrEmpty(queryString)) Then
                        If (personalResult IsNot Nothing) Then
                            If (brandNew) Then
                                If (rnOrDD) Then
                                    Dim _rndetails As RN = Nothing
                                    _rndetails = (From rn In context.RNs
                                                  Where rn.RNLicense_Number = personalResult.RNLicenseOrSSN Select rn).FirstOrDefault()
                                    If (_rndetails IsNot Nothing) Then
                                        retval.AddErrorMessage("This RN is already exists in our system, Please use a new RN details")
                                        Return retval
                                    End If
                                    Dim _rndetails1 As RN_Application = Nothing
                                    _rndetails1 = (From rn In context.RN_Application
                                                  Join app In context.Applications On app.Application_Sid Equals rn.Application_Sid
                                                  Where rn.RNLicense_Number = personalResult.RNLicenseOrSSN And (app.Application_Status_Type_Sid = 1 Or app.Application_Status_Type_Sid = 6 Or app.Application_Status_Type_Sid = 12) _
                                                  And app.Application_Sid <> appId Select rn).FirstOrDefault()
                                    If (_rndetails1 IsNot Nothing) Then
                                        retval.AddErrorMessage("This RN is already exists in our system, Please use a new RN details")
                                        Return retval
                                    End If
                                Else
                                    If (adminPerm = False) Then
                                        Dim _dddetails As DDPersonnel = Nothing
                                        _dddetails = (From rn In context.DDPersonnels
                                                              Where rn.SSN = personalResult.RNLicenseOrSSN And rn.DOB = personalResult.DOBDateOfIssuance Select rn).FirstOrDefault()
                                        If (_dddetails IsNot Nothing) Then
                                            retval.AddErrorMessage("Last 4 and DOB already exist please verify Identification and re-enter or contact DODD Admin at ma.database@dodd.ohio.gov or 1-800-617-6733.")
                                            Return retval
                                        End If
                                        Dim _dddetails1 As DDPersonnel_Application = Nothing
                                        _dddetails1 = (From rn In context.DDPersonnel_Application
                                                      Join app In context.Applications On app.Application_Sid Equals rn.Application_Sid
                                                      Where rn.SSN = personalResult.RNLicenseOrSSN And rn.DOB = personalResult.DOBDateOfIssuance _
                                                      And (app.Application_Status_Type_Sid = 1 Or app.Application_Status_Type_Sid = 6 Or app.Application_Status_Type_Sid = 12) And app.Application_Sid <> appId Select rn).FirstOrDefault()
                                        If (_dddetails1 IsNot Nothing) Then
                                            retval.AddErrorMessage("Last 4 and DOB already exist please verify Identification and re-enter or contact DODD Admin at ma.database@dodd.ohio.gov or 1-800-617-6733.")
                                            Return retval
                                        End If
                                    End If
                                End If
                            Else
                                If (rnOrDD) Then
                                    If (uniqueCode.ToUpper() <> personalResult.RNLicenseOrSSN.ToUpper()) Then
                                        Dim _rndetails As RN_Application = Nothing
                                        _rndetails = (From rn In context.RN_Application
                                                      Join app In context.Applications On app.Application_Sid Equals rn.Application_Sid
                                                      Where rn.RNLicense_Number = personalResult.RNLicenseOrSSN And app.Application_Sid <> appId And (app.Application_Status_Type_Sid = 1 Or app.Application_Status_Type_Sid = 6 Or app.Application_Status_Type_Sid = 12) Select rn).FirstOrDefault()
                                        If (_rndetails IsNot Nothing) Then
                                            retval.AddErrorMessage("This RN is already exists in our system, Please use a new RN details")
                                            Return retval
                                        End If
                                    End If
                                Else
                                    If (adminPerm = False) Then
                                        If (Not String.IsNullOrEmpty(uniqueCode)) Then
                                            If (uniqueCode.ToUpper() <> personalResult.DDPersonnelCode.ToUpper()) Then
                                                Dim _dddetails As DDPersonnel_Application = Nothing
                                                _dddetails = (From rn In context.DDPersonnel_Application
                                                              Join app In context.Applications On app.Application_Sid Equals rn.Application_Sid
                                                              Where rn.SSN = personalResult.RNLicenseOrSSN And rn.DOB = personalResult.DOBDateOfIssuance And app.Application_Sid <> appId _
                                                              And (app.Application_Status_Type_Sid = 1 Or app.Application_Status_Type_Sid = 6 Or app.Application_Status_Type_Sid = 12) Select rn).FirstOrDefault()
                                                If (_dddetails IsNot Nothing) Then
                                                    retval.AddErrorMessage("Last 4 SSN and DOB already exist please verify Identification and re-enter or contact DODD Admin at ma.database@dodd.ohio.gov or 1-800-617-6733.")
                                                    Return retval
                                                End If
                                            End If
                                        Else
                                            Dim _dddetails As DDPersonnel = Nothing
                                            _dddetails = (From rn In context.DDPersonnels
                                                          Where rn.SSN = personalResult.RNLicenseOrSSN And rn.DOB = personalResult.DOBDateOfIssuance Select rn).FirstOrDefault()
                                            If (_dddetails IsNot Nothing) Then
                                                retval.AddErrorMessage("Last 4 SSN and DOB already exist please verify Identification and re-enter or contact DODD Admin at ma.database@dodd.ohio.gov or 1-800-617-6733.")
                                                Return retval
                                            End If
                                            Dim _dddetails1 As DDPersonnel_Application = Nothing
                                            _dddetails1 = (From rn In context.DDPersonnel_Application
                                                          Join app In context.Applications On app.Application_Sid Equals rn.Application_Sid
                                                          Where rn.SSN = personalResult.RNLicenseOrSSN And rn.Last_Name = personalResult.LastName And rn.First_Name = personalResult.FirstName _
                                                          And rn.Middle_Name = personalResult.MiddleName And rn.DOB = personalResult.DOBDateOfIssuance And app.Application_Sid <> appId _
                                                          And (app.Application_Status_Type_Sid = 1 Or app.Application_Status_Type_Sid = 6 Or app.Application_Status_Type_Sid = 12) Select rn).FirstOrDefault()
                                            If (_dddetails1 IsNot Nothing) Then
                                                retval.AddErrorMessage("This DD personnel is already exists in our system, Please use a new DD personnel details")
                                                Return retval
                                            End If
                                        End If
                                    Else
                                        If (rnOrDD) Then
                                            If (uniqueCode.ToUpper() <> personalResult.RNLicenseOrSSN.ToUpper()) Then
                                                Dim _rndetails As RN_Application = Nothing
                                                _rndetails = (From rn In context.RN_Application
                                                              Join app In context.Applications On app.Application_Sid Equals rn.Application_Sid
                                                              Where rn.RNLicense_Number = personalResult.RNLicenseOrSSN And (app.Application_Status_Type_Sid = 1 Or app.Application_Status_Type_Sid = 6 Or app.Application_Status_Type_Sid = 12) _
                                                              And app.Application_Sid <> appId Select rn).FirstOrDefault()
                                                If (_rndetails IsNot Nothing) Then
                                                    retval.AddErrorMessage("This RN is already exists in our system, Please use a new RN details")
                                                    Return retval
                                                End If
                                            End If
                                        Else
                                            If (Not String.IsNullOrEmpty(uniqueCode)) Then
                                                If (uniqueCode.ToUpper() <> personalResult.DDPersonnelCode.ToUpper()) Then
                                                    Dim _dddetails As DDPersonnel_Application = Nothing
                                                    _dddetails = (From rn In context.DDPersonnel_Application
                                                                  Join app In context.Applications On app.Application_Sid Equals rn.Application_Sid
                                                                  Where rn.SSN = personalResult.RNLicenseOrSSN And rn.Last_Name = personalResult.LastName And rn.First_Name = personalResult.FirstName _
                                                                  And rn.Middle_Name = personalResult.MiddleName And rn.DOB = personalResult.DOBDateOfIssuance And app.Application_Sid <> appId _
                                                                  And (app.Application_Status_Type_Sid = 1 Or app.Application_Status_Type_Sid = 6 Or app.Application_Status_Type_Sid = 12) And app.Application_Sid <> appId Select rn).FirstOrDefault()
                                                    If (_dddetails IsNot Nothing) Then
                                                        retval.AddErrorMessage("This DD personnel is already exists in our system, Please use a new DD personnel details")
                                                        Return retval
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If

                            If (rnOrDD) Then
                                Dim _rndetails As RN_Application = Nothing
                                _rndetails = (From rn In context.RN_Application
                                              Where rn.Application_Sid = appId
                                              Select rn).FirstOrDefault()
                                If (_rndetails Is Nothing) Then
                                    Dim _rnInsert As RN_Application = New RN_Application()
                                    _rnInsert.RNLicense_Number = personalResult.RNLicenseOrSSN
                                    _rnInsert.Date_Of_Original_Issuance = personalResult.DOBDateOfIssuance
                                    _rnInsert.Middle_Name = personalResult.MiddleName
                                    _rnInsert.Application_Sid = appId
                                    _rnInsert.First_Name = personalResult.FirstName
                                    _rnInsert.Last_Name = personalResult.LastName
                                    _rnInsert.Gender = personalResult.Gender
                                    _rnInsert.Create_By = Me.UserID
                                    _rnInsert.Create_Date = DateTime.Now
                                    _rnInsert.Last_Update_By = Me.UserID
                                    _rnInsert.Last_Updated_Date = DateTime.Now
                                    context.RN_Application.Add(_rnInsert)
                                Else
                                    _rndetails.RNLicense_Number = personalResult.RNLicenseOrSSN
                                    _rndetails.Date_Of_Original_Issuance = personalResult.DOBDateOfIssuance
                                    _rndetails.Middle_Name = personalResult.MiddleName
                                    _rndetails.First_Name = personalResult.FirstName
                                    _rndetails.Last_Name = personalResult.LastName
                                    _rndetails.Gender = personalResult.Gender
                                    _rndetails.Last_Update_By = Me.UserID
                                    _rndetails.Last_Updated_Date = DateTime.Now
                                End If
                            Else
                                Dim _rndetails As DDPersonnel_Application = Nothing
                                _rndetails = (From rn In context.DDPersonnel_Application
                                              Where rn.Application_Sid = appId
                                              Select rn).FirstOrDefault()
                                If (_rndetails Is Nothing) Then
                                    Dim _ddInsert As DDPersonnel_Application = New DDPersonnel_Application()
                                    _ddInsert.SSN = personalResult.RNLicenseOrSSN
                                    _ddInsert.DOB = personalResult.DOBDateOfIssuance
                                    _ddInsert.Middle_Name = personalResult.MiddleName
                                    _ddInsert.Application_Sid = appId
                                    _ddInsert.First_Name = personalResult.FirstName
                                    _ddInsert.Last_Name = personalResult.LastName
                                    _ddInsert.Gender = personalResult.Gender
                                    _ddInsert.Create_By = Me.UserID
                                    _ddInsert.Create_Date = DateTime.Now
                                    _ddInsert.Last_Update_By = Me.UserID
                                    _ddInsert.Last_Updated_Date = DateTime.Now
                                    context.DDPersonnel_Application.Add(_ddInsert)
                                Else
                                    _rndetails.Middle_Name = personalResult.MiddleName
                                    _rndetails.First_Name = personalResult.FirstName
                                    _rndetails.Last_Name = personalResult.LastName
                                    _rndetails.DOB = personalResult.DOBDateOfIssuance
                                    _rndetails.Gender = personalResult.Gender
                                    _rndetails.SSN = personalResult.RNLicenseOrSSN
                                    _rndetails.Last_Update_By = Me.UserID
                                    _rndetails.Last_Updated_Date = DateTime.Now
                                End If
                            End If

                            If ((Not String.IsNullOrWhiteSpace(personalResult.AddressLine1)) And
                                 (Not String.IsNullOrWhiteSpace(personalResult.City)) And
                                 (Not String.IsNullOrWhiteSpace(personalResult.County)) And
                                 (Not String.IsNullOrWhiteSpace(personalResult.Zip))) Then
                                Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", GetType(Integer))
                                Dim _address As List(Of Address_Lookup_And_Insert_Result) = context.Address_Lookup_And_Insert(parameter, String.Empty, personalResult.AddressLine1, personalResult.AddressLine2, String.Empty, Convert.ToInt32(personalResult.County) _
                                                                                                                              , Convert.ToInt32(personalResult.State), personalResult.Zip + personalResult.ZipPlus, personalResult.City, 0).ToList()

                                Dim _existingAddRef As Application_Address_Xref = Nothing
                                _existingAddRef = (From addR In context.Application_Address_Xref Where addR.Address_Type_Sid = 2 And addR.Application_Sid = appId _
                                                   And addR.Active_Flg = True Select addR).FirstOrDefault()

                                If (_existingAddRef IsNot Nothing) Then
                                    _existingAddRef.Address_Sid = parameter.Value
                                    _existingAddRef.Address_Type_Sid = 2
                                    _existingAddRef.Application_Sid = appId
                                    _existingAddRef.Active_Flg = True
                                    _existingAddRef.Last_Update_By = Me.UserID
                                    _existingAddRef.Last_Update_Date = DateTime.Now
                                Else
                                    Dim _addressRef1 As Application_Address_Xref = New Application_Address_Xref()
                                    _addressRef1.Address_Sid = parameter.Value
                                    _addressRef1.Address_Type_Sid = 2
                                    _addressRef1.Application_Sid = appId
                                    _addressRef1.Active_Flg = True
                                    _addressRef1.Create_By = Me.UserID
                                    _addressRef1.Create_Date = DateTime.Now
                                    _addressRef1.Last_Update_By = Me.UserID
                                    _addressRef1.Last_Update_Date = DateTime.Now
                                    context.Application_Address_Xref.Add(_addressRef1)
                                End If
                            End If
                            For Each ph As Objects.PhoneDetails In personalResult.Phone
                                Dim phoneParameter As System.Data.Objects.ObjectParameter = New ObjectParameter("PhoneSID", GetType(Integer))
                                Dim _phoneaddress As List(Of Phone_Number_Lookup_And_Insert_Result) = context.Phone_Number_Lookup_And_Insert(phoneParameter, ph.PhoneNumber).ToList()
                                Dim _existingRefPhone As Application_Phone_Xref = Nothing
                                _existingRefPhone = (From phoneRef In context.Application_Phone_Xref Where phoneRef.Application_Sid = appId And phoneRef.Contact_Type_Sid = ph.ContactType _
                                                And phoneRef.Active_Flg = True Select phoneRef).FirstOrDefault()
                                If (_existingRefPhone Is Nothing) Then
                                    Dim _phoneRef1 As Application_Phone_Xref = New Application_Phone_Xref()
                                    _phoneRef1.Application_Sid = appId
                                    _phoneRef1.Phone_Sid = _phoneaddress(0).Phone_Number_SID
                                    _phoneRef1.Contact_Type_Sid = ph.ContactType
                                    _phoneRef1.Active_Flg = True
                                    _phoneRef1.Create_Date = DateTime.Now
                                    _phoneRef1.Last_Update_By = Me.UserID
                                    _phoneRef1.Create_By = Me.UserID
                                    _phoneRef1.Last_Update_Date = DateTime.Now
                                    context.Application_Phone_Xref.Add(_phoneRef1)
                                    context.SaveChanges()
                                Else
                                    _existingRefPhone.Application_Sid = appId
                                    _existingRefPhone.Phone_Sid = _phoneaddress(0).Phone_Number_SID
                                    _existingRefPhone.Contact_Type_Sid = ph.ContactType
                                    _existingRefPhone.Active_Flg = True
                                    _existingRefPhone.Last_Update_By = Me.UserID
                                    _existingRefPhone.Last_Update_Date = DateTime.Now
                                    context.SaveChanges()
                                End If
                            Next

                            For Each ph As Objects.EmailAddressDetails In personalResult.Email
                                Dim emailParameter As System.Data.Objects.ObjectParameter = New ObjectParameter("EmailSID", GetType(Integer))
                                Dim _emailaddress As List(Of Email_Lookup_And_Insert_Result) = context.Email_Lookup_And_Insert(emailParameter, ph.EmailAddress).ToList()
                                Dim _existingRefEmail As Application_Email_Xref = Nothing
                                _existingRefEmail = (From phoneRef In context.Application_Email_Xref Where phoneRef.Application_Sid = appId And phoneRef.Contact_Type_Sid = ph.ContactType _
                                                And phoneRef.Active_Flg = True Select phoneRef).FirstOrDefault()
                                If (_existingRefEmail Is Nothing) Then
                                    Dim _emailRef1 As Application_Email_Xref = New Application_Email_Xref()
                                    _emailRef1.Application_Sid = appId
                                    _emailRef1.Email_Sid = _emailaddress(0).Email_SID
                                    _emailRef1.Contact_Type_Sid = ph.ContactType
                                    _emailRef1.Active_Flg = True
                                    _emailRef1.Create_Date = DateTime.Now
                                    _emailRef1.Create_By = Me.UserID
                                    _emailRef1.Last_Update_By = Me.UserID
                                    _emailRef1.Create_Date = DateTime.Now
                                    _emailRef1.Last_Update_Date = DateTime.Now
                                    context.Application_Email_Xref.Add(_emailRef1)
                                    context.SaveChanges()
                                Else
                                    _existingRefEmail.Application_Sid = appId
                                    _existingRefEmail.Email_Sid = _emailaddress(0).Email_SID
                                    _existingRefEmail.Contact_Type_Sid = ph.ContactType
                                    _existingRefEmail.Active_Flg = True
                                    _existingRefEmail.Last_Update_By = Me.UserID
                                    '_existingRefEmail.Create_Date = DateTime.Now
                                    _existingRefEmail.Last_Update_Date = DateTime.Now
                                    context.SaveChanges()
                                End If
                            Next
                            If appId > 0 Then  ' add application history record
                                Dim appInfo As Application = (From ai In context.Applications Where ai.Application_Sid = appId Select ai).FirstOrDefault()
                                Dim RN_DD_Xref_ID As Integer = 0
                                If (rnOrDD) Then
                                    RN_DD_Xref_ID = (From r In context.RNs
                                                     Join rr In context.RN_DD_Person_Type_Xref On r.RN_Sid Equals rr.RN_DDPersonnel_Sid
                                                     Where r.RNLicense_Number = personalResult.RNLicenseOrSSN
                                                     Select rr.RN_DD_Person_Type_Xref_Sid).FirstOrDefault()
                                Else
                                    RN_DD_Xref_ID = (From d In context.DDPersonnels
                                                     Join rr In context.RN_DD_Person_Type_Xref On d.DDPersonnel_Sid Equals rr.RN_DDPersonnel_Sid
                                                     Where d.DDPersonnel_Code = personalResult.DDPersonnelCode
                                                     Select rr.RN_DD_Person_Type_Xref_Sid).FirstOrDefault()
                                End If
                                Dim existingApp As Application_History = (From aa In context.Application_History Where aa.Application_Sid = appId Select aa).FirstOrDefault()
                                If appInfo IsNot Nothing Then
                                    If existingApp Is Nothing Then
                                        Dim appHistory As Application_History = New Application_History()
                                        appHistory.Application_Sid = appInfo.Application_Sid
                                        appHistory.Application_Type_Sid = appInfo.Application_Type_Sid
                                        appHistory.Role_Category_Level_Sid = appInfo.Role_Category_Level_Sid
                                        If RN_DD_Xref_ID > 0 Then
                                            appHistory.RN_DD_Person_Type_Xref_Sid = RN_DD_Xref_ID
                                        End If
                                        appHistory.RN_License_Or_4SSN = personalResult.RNLicenseOrSSN
                                        appHistory.Create_By = Me.UserID
                                        appHistory.Create_Date = DateTime.Now
                                        appHistory.Last_Update_By = Me.UserID
                                        appHistory.Last_Update_Date = DateTime.Now
                                        context.Application_History.Add(appHistory)

                                        Dim appStatus As Application_History_Status = New Application_History_Status()
                                        appStatus.Application_History = appHistory
                                        appStatus.Application_Status_Type_Sid = appInfo.Application_Status_Type_Sid
                                        appStatus.Create_By = Me.UserID
                                        appStatus.Create_Date = DateTime.Now
                                        appStatus.Last_Update_By = Me.UserID
                                        appStatus.Last_Update_Date = DateTime.Now
                                        context.Application_History_Status.Add(appStatus)
                                    Else
                                        existingApp.Application_Sid = appInfo.Application_Sid
                                        existingApp.Application_Type_Sid = appInfo.Application_Type_Sid
                                        existingApp.Role_Category_Level_Sid = appInfo.Role_Category_Level_Sid
                                        If RN_DD_Xref_ID > 0 Then
                                            existingApp.RN_DD_Person_Type_Xref_Sid = RN_DD_Xref_ID
                                        End If
                                        existingApp.RN_License_Or_4SSN = personalResult.RNLicenseOrSSN
                                        existingApp.Last_Update_By = Me.UserID
                                        existingApp.Last_Update_Date = DateTime.Now
                                    End If
                                End If
                            End If
                            context.SaveChanges()
                        End If
                    Else
                        Dim rntable As Integer = 0
                        If (rnOrDD) Then
                            rntable = (From r In context.RNs
                                       Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DDPersonnel_Sid Equals r.RN_Sid
                                       Where r.RNLicense_Number = uniqueCode
                                    Select rn.RN_DD_Person_Type_Xref_Sid).FirstOrDefault()
                        Else
                            rntable = (From r In context.DDPersonnels
                                       Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DDPersonnel_Sid Equals r.DDPersonnel_Sid
                                       Where r.DDPersonnel_Code = uniqueCode
                                    Select rn.RN_DD_Person_Type_Xref_Sid).FirstOrDefault()
                        End If
                        Dim parameter1 As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", GetType(Integer))
                        Dim _address As List(Of Address_Lookup_And_Insert_Result) = context.Address_Lookup_And_Insert(parameter1, String.Empty, personalResult.AddressLine1, personalResult.AddressLine2, String.Empty, Convert.ToInt32(personalResult.County) _
                                                                                                                      , Convert.ToInt32(personalResult.State), personalResult.Zip + personalResult.ZipPlus, personalResult.City, 0).ToList()
                        Dim _existingAddRef As RN_DD_Person_Type_Address_Xref = Nothing
                        _existingAddRef = (From addR In context.RN_DD_Person_Type_Address_Xref Where addR.Address_Type_Sid = 2 And addR.RN_DD_Person_Type_Xref_Sid = rntable _
                                           And addR.Active_Flg = True Select addR).FirstOrDefault()
                        If (_existingAddRef Is Nothing) Then
                            Dim _addressRef1 As RN_DD_Person_Type_Address_Xref = New RN_DD_Person_Type_Address_Xref()
                            _addressRef1.Address_Sid = parameter1.Value
                            _addressRef1.Address_Type_Sid = 2
                            _addressRef1.RN_DD_Person_Type_Xref_Sid = rntable
                            _addressRef1.Active_Flg = True
                            _addressRef1.Create_By = Me.UserID
                            _addressRef1.Create_Date = DateTime.Today
                            _addressRef1.Last_Update_By = Me.UserID
                            _addressRef1.Last_Update_Date = DateTime.Today
                            _addressRef1.Start_Date = Date.Today 'DateTime.Now
                            _addressRef1.End_Date = CDate("12/31/9999")
                            context.RN_DD_Person_Type_Address_Xref.Add(_addressRef1)
                        Else
                            _existingAddRef.Address_Sid = parameter1.Value
                            _existingAddRef.Address_Type_Sid = 2
                            _existingAddRef.Active_Flg = True
                            _existingAddRef.Last_Update_By = Me.UserID
                            _existingAddRef.Last_Update_Date = DateTime.Today
                        End If
                        context.SaveChanges()

                        For Each ph As Objects.PhoneDetails In personalResult.Phone
                            Dim phoneParameter1 As System.Data.Objects.ObjectParameter = New ObjectParameter("PhoneSID", GetType(Integer))
                            Dim _phoneaddress1 As List(Of Phone_Number_Lookup_And_Insert_Result) = context.Phone_Number_Lookup_And_Insert(phoneParameter1, ph.PhoneNumber).ToList()
                            Dim _existingRefPhone As RN_DD_Person_Type_Phone_Xref = Nothing
                            _existingRefPhone = (From phoneRef In context.RN_DD_Person_Type_Phone_Xref Where phoneRef.RN_DD_Person_Type_Xref_Sid = rntable And phoneRef.Contact_Type_Sid = ph.ContactType _
                                            And phoneRef.Active_Flg = True Select phoneRef).FirstOrDefault()
                            If (_existingRefPhone Is Nothing) Then
                                Dim _phoneRef1 As RN_DD_Person_Type_Phone_Xref = New RN_DD_Person_Type_Phone_Xref()
                                _phoneRef1.RN_DD_Person_Type_Xref_Sid = rntable
                                _phoneRef1.Phone_Sid = _phoneaddress1(0).Phone_Number_SID
                                _phoneRef1.Contact_Type_Sid = ph.ContactType
                                _phoneRef1.Active_Flg = True
                                _phoneRef1.Create_Date = DateTime.Now
                                _phoneRef1.Last_Update_By = Me.UserID
                                _phoneRef1.Create_By = Me.UserID
                                _phoneRef1.Last_Update_Date = DateTime.Now
                                _phoneRef1.Start_Date = Date.Today 'DateTime.Now
                                _phoneRef1.End_Date = CDate("12/31/9999")
                                context.RN_DD_Person_Type_Phone_Xref.Add(_phoneRef1)
                                context.SaveChanges()
                            Else
                                '_existingRefPhone.Application_Sid = appId
                                _existingRefPhone.Phone_Sid = _phoneaddress1(0).Phone_Number_SID
                                _existingRefPhone.Contact_Type_Sid = ph.ContactType
                                _existingRefPhone.Active_Flg = True
                                _existingRefPhone.Last_Update_By = Me.UserID
                                _existingRefPhone.Last_Update_Date = DateTime.Now
                                context.SaveChanges()
                            End If
                        Next

                        For Each ph As Objects.EmailAddressDetails In personalResult.Email
                            Dim emailParameter1 As System.Data.Objects.ObjectParameter = New ObjectParameter("EmailSID", GetType(Integer))
                            Dim _emailaddress1 As List(Of Email_Lookup_And_Insert_Result) = context.Email_Lookup_And_Insert(emailParameter1, ph.EmailAddress).ToList()
                            Dim _existingRefEmail As RN_DD_Person_Type_Email_Xref = Nothing
                            _existingRefEmail = (From emailRef In context.RN_DD_Person_Type_Email_Xref Where emailRef.RN_DD_Person_Type_Xref_Sid = rntable And emailRef.Contact_Type_Sid = ph.ContactType _
                                        And emailRef.Active_Flg = True Select emailRef).FirstOrDefault()
                            If (_existingRefEmail Is Nothing) Then
                                Dim _emailRef1 As RN_DD_Person_Type_Email_Xref = New RN_DD_Person_Type_Email_Xref()
                                ' _emailRef1.RN_DD_Person_Type_Email_Xref_Sid = rntable
                                _emailRef1.RN_DD_Person_Type_Xref_Sid = rntable
                                _emailRef1.Email_Sid = _emailaddress1(0).Email_SID
                                _emailRef1.Contact_Type_Sid = ph.ContactType
                                _emailRef1.Active_Flg = True
                                _emailRef1.Create_Date = DateTime.Now
                                _emailRef1.Create_By = Me.UserID
                                _emailRef1.Last_Update_By = Me.UserID
                                _emailRef1.Create_Date = DateTime.Now
                                _emailRef1.Last_Update_Date = DateTime.Now
                                _emailRef1.Start_Date = Date.Today 'DateTime.Now
                                _emailRef1.End_Date = CDate("12/31/9999")
                                context.RN_DD_Person_Type_Email_Xref.Add(_emailRef1)
                                context.SaveChanges()
                            Else
                                '_existingRefEmail.Application_Sid = appId
                                _existingRefEmail.Email_Sid = _emailaddress1(0).Email_SID
                                _existingRefEmail.Contact_Type_Sid = ph.ContactType
                                _existingRefEmail.Active_Flg = True
                                _existingRefEmail.Last_Update_By = Me.UserID
                                '_existingRefEmail.Create_Date = DateTime.Now
                                _existingRefEmail.Last_Update_Date = DateTime.Now
                                context.SaveChanges()
                            End If
                        Next
                    End If
                Catch ex As Exception
                    Me._messages.Add(New ReturnMessage("Error while personal information services.", True, False))
                    Me.LogError("Error while saving personal information services.", CInt(Me.UserID), ex)
                    retval.AddErrorMessage(ex.Message)
                End Try
            End Using
            Return retval
        End Function

        Public Function DeleteTheDataApplication(applicationID As Integer) As Boolean Implements IPersonalInformationQueries.DeleteTheDataApplication
            Dim flag As Boolean = False
            Using context As New MAISContext()
                Try
                    Dim _maApplication As Application = (From app In context.Applications Where app.Application_Sid = applicationID Select app).FirstOrDefault()
                    If (_maApplication IsNot Nothing) Then
                        If (_maApplication.Application_Sid > 0) Then
                            flag = True
                            context.Applications.Remove(_maApplication)
                            context.SaveChanges()
                        End If
                    End If
                Catch ex As Exception
                    Me.LogError("Error Getting deleting application Info", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting deleting application Info.", True, False))
                    Throw
                End Try
            End Using
            Return flag
        End Function
        Public Function GetPersonalPageComplete(applicationID As Integer) As Integer Implements IPersonalInformationQueries.GetPersonalPageComplete
            Dim exists As Integer = 0
            Using context As New MAISContext()
                Try
                    'exists = (From app In context.RN_Application _
                    '                                        Where app.Application_Sid = applicationID Select app.Application_Sid).FirstOrDefault()
                    'If (exists = 0) Then
                    '    exists = (From app In context.DDPersonnel_Application _
                    '                                            Where app.Application_Sid = applicationID Select app.Application_Sid).FirstOrDefault()
                    'End If
                    exists = (From app In context.Applications
                              Join r In context.RN_Application On app.Application_Sid Equals r.Application_Sid
                              Join a In context.Application_Address_Xref On a.Application_Sid Equals app.Application_Sid
                              Join p In context.Application_Phone_Xref On p.Application_Sid Equals app.Application_Sid
                              Join e In context.Application_Email_Xref On e.Application_Sid Equals app.Application_Sid
                              Where app.Application_Sid = applicationID Select app.Application_Sid).FirstOrDefault()
                    If (exists = 0) Then
                        exists = (From app In context.Applications
                              Join d In context.DDPersonnel_Application On app.Application_Sid Equals d.Application_Sid
                              Join a In context.Application_Address_Xref On a.Application_Sid Equals app.Application_Sid
                              Join p In context.Application_Phone_Xref On p.Application_Sid Equals app.Application_Sid
                              Join e In context.Application_Email_Xref On e.Application_Sid Equals app.Application_Sid
                              Where app.Application_Sid = applicationID Select app.Application_Sid).FirstOrDefault()

                    End If
                Catch ex As Exception
                    Me.LogError("Error Getting personal page complete rule.", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error personal page complete rule.", True, False))
                    Throw
                End Try
            End Using
            Return exists
        End Function

        Public Function GetPartialPersonalPageComplete(applicationID As Integer) As Integer Implements IPersonalInformationQueries.GetPartialPersonalPageComplete
            Dim exists As Integer = 0
            Using context As New MAISContext()
                Try                  
                    exists = (From app In context.Applications
                              Join r In context.RN_Application On app.Application_Sid Equals r.Application_Sid                             
                              Where app.Application_Sid = applicationID Select app.Application_Sid).FirstOrDefault()
                    If (exists = 0) Then
                        exists = (From app In context.Applications
                              Join d In context.DDPersonnel_Application On app.Application_Sid Equals d.Application_Sid                            
                              Where app.Application_Sid = applicationID Select app.Application_Sid).FirstOrDefault()

                    End If
                Catch ex As Exception
                    Me.LogError("Error Getting partial personal page complete rule.", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error getting partial personal page complete rule.", True, False))
                    Throw
                End Try
            End Using
            Return exists
        End Function

    End Class
End Namespace

