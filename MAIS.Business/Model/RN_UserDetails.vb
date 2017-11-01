Namespace Model
    Public Class RN_UserDetails
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

        Public ReadOnly Property LastFirstname As String
            Get
                Return String.Format("{0}, {1}", LastName, FirstName)
            End Get
        End Property
        Public ReadOnly Property HomeEmail As String
            Get
                Dim EmailStr As String = Nothing
                EmailStr = (From e In EmailList _
                            Where e.ContactType = 1
                            Select e.EmailAddress).FirstOrDefault
                Return EmailStr
            End Get
        End Property

        Public ReadOnly Property WorkEmail As String
            Get
                Dim EmailStr As String = Nothing
                EmailStr = (From e In EmailList _
                            Where e.ContactType = 2
                            Select e.EmailAddress).FirstOrDefault
                Return EmailStr
            End Get
        End Property
        Public ReadOnly Property Other As String
            Get
                Dim EmailStr As String = Nothing
                EmailStr = (From e In EmailList _
                            Where e.ContactType = 3
                            Select e.EmailAddress).FirstOrDefault
                Return EmailStr
            End Get
        End Property
    End Class
    Public Class DD_Personnel
        Public Property DDPersonnel_Sid As Integer
        Public Property DDPersonnelCode As String
        Public Property DDPersonnelNameSSN As String
    End Class
    Public Class RN_UserMappingDetails
        Public Property UserMapping_Sid As Integer
        Public Property UserID As Integer
        Public Property RN_Sid As Integer
        Public Property CreateDate As DateTime
        Public Property CreateBy As Integer
        Public Property LastUpdateDate As DateTime
        Public Property LastUpdateBy As Integer
    End Class
End Namespace

