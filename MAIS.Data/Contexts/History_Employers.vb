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

Partial Public Class History_Employers
    Public Property History_Employers_SID As Integer
    Public Property Employer_Sid As Integer
    Public Property Employer_Start_Date As Date
    Public Property Employer_End_date As Date
    Public Property Email_SID As Nullable(Of Integer)
    Public Property Phone_SID As Nullable(Of Integer)
    Public Property CEO_First_Name As String
    Public Property CEO_Last_Name As String
    Public Property Created As Nullable(Of Date)

    Public Overridable Property History_Employment As ICollection(Of History_Employment) = New HashSet(Of History_Employment)

End Class
