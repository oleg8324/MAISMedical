Namespace Model
    Public Class EmployerInformationDetails
        Public Property EmployerID As Integer
        Public Property EmployerName As String
        Public Property DODDProviderContractNumber As String
        Public Property CurrentSupervisor As DateTime
        Public Property CurrentWorkLocation As DateTime
        Public Property IdentitficationValue As String
        'Public Property EmployerCertificationStartDate As DateTime
        'Public Property EmployerCertificationEndDate As DateTime
        'Public Property EmployerCertificationStatus As String
        Public Property AgencyPersonalAddressSame As Boolean
        Public Property AgencyWorkAddressSame As Boolean
        Public Property EmployerTaxID As String
        Public Property EmployerIdentificationTypeID As Integer
        Public Property EmployerTypeID As Integer
        Public Property EmployerStartDate As DateTime
        Public Property EmployerEndDate As DateTime
        Public Property CEOFirstName As String
        Public Property CEOLastName As String
        Public Property CEOMiddleName As String
        Public Property SupervisorFirstName As String
        Public Property SupervisorLastName As String
        Public Property EmployerType As String
        Public Property StartDate As DateTime
        Public Property EndDate As DateTime
        Public Pending_Information_Flg As Boolean
        Public Property AgencyLocationAddress As New AddressControlDetails
        Public Property WorkAgencyLocationAddress As New AddressControlDetails
        Public Property SupervisorPhoneEmail As New AddressControlDetails
    End Class
End Namespace

