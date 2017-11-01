Namespace Objects
    Public Class NType
        Public Property NTypeSid As Integer
        Public Property NTypeDesc As String
    End Class
    Public Class NReason
        Public Property NReasonSid As Integer
        Public Property NReasonDesc As String
    End Class
    Public Class NotationObject
        Public Property NotationType As NType
        Public Property OccurenceDate As Date
        Public Property NotationDate As Date
        Public Property UnflaggedDate As DateTime?
        Public Property PersonEnteringNotation As String
        Public Property PersonTitle As String
        Public Property AppNotId As Integer
        Public Property AllReasons As String
        Public Property NotationReasons As List(Of NReason)
        Public Property AppId As Integer?
    End Class
    Public Class CertStatus
        Public Property CertStatusSid As Integer
        Public Property CertStatusDesc As String
    End Class
End Namespace
