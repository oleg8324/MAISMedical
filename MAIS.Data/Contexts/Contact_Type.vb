'------------------------------------------------------------------------------
' <auto-generated>
'    This code was generated from a template.
'
'    Manual changes to this file may cause unexpected behavior in your application.
'    Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic

Partial Public Class Contact_Type
    Public Property Contact_Type_Sid As Integer
    Public Property Contact_Code As String
    Public Property Contact_Desc As String
    Public Property Active_flg As Boolean
    Public Property Start_Date As Date
    Public Property End_Date As Date
    Public Property Create_Date As Date
    Public Property Create_By As Integer
    Public Property Last_Update_Date As Date
    Public Property Last_Update_By As Integer

    Public Overridable Property Application_Email_Xref As ICollection(Of Application_Email_Xref) = New HashSet(Of Application_Email_Xref)
    Public Overridable Property Application_Phone_Xref As ICollection(Of Application_Phone_Xref) = New HashSet(Of Application_Phone_Xref)
    Public Overridable Property Application_Employer_Email_Xref As ICollection(Of Application_Employer_Email_Xref) = New HashSet(Of Application_Employer_Email_Xref)
    Public Overridable Property Application_Employer_Phone_Xref As ICollection(Of Application_Employer_Phone_Xref) = New HashSet(Of Application_Employer_Phone_Xref)
    Public Overridable Property Employer_RN_DD_Person_Type_Email_Xref As ICollection(Of Employer_RN_DD_Person_Type_Email_Xref) = New HashSet(Of Employer_RN_DD_Person_Type_Email_Xref)
    Public Overridable Property Employer_RN_DD_Person_Type_Phone_Xref As ICollection(Of Employer_RN_DD_Person_Type_Phone_Xref) = New HashSet(Of Employer_RN_DD_Person_Type_Phone_Xref)
    Public Overridable Property RN_DD_Person_Type_Email_Xref As ICollection(Of RN_DD_Person_Type_Email_Xref) = New HashSet(Of RN_DD_Person_Type_Email_Xref)
    Public Overridable Property RN_DD_Person_Type_Phone_Xref As ICollection(Of RN_DD_Person_Type_Phone_Xref) = New HashSet(Of RN_DD_Person_Type_Phone_Xref)
    Public Overridable Property RN_Application_Work_Experience_Phone_Xref As ICollection(Of RN_Application_Work_Experience_Phone_Xref) = New HashSet(Of RN_Application_Work_Experience_Phone_Xref)
    Public Overridable Property RN_Work_Experience_Email_Xref As ICollection(Of RN_Work_Experience_Email_Xref) = New HashSet(Of RN_Work_Experience_Email_Xref)
    Public Overridable Property RN_Work_Experience_Phone_Xref As ICollection(Of RN_Work_Experience_Phone_Xref) = New HashSet(Of RN_Work_Experience_Phone_Xref)
    Public Overridable Property RN_Application_Work_Experience_Email_Xref As ICollection(Of RN_Application_Work_Experience_Email_Xref) = New HashSet(Of RN_Application_Work_Experience_Email_Xref)

End Class
