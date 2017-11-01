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

Partial Public Class Certification
    Public Property Certification_Sid As Integer
    Public Property Role_RN_DD_Personnel_Xref_Sid As Integer
    Public Property Attestant As Nullable(Of Integer)
    Public Property Certification_Start_Date As Date
    Public Property Certification_End_Date As Date
    Public Property Application_Sid As Nullable(Of Integer)
    Public Property Active_Flg As Boolean
    Public Property Create_Date As Date
    Public Property Create_By As Integer
    Public Property Last_Update_Date As Date
    Public Property Last_Update_By As Integer
    Public Property Attested_By_Admin_Flg As Boolean

    Public Overridable Property Certification_Status As ICollection(Of Certification_Status) = New HashSet(Of Certification_Status)
    Public Overridable Property RN As RN
    Public Overridable Property Role_RN_DD_Personnel_Xref As Role_RN_DD_Personnel_Xref

End Class