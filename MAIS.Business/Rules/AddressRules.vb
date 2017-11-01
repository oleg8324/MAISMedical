Imports ODMRDDHelperClassLibrary.Utility
Imports System.Text.RegularExpressions
Imports MAIS.Business.Services

Namespace Rules
    Public Class AddressRules
        Inherits BusinessBase

        Public Shared Function CheckAddressControl(ByVal addressInfo As Model.AddressControlDetails) As ReturnObject(Of Boolean)
            Dim retObj As New ReturnObject(Of Boolean)()
            If String.IsNullOrWhiteSpace(addressInfo.AddressLine1) OrElse
              addressInfo.AddressLine1.Length > 100 Then
                retObj.AddErrorMessage("Address 1 must be entered, have less than 100 characters.")
            End If

            If Not String.IsNullOrWhiteSpace(addressInfo.AddressLine2) OrElse
                addressInfo.AddressLine2.Length > 100 Then
                retObj.AddErrorMessage("Address 2 should be less than 100 characters.")
            End If

            If String.IsNullOrWhiteSpace(addressInfo.City) OrElse
                addressInfo.City.Length > 100 OrElse
                 Not Regex.IsMatch(addressInfo.City, Helpers.RegExHelper.Alphabetical) Then
                retObj.AddErrorMessage("City must be entered, have less than 100 characters, and only contain letters.")
            End If

            If Not (addressInfo.StateID > 0) Then
                retObj.AddErrorMessage("State must be entered")
            End If

            If String.IsNullOrWhiteSpace(addressInfo.Zip) OrElse
              addressInfo.Zip.Length > 5 OrElse
              Not IsNumeric(addressInfo.Zip) Then
                retObj.AddErrorMessage("Zip Code must be entered, have 5 Numbers.")
            End If

            If Not String.IsNullOrWhiteSpace(addressInfo.ZipPlus) OrElse
            addressInfo.ZipPlus.Length > 4 OrElse
            Not IsNumeric(addressInfo.ZipPlus) Then
                retObj.AddErrorMessage("ZipPlus Code must be entered, have 4 Numbers.")
            End If


            Return retObj
        End Function

    End Class
End Namespace