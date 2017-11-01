Namespace Objects
    Public Class RN_UserDetailsObject
        Public Property RN_Sid As Integer
        Public Property RNLicense_Number As String
        Public Property FirstName As String
        Public Property LastName As String
        Public Property MiddleName As String
        Public Property CreateDate As DateTime
        Public Property CreateBy As Integer
        Public Property LastUpdatedDate As DateTime
        Public Property LastUpdateBy As Integer
        Public Property DateOfOriginalRNLicIssuance As DateTime
        Public Property Gender As String
        Public Property Active_Flg As Boolean
        Public Property EmailList As List(Of EmailAddressDetails)
    End Class
End Namespace

