Imports System.Data.Linq
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Data.Objects

Namespace Queries
    Public Interface IApplicationDetailInformationQueires
        Inherits IQueriesBase

        Function GetApplicationInformationDetailsByAppID(ByVal AppID As Integer) As Application
        Function UpdateApplicationSignature(ByVal AppID As Integer, ByVal Signature As String, ByVal CurrentUserID As Integer, ByVal RNID As Integer, Optional IsAddmin As Boolean = False) As Boolean
        Function GetRNLicenseIssuenceDateByAppID(ByVal AppID As Integer) As DateTime

    End Interface

    Public Class ApplicationDetailInformationQueires
        Inherits QueriesBase
        Implements IApplicationDetailInformationQueires

        Public Function GetRNLicenseIssuenceDateByAppID(ByVal AppID As Integer) As DateTime Implements IApplicationDetailInformationQueires.GetRNLicenseIssuenceDateByAppID
            Dim retObj As New DateTime
            Try
                Using context As New MAISContext
                    retObj = (From ra In context.RN_Application
                              Where ra.Application_Sid = AppID
                              Select ra.Date_Of_Original_Issuance).FirstOrDefault()
                End Using
            Catch ex As Exception
                Me.LogError("Error fetching RN license issuance date.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error fetching RN license issuance date.", True, False))
                Throw
            End Try
            Return retObj
        End Function
        Public Function GetApplicationInformationDetailsByAppID(AppID As Integer) As Application Implements IApplicationDetailInformationQueires.GetApplicationInformationDetailsByAppID
            Dim retAppInfo As New Application
            Using context As New MAISContext
                Try
                    retAppInfo = (From a In context.Applications _
                                                    Where a.Application_Sid = AppID
                                                    Select a).FirstOrDefault()

                Catch ex As Exception
                    Me.LogError("Error Getting Application Info", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting Application Info.", True, False))
                    Throw
                End Try
            End Using
            Return retAppInfo
        End Function

        Public Function UpdateApplicationSignature(AppID As Integer, Signature As String, ByVal CurrentUserID As Integer, ByVal RNID As Integer, Optional isAddmin As Boolean = False) As Boolean Implements IApplicationDetailInformationQueires.UpdateApplicationSignature
            Dim retVale As Boolean = False
            Using context As New MAISContext
                Try
                    Dim MA_ApplicationRef = (From a In context.Applications
                                             Where a.Application_Sid = AppID
                                             Select a).FirstOrDefault

                    If Not MA_ApplicationRef Is Nothing Then
                        MA_ApplicationRef.Signature = Signature
                        If isAddmin = False Then
                            MA_ApplicationRef.Attestant_Sid = RNID
                        Else
                            MA_ApplicationRef.Is_Admin_Flg = True
                        End If

                        MA_ApplicationRef.Last_Update_By = CurrentUserID
                        MA_ApplicationRef.Last_Update_Date = Now()

                    End If
                    'Below is the code to update attestation date for application history JH 5/14/2013
                    If AppID > 0 Then
                        Dim AppHis As Application_History = (From ah In context.Application_History
                                                              Where ah.Application_Sid = AppID
                                                              Select ah).FirstOrDefault()
                        If AppHis IsNot Nothing Then
                            AppHis.Attestation_Date = DateTime.Now
                            AppHis.Last_Update_Date = DateTime.Now
                            AppHis.Last_Update_By = Me.UserID
                        End If
                    End If
                    context.SaveChanges()
                    Return True
                Catch ex As Exception
                    Me.LogError("Error updateing application signature", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error updateing application signature.", True, False))
                    Throw
                End Try
            End Using
        End Function
    End Class
End Namespace