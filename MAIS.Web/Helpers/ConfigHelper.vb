Public Class ConfigHelper
    Public Shared Function GetWebFileLocation() As String
        Dim strWebFileLocation As String = String.Empty

        If ConfigurationManager.AppSettings("WEBFILELOCATION") IsNot Nothing Then
            strWebFileLocation = ConfigurationManager.AppSettings("WEBFILELOCATION")
        End If

        Return strWebFileLocation
    End Function

    'Public Shared Function GetSecurityConnection() As String
    '    Dim strSecurity As String = String.Empty

    '    If ConfigurationManager.ConnectionStrings("SECURITYCONNECTION").ConnectionString IsNot Nothing Then
    '        strSecurity = ConfigurationManager.ConnectionStrings("SECURITYCONNECTION").ConnectionString
    '    End If

    '    Return strSecurity
    'End Function

    Public Shared Function GetProviderConnectionString() As String
        Dim strPCW As String = String.Empty

        If ConfigurationManager.ConnectionStrings("Provider").ConnectionString IsNot Nothing Then
            strPCW = ConfigurationManager.ConnectionStrings("Provider").ConnectionString
        End If

        Return strPCW
    End Function
    Public Shared Function GetMAISConnectionString() As String
        Dim strPCW As String = String.Empty

        If ConfigurationManager.ConnectionStrings("MAISContext").ConnectionString IsNot Nothing Then
            strPCW = ConfigurationManager.ConnectionStrings("MAISContext").ConnectionString
        End If

        Return strPCW
    End Function

    Public Shared Function GetOIDDBConnectionString() As String
        Dim strOIDDB As String = String.Empty

        If ConfigurationManager.ConnectionStrings("OIDDB").ConnectionString IsNot Nothing Then
            strOIDDB = ConfigurationManager.ConnectionStrings("OIDDB").ConnectionString
        End If

        Return strOIDDB
    End Function

    Public Shared Function GetSRCConnectionString() As String
        Dim strSRC As String = String.Empty

        If ConfigurationManager.ConnectionStrings("SRC").ConnectionString IsNot Nothing Then
            strSRC = ConfigurationManager.ConnectionStrings("SRC").ConnectionString
        End If

        Return strSRC
    End Function

    Public Shared Function GetUser(ByVal userID As Integer) As ODMRDD_NET2.IUser
        Dim user As ODMRDD_NET2.IUser

        Try
            Dim userService As ODMRDD_NET2.IUserService = New ODMRDD_NET2.UserService()

            If userID > 0 Then
                user = userService.GetUserByUserId(userID)
            Else
                user = Nothing
            End If
        Catch iex As InvalidOperationException
            user = Nothing
        Catch ex As Exception
            Throw ex
        End Try

        Return user
    End Function
    Public Shared ReadOnly Property BUCode As String
        Get
            Return ConfigurationManager.AppSettings("BUCode")
        End Get
    End Property
    Public Shared ReadOnly Property CreateUserAccountAccess As Boolean
        Get
            Return ConfigurationManager.AppSettings("CreateUserAccountAccess")
        End Get
    End Property
    Public Shared ReadOnly Property UDSServiceEndpoint As String
        Get
            Return ConfigurationManager.AppSettings("UDSServiceEndpoint")
        End Get
    End Property

    Public Shared ReadOnly Property PublicAccessEndpoint As String
        Get
            Return ConfigurationManager.AppSettings("PublicAcessEndPoint")
        End Get
    End Property

    Public Shared ReadOnly Property DODDSVCServiceEndPoint As String
        Get
            Return ConfigurationManager.AppSettings("DODDSVCServiceEndPoint")
        End Get
    End Property

    Public Shared ReadOnly Property EmailOverride As String
        Get
            Return ConfigurationManager.AppSettings("EmailOverride")
        End Get
    End Property

    Public Shared ReadOnly Property FromEmailAddress As String
        Get
            Return ConfigurationManager.AppSettings("FromEmailAddress")
        End Get
    End Property

    Public Shared ReadOnly Property EmailSubjectStatus As String
        Get
            Return ConfigurationManager.AppSettings("SubjectStatus")
        End Get
    End Property
    Public Shared ReadOnly Property EmailMappingSubjectStatus As String
        Get
            Return ConfigurationManager.AppSettings("EmailMappingSubjectStatus")
        End Get
    End Property

    Public Shared ReadOnly Property ApplicationPath As String
        Get
            Return ConfigurationManager.AppSettings("ApplicationURL")
        End Get
    End Property

    Public Shared ReadOnly Property ToEmailAddress As String
        Get
            Return ConfigurationManager.AppSettings("ToEmailAddress")
        End Get
    End Property
    Public Shared ReadOnly Property CCSummaryEmailAddress As String
        Get
            Return ConfigurationManager.AppSettings("CCSummaryEmailAddress")
        End Get
    End Property

    Public Shared ReadOnly Property CCEmailAddress As String
        Get
            Return ConfigurationManager.AppSettings("CCEmailAddress")
        End Get
    End Property
    Public Shared ReadOnly Property OldMA As String
        Get
            Return ConfigurationManager.AppSettings("OLDMA")
        End Get
    End Property
End Class
