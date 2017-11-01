Imports MAIS.Data

Namespace Mapping

    Public Class ApplicationHistoryMapping
        Public Shared Function MapToModelApplicationInformationDetails(ByVal dbMA_Application As Model.ApplicationHistorySearchCriteria) As Objects.ApplicationHistorySearchCriteria
            Dim returnModelApp As New Objects.ApplicationHistorySearchCriteria

            With returnModelApp
                .RNLicenseNumber = dbMA_Application.RNLicenseNumber
                .Last4SSN = dbMA_Application.Last4SSN
                .ApplicationStatus = dbMA_Application.ApplicationStatus
                .ApplicationTypeID = dbMA_Application.ApplicationTypeID
                .DDpersonnelCode = dbMA_Application.DDpersonnelCode
                .EndDate = dbMA_Application.EndDate
                .RNDDFlag = dbMA_Application.RNDDFlag
                .RNID = dbMA_Application.RNID
                .RoleCategory = dbMA_Application.RoleCategory
                .StartDate = dbMA_Application.StartDate
            End With
            Return returnModelApp
        End Function

        Public Shared Function MapToObjectApplicationInformationDetails(ByVal objectApp As List(Of Objects.ApplicationHistory)) As List(Of Model.ApplicationHistoryModel)
            Dim returnModelApp As New List(Of Model.ApplicationHistoryModel)
            For Each app As Objects.ApplicationHistory In objectApp
                Dim modelApp As New Model.ApplicationHistoryModel
                modelApp.ApplicationID = app.ApplicationID
                modelApp.ApplicationType = app.ApplicationType
                If Not String.IsNullOrWhiteSpace(app.DDPersonnelCode) Then
                    modelApp.UniqueCodeOrLicense = app.DDPersonnelCode
                Else
                    modelApp.UniqueCodeOrLicense = app.Last4SSNorRNLicenseNumber
                End If

                If (app.AttestationDate IsNot Nothing) Then
                    modelApp.AttestationDate = Convert.ToDateTime(app.AttestationDate).ToShortDateString()
                Else
                    modelApp.AttestationDate = Nothing
                End If
                If (app.CertificatePrintDate IsNot Nothing) Then
                    modelApp.CertificatePrintDate = Convert.ToDateTime(app.CertificatePrintDate).ToShortDateString()
                Else
                    modelApp.CertificatePrintDate = Nothing
                End If
                If (Not String.IsNullOrEmpty(app.DecisionMadeRNFirstName) And Not String.IsNullOrEmpty(app.DecisionMadeRNLastName)) Then
                    modelApp.DecisionMadeRNName = app.DecisionMadeRNFirstName + " " + app.DecisionMadeRNLastName
                End If
                If (app.EmailEndDate IsNot Nothing) Then
                    modelApp.EmailEndDate = Convert.ToDateTime(app.EmailEndDate).ToShortDateString()
                Else
                    modelApp.EmailEndDate = Nothing
                End If
                modelApp.FinalApplicationStatus = app.FinalApplicationStatus
                If (Not String.IsNullOrEmpty(app.FinalDecisionFirstName) And Not String.IsNullOrEmpty(app.FinalDecisionLastName)) Then
                    modelApp.FinalDecisionName = app.FinalDecisionFirstName + " " + app.FinalDecisionLastName
                Else
                    modelApp.FinalDecisionName = "DODD Admin"
                End If
                If (app.SkillsEndDate IsNot Nothing) Then
                    modelApp.SkillsEndDate = Convert.ToDateTime(app.SkillsEndDate).ToShortDateString()
                Else
                    modelApp.SkillsEndDate = Nothing
                End If
                If (app.TrainingEndDate IsNot Nothing) Then
                    modelApp.TrainingEndDate = Convert.ToDateTime(app.TrainingEndDate).ToShortDateString()
                Else
                    modelApp.TrainingEndDate = Nothing
                End If
                Dim appDetailHistory As New List(Of Model.ApplicationHistoryStatusDetail)
                If app.ListOfApplicationDetail Is Nothing Then app.ListOfApplicationDetail = New List(Of Objects.ApplicationHistoryStatusDetail)
                For Each b As Objects.ApplicationHistoryStatusDetail In app.ListOfApplicationDetail
                    Dim appHist As New Model.ApplicationHistoryStatusDetail
                    If (Not String.IsNullOrEmpty(b.RNFirstName) And Not String.IsNullOrEmpty(b.RNlastName)) Then
                        appHist.RNName = b.RNFirstName + " " + b.RNlastName
                    Else
                        appHist.RNName = "DODD Admin"
                    End If
                    appHist.ApplicationStatus = b.ApplicationStatus
                    appHist.ApplicationLatestUpdatedDate = b.ApplicationLatestUpdatedDate.ToShortDateString()
                    If (Not String.IsNullOrEmpty(b.UserRole)) Then
                        appHist.UserRole = b.UserRole
                    Else
                        appHist.UserRole = "DODD Admin"
                    End If
                    appDetailHistory.Add(appHist)
                    modelApp.ListOfApplicationDetail = appDetailHistory
                Next
                returnModelApp.Add(modelApp)
            Next
            Return returnModelApp
        End Function
        Public Shared Function MapToObjectApplicationHistoryDetails(ByVal objectApp As List(Of Objects.ApplicationHistoryStatusDetail)) As List(Of Model.ApplicationHistoryStatusDetail)
            Dim returnModelApp As New List(Of Model.ApplicationHistoryStatusDetail)
            returnModelApp = (From app In objectApp
                              Select New Model.ApplicationHistoryStatusDetail With {
            .RNName = If(Not String.IsNullOrEmpty(app.RNFirstName) And Not String.IsNullOrEmpty(app.RNlastName), app.RNFirstName + " " + app.RNlastName, "DODD Admin"),
            .ApplicationStatus = app.ApplicationStatus,
            .ApplicationLatestUpdatedDate = (app.ApplicationLatestUpdatedDate).ToShortDateString(),
            .UserRole = If(Not String.IsNullOrEmpty(app.UserRole), app.UserRole, "DODD Admin")
                                  }).ToList
            Return returnModelApp
        End Function
    End Class
End Namespace
