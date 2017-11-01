Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data
Imports MAIS.Data.Objects
Imports System.Data.Linq
Imports System.Configuration

Namespace Services
    Public Interface IApplicationHistoryService
        Inherits IBusinessBase
        Function GetSearchResults(ByVal searchCriteria As Model.ApplicationHistorySearchCriteria) As List(Of Model.ApplicationHistoryModel)
        Function GetApplicationStatusDetail(ByVal ApplicationID As Integer) As List(Of Model.ApplicationHistoryStatusDetail)
    End Interface
    Public Class ApplicationHistoryService
        Inherits BusinessBase
        Implements IApplicationHistoryService
        Dim _queries As Data.Queries.IApplicationHistoryQueries

        Public Sub New(ByVal user As IUserIdentity, ByVal connectionstring As IConnectionIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of Data.Queries.IApplicationHistoryQueries)()
            _queries.UserID = user.UserID.ToString()
            _queries.MAISConnectionString = connectionstring.ConnectionString
        End Sub

        Public Function GetSearchResults(searchCriteria As Model.ApplicationHistorySearchCriteria) As List(Of ApplicationHistoryModel) Implements IApplicationHistoryService.GetSearchResults
            Dim ModelApplicationDetailInformation As New List(Of ApplicationHistoryModel)
            Try
                ModelApplicationDetailInformation = Mapping.ApplicationHistoryMapping.MapToObjectApplicationInformationDetails(_queries.GetSearchResults(Mapping.ApplicationHistoryMapping.MapToModelApplicationInformationDetails(searchCriteria)))
            Catch ex As Exception
                Me.LogError("Error Getting in search results for application history.", CInt(Me.UserID), ex)
            End Try
            Return ModelApplicationDetailInformation
        End Function

        Public Function GetApplicationStatusDetail(ApplicationID As Integer) As List(Of Model.ApplicationHistoryStatusDetail) Implements IApplicationHistoryService.GetApplicationStatusDetail
            Dim ModelApplicationDetailInformation As New List(Of Model.ApplicationHistoryStatusDetail)
            Try
                ModelApplicationDetailInformation = Mapping.ApplicationHistoryMapping.MapToObjectApplicationHistoryDetails(_queries.GetApplicationStatusDetail(ApplicationID))
            Catch ex As Exception
                Me.LogError("Error Getting in search results for application status detail history.", CInt(Me.UserID), ex)
            End Try
            Return ModelApplicationDetailInformation
        End Function
    End Class
End Namespace
