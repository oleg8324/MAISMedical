Namespace Model
    Public Class DDPersonnelSearchResult
        Public Property Last4SSN As String
        Public Property FirstName As String
        Public Property LastName As String
        Public Property MiddleName As String
        Public Property ApplicationStatus As String
        Public Property HomeAddress As String
        Public Property DDPersonnelCode As String
        Public Property CAT1 As String
        Public Property CAT2 As String
        Public Property CAT3 As String
        Public Property County As String
        Public Property RoleID As Integer
        Public Property Roles As List(Of MAISRNDDRoleDetails)
        Public Property ApplicationID As Integer?
        Public Property DateOfBirth As DateTime
        Public Property ApplicationType As String
    End Class
End Namespace
