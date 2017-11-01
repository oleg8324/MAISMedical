Namespace Model
    Public Class RNSearchResult
        Public Property RNLicenseNumber As String
        Public Property FirstName As String
        Public Property LastName As String
        Public Property MiddleName As String
        Public Property ApplicationStatus As String
        Public Property HomeAddress As String
        Public Property County As String
        Public Property RNTrainer As String
        Public Property RNInstructor As String
        Public Property RNMaster As String
        Public Property ICFRN As String
        Public Property QARN As String
        Public Property RoleID As Integer
        Public Property Roles As List(Of MAISRNDDRoleDetails)
        Public Property ApplicationID As Integer?
        Public Property DateRNIssuance As DateTime
        Public Property ApplicationType As String
    End Class
End Namespace
