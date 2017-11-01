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

Partial Public Class RN
    Public Property RN_Sid As Integer
    Public Property RNLicense_Number As String
    Public Property First_Name As String
    Public Property Last_Name As String
    Public Property Middle_Name As String
    Public Property Create_Date As Date
    Public Property Create_By As Integer
    Public Property Last_Updated_Date As Date
    Public Property Last_Update_By As Integer
    Public Property Date_Of_Original_Issuance As Date
    Public Property Gender As String
    Public Property Active_Flg As Boolean

    Public Overridable Property Certifications As ICollection(Of Certification) = New HashSet(Of Certification)
    Public Overridable Property CEUs_Renewal_Application As ICollection(Of CEUs_Renewal_Application) = New HashSet(Of CEUs_Renewal_Application)
    Public Overridable Property CEUs_Renewal As ICollection(Of CEUs_Renewal) = New HashSet(Of CEUs_Renewal)
    Public Overridable Property Courses As ICollection(Of Course) = New HashSet(Of Course)
    Public Overridable Property RN_Secretary_Association As ICollection(Of RN_Secretary_Association) = New HashSet(Of RN_Secretary_Association)
    Public Overridable Property Applications As ICollection(Of Application) = New HashSet(Of Application)

End Class