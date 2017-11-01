Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data
Imports MAIS.Business.Services
Imports MAIS.Data.Queries
Imports MAIS.Business.Model.Enums

Namespace Services
    Public Interface ISkillPageService
        Inherits IBusinessBase
        Function GetSkillCategorys() As List(Of CategoryDetails)
        Function GetSKillListByCategory(ByVal CategoryID As Integer) As List(Of Model.SkillDetails)
        Function GetSkillCheckListbySkillID(ByVal SkillID As Integer) As List(Of Model.SkillCheckListDetails)
        Function SaveSkillVerificationData(ByVal SkillsData As Model.SkillsVerificationDetails, ByVal SaveToTemp As Boolean) As Boolean
        Function SaveSkillVerificationData(ByVal SkillsData As List(Of Model.SkillsVerificationDetails), ByVal SaveToTemp As Boolean) As Boolean

        Function GetSkillVerificationCheckListData(ByVal user As String, Optional ByVal ApplicationID As Integer = 0) As List(Of Model.SkillVerificationTypeCheckListOnly)
        Function GetSkillVerificationData(ByVal User As String, Optional ByVal ApplicationID As Integer = 0) As List(Of Model.SkillsVerificationDetails)
        Function GetSkillVerificationPageCompletion(ByVal User As String, ByVal CategoryID As Integer, ByVal CheckTemp As Boolean, Optional ByVal ApplicationID As Integer = 0) As Boolean
        Function DeleteSkillVerificationData(ByVal SkillChecklistXrefSid As Integer, ByVal RemoveFromTemp As Boolean) As Boolean

    End Interface

    Public Class SkillPageService
        Inherits BusinessBase
        Implements ISkillPageService


        Private _queries As ISkillPageQueries
        Public Sub New(ByVal user As IUserIdentity, ByVal connectionstring As IConnectionIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of ISkillPageQueries)()
            _queries.UserID = user.UserID.ToString
            _queries.MAISConnectionString = connectionstring.ConnectionString
        End Sub

        Public Function GetSkillCategorys() As List(Of CategoryDetails) Implements ISkillPageService.GetSkillCategorys
            Dim retVal As New List(Of CategoryDetails)
            Try
                retVal = Mapping.SkillsPageMapping.MapDBtoCategoryDetails(_queries.GetSkillCategorys)
            Catch ex As Exception
                Me.LogError("Error GetSkillCategorys", CInt(Me.UserID), ex)
            End Try

            Return retVal
        End Function

        Public Function GetSKillListByCategory(CategoryID As Integer) As List(Of SkillDetails) Implements ISkillPageService.GetSKillListByCategory
            Dim retVal As New List(Of SkillDetails)
            Try
                retVal = Mapping.SkillsPageMapping.MapDBtoSkillDeatils(_queries.GetSKillListByCategory(CategoryID))
            Catch ex As Exception
                Me.LogError("Error GetSKillListByCategory", CInt(Me.UserID), ex)
            End Try

            Return retVal

        End Function

        Public Function GetSkillCheckListbySkillID(SkillID As Integer) As List(Of SkillCheckListDetails) Implements ISkillPageService.GetSkillCheckListbySkillID
            Dim retVal As New List(Of Model.SkillCheckListDetails)
            Try
                retVal = Mapping.SkillsPageMapping.MapDBtoSkillCheckListDetails(_queries.GetSkillCheckListbySkillID(SkillID))
            Catch ex As Exception
                Me.LogError("Error GetSkillCheckListbySkillID", CInt(Me.UserID), ex)
            End Try
            Return retVal

        End Function

        Public Function SaveSkillVerificationData(SkillsData As SkillsVerificationDetails, SaveToTemp As Boolean) As Boolean Implements ISkillPageService.SaveSkillVerificationData
            Dim retVal As Boolean = False
            Try
                If SaveToTemp = True Then
                    'This will save the Data to the Temp Tables. 
                    retVal = _queries.SaveSkillVerificationDataToTemp(Mapping.SkillsPageMapping.MapModelSkillVerificationToDb(SkillsData))
                Else
                    'This will save data to the Perm Tables. 
                    retVal = _queries.SaveSkillVerificationData(Mapping.SkillsPageMapping.MapModelSkillVerificationToDb(SkillsData))
                End If
            Catch ex As Exception
                Me.LogError("Error SaveSkillVerificationData", CInt(Me.UserID), ex)
            End Try

            Return retVal
        End Function

        Public Function SaveSkillVerificationData(SkillsData As List(Of SkillsVerificationDetails), SaveToTemp As Boolean) As Boolean Implements ISkillPageService.SaveSkillVerificationData
            Dim RetVal As Boolean = False
            Try


                If SaveToTemp = True Then
                    'this will save to the Data to the Temp Tables. 
                    RetVal = _queries.SaveSkillVerificationDataToTemp(Mapping.SkillsPageMapping.MapModelSkillVerificationToDb(SkillsData))

                Else
                    'This will save the data to the Perm Tables.
                    RetVal = _queries.SaveSkillVerificationData(Mapping.SkillsPageMapping.MapModelSkillVerificationToDb(SkillsData))
                End If
            Catch ex As Exception
                Me.LogError("Error SaveSkillVerificationData", CInt(Me.UserID), ex)
                Return RetVal
            End Try
            Return RetVal
        End Function

        Public Function GetSkillVerificationData(User As String, Optional ApplicationID As Integer = 0) As List(Of SkillsVerificationDetails) Implements ISkillPageService.GetSkillVerificationData
            Dim retVal As New List(Of Model.SkillsVerificationDetails)
            Try
                retVal = Mapping.SkillsPageMapping.MapDBtoSkillVerification(_queries.GetSkillVerificationData(User, ApplicationID))
            Catch ex As Exception
                Me.LogError("Error GetSkillVerificationData", CInt(Me.UserID), ex)
            End Try
            Return retVal

        End Function

        Public Function GetSkillVerificationCheckListData(user As String, Optional ApplicationID As Integer = 0) As List(Of SkillVerificationTypeCheckListOnly) Implements ISkillPageService.GetSkillVerificationCheckListData
            Dim retVal As New List(Of Model.SkillVerificationTypeCheckListOnly)
            Try
                retVal = Mapping.SkillsPageMapping.MapDBtoSkillCheckListWithSkilltypeDetails(_queries.GetSkillVerificationCheckListData(user, ApplicationID))
            Catch ex As Exception
                Me.LogError("Error GetSkillVerificationCheckListData", CInt(Me.UserID), ex)
            End Try

            Return retVal

        End Function

        Public Function GetSkillVerificationPageCompletion(User As String, CategoryID As Integer, CheckTemp As Boolean, Optional ApplicationID As Integer = 0) As Boolean Implements ISkillPageService.GetSkillVerificationPageCompletion
            Dim res As Boolean
            Try
                res = _queries.GetSkillVerificationPageCompletion(User, CategoryID, CheckTemp, ApplicationID)
            Catch ex As Exception
                Me.LogError("Error GetSkillVerificationPageCompletion", CInt(Me.UserID), ex)
            End Try
            Return res
        End Function

        Public Function DeleteSkillVerificationData(SkillChecklistXrefSid As Integer, RemoveFromTemp As Boolean) As Boolean Implements ISkillPageService.DeleteSkillVerificationData
            Dim retVal As Boolean = False
            Try
                If RemoveFromTemp = True Then ' remove form temp tables
                    retVal = _queries.DeleteSkillVerificationDataFromTemp(SkillChecklistXrefSid)

                Else ' remove to permit tables.
                    retVal = _queries.DeleteSkillVerificationDataFromPerm(SkillChecklistXrefSid)
                End If
            Catch ex As Exception
                Me.LogError("Error DeleteSkillVerificationData", CInt(Me.UserID), ex)
            End Try
            Return retVal
        End Function

    End Class
End Namespace

