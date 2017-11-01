Imports MAIS.Data.Queries
Imports MAIS.Business.Infrastructure

Namespace Services
    Public Interface IMAISDODDService
        Function GetRNData(ByVal lastName As String, ByVal firstname As String, ByVal RnlicenseNumber As String) As List(Of Models.RNInformationDetailsForWS)
        Function GetDDData(ByVal DDpersonnelCode As String, ByVal SSN As Integer?, ByVal lastname As String, ByVal firstname As String, ByVal EmployerName As String) As List(Of Models.DDDetailInformation)
        Function GetTrainingSessionData(ByVal sessionStartDate As Date?, ByVal sessionEndDate As Date?, ByVal county As String, ByVal lastName As String, ByVal firstname As String, ByVal rnsession As Boolean) As List(Of Models.TrainingSessionResults)
    End Interface

    Public Class MAISDODDService
        Inherits BusinessBase
        Implements IMAISDODDService

        Dim _maisQueries As MAISDODDQueries
        Public Sub New(ByVal user As String)
            Me.UserID = user
            _maisQueries = New MAISDODDQueries(user)
        End Sub

        Public Function GetDDData(DDpersonnelCode As String, SSN As Integer?, lastname As String, firstname As String, EmployerName As String) As List(Of Models.DDDetailInformation) Implements IMAISDODDService.GetDDData
            Dim modelsDD As New List(Of Models.DDDetailInformation)
            Try
                modelsDD = Mapping.MAISDODDMapping.MapObjectsToDDModels(_maisQueries.GetDDData(DDpersonnelCode, SSN, lastname, firstname, EmployerName))
            Catch ex As Exception
                Me.LogError("Error GetDDData.", CInt(Me.UserID), ex)
            End Try
            Return modelsDD
        End Function

        Public Function GetRNData(lastName As String, firstname As String, RnlicenseNumber As String) As List(Of Models.RNInformationDetailsForWS) Implements IMAISDODDService.GetRNData
            Dim modelsRN As New List(Of Models.RNInformationDetailsForWS)
            Try
                modelsRN = Mapping.MAISDODDMapping.MapObjectsToRNModels(_maisQueries.GetRNData(lastName, firstname, RnlicenseNumber))
            Catch ex As Exception
                Me.LogError("Error GetRNData.", CInt(Me.UserID), ex)
            End Try
            Return modelsRN
        End Function

        Public Function GetTrainingSessionData(sessionStartDate As Date?, sessionEndDate As Date?, county As String, lastName As String, firstname As String, rnsession As Boolean) As List(Of Models.TrainingSessionResults) Implements IMAISDODDService.GetTrainingSessionData
            Dim modelsTraining As New List(Of Models.TrainingSessionResults)
            Try
                modelsTraining = Mapping.MAISDODDMapping.MapObjectsToTrainingModels(_maisQueries.GetTrainingSessionData(sessionStartDate, sessionEndDate, county, lastName, firstname, rnsession))
            Catch ex As Exception
                Me.LogError("Error GetTrainingSessionData.", CInt(Me.UserID), ex)
            End Try
            Return modelsTraining
        End Function
    End Class
End Namespace
