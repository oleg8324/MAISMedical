Namespace Objects
    Public Class PersonalInformationDetails
        Public Property RNLicenseOrSSN As String
        Public Property DOBDateOfIssuance As DateTime
        Public Property LastName As String
        Public Property FirstName As String
        Public Property MiddleName As String
        Public Property DDPersonnelCode As String
        Public Property Gender As String
        Public Property AddressLine1 As String
        Public Property AddressLine2 As String
        Public Property State As String
        Public Property StateID As Integer
        Public Property City As String
        Public Property County As String
        Public Property CountyID As Integer
        Public Property Zip As String
        Public Property ZipPlus As String
        Public Property AddressSID As Integer
        Public Property ApplicationSID As Integer
        Public Property Phone As List(Of PhoneDetails)
        Public Property Email As List(Of EmailAddressDetails)
    End Class
End Namespace

