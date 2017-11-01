Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data
Imports MAIS.Data.Objects
Imports System.Data.Linq
Imports System.Configuration

Namespace Services
    Public Interface INotationService
        Inherits IBusinessBase
        Function GetNotations(ByVal code As String, ByVal RN_flg As Boolean) As ReturnObject(Of List(Of Model.NotationDetails))
        Function SaveNotation(ByVal n As Model.NotationDetails, ByVal code As String, ByVal RN_flg As Boolean) As ReturnObject(Of Long)
        Function UpdateNotation(ByVal n As Model.NotationDetails, ByVal nid As Integer) As ReturnObject(Of Long)
        Function GetNotationByNotID(ByVal nid As Integer) As ReturnObject(Of NotationDetails)
        Function GetNotationsByApp(ByVal appid As Integer, ByVal RN_flg As Boolean) As ReturnObject(Of List(Of NotationDetails))
        Function SetAppStatus(ByVal appid As Integer, ByVal appstat As String) As Long
        Function GetNotationTypes() As List(Of Model.NType)
        Function GetNotationReasons() As List(Of Model.NReason)
        Function GetCertStatuses() As List(Of Model.CertStatus)
    End Interface
    Public Class NotationService
        Inherits BusinessBase
        Implements INotationService
        Private _queries As Data.Queries.INotationQueries
        Public Sub New(ByVal user As IUserIdentity, ByVal connectionstring As IConnectionIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of Data.Queries.INotationQueries)()
            _queries.UserID = user.UserID.ToString()
            _queries.MAISConnectionString = connectionstring.ConnectionString
        End Sub
        Public Function GetNotationByNotID(ByVal nid As Integer) As ReturnObject(Of NotationDetails) Implements INotationService.GetNotationByNotID
            Dim retObj As New ReturnObject(Of NotationDetails)
            Try
                retObj.ReturnValue = Mapping.NotationMapping.MapDBToNotation(_queries.GetNotationByNotID(nid))
            Catch ex As Exception
                Me.LogError("Error Notation services by id", CInt(Me.UserID), ex)
                retObj.AddErrorMessage("An error occurred while Notation services by id.")
            End Try

            Return retObj
        End Function
        Public Function GetNotations(ByVal code As String, ByVal RN_flg As Boolean) As ReturnObject(Of List(Of NotationDetails)) Implements INotationService.GetNotations
            Dim retObj As New ReturnObject(Of List(Of NotationDetails))
            Dim DataNotations As List(Of Objects.NotationObject) = Nothing
            Dim ModelNotations As New List(Of NotationDetails)
            Try

                DataNotations = _queries.GetNotations(code, RN_flg)
                For Each no As Objects.NotationObject In DataNotations
                    ModelNotations.Add(Mapping.NotationMapping.MapDBToNotation(no))
                Next
                retObj.ReturnValue = ModelNotations
            Catch ex As Exception
                Me.LogError("Error Notation services", CInt(Me.UserID), ex)
                retObj.AddErrorMessage("An error occurred while Notation services.")
            End Try

            Return retObj
        End Function
        Public Function GetNotationsByApp(ByVal appid As Integer, ByVal RN_flg As Boolean) As ReturnObject(Of List(Of NotationDetails)) Implements INotationService.GetNotationsByApp
            Dim retObj As New ReturnObject(Of List(Of NotationDetails))
            Dim DataNotations As List(Of Objects.NotationObject) = Nothing
            Dim ModelNotations As New List(Of NotationDetails)
            Try
                DataNotations = _queries.GetNotationsByApp(appid, RN_flg)
                For Each no As Objects.NotationObject In DataNotations
                    ModelNotations.Add(Mapping.NotationMapping.MapDBToNotation(no))
                Next
                retObj.ReturnValue = ModelNotations
            Catch ex As Exception
                Me.LogError("Error Notation services by app", CInt(Me.UserID), ex)
                retObj.AddErrorMessage("An error occurred while Notation services by app.")
            End Try

            Return retObj
        End Function
        Public Function SetAppStatus(ByVal appid As Integer, ByVal appstat As String) As Long Implements INotationService.SetAppStatus
            Dim r As Long = -1
            Try
                r = _queries.SetAppStatus(appid, appstat)
            Catch ex As Exception
                Me.LogError("Error Notation services by app status", CInt(Me.UserID), ex)

            End Try
            Return r
        End Function

        Public Function SaveNotation(ByVal n As Model.NotationDetails, ByVal code As String, ByVal RN_flg As Boolean) As ReturnObject(Of Long) Implements INotationService.SaveNotation
            Dim retObj As New ReturnObject(Of Long)(-1L)
            Dim DataNotations As Objects.NotationObject = Nothing
            Dim ModelNotations As NotationDetails = Nothing
            Try
                retObj = _queries.SaveNotation(Mapping.NotationMapping.MapNotationToDB(n), code, RN_flg)
            Catch ex As Exception
                Me.LogError("Error save notation services", CInt(Me.UserID), ex)
                retObj.AddErrorMessage("Error save notation services.")
            End Try
            Return retObj
        End Function
        Public Function UpdateNotation(ByVal n As Model.NotationDetails, ByVal nid As Integer) As ReturnObject(Of Long) Implements INotationService.UpdateNotation
            Dim retObj As New ReturnObject(Of Long)(-1L)
            Try
                retObj = _queries.UpdateNotation(Mapping.NotationMapping.MapNotationToDB(n), nid)
            Catch ex As Exception
                Me.LogError("Error save notation services", CInt(Me.UserID), ex)
                retObj.AddErrorMessage("Error save notation services.")
            End Try
            Return retObj
        End Function
        Public Function GetNotationTypes() As List(Of Model.NType) Implements INotationService.GetNotationTypes
            Dim NotationTypesList As New List(Of Model.NType)
            Try
                NotationTypesList = Mapping.NotationMapping.MapNTypeListToModel(_queries.GetNotationTypes())
            Catch ex As Exception
                Me.LogError("Error Notation services for notation type", CInt(Me.UserID), ex)
            End Try
            Return NotationTypesList
        End Function

        Public Function GetNotationReasons() As List(Of Model.NReason) Implements INotationService.GetNotationReasons
            Dim NotationRList As List(Of Model.NReason) = Nothing
            Try
                NotationRList = Mapping.NotationMapping.MapNReasonListToModel(_queries.GetNotationReasons())
            Catch ex As Exception
                Me.LogError("Error Notation services for notation reason", CInt(Me.UserID), ex)
            End Try
            Return NotationRList
        End Function
        Public Function GetCertStatuses() As List(Of Model.CertStatus) Implements INotationService.GetCertStatuses
            Dim NotationRList As List(Of Model.CertStatus) = Nothing
            Try
                NotationRList = Mapping.NotationMapping.MapCertStatusListToModel(_queries.GetCertStatuses())
            Catch ex As Exception
                Me.LogError("Error Notation services for cert status", CInt(Me.UserID), ex)
            End Try
            Return NotationRList
        End Function
    End Class
End Namespace
