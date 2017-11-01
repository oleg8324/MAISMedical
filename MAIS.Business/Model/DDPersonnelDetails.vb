Imports System.ComponentModel.DataAnnotations
Namespace Model
    Public Class DDPersonnelDetails
        Public Property DODDLast4SSN As String
        Public Property DODDLastName As String
        Public Property DODDFirstName As String
        Public Property DODDMiddleName As String
        Public Property DODDGender As String
        <Required()>
        Public Property DODDHomeAddressLine1 As String
        Public Property DODDHomeAddressLine2 As String
        <Required()>
        <StringLength(2, ErrorMessage:="Please enter a valid state")>
        Public Property DODDHomeState As String
        Public Property DODDHomeStateID As Integer
        <Required()>
        Public Property DODDHomeCity As String
        Public Property DODDHomeCounty As String
        Public Property DODDHomeCountyID As Integer
        <Required()>
        Public Property DODDHomeZip As String
        Public Property DODDHomeZipPlus As String
        Public Property DODDDateOfBirth As DateTime
        Public Property Address As New PersonalInformationDetails
    End Class
End Namespace
