Imports System.ComponentModel.DataAnnotations

Namespace Model
    Public Class RNInformationDetails
        Public Property RNLicense As String
        Public Property LastName As String
        Public Property FirstName As String
        Public Property MiddleName As String
        Public Property Gender As String
        <Required()>
        Public Property HomeAddressLine1 As String
        Public Property HomeAddressLine2 As String
        <Required()>
        <StringLength(2, ErrorMessage:="Please enter a valid state")>
        Public Property HomeState As String
        Public Property HomeStateID As Integer
        <Required()>
        Public Property HomeCity As String
        Public Property HomeCounty As String
        Public Property HomeCountyID As Integer
        <Required()>
        Public Property HomeZip As String
        Public Property HomeZipPlus As String
        Public Property DateOforiginalRNLicIssuance As DateTime
        Public Property Address As New PersonalInformationDetails
    End Class
End Namespace
