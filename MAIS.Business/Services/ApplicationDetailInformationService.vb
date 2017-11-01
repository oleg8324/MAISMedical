Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data
Imports MAIS.Data.Objects
Imports System.Data.Linq
Imports System.Configuration

Namespace Services
    Public Interface IApplicationDetailInformationService
        Inherits IBusinessBase

        Function GetApplicationInfromationByAppID(ByVal appid As Integer) As Model.ApplicationInformationDetails
        Function UpdateApplicationSignature(ByVal AppID As Integer, ByVal Signature As String, ByVal CurrentUserID As Integer, ByVal RNID As Integer, Optional IsAddmin As Boolean = False) As Boolean
        Function GetRNLicenseIssuenceDateByAppID(ByVal AppID As Integer) As DateTime

    End Interface

    Public Class ApplicationDetailInformationService
        Inherits BusinessBase
        Implements IApplicationDetailInformationService


        Dim _queries As Data.Queries.IApplicationDetailInformationQueires

        Public Sub New(ByVal user As IUserIdentity, ByVal connectionstring As IConnectionIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of Data.Queries.IApplicationDetailInformationQueires)()
            _queries.UserID = user.UserID.ToString()
            _queries.MAISConnectionString = connectionstring.ConnectionString
        End Sub

        Public Function GetRNLicenseIssuenceDateByAppID(ByVal AppID As Integer) As DateTime Implements IApplicationDetailInformationService.GetRNLicenseIssuenceDateByAppID
            Dim rnIssueDate As DateTime
            Try
                rnIssueDate = _queries.GetRNLicenseIssuenceDateByAppID(AppID)
            Catch ex As Exception
                Me.LogError("Error fetching RN license issuance date.", CInt(Me.UserID), ex)
            End Try
            Return rnIssueDate
        End Function
        Public Function GetApplicationInfromationByAppID(appid As Integer) As Model.ApplicationInformationDetails Implements IApplicationDetailInformationService.GetApplicationInfromationByAppID
            Dim ModelApplicationDetailInformation As New Model.ApplicationInformationDetails
            Try
                ModelApplicationDetailInformation = Mapping.ApplicationInformationDetailsMapping.MapToModelApplicationInformationDetails(_queries.GetApplicationInformationDetailsByAppID(appid))
            Catch ex As Exception
                Me.LogError("Error fetching application information by application id.", CInt(Me.UserID), ex)
            End Try
            Return ModelApplicationDetailInformation
        End Function

        Public Function UpdateApplicationSignature(AppID As Integer, Signature As String, ByVal CurrentUserID As Integer, ByVal RNID As Integer, Optional IsAddmin As Boolean = False) As Boolean Implements IApplicationDetailInformationService.UpdateApplicationSignature
            Try
                Return _queries.UpdateApplicationSignature(AppID, Signature, CurrentUserID, RNID, IsAddmin)

            Catch ex As Exception
                Return False
                Me.LogError("Error updating application signature.", CInt(Me.UserID), ex)
            End Try
        End Function
    End Class
End Namespace