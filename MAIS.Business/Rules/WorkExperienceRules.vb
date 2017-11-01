Imports ODMRDDHelperClassLibrary.Utility
Imports System.Text.RegularExpressions
Imports MAIS.Business.Services

Namespace Rules
    Public Class WorkExperienceRules
        Inherits BusinessBase

        Public Shared Function CheckWorkExperiencePage(ByVal newWorkExpInfo As Model.WorkExperienceDetails) As ReturnObject(Of Boolean)
            Dim retObj As New ReturnObject(Of Boolean)()
            'Employer/Agency name
            If String.IsNullOrWhiteSpace(newWorkExpInfo.EmpName) OrElse
                newWorkExpInfo.EmpName.Length > 100 Then
                retObj.AddErrorMessage("Employer/Agency Name must be entered, have less than 100 characters.")
            End If
            'Work Start date
            If newWorkExpInfo.EmpStartDate Is Nothing Then
                retObj.AddErrorMessage("Start date of work must be entered")
            Else
                If Not IsDate(newWorkExpInfo.EmpStartDate) Then
                    retObj.AddErrorMessage("Invalid start date of work")
                ElseIf newWorkExpInfo.EmpStartDate = "12/31/9999" Then
                    retObj.AddErrorMessage("Invalid start date of work")
                End If
            End If
            'Work End date
            If Not newWorkExpInfo.EmpEndDate Is Nothing Then
                If Not IsDate(newWorkExpInfo.EmpEndDate) Then
                    retObj.AddErrorMessage("Invalid end date of work")
                End If
            End If
            'Title/Designation
            If String.IsNullOrWhiteSpace(newWorkExpInfo.Title) OrElse
               newWorkExpInfo.Title.Length > 50 OrElse
               Not Regex.IsMatch(newWorkExpInfo.Title, Helpers.RegExHelper.Alphanumeric) Then
                retObj.AddErrorMessage("Designation/Title Name must be entered, have less than 50 characters, and only letters and numbers.")
            End If
            'Role/Duties
            If String.IsNullOrWhiteSpace(newWorkExpInfo.JobDuties) Then
                retObj.AddErrorMessage("Role/Duties must be entered.")
            End If

            retObj = Business.Rules.AddressRules.CheckAddressControl(newWorkExpInfo.Address)


            Return retObj
        End Function

    End Class
End Namespace
