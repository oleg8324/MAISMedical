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

Partial Public Class RN_Work_Experience
    Public Property RN_Work_Experience_Sid As Integer
    Public Property RN_DD_Person_Type_Xref_Sid As Integer
    Public Property Agency_Name As String
    Public Property WE_Start_Date As Date
    Public Property WE_End_Date As Date
    Public Property Title As String
    Public Property Role_Description As String
    Public Property RN_Experience_Flg As Boolean
    Public Property DD_Experience_Flg As Boolean
    Public Property Active_Flg As Boolean
    Public Property Create_Date As Date
    Public Property Create_By As Integer
    Public Property Last_Update_Date As Date
    Public Property Last_Update_By As Integer

    Public Overridable Property RN_DD_Person_Type_Xref As RN_DD_Person_Type_Xref
    Public Overridable Property RN_Work_Experience_Address_Xref As ICollection(Of RN_Work_Experience_Address_Xref) = New HashSet(Of RN_Work_Experience_Address_Xref)
    Public Overridable Property RN_Work_Experience_Email_Xref As ICollection(Of RN_Work_Experience_Email_Xref) = New HashSet(Of RN_Work_Experience_Email_Xref)
    Public Overridable Property RN_Work_Experience_Phone_Xref As ICollection(Of RN_Work_Experience_Phone_Xref) = New HashSet(Of RN_Work_Experience_Phone_Xref)

End Class
