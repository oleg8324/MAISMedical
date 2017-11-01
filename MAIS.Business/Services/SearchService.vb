Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data

Namespace Services
    Public Interface ISearchService
        Inherits IBusinessBase
        Function GetRNInfoFromTables(ByVal searchCriteria As MAISSearchCriteria, ByVal userRoleID As Integer, ByVal loginUserRNLicense As String, ByVal admin As Boolean, ByVal userID As Integer) As List(Of RNSearchResult)
        Function GetDDInfoFromTables(ByVal searchCriteria As MAISSearchCriteria, ByVal ssnCriteria As Boolean) As List(Of DDPersonnelSearchResult)
        Function GetRRDDAsMoreThanThreeNotation(flag As Boolean, UniqueID As String) As Boolean
    End Interface
    Public Class SearchService
        Inherits BusinessBase
        Implements ISearchService
        Private _queries As Data.Queries.ISearchQueries
        Public Sub New(ByVal user As IUserIdentity, ByVal connectionstring As IConnectionIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of Data.Queries.ISearchQueries)()
            _queries.UserID = user.UserID.ToString()
            _queries.MAISConnectionString = connectionstring.ConnectionString
        End Sub
        Public Function GetDDInfoFromTables(searchCriteria As MAISSearchCriteria, ByVal ssnCriteria As Boolean) As List(Of DDPersonnelSearchResult) Implements ISearchService.GetDDInfoFromTables
            Dim ddInfoValue As List(Of DDPersonnelSearchResult) = Nothing
            Try
                ddInfoValue = Mapping.SearchMapping.MapDBToModelDDSearch(_queries.GetDDInfoFromTables(Mapping.SearchMapping.MapModelToObjects(searchCriteria), ssnCriteria))
            Catch ex As Exception
                Me.LogError("Error Getting DDPersonnel search Info", CInt(Me.UserID), ex)
            End Try
            Return ddInfoValue
        End Function
        Public Function GetRNInfoFromTables(searchCriteria As MAISSearchCriteria, ByVal userRoleID As Integer, ByVal loginUserRNLicense As String, ByVal admin As Boolean, ByVal userID As Integer) As List(Of RNSearchResult) Implements ISearchService.GetRNInfoFromTables
            Dim rnInfoValue As List(Of RNSearchResult) = Nothing
            Try
                rnInfoValue = Mapping.SearchMapping.MapDBToModelRNSearch(_queries.GetRNInfoFromTables(Mapping.SearchMapping.MapModelToObjects(searchCriteria)), userRoleID, loginUserRNLicense, admin, userID)
            Catch ex As Exception
                Me.LogError("Error Getting RN search Info", CInt(Me.UserID), ex)
            End Try
            Return rnInfoValue
        End Function
        Public Function GetRRDDAsMoreThanThreeNotation(flag As Boolean, UniqueID As String) As Boolean Implements ISearchService.GetRRDDAsMoreThanThreeNotation
            Dim flagNotation As Boolean
            Try
                flagNotation = _queries.GetRRDDAsMoreThanThreeNotation(flag, UniqueID)
            Catch ex As Exception
                Me.LogError("Error Getting RN search Info", CInt(Me.UserID), ex)
            End Try
            Return flagNotation
        End Function
    End Class
End Namespace
