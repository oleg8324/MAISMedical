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

Partial Public Class Certification_Status_Type
    Public Property Certification_Status_Type_Sid As Integer
    Public Property Certification_Status_Code As String
    Public Property Certification_Status_Desc As String
    Public Property Active_Flg As Boolean
    Public Property Start_Date As Date
    Public Property End_Date As Date
    Public Property Create_Date As Date
    Public Property Create_By As Integer
    Public Property Last_Update_Date As Date
    Public Property Last_Update_By As Integer

    Public Overridable Property Certification_Status As ICollection(Of Certification_Status) = New HashSet(Of Certification_Status)

End Class
