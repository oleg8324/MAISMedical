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

Partial Public Class Role_RN_DD_Personnel_Xref
    Public Property Role_RN_DD_Personnel_Xref_Sid As Integer
    Public Property RN_DD_Person_Type_Xref_Sid As Integer
    Public Property Role_Category_Level_Sid As Integer
    Public Property Role_Start_Date As Date
    Public Property Role_End_Date As Date
    Public Property Active_Flg As Boolean
    Public Property Create_Date As Date
    Public Property Create_By As Integer
    Public Property Last_Update_Date As Date
    Public Property Last_Update_By As Integer

    Public Overridable Property Certifications As ICollection(Of Certification) = New HashSet(Of Certification)
    Public Overridable Property Person_Course_Xref As ICollection(Of Person_Course_Xref) = New HashSet(Of Person_Course_Xref)
    Public Overridable Property RN_DD_Person_Type_Xref As RN_DD_Person_Type_Xref
    Public Overridable Property Role_Category_Level_Xref As Role_Category_Level_Xref

End Class
