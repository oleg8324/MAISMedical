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

Partial Public Class Role_Category_Level_Xref
    Public Property Role_Category_Level_Sid As Integer
    Public Property Role_Sid As Integer
    Public Property Level_Type_Sid As Integer
    Public Property Category_Type_Sid As Integer
    Public Property Start_Date As Date
    Public Property End_Date As Date
    Public Property Active_Flg As Boolean
    Public Property Create_Date As Date
    Public Property Create_By As Integer
    Public Property Last_Update_Date As Date
    Public Property Last_Update_By As Integer

    Public Overridable Property Category_Type As Category_Type
    Public Overridable Property Level_Type As Level_Type
    Public Overridable Property MAIS_Role As MAIS_Role
    Public Overridable Property Certification_Requirement_Information As ICollection(Of Certification_Requirement_Information) = New HashSet(Of Certification_Requirement_Information)
    Public Overridable Property Certification_Requirement_Information1 As ICollection(Of Certification_Requirement_Information) = New HashSet(Of Certification_Requirement_Information)
    Public Overridable Property Role_RN_DD_Personnel_Xref As ICollection(Of Role_RN_DD_Personnel_Xref) = New HashSet(Of Role_RN_DD_Personnel_Xref)
    Public Overridable Property Applications As ICollection(Of Application) = New HashSet(Of Application)
    Public Overridable Property Application_History As ICollection(Of Application_History) = New HashSet(Of Application_History)
    Public Overridable Property Renewal_History_Count As ICollection(Of Renewal_History_Count) = New HashSet(Of Renewal_History_Count)

End Class
