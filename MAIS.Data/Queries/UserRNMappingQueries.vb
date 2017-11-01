Imports System.Data.Linq
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Data.Objects

Namespace Queries
    Public Interface IUserRNMappingQueries
        Inherits IQueriesBase
        Function GetUserRNMappingByuserID(ByVal iuserID As Integer) As Data.User_RN_Mapping
        Function GetUserRNMappingByRN_SID(ByVal iRN_sid As Integer) As Data.User_RN_Mapping

    End Interface
    Public Class UserRNMappingQueries
        Inherits QueriesBase
        Implements IUserRNMappingQueries

        Public Function GetUserRNMappingByRN_SID(iRN_sid As Integer) As User_RN_Mapping Implements IUserRNMappingQueries.GetUserRNMappingByRN_SID
            Dim userRn As New User_RN_Mapping
            Try
                Using context As New MAISContext
                    userRn = (From a In context.User_RN_Mapping _
                              Where a.RN_Sid = iRN_sid _
                              Select a).FirstOrDefault

                End Using
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error in GetUserRNMappingByRN_SID.", True, False))
                Me.LogError("Error in GetUserRNMappingByRN_SID.", CInt(Me.UserID), ex)
            End Try
            Return userRn
        End Function

        Public Function GetUserRNMappingByuserID(iuserID As Integer) As User_RN_Mapping Implements IUserRNMappingQueries.GetUserRNMappingByuserID
            Dim userRn As New Data.User_RN_Mapping
            Try
                Using context As New MAISContext
                    userRn = (From a In context.User_RN_Mapping _
                              Where a.UserID = iuserID _
                              Select a).FirstOrDefault

                End Using
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error in GetUserRNMappingByuserID.", True, False))
                Me.LogError("Error in GetUserRNMappingByuserID.", CInt(Me.UserID), ex)
            End Try
            Return userRn
        End Function
    End Class
End Namespace

