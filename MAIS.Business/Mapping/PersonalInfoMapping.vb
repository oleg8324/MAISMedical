Imports MAIS.Data

Namespace Mapping
    Public Class PersonalInfoMapping
        'Public Shared Function MapOnlyPErsonnalInfo(ByVal pinfo As Objects.PersonalInformationDetails) As Model.PersonalInformationDetails
        '    Dim pp As New Model.PersonalInformationDetails
        '    If pinfo IsNot Nothing Then
        '        With pp
        '            .FirstName = pinfo.FirstName
        '            .LastName = pinfo.LastName
        '            .MiddleName = pinfo.MiddleName
        '            If Not String.IsNullOrWhiteSpace(pinfo.Gender) Then
        '                .Gender = pinfo.Gender
        '            Else
        '                .Gender = String.Empty
        '            End If
        '            .DOBDateOfIssuance = pinfo.DOBDateOfIssuance
        '            .RNLicenseOrSSN = pinfo.RNLicenseOrSSN
        '        End With
        '    End If
        '    Return pp
        'End Function
        Public Shared Function MapPersonalResultToDB(ByVal personalResult As Model.PersonalInformationDetails) As Data.Objects.PersonalInformationDetails
            If personalResult Is Nothing Then
                Return Nothing
            Else
                Dim obj As New Data.Objects.PersonalInformationDetails()
                Dim listPhoneObj As New List(Of Data.Objects.PhoneDetails)
                If (personalResult.Phone IsNot Nothing) Then
                    For Each ph As Model.PhoneDetails In personalResult.Phone
                        Dim phoneObj As New Data.Objects.PhoneDetails
                        phoneObj.PhoneNumber = ph.PhoneNumber
                        phoneObj.ContactType = ph.ContactType
                        listPhoneObj.Add(phoneObj)
                    Next
                End If
                Dim listEmailObj As New List(Of Data.Objects.EmailAddressDetails)
                If (personalResult.Email IsNot Nothing) Then
                    For Each ph As Model.EmailAddressDetails In personalResult.Email
                        Dim emailObj As New Data.Objects.EmailAddressDetails
                        emailObj.EmailAddress = ph.EmailAddress
                        emailObj.ContactType = ph.ContactType
                        listEmailObj.Add(emailObj)
                    Next
                End If
                With obj

                    .AddressLine1 = personalResult.AddressLine1
                    .AddressLine2 = personalResult.AddressLine2
                    .County = personalResult.County
                    .DOBDateOfIssuance = personalResult.DOBDateOfIssuance
                    .FirstName = personalResult.FirstName
                    .LastName = personalResult.LastName
                    .MiddleName = personalResult.MiddleName
                    .Gender = personalResult.Gender
                    .RNLicenseOrSSN = personalResult.RNLicenseOrSSN
                    .State = personalResult.State
                    .City = personalResult.City
                    .Zip = personalResult.Zip
                    .ZipPlus = personalResult.ZipPlus
                    .Phone = listPhoneObj
                    .Email = listEmailObj
                    .DDPersonnelCode = personalResult.DDPersonnelCode
                End With
                Return obj
                End If
        End Function
        Public Shared Function MapDBToModelPersonalInfo(ByVal cd As Objects.PersonalInformationDetails) As Model.DDPersonnelDetails
            If (cd IsNot Nothing) Then
                Dim DDPersonal As New Model.PersonalInformationDetails
                DDPersonal.AddressLine1 = cd.AddressLine1
                DDPersonal.AddressLine2 = cd.AddressLine2
                DDPersonal.City = cd.City
                DDPersonal.County = cd.County
                DDPersonal.State = cd.State
                If (Not String.IsNullOrEmpty(cd.Zip)) Then
                    If (cd.Zip.Length > 5) Then
                        DDPersonal.Zip = cd.Zip.Substring(0, 5)
                        DDPersonal.ZipPlus = cd.Zip.Substring(5, cd.Zip.Length - 5)
                    Else
                        DDPersonal.Zip = cd.Zip
                        DDPersonal.ZipPlus = cd.ZipPlus
                    End If
                End If
                If cd.Phone IsNot Nothing Then
                    Dim ListPhone As New List(Of Model.PhoneDetails)
                    For Each ph As Objects.PhoneDetails In cd.Phone
                        Dim Phone As New Model.PhoneDetails
                        Phone.ContactType = ph.ContactType
                        Phone.PhoneNumber = ph.PhoneNumber
                        Phone.PhoneSID = ph.PhoneSID
                        ListPhone.Add(Phone)
                    Next
                    DDPersonal.Phone = ListPhone
                End If
                If cd.Email IsNot Nothing Then
                    Dim ListEmail As New List(Of Model.EmailAddressDetails)
                    For Each email As Objects.EmailAddressDetails In cd.Email
                        Dim Email1 As New Model.EmailAddressDetails
                        Email1.ContactType = email.ContactType
                        Email1.EmailAddress = email.EmailAddress
                        Email1.EmailAddressSID = email.EmailAddressSID
                        ListEmail.Add(Email1)
                    Next
                    DDPersonal.Email = ListEmail
                End If
                Return (New Model.DDPersonnelDetails() With
                        {
                            .DODDDateOfBirth = Convert.ToDateTime(cd.DOBDateOfIssuance),
                            .DODDFirstName = cd.FirstName,
                            .DODDHomeAddressLine1 = cd.AddressLine1,
                            .DODDHomeAddressLine2 = cd.AddressLine2,
                            .DODDHomeCity = cd.City,
                            .DODDHomeCounty = cd.County,
                            .DODDHomeState = cd.State,
                            .DODDHomeZip = cd.Zip,
                            .DODDLast4SSN = cd.RNLicenseOrSSN.PadLeft(4, "0"c),
                            .DODDLastName = cd.LastName,
                            .DODDMiddleName = cd.MiddleName,
                            .DODDGender = cd.Gender,
                            .DODDHomeCountyID = cd.CountyID,
                            .DODDHomeStateID = cd.StateID,
                .Address = DDPersonal
                        })
            Else
                Dim dd As Model.DDPersonnelDetails = Nothing
                Return dd
            End If
        End Function

        Public Shared Function MapDBToModelRNPersonalInfo(ByVal cd As Objects.PersonalInformationDetails) As Model.RNInformationDetails
            If (cd IsNot Nothing) Then
                Dim RNInfo As New Model.PersonalInformationDetails
                RNInfo.AddressLine1 = cd.AddressLine1
                RNInfo.AddressLine2 = cd.AddressLine2
                RNInfo.City = cd.City
                RNInfo.County = cd.County
                RNInfo.State = cd.State
                If (Not String.IsNullOrEmpty(cd.Zip)) Then
                    If (cd.Zip.Length > 5) Then
                        RNInfo.Zip = cd.Zip.Substring(0, 5)
                        RNInfo.ZipPlus = cd.Zip.Substring(5, cd.Zip.Length - 5)
                    Else
                        RNInfo.Zip = cd.Zip
                        RNInfo.ZipPlus = cd.ZipPlus
                    End If
                End If
                Dim ListPhone As New List(Of Model.PhoneDetails)
                If (cd.Phone IsNot Nothing) Then
                    For Each ph As Objects.PhoneDetails In cd.Phone
                        Dim Phone As New Model.PhoneDetails
                        Phone.ContactType = ph.ContactType
                        Phone.PhoneNumber = ph.PhoneNumber
                        Phone.PhoneSID = ph.PhoneSID
                        ListPhone.Add(Phone)
                    Next
                End If
                Dim ListEmail As New List(Of Model.EmailAddressDetails)
                If (cd.Email IsNot Nothing) Then
                    For Each email As Objects.EmailAddressDetails In cd.Email
                        Dim Email1 As New Model.EmailAddressDetails
                        Email1.ContactType = email.ContactType
                        Email1.EmailAddress = email.EmailAddress
                        Email1.EmailAddressSID = email.EmailAddressSID
                        ListEmail.Add(Email1)
                    Next
                End If
                RNInfo.Email = ListEmail
                RNInfo.Phone = ListPhone
                Return (New Model.RNInformationDetails() With
                        {
                            .DateOforiginalRNLicIssuance = Convert.ToDateTime(cd.DOBDateOfIssuance),
                            .FirstName = cd.FirstName,
                            .HomeAddressLine1 = cd.AddressLine1,
                            .HomeAddressLine2 = cd.AddressLine2,
                            .HomeCity = cd.City,
                            .HomeCounty = cd.County,
                            .HomeState = cd.State,
                            .HomeZip = cd.Zip,
                            .HomeZipPlus = cd.ZipPlus,
                            .RNLicense = cd.RNLicenseOrSSN,
                            .LastName = cd.LastName,
                            .MiddleName = cd.MiddleName,
                            .Gender = cd.Gender,
                            .HomeCountyID = cd.CountyID,
                .HomeStateID = cd.StateID,
                .Address = RNInfo
                        })
            Else
                Dim rns As Model.RNInformationDetails = Nothing
                Return rns
            End If
        End Function
    End Class
End Namespace
