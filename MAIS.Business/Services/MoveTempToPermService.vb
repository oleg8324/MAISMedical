Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data
Imports MAIS.Data.Objects
Imports System.Configuration

Namespace Services
    Public Interface IMoveTempToPermService
        Inherits IBusinessBase
        Function SaveTempToPerm(ByVal appId As Integer, ByVal rnflag As Boolean, ByVal currentRCL As Integer, ByVal stDate As Date, ByVal endDate As Date, ByVal statusDesc As String, ByVal adminStatus As String) As ReturnObject(Of Long)
        Function InsertPersonIfNotExists(ByVal appId As Integer, ByVal rnflag As Boolean) As ReturnObject(Of Long)
    End Interface

    Public Class MoveTempToPermService
        Inherits BusinessBase
        Implements IMoveTempToPermService
        Private _queries As Data.Queries.IMoveTempToPermQueries
        Public Sub New(ByVal user As IUserIdentity, ByVal connectionstring As IConnectionIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of Data.Queries.IMoveTempToPermQueries)()
            _queries.UserID = user.UserID.ToString()
            _queries.MAISConnectionString = connectionstring.ConnectionString
        End Sub
        Public Function SaveTempToPerm(ByVal appId As Integer, ByVal rnflag As Boolean, ByVal currentRCL As Integer, ByVal stDate As Date, ByVal endDate As Date, ByVal statusDesc As String, ByVal adminStatus As String) As ReturnObject(Of Long) Implements IMoveTempToPermService.SaveTempToPerm
            Dim rv As New ReturnObject(Of Long)(-1L)
            Try
                rv = _queries.SaveToPerm(appId, rnflag, currentRCL, stDate, endDate, statusDesc, adminStatus)
                '    Throw New Exception
            Catch ex As Exception
                Me.LogError("Error in Save to Permanent Service.", CInt(Me.UserID), ex)
            End Try
            Return rv
        End Function
        Public Function InsertPersonIfNotExists(ByVal appId As Integer, ByVal rnflag As Boolean) As ReturnObject(Of Long) Implements IMoveTempToPermService.InsertPersonIfNotExists
            Dim rv As New ReturnObject(Of Long)(-1L)
            Try
                rv = _queries.InsertPersonIfNotExists(appId, rnflag)
            Catch ex As Exception
                Me.LogError("Error in Save to Permanent Service.", CInt(Me.UserID), ex)
            End Try
            Return rv
        End Function
    End Class
End Namespace
