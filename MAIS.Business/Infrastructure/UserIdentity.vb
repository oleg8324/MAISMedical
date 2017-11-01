Namespace Infrastructure
    Public Interface IUserIdentity
        Property UserID As Integer
    End Interface

    Public Class UserIdentity
        Implements IUserIdentity

        Public Property UserID As Integer Implements IUserIdentity.UserID
    End Class
    Public Interface IConnectionIdentity
        Property ConnectionString As String
    End Interface

    Public Class ConnectionIdentity
        Implements IConnectionIdentity

        Public Property ConnectionString As String Implements IConnectionIdentity.ConnectionString

    End Class
End Namespace