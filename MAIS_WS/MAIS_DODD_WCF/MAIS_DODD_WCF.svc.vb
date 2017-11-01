Imports MAIS.Business
Imports MAIS.Business.Services

' NOTE: You can use the "Rename" command on the context menu to change the class name "Service1" in code, svc and config file together.
' NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.vb at the Solution Explorer and start debugging.
Public Class MAIS_DODD_WCF
    Implements IMAIS_DODD_WCF

    Public Sub New()
    End Sub

    Public Function GetDDData(DDpersonnelCode As String, SSN As String, lastname As String, firstname As String, EmployerName As String) As List(Of DTO.DDDetailInformation) Implements IMAIS_DODD_WCF.GetDDData
        Dim maisService As New MAISDODDService("0")
        Dim ddssn As Integer?
        If (Not String.IsNullOrEmpty(SSN)) Then
            ddssn = Convert.ToInt32(SSN)
        Else
            ddssn = Nothing
        End If
        Dim ddInfo As List(Of Models.DDDetailInformation) = maisService.GetDDData(DDpersonnelCode, ddssn, lastname, firstname, EmployerName)
        Dim returnModelApp As New List(Of DTO.DDDetailInformation)
        Try
            For Each dd As Models.DDDetailInformation In ddInfo
                Dim modelApp As New DTO.DDDetailInformation
                modelApp.DDPersonnelCode = dd.DDPersonnelCode
                modelApp.DOB = dd.dob.ToShortDateString()
                modelApp.FirstName = dd.FirstName
                modelApp.LastName = dd.LastName
                Dim appDetailHistory As New List(Of DTO.CertificateDetails)
                If dd.CertificateDetails Is Nothing Then dd.CertificateDetails = New List(Of Models.CertificateDetails)
                For Each b As Models.CertificateDetails In dd.CertificateDetails
                    Dim appHist As New DTO.CertificateDetails
                    appHist.ConsectiveRenewals = b.ConsectiveRenewals
                    appHist.CurrentStatus = b.CurrentStatus
                    appHist.EffectiveDate = b.EffectiveDate.ToShortDateString()
                    appHist.ExpirationDate = b.ExpirationDate.ToShortDateString()
                    appHist.RoleDescription = b.RoleDescription
                    appDetailHistory.Add(appHist)
                Next
                modelApp.CertificateDetails = appDetailHistory
                returnModelApp.Add(modelApp)
            Next
        Catch ex As Exception
            Throw ex
        End Try
        HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Origin", "*")
        Return returnModelApp
    End Function

    Public Function GetRNData(lastName As String, firstname As String, RnlicenseNumber As String) As List(Of DTO.RNDetailInformation) Implements IMAIS_DODD_WCF.GetRNData
        Dim maisService As New MAISDODDService("0")

        Dim rnInfo As List(Of Models.RNInformationDetailsForWS) = maisService.GetRNData(lastName, firstname, RnlicenseNumber)
        Dim returnModelApp As New List(Of DTO.RNDetailInformation)
        Try
            For Each rn As Models.RNInformationDetailsForWS In rnInfo
                Dim modelApp As New DTO.RNDetailInformation
                modelApp.RNLicenseNumber = rn.RNLicenseNumber
                modelApp.FirstName = rn.FirstName
                modelApp.LastName = rn.LastName
                Dim appDetailHistory As New List(Of DTO.CertificateDetails)
                If rn.CertificateDetails Is Nothing Then rn.CertificateDetails = New List(Of Models.CertificateDetails)
                For Each b As Models.CertificateDetails In rn.CertificateDetails
                    Dim appHist As New DTO.CertificateDetails
                    appHist.ConsectiveRenewals = b.ConsectiveRenewals
                    appHist.CurrentStatus = b.CurrentStatus
                    appHist.EffectiveDate = b.EffectiveDate.ToShortDateString()
                    appHist.ExpirationDate = b.ExpirationDate.ToShortDateString()
                    appHist.RoleDescription = b.RoleDescription
                    appDetailHistory.Add(appHist)
                Next
                modelApp.CertificateDetails = appDetailHistory
                returnModelApp.Add(modelApp)
            Next
        Catch ex As Exception
            Throw ex
        End Try
        HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Origin", "*")
        Return returnModelApp
    End Function

    Public Function GetTrainingSessionData(sessionStartDate As String, sessionEndDate As String, county As String, lastName As String, firstname As String, rnsession As String) As List(Of DTO.TrainingSessionResults) Implements IMAIS_DODD_WCF.GetTrainingSessionData
        Dim maisService As New MAISDODDService("0")
        Dim sessionStart As Date?
        If (Not String.IsNullOrEmpty(sessionStartDate)) Then
            sessionStart = CDate(sessionStartDate)
        Else
            sessionStart = Nothing
        End If
        Dim rns As Boolean = IIf(rnsession = "1", True, False)
        Dim sessionEnd As Date?
        If (Not String.IsNullOrEmpty(sessionEndDate)) Then
            sessionEnd = CDate(sessionEndDate)
        Else
            sessionEnd = Nothing
        End If
        Dim trainingInfo As List(Of Models.TrainingSessionResults) = maisService.GetTrainingSessionData(sessionStart, sessionEnd, county, lastName, firstname, rns)
        Dim returnModelApp As New List(Of DTO.TrainingSessionResults)
        Try
            For Each dd As Models.TrainingSessionResults In trainingInfo
                Dim modelApp As New DTO.TrainingSessionResults
                modelApp.County = dd.County
                modelApp.CourseCategory = dd.CourseCategory
                modelApp.EndDate = dd.EndDate.ToShortDateString()
                modelApp.RNTrainerEmail = dd.RNTrainerEmail
                modelApp.RNTrainerName = dd.RNTrainerName
                modelApp.StartDate = dd.StartDate.ToShortDateString()
                modelApp.OBNNumber = dd.OBNNumber
                returnModelApp.Add(modelApp)
            Next
        Catch ex As Exception
            Throw ex
        End Try

        HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Origin", "*")
        Return returnModelApp
    End Function
End Class
