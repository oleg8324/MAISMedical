Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Public Class EnumGenerator
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        generate("")
    End Sub

    Public Sub generate(sep As String)
        Dim sb As New StringBuilder()

        Dim conn As New SqlConnection(ConfigurationManager.ConnectionStrings("MAISContext").ConnectionString)
        conn.Open()
        Dim myCmd1 As SqlCommand = New SqlCommand("SELECT * FROM [lkp].[Employer_Type]", conn)
        Dim myCmd2 As SqlCommand = New SqlCommand("SELECT * FROM [lkp].[Contact_Type]", conn)
        Dim myCmd3 As SqlCommand = New SqlCommand("SELECT * FROM [lkp].[Address_Type]", conn)
        Dim myCmd4 As SqlCommand = New SqlCommand("SELECT * FROM [lkp].[Application_Type]", conn)
        Dim myCmd5 As SqlCommand = New SqlCommand("SELECT * FROM [lkp].[ApplicationStatus_Type]", conn)
        Dim myCmd6 As SqlCommand = New SqlCommand("SELECT * FROM [lkp].[CertificationStatus_Type]", conn)
        Dim myCmd7 As SqlCommand = New SqlCommand("SELECT * FROM [lkp].[Notation_Type]", conn)
        Dim myCmd8 As SqlCommand = New SqlCommand("SELECT * FROM [lkp].[Reasons_For_Notation]", conn)
        'add roles, cats and levels above

        sb.AppendLine("Imports System.ComponentModel  " & sep)
        sb.AppendLine("Namespace Model.Enums" & sep)
        'First Enum
        sb.AppendLine("Public Enum EmployerType" & sep)
        Using rd1 As SqlDataReader = myCmd1.ExecuteReader()
            Do While rd1.Read
                sb.AppendLine("<Description(" & Chr(34) & rd1("EmployerDesc") & Chr(34) & ")> _" & sep)
                sb.AppendLine(rd1("EmployerCode") & "=" & rd1("EmployerType_Sid") & sep)
            Loop
        End Using
        sb.AppendLine("End Enum" & sep)
        'Next enum
        sb.AppendLine("Public Enum ContactType" & sep)
        Using rd1 As SqlDataReader = myCmd2.ExecuteReader()
            Do While rd1.Read
                sb.AppendLine("<Description(" & Chr(34) & rd1("ContactDesc") & Chr(34) & ")> _" & sep)
                sb.AppendLine(rd1("ContactCode") & "=" & rd1("ContactType_Sid") & sep)
            Loop
        End Using
        sb.AppendLine("End Enum" & sep)

        sb.AppendLine("Public Enum AddressType" & sep)
        Using rd1 As SqlDataReader = myCmd3.ExecuteReader()
            Do While rd1.Read
                sb.AppendLine("<Description(" & Chr(34) & rd1("AddressDesc") & Chr(34) & ")> _" & sep)
                sb.AppendLine(rd1("AddressCode") & "=" & rd1("AddressType_Sid") & sep)
            Loop
        End Using
        sb.AppendLine("End Enum" & sep)

        sb.AppendLine("Public Enum ApplicationType" & sep)
        Using rd1 As SqlDataReader = myCmd4.ExecuteReader()
            Do While rd1.Read
                sb.AppendLine("<Description(" & Chr(34) & rd1("ApplicationDesc") & Chr(34) & ")> _" & sep)
                sb.AppendLine(rd1("ApplicationCode") & "=" & rd1("ApplicationType_Sid") & sep)
            Loop
        End Using
        sb.AppendLine("End Enum" & sep)

        sb.AppendLine("Public Enum ApplicationStatusType" & sep)
        Using rd1 As SqlDataReader = myCmd5.ExecuteReader()
            Do While rd1.Read
                sb.AppendLine("<Description(" & Chr(34) & rd1("ApplicationStatusDesc") & Chr(34) & ")> _" & sep)
                sb.AppendLine(rd1("ApplicationStatusCode") & "=" & rd1("ApplicationStatusType_Sid") & sep)
            Loop
        End Using
        sb.AppendLine("End Enum" & sep)

        sb.AppendLine("Public Enum CertificationStatusType" & sep)
        Using rd1 As SqlDataReader = myCmd6.ExecuteReader()
            Do While rd1.Read
                sb.AppendLine("<Description(" & Chr(34) & rd1("CertificationStatusDesc") & Chr(34) & ")> _" & sep)
                sb.AppendLine(rd1("CertificationStatusCode") & "=" & rd1("CertificationStatus_Type_Sid") & sep)
            Loop
        End Using
        sb.AppendLine("End Enum" & sep)

        sb.AppendLine("Public Enum NotationType" & sep)
        Using rd1 As SqlDataReader = myCmd7.ExecuteReader()
            Do While rd1.Read
                sb.AppendLine("<Description(" & Chr(34) & rd1("NotationDesc") & Chr(34) & ")> _" & sep)
                sb.AppendLine(rd1("NotationCode") & "=" & rd1("Notation_Type_Sid") & sep)
            Loop
        End Using
        sb.AppendLine("End Enum" & sep)

        sb.AppendLine("Public Enum ReasonsForNotation" & sep)
        Using rd1 As SqlDataReader = myCmd8.ExecuteReader()
            Do While rd1.Read
                sb.AppendLine("<Description(" & Chr(34) & rd1("ReasonDesc") & Chr(34) & ")> _" & sep)
                sb.AppendLine(rd1("ReasonCode") & "=" & rd1("Reasons_For_Notation_Sid") & sep)
            Loop
        End Using
        sb.AppendLine("End Enum" & sep)

        sb.AppendLine("End Namespace" & sep)
        Using outfile As New StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\Enums.vb")
            outfile.Write(sb.ToString())
        End Using

    End Sub

End Class