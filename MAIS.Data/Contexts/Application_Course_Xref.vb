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

Partial Public Class Application_Course_Xref
    Public Property Application_Course_Xref_Sid As Integer
    Public Property Course_Sid As Integer
    Public Property Application_Sid As Integer
    Public Property Active_Flg As Boolean
    Public Property Create_Date As Date
    Public Property Create_By As Integer
    Public Property Last_Update_Date As Date
    Public Property Last_Update_By As Integer

    Public Overridable Property Application_Course_Session_Xref As ICollection(Of Application_Course_Session_Xref) = New HashSet(Of Application_Course_Session_Xref)
    Public Overridable Property Course As Course
    Public Overridable Property Application As Application

End Class