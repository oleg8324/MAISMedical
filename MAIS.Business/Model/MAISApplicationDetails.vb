Imports System.ComponentModel.DataAnnotations

Namespace Model
    Public Class MAISApplicationDetails
        'RN Information Details
        Public Property DDPersonnelInfo As New DDPersonnelDetails
        Public Property RNInfo As New RNInformationDetails
        'Application Details
        Public Property ApplicationID As Integer
        Public Property ApplicationTypeID As Integer
        Public Property ApplicationStartDate As DateTime?
        Public Property ApplicationEndDate As DateTime?
        <Required()>
        Public Property RNFlag As Boolean
        Public Property ApplicationStatusTypeID As Integer
        Public Property Signature As String
        Public Property RoleCategoryLevelID As Integer
        Public Property AttestantID As Integer
        Public Property CertificationType As String
        Public Property InitialCertificationReq As String
        Public Property RenewalCertificationReq As String
        Public Property DDPersonnelRNID As String

        'Employer Information
        Public Property EmployerInformation As New List(Of EmployerInformationDetails)
        'Public Property PersonalInformation As New PersonalInformationDetails
    End Class
End Namespace
