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

Partial Public Class RN_DD_Person_Type_Xref
    Public Property RN_DD_Person_Type_Xref_Sid As Integer
    Public Property RN_DDPersonnel_Sid As Integer
    Public Property Person_Type_Sid As Integer
    Public Property Active_Flg As Boolean
    Public Property Create_Date As Date
    Public Property Create_By As Integer
    Public Property Last_Update_Date As Date
    Public Property Last_Update_By As Integer

    Public Overridable Property Person_Type As Person_Type
    Public Overridable Property Employer_RN_DD_Person_Type_Xref As ICollection(Of Employer_RN_DD_Person_Type_Xref) = New HashSet(Of Employer_RN_DD_Person_Type_Xref)
    Public Overridable Property Role_RN_DD_Personnel_Xref As ICollection(Of Role_RN_DD_Personnel_Xref) = New HashSet(Of Role_RN_DD_Personnel_Xref)
    Public Overridable Property Skill_Verification As ICollection(Of Skill_Verification) = New HashSet(Of Skill_Verification)
    Public Overridable Property CEUs_Renewal As ICollection(Of CEUs_Renewal) = New HashSet(Of CEUs_Renewal)
    Public Overridable Property RN_Work_Experience As ICollection(Of RN_Work_Experience) = New HashSet(Of RN_Work_Experience)
    Public Overridable Property Notations As ICollection(Of Notation) = New HashSet(Of Notation)
    Public Overridable Property Employer_RN_DD_Person_Type_Address_Xref As ICollection(Of Employer_RN_DD_Person_Type_Address_Xref) = New HashSet(Of Employer_RN_DD_Person_Type_Address_Xref)
    Public Overridable Property Employer_RN_DD_Person_Type_Email_Xref As ICollection(Of Employer_RN_DD_Person_Type_Email_Xref) = New HashSet(Of Employer_RN_DD_Person_Type_Email_Xref)
    Public Overridable Property Employer_RN_DD_Person_Type_Phone_Xref As ICollection(Of Employer_RN_DD_Person_Type_Phone_Xref) = New HashSet(Of Employer_RN_DD_Person_Type_Phone_Xref)
    Public Overridable Property RN_DD_Person_Type_Address_Xref As ICollection(Of RN_DD_Person_Type_Address_Xref) = New HashSet(Of RN_DD_Person_Type_Address_Xref)
    Public Overridable Property RN_DD_Person_Type_Email_Xref As ICollection(Of RN_DD_Person_Type_Email_Xref) = New HashSet(Of RN_DD_Person_Type_Email_Xref)
    Public Overridable Property RN_DD_Person_Type_Phone_Xref As ICollection(Of RN_DD_Person_Type_Phone_Xref) = New HashSet(Of RN_DD_Person_Type_Phone_Xref)
    Public Overridable Property History_Employment As ICollection(Of History_Employment) = New HashSet(Of History_Employment)
    Public Overridable Property Application_History As ICollection(Of Application_History) = New HashSet(Of Application_History)
    Public Overridable Property DODD_Message_RN_DD_Person_Type_Xref_Xref As ICollection(Of DODD_Message_RN_DD_Person_Type_Xref_Xref) = New HashSet(Of DODD_Message_RN_DD_Person_Type_Xref_Xref)
    Public Overridable Property Renewal_History_Count As ICollection(Of Renewal_History_Count) = New HashSet(Of Renewal_History_Count)

End Class
