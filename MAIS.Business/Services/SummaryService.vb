Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data
Imports MAIS.Data.Objects
Imports System.Data.Linq
Imports System.Configuration

Namespace Services
    Public Interface ISummaryService
        Inherits IBusinessBase
        Function GetCurCertificates(ByVal code As String, ByVal rnflag As Boolean) As List(Of Model.Certificate)
        Function GetApplicationStatusSummary(ByVal applicationID As Integer) As Boolean
        Function GetCertificateTime(ByVal srcl As Integer, ByVal apptype As String) As Integer
        Function getRNInfo(ByVal rnsid As Integer) As Model.RNInformationDetails
        Function GetAppStatuses() As List(Of Model.AppStatus)
        Function GetEmailDateInHistory(ByVal applicationID As Integer) As Boolean
        'Function GetNotations(ByVal appId As Integer) As ReturnObject(Of List(Of NotationObject))
        'Function SaveNotation(ByVal n As Model.NotationDetails, ByVal appId As Integer, ByVal sessionId As String) As ReturnObject(Of Long)
    End Interface
    Public Class SummaryService
        Inherits BusinessBase
        Implements ISummaryService
        Private _queries As Data.Queries.ISummaryQueries
        Public Sub New(ByVal user As IUserIdentity, ByVal connectionstring As IConnectionIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of Data.Queries.ISummaryQueries)()
            _queries.UserID = user.UserID.ToString()
            _queries.MAISConnectionString = connectionstring.ConnectionString
        End Sub
        Public Function getRNInfo(ByVal rnsid As Integer) As Model.RNInformationDetails Implements ISummaryService.getRNInfo
            Dim rnInf As Model.RNInformationDetails = Nothing
            Try
                rnInf = Mapping.SummaryMapping.MapDBToModel(_queries.getRNInfo(rnsid))
            Catch ex As Exception
                Me.LogError("Error Getting RN Info.", CInt(Me.UserID), ex)
            End Try
            Return rnInf
        End Function
        Public Function GetCertificateTime(ByVal srcl As Integer, ByVal apptype As String) As Integer Implements ISummaryService.GetCertificateTime
            Dim rv As Integer = 0
            Try
                rv = _queries.GetCertTime(srcl, apptype)
            Catch ex As Exception
                Me.LogError("Error Getting Certificate Time.", CInt(Me.UserID), ex)
            End Try
            Return rv
        End Function
        Public Function GetCurCertificates(ByVal code As String, ByVal rnflag As Boolean) As List(Of Model.Certificate) Implements ISummaryService.GetCurCertificates
            Dim cl As New List(Of Model.Certificate)
            Try
                cl = Mapping.SummaryMapping.MapCertsDBToModel(_queries.GetCurCertificates(code, rnflag))
            Catch ex As Exception
                Me.LogError("Error Getting RN Info.", CInt(Me.UserID), ex)
            End Try
            Return cl
        End Function
        Public Function GetAppStatuses() As List(Of Model.AppStatus) Implements ISummaryService.GetAppStatuses
            Dim aslist As New List(Of Model.AppStatus)
            Try
                aslist = Mapping.SummaryMapping.MapASListToModel(_queries.GetAppStatuses())
            Catch ex As Exception
                Me.LogError("Error Notation services", CInt(Me.UserID), ex)
            End Try
            Return aslist
        End Function
        Public Function GetApplicationStatusSummary(applicationID As Integer) As Boolean Implements ISummaryService.GetApplicationStatusSummary
            Dim flag As Boolean = False
            Try
                flag = _queries.GetApplicationStatusSummary(applicationID)
            Catch ex As Exception
                Me.LogError("Error Getting summary page complete rule.", CInt(Me.UserID), ex)
            End Try
            Return flag
        End Function

        Public Function GetEmailDateInHistory(applicationID As Integer) As Boolean Implements ISummaryService.GetEmailDateInHistory
            Dim flag As Boolean = False
            Try
                flag = _queries.GetEmailDateInHistory(applicationID)
            Catch ex As Exception
                Me.LogError("Error Getting summary page for email date in history.", CInt(Me.UserID), ex)
            End Try
            Return flag
        End Function
    End Class
End Namespace
