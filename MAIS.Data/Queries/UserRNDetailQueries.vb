Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Data.Objects
Imports System.Data.Entity.Validation

Namespace Queries
    Public Interface IUserRNDetailQueries
        Inherits IQueriesBase
        Function getAllRNDetails() As List(Of Data.RN)
        Function GetRNDetailsByRNLicenseNumber(ByVal RNLicenseNumber As String) As Data.RN
        Function GetAllDDPersonnel() As List(Of DDPersonnel)
        Function GetRNDetailsWithEmailsByRN_Sid(ByVal RN_Sid As Integer) As Objects.RN_UserDetailsObject
        Function GetRNDetailsWithEmailsByRoleID(ByVal RoleIDList As String) As List(Of Objects.RN_UserDetailsObject)
    End Interface
    Public Class UserRNDetailQueries
        Inherits QueriesBase
        Implements IUserRNDetailQueries

        Public Function getAllRNDetails() As List(Of Data.RN) Implements IUserRNDetailQueries.getAllRNDetails
            Dim retVal As New List(Of RN)

            Try
                Using Context As New MAISContext
                    retVal = (From a In Context.RNs _
                              Order By a.Last_Name
                              Where a.Active_Flg = True _
                              Select a).ToList

                End Using


            Catch ex As DbEntityValidationException
                Me._messages.Add(New ReturnMessage("Error while saving work experience information queries.", True, False))
                Me.LogError("Error while saving work experience information queries.", CInt(Me.UserID), ex)
                'retVal.AddErrorMessage(ex.Message)
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while saving work experience information queries.", True, False))
                Me.LogError("Error while saving work experience information queries.", CInt(Me.UserID), ex)
                'retVal.AddErrorMessage(ex.Message)
            End Try

            Return retVal
        End Function

        Public Function GetRNDetailsByRNLicenseNumber(RNLicenseNumber As String) As Data.RN Implements IUserRNDetailQueries.GetRNDetailsByRNLicenseNumber
            Dim retVal As New Data.RN
            Try
                Using Context As New MAISContext
                    retVal = (From a In Context.RNs _
                              Where a.RNLicense_Number = RNLicenseNumber _
                              Select a).FirstOrDefault

                End Using

            Catch ex As DbEntityValidationException
                Me._messages.Add(New ReturnMessage("Error while saving work experience information queries.", True, False))
                Me.LogError("Error while saving work experience information queries.", CInt(Me.UserID), ex)
                'retVal.AddErrorMessage(ex.Message)
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while saving work experience information queries.", True, False))
                Me.LogError("Error while saving work experience information queries.", CInt(Me.UserID), ex)
                'retVal.AddErrorMessage(ex.Message)
            End Try

            Return retVal
        End Function

        Public Function GetAllDDPersonnel() As List(Of DDPersonnel) Implements IUserRNDetailQueries.GetAllDDPersonnel
            Dim retValLst As New List(Of Data.DDPersonnel)
            Try
                Using Context As New MAISContext
                    retValLst = (From a In Context.DDPersonnels _
                                 Order By a.Last_Name
                              Where a.Active_Flg = True
                              Select a).ToList()
                End Using
            Catch ex As DbEntityValidationException
                Me._messages.Add(New ReturnMessage("Error while fetching ddpersonnel list.", True, False))
                Me.LogError("Error while fetching ddpersonnel list.", CInt(Me.UserID), ex)
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while fetching ddpersonnel list.", True, False))
                Me.LogError("Error while fetching ddpersonnel list.", CInt(Me.UserID), ex)
            End Try
            Return retValLst
        End Function

        Public Function GetRNDetailsWithEmailsByRN_Sid(RN_Sid As Integer) As RN_UserDetailsObject Implements IUserRNDetailQueries.GetRNDetailsWithEmailsByRN_Sid
            Dim retVal As New Objects.RN_UserDetailsObject
            Try
                Using context As New MAISContext
                    retVal = (From r In context.RNs
                              Where r.RN_Sid = RN_Sid
                              Select New Objects.RN_UserDetailsObject With {
                                  .RN_Sid = r.RN_Sid,
                                  .RNLicense_Number = r.RNLicense_Number,
                                  .FirstName = r.First_Name,
                                  .LastName = r.Last_Name,
                                  .MiddleName = r.Middle_Name,
                                  .CreateDate = r.Create_Date,
                                  .CreateBy = r.Create_By,
                                  .LastUpdateBy = r.Last_Update_By,
                                  .LastUpdatedDate = r.Last_Updated_Date,
                                  .DateOfOriginalRNLicIssuance = r.Date_Of_Original_Issuance,
                                  .Gender = r.Gender,
                                  .Active_Flg = r.Active_Flg}).FirstOrDefault


                    With retVal
                        .EmailList = (From r In context.RNs
                        Join rn In context.RN_DD_Person_Type_Xref On r.RN_Sid Equals rn.RN_DDPersonnel_Sid
                        Join en In context.RN_DD_Person_Type_Email_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals en.RN_DD_Person_Type_Xref_Sid
                        Join em In context.Email1 On em.Email_SID Equals en.Email_Sid
                                      Where r.RN_Sid = .RN_Sid
                                      Select New Objects.EmailAddressDetails With {
                                          .EmailAddressSID = em.Email_SID,
                                          .EmailAddress = em.Email_Address,
                                          .ContactType = en.Contact_Type_Sid}).ToList
                    End With

                End Using

            Catch ex As DbEntityValidationException
                Me._messages.Add(New ReturnMessage("Error while fetching RN Details with email list.", True, False))
                Me.LogError("Error while fetching RN Details with email list.", CInt(Me.UserID), ex)
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while fetching RN Details with email list.", True, False))
                Me.LogError("Error while fetching RN Details with email list.", CInt(Me.UserID), ex)
            End Try
            Return retVal

        End Function

        Public Function GetRNDetailsWithEmailsByRoleID(RoleIDList As String) As List(Of RN_UserDetailsObject) Implements IUserRNDetailQueries.GetRNDetailsWithEmailsByRoleID
            Dim retVal As New List(Of Objects.RN_UserDetailsObject)
            Try
                Using context As New MAISContext
                    retVal = (From r In context.RNs
                             Join rtypeXrf In context.RN_DD_Person_Type_Xref On rtypeXrf.RN_DDPersonnel_Sid Equals r.RN_Sid
                             Join RTRN_Xref In context.Role_RN_DD_Personnel_Xref On RTRN_Xref.RN_DD_Person_Type_Xref_Sid Equals rtypeXrf.RN_DD_Person_Type_Xref_Sid
                             Where RoleIDList.Contains(RTRN_Xref.Role_Category_Level_Xref.Role_Sid)
                             Select New Objects.RN_UserDetailsObject With {
                                 .RN_Sid = r.RN_Sid,
                                  .RNLicense_Number = r.RNLicense_Number,
                                  .FirstName = r.First_Name,
                                  .LastName = r.Last_Name,
                                  .MiddleName = r.Middle_Name,
                                  .CreateDate = r.Create_Date,
                                  .CreateBy = r.Create_By,
                                  .LastUpdateBy = r.Last_Update_By,
                                  .LastUpdatedDate = r.Last_Updated_Date,
                                  .DateOfOriginalRNLicIssuance = r.Date_Of_Original_Issuance,
                                  .Gender = r.Gender,
                                  .Active_Flg = r.Active_Flg}).ToList

                    For Each RN_ret In retVal
                        RN_ret.EmailList = (From r In context.RNs
                                  Join rn In context.RN_DD_Person_Type_Xref On r.RN_Sid Equals rn.RN_DDPersonnel_Sid
                                  Join en In context.RN_DD_Person_Type_Email_Xref On rn.RN_DD_Person_Type_Xref_Sid Equals en.RN_DD_Person_Type_Xref_Sid
                                  Join em In context.Email1 On em.Email_SID Equals en.Email_Sid
                                                Where r.RN_Sid = RN_ret.RN_Sid
                                                Select New Objects.EmailAddressDetails With {
                                                    .EmailAddressSID = em.Email_SID,
                                                    .EmailAddress = em.Email_Address,
                                                    .ContactType = en.Contact_Type_Sid}).ToList
                    Next

                    Dim FilerList = (From e In retVal
                    Select e).Where(Function(w) w.EmailList.Count > 0).ToList

                    retVal = FilerList

                End Using

            Catch ex As DbEntityValidationException
                Me._messages.Add(New ReturnMessage("Error while fetching RN Details with email list by Role IDs.", True, False))
                Me.LogError("Error while fetching RN Details with email list by Role IDs.", CInt(Me.UserID), ex)
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while fetching RN Details with email list by Role IDs.", True, False))
                Me.LogError("Error while fetching RN Details with email list by Role IDs.", CInt(Me.UserID), ex)
            End Try
            Return retVal

        End Function
    End Class
End Namespace
