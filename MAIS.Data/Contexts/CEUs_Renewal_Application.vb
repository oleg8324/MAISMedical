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

Partial Public Class CEUs_Renewal_Application
    Public Property CEUs_Renewal_Application_Sid As Integer
    Public Property Application_Sid As Integer
    Public Property Category_Type_Sid As Integer
    Public Property Attended_Date As Date
    Public Property Total_CEUs As Byte
    Public Property Instructor_Name As String
    Public Property RN_Sid As Nullable(Of Integer)
    Public Property Title As String
    Public Property Course_Description As String
    Public Property Active_Flg As Boolean
    Public Property Create_Date As Date
    Public Property Create_By As Integer
    Public Property Last_Update_Date As Date
    Public Property Last_Update_By As Integer

    Public Overridable Property Category_Type As Category_Type
    Public Overridable Property RN As RN
    Public Overridable Property Application As Application

End Class
