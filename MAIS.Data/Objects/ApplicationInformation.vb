Imports System.ComponentModel.DataAnnotations

Namespace Objects
    Public Class ApplicationInformation
        Public Property DDRNPersonnelInfo As New PersonalInformationDetails
        'Application Details
        Public Property ApplicationID As Integer
        <Required()>
        Public Property ApplicationTypeID As Integer
        Public Property ApplicationStartDate As DateTime?
        Public Property ApplicationEndDate As DateTime?
        <Required()>
        Public Property RNFlag As Boolean
        Public Property ApplicationStatusTypeID As Integer
        Public Property Signature As String
        Public Property RoleCategoryLevelID As Integer
        Public Property AttestantID As Integer
        Public Property UniqueCode As String
        Public Property CertificationType As String
        Public Property InitialCertificationReq As String
        Public Property RenewalCertificationReq As String

        'Employer Information
        'Public Property EmployerInformation As New List(Of EmployerInformationDetails)
    End Class
End Namespace