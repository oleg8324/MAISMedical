Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
'Imports MAIS.Data
'Imports MAIS.Data.Objects
'Imports System.Data.Linq
Imports System.Configuration

Namespace Services
    Public Interface IUserRNDetailService
        Inherits IBusinessBase
        Function getAllRNDetails() As List(Of Model.RN_UserDetails)
        Function GetRNDetailsByRNLicenseNumber(ByVal RNLicenseNumber As String) As RN_UserDetails
        Function GetAllDDPersonnel() As List(Of Model.DD_Personnel)
        Function GetRNDetailsWithEmailsByRN_Sid(ByVal RN_Sid As Integer) As RN_UserDetails
        Function GetRNDetailsWithEmailsByRoleID(ByVal RoleIDList As String) As List(Of Model.RN_UserDetails)
    End Interface
    Public Class UserRNDetailService
        Inherits BusinessBase
        Implements IUserRNDetailService

        Private _queries As Data.Queries.IUserRNDetailQueries


        <Obsolete("Use StructureMap.objectFactory.Getinstance(Of IUserRNDetailService)() instead!", True)> _
        Public Sub New()
            Throw New NotImplementedException("Method not usable. User StructureMap.ObjectFactory.GetInstance(of IUserRNDetailService)() instead!")
        End Sub

        Public Sub New(ByVal user As IUserIdentity, ByVal maisConnectionString As IConnectionIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of Data.Queries.IUserRNDetailQueries)()
            _queries.UserID = user.UserID
            _queries.MAISConnectionString = maisConnectionString.ConnectionString
        End Sub

        Public Function getAllRNDetails() As List(Of RN_UserDetails) Implements IUserRNDetailService.getAllRNDetails

            Dim retValModleRNUserDetails As New List(Of RN_UserDetails)

            Try
                retValModleRNUserDetails = Mapping.RN_DetailsMapping.MappingToModelRn_userList(_queries.getAllRNDetails())
            Catch ex As Exception
                If Me._queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(Me._queries.Messages)
                    For Each message In Me._queries.Messages
                        If message.IsError Then
                            Me.LogError(message.ToString(), CInt(Me.UserID), ex)
                        ElseIf message.IsWarning Then
                            Me.LogWarning(message.ToString(), ex)

                        End If

                    Next
                    Me._queries.Messages.Clear()
                Else
                    Me._messages.AddRange(New ODMRDDHelperClassLibrary.Utility.ReturnMessage("Error while populating Application Attestation Entity for Attestation mapping.", True, False))
                    Me.LogError("Error while populating Application Attestation Entity for Attestation mapping.", CInt(Me.UserID), ex)
                End If
            End Try
            Return retValModleRNUserDetails

        End Function

        Public Function GetRNDetailsByRNLicenseNumber(RNLicenseNumber As String) As RN_UserDetails Implements IUserRNDetailService.GetRNDetailsByRNLicenseNumber
            Dim retValModleRNUserDetails As New RN_UserDetails

            Try
                retValModleRNUserDetails = Mapping.RN_DetailsMapping.MappingToModelRN_User(_queries.GetRNDetailsByRNLicenseNumber(RNLicenseNumber))
            Catch ex As Exception
                If Me._queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(Me._queries.Messages)
                    For Each message In Me._queries.Messages
                        If message.IsError Then
                            Me.LogError(message.ToString(), CInt(Me.UserID), ex)
                        ElseIf message.IsWarning Then
                            Me.LogWarning(message.ToString(), ex)

                        End If

                    Next
                    Me._queries.Messages.Clear()
                Else
                    Me._messages.AddRange(New ODMRDDHelperClassLibrary.Utility.ReturnMessage("Error while populating Application Attestation Entity for Attestation mapping.", True, False))
                    Me.LogError("Error while populating Application Attestation GetRNDetailsByRNLicenseNumber.", CInt(Me.UserID), ex)
                End If
            End Try
            Return retValModleRNUserDetails
        End Function

        Public Function GetAllDDPersonnel() As List(Of DD_Personnel) Implements IUserRNDetailService.GetAllDDPersonnel
            Dim retLst As New List(Of DD_Personnel)
            Try
                retLst = Mapping.RN_DetailsMapping.Map_DDPersonnelList(_queries.GetAllDDPersonnel())
            Catch ex As Exception
                Me.LogError("Error getting ALL DDPersonnels.", CInt(Me.UserID), ex)
            End Try
            Return retLst
        End Function

        Public Function GetRNDetailsWithEmailsByRN_Sid(RN_Sid As Integer) As RN_UserDetails Implements IUserRNDetailService.GetRNDetailsWithEmailsByRN_Sid
            Dim retVal As New Model.RN_UserDetails
            Try
                retVal = Mapping.RN_DetailsMapping.Map_DBtoRN_UserDetailsWithEmail(_queries.GetRNDetailsWithEmailsByRN_Sid(RN_Sid))

            Catch ex As Exception
                Me.LogError("Error getting RN_UserDetails by RN_Sid.", CInt(Me.UserID), ex)
            End Try
            Return retVal
        End Function

        Public Function GetRNDetailsWithEmailsByRoleID(RoleIDList As String) As List(Of RN_UserDetails) Implements IUserRNDetailService.GetRNDetailsWithEmailsByRoleID
            Dim retVal As New List(Of Model.RN_UserDetails)

            Try
                retVal = Mapping.RN_DetailsMapping.Map_DBtoRN_UserDetailsWithEmail(_queries.GetRNDetailsWithEmailsByRoleID(RoleIDList))

            Catch ex As Exception
                Me.LogError("Error getting RN_UserDetailsWithEmail by role IDs.", CInt(Me.UserID), ex)
            End Try
            Return retVal
        End Function
    End Class
End Namespace

