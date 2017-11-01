Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
'Imports MAIS.Data
'Imports MAIS.Data.Objects
Imports System.Data.Linq
Imports System.Configuration

Namespace Services
    Public Interface IUserRNMappingService
        Inherits IBusinessBase

        Function GetUserRNMappingByuserID(ByVal iuserID As Integer) As Model.RN_UserMappingDetails
        Function GetUserRNMappingByRN_SID(ByVal iRN_sid As Integer) As Model.RN_UserMappingDetails
        Function IsCurrentUser_RN_ByUserID(ByVal iuserID As Integer) As Boolean

    End Interface
    Public Class UserRNMappingService
        Inherits BusinessBase
        Implements IUserRNMappingService



        Private _queries As Data.Queries.IUserRNMappingQueries

        Public Sub New(ByVal user As IUserIdentity, ByVal maisConnectionString As IConnectionIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of Data.Queries.IUserRNMappingQueries)()
            _queries.UserID = user.UserID
            _queries.MAISConnectionString = maisConnectionString.ConnectionString
        End Sub

        Public Function GetUserRNMappingByRN_SID(iRN_sid As Integer) As RN_UserMappingDetails Implements IUserRNMappingService.GetUserRNMappingByRN_SID
            Dim RetVal As New Model.RN_UserMappingDetails
            Try
                RetVal = Mapping.User_RN_Mapping_Mapping.MappingDBtoModelUserRN_Mapping(_queries.GetUserRNMappingByRN_SID(iRN_sid))

            Catch ex As Exception
                Me.LogError("GetUserRNMappingByRN_SID", CInt(Me.UserID), ex)
                Return RetVal
            End Try
            Return RetVal
        End Function

        Public Function GetUserRNMappingByuserID(iuserID As Integer) As RN_UserMappingDetails Implements IUserRNMappingService.GetUserRNMappingByuserID
            Dim retVal As New Model.RN_UserMappingDetails
            Try
                retVal = Mapping.User_RN_Mapping_Mapping.MappingDBtoModelUserRN_Mapping(_queries.GetUserRNMappingByuserID(iuserID))

            Catch ex As Exception
                Me.LogError("GetUserRNMappingByuserID", CInt(Me.UserID), ex)
                Return retVal
            End Try
            Return retVal

        End Function

        Public Function IsCurrentUser_RN_ByUserID(iuserID As Integer) As Boolean Implements IUserRNMappingService.IsCurrentUser_RN_ByUserID
            Dim RetVal As Boolean = False
            Dim UserRNData As New Model.RN_UserMappingDetails
            Try
                UserRNData = Mapping.User_RN_Mapping_Mapping.MappingDBtoModelUserRN_Mapping(_queries.GetUserRNMappingByuserID(iuserID))
                If Not UserRNData Is Nothing Then
                    If IsNumeric(UserRNData.RN_Sid) Then
                        RetVal = True
                    End If
                End If
            Catch ex As Exception
                Me.LogError("IsCurrentUser_RN_ByUserID", CInt(Me.UserID), ex)
            End Try
            Return RetVal
        End Function
    End Class
End Namespace

