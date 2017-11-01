Namespace DTO
    <DataContract()>
    Public Class RNDetailInformation
        <DataMember()>
        Property FirstName As String

        <DataMember()>
        Property RNLicenseNumber As String

        <DataMember()>
        Property LastName As String

        <DataMember()>
        Property CertificateDetails As List(Of CertificateDetails)
    End Class
    <DataContract()>
    Public Class DDDetailInformation

        <DataMember()>
        Property LastName As String

        <DataMember()>
        Property DDPersonnelCode As String

        <DataMember()>
        Property FirstName As String

        <DataMember()>
        Property DOB As String

        <DataMember()>
        Property CertificateDetails As List(Of CertificateDetails)
    End Class
    <DataContract()>
    Public Class CertificateDetails
        <DataMember()>
        Property RoleDescription As String

        <DataMember()>
        Property EffectiveDate As String

        <DataMember()>
        Property ExpirationDate As String

        <DataMember()>
        Property ConsectiveRenewals As Integer

        <DataMember()>
        Property CurrentStatus As String
    End Class
    <DataContract()>
    Public Class TrainingSessionResults
        <DataMember()>
        Property StartDate As String

        <DataMember()>
        Property EndDate As String

        <DataMember()>
        Property RNTrainerName As String

        <DataMember()>
        Property CourseCategory As String

        <DataMember()>
        Property County As String

        <DataMember()>
        Property RNTrainerEmail As String

        <DataMember()>
        Property OBNNumber As String
    End Class
End Namespace
