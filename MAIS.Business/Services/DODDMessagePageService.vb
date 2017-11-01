Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data
Imports MAIS.Business.Services
Imports MAIS.Data.Queries
Imports MAIS.Business.Model.Enums

Namespace Services
    Public Interface IDODDMessagePageService
        Inherits IBusinessBase
        Function Save_DODDMessage(ByVal iMessage As Model.DODDMessageInfo) As Integer
        Function GetCurrentMessage() As List(Of Model.DODDMessageInfo)
        Function GetMessageDataByMessageID(ByVal iMessageID As Integer) As Model.DODDMessageInfo
        Function GetMessageDataByUserRoles(ByVal iRoleList As String, ByVal UserID As Integer) As List(Of Model.DODDMessageInfo)
        Function GetMessageDataArchivedDataByUserRolesAndPersionID(ByVal iRoleList As String, ByVal UserID As Integer) As List(Of Model.DODDMessageInfo)
        Function SearchMessageDataByDates(ByVal iStartDate As Date, ByVal iEndDate As Date) As List(Of Model.DODDMessageInfo)
    End Interface

    Public Class DODDMessagePageService
        Inherits BusinessBase
        Implements IDODDMessagePageService

        Private _queries As IDODDMessagePageQueries
        Public Sub New(ByVal user As IUserIdentity, ByVal connectionstring As IConnectionIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of IDODDMessagePageQueries)()
            _queries.UserID = user.UserID.ToString()
            _queries.MAISConnectionString = connectionstring.ConnectionString
        End Sub

        Public Function Save_DODDMessage(iMessage As DODDMessageInfo) As Integer Implements IDODDMessagePageService.Save_DODDMessage
            Dim retVal As Integer
            Try
                retVal = _queries.Save_DODDMessage(Mapping.DODDMessageMapping.mapDODDMessageToDB(iMessage))
                Return retVal
            Catch ex As Exception
                Return -1
            End Try
        End Function

        Public Function GetCurrentMessage() As List(Of DODDMessageInfo) Implements IDODDMessagePageService.GetCurrentMessage
            Dim retVal As New List(Of DODDMessageInfo)
            Try
                retVal = Mapping.DODDMessageMapping.mapDBtoDODDMessage(_queries.GetCurrentMessage())
            Catch ex As Exception
                Me.LogError("Error in GetCurrentMessage()", CInt(Me.UserID), ex)
            End Try
            Return retVal

        End Function

        Public Function GetMessageDataByMessageID(iMessageID As Integer) As DODDMessageInfo Implements IDODDMessagePageService.GetMessageDataByMessageID
            Dim retVal As New Model.DODDMessageInfo
            Try
                retVal = Mapping.DODDMessageMapping.mapDBtoDODDMessage(_queries.GetMessageDataByMessageID(iMessageID))
            Catch ex As Exception
                Me.LogError("Error in GetMessageDataByMessageID", CInt(Me.UserID), ex)
            End Try
            Return retVal

        End Function

        Public Function GetMessageDataByUserRoles(iRoleList As String, UserID As Integer) As List(Of DODDMessageInfo) Implements IDODDMessagePageService.GetMessageDataByUserRoles
            Dim retVal As New List(Of Model.DODDMessageInfo)
            Try
                retVal = Mapping.DODDMessageMapping.mapDBtoDODDMessage(_queries.GetMessageDataByUserRoles(iRoleList, UserID))
            Catch ex As Exception
                Me.LogError("Error in GetMessageDataByUserRoles", CInt(Me.UserID), ex)
            End Try
            Return retVal

        End Function

        Public Function GetMessageDataArchivedDataByUserRolesAndPersionID(iRoleList As String, UserID As Integer) As List(Of DODDMessageInfo) Implements IDODDMessagePageService.GetMessageDataArchivedDataByUserRolesAndPersionID
            Dim retVal As New List(Of Model.DODDMessageInfo)
            Try
                retVal = Mapping.DODDMessageMapping.mapDBtoDODDMessage(_queries.GetMessageDataArchivedDataByUserRolesAndPersionID(iRoleList, UserID))
            Catch ex As Exception
                Me.LogError("Error in GetMessageDataArchivedDataByUserRolesAndPersionID", CInt(Me.UserID), ex)
            End Try
            Return retVal

        End Function

        Public Function SearchMessageDataByDates(iStartDate As Date, iEndDate As Date) As List(Of DODDMessageInfo) Implements IDODDMessagePageService.SearchMessageDataByDates
            Dim retVal As New List(Of Model.DODDMessageInfo)
            Try
                retVal = Mapping.DODDMessageMapping.mapDBtoDODDMessage(_queries.SearchMessageDataByDates(iStartDate, iEndDate))
            Catch ex As Exception
                Me.LogError("Error in SearchMessageDataByDates", CInt(Me.UserID), ex)
            End Try
            Return retVal
        End Function
    End Class
End Namespace

