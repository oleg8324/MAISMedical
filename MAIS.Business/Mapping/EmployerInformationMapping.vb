Imports MAIS.Data
Namespace Mapping
    Public Class EmployerInformationMapping
        Public Shared Function MapDBToPermEmployerInformation(ByVal lstEmpInfo As List(Of Objects.EmployerInformationDetails)) As List(Of Model.EmployerInformationDetails)
            Dim lstEmp As New List(Of Model.EmployerInformationDetails)
            lstEmp = (From emp In lstEmpInfo
                      Select New Model.EmployerInformationDetails With {
            .EmployerName = emp.EmployerName,
            .CEOFirstName = emp.CEOFirstName,
            .CEOLastName = emp.CEOLastName,
            .CEOMiddleName = emp.CEOMiddleName,
            .EmployerEndDate = emp.EmployerEndDate,
            .EmployerStartDate = emp.EmployerStartDate,
            .SupervisorFirstName = emp.SupervisorFirstName,
            .SupervisorLastName = emp.SupervisorLastName,
            .EmployerType = emp.EmployerType,
            .EmployerID = emp.EmployerID,
            .IdentitficationValue = emp.IdentitficationValue,
            .DODDProviderContractNumber = emp.DODDProviderContractNumber
                          }).ToList
            Return lstEmp
        End Function

        Public Shared Function MapDBToPermEmployerInformationWithAddress(ByVal lstEmpInfo As List(Of Objects.EmployerInformationDetails)) As List(Of Model.EmployerInformationDetails)
            Dim lstEmp As New List(Of Model.EmployerInformationDetails)
            lstEmp = (From emp In lstEmpInfo
                      Select New Model.EmployerInformationDetails With {
            .EmployerName = emp.EmployerName,
            .CEOFirstName = emp.CEOFirstName,
            .CEOLastName = emp.CEOLastName,
            .CEOMiddleName = emp.CEOMiddleName,
            .EmployerEndDate = emp.EmployerEndDate,
            .EmployerStartDate = emp.EmployerStartDate,
            .SupervisorFirstName = emp.SupervisorFirstName,
            .SupervisorLastName = emp.SupervisorLastName,
            .EmployerType = emp.EmployerType,
            .EmployerID = emp.EmployerID,
            .IdentitficationValue = emp.IdentitficationValue,
            .DODDProviderContractNumber = emp.DODDProviderContractNumber,
            .AgencyLocationAddress  = MapDBObjectsAddressToModelAddressContect( emp.AgencyLocationAddress) ,
            .WorkAgencyLocationAddress = MapDBObjectsAddressToModelAddressContect( emp.WorkAgencyLocationAddress),
            .SupervisorPhoneEmail = MapDBObjectsAddressToModelAddressContect(emp.SupervisorPhoneEmail),
            .AgencyPersonalAddressSame = emp.AgencyPersonalAddressSame,
            .AgencyWorkAddressSame = emp.AgencyWorkAddressSame
                          }).ToList
            Return lstEmp
        End Function

        Public Shared Function MapDBObjectsAddressToModelAddressContect(ByVal dbAddr As Objects.AddressControlDetails) As Model.AddressControlDetails
            Dim returnAddress As New Model.AddressControlDetails

            With returnAddress
                .AddressLine1 = dbAddr.AddressLine1
                .AddressLine2 = dbAddr.AddressLine2
                .City = dbAddr.City
                .State = dbAddr.State
                .Zip = dbAddr.Zip
                .ZipPlus = dbAddr.ZipPlus
                .AddressType = dbAddr.AddressType
                .Phone = dbAddr.Phone
                .Email = dbAddr.Email
                .County = dbAddr.County
                .ContactType = dbAddr.ContactType
                .StartDate = dbAddr.StartDate
                .EndDate = dbAddr.EndDate
            End With

            Return returnAddress
        End Function

        Public Shared Function MapDBToModelEmployerInfo(ByVal cd As List(Of Objects.EmployerInformationDetails)) As List(Of Model.EmployerInformationDetails)
            Dim employerInfo As New List(Of Model.EmployerInformationDetails)
            Dim testdate As New Date
          
            employerInfo = (From emp In cd
                            Select New Model.EmployerInformationDetails With {
                                .EmployerID = emp.EmployerID,
            .IdentitficationValue = emp.IdentitficationValue,
            .CEOFirstName = emp.CEOFirstName,
            .CEOLastName = emp.CEOLastName,
            .CEOMiddleName = emp.CEOMiddleName,
            .DODDProviderContractNumber = emp.DODDProviderContractNumber,
            .SupervisorFirstName = emp.SupervisorFirstName,
            .SupervisorLastName = emp.SupervisorLastName,
            .EmployerName = emp.EmployerName,
            .CurrentSupervisor = If(testdate = emp.CurrentSupervisor, "12/31/9999", emp.CurrentSupervisor),
            .CurrentWorkLocation = If(testdate = emp.CurrentWorkLocation, "12/31/9999", emp.CurrentWorkLocation),
            .Pending_Information_Flg = emp.Pending_Information_Flg
                                }).ToList
            Return employerInfo
        End Function

        Public Shared Function MapEmployerResultToDB(ByVal cd As Model.EmployerInformationDetails) As Objects.EmployerInformationDetails
            Dim employer As New Objects.EmployerInformationDetails
            employer.AgencyLocationAddress.AddressLine1 = cd.AgencyLocationAddress.AddressLine1
            employer.AgencyLocationAddress.AddressLine2 = cd.AgencyLocationAddress.AddressLine2
            employer.AgencyLocationAddress.City = cd.AgencyLocationAddress.City
            employer.AgencyLocationAddress.State = cd.AgencyLocationAddress.State
            employer.AgencyLocationAddress.Zip = cd.AgencyLocationAddress.Zip
            employer.AgencyLocationAddress.ZipPlus = cd.AgencyLocationAddress.ZipPlus
            employer.AgencyLocationAddress.AddressType = cd.AgencyLocationAddress.AddressType
            employer.AgencyLocationAddress.Phone = cd.AgencyLocationAddress.Phone
            employer.AgencyLocationAddress.Email = cd.AgencyLocationAddress.Email
            employer.AgencyLocationAddress.County = cd.AgencyLocationAddress.County
            employer.AgencyLocationAddress.ContactType = cd.AgencyLocationAddress.ContactType
            employer.AgencyLocationAddress.StartDate = cd.AgencyLocationAddress.StartDate
            employer.AgencyLocationAddress.EndDate = cd.AgencyLocationAddress.EndDate
            employer.WorkAgencyLocationAddress.AddressLine1 = cd.WorkAgencyLocationAddress.AddressLine1
            employer.WorkAgencyLocationAddress.AddressLine2 = cd.WorkAgencyLocationAddress.AddressLine2
            employer.WorkAgencyLocationAddress.City = cd.WorkAgencyLocationAddress.City
            employer.WorkAgencyLocationAddress.State = cd.WorkAgencyLocationAddress.State
            employer.WorkAgencyLocationAddress.Zip = cd.WorkAgencyLocationAddress.Zip
            employer.WorkAgencyLocationAddress.ZipPlus = cd.WorkAgencyLocationAddress.ZipPlus
            employer.WorkAgencyLocationAddress.Phone = cd.WorkAgencyLocationAddress.Phone
            employer.WorkAgencyLocationAddress.Email = cd.WorkAgencyLocationAddress.Email
            employer.WorkAgencyLocationAddress.County = cd.WorkAgencyLocationAddress.County
            employer.WorkAgencyLocationAddress.AddressType = cd.WorkAgencyLocationAddress.AddressType
            employer.WorkAgencyLocationAddress.ContactType = cd.WorkAgencyLocationAddress.ContactType
            employer.WorkAgencyLocationAddress.StartDate = cd.WorkAgencyLocationAddress.StartDate
            employer.WorkAgencyLocationAddress.EndDate = cd.WorkAgencyLocationAddress.EndDate
            employer.SupervisorPhoneEmail.Phone = cd.SupervisorPhoneEmail.Phone
            employer.SupervisorPhoneEmail.Email = cd.SupervisorPhoneEmail.Email
            employer.SupervisorPhoneEmail.ContactType = cd.SupervisorPhoneEmail.ContactType
            employer.AgencyPersonalAddressSame = cd.AgencyPersonalAddressSame
            employer.AgencyWorkAddressSame = cd.AgencyWorkAddressSame
            employer.CEOFirstName = cd.CEOFirstName
            employer.CEOLastName = cd.CEOLastName
            employer.CEOMiddleName = cd.CEOMiddleName
            employer.DODDProviderContractNumber = cd.DODDProviderContractNumber
            employer.EmployerEndDate = cd.EmployerEndDate
            employer.EmployerName = cd.EmployerName
            employer.EmployerStartDate = cd.EmployerStartDate
            employer.EmployerTaxID = cd.EmployerTaxID
            employer.EmployerIdentificationTypeID = cd.EmployerIdentificationTypeID
            employer.EmployerTypeID = cd.EmployerTypeID
            employer.SupervisorFirstName = cd.SupervisorFirstName
            employer.SupervisorLastName = cd.SupervisorLastName
            employer.StartDate = cd.StartDate
            employer.EndDate = cd.EndDate
            Return employer
        End Function

        Public Shared Function MapObjectToModel(ByVal cd As Objects.EmployerInformationDetails) As Model.EmployerInformationDetails
            Dim employer As New Model.EmployerInformationDetails
            If (cd.AgencyLocationAddress IsNot Nothing) Then
                employer.AgencyLocationAddress.AddressLine1 = cd.AgencyLocationAddress.AddressLine1
                employer.AgencyLocationAddress.AddressLine2 = cd.AgencyLocationAddress.AddressLine2
                employer.AgencyLocationAddress.City = cd.AgencyLocationAddress.City
                employer.AgencyLocationAddress.State = cd.AgencyLocationAddress.State
                If (cd.AgencyLocationAddress.Zip.Length > 5) Then
                    employer.AgencyLocationAddress.Zip = cd.AgencyLocationAddress.Zip.Substring(0, 5)
                    employer.AgencyLocationAddress.ZipPlus = cd.AgencyLocationAddress.Zip.Substring(5, cd.AgencyLocationAddress.Zip.Length - 5)
                Else
                    employer.AgencyLocationAddress.Zip = cd.AgencyLocationAddress.Zip
                    employer.AgencyLocationAddress.ZipPlus = cd.AgencyLocationAddress.ZipPlus
                End If
                'employer.AgencyLocationAddress.Zip = cd.AgencyLocationAddress.Zip
                'employer.AgencyLocationAddress.ZipPlus = cd.AgencyLocationAddress.ZipPlus
                employer.AgencyLocationAddress.Phone = cd.AgencyLocationAddress.Phone
                employer.AgencyLocationAddress.Email = cd.AgencyLocationAddress.Email
                employer.AgencyLocationAddress.County = cd.AgencyLocationAddress.County
                employer.AgencyLocationAddress.ContactType = cd.AgencyLocationAddress.ContactType
                employer.AgencyLocationAddress.AddressType = cd.AgencyLocationAddress.AddressType
                employer.AgencyLocationAddress.StateID = cd.AgencyLocationAddress.StateID
                employer.AgencyLocationAddress.CountyID = cd.AgencyLocationAddress.CountyID
            End If
            If (cd.WorkAgencyLocationAddress IsNot Nothing) Then
                employer.WorkAgencyLocationAddress.AddressType = cd.WorkAgencyLocationAddress.AddressType
                employer.WorkAgencyLocationAddress.AddressLine1 = cd.WorkAgencyLocationAddress.AddressLine1
                employer.WorkAgencyLocationAddress.AddressLine2 = cd.WorkAgencyLocationAddress.AddressLine2
                employer.WorkAgencyLocationAddress.City = cd.WorkAgencyLocationAddress.City
                employer.WorkAgencyLocationAddress.State = cd.WorkAgencyLocationAddress.State
                If (cd.WorkAgencyLocationAddress.Zip.Length > 5) Then
                    employer.WorkAgencyLocationAddress.Zip = cd.WorkAgencyLocationAddress.Zip.Substring(0, 5)
                    employer.WorkAgencyLocationAddress.ZipPlus = cd.WorkAgencyLocationAddress.Zip.Substring(5, cd.WorkAgencyLocationAddress.Zip.Length - 5)
                Else
                    employer.WorkAgencyLocationAddress.Zip = cd.WorkAgencyLocationAddress.Zip
                    employer.WorkAgencyLocationAddress.ZipPlus = cd.WorkAgencyLocationAddress.ZipPlus
                End If
                'employer.WorkAgencyLocationAddress.Zip = cd.WorkAgencyLocationAddress.Zip
                'employer.WorkAgencyLocationAddress.ZipPlus = cd.WorkAgencyLocationAddress.ZipPlus
                employer.WorkAgencyLocationAddress.Phone = cd.WorkAgencyLocationAddress.Phone
                employer.WorkAgencyLocationAddress.Email = cd.WorkAgencyLocationAddress.Email
                employer.WorkAgencyLocationAddress.County = cd.WorkAgencyLocationAddress.County
                employer.WorkAgencyLocationAddress.ContactType = cd.WorkAgencyLocationAddress.ContactType
                employer.WorkAgencyLocationAddress.StartDate = cd.WorkAgencyLocationAddress.StartDate
                employer.WorkAgencyLocationAddress.EndDate = cd.WorkAgencyLocationAddress.EndDate
                employer.WorkAgencyLocationAddress.StateID = cd.WorkAgencyLocationAddress.StateID
                employer.WorkAgencyLocationAddress.CountyID = cd.WorkAgencyLocationAddress.CountyID
            End If
            employer.SupervisorPhoneEmail.Phone = cd.SupervisorPhoneEmail.Phone
            employer.SupervisorPhoneEmail.Email = cd.SupervisorPhoneEmail.Email
            employer.SupervisorPhoneEmail.ContactType = cd.SupervisorPhoneEmail.ContactType
            employer.AgencyPersonalAddressSame = cd.AgencyPersonalAddressSame
            employer.AgencyWorkAddressSame = cd.AgencyWorkAddressSame
            employer.CEOFirstName = cd.CEOFirstName
            employer.CEOLastName = cd.CEOLastName
            employer.CEOMiddleName = cd.CEOMiddleName
            employer.DODDProviderContractNumber = cd.DODDProviderContractNumber
            employer.EmployerEndDate = cd.EmployerEndDate
            employer.EmployerName = cd.EmployerName
            employer.EmployerStartDate = cd.EmployerStartDate
            employer.EmployerTaxID = cd.EmployerTaxID
            employer.EmployerIdentificationTypeID = cd.EmployerIdentificationTypeID
            employer.EmployerTypeID = cd.EmployerTypeID
            employer.SupervisorFirstName = cd.SupervisorFirstName
            employer.SupervisorLastName = cd.SupervisorLastName
            employer.EmployerID = cd.EmployerID
            employer.StartDate = cd.StartDate
            employer.EndDate = cd.EndDate
            Return employer
        End Function
    End Class
End Namespace
