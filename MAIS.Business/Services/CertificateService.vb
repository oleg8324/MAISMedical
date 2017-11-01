Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data
Imports MAIS.Data.Objects
Imports System.Data.Linq
Imports System.Configuration
Imports ODMRDD_NET2

Namespace Services
    Public Interface ICertificateService
        Inherits IBusinessBase

        Function GetCertificateInfo(ByVal rnLicenseNumber As String, ByVal applicationType As String, ByVal role As Integer) As List(Of CertificateInfo)
        Function GetDDCertificateInfo(rnLicenseNumber As String, applicationType As String, ByVal role As Integer) As List(Of CertificateDDInfo)
        Function GetCertificateDetailsInfo(rnLicenseNumberDD As String, rnDD As Boolean) As List(Of Model.CertificationDetails)
        Function GetCertificationDateInHistory(ByVal applicationID As Integer) As Boolean
        Function GetCertificateInfoUsingCertMod(ByVal rnLicenseNumber As String, ByVal applicationType As String, ByVal role As Integer, ByVal certID As Integer) As List(Of CertificateInfo)
        Function GetCertificateDDInfoUsingCertMod(ByVal ddCode As String, ByVal applicationType As String, ByVal role As Integer, ByVal certID As Integer) As List(Of CertificateDDInfo)
    End Interface

    Public Class CertificateService
        Inherits BusinessBase
        Implements ICertificateService

        Private _queries As Data.Queries.ICertificateQueries
        Public Sub New(ByVal user As IUserIdentity, ByVal connectionstring As IConnectionIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of Data.Queries.ICertificateQueries)()
            _queries.UserID = user.UserID.ToString()
            _queries.MAISConnectionString = connectionstring.ConnectionString
        End Sub
       
        Public Function GetCertificateInfo(rnLicenseNumber As String, applicationType As String, ByVal role As Integer) As List(Of CertificateInfo) Implements ICertificateService.GetCertificateInfo
            Dim certInfoList As New List(Of Model.CertificateInfo)
            Try
                certInfoList = Mapping.CertificateMapping.MapDBToPermCertInformation(_queries.GetCertificateInfo(rnLicenseNumber, applicationType, role), applicationType)
            Catch ex As Exception
                Me.LogError("Error getting employer information from permanent.", CInt(Me.UserID), ex)
            End Try
            Return certInfoList
        End Function
        Public Function GetDDCertificateInfo(rnLicenseNumber As String, applicationType As String, ByVal role As Integer) As List(Of CertificateDDInfo) Implements ICertificateService.GetDDCertificateInfo
            Dim certInfoList As New List(Of Model.CertificateDDInfo)
            Try
                certInfoList = Mapping.CertificateMapping.MapDBToPermDDCertInformation(_queries.GetCertificateDDInfo(rnLicenseNumber, applicationType, role), applicationType)
            Catch ex As Exception
                Me.LogError("Error getting employer information from permanent.", CInt(Me.UserID), ex)
            End Try
            Return certInfoList
        End Function

        Public Function GetCertificateDetailsInfo(rnLicenseNumberDD As String, rnDD As Boolean) As List(Of Model.CertificationDetails) Implements ICertificateService.GetCertificateDetailsInfo
            Dim certInfoList As New List(Of Model.CertificationDetails)
            Try
                certInfoList = Mapping.CertificateMapping.MapDBToPermCertDetailsInformation(_queries.GetCertificateDetialsInfo(rnLicenseNumberDD, rnDD))
            Catch ex As Exception
                Me.LogError("Error getting employer information from permanent.", CInt(Me.UserID), ex)
            End Try
            Return certInfoList
        End Function

        Public Function GetCertificationDateInHistory(applicationID As Integer) As Boolean Implements ICertificateService.GetCertificationDateInHistory
            Dim flag As Boolean = False
            Try
                flag = _queries.GetCertificationDateInHistory(applicationID)
            Catch ex As Exception
                Me.LogError("Error Getting summary page for certificate date in history.", CInt(Me.UserID), ex)
            End Try
            Return flag
        End Function

        Public Function GetCertificateDDInfoUsingCertMod(ddCode As String, applicationType As String, role As Integer, certID As Integer) As List(Of CertificateDDInfo) Implements ICertificateService.GetCertificateDDInfoUsingCertMod
            Dim certInfoList As New List(Of Model.CertificateDDInfo)
            Try
                certInfoList = Mapping.CertificateMapping.MapDBToPermDDCertInformation(_queries.GetCertificateDDInfoUsingCertMod(ddCode, applicationType, role, certID), applicationType)
            Catch ex As Exception
                Me.LogError("Error getting employer information from permanent for certification modification.", CInt(Me.UserID), ex)
            End Try
            Return certInfoList
        End Function

        Public Function GetCertificateInfoUsingCertMod(rnLicenseNumber As String, applicationType As String, role As Integer, certID As Integer) As List(Of CertificateInfo) Implements ICertificateService.GetCertificateInfoUsingCertMod
            Dim certInfoList As New List(Of Model.CertificateInfo)
            Try
                certInfoList = Mapping.CertificateMapping.MapDBToPermCertInformation(_queries.GetCertificateInfoUsingCertMod(rnLicenseNumber, applicationType, role, certID), applicationType)
            Catch ex As Exception
                Me.LogError("Error getting employer information from permanent for certification modification.", CInt(Me.UserID), ex)
            End Try
            Return certInfoList
        End Function
    End Class
End Namespace
