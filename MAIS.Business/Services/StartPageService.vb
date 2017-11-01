Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data
Imports MAIS.Business.Services
Imports MAIS.Data.Queries
Imports MAIS.Business.Model.Enums

Namespace Services
    Public Interface IStartPageService
        Inherits IBusinessBase
        Function GetCertificationDetails(ByVal role As Integer, ByVal currentRole As Integer, ByVal applicationType As String) As Model.CertificationEligibleInfo
        Function SaveAppInfo(ByVal appDetails As MAISApplicationDetails) As Integer
        Function GetApplicationInformation(ByVal applicationID As Integer) As Model.MAISApplicationDetails
    End Interface
End Namespace
Public Class StartPageService
    Inherits BusinessBase
    Implements IStartPageService
    Private _queries As IStartPageQueries
    Public Sub New(ByVal user As IUserIdentity, ByVal connectionstring As IConnectionIdentity)
        _queries = StructureMap.ObjectFactory.GetInstance(Of IStartPageQueries)()
        _queries.UserID = user.UserID.ToString()
        _queries.MAISConnectionString = connectionstring.ConnectionString
    End Sub
    Public Function GetCertificationDetails(role As Integer, currentRole As Integer, ByVal applicationType As String) As Model.CertificationEligibleInfo Implements Services.IStartPageService.GetCertificationDetails
        Dim certificationDetails As New Model.CertificationEligibleInfo
        Try
            certificationDetails = Mapping.StartPageMapping.MapDBToModelStartpageInfo(_queries.GetCertificationDetails(role, currentRole, applicationType))
        Catch ex As Exception
            Me.LogError("Error Getting certification details Info", CInt(Me.UserID), ex)
        End Try
        Return certificationDetails
    End Function

    Public Function SaveAppInfo(ByVal appDetails As MAISApplicationDetails) As Integer Implements IStartPageService.SaveAppInfo
        Dim retObj As New Integer
        Try
            retObj = _queries.SaveAppInfo(Mapping.StartPageMapping.MapDBToModelStartpageToSaveInfo(appDetails))

        Catch ex As Exception
            Me.LogError("Error in saving application information", CInt(Me.UserID), ex)
        End Try
        Return retObj
    End Function

    Public Function GetApplicationInformation(applicationID As Integer) As MAISApplicationDetails Implements IStartPageService.GetApplicationInformation
        Dim appDetails As New Model.MAISApplicationDetails
        Try
            appDetails = Mapping.StartPageMapping.MapDBToModelObjectInfo(_queries.GetApplicationInformation(applicationID))
        Catch ex As Exception
            Me.LogError("Error Getting certification details Info", CInt(Me.UserID), ex)
        End Try
        Return appDetails
    End Function
End Class
