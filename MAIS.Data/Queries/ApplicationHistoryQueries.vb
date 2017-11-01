Imports System.Data.Linq
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Data.Objects

Namespace Queries
    Public Interface IApplicationHistoryQueries
        Inherits IQueriesBase

        Function GetSearchResults(ByVal searchCriteria As Objects.ApplicationHistorySearchCriteria) As List(Of Objects.ApplicationHistory)
        Function GetApplicationStatusDetail(ByVal ApplicationID As Integer) As List(Of Objects.ApplicationHistoryStatusDetail)
    End Interface

    Public Class ApplicationHistoryQueries
        Inherits QueriesBase
        Implements IApplicationHistoryQueries


        Public Function GetSearchResults(searchCriteria As ApplicationHistorySearchCriteria) As List(Of ApplicationHistory) Implements IApplicationHistoryQueries.GetSearchResults
            Dim appHistory As New List(Of ApplicationHistory)
            If (searchCriteria.StartDate IsNot Nothing And searchCriteria.EndDate Is Nothing) Then
                searchCriteria.EndDate = DateTime.Now
            End If
            Using context As New MAISContext()
                Try
                    If (searchCriteria.RNDDFlag) Then
                        appHistory = (From appHist In context.Application_History
                                      Join rnRNDDXref In context.RN_DD_Person_Type_Xref On rnRNDDXref.RN_DD_Person_Type_Xref_Sid Equals appHist.RN_DD_Person_Type_Xref_Sid
                                      Join appType In context.Application_Type On appHist.Application_Type_Sid Equals appType.Application_Type_Sid
                                      Group Join rn In context.RNs On rn.RN_Sid Equals appHist.Attestant_SId Into appRN = Group
                                      From rnAppHist In appRN.DefaultIfEmpty()
                                      Group Join rnuserMapping In context.User_RN_Mapping On appHist.Last_Update_By Equals rnuserMapping.UserID Into userRN = Group
                                      From rnUserAppHist In userRN.DefaultIfEmpty()
                                      Where If(searchCriteria.RNLicenseNumber.Equals(String.Empty), True, appHist.RN_License_Or_4SSN = searchCriteria.RNLicenseNumber) AndAlso _
                                      If(searchCriteria.RoleCategory = 0 And searchCriteria.ApplicationTypeID = 0, True, appHist.Role_Category_Level_Sid = searchCriteria.RoleCategory And appHist.Application_Type_Sid = searchCriteria.ApplicationTypeID) _
                                      AndAlso If(searchCriteria.StartDate Is Nothing, True, appHist.Create_Date >= searchCriteria.StartDate And appHist.Create_Date <= searchCriteria.EndDate) AndAlso rnRNDDXref.Person_Type_Sid = 1 _
                                      AndAlso If(searchCriteria.RNID = 0, True, rnUserAppHist.RN_Sid = searchCriteria.RNID)
                                        Select New Objects.ApplicationHistory() With {
                                            .ApplicationID = appHist.Application_Sid,
                                            .ApplicationTypeID = appHist.Application_Type_Sid,
                                            .ApplicationType = appType.Application_Code,
                                            .AttestationDate = appHist.Attestation_Date,
                                            .CertificatePrintDate = appHist.Certification_Printed_Date,
                                            .DecisionMadeRNFirstName = rnAppHist.First_Name,
                                            .DecisionMadeRNLastName = rnAppHist.Last_Name,
                                            .EmailEndDate = appHist.Certification_Sent_Email_Date,
                                            .SkillsEndDate = appHist.DDPersonnel_Skills_Date,
                                            .TrainingEndDate = appHist.Training_CEUS_Date,
                                            .CreateDate = appHist.Create_Date,
                                            .RoleLevelCategory = appHist.Role_Category_Level_Sid,
                                            .LastUpdatedUserID = appHist.Last_Update_By,
                        .Last4SSNorRNLicenseNumber = appHist.RN_License_Or_4SSN
                                            }).Distinct().ToList()
                        For Each appH As ApplicationHistory In appHistory
                            appH.FinalApplicationStatus = (From app In context.Application_History
                                                          Join appStatus In context.Application_History_Status On appStatus.Application_History_Sid Equals app.Application_History_Sid
                                                          Join appStatusType In context.Application_Status_Type On appStatusType.Application_Status_Type_Sid Equals appStatus.Application_Status_Type_Sid
                                                          Where app.Application_Sid = appH.ApplicationID Order By appStatus.Last_Update_Date Descending Select appStatusType.Application_Status_Desc).FirstOrDefault()
                            appH.FinalApplicationStatusID = (From app In context.Application_History
                                                          Join appStatus In context.Application_History_Status On appStatus.Application_History_Sid Equals app.Application_History_Sid
                                                          Join appStatusType In context.Application_Status_Type On appStatusType.Application_Status_Type_Sid Equals appStatus.Application_Status_Type_Sid
                                                          Where app.Application_Sid = appH.ApplicationID Order By appStatus.Last_Update_Date Descending Select appStatus.Application_Status_Type_Sid).FirstOrDefault()
                            appH.FinalDecisionLastName = (From app In context.Application_History
                                                          Join appStatus In context.Application_History_Status On appStatus.Application_History_Sid Equals app.Application_History_Sid
                                                          Join usermap In context.User_RN_Mapping On usermap.UserID Equals appStatus.Last_Update_By
                                                          Group Join rn In context.RNs On rn.RN_Sid Equals usermap.RN_Sid Into appRN = Group
                                                          From rnAppHist In appRN.DefaultIfEmpty()
                                                          Where app.Application_Sid = appH.ApplicationID Order By appStatus.Last_Update_Date Descending Select rnAppHist.Last_Name).FirstOrDefault()
                            appH.FinalDecisionFirstName = (From app In context.Application_History
                                                          Join appStatus In context.Application_History_Status On appStatus.Application_History_Sid Equals app.Application_History_Sid
                                                          Join usermap In context.User_RN_Mapping On usermap.UserID Equals appStatus.Last_Update_By
                                                          Group Join rn In context.RNs On rn.RN_Sid Equals usermap.RN_Sid Into appRN = Group
                                                          From rnAppHist In appRN.DefaultIfEmpty()
                                                          Where app.Application_Sid = appH.ApplicationID Order By appStatus.Last_Update_Date Descending Select rnAppHist.First_Name).FirstOrDefault()
                        Next
                    Else

                        appHistory = (From appHist In context.Application_History
                                      Join rnRNDDXref In context.RN_DD_Person_Type_Xref On rnRNDDXref.RN_DD_Person_Type_Xref_Sid Equals appHist.RN_DD_Person_Type_Xref_Sid
                                      Join dd In context.DDPersonnels On dd.DDPersonnel_Sid Equals rnRNDDXref.RN_DDPersonnel_Sid
                                      Join appType In context.Application_Type On appHist.Application_Type_Sid Equals appType.Application_Type_Sid
                                      Group Join rn In context.RNs On rn.RN_Sid Equals appHist.Attestant_SId Into appRN = Group
                                      From rnAppHist In appRN.DefaultIfEmpty()
                                      Group Join rnuserMapping In context.User_RN_Mapping On appHist.Last_Update_By Equals rnuserMapping.UserID Into userRN = Group
                                      From rnUserAppHist In userRN.DefaultIfEmpty()
                                      Where If(searchCriteria.Last4SSN.Equals(String.Empty), True, appHist.RN_License_Or_4SSN = searchCriteria.Last4SSN) AndAlso rnRNDDXref.Person_Type_Sid = 2 _
                        AndAlso If(searchCriteria.RoleCategory = 0 And searchCriteria.ApplicationTypeID = 0, True, appHist.Role_Category_Level_Sid = searchCriteria.RoleCategory And appHist.Application_Type_Sid = searchCriteria.ApplicationTypeID) _
                                  AndAlso If(searchCriteria.StartDate Is Nothing, True, appHist.Create_Date >= searchCriteria.StartDate And appHist.Create_Date <= searchCriteria.EndDate) _
                                  AndAlso If(searchCriteria.RNID = 0, True, rnUserAppHist.RN_Sid = searchCriteria.RNID) AndAlso If(searchCriteria.DDpersonnelCode.Equals(String.Empty), True, dd.DDPersonnel_Code = searchCriteria.DDpersonnelCode)
                                        Select New Objects.ApplicationHistory() With {
                                            .ApplicationID = appHist.Application_Sid,
                                            .ApplicationTypeID = appHist.Application_Type_Sid,
                                            .ApplicationType = appType.Application_Code,
                                            .AttestationDate = appHist.Attestation_Date,
                                            .CertificatePrintDate = appHist.Certification_Printed_Date,
                                            .DecisionMadeRNFirstName = rnAppHist.First_Name,
                                            .DecisionMadeRNLastName = rnAppHist.Last_Name,
                                            .EmailEndDate = appHist.Certification_Sent_Email_Date,
                                            .SkillsEndDate = appHist.DDPersonnel_Skills_Date,
                                            .TrainingEndDate = appHist.Training_CEUS_Date,
                                            .CreateDate = appHist.Create_Date,
                                            .RoleLevelCategory = appHist.Role_Category_Level_Sid,
                                            .LastUpdatedUserID = appHist.Last_Update_By,
                                            .DDPersonnelCode = dd.DDPersonnel_Code,
                                            .Last4SSNorRNLicenseNumber = appHist.RN_License_Or_4SSN
                                            }).Distinct().ToList()
                        For Each appH As ApplicationHistory In appHistory
                            appH.FinalApplicationStatus = (From app In context.Application_History
                                                          Join appStatus In context.Application_History_Status On appStatus.Application_History_Sid Equals app.Application_History_Sid
                                                          Join appStatusType In context.Application_Status_Type On appStatusType.Application_Status_Type_Sid Equals appStatus.Application_Status_Type_Sid
                                                          Where app.Application_Sid = appH.ApplicationID Order By appStatus.Last_Update_Date Descending Select appStatusType.Application_Status_Desc).FirstOrDefault()
                            appH.FinalApplicationStatusID = (From app In context.Application_History
                                                          Join appStatus In context.Application_History_Status On appStatus.Application_History_Sid Equals app.Application_History_Sid
                                                          Join appStatusType In context.Application_Status_Type On appStatusType.Application_Status_Type_Sid Equals appStatus.Application_Status_Type_Sid
                                                          Where app.Application_Sid = appH.ApplicationID Order By appStatus.Last_Update_Date Descending Select appStatus.Application_Status_Type_Sid).FirstOrDefault()
                            appH.FinalDecisionLastName = (From app In context.Application_History
                                                          Join appStatus In context.Application_History_Status On appStatus.Application_History_Sid Equals app.Application_History_Sid
                                                          Join usermap In context.User_RN_Mapping On usermap.UserID Equals appStatus.Last_Update_By
                                                          Group Join rn In context.RNs On rn.RN_Sid Equals usermap.RN_Sid Into appRN = Group
                                                          From rnAppHist In appRN.DefaultIfEmpty()
                                                          Where app.Application_Sid = appH.ApplicationID Order By appStatus.Last_Update_Date Descending Select rnAppHist.Last_Name).FirstOrDefault()
                            appH.FinalDecisionFirstName = (From app In context.Application_History
                                                          Join appStatus In context.Application_History_Status On appStatus.Application_History_Sid Equals app.Application_History_Sid
                                                          Join usermap In context.User_RN_Mapping On usermap.UserID Equals appStatus.Last_Update_By
                                                          Group Join rn In context.RNs On rn.RN_Sid Equals usermap.RN_Sid Into appRN = Group
                                                          From rnAppHist In appRN.DefaultIfEmpty()
                                                          Where app.Application_Sid = appH.ApplicationID Order By appStatus.Last_Update_Date Descending Select rnAppHist.First_Name).FirstOrDefault()
                        Next
                    End If
                    If (searchCriteria.ApplicationStatus > 0) Then
                        If (appHistory.Count > 0) Then
                            appHistory = appHistory.Where(Function(a) a.FinalApplicationStatusID = searchCriteria.ApplicationStatus).ToList()
                        Else
                            If (appHistory.Count = 0 And searchCriteria.ApplicationTypeID = 0 And String.IsNullOrEmpty(searchCriteria.DDpersonnelCode) And _
                               searchCriteria.StartDate Is Nothing And String.IsNullOrEmpty(searchCriteria.Last4SSN) And searchCriteria.RNID = 0 And searchCriteria.RoleCategory = 0 And String.IsNullOrEmpty(searchCriteria.RNLicenseNumber)) Then
                                appHistory = (From appHist In context.Application_History
                                                  Join appType In context.Application_Type On appHist.Application_Type_Sid Equals appType.Application_Type_Sid
                                                Group Join rnRNDDXref In context.RN_DD_Person_Type_Xref On rnRNDDXref.RN_DD_Person_Type_Xref_Sid Equals appHist.RN_DD_Person_Type_Xref_Sid Into rnDDXref = Group
                                                From rnDD In rnDDXref.DefaultIfEmpty()
                                                   Group Join dd In context.DDPersonnels On dd.DDPersonnel_Sid Equals rnDD.RN_DDPersonnel_Sid Into ddPersonnels = Group
                                                   From ddp In ddPersonnels.DefaultIfEmpty()
                                                  Group Join rn In context.RNs On rn.RN_Sid Equals appHist.Attestant_SId Into appRN = Group
                                                  From rnAppHist In appRN.DefaultIfEmpty() Where If(searchCriteria.RNDDFlag, rnDD.Person_Type_Sid = 1, rnDD.Person_Type_Sid = 2)
                                                    Select New Objects.ApplicationHistory() With {
                                                        .ApplicationID = appHist.Application_Sid,
                                                        .ApplicationTypeID = appHist.Application_Type_Sid,
                                                        .ApplicationType = appType.Application_Code,
                                                        .AttestationDate = appHist.Attestation_Date,
                                                        .CertificatePrintDate = appHist.Certification_Printed_Date,
                                                        .DecisionMadeRNFirstName = rnAppHist.First_Name,
                                                        .DecisionMadeRNLastName = rnAppHist.Last_Name,
                                                        .EmailEndDate = appHist.Certification_Sent_Email_Date,
                                                        .SkillsEndDate = appHist.DDPersonnel_Skills_Date,
                                                        .TrainingEndDate = appHist.Training_CEUS_Date,
                                                        .CreateDate = appHist.Create_Date,
                                                        .RoleLevelCategory = appHist.Role_Category_Level_Sid,
                                                        .LastUpdatedUserID = appHist.Last_Update_By,
                                                        .DDPersonnelCode = ddp.DDPersonnel_Code,
                                                        .Last4SSNorRNLicenseNumber = appHist.RN_License_Or_4SSN
                                                        }).Distinct().ToList()
                                For Each appH As ApplicationHistory In appHistory
                                    appH.FinalApplicationStatus = (From app In context.Application_History
                                                                  Join appStatus In context.Application_History_Status On appStatus.Application_History_Sid Equals app.Application_History_Sid
                                                                  Join appStatusType In context.Application_Status_Type On appStatusType.Application_Status_Type_Sid Equals appStatus.Application_Status_Type_Sid
                                                                  Where app.Application_Sid = appH.ApplicationID Order By appStatus.Last_Update_Date Descending Select appStatusType.Application_Status_Desc).FirstOrDefault()
                                    appH.FinalApplicationStatusID = (From app In context.Application_History
                                                                  Join appStatus In context.Application_History_Status On appStatus.Application_History_Sid Equals app.Application_History_Sid
                                                                  Join appStatusType In context.Application_Status_Type On appStatusType.Application_Status_Type_Sid Equals appStatus.Application_Status_Type_Sid
                                                                  Where app.Application_Sid = appH.ApplicationID Order By appStatus.Last_Update_Date Descending Select appStatus.Application_Status_Type_Sid).FirstOrDefault()
                                    appH.FinalDecisionLastName = (From app In context.Application_History
                                                                  Join appStatus In context.Application_History_Status On appStatus.Application_History_Sid Equals app.Application_History_Sid
                                                                  Join usermap In context.User_RN_Mapping On usermap.UserID Equals appStatus.Last_Update_By
                                                                  Group Join rn In context.RNs On rn.RN_Sid Equals usermap.RN_Sid Into appRN = Group
                                                                  From rnAppHist In appRN.DefaultIfEmpty()
                                                                  Where app.Application_Sid = appH.ApplicationID Order By appStatus.Last_Update_Date Descending Select rnAppHist.Last_Name).FirstOrDefault()
                                    appH.FinalDecisionFirstName = (From app In context.Application_History
                                                                  Join appStatus In context.Application_History_Status On appStatus.Application_History_Sid Equals app.Application_History_Sid
                                                                  Join usermap In context.User_RN_Mapping On usermap.UserID Equals appStatus.Last_Update_By
                                                                  Group Join rn In context.RNs On rn.RN_Sid Equals usermap.RN_Sid Into appRN = Group
                                                                  From rnAppHist In appRN.DefaultIfEmpty()
                                                                  Where app.Application_Sid = appH.ApplicationID Order By appStatus.Last_Update_Date Descending Select rnAppHist.First_Name).FirstOrDefault()
                                Next
                                appHistory = appHistory.Where(Function(a) a.FinalApplicationStatusID = searchCriteria.ApplicationStatus).ToList()
                            End If
                        End If
                    End If
                    'Dim listOfAppDetails As New List(Of Objects.ApplicationHistoryStatusDetail)
                    Dim listOfAppDetails1 As New List(Of Objects.ApplicationHistoryStatusDetail)
                    For Each appDetails In appHistory
                        listOfAppDetails1 = (From appHist In context.Application_History
                                              Join appHistStatus In context.Application_History_Status On appHistStatus.Application_History_Sid Equals appHist.Application_History_Sid
                                              Join appStatus In context.Application_Status_Type On appStatus.Application_Status_Type_Sid Equals appHistStatus.Application_Status_Type_Sid
                                              Group Join usermap In context.User_RN_Mapping On usermap.UserID Equals appHistStatus.Last_Update_By Into userApp = Group
                                              From userMapID In userApp.DefaultIfEmpty()
                                              Group Join rn In context.RNs On rn.RN_Sid Equals userMapID.RN_Sid Into appRN = Group
                                          From rnAppHist In appRN.DefaultIfEmpty()
                                              Where appHist.Application_Sid = appDetails.ApplicationID
                                              Select New Objects.ApplicationHistoryStatusDetail() With {
                                                  .ApplicationLatestUpdatedDate = appHistStatus.Last_Update_Date,
                                                  .ApplicationStatus = appStatus.Application_Status_Desc,
                                                  .RNFirstName = (From usr In context.User_Mapping Where usr.UserID = appDetails.LastUpdatedUserID
                                                                    Select usr.First_Name).FirstOrDefault,
                                                  .RNlastName = (From usr In context.User_Mapping Where usr.UserID = appDetails.LastUpdatedUserID
                                                                    Select usr.Last_Name).FirstOrDefault,
                                                  .UserRole = (From usr In context.User_Mapping Where usr.UserID = appDetails.LastUpdatedUserID
                                                                    Select usr.Portal_User_Role).FirstOrDefault
                                                  }).Distinct().ToList()
                        appDetails.ListOfApplicationDetail = listOfAppDetails1
                    Next
                Catch ex As Exception
                    Me.LogError("Error Getting in search results for application history", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting in search results for application history", True, False))
                    Throw
                End Try
            End Using
            Return appHistory
        End Function

        Public Function GetApplicationStatusDetail(ApplicationID As Integer) As List(Of ApplicationHistoryStatusDetail) Implements IApplicationHistoryQueries.GetApplicationStatusDetail
            Dim appStatusHistorydetail As New List(Of ApplicationHistoryStatusDetail)
            Using context As New MAISContext()
                Try
                    appStatusHistorydetail = (From appHist In context.Application_History
                                              Join appHistStatus In context.Application_History_Status On appHistStatus.Application_History_Sid Equals appHist.Application_History_Sid
                                              Join appStatus In context.Application_Status_Type On appStatus.Application_Status_Type_Sid Equals appHistStatus.Application_Status_Type_Sid
                                              Join usermap In context.User_RN_Mapping On usermap.UserID Equals appHistStatus.Last_Update_By
                                              Group Join rn In context.RNs On rn.RN_Sid Equals usermap.RN_Sid Into appRN = Group
                                          From rnAppHist In appRN.DefaultIfEmpty()
                                              Where appHist.Application_Sid = ApplicationID
                                              Select New Objects.ApplicationHistoryStatusDetail() With {
                                                  .ApplicationLatestUpdatedDate = appHistStatus.Last_Update_Date,
                                                  .ApplicationStatus = appStatus.Application_Status_Desc,
                                                  .RNFirstName = rnAppHist.First_Name,
                                                  .RNlastName = rnAppHist.Last_Name
                                                  }).Distinct().ToList()
                Catch ex As Exception
                    Me.LogError("Error Getting in search results for application status detail history.", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting in search results for application status detail history.", True, False))
                    Throw
                End Try
            End Using
            Return appStatusHistorydetail
        End Function
    End Class
End Namespace
