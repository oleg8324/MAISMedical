Imports System.Data.Linq
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Data.Objects

Namespace Queries
    Public Interface ICertificateQueries
        Inherits IQueriesBase
        Function GetCertificateInfo(ByVal rnLicenseNumber As String, ByVal applicationType As String, ByVal role As Integer) As List(Of Objects.CertificationInfo)
        Function GetCertificateDDInfo(ByVal ddCode As String, ByVal applicationType As String, ByVal role As Integer) As List(Of Objects.CertificationInfo)
        Function GetCertificateInfoUsingCertMod(ByVal rnLicenseNumber As String, ByVal applicationType As String, ByVal role As Integer, ByVal certID As Integer) As List(Of Objects.CertificationInfo)
        Function GetCertificateDDInfoUsingCertMod(ByVal ddCode As String, ByVal applicationType As String, ByVal role As Integer, ByVal certID As Integer) As List(Of Objects.CertificationInfo)
        Function GetCertificateDetialsInfo(rnLicenseNumberDD As String, rnDD As Boolean) As List(Of Objects.CertificationDetails)
        Function GetCertificationDateInHistory(ByVal applicationID As Integer) As Boolean
    End Interface
    Public Class CertificateQueries
        Inherits QueriesBase
        Implements ICertificateQueries

        Public Function GetCertificateInfo(ByVal rnLicenseNumber As String, ByVal applicationType As String, ByVal role As Integer) As List(Of CertificationInfo) Implements ICertificateQueries.GetCertificateInfo
            Dim certinfo As New List(Of Objects.CertificationInfo)
            Using context As New MAISContext()
                Try
                    If (role <> 5) Then
                        If (applicationType = "Initial" Or applicationType = "AddOn") Then
                            Dim _count As Integer = (From cert In context.Certifications
                                                     Join roleRN In context.Role_RN_DD_Personnel_Xref On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRN.Role_RN_DD_Personnel_Xref_Sid
                                                     Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRN.Role_Category_Level_Sid
                                                     Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRN.RN_DD_Person_Type_Xref_Sid
                                                    Join r In context.RNs On r.RN_Sid Equals rn.RN_DDPersonnel_Sid
                                                    Join appHistory In context.Application_History On appHistory.Application_Sid Equals cert.Application_Sid
                                                    Where r.RNLicense_Number = rnLicenseNumber And role1.Role_Category_Level_Sid = role And appHistory.Application_Type_Sid <> 2 Select cert.Certification_Sid).FirstOrDefault()
                            If (_count > 0) Then
                                certinfo = (From personSession In context.Person_Course_Session_Xref
                                            Join personCourse In context.Person_Course_Xref On personCourse.Person_Course_Xref_Sid Equals personSession.Person_Course_Xref_Sid
                                            Join course In context.Courses On course.Course_sid Equals personCourse.Course_Sid
                                            Join session In context.Sessions On session.Session_Sid Equals personSession.Session_Sid
                                            Join roleRn In context.Role_RN_DD_Personnel_Xref On personCourse.Role_RN_DD_Personnel_Xref_Sid Equals roleRn.Role_RN_DD_Personnel_Xref_Sid
                                            Join cert In context.Certifications On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRn.Role_RN_DD_Personnel_Xref_Sid
                                            Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRn.Role_Category_Level_Sid
                                            Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRn.RN_DD_Person_Type_Xref_Sid
                                            Join r In context.RNs On r.RN_Sid Equals rn.RN_DDPersonnel_Sid
                                            Join rSession In context.RNs On rSession.RN_Sid Equals course.RN_Sid
                                            Join appHistory In context.Application_History On appHistory.Application_Sid Equals cert.Application_Sid
                                            Where r.RNLicense_Number = rnLicenseNumber And role1.Role_Category_Level_Sid = role And appHistory.Application_Type_Sid <> 2
                                            Select New Objects.CertificationInfo() With {
                                                .StartDatesOfTraining = session.Start_Date,
                                                .EndDatesOfTraining = session.End_Date,
                                                .StartDatesOfCertification = cert.Certification_Start_Date,
                                                .EndDatesOfCertification = cert.Certification_End_Date,
                                                .TotalCEs = course.Total_CEs,
                                                .TotalACEs = course.Category_A_CEs,
                                                .LocationOfTraining = session.Location_Name,
                                .OBNNumber = course.OBN_Course_Number,
                                                .RNInstructorName = rSession.First_Name,
                                                .RNLastInstructorName = rSession.Last_Name,
                                                .RNMiddleInstructorName = rSession.Middle_Name,
                                                .SponsorName = session.Sponsor,
                                                .RNName = r.First_Name,
                                                .RNLastName = r.Last_Name,
                                                .RNMiddleName = r.Middle_Name
                                                }).ToList()
                            End If
                        End If
                    Else
                        If (applicationType <> "Renewal") Then
                            Dim _count As Integer = (From cert In context.Certifications
                                                         Join roleRN In context.Role_RN_DD_Personnel_Xref On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRN.Role_RN_DD_Personnel_Xref_Sid
                                                         Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRN.Role_Category_Level_Sid
                                                         Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRN.RN_DD_Person_Type_Xref_Sid
                                                        Join r In context.RNs On r.RN_Sid Equals rn.RN_DDPersonnel_Sid
                                                        Join appHistory In context.Application_History On appHistory.Application_Sid Equals cert.Application_Sid
                                                        Where r.RNLicense_Number = rnLicenseNumber And role1.Role_Category_Level_Sid = role And appHistory.Application_Type_Sid <> 2 Select cert.Certification_Sid).FirstOrDefault()
                            If (_count > 0) Then
                                certinfo = (From roleRn In context.Role_RN_DD_Personnel_Xref
                                            Join cert In context.Certifications On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRn.Role_RN_DD_Personnel_Xref_Sid
                                            Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRn.Role_Category_Level_Sid
                                            Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRn.RN_DD_Person_Type_Xref_Sid
                                            Join r In context.RNs On r.RN_Sid Equals rn.RN_DDPersonnel_Sid
                                            Join appHistory In context.Application_History On appHistory.Application_Sid Equals cert.Application_Sid
                                            Join app In context.Applications On app.Application_Sid Equals appHistory.Application_Sid
                                            Group Join rRN In context.RNs On rRN.RN_Sid Equals app.Attestant_Sid Into appRN = Group
                                            From rnAppID In appRN.DefaultIfEmpty()
                                            Where r.RNLicense_Number = rnLicenseNumber And role1.Role_Category_Level_Sid = role And appHistory.Application_Type_Sid <> 2
                                            Select New Objects.CertificationInfo() With {
                                                .StartDatesOfTraining = DateTime.Now,
                                                .EndDatesOfTraining = DateTime.Now,
                                                .StartDatesOfCertification = cert.Certification_Start_Date,
                                                .EndDatesOfCertification = cert.Certification_End_Date,
                                                .TotalCEs = 0.0,
                                                .TotalACEs = 0.0,
                                                .LocationOfTraining = String.Empty,
                                                .OBNNumber = String.Empty,
                                                .RNInstructorName = rnAppID.First_Name,
                                                .RNLastInstructorName = rnAppID.Last_Name,
                                                .RNMiddleInstructorName = rnAppID.Middle_Name,
                                                .SponsorName = String.Empty,
                                                .RNName = r.First_Name,
                                                .RNLastName = r.Last_Name,
                                .RNMiddleName = r.Middle_Name
                                                }).ToList()
                            End If
                        End If
                    End If
                    If (applicationType = "Renewal") Then
                        Dim _count As Integer = (From cert In context.Certifications
                                                 Join roleRN In context.Role_RN_DD_Personnel_Xref On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRN.Role_RN_DD_Personnel_Xref_Sid
                                                 Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRN.Role_Category_Level_Sid
                                                 Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRN.RN_DD_Person_Type_Xref_Sid
                                                Join r In context.RNs On r.RN_Sid Equals rn.RN_DDPersonnel_Sid
                                                Join appHistory In context.Application_History On appHistory.Application_Sid Equals cert.Application_Sid
                                                Where r.RNLicense_Number = rnLicenseNumber And role1.Role_Category_Level_Sid = role And appHistory.Application_Type_Sid = 2 Select cert.Certification_Sid).FirstOrDefault()
                        If (_count > 0) Then
                            certinfo = (From roleRn In context.Role_RN_DD_Personnel_Xref
                                        Join cert In context.Certifications On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRn.Role_RN_DD_Personnel_Xref_Sid
                                        Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRn.Role_Category_Level_Sid
                                        Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRn.RN_DD_Person_Type_Xref_Sid
                                        Join r In context.RNs On r.RN_Sid Equals rn.RN_DDPersonnel_Sid
                                        Join appHistory In context.Application_History On appHistory.Application_Sid Equals cert.Application_Sid
                                        Where r.RNLicense_Number = rnLicenseNumber And role1.Role_Category_Level_Sid = role And appHistory.Application_Type_Sid = 2
                                        Select New Objects.CertificationInfo() With {
                                            .StartDatesOfCertification = cert.Certification_Start_Date,
                                            .EndDatesOfCertification = cert.Certification_End_Date,
                                            .RNName = r.First_Name,
                                            .RNLastName = r.Last_Name,
                                            .RNMiddleName = r.Middle_Name
                                            }).ToList()
                        End If
                    End If
                Catch ex As Exception
                    Me.LogError("Error Getting in certification Info", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting in certification Info", True, False))
                    Throw
                End Try
            End Using
            Return certinfo
        End Function

        Public Function GetCertificateDDInfo(ddCode As String, applicationType As String, ByVal role As Integer) As List(Of CertificationInfo) Implements ICertificateQueries.GetCertificateDDInfo
            Dim certinfo As New List(Of Objects.CertificationInfo)
            Using context As New MAISContext()
                Try
                    If (applicationType = "Initial" Or applicationType = "AddOn") Then
                        Dim _count As Integer = (From cert In context.Certifications
                                                 Join roleRN In context.Role_RN_DD_Personnel_Xref On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRN.Role_RN_DD_Personnel_Xref_Sid
                                                 Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRN.Role_Category_Level_Sid
                                                 Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRN.RN_DD_Person_Type_Xref_Sid
                                                Join r In context.DDPersonnels On r.DDPersonnel_Sid Equals rn.RN_DDPersonnel_Sid
                                                Join appHistory In context.Application_History On appHistory.Application_Sid Equals cert.Application_Sid
                                                Where r.DDPersonnel_Code = ddCode And role1.Role_Category_Level_Sid = role And appHistory.Application_Type_Sid <> 2 Select cert.Certification_Sid).FirstOrDefault()
                        If (_count > 0) Then
                            certinfo = (From personSession In context.Person_Course_Session_Xref
                                        Join personCourse In context.Person_Course_Xref On personCourse.Person_Course_Xref_Sid Equals personSession.Person_Course_Xref_Sid
                                        Join course In context.Courses On course.Course_sid Equals personCourse.Course_Sid
                                        Join session In context.Sessions On session.Session_Sid Equals personSession.Session_Sid
                                        Join roleRn In context.Role_RN_DD_Personnel_Xref On personCourse.Role_RN_DD_Personnel_Xref_Sid Equals roleRn.Role_RN_DD_Personnel_Xref_Sid
                                        Join cert In context.Certifications On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRn.Role_RN_DD_Personnel_Xref_Sid
                                        Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRn.Role_Category_Level_Sid
                                        Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRn.RN_DD_Person_Type_Xref_Sid
                                        Join r In context.DDPersonnels On r.DDPersonnel_Sid Equals rn.RN_DDPersonnel_Sid
                                        Join rSession In context.RNs On rSession.RN_Sid Equals course.RN_Sid
                                        Join appHistory In context.Application_History On appHistory.Application_Sid Equals cert.Application_Sid
                                        Where r.DDPersonnel_Code = ddCode And role1.Role_Category_Level_Sid = role And appHistory.Application_Type_Sid <> 2
                                        Select New Objects.CertificationInfo() With {
                                            .StartDatesOfTraining = session.Start_Date,
                                            .EndDatesOfTraining = session.End_Date,
                                            .StartDatesOfCertification = cert.Certification_Start_Date,
                                            .EndDatesOfCertification = cert.Certification_End_Date,
                                            .TotalCEs = course.Total_CEs,
                                            .RNInstructorName = rSession.First_Name,
                                            .RNLastInstructorName = rSession.Last_Name,
                                            .RNMiddleInstructorName = rSession.Middle_Name,
                                            .RNName = r.First_Name,
                                            .RNLastName = r.Last_Name,
                                            .RNMiddleName = r.Middle_Name
                                            }).ToList()
                        End If
                    End If
                    If (applicationType = "Renewal") Then
                        Dim _count As Integer = (From cert In context.Certifications
                                                 Join roleRN In context.Role_RN_DD_Personnel_Xref On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRN.Role_RN_DD_Personnel_Xref_Sid
                                                 Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRN.Role_Category_Level_Sid
                                                 Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRN.RN_DD_Person_Type_Xref_Sid
                                                Join r In context.DDPersonnels On r.DDPersonnel_Sid Equals rn.RN_DDPersonnel_Sid
                                                Join appHistory In context.Application_History On appHistory.Application_Sid Equals cert.Application_Sid
                                                Where r.DDPersonnel_Code = ddCode And role1.Role_Category_Level_Sid = role And appHistory.Application_Type_Sid = 2 Select cert.Certification_Sid).FirstOrDefault()
                        If (_count > 0) Then
                            certinfo = (From roleRn In context.Role_RN_DD_Personnel_Xref
                                        Join cert In context.Certifications On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRn.Role_RN_DD_Personnel_Xref_Sid
                                        Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRn.Role_Category_Level_Sid
                                        Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRn.RN_DD_Person_Type_Xref_Sid
                                        Join r In context.DDPersonnels On r.DDPersonnel_Sid Equals rn.RN_DDPersonnel_Sid
                                        Join appHistory In context.Application_History On appHistory.Application_Sid Equals cert.Application_Sid
                                        Join app In context.Applications On app.Application_Sid Equals appHistory.Application_Sid
                                        Group Join rRN In context.RNs On rRN.RN_Sid Equals app.Attestant_Sid Into appRN = Group
                                        From rnAppID In appRN.DefaultIfEmpty()
                            Where r.DDPersonnel_Code = ddCode And role1.Role_Category_Level_Sid = role And appHistory.Application_Type_Sid = 2
                                        Select New Objects.CertificationInfo() With {
                                            .StartDatesOfCertification = cert.Certification_Start_Date,
                                            .EndDatesOfCertification = cert.Certification_End_Date,
                                            .RNInstructorName = rnAppID.First_Name,
                                            .RNLastInstructorName = rnAppID.Last_Name,
                                            .RNMiddleInstructorName = rnAppID.Middle_Name,
                                            .RNName = r.First_Name,
                                            .RNLastName = r.Last_Name,
                                            .RNMiddleName = r.Middle_Name
                                            }).ToList()
                        End If
                    End If
                Catch ex As Exception
                    Me.LogError("Error Getting in dd certification Info", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting in dd certification Info", True, False))
                End Try
            End Using
            Return certinfo
        End Function

        Public Function GetCertificateDetialsInfo(rnLicenseNumberDD As String, rnDD As Boolean) As List(Of CertificationDetails) Implements ICertificateQueries.GetCertificateDetialsInfo
            Dim certDetails As New List(Of CertificationDetails)
            Using context As New MAISContext()
                Try
                    If (rnDD) Then
                        certDetails = (From cert In context.Certifications
                                       Join rnDDRole In context.Role_RN_DD_Personnel_Xref On rnDDRole.Role_RN_DD_Personnel_Xref_Sid Equals cert.Role_RN_DD_Personnel_Xref_Sid
                                       Join role In context.Role_Category_Level_Xref On role.Role_Category_Level_Sid Equals rnDDRole.Role_Category_Level_Sid
                                       Join rnPersonType In context.RN_DD_Person_Type_Xref On rnPersonType.RN_DD_Person_Type_Xref_Sid Equals rnDDRole.RN_DD_Person_Type_Xref_Sid
                                       Join rn In context.RNs On rn.RN_Sid Equals rnPersonType.RN_DDPersonnel_Sid
                                       Join appHistory In context.Application_History On appHistory.Application_Sid Equals cert.Application_Sid
                                       Join appType In context.Application_Type On appType.Application_Type_Sid Equals appHistory.Application_Type_Sid
                        Where cert.Active_Flg = True And rn.RNLicense_Number = rnLicenseNumberDD
                                       Select New Objects.CertificationDetails() With {
                                           .RoleLevelCategoryID = role.Role_Category_Level_Sid,
                                .CertificateID = cert.Certification_Sid,
                                           .ApplicationType = appType.Application_Desc
                                           }).Distinct.ToList()
                    Else
                        certDetails = (From cert In context.Certifications
                                       Join rnDDRole In context.Role_RN_DD_Personnel_Xref On rnDDRole.Role_RN_DD_Personnel_Xref_Sid Equals cert.Role_RN_DD_Personnel_Xref_Sid
                                       Join role In context.Role_Category_Level_Xref On role.Role_Category_Level_Sid Equals rnDDRole.Role_Category_Level_Sid
                                       Join rnPersonType In context.RN_DD_Person_Type_Xref On rnPersonType.RN_DD_Person_Type_Xref_Sid Equals rnDDRole.RN_DD_Person_Type_Xref_Sid
                                       Join rn In context.DDPersonnels On rn.DDPersonnel_Sid Equals rnPersonType.RN_DDPersonnel_Sid
                                       Join appHistory In context.Application_History On appHistory.Application_Sid Equals cert.Application_Sid
                                       Join appType In context.Application_Type On appType.Application_Type_Sid Equals appHistory.Application_Type_Sid
                                       Where cert.Active_Flg = True And rn.DDPersonnel_Code = rnLicenseNumberDD
                                       Select New Objects.CertificationDetails() With {
                                           .RoleLevelCategoryID = role.Role_Category_Level_Sid,
                                .CertificateID = cert.Certification_Sid,
                                           .ApplicationType = appType.Application_Desc
                                           }).Distinct.ToList()
                    End If
                    For Each certDet As Objects.CertificationDetails In certDetails
                        certDet.CertificateStatus = (From certStatus In context.Certification_Status
                                                     Join certStatusTYpe In context.Certification_Status_Type On certStatusTYpe.Certification_Status_Type_Sid Equals certStatus.Certification_Status_Type_Sid
                                                     Where certStatus.Active_Flg = True And certStatusTYpe.Active_Flg = True And certStatus.Certification_Sid = certDet.CertificateID Order By certStatus.Last_Update_Date Descending
                                                     Select certStatusTYpe.Certification_Status_Code).FirstOrDefault()
                    Next
                Catch ex As Exception
                    Me.LogError("Error Getting in certification details Info", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting in certification details Info", True, False))
                    Throw
                End Try
            End Using
            Return certDetails
        End Function

        Public Function GetCertificationDateInHistory(applicationID As Integer) As Boolean Implements ICertificateQueries.GetCertificationDateInHistory
            Dim flag As Boolean = False
            Try
                Using context As New MAISContext
                    If applicationID > 0 Then
                        Dim apphis As Application_History = (From ah In context.Application_History
                                                             Where ah.Application_Sid = applicationID
                                                             Select ah).FirstOrDefault()
                        If apphis IsNot Nothing Then
                            apphis.Certification_Printed_Date = DateTime.Now
                            apphis.Last_Update_By = Me.UserID
                            apphis.Last_Update_Date = DateTime.Now
                        End If
                    End If
                    context.SaveChanges()
                    flag = True
                End Using
            Catch ex As Exception
                Me.LogError("Error Getting summary page for view or print certificate date in history.", CInt(Me.UserID), ex)
            End Try
            Return flag
        End Function

        Public Function GetCertificateDDInfoUsingCertMod(ddCode As String, applicationType As String, role As Integer, ByVal certID As Integer) As List(Of CertificationInfo) Implements ICertificateQueries.GetCertificateDDInfoUsingCertMod
            Dim certinfo As New List(Of Objects.CertificationInfo)
            Using context As New MAISContext()
                Try
                    If (applicationType = "Initial" Or applicationType = "AddOn") Then
                        Dim _count As Integer = (From cert In context.Certifications
                                                 Join roleRN In context.Role_RN_DD_Personnel_Xref On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRN.Role_RN_DD_Personnel_Xref_Sid
                                                 Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRN.Role_Category_Level_Sid
                                                 Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRN.RN_DD_Person_Type_Xref_Sid
                                                Join r In context.DDPersonnels On r.DDPersonnel_Sid Equals rn.RN_DDPersonnel_Sid
                                                Join appHistory In context.Renewal_History_Count On appHistory.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid Where appHistory.Role_Category_Level_sid = role
                                                Where r.DDPersonnel_Code = ddCode And role1.Role_Category_Level_Sid = role And appHistory.Application_type_SId <> 2 And cert.Certification_Sid = certID Select cert.Certification_Sid).FirstOrDefault()
                        If (_count > 0) Then
                            certinfo = (From personSession In context.Person_Course_Session_Xref
                                        Join personCourse In context.Person_Course_Xref On personCourse.Person_Course_Xref_Sid Equals personSession.Person_Course_Xref_Sid
                                        Join course In context.Courses On course.Course_sid Equals personCourse.Course_Sid
                                        Join session In context.Sessions On session.Session_Sid Equals personSession.Session_Sid
                                        Join roleRn In context.Role_RN_DD_Personnel_Xref On personCourse.Role_RN_DD_Personnel_Xref_Sid Equals roleRn.Role_RN_DD_Personnel_Xref_Sid
                                        Join cert In context.Certifications On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRn.Role_RN_DD_Personnel_Xref_Sid
                                        Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRn.Role_Category_Level_Sid
                                        Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRn.RN_DD_Person_Type_Xref_Sid
                                        Join r In context.DDPersonnels On r.DDPersonnel_Sid Equals rn.RN_DDPersonnel_Sid
                                        Join rSession In context.RNs On rSession.RN_Sid Equals course.RN_Sid
                                        Join appHistory In context.Renewal_History_Count On appHistory.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid Where appHistory.Role_Category_Level_sid = role
                                        Where r.DDPersonnel_Code = ddCode And role1.Role_Category_Level_Sid = role And appHistory.Application_type_SId <> 2 And cert.Certification_Sid = certID
                                        Select New Objects.CertificationInfo() With {
                                            .StartDatesOfTraining = session.Start_Date,
                                            .EndDatesOfTraining = session.End_Date,
                                            .StartDatesOfCertification = cert.Certification_Start_Date,
                                            .EndDatesOfCertification = cert.Certification_End_Date,
                                            .TotalCEs = course.Total_CEs,
                                            .RNInstructorName = rSession.First_Name,
                                            .RNLastInstructorName = rSession.Last_Name,
                                            .RNMiddleInstructorName = rSession.Middle_Name,
                                            .RNName = r.First_Name,
                                            .RNLastName = r.Last_Name,
                                            .RNMiddleName = r.Middle_Name
                                            }).ToList()
                        End If
                    End If
                    If (applicationType = "Renewal") Then
                        Dim _count As Integer = (From cert In context.Certifications
                                                 Join roleRN In context.Role_RN_DD_Personnel_Xref On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRN.Role_RN_DD_Personnel_Xref_Sid
                                                 Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRN.Role_Category_Level_Sid
                                                 Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRN.RN_DD_Person_Type_Xref_Sid
                                                Join r In context.DDPersonnels On r.DDPersonnel_Sid Equals rn.RN_DDPersonnel_Sid
                                                Join appHistory In context.Renewal_History_Count On appHistory.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid Where appHistory.Role_Category_Level_sid = role
                                                Where r.DDPersonnel_Code = ddCode And role1.Role_Category_Level_Sid = role And appHistory.Application_type_SId = 2 And cert.Certification_Sid = certID Select cert.Certification_Sid).FirstOrDefault()
                        If (_count > 0) Then
                            certinfo = (From roleRn In context.Role_RN_DD_Personnel_Xref
                                        Join cert In context.Certifications On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRn.Role_RN_DD_Personnel_Xref_Sid
                                        Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRn.Role_Category_Level_Sid
                                        Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRn.RN_DD_Person_Type_Xref_Sid
                                        Join r In context.DDPersonnels On r.DDPersonnel_Sid Equals rn.RN_DDPersonnel_Sid
                                        Join appHistory In context.Renewal_History_Count On appHistory.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid Where appHistory.Role_Category_Level_sid = role
                                        Group Join rRN In context.RNs On rRN.RN_Sid Equals cert.Attestant Into appRN = Group
                                        From rnAppID In appRN.DefaultIfEmpty()
                            Where r.DDPersonnel_Code = ddCode And role1.Role_Category_Level_Sid = role And appHistory.Application_type_SId = 2 And cert.Certification_Sid = certID
                                        Select New Objects.CertificationInfo() With {
                                            .StartDatesOfCertification = cert.Certification_Start_Date,
                                            .EndDatesOfCertification = cert.Certification_End_Date,
                                            .RNInstructorName = rnAppID.First_Name,
                                            .RNLastInstructorName = rnAppID.Last_Name,
                                            .RNMiddleInstructorName = rnAppID.Middle_Name,
                                            .RNName = r.First_Name,
                                            .RNLastName = r.Last_Name,
                                            .RNMiddleName = r.Middle_Name
                                            }).ToList()
                        End If
                    End If
                Catch ex As Exception
                    Me.LogError("Error Getting in dd certification Info in certification modification", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting in dd certification Info in certification modification", True, False))
                End Try
            End Using
            Return certinfo
        End Function

        Public Function GetCertificateInfoUsingCertMod(rnLicenseNumber As String, applicationType As String, role As Integer, ByVal certID As Integer) As List(Of CertificationInfo) Implements ICertificateQueries.GetCertificateInfoUsingCertMod
            Dim certinfo As New List(Of Objects.CertificationInfo)
            Using context As New MAISContext()
                Try
                    If (role <> 5) Then
                        If (applicationType = "Initial" Or applicationType = "AddOn") Then
                            Dim _count As Integer = (From cert In context.Certifications
                                                     Join roleRN In context.Role_RN_DD_Personnel_Xref On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRN.Role_RN_DD_Personnel_Xref_Sid
                                                     Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRN.Role_Category_Level_Sid
                                                     Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRN.RN_DD_Person_Type_Xref_Sid
                                                    Join r In context.RNs On r.RN_Sid Equals rn.RN_DDPersonnel_Sid
                                                    Join appHistory In context.Renewal_History_Count On appHistory.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid Where appHistory.Role_Category_Level_sid = role
                                                    Where r.RNLicense_Number = rnLicenseNumber And role1.Role_Category_Level_Sid = role And appHistory.Application_type_SId <> 2 And cert.Certification_Sid = certID Select cert.Certification_Sid).FirstOrDefault()
                            If (_count > 0) Then
                                certinfo = (From personSession In context.Person_Course_Session_Xref
                                            Join personCourse In context.Person_Course_Xref On personCourse.Person_Course_Xref_Sid Equals personSession.Person_Course_Xref_Sid
                                            Join course In context.Courses On course.Course_sid Equals personCourse.Course_Sid
                                            Join session In context.Sessions On session.Session_Sid Equals personSession.Session_Sid
                                            Join roleRn In context.Role_RN_DD_Personnel_Xref On personCourse.Role_RN_DD_Personnel_Xref_Sid Equals roleRn.Role_RN_DD_Personnel_Xref_Sid
                                            Join cert In context.Certifications On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRn.Role_RN_DD_Personnel_Xref_Sid
                                            Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRn.Role_Category_Level_Sid
                                            Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRn.RN_DD_Person_Type_Xref_Sid
                                            Join r In context.RNs On r.RN_Sid Equals rn.RN_DDPersonnel_Sid
                                            Join rSession In context.RNs On rSession.RN_Sid Equals course.RN_Sid
                                            Join appHistory In context.Renewal_History_Count On appHistory.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid Where appHistory.Role_Category_Level_sid = role
                                            Where r.RNLicense_Number = rnLicenseNumber And role1.Role_Category_Level_Sid = role And appHistory.Application_type_SId <> 2 And cert.Certification_Sid = certID
                                            Select New Objects.CertificationInfo() With {
                                                .StartDatesOfTraining = session.Start_Date,
                                                .EndDatesOfTraining = session.End_Date,
                                                .StartDatesOfCertification = cert.Certification_Start_Date,
                                                .EndDatesOfCertification = cert.Certification_End_Date,
                                                .TotalCEs = course.Total_CEs,
                                                .TotalACEs = course.Category_A_CEs,
                                                .LocationOfTraining = session.Location_Name,
                                .OBNNumber = course.OBN_Course_Number,
                                                .RNInstructorName = rSession.First_Name,
                                                .RNLastInstructorName = rSession.Last_Name,
                                                .RNMiddleInstructorName = rSession.Middle_Name,
                                                .SponsorName = session.Sponsor,
                                                .RNName = r.First_Name,
                                                .RNLastName = r.Last_Name,
                                                .RNMiddleName = r.Middle_Name
                                                }).ToList()
                            End If
                        End If
                    Else
                        If (applicationType <> "Renewal") Then
                            Dim _count As Integer = (From cert In context.Certifications
                                                         Join roleRN In context.Role_RN_DD_Personnel_Xref On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRN.Role_RN_DD_Personnel_Xref_Sid
                                                         Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRN.Role_Category_Level_Sid
                                                         Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRN.RN_DD_Person_Type_Xref_Sid
                                                        Join r In context.RNs On r.RN_Sid Equals rn.RN_DDPersonnel_Sid
                                                        Join appHistory In context.Renewal_History_Count On appHistory.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid Where appHistory.Role_Category_Level_sid = role
                                                        Where r.RNLicense_Number = rnLicenseNumber And role1.Role_Category_Level_Sid = role And appHistory.Application_type_SId <> 2 And cert.Certification_Sid = certID Select cert.Certification_Sid).FirstOrDefault()
                            If (_count > 0) Then
                                certinfo = (From roleRn In context.Role_RN_DD_Personnel_Xref
                                            Join cert In context.Certifications On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRn.Role_RN_DD_Personnel_Xref_Sid
                                            Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRn.Role_Category_Level_Sid
                                            Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRn.RN_DD_Person_Type_Xref_Sid
                                            Join r In context.RNs On r.RN_Sid Equals rn.RN_DDPersonnel_Sid
                                            Join appHistory In context.Renewal_History_Count On appHistory.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid Where appHistory.Role_Category_Level_sid = role
                                            Group Join rRN In context.RNs On rRN.RN_Sid Equals cert.Attestant Into appRN = Group
                                            From rnAppID In appRN.DefaultIfEmpty()
                                            Where r.RNLicense_Number = rnLicenseNumber And role1.Role_Category_Level_Sid = role And appHistory.Application_type_SId <> 2 And cert.Certification_Sid = certID
                                            Select New Objects.CertificationInfo() With {
                                                .StartDatesOfTraining = DateTime.Now,
                                                .EndDatesOfTraining = DateTime.Now,
                                                .StartDatesOfCertification = cert.Certification_Start_Date,
                                                .EndDatesOfCertification = cert.Certification_End_Date,
                                                .TotalCEs = 0.0,
                                                .TotalACEs = 0.0,
                                                .LocationOfTraining = String.Empty,
                                                .OBNNumber = String.Empty,
                                                .RNInstructorName = rnAppID.First_Name,
                                                .RNLastInstructorName = rnAppID.Last_Name,
                                                .RNMiddleInstructorName = rnAppID.Middle_Name,
                                                .SponsorName = String.Empty,
                                                .RNName = r.First_Name,
                                                .RNLastName = r.Last_Name,
                                .RNMiddleName = r.Middle_Name
                                                }).ToList()
                            End If
                        End If
                    End If
                    If (applicationType = "Renewal") Then
                        Dim _count As Integer = (From cert In context.Certifications
                                                 Join roleRN In context.Role_RN_DD_Personnel_Xref On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRN.Role_RN_DD_Personnel_Xref_Sid
                                                 Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRN.Role_Category_Level_Sid
                                                 Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRN.RN_DD_Person_Type_Xref_Sid
                                                Join r In context.RNs On r.RN_Sid Equals rn.RN_DDPersonnel_Sid
                                                Join appHistory In context.Renewal_History_Count On appHistory.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid Where appHistory.Role_Category_Level_sid = role
                                                Where r.RNLicense_Number = rnLicenseNumber And role1.Role_Category_Level_Sid = role And appHistory.Application_type_SId = 2 And cert.Certification_Sid = certID Select cert.Certification_Sid).FirstOrDefault()
                        If (_count > 0) Then
                            certinfo = (From roleRn In context.Role_RN_DD_Personnel_Xref
                                        Join cert In context.Certifications On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRn.Role_RN_DD_Personnel_Xref_Sid
                                        Join role1 In context.Role_Category_Level_Xref On role1.Role_Category_Level_Sid Equals roleRn.Role_Category_Level_Sid
                                        Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals roleRn.RN_DD_Person_Type_Xref_Sid
                                        Join r In context.RNs On r.RN_Sid Equals rn.RN_DDPersonnel_Sid
                                        Join appHistory In context.Renewal_History_Count On appHistory.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid Where appHistory.Role_Category_Level_sid = role
                                        Where r.RNLicense_Number = rnLicenseNumber And role1.Role_Category_Level_Sid = role And appHistory.Application_type_SId = 2 And cert.Certification_Sid = certID
                                        Select New Objects.CertificationInfo() With {
                                            .StartDatesOfCertification = cert.Certification_Start_Date,
                                            .EndDatesOfCertification = cert.Certification_End_Date,
                                            .RNName = r.First_Name,
                                            .RNLastName = r.Last_Name,
                                            .RNMiddleName = r.Middle_Name
                                            }).ToList()
                        End If
                    End If
                Catch ex As Exception
                    Me.LogError("Error Getting in certification Info in certification modification", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting in certification Info in certification modification", True, False))
                    Throw
                End Try
            End Using
            Return certinfo
        End Function
    End Class
End Namespace
