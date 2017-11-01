Imports MAIS.Data

Namespace Mapping
    Public Class MAISDODDMapping

        Public Shared Function MapObjectsToDDModels(ByVal cd As List(Of Objects.DDDetailInformation)) As List(Of Models.DDDetailInformation)
            Dim returnModelApp As New List(Of Models.DDDetailInformation)
            For Each dd As Objects.DDDetailInformation In cd
                Dim modelApp As New Models.DDDetailInformation
                modelApp.dob = dd.dob
                modelApp.DDPersonnelCode = dd.DDPersonnelCode
                modelApp.FirstName = dd.FirstName
                modelApp.Last4SSN = dd.Last4SSN.ToString()
                modelApp.LastName = dd.LastName
                Dim appDetailHistory As New List(Of Models.CertificateDetails)
                If dd.CertificateDetails Is Nothing Then dd.CertificateDetails = New List(Of Objects.CertificateDetails)
                For Each b As Objects.CertificateDetails In dd.CertificateDetails
                    Dim appHist As New Models.CertificateDetails
                    appHist.ConsectiveRenewals = b.ConsectiveRenewals
                    appHist.CurrentStatus = b.CurrentStatus
                    appHist.EffectiveDate = b.EffectiveDate
                    appHist.ExpirationDate = b.ExpirationDate
                    appHist.RoleDescription = b.RoleDescription
                    appDetailHistory.Add(appHist)
                Next
                modelApp.CertificateDetails = appDetailHistory
                returnModelApp.Add(modelApp)
            Next
            Return returnModelApp
        End Function
        Public Shared Function MapObjectsToRNModels(ByVal cd As List(Of Objects.RNInformationDetailsForWS)) As List(Of Models.RNInformationDetailsForWS)
            Dim returnModelApp As New List(Of Models.RNInformationDetailsForWS)
            For Each dd As Objects.RNInformationDetailsForWS In cd
                Dim modelApp As New Models.RNInformationDetailsForWS
                modelApp.RNLicenseNumber = dd.RNLicenseNumber
                modelApp.FirstName = dd.FirstName
                modelApp.LastName = dd.LastName
                Dim appDetailHistory As New List(Of Models.CertificateDetails)
                If dd.CertificateDetails Is Nothing Then dd.CertificateDetails = New List(Of Objects.CertificateDetails)
                For Each b As Objects.CertificateDetails In dd.CertificateDetails
                    Dim appHist As New Models.CertificateDetails
                    appHist.ConsectiveRenewals = b.ConsectiveRenewals
                    appHist.CurrentStatus = b.CurrentStatus
                    appHist.EffectiveDate = b.EffectiveDate
                    appHist.ExpirationDate = b.ExpirationDate
                    appHist.RoleDescription = b.RoleDescription
                    appDetailHistory.Add(appHist)
                Next
                modelApp.CertificateDetails = appDetailHistory
                returnModelApp.Add(modelApp)
            Next
            Return returnModelApp
        End Function
        Public Shared Function MapObjectsToTrainingModels(ByVal cd As List(Of Objects.TrainingSessionResults)) As List(Of Models.TrainingSessionResults)
            Dim returnModelApp As New List(Of Models.TrainingSessionResults)
            For Each dd As Objects.TrainingSessionResults In cd
                Dim modelApp As New Models.TrainingSessionResults
                modelApp.County = dd.County
                modelApp.CourseCategory = dd.CourseCategory
                modelApp.EndDate = dd.EndDate
                modelApp.RNTrainerEmail = dd.RNTrainerEmail
                modelApp.RNTrainerName = dd.RNTrainerFirstName + " " + dd.RNTrainerlastName
                modelApp.StartDate = dd.StartDate
                modelApp.OBNNumber = dd.OBNNumber
                returnModelApp.Add(modelApp)
            Next
            Return returnModelApp
        End Function
    End Class
End Namespace
