Imports MAIS.Data.Queries
Imports ODMRDDHelperClassLibrary.Utility

Namespace Queries
    Public Interface IStartPageQueries
        Inherits IQueriesBase
        Function GetCertificationDetails(ByVal role As Integer, ByVal currentRole As Integer, ByVal applicationType As String) As Objects.CertificationEligibleInfo
        Function SaveAppInfo(ByVal appDetails As Objects.ApplicationInformation) As Integer
        Function GetApplicationInformation(ByVal applicationID As Integer) As Objects.ApplicationInformation
    End Interface
End Namespace
Public Class StartPageQueries
    Inherits QueriesBase
    Implements IStartPageQueries
    Public Function GetCertificationDetails(role As Integer, currentRole As Integer, ByVal applicationType As String) As Objects.CertificationEligibleInfo Implements Queries.IStartPageQueries.GetCertificationDetails
        Dim certificationDetails As New Objects.CertificationEligibleInfo
        Using context As New MAISContext()
            Try
                If (applicationType = "Initial") Then
                    certificationDetails = (From cr In context.Certification_Requirement_Information _
                                            Join app In context.Application_Type On cr.Application_Type_Sid Equals app.Application_Type_Sid _
                                            Join appRole In context.Role_Category_Level_Xref On cr.Role_Category_Level_Sid Equals appRole.Role_Category_Level_Sid _
                                            Join role1 In context.MAIS_Role On appRole.Role_Sid Equals role1.Role_Sid _
                                            Where cr.Active_Flg = True And cr.Role_Category_Level_Sid = role And cr.Application_Type_Sid = 1 _
                                            And cr.Current_Role_Category_Level_Sid = currentRole _
                                            Select New Objects.CertificationEligibleInfo() With {
                    .CertificationType = role1.Role_Desc,
                    .InitialCertificationReq = cr.Requirement_Description
                                                }).FirstOrDefault()
                End If
                If (applicationType = "Renewal") Then
                    certificationDetails = (From cr In context.Certification_Requirement_Information _
                                            Join app In context.Application_Type On cr.Application_Type_Sid Equals app.Application_Type_Sid _
                                            Join appRole In context.Role_Category_Level_Xref On cr.Role_Category_Level_Sid Equals appRole.Role_Category_Level_Sid _
                                            Join role1 In context.MAIS_Role On appRole.Role_Sid Equals role1.Role_Sid _
                                            Where cr.Active_Flg = True And cr.Role_Category_Level_Sid = role And cr.Application_Type_Sid = 2 _
                                            And cr.Current_Role_Category_Level_Sid = currentRole _
                                            Select New Objects.CertificationEligibleInfo() With {
                                                .CertificationType = role1.Role_Desc,
                    .RenewalCertificationReq = cr.Requirement_Description
                                                }).FirstOrDefault()
                End If
                If (applicationType = "AddOn") Then
                    certificationDetails = (From cr In context.Certification_Requirement_Information _
                                            Join app In context.Application_Type On cr.Application_Type_Sid Equals app.Application_Type_Sid _
                                            Join appRole In context.Role_Category_Level_Xref On cr.Role_Category_Level_Sid Equals appRole.Role_Category_Level_Sid _
                                            Join role1 In context.MAIS_Role On appRole.Role_Sid Equals role1.Role_Sid _
                                            Join appCurrentRole In context.Role_Category_Level_Xref On cr.Current_Role_Category_Level_Sid Equals appCurrentRole.Role_Category_Level_Sid _
                                            Join role2 In context.MAIS_Role On appCurrentRole.Role_Sid Equals role2.Role_Sid _
                                            Where cr.Active_Flg = True And cr.Role_Category_Level_Sid = role And cr.Application_Type_Sid = 3 _
                                            And cr.Current_Role_Category_Level_Sid = currentRole
                                            Select New Objects.CertificationEligibleInfo() With {
                                                .CertificationType = role2.Role_Desc,
                    .AddOnCertificationReq = cr.Requirement_Description
                                                }).FirstOrDefault()
                End If
            Catch ex As Exception
                Me.LogError("Error in Getting Certification Info", CInt(Me.UserID), ex)
                Throw
            Finally
                'context.Connection.Close()
            End Try
        End Using
        Return certificationDetails
    End Function

    Public Function SaveAppInfo(appDetails As Objects.ApplicationInformation) As Integer Implements IStartPageQueries.SaveAppInfo
        Dim retval As Integer
        Using context As New MAISContext()
            Try
                If (appDetails IsNot Nothing) Then
                    Dim _app As Application = Nothing
                    _app = (From application In context.Applications
                            Where application.Application_Sid = appDetails.ApplicationID And (application.Application_Status_Type_Sid = 1 Or application.Application_Status_Type_Sid = 6 Or application.Application_Status_Type_Sid = 12)).FirstOrDefault()
                    If (_app Is Nothing) Then
                        Dim _maApplication As Application = New Application()
                        _maApplication.Application_Status_Type_Sid = appDetails.ApplicationStatusTypeID
                        _maApplication.Application_Type_Sid = appDetails.ApplicationTypeID
                        _maApplication.Role_Category_Level_Sid = appDetails.RoleCategoryLevelID
                        _maApplication.RN_flg = appDetails.RNFlag
                        _maApplication.RN_DD_Unique_Code = appDetails.UniqueCode
                        _maApplication.Create_By = Me.UserID
                        _maApplication.Create_Date = DateTime.Now
                        _maApplication.Last_Update_By = Me.UserID
                        _maApplication.Last_Update_Date = DateTime.Now
                        context.Applications.Add(_maApplication)
                        context.SaveChanges()
                        retval = _maApplication.Application_Sid
                    Else
                        _app.Role_Category_Level_Sid = appDetails.RoleCategoryLevelID
                        _app.Application_Status_Type_Sid = appDetails.ApplicationStatusTypeID
                        _app.Application_Type_Sid = appDetails.ApplicationTypeID
                        _app.RN_DD_Unique_Code = appDetails.UniqueCode
                        context.SaveChanges()
                        retval = _app.Application_Sid
                    End If
                End If

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while start page information services.", True, False))
                Me.LogError("Error while saving start page information services.", CInt(Me.UserID), ex)
                Throw
            End Try
        End Using
        Return retval
    End Function

    Public Function GetApplicationInformation(applicationID As Integer) As Objects.ApplicationInformation Implements IStartPageQueries.GetApplicationInformation
        Dim appDetails As New Objects.ApplicationInformation
        Using context As New MAISContext()
            Try
                appDetails = (From application In context.Applications
                        Where application.Application_Sid = applicationID
                        Select New Objects.ApplicationInformation() With {
                                .ApplicationID = application.Application_Sid,
                                .ApplicationTypeID = application.Application_Type_Sid,
                                .RNFlag = application.RN_flg,
                                .RoleCategoryLevelID = application.Role_Category_Level_Sid
                                       }
                            ).FirstOrDefault()
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while getting application information services.", True, False))
                Me.LogError("Error while getting application information services.", CInt(Me.UserID), ex)
                Throw
            End Try
            Return appDetails
        End Using
    End Function
End Class
