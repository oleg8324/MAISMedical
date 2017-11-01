Imports ODMRDDHelperClassLibrary.Utility
Imports System.Data.Entity.Validation
Imports System.Data.Objects

Namespace Queries
    Public Interface ISearchQueries
        Inherits IQueriesBase

        Function GetRNInfoFromTables(ByVal searchCriteria As Objects.MAISSearchCriteria) As List(Of USP_Get_RN_Search_Results_Result)
        Function GetDDInfoFromTables(ByVal searchCriteria As Objects.MAISSearchCriteria, ByVal ssnCriteria As Boolean) As List(Of USP_Get_DDPersonnel_Search_Results_Result)
        Function GetRRDDAsMoreThanThreeNotation(ByVal flag As Boolean, ByVal UniqueID As String) As Boolean

    End Interface
    Public Class SearchQueries
        Inherits QueriesBase
        Implements ISearchQueries
        Public Function GetRNInfoFromTables(searchCriteria As Objects.MAISSearchCriteria) As List(Of USP_Get_RN_Search_Results_Result) Implements ISearchQueries.GetRNInfoFromTables
            Dim ret As New List(Of USP_Get_RN_Search_Results_Result)
            Dim retVal As List(Of USP_Get_RN_Search_Results_Result) = Nothing
            Dim retVal1 As New List(Of USP_Get_RN_Search_Results_Result)
            Try
                Using context As New MAISContext()
                    ret = context.USP_Get_RN_Search_Results(searchCriteria.ApplicationID, searchCriteria.RNLicenseNumber, searchCriteria.LastName, searchCriteria.FirstName, searchCriteria.EmployerName,
                                                             searchCriteria.ApplicationStatus, searchCriteria.DateOfBirth).ToList()
                    For Each search As USP_Get_RN_Search_Results_Result In ret
                        retVal = ret.FindAll(Function(rn) rn.RNLicenseNumber = search.RNLicenseNumber)
                        If (retVal.Count > 1) Then
                            'ret.RemoveAll(Function(rn1) rn1.RNLicenseNumber = search.RNLicenseNumber And rn1.Permanent_Flg = 0)
                            'retVal.AddRange(ret.Where(Function(fn) fn.Permanent_Flg = 1))
                            If (search.Permanent_Flg = 1) Then
                                retVal1.Add(search)
                            End If
                        Else
                            retVal1.Add(search)
                        End If
                    Next
                End Using
            Catch ex As Exception
                Me.LogError("Error in Getting RN Info. for search page", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error in Getting RN Info. for search page", True, False))
                Throw
            End Try
            Return retVal1
        End Function
        Public Function GetDDInfoFromTables(searchCriteria As Objects.MAISSearchCriteria, ByVal ssnCriteria As Boolean) As List(Of USP_Get_DDPersonnel_Search_Results_Result) Implements ISearchQueries.GetDDInfoFromTables
            Dim search As New List(Of USP_Get_DDPersonnel_Search_Results_Result)
            Dim ddInfoFromPermanent As New List(Of USP_Get_DDPersonnel_Search_Results_Result)
            Dim ddInfoFromTemp As List(Of USP_Get_DDPersonnel_Search_Results_Result) = Nothing
            Try
                Using context As New MAISContext()
                    If ssnCriteria = False Then
                        searchCriteria.Last4SSN = ""
                    End If
                    ddInfoFromPermanent = context.USP_Get_DDPersonnel_Search_Results(searchCriteria.ApplicationID, searchCriteria.Last4SSN, searchCriteria.DDcode, searchCriteria.LastName, searchCriteria.FirstName, searchCriteria.EmployerName,
                                                             searchCriteria.ApplicationStatus, searchCriteria.DateOfBirth).ToList()
                    For Each ddDetails As USP_Get_DDPersonnel_Search_Results_Result In ddInfoFromPermanent
                        ddInfoFromTemp = ddInfoFromPermanent.FindAll(Function(rn) rn.SSN = ddDetails.SSN And rn.FirstName = ddDetails.FirstName And rn.LastName = ddDetails.LastName And rn.DOB = ddDetails.DOB)
                        If (ddInfoFromTemp.Count > 1) Then
                            If (ddDetails.Permanent_Flg = 1) Then
                                search.Add(ddDetails)
                            End If
                        Else
                            search.Add(ddDetails)
                        End If
                    Next
                End Using
            Catch ex As Exception
                Me.LogError("Error in Getting DD personnel Info for search page", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error in Getting DD personnel Info. for search page", True, False))
                Throw
            End Try
            Return search
        End Function
        Public Function GetRRDDAsMoreThanThreeNotation(flag As Boolean, UniqueID As String) As Boolean Implements ISearchQueries.GetRRDDAsMoreThanThreeNotation
            Dim notationFlag As Boolean = False
            Using context As New MAISContext()
                Try
                    Dim DatePlus18 As Date = Date.Today.AddMonths(-18)
                    If (flag) Then
                        Dim _count As Integer = (From notation In context.Notations _
                                                 Join rnDDXref In context.RN_DD_Person_Type_Xref On rnDDXref.RN_DD_Person_Type_Xref_Sid Equals notation.RN_DD_Person_Type_Xref_Sid _
                                                 Join rnDD In context.RNs On rnDD.RN_Sid Equals rnDDXref.RN_DDPersonnel_Sid
                                                 Where rnDD.RNLicense_Number = UniqueID And notation.Notation_Date > DatePlus18 And notation.UnFlagged_Date.HasValue = False
                                                 Select notation).Count()
                        If (_count >= 4) Then
                            notationFlag = True
                        End If
                    Else
                        Dim _count As Integer = (From notation In context.Notations _
                                                 Join rnDDXref In context.RN_DD_Person_Type_Xref On rnDDXref.RN_DD_Person_Type_Xref_Sid Equals notation.RN_DD_Person_Type_Xref_Sid _
                                                 Join rnDD In context.DDPersonnels On rnDD.DDPersonnel_Sid Equals rnDDXref.RN_DDPersonnel_Sid
                                                 Where rnDD.DDPersonnel_Code = UniqueID And notation.Notation_Date > DatePlus18 And notation.UnFlagged_Date.HasValue = False
                                                 Select notation).Count()
                        If (_count >= 4) Then
                            notationFlag = True
                        End If
                    End If
                Catch ex As Exception
                    Me.LogError("Error Getting in notation flag for employer Info", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting in notation flag for employer Info.", True, False))
                End Try
            End Using
            Return notationFlag
        End Function
    End Class
End Namespace
