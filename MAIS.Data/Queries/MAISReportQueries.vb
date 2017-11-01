Imports ODMRDDHelperClassLibrary.Utility
Imports System.Configuration
Imports System.Data.Objects
Imports System.Data.Entity.Validation
Imports MAIS.Data.Objects


Namespace Queries
    Public Interface IMAISReportQueries
        Inherits IQueriesBase
        Function GetAllCourseByRNID(rn_Sid As Integer) As List(Of Course_Info)
        Function GetSessionsByCourseID(ByVal c_id As Integer) As List(Of Course_Info)
        Function GetDDPersonnelSkillsByUniqueID(ByVal uniqueID As String) As List(Of DDSkills_Info)
        Function GetAllCourses() As List(Of Course_Info)
        Function GetAllCoursesByUniqueID(ByVal uniqueID As String) As List(Of Course_Info)
        Function GetCertificationTypes() As List(Of MAIS_Role)
        Function GetCertificationStaus() As List(Of Certification_Status_Type)
        Function GetEmployersList(ByVal ID As Integer, ByVal empName As String, ByVal rblValue As String) As List(Of EmployerDetails)
        Function GetEmployersListByUniqueID(ByVal uniqueID As String) As List(Of EmployerDetails)
        Function GetSupervisorList(ByVal ID As Integer, ByVal FirstName As String, ByVal LastName As String, ByVal rblValue As String) As List(Of SupervisorDetails)
        Function GetSupervisorListByUniqueID(ByVal UniqueID As String) As List(Of SupervisorDetails)
        Function GetALLDDs() As List(Of USP_Get_DDPersonnel_MAIS_Report_Results_Result)
        Function GetALLRNs() As List(Of USP_Get_RN_MAIS_Report_Results_Result)
        Function GetDDNotations(ByVal notationTypeId As Integer, ByVal notationReasonId As Integer, ByVal dateFrom As Date, ByVal dateTo As Date) As List(Of DDPersonnelSearchResult)
        Function GetRNNotations(ByVal notationTypeId As Integer, ByVal notationReasonId As Integer, ByVal dateFrom As Date, ByVal dateTo As Date) As List(Of RNSearchResult)
        Function GetRNSearchReport(ByVal params As SearchParameters) As List(Of USP_Get_RN_MAIS_Report_Results_Result)
        Function GetDDSearchReport(ByVal params As SearchParameters) As List(Of USP_Get_DDPersonnel_MAIS_Report_Results_Result)
        Function GetCertificationHistory(ByVal uniqueID As String) As List(Of Cert_Info)
        Function GetDDRNCEUSRenewal(uniqueID As String) As List(Of CEUsDetailsObject)
    End Interface
    Public Class MAISReportQueries
        Inherits QueriesBase
        Implements IMAISReportQueries

        Public Function GetEmployersListByUniqueID(ByVal uniqueID As String) As List(Of EmployerDetails) Implements IMAISReportQueries.GetEmployersListByUniqueID
            Dim retList As New List(Of EmployerDetails)
            Dim retHistory As New List(Of EmployerDetails)
            Dim retCurrent As New List(Of EmployerDetails)
            Try
                Using context As New MAISContext
                    Dim rndd_Sid As Integer = 0
                    If (uniqueID.Contains("RN")) Then
                        rndd_Sid = (From rd In context.RN_DD_Person_Type_Xref
                                    Join rn In context.RNs On rn.RN_Sid Equals rd.RN_DDPersonnel_Sid
                                    Where rn.RNLicense_Number = uniqueID
                                    Select rd.RN_DD_Person_Type_Xref_Sid).FirstOrDefault()
                    Else
                        rndd_Sid = (From rd In context.RN_DD_Person_Type_Xref
                                   Join dd In context.DDPersonnels On dd.DDPersonnel_Sid Equals rd.RN_DDPersonnel_Sid
                                   Where dd.DDPersonnel_Code = uniqueID
                                   Select rd.RN_DD_Person_Type_Xref_Sid).FirstOrDefault()
                    End If

                    If (rndd_Sid > 0) Then
                        retHistory = (From eh In context.History_Employers
                           Join e In context.Employers On e.Employer_Sid Equals eh.Employer_Sid
                           Join pe In context.History_Employment On pe.History_Employers_SID Equals eh.History_Employers_SID
                           Join rn In context.RN_DD_Person_Type_Xref On pe.RN_DD_Person_Type_Xref_SID Equals rn.RN_DD_Person_Type_Xref_Sid
                           Group Join em In context.Email1 On em.Email_SID Equals eh.Email_SID Into emm = Group
                           From em1 In emm.DefaultIfEmpty()
                           Group Join ph In context.Phone_Number On ph.Phone_Number_SID Equals eh.Phone_SID Into phh = Group
                           From ph1 In phh.DefaultIfEmpty()
                           Order By e.Employer_Name
                           Where pe.RN_DD_Person_Type_Xref_SID = rndd_Sid
                           Select New Objects.EmployerDetails() With {
                               .CEOFirstName = eh.CEO_First_Name,
                               .CEOLastName = eh.CEO_Last_Name,
                               .EmailAddress = em1.Email_Address,
                               .EmpEndDate = eh.Employer_End_date,
                               .EmployerName = e.Employer_Name,
                               .EmpStartDate = eh.Employer_Start_Date,
                               .IdentificationValue = e.Identification_Value,
                               .PhoneNumber = ph1.Phone_Number1
                           }).Distinct.ToList()

                        If (retHistory.Count > 0) Then
                            retList.AddRange(retHistory)
                        End If

                        retCurrent = (From ep In context.Employer_RN_DD_Person_Type_Xref
                          Join ee In context.Employers On ee.Employer_Sid Equals ep.Employer_Sid
                          Group Join eep In context.Employer_RN_DD_Person_Type_Email_Xref On eep.RN_DD_Person_Type_Xref_Sid Equals ep.RN_DD_Person_Type_Xref_Sid And ee.Employer_Sid Equals eep.Employer_Sid Into empmail = Group
                          From eep1 In empmail.DefaultIfEmpty()
                          Group Join epp In context.Employer_RN_DD_Person_Type_Phone_Xref On epp.RN_DD_Person_Type_Xref_Sid Equals ep.RN_DD_Person_Type_Xref_Sid And ee.Employer_Sid Equals epp.Employer_Sid Into empphone = Group
                          From epp1 In empphone.DefaultIfEmpty()
                          Group Join emm In context.Email1 On emm.Email_SID Equals eep1.Email_Sid Into emmm = Group
                          From em1 In emmm.DefaultIfEmpty()
                          Group Join phh In context.Phone_Number On phh.Phone_Number_SID Equals epp1.Phone_Sid Into phhh = Group
                          From ph1 In phhh.DefaultIfEmpty()
                          Order By ee.Employer_Name
                          Where ep.RN_DD_Person_Type_Xref_Sid = rndd_Sid And epp1.Contact_Type_Sid = 4 And eep1.Contact_Type_Sid = 4
                          Select New Objects.EmployerDetails() With {
                              .CEOFirstName = ep.CEO_First_Name,
                              .CEOLastName = ep.CEO_Last_Name,
                              .EmailAddress = em1.Email_Address,
                              .EmpEndDate = ep.Employment_End_Date,
                              .EmployerName = ee.Employer_Name,
                              .EmpStartDate = ep.Employment_Start_Date,
                              .IdentificationValue = ee.Identification_Value,
                              .PhoneNumber = ph1.Phone_Number1
                          }).Distinct.ToList()
                        If (retCurrent.Count > 0) Then
                            retList.AddRange(retCurrent)
                        End If
                    End If
                End Using
            Catch ex As Exception
                Me.LogError("Error getting Employers List.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting Employers List.", True, False))
            End Try
            Return retList.Distinct.ToList()
        End Function
        Public Function GetEmployersList(ByVal ID As Integer, ByVal empName As String, ByVal rblValue As String) As List(Of EmployerDetails) Implements IMAISReportQueries.GetEmployersList
            Dim retList As New List(Of EmployerDetails)
            Dim retHist As New List(Of EmployerDetails)
            Dim retCurrent As New List(Of EmployerDetails)
            Try
                Using context As New MAISContext
                    Dim rndd_Sid As Integer = 0
                    If (ID > 0) Then
                        rndd_Sid = (From rd In context.RN_DD_Person_Type_Xref
                                    Where rd.RN_DDPersonnel_Sid = ID
                                    Select rd.RN_DD_Person_Type_Xref_Sid).FirstOrDefault()
                    End If
                    If ((rblValue = "1") OrElse (rblValue = "2")) Then

                        retHist = (From eh In context.History_Employers
                            Join e In context.Employers On e.Employer_Sid Equals eh.Employer_Sid
                            Join pe In context.History_Employment On pe.History_Employers_SID Equals eh.History_Employers_SID
                            Join rn In context.RN_DD_Person_Type_Xref On pe.RN_DD_Person_Type_Xref_SID Equals rn.RN_DD_Person_Type_Xref_Sid
                            Group Join em In context.Email1 On em.Email_SID Equals eh.Email_SID Into emm = Group
                            From em1 In emm.DefaultIfEmpty()
                            Group Join ph In context.Phone_Number On ph.Phone_Number_SID Equals eh.Phone_SID Into phh = Group
                            From ph1 In phh.DefaultIfEmpty()
                            Order By e.Employer_Name
                            Where If(empName.Equals(String.Empty), True, e.Employer_Name.Contains(empName)) AndAlso
                            If(rndd_Sid > 0, pe.RN_DD_Person_Type_Xref_SID = rndd_Sid, True) AndAlso
                            If(rblValue = "1", rn.Person_Type_Sid = 1, rn.Person_Type_Sid = 2)
                            Select New Objects.EmployerDetails() With {
                                .CEOFirstName = eh.CEO_First_Name,
                                .CEOLastName = eh.CEO_Last_Name,
                                .EmailAddress = em1.Email_Address,
                                .EmpEndDate = eh.Employer_End_date,
                                .EmployerName = e.Employer_Name,
                                .EmpStartDate = eh.Employer_Start_Date,
                                .IdentificationValue = e.Identification_Value,
                                .PhoneNumber = ph1.Phone_Number1
                            }).Distinct.ToList()

                        If (retHist.Count > 0) Then
                            retList.AddRange(retHist)
                        End If

                        retCurrent = (From ep In context.Employer_RN_DD_Person_Type_Xref
                          Join ee In context.Employers On ee.Employer_Sid Equals ep.Employer_Sid
                          Join rn In context.RN_DD_Person_Type_Xref On ep.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid
                          Group Join eep In context.Employer_RN_DD_Person_Type_Email_Xref On eep.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid And ee.Employer_Sid Equals eep.Employer_Sid Into empmail = Group
                          From eep1 In empmail.DefaultIfEmpty()
                          Group Join epp In context.Employer_RN_DD_Person_Type_Phone_Xref On epp.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid And ee.Employer_Sid Equals epp.Employer_Sid Into empphone = Group
                          From epp1 In empphone.DefaultIfEmpty()
                          Group Join emm In context.Email1 On emm.Email_SID Equals eep1.Email_Sid Into emmm = Group
                          From em1 In emmm.DefaultIfEmpty()
                          Group Join phh In context.Phone_Number On phh.Phone_Number_SID Equals epp1.Phone_Sid Into phhh = Group
                          From ph1 In phhh.DefaultIfEmpty()
                          Order By ee.Employer_Name
                          Where If(empName.Equals(String.Empty), True, ee.Employer_Name.Contains(empName)) AndAlso
                            If(rndd_Sid > 0, ep.RN_DD_Person_Type_Xref_Sid = rndd_Sid, True) AndAlso
                             If(rblValue = "1", rn.Person_Type_Sid = 1, rn.Person_Type_Sid = 2) AndAlso
                         epp1.Contact_Type_Sid = 4 AndAlso eep1.Contact_Type_Sid = 4
                          Select New Objects.EmployerDetails() With {
                              .CEOFirstName = ep.CEO_First_Name,
                              .CEOLastName = ep.CEO_Last_Name,
                              .EmailAddress = em1.Email_Address,
                              .EmpEndDate = ep.Employment_End_Date,
                              .EmployerName = ee.Employer_Name,
                              .EmpStartDate = ep.Employment_Start_Date,
                              .IdentificationValue = ee.Identification_Value,
                              .PhoneNumber = ph1.Phone_Number1
                          }).Distinct.ToList()
                        If (retCurrent.Count > 0) Then
                            retList.AddRange(retCurrent)
                        End If
                    ElseIf (rblValue = "3") Then
                        retHist = (From eh In context.History_Employers
                             Join e In context.Employers On e.Employer_Sid Equals eh.Employer_Sid
                             Group Join em In context.Email1 On em.Email_SID Equals eh.Email_SID Into emm = Group
                             From em1 In emm.DefaultIfEmpty()
                             Group Join ph In context.Phone_Number On ph.Phone_Number_SID Equals eh.Phone_SID Into phh = Group
                             From ph1 In phh.DefaultIfEmpty()
                             Order By e.Employer_Name
                             Select New Objects.EmployerDetails() With {
                                 .CEOFirstName = eh.CEO_First_Name,
                                 .CEOLastName = eh.CEO_Last_Name,
                                 .EmailAddress = em1.Email_Address,
                                 .EmpEndDate = eh.Employer_End_date,
                                 .EmployerName = e.Employer_Name,
                                 .EmpStartDate = eh.Employer_Start_Date,
                                 .IdentificationValue = e.Identification_Value,
                                 .PhoneNumber = ph1.Phone_Number1
                             }).Distinct.ToList()
                        If (retHist.Count > 0) Then
                            retList.AddRange(retHist)
                        End If

                        retCurrent = (From ep In context.Employer_RN_DD_Person_Type_Xref
                          Join ee In context.Employers On ee.Employer_Sid Equals ep.Employer_Sid
                          Join rn In context.RN_DD_Person_Type_Xref On ep.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid
                          Group Join eep In context.Employer_RN_DD_Person_Type_Email_Xref On eep.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid And ee.Employer_Sid Equals eep.Employer_Sid Into empmail = Group
                          From eep1 In empmail.DefaultIfEmpty()
                          Group Join epp In context.Employer_RN_DD_Person_Type_Phone_Xref On epp.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid And ee.Employer_Sid Equals epp.Employer_Sid Into empphone = Group
                          From epp1 In empphone.DefaultIfEmpty()
                          Group Join emm In context.Email1 On emm.Email_SID Equals eep1.Email_Sid Into emmm = Group
                          From em1 In emmm.DefaultIfEmpty()
                          Group Join phh In context.Phone_Number On phh.Phone_Number_SID Equals epp1.Phone_Sid Into phhh = Group
                          From ph1 In phhh.DefaultIfEmpty()
                          Order By ee.Employer_Name
                          Where epp1.Contact_Type_Sid = 4 AndAlso eep1.Contact_Type_Sid = 4
                          Select New Objects.EmployerDetails() With {
                              .CEOFirstName = ep.CEO_First_Name,
                              .CEOLastName = ep.CEO_Last_Name,
                              .EmailAddress = em1.Email_Address,
                              .EmpEndDate = ep.Employment_End_Date,
                              .EmployerName = ee.Employer_Name,
                              .EmpStartDate = ep.Employment_Start_Date,
                              .IdentificationValue = ee.Identification_Value,
                              .PhoneNumber = ph1.Phone_Number1
                          }).Distinct.ToList()
                        If (retCurrent.Count > 0) Then
                            retList.AddRange(retCurrent)
                        End If
                    End If

                End Using
            Catch ex As Exception
                Me.LogError("Error getting Employers List.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting Employers List.", True, False))
            End Try
            Return retList.Distinct.ToList()
        End Function
        Public Function GetCertificationStaus() As List(Of Certification_Status_Type) Implements IMAISReportQueries.GetCertificationStaus
            Dim retObj As New List(Of Certification_Status_Type)
            Try
                Using context As New MAISContext
                    retObj = (From c In context.Certification_Status_Type
                              Where c.End_Date >= Today
                              Select c).ToList()
                End Using
            Catch ex As Exception
                Me.LogError("Error getting Certification_Status_Type.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting Certification_Status_Type.", True, False))
            End Try
            Return retObj
        End Function
        Public Function GetCertificationTypes() As List(Of MAIS_Role) Implements IMAISReportQueries.GetCertificationTypes
            Dim retObj As New List(Of MAIS_Role)
            Try
                Using context As New MAISContext
                    retObj = (From mr In context.MAIS_Role
                              Where mr.End_Date >= Today
                              Select mr).ToList()
                End Using
            Catch ex As Exception
                Me.LogError("Error getting MAIS_Role.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting MAIS_Role.", True, False))
            End Try
            Return retObj
        End Function

        Public Function GetSupervisorList(ByVal ID As Integer, ByVal FirstName As String, ByVal LastName As String, ByVal rblValue As String) As List(Of SupervisorDetails) Implements IMAISReportQueries.GetSupervisorList
            Dim retList As New List(Of SupervisorDetails)
            Dim retHist As New List(Of SupervisorDetails)
            Dim retCurrent As New List(Of SupervisorDetails)
            Try
                Using context As New MAISContext
                    Dim rndd_Sid As Integer = 0
                    If (ID > 0) Then
                        rndd_Sid = (From rd In context.RN_DD_Person_Type_Xref
                                    Where rd.RN_DDPersonnel_Sid = ID
                                    Select rd.RN_DD_Person_Type_Xref_Sid).FirstOrDefault()
                    End If
                    If ((rblValue = "1") OrElse (rblValue = "2")) Then
                        retHist = (From ss In context.History_Supervisor
                                    Join pe In context.History_Employment On pe.History_Supervisor_SID Equals ss.History_Supervisor_SID
                                    Join rn In context.RN_DD_Person_Type_Xref On pe.RN_DD_Person_Type_Xref_SID Equals rn.RN_DD_Person_Type_Xref_Sid
                                    Group Join em In context.Email1 On em.Email_SID Equals ss.Email_SID Into emm = Group
                                    From em1 In emm.DefaultIfEmpty()
                                    Group Join ph In context.Phone_Number On ph.Phone_Number_SID Equals ss.Phone_SID Into phh = Group
                                    From ph1 In phh.DefaultIfEmpty()
                                    Order By ss.Last_Name
                                    Where If(FirstName.Equals(String.Empty), True, ss.First_Name.Contains(FirstName)) AndAlso
                                         If(LastName.Equals(String.Empty), True, ss.Last_Name.Contains(LastName)) AndAlso
                                         If(rndd_Sid > 0, pe.RN_DD_Person_Type_Xref_SID = rndd_Sid, True) AndAlso
                                        If(rblValue = "1", rn.Person_Type_Sid = 1, rn.Person_Type_Sid = 2)
                                    Select New Objects.SupervisorDetails() With {
                                        .supFirstName = ss.First_Name,
                                        .supLastName = ss.Last_Name,
                                        .EmailAddress = em1.Email_Address,
                                        .supEndDate = ss.Supervisor_End_Date,
                                        .supStartDate = ss.Supervisor_Start_Date,
                                        .PhoneNumber = ph1.Phone_Number1
                                    }).Distinct.ToList()

                        If (retHist.Count > 0) Then
                            retList.AddRange(retHist)
                        End If

                        retCurrent = (From ep In context.Employer_RN_DD_Person_Type_Xref
                                       Join ee In context.Employers On ee.Employer_Sid Equals ep.Employer_Sid
                                         Join rn In context.RN_DD_Person_Type_Xref On ep.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid
                                         Group Join eep In context.Employer_RN_DD_Person_Type_Email_Xref On eep.RN_DD_Person_Type_Xref_Sid Equals ep.RN_DD_Person_Type_Xref_Sid And ee.Employer_Sid Equals eep.Employer_Sid Into supemail = Group
                                         From eep1 In supemail.DefaultIfEmpty()
                                        Group Join epp In context.Employer_RN_DD_Person_Type_Phone_Xref On epp.RN_DD_Person_Type_Xref_Sid Equals ep.RN_DD_Person_Type_Xref_Sid And ee.Employer_Sid Equals epp.Employer_Sid Into supphone = Group
                                        From epp1 In supphone.DefaultIfEmpty()
                                         Group Join emm In context.Email1 On emm.Email_SID Equals eep1.Email_Sid Into emmm = Group
                                         From em1 In emmm.DefaultIfEmpty()
                                         Group Join phh In context.Phone_Number On phh.Phone_Number_SID Equals epp1.Phone_Sid Into phhh = Group
                                         From ph1 In phhh.DefaultIfEmpty()
                                         Order By ep.Supervisor_Last_Name
                                         Where If(FirstName.Equals(String.Empty), True, ep.Supervisor_First_Name.Contains(FirstName)) AndAlso
                                         If(LastName.Equals(String.Empty), True, ep.Supervisor_Last_Name.Contains(LastName)) AndAlso
                                         If(rndd_Sid > 0, ep.RN_DD_Person_Type_Xref_Sid = rndd_Sid, True) AndAlso
                                        If(rblValue = "1", rn.Person_Type_Sid = 1, rn.Person_Type_Sid = 2) AndAlso
                                         eep1.Contact_Type_Sid = 7 AndAlso epp1.Contact_Type_Sid = 7
                                        Select New Objects.SupervisorDetails() With {
                                                       .supFirstName = ep.Supervisor_First_Name,
                                                       .supLastName = ep.Supervisor_Last_Name,
                                                       .EmailAddress = em1.Email_Address,
                                                       .supEndDate = ep.Supervisor_End_date,
                                                       .supStartDate = ep.Supervisor_Start_date,
                                                       .PhoneNumber = ph1.Phone_Number1
                                                   }).Distinct.ToList()

                        If (retCurrent.Count > 0) Then
                            retList.AddRange(retCurrent)
                        End If

                    ElseIf (rblValue = "3") Then
                        retHist = (From ss In context.History_Supervisor
                             Group Join em In context.Email1 On em.Email_SID Equals ss.Email_SID Into emm = Group
                             From em1 In emm.DefaultIfEmpty()
                             Group Join ph In context.Phone_Number On ph.Phone_Number_SID Equals ss.Phone_SID Into phh = Group
                             From ph1 In phh.DefaultIfEmpty()
                              Order By ss.Last_Name
                             Select New Objects.SupervisorDetails() With {
                                 .supFirstName = ss.First_Name,
                                 .supLastName = ss.Last_Name,
                                 .EmailAddress = em1.Email_Address,
                                 .supEndDate = ss.Supervisor_End_Date,
                                 .supStartDate = ss.Supervisor_Start_Date,
                                 .PhoneNumber = ph1.Phone_Number1
                             }).Distinct.ToList()
                        If (retHist.Count > 0) Then
                            retList.AddRange(retHist)
                        End If

                        retCurrent = (From ep In context.Employer_RN_DD_Person_Type_Xref
                                        Join ee In context.Employers On ee.Employer_Sid Equals ep.Employer_Sid
                                         Group Join eep In context.Employer_RN_DD_Person_Type_Email_Xref On eep.RN_DD_Person_Type_Xref_Sid Equals ep.RN_DD_Person_Type_Xref_Sid And ee.Employer_Sid Equals eep.Employer_Sid Into supemail = Group
                                         From eep1 In supemail.DefaultIfEmpty()
                                         Group Join epp In context.Employer_RN_DD_Person_Type_Phone_Xref On epp.RN_DD_Person_Type_Xref_Sid Equals ep.RN_DD_Person_Type_Xref_Sid And ee.Employer_Sid Equals epp.Employer_Sid Into supphone = Group
                                         From epp1 In supphone.DefaultIfEmpty()
                                         Group Join emm In context.Email1 On emm.Email_SID Equals eep1.Email_Sid Into emmm = Group
                                         From em1 In emmm.DefaultIfEmpty()
                                         Group Join phh In context.Phone_Number On phh.Phone_Number_SID Equals epp1.Phone_Sid Into phhh = Group
                                         From ph1 In phhh.DefaultIfEmpty()
                                         Order By ep.Supervisor_Last_Name
                                         Where eep1.Contact_Type_Sid = 7 AndAlso epp1.Contact_Type_Sid = 7
                                        Select New Objects.SupervisorDetails() With {
                                                       .supFirstName = ep.Supervisor_First_Name,
                                                       .supLastName = ep.Supervisor_Last_Name,
                                                       .EmailAddress = em1.Email_Address,
                                                       .supEndDate = ep.Supervisor_End_date,
                                                       .supStartDate = ep.Supervisor_Start_date,
                                                       .PhoneNumber = ph1.Phone_Number1
                                                   }).Distinct.ToList()

                        If (retCurrent.Count > 0) Then
                            retList.AddRange(retCurrent)
                        End If
                    End If


                End Using
            Catch ex As Exception
                Me.LogError("Error getting Supervisor list.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting Supervisor list.", True, False))
            End Try
            Return retList
        End Function

        Public Function GetDDNotations(notationTypeId As Integer, notationReasonId As Integer, dateFrom As Date, dateTo As Date) As List(Of DDPersonnelSearchResult) Implements IMAISReportQueries.GetDDNotations
            Dim retList As New List(Of DDPersonnelSearchResult)
            Try
                Using context As New MAISContext
                    If ((notationTypeId > 0) OrElse (notationReasonId > 0) OrElse (dateFrom <> "12/31/9999") OrElse (dateTo <> "12/31/9999")) Then
                        Dim dd = (From n In context.Notations
                                  Join nr In context.Notation_Reason_RN_DD_Xref On n.Notation_Sid Equals nr.Notation_Sid
                                  Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals n.RN_DD_Person_Type_Xref_Sid
                                  Where If(notationTypeId = 0, True, n.Notation_Type_Sid = notationTypeId) AndAlso _
                                        If(notationReasonId = 0, True, nr.Reasons_For_Notation_Sid = notationReasonId) AndAlso _
                                        If(dateFrom = "12/31/9999", True, n.Occurrence_Date >= dateFrom) AndAlso _
                                        If(dateTo = "12/31/9999", True, n.Occurrence_Date <= dateTo) AndAlso _
                                        rn.Person_Type_Sid = 2
                                        Select n.RN_DD_Person_Type_Xref_Sid).Distinct.ToList()
                        If (dd.Count > 0) Then
                            For Each d In dd
                                Dim ddDetail As New DDPersonnelSearchResult
                                ddDetail.RN_DD_Person_Type_Sid = d
                                Dim ddper = (From rn In context.RN_DD_Person_Type_Xref
                                             Join ddp In context.DDPersonnels On ddp.DDPersonnel_Sid Equals rn.RN_DDPersonnel_Sid
                                             Where rn.RN_DD_Person_Type_Xref_Sid = d
                                             Select ddp).FirstOrDefault()
                                Dim roleList = (From rol In context.Role_RN_DD_Personnel_Xref
                                                Join rlc In context.Role_Category_Level_Xref On rol.Role_Category_Level_Sid Equals rlc.Role_Category_Level_Sid
                                                Join rrr In context.MAIS_Role On rrr.Role_Sid Equals rlc.Role_Sid
                                                  Where rol.RN_DD_Person_Type_Xref_Sid = d
                                                   Select rol.Role_Category_Level_Sid, rrr.Role_Desc, rol.Role_End_Date).ToList()
                                Dim addr = (From aa In context.RN_DD_Person_Type_Address_Xref
                                            Where aa.RN_DD_Person_Type_Xref_Sid = d And aa.Active_Flg = True
                                            Select aa.Address_Sid).FirstOrDefault()
                                If (addr > 0) Then
                                    Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", addr)
                                    Dim address11 As Address_Lookup_And_Insert_Result = context.Address_Lookup_And_Insert(parameter, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).FirstOrDefault()
                                    ddDetail.Address1 = address11.Address_Line1
                                    ddDetail.Address2 = address11.Address_Line2
                                    ddDetail.City = address11.City
                                    ddDetail.County = address11.County_Desc
                                    ddDetail.State = address11.State_Abbr
                                    If address11.Zip.Length > 5 Then
                                        ddDetail.Zip = address11.Zip.Substring(0, 5)
                                        ddDetail.ZipPlus = address11.Zip.Substring(5, address11.Zip.Length - 5)
                                    Else
                                        ddDetail.Zip = address11.Zip.Substring(0, 5)
                                        ddDetail.ZipPlus = String.Empty
                                    End If
                                End If
                                If (ddper IsNot Nothing) Then
                                    ddDetail.DateOfBirth = ddper.DOB
                                    ddDetail.DDPersonnelCode = ddper.DDPersonnel_Code
                                    ddDetail.FirstName = ddper.First_Name
                                    ddDetail.Last4SSN = ddper.SSN
                                    ddDetail.LastName = ddper.Last_Name
                                    ddDetail.MiddleName = ddper.Middle_Name
                                End If
                                If (roleList.Count > 0) Then
                                    For Each ro In roleList
                                        If (ro.Role_Category_Level_Sid = 15) Then 'cat1
                                            ddDetail.Cat1 = ro.Role_Desc + " " + ro.Role_End_Date
                                        ElseIf (ro.Role_Category_Level_Sid = 16) Then 'cat2
                                            ddDetail.Cat2 = ro.Role_Desc + " " + ro.Role_End_Date
                                        ElseIf (ro.Role_Category_Level_Sid = 17) Then 'cat3                                      
                                            ddDetail.Cat3 = ro.Role_Desc + " " + ro.Role_End_Date
                                        End If
                                    Next
                                End If
                                retList.Add(ddDetail)
                            Next
                        End If
                    End If
                End Using
            Catch ex As Exception
                Me.LogError("Error getting ddpersonnel notation list.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting ddpersonnel notation list.", True, False))
            End Try
            Return retList
        End Function

        Public Function GetALLDDs() As List(Of USP_Get_DDPersonnel_MAIS_Report_Results_Result) Implements IMAISReportQueries.GetALLDDs
            Dim retList As New List(Of USP_Get_DDPersonnel_MAIS_Report_Results_Result)
            Try
                Using context As New MAISContext
                    Dim testRes As List(Of USP_Get_DDPersonnel_MAIS_Report_Results_Result) = context.USP_Get_DDPersonnel_MAIS_Report_Results(String.Empty, 0, String.Empty, String.Empty, String.Empty,
                                                                                                                           String.Empty, String.Empty, String.Empty, String.Empty,
                                                                                                                           0, 0, 0,
                                                                                                                           0, 0, Nothing,
                                                                                                                           Nothing, False, False,
                                                                                                                           False, False, 0, True).Distinct.ToList()

                    If (testRes.Count > 0) Then
                        retList = testRes
                    End If

                End Using
            Catch ex As Exception
                Me.LogError("Error getting  ALL DDpersonnels", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting All DDpersonnels", True, False))
            End Try
            Return retList
        End Function

        Public Function GetALLRNs() As List(Of USP_Get_RN_MAIS_Report_Results_Result) Implements IMAISReportQueries.GetALLRNs
            Dim retList As New List(Of USP_Get_RN_MAIS_Report_Results_Result)
            Try
                Using context As New MAISContext
                    Dim testRes As List(Of USP_Get_RN_MAIS_Report_Results_Result) = context.USP_Get_RN_MAIS_Report_Results(String.Empty, String.Empty, String.Empty, String.Empty,
                                                                                                                           String.Empty, String.Empty, String.Empty, String.Empty,
                                                                                                                           0, 0, 0,
                                                                                                                           0, 0, Nothing,
                                                                                                                           Nothing, False, False,
                                                                                                                           False, False, 0).Distinct.ToList()

                    If (testRes.Count > 0) Then
                        retList = testRes
                    End If

                End Using
            Catch ex As Exception
                Me.LogError("Error getting  ALL RN's", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting All RN's", True, False))
            End Try
            Return retList
        End Function

        Public Function GetRNSearchReport(ByVal params As SearchParameters) As List(Of USP_Get_RN_MAIS_Report_Results_Result) Implements IMAISReportQueries.GetRNSearchReport
            Dim retObj As New List(Of USP_Get_RN_MAIS_Report_Results_Result)
            Try
                If params IsNot Nothing Then
                    Using context As New MAISContext
                        Dim rnLicenseNumber As String = String.Empty
                        Dim fn As String = String.Empty

                        Dim testRes As List(Of USP_Get_RN_MAIS_Report_Results_Result) = context.USP_Get_RN_MAIS_Report_Results(params.Licence_Code, params.FirstName, params.LastName, params.SupFirstName,
                                                                                                                               params.SupLastName, params.EmployerName, params.CEOFirstName, params.CEOLastName,
                                                                                                                               params.Role_Level_Cat_Sid, params.Cert_Status_Type_Sid, params.Course_Sid, params.Session_sid,
                                                                                                                               params.Trainer_RN_Sid, params.ExpDateFrom, params.ExpDateTo, params.ExpWithinLast30Days,
                                                                                                                               params.ExpWithinLast60Days, params.ExpWithinLast90Days, params.ExpWithinLast180Days, params.workAddr_Sid
                                                                                                                               ).Distinct.ToList()
                        If testRes.Count > 0 Then
                            retObj = testRes
                        End If

                    End Using
                End If

            Catch ex As Exception
                Me.LogError("Error getting RN's report list.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting RN's report list.", True, False))
            End Try
            Return retObj
        End Function

        Public Function GetAllCourses() As List(Of Course_Info) Implements IMAISReportQueries.GetAllCourses
            Dim cou_list As New List(Of Course_Info)
            Try
                Using context As New MAISContext
                    cou_list = (From c In context.Courses
                                Join r In context.RNs On c.RN_Sid Equals r.RN_Sid
                                Select New Objects.Course_Info With {
                                    .Course_Number = c.OBN_Course_Number,
                                    .Course_Sid = c.Course_sid,
                                    .Trainer_Name_FN = r.First_Name,
                                    .Trainer_Name_LN = r.Last_Name
                                    }).ToList()
                End Using
            Catch ex As Exception
                Me.LogError("Error getting course list.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting course list.", True, False))
            End Try
            Return cou_list
        End Function

        Public Function GetSessionsByCourseID(c_id As Integer) As List(Of Course_Info) Implements IMAISReportQueries.GetSessionsByCourseID
            Dim ses_lst As New List(Of Course_Info)
            Try

                Using context As New MAISContext
                    ses_lst = (From c In context.Sessions
                               Where c.Course_Sid = c_id
                               Select New Objects.Course_Info With {
                                   .Session_Sid = c.Session_Sid,
                                   .Course_Sid = c.Course_Sid,
                                   .Session_CEUs = c.Total_CEs,
                                   .Session_Start_Date = c.Start_Date,
                                   .Session_End_Date = c.End_Date
                                   }).ToList()
                End Using

            Catch ex As Exception
                Me.LogError("Error getting session by course id.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting session by course id.", True, False))
            End Try
            Return ses_lst
        End Function

        Public Function GetAllCourseByRNID(rn_Sid As Integer) As List(Of Course_Info) Implements IMAISReportQueries.GetAllCourseByRNID
            Dim courseLst As New List(Of Course_Info)
            Try
                Using context As New MAISContext
                    courseLst = (From c In context.Courses
                                    Join r In context.RNs On c.RN_Sid Equals r.RN_Sid
                                    Where c.RN_Sid = rn_Sid
                                    Select New Objects.Course_Info With {
                                        .Course_Number = c.OBN_Course_Number,
                                        .Course_Sid = c.Course_sid,
                                        .Trainer_Name_FN = r.First_Name,
                                        .Trainer_Name_LN = r.Last_Name
                                        }).ToList()
                End Using
            Catch ex As Exception
                Me.LogError("Error getting course list by rn_sid.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting course list by rn_sid.", True, False))
            End Try
            Return courseLst
        End Function

        Public Function GetDDSearchReport(params As SearchParameters) As List(Of USP_Get_DDPersonnel_MAIS_Report_Results_Result) Implements IMAISReportQueries.GetDDSearchReport
            Dim retObj As New List(Of USP_Get_DDPersonnel_MAIS_Report_Results_Result)
            Try
                If params IsNot Nothing Then
                    Using context As New MAISContext
                        Dim rnLicenseNumber As String = String.Empty
                        Dim fn As String = String.Empty

                        Dim testRes As List(Of USP_Get_DDPersonnel_MAIS_Report_Results_Result) = context.USP_Get_DDPersonnel_MAIS_Report_Results(params.Licence_Code, params.Last4SSN, params.FirstName, params.LastName, params.SupFirstName,
                                                                                                                               params.SupLastName, params.EmployerName, params.CEOFirstName, params.CEOLastName,
                                                                                                                               params.Role_Level_Cat_Sid, params.Cert_Status_Type_Sid, params.Course_Sid, params.Session_sid,
                                                                                                                               params.Trainer_RN_Sid, params.ExpDateFrom, params.ExpDateTo, params.ExpWithinLast30Days,
                                                                                                                               params.ExpWithinLast60Days, params.ExpWithinLast90Days, params.ExpWithinLast180Days, params.workAddr_Sid, params.AdminFlg_GetAllRecords
                                                                                                                               ).Distinct.ToList()
                        If testRes.Count > 0 Then
                            retObj = testRes
                        End If

                    End Using
                End If

            Catch ex As Exception
                Me.LogError("Error getting DDpersonnel report list.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting DDpersonnel report list.", True, False))
            End Try
            Return retObj
        End Function

        Public Function GetRNNotations(notationTypeId As Integer, notationReasonId As Integer, dateFrom As Date, dateTo As Date) As List(Of RNSearchResult) Implements IMAISReportQueries.GetRNNotations
            Dim retList As New List(Of RNSearchResult)
            Try
                Using context As New MAISContext
                    If ((notationTypeId > 0) OrElse (notationReasonId > 0) OrElse (dateFrom <> "12/31/9999") OrElse (dateTo <> "12/31/9999")) Then
                        Dim rr = (From n In context.Notations
                                  Join nr In context.Notation_Reason_RN_DD_Xref On n.Notation_Sid Equals nr.Notation_Sid
                                  Join rn In context.RN_DD_Person_Type_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals n.RN_DD_Person_Type_Xref_Sid
                                  Where If(notationTypeId = 0, True, n.Notation_Type_Sid = notationTypeId) AndAlso _
                                        If(notationReasonId = 0, True, nr.Reasons_For_Notation_Sid = notationReasonId) AndAlso _
                                        If(dateFrom = "12/31/9999", True, n.Occurrence_Date >= dateFrom) AndAlso _
                                        If(dateTo = "12/31/9999", True, n.Occurrence_Date <= dateTo) AndAlso _
                                          rn.Person_Type_Sid = 1
                                        Select n.RN_DD_Person_Type_Xref_Sid).Distinct.ToList()
                        If (rr.Count > 0) Then
                            For Each r In rr
                                Dim rnDetail As New RNSearchResult
                                rnDetail.RN_DD_Person_Type_Sid = r
                                Dim rninfo = (From rn In context.RN_DD_Person_Type_Xref
                                             Join ddp In context.RNs On ddp.RN_Sid Equals rn.RN_DDPersonnel_Sid
                                             Where rn.RN_DD_Person_Type_Xref_Sid = r
                                             Select ddp).FirstOrDefault()
                                Dim roleList = (From rol In context.Role_RN_DD_Personnel_Xref
                                                Join rlc In context.Role_Category_Level_Xref On rol.Role_Category_Level_Sid Equals rlc.Role_Category_Level_Sid
                                                Join rrr In context.MAIS_Role On rrr.Role_Sid Equals rlc.Role_Sid
                                                  Where rol.RN_DD_Person_Type_Xref_Sid = r
                                                   Select rol.Role_Category_Level_Sid, rrr.Role_Desc, rol.Role_End_Date).ToList()
                                Dim addr = (From aa In context.RN_DD_Person_Type_Address_Xref
                                            Where aa.RN_DD_Person_Type_Xref_Sid = r And aa.Active_Flg = True
                                            Select aa.Address_Sid).FirstOrDefault()
                                If (addr > 0) Then
                                    Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", addr)
                                    Dim address11 As Address_Lookup_And_Insert_Result = context.Address_Lookup_And_Insert(parameter, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).FirstOrDefault()
                                    rnDetail.Address1 = address11.Address_Line1
                                    rnDetail.Address2 = address11.Address_Line2
                                    rnDetail.City = address11.City
                                    rnDetail.County = address11.County_Desc
                                    rnDetail.State = address11.State_Abbr
                                    If address11.Zip.Length > 5 Then
                                        rnDetail.Zip = address11.Zip.Substring(0, 5)
                                        rnDetail.ZipPlus = address11.Zip.Substring(5, address11.Zip.Length - 5)
                                    Else
                                        rnDetail.Zip = address11.Zip.Substring(0, 5)
                                        rnDetail.ZipPlus = String.Empty
                                    End If
                                End If
                                If (rninfo IsNot Nothing) Then
                                    rnDetail.DateRNIssuance = rninfo.Date_Of_Original_Issuance
                                    rnDetail.RNLicenseNumber = rninfo.RNLicense_Number
                                    rnDetail.FirstName = rninfo.First_Name
                                    rnDetail.LastName = rninfo.Last_Name
                                    rnDetail.MiddleName = rninfo.Middle_Name
                                End If
                                If (roleList.Count > 0) Then
                                    For Each ro In roleList
                                        If (ro.Role_Category_Level_Sid = 4) Then 'RN Trainer
                                            rnDetail.RN_Trainer = ro.Role_Desc + " " + ro.Role_End_Date
                                        ElseIf (ro.Role_Category_Level_Sid = 5) Then 'RN Instructor
                                            rnDetail.RN_Instructor = ro.Role_Desc + " " + ro.Role_End_Date
                                        ElseIf (ro.Role_Category_Level_Sid = 6) Then 'RN Master                                    
                                            rnDetail.RN_Master = ro.Role_Desc + " " + ro.Role_End_Date
                                        ElseIf (ro.Role_Category_Level_Sid = 7) Then 'RN QA                                    
                                            rnDetail.RN_QA = ro.Role_Desc + " " + ro.Role_End_Date
                                        ElseIf (ro.Role_Category_Level_Sid = 8) Then 'RN 17 Bed                                    
                                            rnDetail.RN_17Bed = ro.Role_Desc + " " + ro.Role_End_Date
                                        End If
                                    Next
                                End If
                                retList.Add(rnDetail)
                            Next
                        End If
                    End If
                End Using
            Catch ex As Exception
                Me.LogError("Error getting RN's notation list.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting RN's notation list.", True, False))
            End Try
            Return retList
        End Function

        Public Function GetCertificationHistory(uniqueID As String) As List(Of Cert_Info) Implements IMAISReportQueries.GetCertificationHistory
            Dim retList As New List(Of Cert_Info)
            Try
                Using context As New MAISContext
                    Dim RN_DD_Sid As Integer
                    If uniqueID.Contains("RN") Then
                        RN_DD_Sid = (From r In context.RNs
                                     Join rd In context.RN_DD_Person_Type_Xref On r.RN_Sid Equals rd.RN_DDPersonnel_Sid
                                     Where r.RNLicense_Number = uniqueID
                                     Select rd.RN_DD_Person_Type_Xref_Sid).FirstOrDefault()
                    Else
                        RN_DD_Sid = (From d In context.DDPersonnels
                                   Join rd In context.RN_DD_Person_Type_Xref On d.DDPersonnel_Sid Equals rd.RN_DDPersonnel_Sid
                                   Where d.DDPersonnel_Code = uniqueID
                                   Select rd.RN_DD_Person_Type_Xref_Sid).FirstOrDefault()
                    End If
                    If RN_DD_Sid > 0 Then
                        retList = (From rol In context.Role_RN_DD_Personnel_Xref
                                    Join c In context.Certifications On c.Role_RN_DD_Personnel_Xref_Sid Equals rol.Role_RN_DD_Personnel_Xref_Sid
                                    Join cs In context.Certification_Status On c.Certification_Sid Equals cs.Certification_Sid
                                    Join cst In context.Certification_Status_Type On cs.Certification_Status_Type_Sid Equals cst.Certification_Status_Type_Sid
                                    Join rcl In context.Role_Category_Level_Xref On rol.Role_Category_Level_Sid Equals rcl.Role_Category_Level_Sid
                                    Join mr In context.MAIS_Role On rcl.Role_Sid Equals mr.Role_Sid
                                    Join cat In context.Category_Type On rcl.Category_Type_Sid Equals cat.Category_Type_Sid
                                 Group Join rrn In context.RNs On rrn.RN_Sid Equals c.Attestant Into rnnn = Group
                                 From rrn1 In rnnn.DefaultIfEmpty()
                                Group Join rcount In context.Renewal_History_Count On rcount.RN_DD_Person_Type_Xref_Sid Equals rol.RN_DD_Person_Type_Xref_Sid Into rc = Group
                                From rc1 In rc.DefaultIfEmpty()
                                    Where rol.RN_DD_Person_Type_Xref_Sid = RN_DD_Sid And
                                        cs.Status_Start_Date = (From csRWC In context.Certification_Status
                                                    Where csRWC.Certification_Sid = c.Certification_Sid
                                                    Group By csRWC.Certification_Sid Into Latestsid = Group
                                                    Select Latestsid.Max(Function(l) l.Status_Start_Date)).FirstOrDefault
                                    Select New Objects.Cert_Info With {
                                            .Certification_Sid = c.Certification_Sid,
                                            .Attestant_Sid = If(c.Attestant > 0, c.Attestant, 0),
                                            .Certification_Start_Date = c.Certification_Start_Date,
                                            .Certification_End_Date = c.Certification_End_Date,
                                            .Attestant_Name_LN = rrn1.Last_Name,
                                            .Attestant_Name_FN = rrn1.First_Name,
                                            .Certification_Type = mr.Role_Desc,
                                            .Certification_Status = cst.Certification_Status_Desc,
                                            .Category_Code = cat.Category_Desc,
                                           .RenewalCount = If(rc1.Renewal_Count > 0, rc1.Renewal_Count, 0)
                                        }).ToList()
                    End If
                End Using

            Catch ex As Exception
                Me.LogError("Error getting certification list.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting certification list.", True, False))
            End Try
            Return retList
        End Function
        Public Function GetDDPersonnelSkillsByUniqueID(ByVal uniqueID As String) As List(Of DDSkills_Info) Implements IMAISReportQueries.GetDDPersonnelSkillsByUniqueID
            Dim skill_list As New List(Of DDSkills_Info)
            Try
                Using context As New MAISContext
                    Dim rndd_Sid As Integer = 0
                    If (uniqueID.Contains("RN")) Then
                        Return skill_list
                    Else
                        rndd_Sid = (From rd In context.RN_DD_Person_Type_Xref
                                   Join dd In context.DDPersonnels On dd.DDPersonnel_Sid Equals rd.RN_DDPersonnel_Sid
                                   Where dd.DDPersonnel_Code = uniqueID
                                   Select rd.RN_DD_Person_Type_Xref_Sid).FirstOrDefault()

                        If (rndd_Sid > 0) Then
                            skill_list = (From sv In context.Skill_Verification
                                          Join svs In context.Skill_Verification_Skill_Type_Xref On sv.Skill_Verification_Sid Equals svs.Skill_Verification_Sid
                                          Join svc In context.Skill_Verification_Type_CheckList_Xref On svs.Skill_Verification_Skill_Type_Xref_Sid Equals svc.Skill_Verification_Skill_Type_Xref_Sid
                                          Join ct In context.Category_Type On ct.Category_Type_Sid Equals sv.Category_Type_Sid
                                          Join st In context.Skill_Type On st.Skill_Type_Sid Equals svs.Skill_Type_Sid
                                          Join sc In context.Skill_CheckList On sc.Skill_CheckList_Sid Equals svc.Skill_CheckList_Sid
                                          Where sv.RN_DD_Person_Type_Xref_Sid = rndd_Sid And sv.Application_Sid IsNot Nothing And sv.Application_Sid > 0
                                        Select New Objects.DDSkills_Info With {
                                            .Category_Desc = ct.Category_Code,
                                            .Skill_Desc = st.Skill_Desc,
                                            .Skill_Type_Sid = svs.Skill_Type_Sid,
                                            .Category_Type_Sid = ct.Category_Type_Sid,
                                            .RN_DD_Person_Type_Xref_Sid = sv.RN_DD_Person_Type_Xref_Sid,
                                            .Verification_Date = svc.Verification_Date,
                                            .Verified_Person_Name = svc.Verified_Person_Name,
                                            .Verified_Person_Title = svc.Verified_Person_Title,
                                            .Skill_CheckList_Desc = sc.Skill_CheckList_Desc
                                            }).Distinct.ToList()
                        End If
                    End If
                End Using
            Catch ex As Exception
                Me.LogError("Error getting Skills list by uniqueid.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting Skills list by uniqueid.", True, False))
            End Try
            Return skill_list
        End Function


        Public Function GetSupervisorListByUniqueID(UniqueID As String) As List(Of SupervisorDetails) Implements IMAISReportQueries.GetSupervisorListByUniqueID
            Dim retList As New List(Of SupervisorDetails)
            Dim retHistory As New List(Of SupervisorDetails)
            Dim retCurrent As New List(Of SupervisorDetails)
            Try
                Using context As New MAISContext
                    Dim rndd_Sid As Integer = 0
                    If (UniqueID.Contains("RN")) Then
                        rndd_Sid = (From rd In context.RN_DD_Person_Type_Xref
                                    Join rn In context.RNs On rn.RN_Sid Equals rd.RN_DDPersonnel_Sid
                                    Where rn.RNLicense_Number = UniqueID
                                    Select rd.RN_DD_Person_Type_Xref_Sid).FirstOrDefault()
                    Else
                        rndd_Sid = (From rd In context.RN_DD_Person_Type_Xref
                                   Join dd In context.DDPersonnels On dd.DDPersonnel_Sid Equals rd.RN_DDPersonnel_Sid
                                   Where dd.DDPersonnel_Code = UniqueID
                                   Select rd.RN_DD_Person_Type_Xref_Sid).FirstOrDefault()
                    End If

                    If (rndd_Sid > 0) Then
                        retHistory = (From ss In context.History_Supervisor
                                    Join pe In context.History_Employment On pe.History_Supervisor_SID Equals ss.History_Supervisor_SID
                                    Join rn In context.RN_DD_Person_Type_Xref On pe.RN_DD_Person_Type_Xref_SID Equals rn.RN_DD_Person_Type_Xref_Sid
                                    Group Join em In context.Email1 On em.Email_SID Equals ss.Email_SID Into emm = Group
                                    From em1 In emm.DefaultIfEmpty()
                                    Group Join ph In context.Phone_Number On ph.Phone_Number_SID Equals ss.Phone_SID Into phh = Group
                                    From ph1 In phh.DefaultIfEmpty()
                                    Order By ss.Last_Name
                                    Where pe.RN_DD_Person_Type_Xref_SID = rndd_Sid
                                    Select New Objects.SupervisorDetails() With {
                                        .supFirstName = ss.First_Name,
                                        .supLastName = ss.Last_Name,
                                        .EmailAddress = em1.Email_Address,
                                        .supEndDate = ss.Supervisor_End_Date,
                                        .supStartDate = ss.Supervisor_Start_Date,
                                        .PhoneNumber = ph1.Phone_Number1
                                    }).Distinct.ToList()
                        If (retHistory.Count > 0) Then
                            retList.AddRange(retHistory)
                        End If
                        'retCurrent = (From ep In context.Employer_RN_DD_Person_Type_Xref
                        '  Join rn In context.RN_DD_Person_Type_Xref On ep.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid
                        '  Order By ep.Supervisor_First_Name
                        '  Where ep.RN_DD_Person_Type_Xref_Sid = rndd_Sid
                        ' Select New Objects.SupervisorDetails() With {
                        '                .supFirstName = ep.Supervisor_First_Name,
                        '                .supLastName = ep.Supervisor_Last_Name,
                        '                .supEndDate = ep.Supervisor_End_date,
                        '                .supStartDate = ep.Supervisor_Start_date
                        '            }).Distinct.ToList()
                        retCurrent = (From ep In context.Employer_RN_DD_Person_Type_Xref
                                      Join ee In context.Employers On ee.Employer_Sid Equals ep.Employer_Sid
                          Group Join eep In context.Employer_RN_DD_Person_Type_Email_Xref On eep.RN_DD_Person_Type_Xref_Sid Equals ep.RN_DD_Person_Type_Xref_Sid And ee.Employer_Sid Equals eep.Employer_Sid Into supemail = Group
                          From eep1 In supemail.DefaultIfEmpty(Nothing)
                          Group Join epp In context.Employer_RN_DD_Person_Type_Phone_Xref On epp.RN_DD_Person_Type_Xref_Sid Equals ep.RN_DD_Person_Type_Xref_Sid And ee.Employer_Sid Equals epp.Employer_Sid Into supphone = Group
                          From epp1 In supphone.DefaultIfEmpty(Nothing)
                          Group Join emm In context.Email1 On emm.Email_SID Equals eep1.Email_Sid Into emmm = Group
                          From em1 In emmm.DefaultIfEmpty(Nothing)
                          Group Join phh In context.Phone_Number On phh.Phone_Number_SID Equals epp1.Phone_Sid Into phhh = Group
                          From ph1 In phhh.DefaultIfEmpty(Nothing)
                          Order By ep.Supervisor_First_Name
                          Where ep.RN_DD_Person_Type_Xref_Sid = rndd_Sid And If(epp1.Contact_Type_Sid = Nothing, True, epp1.Contact_Type_Sid = 7) And If(eep1.Contact_Type_Sid = Nothing, True, eep1.Contact_Type_Sid = 7)
                         Select New Objects.SupervisorDetails() With {
                                        .supFirstName = ep.Supervisor_First_Name,
                                        .supLastName = ep.Supervisor_Last_Name,
                                        .EmailAddress = em1.Email_Address,
                                        .supEndDate = ep.Supervisor_End_date,
                                        .supStartDate = ep.Supervisor_Start_date,
                                        .PhoneNumber = ph1.Phone_Number1
                                    }).Distinct.ToList()
                        If (retCurrent.Count > 0) Then
                            retList.AddRange(retCurrent)
                        End If
                    End If
                End Using
            Catch ex As Exception
                Me.LogError("Error getting Supervisors List.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting Supervisors List.", True, False))
            End Try
            Return retList.Distinct.ToList()
        End Function

        Public Function GetAllCoursesByUniqueID(ByVal uniqueid As String) As List(Of Course_Info) Implements IMAISReportQueries.GetAllCoursesByUniqueID
            Dim cou_list As New List(Of Course_Info)
            Try
                Using context As New MAISContext
                    Dim rndd_Sid As Integer = 0
                    If (uniqueid.Contains("RN")) Then
                        rndd_Sid = (From rd In context.RN_DD_Person_Type_Xref
                                    Join rn In context.RNs On rn.RN_Sid Equals rd.RN_DDPersonnel_Sid
                                    Where rn.RNLicense_Number = uniqueid
                                    Select rd.RN_DD_Person_Type_Xref_Sid).FirstOrDefault()
                    Else
                        rndd_Sid = (From rd In context.RN_DD_Person_Type_Xref
                                   Join dd In context.DDPersonnels On dd.DDPersonnel_Sid Equals rd.RN_DDPersonnel_Sid
                                   Where dd.DDPersonnel_Code = uniqueid
                                   Select rd.RN_DD_Person_Type_Xref_Sid).FirstOrDefault()
                    End If
                    If (rndd_Sid > 0) Then
                        cou_list = (From rol In context.Role_RN_DD_Personnel_Xref
                                    Join p In context.Person_Course_Xref On p.Role_RN_DD_Personnel_Xref_Sid Equals rol.Role_RN_DD_Personnel_Xref_Sid
                                    Join s In context.Person_Course_Session_Xref On s.Person_Course_Xref_Sid Equals p.Person_Course_Xref_Sid
                                    Join ss In context.Sessions On ss.Session_Sid Equals s.Session_Sid
                                    Join c In context.Courses On c.Course_sid Equals ss.Course_Sid
                                    Join r In context.RNs On r.RN_Sid Equals c.RN_Sid
                                    Where rol.RN_DD_Person_Type_Xref_Sid = rndd_Sid
                                    Select New Objects.Course_Info With {
                                        .Course_Number = c.OBN_Course_Number,
                                        .Course_Sid = c.Course_sid,
                                        .Session_CEUs = ss.Total_CEs,
                                        .Session_End_Date = ss.End_Date,
                                        .Session_Sid = ss.Session_Sid,
                                        .Session_Start_Date = ss.Start_Date,
                                        .Trainer_Name_FN = r.First_Name,
                                        .Trainer_Name_LN = r.Last_Name
                                        }).Distinct.ToList()
                    End If
                End Using
            Catch ex As Exception
                Me.LogError("Error getting course list by uniqueid.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting course list by uniqueid.", True, False))
            End Try
            Return cou_list
        End Function

        Public Function GetDDRNCEUSRenewal(uniqueID As String) As List(Of CEUsDetailsObject) Implements IMAISReportQueries.GetDDRNCEUSRenewal
            Dim ceusLst As New List(Of CEUsDetailsObject)
            Try
                Using context As New MAISContext
                    Dim rnDDPerID As Integer
                    'Dim certEndDate As DateTime

                    If uniqueID.Contains("DD") Then
                        rnDDPerID = (From r In context.RN_DD_Person_Type_Xref _
                                                                    Join ddP In context.DDPersonnels On ddP.DDPersonnel_Sid Equals r.RN_DDPersonnel_Sid
                                                                   Where ddP.DDPersonnel_Code = uniqueID
                                                                   Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault

                    Else ' Search the RN Personnel
                        rnDDPerID = (From r In context.RN_DD_Person_Type_Xref _
                                                              Join RN In context.RNs On RN.RN_Sid Equals r.RN_DDPersonnel_Sid
                                                              Where RN.RNLicense_Number = uniqueID
                                                              Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault
                    End If

                    If (rnDDPerID > 0) Then

                        ceusLst = (From c In context.CEUs_Renewal
                                  Where c.RN_DD_Person_Type_Xref_Sid = rnDDPerID
                                  Select New Objects.CEUsDetailsObject With {
                                      .CEUs_Renewal_Sid = c.CEUs_Renewal_Sid,
                                      .Application_Sid = c.Application_Sid,
                                      .Permanent_Flg = c.Permanent_Flg,
                                      .Start_Date = c.Start_Date,
                                      .End_Date = c.End_Date,
                                      .Category_Type_Sid = c.Category_Type_Sid,
                                      .Category_Type_Code = c.Category_Type.Category_Code,
                                      .Attended_Date = c.Attended_Date,
                                      .Total_CEUs = c.Total_CEUs,
                                      .RN_Sid = c.RN_Sid,
                                      .RN_Name = c.RN.Last_Name & ", " & c.RN.First_Name,
                                      .Instructor_Name = c.Instructor_Name,
                                      .Title = c.Title,
                                      .Course_Description = c.Course_Description,
                                      .Active_Flag = c.Active_Flg}).ToList
                    End If

                End Using
            Catch ex As Exception
                Me.LogError("Error getting ceus renewal list by uniqueid.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting ceus renewal list by uniqueid.", True, False))
            End Try
            Return ceusLst
        End Function
    End Class
End Namespace