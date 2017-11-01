Imports System.Data.Linq
Imports ODMRDDHelperClassLibrary.Utility
Imports System.Configuration
Imports System.Data.Objects
Imports MAIS.Data
Namespace Queries
    Public Interface ISummaryQueries
        Inherits IQueriesBase
        Function GetAppStatuses() As List(Of Objects.AppStatus)
        Function GetApplicantSid(ByVal code As String, ByVal RN_flg As Boolean) As Integer
        Function GetCertTime(ByVal rcl As Integer, ByVal apptype As String) As Integer
        Function GetContactTypes() As List(Of Objects.Contact)
        Function getRNInfo(ByVal rnsid As Integer) As Objects.PersonalInformationDetails
        Function GetApplicationStatusSummary(ByVal applicationID As Integer) As Boolean
        Function GetEmailDateInHistory(applicationID As Integer) As Boolean
        Function GetCurCertificates(ByVal code As String, ByVal rnflag As Boolean) As List(Of Objects.Certificate)
    End Interface
    Public Class SummaryQueries
        Inherits QueriesBase
        Implements ISummaryQueries
        Public Function GetAppStatuses() As List(Of Objects.AppStatus) Implements ISummaryQueries.GetAppStatuses
            Dim CList As List(Of Objects.AppStatus) = Nothing
            Try
                Using context As New MAISContext()
                    Return (From cs In context.Application_Status_Type Select New Objects.AppStatus() With {
                        .ASTypeSid = cs.Application_Status_Type_Sid,
                        .ASTypeDesc = cs.Application_Status_Desc}).ToList
                End Using
            Catch ex As Exception
                Me.LogError("Error Getting Cert Status Info", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error Getting Cert Status Info", True, False))
            End Try
            Return CList
        End Function
        Public Function GetApplicantSid(ByVal code As String, ByVal RN_flg As Boolean) As Integer Implements ISummaryQueries.GetApplicantSid
            Dim retInt As Integer
            Try
                Using context As New MAISContext
                    If RN_flg Then
                        retInt = (From r In context.RNs
                                       Where r.RNLicense_Number = code
                                       Select r.RN_Sid).FirstOrDefault()
                    Else
                        retInt = (From dd In context.DDPersonnels
                                      Where dd.DDPersonnel_Code = code
                                      Select dd.DDPersonnel_Sid).FirstOrDefault()
                    End If
                End Using

            Catch ex As Exception
                Me.LogError("Error Getting Application sid", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error Getting application sid", True, False))
            End Try
            Return retInt
        End Function
        Public Function GetCertTime(ByVal rcl As Integer, ByVal apptype As String) As Integer Implements ISummaryQueries.GetCertTime
            Dim rval As Integer = 0
            Dim roledesc As String
            Try
                Using context As New MAISContext()
                    roledesc = (From rclxref In context.Role_Category_Level_Xref Where rclxref.Role_Category_Level_Sid = rcl Select rclxref.MAIS_Role.Role_Desc).FirstOrDefault
                    If roledesc = "QA RN" Then
                        rval = 2
                    ElseIf roledesc = "DDPersonnel" Then
                        rval = 1
                    Else
                        rval = 2
                    End If
                    If apptype = "Renewal" Then
                        rval += 10
                    End If
                End Using
            Catch ex As Exception
                Me.LogError("Error Getting Certification time", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error Getting Certification time", True, False))
            End Try
            Return rval
        End Function
        Public Function GetContactTypes() As List(Of Objects.Contact) Implements ISummaryQueries.GetContactTypes
            Dim CList As List(Of Objects.Contact) = Nothing
            Try
                Using context As New MAISContext()
                    Return (From cs In context.Contact_Type Select New Objects.Contact() With {
                        .CTypeSid = cs.Contact_Type_Sid,
                        .CTypeDesc = cs.Contact_Desc}).ToList
                End Using
            Catch ex As Exception
                Me.LogError("Error Getting Contacts Info", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error Getting Contacts Info", True, False))
            End Try
            Return CList
        End Function
        Public Function getRNInfo(ByVal rnsid As Integer) As Objects.PersonalInformationDetails Implements ISummaryQueries.getRNInfo
            Dim rndet As Objects.PersonalInformationDetails = Nothing
            Try
                Using context As New MAISContext()
                    rndet = (From dd In context.RNs Join rnDD In context.RN_DD_Person_Type_Xref On rnDD.RN_DDPersonnel_Sid Equals dd.RN_Sid
                    Where rnDD.RN_DDPersonnel_Sid = rnsid And rnDD.Person_Type_Sid = 1
                            Select New Objects.PersonalInformationDetails() With {
                                .FirstName = dd.First_Name,
                                .LastName = dd.Last_Name,
                                .Gender = dd.Gender,
                                .DOBDateOfIssuance = dd.Date_Of_Original_Issuance,
                                .RNLicenseOrSSN = dd.RNLicense_Number,
                                .MiddleName = dd.Middle_Name
                            }).FirstOrDefault()
                End Using
            Catch ex As Exception
                Me.LogError("Error geting rn information", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error geting rn information", True, False))
            End Try
            Return rndet
        End Function
        Public Function GetApplicationStatusSummary(ByVal applicationID As Integer) As Boolean Implements ISummaryQueries.GetApplicationStatusSummary
            Dim flag As Boolean = False
            Using context As New MAISContext()
                Try
                    If Not IsNothing((From app In context.Applications _
                                                            Where app.Application_Sid = applicationID And (app.Application_Status_Type.Application_Status_Desc <> "Pending" And app.Application_Status_Type.Application_Status_Desc <> "DODD Review" And app.Application_Status_Type.Application_Status_Desc <> "Intent to Deny")
                                                            Select app).FirstOrDefault()) Then
                        Return True
                    End If
                Catch ex As Exception
                    Me.LogError("Error Getting summary page complete rule.", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error summary page complete rule.", True, False))

                End Try
            End Using
            Return flag
        End Function
        Public Function GetEmailDateInHistory(applicationID As Integer) As Boolean Implements ISummaryQueries.GetEmailDateInHistory
            Dim flag As Boolean = False
            Try
                Using context As New MAISContext
                    If applicationID > 0 Then
                        Dim apphis As Application_History = (From ah In context.Application_History
                                                             Where ah.Application_Sid = applicationID
                                                             Select ah).FirstOrDefault()
                        If apphis IsNot Nothing Then
                            apphis.Certification_Sent_Email_Date = DateTime.Now
                            apphis.Last_Update_By = Me.UserID
                            apphis.Last_Update_Date = DateTime.Now
                        End If
                    End If
                    context.SaveChanges()
                    flag = True
                End Using
            Catch ex As Exception
                Me.LogError("Error Getting summary page for email date in history.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error Getting summary page for email date in history.", True, False))
            End Try
            Return flag
        End Function
        'Public Function getAttestantSid(ByVal appid As Integer) As Integer
        '    Try
        '        Using context As New MAISContext()
        '            Return (From at In context.Application_Attestation Where at.Application_Sid=appid Select at.
        '                    dd In context.RNs Join rnDD In context.RN_DD_Person_Type_Xref On rnDD.RN_DDPersonnel_Sid Equals dd.RN_Sid
        '            Where rnDD.RN_DDPersonnel_Sid = rnsid And rnDD.RN_DD_Person_Type_Xref_Sid = 1
        '                    Select New Objects.PersonalInformationDetails() With {
        '                        .FirstName = dd.First_Name,
        '                        .LastName = dd.Last_Name,
        '                        .Gender = dd.Gender,
        '                        .DOBDateOfIssuance = dd.Date_Of_Original_Issuance,
        '                        .RNLicenseOrSSN = dd.RNLicense_Number,
        '                        .MiddleName = dd.Middle_Name
        '                    }
        '                    ).FirstOrDefault()
        '        End Using
        '    Catch ex As Exception
        '        Me.LogError("Error", ex)
        '        Return -1
        '    End Try
        'End Function
        Public Function GetCurCertificates(ByVal code As String, ByVal rnflag As Boolean) As List(Of Objects.Certificate) Implements ISummaryQueries.GetCurCertificates
            Dim clist As List(Of Objects.Certificate) = Nothing
            Dim ptypeSid As Integer = IIf(rnflag, 1, 2)
            Dim rnorddSid = Me.GetApplicantSid(code, rnflag)
            Try
                If rnorddSid > 0 Then
                    Using Context As New MAISContext
                        clist = (From rdx In Context.RN_DD_Person_Type_Xref _
                                 Join mrd In Context.Role_RN_DD_Personnel_Xref On rdx.RN_DD_Person_Type_Xref_Sid Equals mrd.RN_DD_Person_Type_Xref_Sid
                                 Join mh In Context.Certifications On mrd.Role_RN_DD_Personnel_Xref_Sid Equals mh.Role_RN_DD_Personnel_Xref_Sid
                                 Join st In Context.Certification_Status On mh.Certification_Sid Equals st.Certification_Sid
                                  Where rdx.RN_DDPersonnel_Sid = rnorddSid AndAlso rdx.Person_Type_Sid = ptypeSid
                                  Select New Objects.Certificate() With {
                                      .StartDate = mh.Certification_Start_Date,
                                      .EndDate = mh.Certification_End_Date,
                                      .Status = st.Certification_Status_Type.Certification_Status_Desc,
                                      .Role = mrd.Role_Category_Level_Xref.MAIS_Role.Role_Desc,
                                      .Category = mrd.Role_Category_Level_Xref.Category_Type.Category_Desc,
                                      .Level = mrd.Role_Category_Level_Xref.Level_Type.Level_Desc
                        }).ToList

                    End Using
                    's = (From rxr In context.RoleCategoryLevel_Xref Join rl In context.MAIS_Role On rxr.Role_Sid Equals rl.Role_Sid
                    '   Where rxr.RoleCategoryLevel_Sid = rolexrefID
                    '   Select rl.RoleDesc).FirstOrDefault
                End If
            Catch ex As Exception
                Me.LogError("Error Getting current certificates .", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error Getting current certificates .", True, False))
            End Try
            Return clist
        End Function

    End Class
End Namespace
