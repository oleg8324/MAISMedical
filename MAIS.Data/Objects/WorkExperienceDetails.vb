Namespace Objects
    Public Class WorkExperienceDetails
        Public Property WorkID As Integer
        Public Property AppID As Integer
        Public Property EmpName As String
        Public Property ChkRNFlg As Boolean
        Public Property ChkDDFlg As Boolean
        Public Property EmpStartDate As DateTime?
        Public Property EmpEndDate As DateTime?
        Public Property Title As String
        Public Property JobDuties As String
        Public Property Address As New AddressControlDetails
    End Class
End Namespace