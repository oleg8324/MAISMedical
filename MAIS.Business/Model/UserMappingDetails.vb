Namespace Model
    Public Class UserMappingDetails
        Public Property UserMappingSid As Integer
        Public Property UserID As Integer
        Public Property PortalUserRole As String
        Public Property FirstName As String
        Public Property LastName As String
        Public Property MiddleName As String
        Public Property User_Code As String
        Public Property Email As String
        Public Property Is_Secretary As Boolean
    End Class
    Public Class UserLoginSearch
        Public Property RNLicenseNumber As String
        Public Property FirstName As String
        Public Property LastName As String
        Public Property UserMappingID As Integer
        'Public Property MiddleName As String
    End Class
End Namespace
