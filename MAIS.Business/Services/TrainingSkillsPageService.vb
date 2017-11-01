Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data
Imports MAIS.Business.Services
Imports MAIS.Data.Queries
Imports MAIS.Business.Model.Enums

Namespace Services
    Public Interface ITrainingSkillsPageService
        Inherits IBusinessBase
        Function GetRNCategoryAndLevel(ByVal RN_sid As Integer) As Data.Role_Category_Level_Xref
        Function GetCourseAll() As List(Of CourseDetails)
        Function GetApplicationCourseAndSessionsByAppID(ByVal AppID As Integer) As List(Of CourseDetails)
        Function GetCourseAndSessionFromPermByUserID(ByVal RN_ID As String) As List(Of CourseDetails)
        Function GetCourseSessionByRN_LicenseNumber(ByVal rn_LicenseNumber As String) As List(Of Model.SessionCourseInfoDetails)
        Function GetCourseSessionByRN_Name(ByVal rn_Name As String) As List(Of Model.SessionCourseInfoDetails)

        Function GetSecretaryRNS(ByVal SecretaryUserID As Integer) As List(Of Model.RN_UserDetails)

        Function GetCourseSessionByRN_LicenseNumber(ByVal rn_LicenseNumber As String, ByVal RoleCategoryLevelsID As Integer) As List(Of Model.SessionCourseInfoDetails)
        Function GetCourseSessionByRN_Name(ByVal rn_Name As String, ByVal RoleCategoryLevelsID As Integer) As List(Of Model.SessionCourseInfoDetails)
        Function GetCourseSessionByRN_Sid(RN_SID As Integer, RoleCategoryLevelSid As Integer) As List(Of Model.SessionCourseInfoDetails)

        Function GetCourseSessionByRN_LicenseNumber(ByVal rn_LicenseNumber As String, ByVal RoleCategoryLevelsID As Integer, ByVal StartDate As Date) As List(Of Model.SessionCourseInfoDetails)
        Function GetCourseSessionByRN_Name(ByVal rn_Name As String, ByVal RoleCategoryLevelsID As Integer, ByVal StartDate As Date) As List(Of Model.SessionCourseInfoDetails)

        Function SaveCourseSessoin(ByVal AppID As Integer, ByVal SessionID As Integer, ByVal UserID As Integer) As Boolean
        Function DeleteCourseSession(ByVal AppID As Integer) As Boolean
        Function GetTrainingPageHelper(ByVal AppID As Integer) As Integer
        Function GetTrainingPageTotalHrOfSession(ByVal AppID As Integer) As Double
        Function GetTrainingPageTotalCEUs(ByVal AppID As Integer, ByVal RNDDUnique_Code As String) As Double
        Function GetTrainingPageTotalCESs(ByVal AppID As Integer, ByVal RNDDUnique_Code As String, ByVal CategoryID As Integer) As Double
        Function GetCEUByUserID(ByVal UserID As String, ByVal CategoryTypeID As Integer, Optional AppID As Integer = -1) As List(Of Model.CEUDetails)
        Function GetAllCEUByUserID(UserID As String, CategoryTypeID As Integer, Optional AppID As Integer = -1) As List(Of CEUDetails)
        Function GetCategoryByRoleCategoryLevelSid(ByVal RoleCategoryLevelSid As Integer) As Model.CategoryDetails

        Function SaveCEUDetail(ByVal UserSavingID As Integer, ByVal CEUList As List(Of Model.CEUDetails), ByVal AppID As Integer) As Boolean
        Function DeleteCEUByID(ByVal CEU_Sid As Integer) As Boolean
    End Interface

    Public Class TrainingSkillsPageService
        Inherits BusinessBase
        Implements ITrainingSkillsPageService

        Private _queries As ITrainingSkillsPageQueires
        Public Sub New(ByVal user As IUserIdentity, ByVal connectionstring As IConnectionIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageQueires)()
            _queries.UserID = user.UserID.ToString()
            _queries.MAISConnectionString = connectionstring.ConnectionString
        End Sub

        Public Function GetRNCategoryAndLevel(RN_sid As Integer) As Role_Category_Level_Xref Implements ITrainingSkillsPageService.GetRNCategoryAndLevel
            Dim CategoryandLevelItem As New Data.Role_Category_Level_Xref

            Try
                CategoryandLevelItem = _queries.GetRNCategoryAndLevel(RN_sid)

            Catch ex As Exception
                Me.LogError("Error GetRNCategoryAndLevel.", CInt(Me.UserID), ex)
            End Try
            Return CategoryandLevelItem

        End Function

        Public Function GetCourseAll() As List(Of CourseDetails) Implements ITrainingSkillsPageService.GetCourseAll
            Dim CourseDetails As New List(Of Model.CourseDetails)

            Try
                CourseDetails = Mapping.TrainingSkillsPageMapping.MapDBToModelCourseDetailesList(_queries.GetCourseAll())

            Catch ex As Exception
                Me.LogError("Error GetCourseAll.", CInt(Me.UserID), ex)
            End Try
            Return CourseDetails

        End Function

        Public Function GetApplicationCourseAndSessionsByAppID(AppID As Integer) As List(Of CourseDetails) Implements ITrainingSkillsPageService.GetApplicationCourseAndSessionsByAppID
            Dim CourseDetails As New List(Of Model.CourseDetails)
            Try
                CourseDetails = Mapping.TrainingSkillsPageMapping.MapDBToModelCourseDetailesList(_queries.GetApplicationCourseAndSessionsByAppID(AppID))
            Catch ex As Exception
                Me.LogError("Error GetApplicationCourseAndSessionsByAppID.", CInt(Me.UserID), ex)
            End Try
            Return CourseDetails
        End Function

        Public Function GetCourseAndSessionFromPermByUserID(RN_ID As String) As List(Of CourseDetails) Implements ITrainingSkillsPageService.GetCourseAndSessionFromPermByUserID
            Dim CourseDetails As New List(Of Model.CourseDetails)
            Try
                CourseDetails = Mapping.TrainingSkillsPageMapping.MapDBToModelCourseDetailesList(_queries.GetCourseAndSessionFromPermByUserID(RN_ID))
            Catch ex As Exception
                Me.LogError("Error GetCourseAndSessionFromPermByUserID.", CInt(Me.UserID), ex)
            End Try
            Return CourseDetails
        End Function

        Public Function GetCourseSessionByRN_LicenseNumber(rn_LicenseNumber As String) As List(Of SessionCourseInfoDetails) Implements ITrainingSkillsPageService.GetCourseSessionByRN_LicenseNumber
            Dim CourseDetails As New List(Of Model.SessionCourseInfoDetails)

            Try
                CourseDetails = Mapping.TrainingSkillsPageMapping.mapDBtoModelSessionCourseDetails(_queries.GetCourseSessionByRN_LicenseNumber(rn_LicenseNumber))

            Catch ex As Exception
                Me.LogError("Error GetCourseSessionByRN_LicenseNumber.", CInt(Me.UserID), ex)
            End Try
            Return CourseDetails
        End Function

        Public Function GetCourseSessionByRN_Name(rn_Name As String) As List(Of SessionCourseInfoDetails) Implements ITrainingSkillsPageService.GetCourseSessionByRN_Name
            Dim CourseDetails As New List(Of Model.SessionCourseInfoDetails)

            Try
                CourseDetails = Mapping.TrainingSkillsPageMapping.mapDBtoModelSessionCourseDetails(_queries.GetCourseSessionByRN_Name(rn_Name))

            Catch ex As Exception
                ' Dim errorMessage As String = ex.Message
                Me.LogError("Error GetCourseSessionByRN_Name.", CInt(Me.UserID), ex)

            End Try
            Return CourseDetails
        End Function

        Public Function GetCourseSessionByRN_LicenseNumber(rn_LicenseNumber As String, RoleCategoryLevelsID As Integer) As List(Of SessionCourseInfoDetails) Implements ITrainingSkillsPageService.GetCourseSessionByRN_LicenseNumber
            Dim CourseDetails As New List(Of Model.SessionCourseInfoDetails)

            Try
                CourseDetails = Mapping.TrainingSkillsPageMapping.mapDBtoModelSessionCourseDetails(_queries.GetCourseSessionByRN_LicenseNumber(rn_LicenseNumber, RoleCategoryLevelsID))
            Catch ex As Exception
                '  Dim errorMessage As String = ex.Message

                Me.LogError("Error GetCourseSessionByRN_LicenseNumber.", CInt(Me.UserID), ex)
            End Try
            Return CourseDetails
        End Function

        Public Function GetCourseSessionByRN_LicenseNumber(rn_LicenseNumber As String, RoleCategoryLevelsID As Integer, StartDate As Date) As List(Of SessionCourseInfoDetails) Implements ITrainingSkillsPageService.GetCourseSessionByRN_LicenseNumber
            Dim CourseDetails As New List(Of Model.SessionCourseInfoDetails)

            Try
                CourseDetails = Mapping.TrainingSkillsPageMapping.mapDBtoModelSessionCourseDetails(_queries.GetCourseSessionByRN_LicenseNumber(rn_LicenseNumber, RoleCategoryLevelsID, StartDate))

            Catch ex As Exception
                ' Dim errorMessage As String = ex.Message
                Me.LogError("Error GetCourseSessionByRN_LicenseNumber by date.", CInt(Me.UserID), ex)

            End Try
            Return CourseDetails
        End Function

        Public Function GetCourseSessionByRN_Name(rn_Name As String, RoleCategoryLevelsID As Integer) As List(Of SessionCourseInfoDetails) Implements ITrainingSkillsPageService.GetCourseSessionByRN_Name
            Dim CourseDetails As New List(Of Model.SessionCourseInfoDetails)

            Try
                CourseDetails = Mapping.TrainingSkillsPageMapping.mapDBtoModelSessionCourseDetails(_queries.GetCourseSessionByRN_Name(rn_Name, RoleCategoryLevelsID))

            Catch ex As Exception
                '  Dim errorMessage As String = ex.Message
                Me.LogError("Error GetCourseSessionByRN_Name.", CInt(Me.UserID), ex)
            End Try
            Return CourseDetails
        End Function

        Public Function GetCourseSessionByRN_Name(rn_Name As String, RoleCategoryLevelsID As Integer, StartDate As Date) As List(Of SessionCourseInfoDetails) Implements ITrainingSkillsPageService.GetCourseSessionByRN_Name
            Dim CourseDetails As New List(Of Model.SessionCourseInfoDetails)

            Try
                CourseDetails = Mapping.TrainingSkillsPageMapping.mapDBtoModelSessionCourseDetails(_queries.GetCourseSessionByRN_Name(rn_Name, RoleCategoryLevelsID, StartDate))

            Catch ex As Exception
                ' Dim errorMessage As String = ex.Message
                Me.LogError("Error GetCourseSessionByRN_Name by date.", CInt(Me.UserID), ex)

            End Try
            Return CourseDetails
        End Function

        Public Function SaveCourseSessoin(AppID As Integer, SessionID As Integer, UserID As Integer) As Boolean Implements ITrainingSkillsPageService.SaveCourseSessoin

            Dim retVal As Boolean = False
            Try
                retVal = _queries.SaveCourseSessoin(AppID, SessionID, UserID)

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while saving Course Session to the Applicaiotn  services.", True, False))
                Me.LogError("Error while saving Course Session to the Applicaiotn in services.", CInt(Me.UserID), ex)
                retVal = False
            End Try
            Return retVal

        End Function


        Public Function DeleteCourseSession(AppID As Integer) As Boolean Implements ITrainingSkillsPageService.DeleteCourseSession
            Dim retVal As Boolean = False
            Try
                retVal = _queries.DeleteCourseSession(AppID)

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while removing Course Session to the Applicaiotn  services.", True, False))
                Me.LogError("Error while removing Course Session to the Applicaiotn in services.", CInt(Me.UserID), ex)
                retVal = False
            End Try
            Return retVal
        End Function

        Public Function GetTrainingPageHelper(AppID As Integer) As Integer Implements ITrainingSkillsPageService.GetTrainingPageHelper
            Dim flag As Integer = 0
            Try
                flag = _queries.GetTrainingPageHelper(AppID)
            Catch ex As Exception
                Me.LogError("Error Getting training and skills page complete rule.", CInt(Me.UserID), ex)
            End Try
            Return flag
        End Function

        Public Function GetTrainingPageTotalHrOfSession(AppID As Integer) As Double Implements ITrainingSkillsPageService.GetTrainingPageTotalHrOfSession
            Dim res As Double
            Try
                res = _queries.GetTrainingPageTotalHrOfSession(AppID)
            Catch ex As Exception
                Me.LogError("Error in GetTrainingPageTotalHrOfSession", CInt(Me.UserID), ex)
            End Try
            Return res
        End Function

        Public Function GetTrainingPageTotalCEUs(AppID As Integer, RNDDUnique_Code As String) As Double Implements ITrainingSkillsPageService.GetTrainingPageTotalCEUs
            Dim res As Double
            Try
                res = _queries.GetTrainingPageTotalCEUs(AppID, RNDDUnique_Code)
            Catch ex As Exception
                Me.LogError("Error in GetTrainingPageTotalHrOfSession", CInt(Me.UserID), ex)
            End Try
            Return res

        End Function

        Public Function GetTrainingPageTotalCESs(AppID As Integer, RNDDUnique_Code As String, CategoryID As Integer) As Double Implements ITrainingSkillsPageService.GetTrainingPageTotalCESs
            Dim res As Double
            Try
                res = _queries.GetTrainingPageTotalCESs(AppID, RNDDUnique_Code, CategoryID)
            Catch ex As Exception
                Me.LogError("Error in GetTrainingPageTotalHrOfSession", CInt(Me.UserID), ex)
            End Try
            Return res

        End Function

        Public Function GetCEUByUserID(UserID As String, CategoryTypeID As Integer, Optional AppID As Integer = -1) As List(Of CEUDetails) Implements ITrainingSkillsPageService.GetCEUByUserID
            Dim retVal As New List(Of Model.CEUDetails)

            Try
                retVal = Mapping.CEUMapping.mapDBtoModelCEUDetails(_queries.GetCEUByUserID(UserID, CategoryTypeID, AppID))
            Catch ex As Exception
                Me.LogError("Error GetCEUByUserID.", CInt(Me.UserID), ex)
            End Try
            Return retVal
        End Function

        Public Function GetAllCEUByUserID(UserID As String, CategoryTypeID As Integer, Optional AppID As Integer = -1) As List(Of CEUDetails) Implements ITrainingSkillsPageService.GetAllCEUByUserID
            Dim retVal As New List(Of Model.CEUDetails)

            Try
                retVal = Mapping.CEUMapping.mapDBtoModelCEUDetails(_queries.GetAllCEUByUserID(UserID, CategoryTypeID, AppID))
            Catch ex As Exception
                Me.LogError("Error GetAllCEUByUserID.", CInt(Me.UserID), ex)
            End Try
            Return retVal
        End Function

        Public Function GetCategoryByRoleCategoryLevelSid(RoleCategoryLevelSid As Integer) As CategoryDetails Implements ITrainingSkillsPageService.GetCategoryByRoleCategoryLevelSid
            Dim retVal As New Model.CategoryDetails
            Try
                retVal = Mapping.TrainingSkillsPageMapping.mapDBtoModelCacategoryDetails(_queries.GetCategoryByRoleCategoryLevelSid(RoleCategoryLevelSid))
            Catch ex As Exception
                Me.LogError("GetCategoryByRoleCategoryLevelSid.", CInt(Me.UserID), ex)
            End Try
            Return retVal

        End Function

        Public Function SaveCEUDetail(UserSavingID As Integer, CEUList As List(Of CEUDetails), AppID As Integer) As Boolean Implements ITrainingSkillsPageService.SaveCEUDetail
            Dim retVal As Boolean
            Try
                retVal = _queries.SaveCEUDetail(UserSavingID, Mapping.TrainingSkillsPageMapping.MapModelCEUsDetailsToDB(CEUList), AppID)
            Catch ex As Exception
                Me.LogError("Error SaveCEUDetail.", CInt(Me.UserID), ex)
            End Try

            Return retVal
        End Function

        Public Function DeleteCEUByID(CEU_Sid As Integer) As Boolean Implements ITrainingSkillsPageService.DeleteCEUByID
            Dim retVal As Boolean
            Try
                retVal = _queries.DeleteCEUByID(CEU_Sid)
            Catch ex As Exception
                Me.LogError("Error DeleteCEUByID.", CInt(Me.UserID), ex)
            End Try
            Return retVal
        End Function


        Public Function GetSecretaryRNS(SecretaryUserID As Integer) As List(Of RN_UserDetails) Implements ITrainingSkillsPageService.GetSecretaryRNS
            Dim retVal As New List(Of Model.RN_UserDetails)
            Try
                retVal = Mapping.RN_DetailsMapping.MappingToModelRn_userList(_queries.GetSecretaryRNS(SecretaryUserID))
            Catch ex As Exception
                Me.LogError("Error GetSecretaryRNS.", CInt(Me.UserID), ex)
            End Try

            Return retVal
        End Function

        Public Function GetCourseSessionByRN_Sid(RN_SID As Integer, RoleCategoryLevelSid As Integer) As List(Of SessionCourseInfoDetails) Implements ITrainingSkillsPageService.GetCourseSessionByRN_Sid
            Dim retVal As New List(Of Model.SessionCourseInfoDetails)
            Try
                retVal = Mapping.TrainingSkillsPageMapping.mapDBtoModelSessionCourseDetails(_queries.GetCourseSessionByRN_Sid(RN_SID, RoleCategoryLevelSid))
            Catch ex As Exception
                Me.LogError("Error GetCourseSessionByRN_Sid.", CInt(Me.UserID), ex)
            End Try
            Return retVal
        End Function


    End Class
End Namespace
