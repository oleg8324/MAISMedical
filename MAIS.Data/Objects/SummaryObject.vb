Namespace Objects
    Public Class SummaryObject
        Public Property appStatus As String
    End Class
    Public Class AppStatus
        Public Property ASTypeSid As Integer
        Public Property ASTypeDesc As String
    End Class
    Public Class Contact
        Public Property CTypeSid As Integer
        Public Property CTypeDesc As String
        Public Property CEmail As String
        Public Property CPhone As String
    End Class
    Public Class Certificate
        Public Property Certification_Sid As Integer
        Public Property Application_Sid As Integer
        Public Property StartDate As Date
        Public Property EndDate As Date
        Public Property Role_Category_Level_Sid As Integer
        Public Property Role_RN_DD_Personnel_Xref_Sid As Integer
        Public Property Role As String
        Public Property Category As String
        Public Property Level As String
        Public Property Status As String
        Public Property RolePriority As Integer?
        Public Property ApplicationType As String
        Public TestId As Integer
    End Class
    Public Class CertificateExpirationTotals
        Public Property Role_RN_DD_Personnel_Xref_Sid As Integer
        Public Property Role_Category_Level_Sid As Integer
        Public Property Role As String
        Public Property Category As String
        Public Property Level As String
        Public Property Status As String
        Public Property RolePriority As Integer?
        Public Property Exp30Days As Integer?
        Public Property Exp60Days As Integer?
        Public Property Exp90Days As Integer?
        Public Property Exp180Days As Integer?
    End Class
End Namespace
