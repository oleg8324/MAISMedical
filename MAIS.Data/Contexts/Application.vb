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

Partial Public Class Application
    Public Property Application_Sid As Integer
    Public Property Application_Type_Sid As Integer
    Public Property RN_flg As Boolean
    Public Property Application_Status_Type_Sid As Integer
    Public Property Create_Date As Date
    Public Property Create_By As Integer
    Public Property Last_Update_Date As Date
    Public Property Last_Update_By As Integer
    Public Property Signature As String
    Public Property Role_Category_Level_Sid As Integer
    Public Property RN_DD_Unique_Code As String
    Public Property Attestant_Sid As Nullable(Of Integer)
    Public Property Is_Admin_Flg As Boolean

    Public Overridable Property Application_Status_Type As Application_Status_Type
    Public Overridable Property Application_Type As Application_Type
    Public Overridable Property RN As RN
    Public Overridable Property Role_Category_Level_Xref As Role_Category_Level_Xref
    Public Overridable Property Application_Attestation As ICollection(Of Application_Attestation) = New HashSet(Of Application_Attestation)
    Public Overridable Property Application_Course_Xref As ICollection(Of Application_Course_Xref) = New HashSet(Of Application_Course_Xref)
    Public Overridable Property Application_Skill_Verification As ICollection(Of Application_Skill_Verification) = New HashSet(Of Application_Skill_Verification)
    Public Overridable Property CEUs_Renewal_Application As ICollection(Of CEUs_Renewal_Application) = New HashSet(Of CEUs_Renewal_Application)
    Public Overridable Property RN_Application As RN_Application
    Public Overridable Property RN_Application_Work_Experience As ICollection(Of RN_Application_Work_Experience) = New HashSet(Of RN_Application_Work_Experience)
    Public Overridable Property DDPersonnel_Application As DDPersonnel_Application
    Public Overridable Property Application_Employer As ICollection(Of Application_Employer) = New HashSet(Of Application_Employer)
    Public Overridable Property Application_Address_Xref As ICollection(Of Application_Address_Xref) = New HashSet(Of Application_Address_Xref)
    Public Overridable Property Application_Email_Xref As ICollection(Of Application_Email_Xref) = New HashSet(Of Application_Email_Xref)
    Public Overridable Property Application_Phone_Xref As ICollection(Of Application_Phone_Xref) = New HashSet(Of Application_Phone_Xref)
    Public Overridable Property Application_Employer_Address_Xref As ICollection(Of Application_Employer_Address_Xref) = New HashSet(Of Application_Employer_Address_Xref)
    Public Overridable Property Application_Employer_Email_Xref As ICollection(Of Application_Employer_Email_Xref) = New HashSet(Of Application_Employer_Email_Xref)
    Public Overridable Property Application_Employer_Phone_Xref As ICollection(Of Application_Employer_Phone_Xref) = New HashSet(Of Application_Employer_Phone_Xref)
    Public Overridable Property Application_Uploaded_Document As ICollection(Of Application_Uploaded_Document) = New HashSet(Of Application_Uploaded_Document)

End Class
