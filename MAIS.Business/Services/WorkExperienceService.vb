Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model

Namespace Services
    Public Interface IWorkExperienceService
        Inherits IBusinessBase
        Function SaveWorkExperience(ByVal workInfo As Model.WorkExperienceDetails) As ReturnObject(Of Long)
        Function GetAddedWorkExpList(ByVal applicationID As Integer) As List(Of WorkExperienceDetails)
        Function GetWorkExperienceByID(ByVal workID As Integer) As WorkExperienceDetails
        Function GetExistingWorkExperience(ByVal RNLicense As String) As List(Of WorkExperienceDetails)
        Function GetAddedExpFullList(ByVal applicationID As Integer) As List(Of WorkExperienceDetails)
        Function DeleteWorkExperienceByID(ByVal workID As Integer) As ReturnObject(Of Boolean)
        Function GetDDExperienceFlg(ByVal RNDDUnique_Code As String, ByVal maisAppID As Integer) As RN_DD_Flags
        Function GetExperience(ByVal UniqueCode As String, ByVal AppID As Integer) As Integer
    End Interface
    Public Class WorkExperienceService
        Inherits BusinessBase
        Implements IWorkExperienceService
        Private _queries As Data.Queries.IWorkExperienceQueries
        Public Sub New(ByVal user As IUserIdentity, ByVal connectionstring As IConnectionIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of Data.Queries.IWorkExperienceQueries)()
            _queries.UserID = user.UserID.ToString()
            _queries.MAISConnectionString = connectionstring.ConnectionString
        End Sub
        Public Function GetDDExperienceFlg(ByVal RNDDUnique_Code As String, ByVal maisAppID As Integer) As RN_DD_Flags Implements IWorkExperienceService.GetDDExperienceFlg
            Dim retObj As New RN_DD_Flags
            Try
                retObj = Mapping.WorkExperienceMapping.MapFlags(_queries.GetDDExperienceFlg(RNDDUnique_Code, maisAppID))
            Catch ex As Exception
                Me.LogError("Error getting DD experience flag.", CInt(Me.UserID), ex)
            End Try
            Return retObj
        End Function
        Public Function GetExperience(ByVal UniqueCode As String, ByVal AppID As Integer) As Integer Implements IWorkExperienceService.GetExperience
            Dim retExp As Integer = 0
            Try
                retExp = _queries.GetExperience(UniqueCode, AppID)
            Catch ex As Exception
                Me.LogError("Error getting RN work experience count.", CInt(Me.UserID), ex)
            End Try
            Return retExp
        End Function
        Public Function DeleteWorkExperienceByID(ByVal workID As Integer) As ReturnObject(Of Boolean) Implements IWorkExperienceService.DeleteWorkExperienceByID
            Dim retObj As New ReturnObject(Of Boolean)(False)
            Try
                retObj = _queries.DeleteWorkExperienceByID(workID)
            Catch ex As Exception
                Me.LogError("Error deleting Currently Added Work Experience.", CInt(Me.UserID), ex)
                retObj.AddErrorMessage("ex.Message")
            End Try
            Return retObj
        End Function
        Public Function SaveWorkExperience(ByVal workInfo As Model.WorkExperienceDetails) As ReturnObject(Of Long) Implements IWorkExperienceService.SaveWorkExperience
            Dim retObj As New ReturnObject(Of Long)(-1L)
            Try
                retObj = _queries.SaveWorkExperience(Mapping.WorkExperienceMapping.MapWorkResultToDB(workInfo))
            Catch ex As Exception
                Me.LogError("Error work experience information services", CInt(Me.UserID), ex)
                retObj.AddErrorMessage("ex.Message")
            End Try
            Return retObj
        End Function
        Public Function GetAddedWorkExpList(applicationID As Integer) As List(Of WorkExperienceDetails) Implements IWorkExperienceService.GetAddedWorkExpList
            Dim listCurrentWorkExp As New List(Of WorkExperienceDetails)
            Try
                listCurrentWorkExp = Mapping.WorkExperienceMapping.MapDBToModelWorkExp(_queries.GetAddedWorkExpList(applicationID))
            Catch ex As Exception
                Me.LogError("Error Getting Added Work Experience List", CInt(Me.UserID), ex)
            End Try
            Return listCurrentWorkExp
        End Function
        Public Function GetWorkExperienceByID(workID As Integer) As WorkExperienceDetails Implements IWorkExperienceService.GetWorkExperienceByID
            Dim WorkExpInfo As New WorkExperienceDetails
            Try
                WorkExpInfo = Mapping.WorkExperienceMapping.MapDBToWorkResult(_queries.GetWorkExperienceByID(workID))
            Catch ex As Exception
                Me.LogError("Error Getting Added Work Experience List", CInt(Me.UserID), ex)
            End Try
            Return WorkExpInfo
        End Function
        Public Function GetExistingWorkExperience(RNLicense As String) As List(Of WorkExperienceDetails) Implements IWorkExperienceService.GetExistingWorkExperience
            Dim listExistingWorkExp As New List(Of WorkExperienceDetails)
            Try
                listExistingWorkExp = Mapping.WorkExperienceMapping.MapDBToModelWorkExp(_queries.GetExistingWorkExperience(RNLicense))
            Catch ex As Exception
                Me.LogError("Error Getting Existing Work Experience List", CInt(Me.UserID), ex)
            End Try
            Return listExistingWorkExp
        End Function
        Public Function GetAddedExpFullList(ByVal applicationID As Integer) As List(Of WorkExperienceDetails) Implements IWorkExperienceService.GetAddedExpFullList
            Dim wel As New List(Of WorkExperienceDetails)

            Dim t As New List(Of Integer)
            Try
                t = _queries.GetAddedIDs(applicationID)
                For Each i As Integer In t
                    wel.Add(Mapping.WorkExperienceMapping.MapDBToWorkResult(_queries.GetWorkExperienceByID(i)))
                Next
            Catch ex As Exception
                Me.LogError("GetAddedExpFullList", CInt(Me.UserID), ex)
            End Try
            Return wel
        End Function
      
    End Class
End Namespace