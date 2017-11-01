Imports System.Configuration
Imports ODMRDDHelperClassLibrary.Utility

Public Interface IQueriesBase
    ReadOnly Property Messages() As List(Of ReturnMessage)
    ReadOnly Property ProgramID As Integer
    Property UserID As String
    Property MAISConnectionString As String
    Function HasErrors() As Boolean
    Function HasWarnings() As Boolean

End Interface

Public MustInherit Class QueriesBase
    Inherits Logging.AbstractLoggingBase
    Implements IQueriesBase

    Friend _connMAISDB As String = ConfigurationManager.ConnectionStrings("MAISContext").ConnectionString
    'Protected _connProviderDB As String = ConfigurationManager.ConnectionStrings("Provider").ConnectionString
    Protected _connOIDDB As String = ConfigurationManager.ConnectionStrings("OIDDB").ConnectionString
    Protected _programID As Integer = CInt(ConfigurationManager.AppSettings("PROGSID"))
    Protected _messages As New List(Of ReturnMessage)

    Public ReadOnly Property Messages() As List(Of ReturnMessage) Implements IQueriesBase.Messages
        Get
            Return _messages
        End Get
    End Property

    Protected ReadOnly Property ProgramID As Integer Implements IQueriesBase.ProgramID
        Get
            Return _programID
        End Get
    End Property

    Public Property UserID As String Implements IQueriesBase.UserID

    Public Property MAISConnectionString As String Implements IQueriesBase.MAISConnectionString
        Get
            Return _connMAISDB
        End Get
        Set(value As String)
            _connMAISDB = value
        End Set
    End Property


    Public Function HasErrors() As Boolean Implements IQueriesBase.HasErrors
        Return (From m In Me._messages Where m.IsError = True).Any
    End Function

    Public Function HasWarnings() As Boolean Implements IQueriesBase.HasWarnings
        Return (From m In Me._messages Where m.IsWarning = True).Any
    End Function
End Class
