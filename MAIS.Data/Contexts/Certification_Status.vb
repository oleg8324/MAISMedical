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

Partial Public Class Certification_Status
    Public Property Certification_Status_Sid As Integer
    Public Property Certification_Sid As Nullable(Of Integer)
    Public Property Certification_Status_Type_Sid As Integer
    Public Property Status_Start_Date As Date
    Public Property Status_End_Date As Date
    Public Property Active_Flg As Boolean
    Public Property Create_Date As Date
    Public Property Create_By As Integer
    Public Property Last_Update_Date As Date
    Public Property Last_Update_By As Integer

    Public Overridable Property Certification_Status_Type As Certification_Status_Type
    Public Overridable Property Certification As Certification

End Class
