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

Partial Public Class Skill_Verification_Skill_Type_Xref
    Public Property Skill_Verification_Skill_Type_Xref_Sid As Integer
    Public Property Skill_Verification_Sid As Integer
    Public Property Skill_Type_Sid As Integer
    Public Property Active_Flg As Boolean
    Public Property Create_Date As Date
    Public Property Create_By As Integer
    Public Property Last_Update_Date As Date
    Public Property Last_Update_By As Integer

    Public Overridable Property Skill_Type As Skill_Type
    Public Overridable Property Skill_Verification As Skill_Verification
    Public Overridable Property Skill_Verification_Type_CheckList_Xref As ICollection(Of Skill_Verification_Type_CheckList_Xref) = New HashSet(Of Skill_Verification_Type_CheckList_Xref)

End Class
