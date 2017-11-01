Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data
Imports MAIS.Data.Objects
Imports System.Data.Linq
Imports System.Configuration



Namespace Services
    Public Interface IManageCourseService
        Inherits IBusinessBase

        Function SaveMangeCourse(ByVal Course As Model.CourseDetails) As Integer
        Function GetLevelsByRoleID(ByVal RoleID As Integer) As List(Of LevelsDetails)
        Function GetCategories(ByVal LevelID As Integer) As List(Of CategoryDetails)
        Function getCourseByCourseID(ByVal CourseID As Integer) As Model.CourseDetails
        Function DoesCourseExitAlready(ByVal CourseNumber As String) As Boolean
        Function DoeseCourseSessionOverLap(ByVal CourseNumber As String, ByVal SessionStartDate As Date, ByVal SessionEndDate As Date) As Boolean
        Function SearchCourseByRN(ByVal RN_Number As String) As List(Of Model.CourseDetails)
        Function SearchCourseByName(ByVal Name As String, ByVal NameType As String) As List(Of Model.CourseDetails)
        Function GetAllCourseAndSessionInfoByCourseID(ByVal CourseID As Integer) As List(Of Model.CourseDetails)
        Function SearchSessionByStartDate(ByVal StartDate As Date) As List(Of Model.CourseDetails)
        Function SearchMangeCourse(Optional ByVal RNNumber As String = Nothing, Optional ByVal FirstName As String = Nothing, Optional ByVal LastName As String = Nothing, Optional ByVal SessionStartDate As Date = Nothing) As List(Of Model.CourseDetails)
        Function DeleteSessionBySessionSid(ByVal SessionSid As Integer) As Boolean
    End Interface
    Public Class ManageCourseService
        Inherits BusinessBase
        Implements IManageCourseService

        Private _queries As Data.Queries.IManageCourseQueires


        <Obsolete("Use StructureMap.objectFactory.Getinstance(Of IManageCourseService() instead!", True)> _
        Public Sub New()
            ' Throw New NotImplementedException("Method not usable. User StructureMap.ObjectFactory.GetInstance(of IManageCourseService() )() instead!")
        End Sub

        Public Sub New(ByVal user As IUserIdentity, ByVal maisConnectionString As IConnectionIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of Data.Queries.ManageCourseQueires)()
            _queries.UserID = user.UserID
            _queries.MAISConnectionString = maisConnectionString.ConnectionString
        End Sub

        Public Function SaveMangeCourse(Course As Model.CourseDetails) As Integer Implements IManageCourseService.SaveMangeCourse

            Try
                Return _queries.SaveMangeCourse(Mapping.ManageCousrePageMapping.MapCouresDataToDB(Course))
                Return 0
            Catch ex As Exception
                Me.LogError("Error SaveMangeCourse", CInt(Me.UserID), ex)
                Return -1
            End Try

        End Function

        Public Function GetCategories(LevelID As Integer) As List(Of CategoryDetails) Implements IManageCourseService.GetCategories
            Dim res As New List(Of CategoryDetails)
            Try
                res = Mapping.ManageCousrePageMapping.MapCategoryfromDB(_queries.GetCategories(LevelID))
            Catch ex As Exception
                Me.LogError("Error GetCategories", CInt(Me.UserID), ex)
            End Try
            Return res
        End Function



        Public Function GetLevelsByRoleID(RoleID As Integer) As List(Of LevelsDetails) Implements IManageCourseService.GetLevelsByRoleID
            Dim res As New List(Of LevelsDetails)
            Try
                res = Mapping.ManageCousrePageMapping.MapLevelFromDB(_queries.GetLevelsByRoleID(RoleID))
            Catch ex As Exception
                Me.LogError("Error GetCategories", CInt(Me.UserID), ex)
            End Try
            Return res

        End Function

        Public Function getCourseByCourseID(CourseID As Integer) As Model.CourseDetails Implements IManageCourseService.getCourseByCourseID
            Dim retVal As New Model.CourseDetails
            Dim cList As New List(Of Model.CourseDetails)
            Try
                cList = Mapping.TrainingSkillsPageMapping.MapDBToModelCourseDetailesList(_queries.getCourseByCourseID(CourseID))

                Select Case True
                    Case cList.Count = 0
                        retVal = Nothing
                    Case cList.Count > 1
                        retVal = cList(0)
                    Case Else
                        retVal = cList(0)

                End Select
            Catch ex As Exception
                Me.LogError("Error getCourseByCourseID", CInt(Me.UserID), ex)
            End Try
            Return retVal

        End Function
        Public Function DoesCourseExitAlready(CourseNumber As String) As Boolean Implements IManageCourseService.DoesCourseExitAlready
            Dim res As Boolean

            Try
                res = _queries.DoesCourseExitAlready(CourseNumber)
            Catch ex As Exception
                Me.LogError("Error DoesCourseExitAlready", CInt(Me.UserID), ex)
            End Try
            Return res

        End Function

        Public Function DoeseCourseSessionOverLap(CourseNumber As String, SessionStartDate As Date, SessionEndDate As Date) As Boolean Implements IManageCourseService.DoeseCourseSessionOverLap
            Dim res As Boolean

            Try
                res = _queries.DoeseCourseSessionOverLap(CourseNumber, SessionStartDate, SessionEndDate)
            Catch ex As Exception
                Me.LogError("Error DoesCourseExitAlready", CInt(Me.UserID), ex)
            End Try
            Return res

        End Function

        Public Function SearchCourseByRN(RN_Number As String) As List(Of Model.CourseDetails) Implements IManageCourseService.SearchCourseByRN
            Try
                Return Mapping.TrainingSkillsPageMapping.MapDBToModelCourseDetailesList(_queries.SearchCourseByRN(RN_Number))

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while testing for a course number in Manage Course Service.", True, False))
                Me.LogError("Error while testing for a course number in Manage Course Service.", CInt(Me.UserID), ex)
                Return Nothing
            End Try
        End Function

        Public Function SearchCourseByName(Name As String, NameType As String) As List(Of Model.CourseDetails) Implements IManageCourseService.SearchCourseByName
            Try
                Dim RetVal As New List(Of Model.CourseDetails)

                Select Case NameType
                    Case "FirstName"
                        RetVal = Mapping.TrainingSkillsPageMapping.MapDBToModelCourseDetailesList(_queries.SearchCourseByFirstName(Name))
                    Case "LastName"
                        RetVal = Mapping.TrainingSkillsPageMapping.MapDBToModelCourseDetailesList(_queries.SearchCourseByLastName(Name))
                End Select

                Return RetVal

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while testing for a course number in Manage Course Service.", True, False))
                Me.LogError("Error while testing for a course number in Manage Course Service.", CInt(Me.UserID), ex)
                Return Nothing
            End Try
        End Function


        Public Function GetAllCourseAndSessionInfoByCourseID(CourseID As Integer) As List(Of Model.CourseDetails) Implements IManageCourseService.GetAllCourseAndSessionInfoByCourseID
            Try
                Dim RetVal As New List(Of Model.CourseDetails)
                RetVal = Mapping.TrainingSkillsPageMapping.MapDBToModelCourseDetailesList(_queries.GetAllCourseAndSessionInfoByCourseID(CourseID))

                Return RetVal
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while testing for a course number in Manage Course Service.", True, False))
                Me.LogError("Error while testing for a course number in Manage Course Service.", CInt(Me.UserID), ex)
                Return Nothing
            End Try
        End Function

        Public Function SearchSessionByStartDate(StartDate As Date) As List(Of Model.CourseDetails) Implements IManageCourseService.SearchSessionByStartDate
            Try
                Dim RetVal As New List(Of Model.CourseDetails)
                RetVal = Mapping.TrainingSkillsPageMapping.MapDBToModelCourseDetailesList(_queries.SearchSessionByStartDate(StartDate))
                Return RetVal

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while testing for a course number in Manage Course Service.", True, False))
                Me.LogError("Error while testing for a course number in Manage Course Service.", CInt(Me.UserID), ex)
                Return Nothing
            End Try
        End Function

        Public Function SearchMangeCourse(Optional RNNumber As String = Nothing, Optional FirstName As String = Nothing, Optional LastName As String = Nothing, Optional SessionStartDate As Date = #12:00:00 AM#) As List(Of Model.CourseDetails) Implements IManageCourseService.SearchMangeCourse
            Dim RetVal As New List(Of Model.CourseDetails)
            Try
                RetVal = Mapping.TrainingSkillsPageMapping.MapDBToModelCourseDetailesList(_queries.SearchMangeCourse(RNNumber, FirstName, LastName, SessionStartDate))
            Catch ex As Exception
                Me.LogError("Error SearchMangeCourse", CInt(Me.UserID), ex)
            End Try
            Return RetVal

        End Function


        Public Function DeleteSessionBySessionSid(SessionSid As Integer) As Boolean Implements IManageCourseService.DeleteSessionBySessionSid
            Dim retVal As Boolean = False
            Try
                retVal = _queries.DeleteSessionBySessionSid(SessionSid)
            Catch ex As Exception
                Me.LogError("Error DeleteSessionBySessionSid", CInt(Me.UserID), ex)
            End Try
            Return retVal

        End Function
    End Class
End Namespace
