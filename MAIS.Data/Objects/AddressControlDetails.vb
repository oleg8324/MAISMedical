Namespace Objects
    Public Class AddressControlDetails
        Public Property AddressLine1 As String
        Public Property AddressLine2 As String
        Public Property State As String
        Public Property City As String
        Public Property County As String
        Public Property Zip As String
        Public Property ZipPlus As String
        Public Property Phone As String
        Public Property Email As String
        Public Property AddressType As Integer
        Public Property CountyID As Integer
        Public Property StateID As Integer
        Public Property AddressSID As Integer
        Public Property ContactType As Integer
        Public Property StartDate As DateTime
        Public Property EndDate As DateTime
        Public Property AgencyStartDate As DateTime?
        Public Property AgencyEndDate As DateTime?
    End Class
    Public Class StateDetails
        Public Property StateAbr As String
        Public Property StateID As Integer
    End Class
    Public Class CountyDetails
        Public Property CountyAlias As String
        Public Property CountyID As Integer
    End Class
End Namespace
