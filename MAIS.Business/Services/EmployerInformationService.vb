Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data
Namespace Services
    Public Interface IEmployerInformationService
        Inherits IBusinessBase
        Function GetRecentlyAddedEmplyerInfo(ByVal applicationID As Integer) As List(Of EmployerInformationDetails)
        Function GetDataRecentlyAddedEmplyerInfo(ByVal employerID As Integer) As EmployerInformationDetails
        Function GetDataHistoryAddedEmplyerInfo(ByVal employerID As Integer, ByVal UniqueCode As String, ByVal RN_Flg As Boolean) As EmployerInformationDetails
        Function SaveEmployerInformation(ByVal employerInfo As Model.EmployerInformationDetails, ByVal applicationID As Integer, ByVal employerId As Integer, ByVal flag As Boolean, ByVal supflag As Boolean) As ReturnObject(Of Long)
        Function GetAddedEmployerFull(ByVal applicationID As Integer) As List(Of EmployerInformationDetails)
        Function DeleteEmployerInfo(ByVal employerId As Integer, ByVal applicationId As Integer) As Boolean
        Function GetEmployerPageComplete(ByVal applicationId As Integer, ByVal RNDDUniqueCode As String) As Integer
        Function GetEmployerInformationFromPerm(ByVal UniqueCode As String, ByVal RN_Flg As Boolean) As List(Of EmployerInformationDetails)
        Function GetEmployerInformationWithAddressFromPerm(ByVal UniqueCode As String, ByVal RN_Flg As Boolean) As List(Of EmployerInformationDetails)
    End Interface

    Public Class EmployerInformationService
        Inherits BusinessBase
        Implements IEmployerInformationService
        Private _queries As Data.Queries.IEmployerInformationQueries
        Public Sub New(ByVal user As IUserIdentity, ByVal connectionstring As IConnectionIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of Data.Queries.IEmployerInformationQueries)()
            _queries.UserID = user.UserID.ToString()
            _queries.MAISConnectionString = connectionstring.ConnectionString
        End Sub
        Public Function GetEmployerInformationFromPerm(ByVal UniqueCode As String, ByVal RN_Flg As Boolean) As List(Of EmployerInformationDetails) Implements IEmployerInformationService.GetEmployerInformationFromPerm
            Dim empInfoList As New List(Of Model.EmployerInformationDetails)
            Try
                empInfoList = Mapping.EmployerInformationMapping.MapDBToPermEmployerInformation(_queries.GetEmployerInformationFromPerm(UniqueCode, RN_Flg))
            Catch ex As Exception
                Me.LogError("Error getting employer information from permanent.", CInt(Me.UserID), ex)
            End Try
            Return empInfoList
        End Function
        Public Function GetRecentlyAddedEmplyerInfo(applicationID As Integer) As List(Of EmployerInformationDetails) Implements IEmployerInformationService.GetRecentlyAddedEmplyerInfo
            Dim listOfRecentEmployer As New List(Of EmployerInformationDetails)
            Try
                listOfRecentEmployer = Mapping.EmployerInformationMapping.MapDBToModelEmployerInfo(_queries.GetRecentlyAddedEmplyeInfo(applicationID))
            Catch ex As Exception
                Me.LogError("Error Getting recently added employer Info", CInt(Me.UserID), ex)
            End Try
            Return listOfRecentEmployer
        End Function
        Public Function GetAddedEmployerFull(ByVal applicationID As Integer) As List(Of EmployerInformationDetails) Implements IEmployerInformationService.GetAddedEmployerFull
            Dim t As List(Of Integer)
            Dim listOfRecentEmployer As New List(Of EmployerInformationDetails)
            Try
                t = _queries.GetAddedEmpIDs(applicationID)
                For Each i As Integer In t
                    listOfRecentEmployer.Add(Mapping.EmployerInformationMapping.MapObjectToModel(_queries.GetDataRecentlyAddedEmplyerInfo(i)))
                Next
            Catch ex As Exception
                Me.LogError("Error in GetAddedEmployerFull", CInt(Me.UserID), ex)
            End Try
            Return listOfRecentEmployer
        End Function
        Public Function SaveEmployerInformation(ByVal employerInfo As Model.EmployerInformationDetails, ByVal applicationID As Integer, ByVal employerId As Integer, ByVal flag As Boolean, supflag As Boolean) As ReturnObject(Of Long) Implements IEmployerInformationService.SaveEmployerInformation
            Dim retObj As New ReturnObject(Of Long)(-1L)
            Dim personalInfoValue As Objects.PersonalInformationDetails = Nothing
            Dim appDetails As MAISApplicationDetails = Nothing
            Try
                retObj = _queries.SaveEmployerInformation(Mapping.EmployerInformationMapping.MapEmployerResultToDB(employerInfo), applicationID, employerId, flag, supflag)
            Catch ex As Exception
                Me.LogError("Error personal information services", CInt(Me.UserID), ex)
                retObj.AddErrorMessage("ex.Message")
            End Try

            Return retObj
        End Function
        Public Function GetDataRecentlyAddedEmplyerInfo(employerID As Integer) As EmployerInformationDetails Implements IEmployerInformationService.GetDataRecentlyAddedEmplyerInfo
            Dim recentEmployer As New EmployerInformationDetails
            Try
                recentEmployer = Mapping.EmployerInformationMapping.MapObjectToModel(_queries.GetDataRecentlyAddedEmplyerInfo(employerID))
            Catch ex As Exception
                Me.LogError("Error Getting details of recently added employer Info", CInt(Me.UserID), ex)

            End Try
            Return recentEmployer
        End Function
        Public Function DeleteEmployerInfo(employerId As Integer, applicationId As Integer) As Boolean Implements IEmployerInformationService.DeleteEmployerInfo
            Dim flag As Boolean = False
            Try
                flag = _queries.DeleteEmployerInfo(employerId, applicationId)
            Catch ex As Exception
                Me.LogError("Error Getting in delete the employer information", CInt(Me.UserID), ex)
            End Try
            Return flag
        End Function
        Public Function GetEmployerPageComplete(applicationId As Integer, ByVal RNDDUniqueCode As String) As Integer Implements IEmployerInformationService.GetEmployerPageComplete
            Dim flag As Integer = 0
            Try
                flag = _queries.GetEmployerPageComplete(applicationId, RNDDUniqueCode)
            Catch ex As Exception
                Me.LogError("Error Getting in employer information for page completion rule.", CInt(Me.UserID), ex)
            End Try
            Return flag
        End Function
        Public Function GetDataHistoryAddedEmplyerInfo(employerID As Integer, ByVal UniqueCode As String, ByVal RN_Flg As Boolean) As EmployerInformationDetails Implements IEmployerInformationService.GetDataHistoryAddedEmplyerInfo
            Dim recentEmployer As New EmployerInformationDetails
            Try
                recentEmployer = Mapping.EmployerInformationMapping.MapObjectToModel(_queries.GetDataHistoryAddedEmplyerInfo(employerID, UniqueCode, RN_Flg))
            Catch ex As Exception
                Me.LogError("Error Getting details of approved employer Info", CInt(Me.UserID), ex)
            End Try
            Return recentEmployer
        End Function

        Public Function GetEmployerInformationWithAddressFromPerm(UniqueCode As String, RN_Flg As Boolean) As List(Of EmployerInformationDetails) Implements IEmployerInformationService.GetEmployerInformationWithAddressFromPerm
            Dim empInfoList As New List(Of Model.EmployerInformationDetails)
            Try
                empInfoList = Mapping.EmployerInformationMapping.MapDBToPermEmployerInformationWithAddress(_queries.GetEmployerInformationWithAddressFromPerm(UniqueCode, RN_Flg))
            Catch ex As Exception
                Me.LogError("Error getting employer information from permanent with Address.", CInt(Me.UserID), ex)
            End Try
            Return empInfoList
        End Function
    End Class
End Namespace
