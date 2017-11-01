Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data
Imports MAIS.Business.Services
Imports MAIS.Data.Queries
Imports MAIS.Business.Model.Enums

Namespace Services
    Public Interface IResourcePageService
        Inherits IBusinessBase
        Function Save_Resource(ByVal iMessage As Model.Resource) As Integer
        Function GetCurrentResource() As List(Of Model.Resource)
        Function GetResourceDataByResourceID(ByVal iMessageID As Integer) As Model.Resource
    End Interface

    Public Class ResourcePageService
        Inherits BusinessBase
        Implements IResourcePageService

        Private _queries As IResourcePageQueries
        Public Sub New(ByVal user As IUserIdentity, ByVal connectionstring As IConnectionIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of IResourcePageQueries)()
            _queries.UserID = user.UserID.ToString()
            _queries.MAISConnectionString = connectionstring.ConnectionString
        End Sub

        Public Function GetCurrentResource() As List(Of Model.Resource) Implements IResourcePageService.GetCurrentResource
            Dim retVal As New List(Of Model.Resource)
            Try
                retVal = Mapping.ResourceMapping.mapDBtoModelResources(_queries.GetCurrentResource())
            Catch ex As Exception
                Me.LogError("Error in GetCurrentResource()", CInt(Me.UserID), ex)
            End Try
            Return retVal
        End Function


        Public Function GetResourceDataByResourceID(iMessageID As Integer) As Model.Resource Implements IResourcePageService.GetResourceDataByResourceID
            Dim retVal As New Model.Resource
            Try
                retVal = Mapping.ResourceMapping.mapDBtoModelResources(_queries.GetResourceDataByResourceID(iMessageID))
            Catch ex As Exception
                Me.LogError("Error in GetCurrentResource()", CInt(Me.UserID), ex)
            End Try
            Return retVal
        End Function

        Public Function Save_Resource(iMessage As Model.Resource) As Integer Implements IResourcePageService.Save_Resource
            Dim retVal As Integer
            Try
                retVal = _queries.Save_Resource(Mapping.ResourceMapping.MapModelToDBResources(iMessage))
            Catch ex As Exception
                Me.LogError("Error in GetCurrentResource()", CInt(Me.UserID), ex)
            End Try
            Return retVal
        End Function
    End Class
End Namespace