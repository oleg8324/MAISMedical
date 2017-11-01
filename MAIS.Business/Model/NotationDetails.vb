
Namespace Model
    Public Class NType
        Public Property NTypeSid As Integer
        Public Property NTypeDesc As String
    End Class
    Public Class NReason
        Public Property NReasonSid As Integer
        Public Property NReasonDesc As String
    End Class

    Public Class NotationDetails
        Public Property NotationType As NType
        Public Property AppId As Integer?
        'Public Property NotationReason As String?
        Public Property OccurenceDate As Date
        Public Property NotationDate As Date
        Public Property UnflaggedDate As Date?
        Public Property PersonEnteringNotation As String
        Public Property PersonTitle As String
        Public Property AllReasons As String
        Public Property AppNotId As Integer
        Public Property NotationReasons As List(Of NReason)
    End Class
End Namespace
