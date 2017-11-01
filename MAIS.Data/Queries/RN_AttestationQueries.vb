Imports System.Data.Linq
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Data.Objects

Namespace Queries
    Public Interface IRN_AttestationQueries
        Inherits IQueriesBase

        '  Function InsertRN_Attestation(rn_Attestation As List(Of Data.Application_Attestation)) As List(Of Long)
        Function InsertRn_AttestationAnswer(ByVal Application_sid As Long, attestation_ApplicationType_Xref As Long, ByVal YesNo As String, ByVal Attestant_SID As Integer) As Boolean
        Function GetRn_AttestationEntitiesByApplicationIDAndAttApplicationtypeID(ByVal applicationID As Long, ByVal AttApplicationtype_xrefSid As Long) As List(Of Data.Application_Attestation)
        Function GetRn_AttestationPanlebyApplicaitonID_Attestation_Applicationtype_xrefSid(ByVal applicationID As Long, Attapplicationtyp_xrefSid As Long) As Objects.AttestationPanel
        Function DeleteRn_AttestationEntitieByEntityID(ByVal entityid As Long) As Boolean
        Function DeleteRn_AttestationByID(ByVal Application_SiD As Integer, ByVal Attestatin_Applicationtypt_xref_Sid As Long) As Boolean

        Function GetRN_AttestationQuestionForPage(RoleID As Integer, ApplicationTypeID As Integer) As List(Of Objects.AttestationQuestions)
        Function GetAttestationForpageComplete(ByVal applicationID As Integer)
    End Interface

    Public Class RN_AttestationQueries
        Inherits QueriesBase
        Implements IRN_AttestationQueries

        Public Function DeleteRn_AttestationByID(Application_SiD As Integer, Attestatin_Applicationtypt_xref_Sid As Long) As Boolean Implements IRN_AttestationQueries.DeleteRn_AttestationByID
            Return Nothing
        End Function

        Public Function DeleteRn_AttestationEntitieByEntityID(entityid As Long) As Boolean Implements IRN_AttestationQueries.DeleteRn_AttestationEntitieByEntityID
            Return Nothing
        End Function

        Public Function GetRn_AttestationEntitiesByApplicationIDAndAttApplicationtypeID(applicationID As Long, AttApplicationtype_xrefSid As Long) As List(Of Data.Application_Attestation) Implements IRN_AttestationQueries.GetRn_AttestationEntitiesByApplicationIDAndAttApplicationtypeID
            Dim _returnApplicationAttestations As New List(Of Data.Application_Attestation)

            Using context As New MAISContext()
                Try
                    _returnApplicationAttestations = (From a In context.Application_Attestation _
                                                      Where a.Application_Sid = applicationID And a.Attestation_Sid = AttApplicationtype_xrefSid
                                                      Select a).ToList


                Catch ex As Exception
                    Me.LogError("Error while retrieving Application_Attestation rows.", CInt(Me.UserID), ex)
                    Throw ex
                End Try

            End Using

            Return _returnApplicationAttestations

        End Function

        Public Function GetRn_AttestationPanlebyApplicaitonID_Attestation_Applicationtype_xrefSid(applicationID As Long, Attapplicationtyp_xrefSid As Long) As Objects.AttestationPanel Implements IRN_AttestationQueries.GetRn_AttestationPanlebyApplicaitonID_Attestation_Applicationtype_xrefSid
            Dim _returnAttestationPanel As New Objects.AttestationPanel

            Using context As New MAISContext
                Try
                    _returnAttestationPanel = (From a In context.Application_Attestation _
                                               Join b In context.Attestation_Application_Type_Role_Xref On a.Attestation_Sid Equals b.Attestation_Application_Type_Role_Xref_Sid _
                                               Where a.Application_Sid = applicationID And a.Attestation_Sid = Attapplicationtyp_xrefSid _
                                               Select New Objects.AttestationPanel With {
                                                   .Application_SID = a.Application_Sid,
                                                   .Attestation_SID = b.Attestation_Sid,
                                                   .YesNo = a.Acceptance_Flg}).FirstOrDefault
                Catch ex As Exception
                    Me.LogError("Error while retreiving Attestation row information for the panel", CInt(Me.UserID), ex)
                    Throw ex
                End Try
            End Using
            Return _returnAttestationPanel

        End Function

        'Public Function InsertRN_Attestation(rn_Attestation As List(Of Application_Attestation)) As List(Of Long) Implements IRN_AttestationQueries.InsertRN_Attestation
        '    Dim insertedIDs As New List(Of Long)

        '    Try
        '        Using context As New MAISContext()
        '            For Each misc As Data.Application_Attestation In rn_Attestation

        '            Next

        '        End Using

        '    Catch ex As Exception

        '    End Try
        '    Return Nothing
        'End Function

        Public Function InsertRn_AttestationAnswer(Application_sid As Long, attestation_ApplicationType_Xref As Long, YesNo As String, Attestant_SID As Integer) As Boolean Implements IRN_AttestationQueries.InsertRn_AttestationAnswer
            Dim retVal As Boolean = False
            Try
                Using context As New MAISContext
                    Dim _returnApplicationAttestation As New Data.Application_Attestation
                    _returnApplicationAttestation = (From a In context.Application_Attestation _
                        Where a.Application_Sid = Application_sid And a.Attestation_Sid = attestation_ApplicationType_Xref
                        Select a).FirstOrDefault

                    If (_returnApplicationAttestation Is Nothing) Then
                        Dim _InsertAppAttestation As New Data.Application_Attestation
                        With _InsertAppAttestation
                            .Application_Sid = Application_sid
                            .Acceptance_Flg = YesNo
                            .Attestation_Sid = attestation_ApplicationType_Xref
                            .Create_Date = Now()
                            .Create_By = Me.UserID
                            .Last_Update_By = Me.UserID
                            .Last_Update_Date = Now()
                        End With
                        context.Application_Attestation.Add(_InsertAppAttestation)
                    Else
                        With _returnApplicationAttestation
                            .Acceptance_Flg = YesNo
                            .Last_Update_By = Me.UserID
                            .Last_Update_Date = Now()
                        End With

                    End If
                    'Below is the code to update attestaion date in application history JH 5/14/2013
                    If Application_sid > 0 Then
                        Dim apphis As Application_History = (From ah In context.Application_History
                                                             Where ah.Application_Sid = Application_sid
                                                             Select ah).FirstOrDefault()
                        If apphis IsNot Nothing Then
                            If (Attestant_SID > 0) Then
                                apphis.Attestant_SId = Attestant_SID
                            End If

                            apphis.Attestation_Date = DateTime.Now
                            apphis.Last_Update_By = Me.UserID
                            apphis.Last_Update_Date = DateTime.Now
                        End If
                    End If
                    context.SaveChanges()
                    retVal = True
                End Using

            Catch ex As Exception
                Me.LogError("Error on Inserting into the Application_Attestation Info", CInt(Me.UserID), ex)
                Throw ex
            End Try

            Return retVal

        End Function


        Public Function GetRN_AttestationQuestionForPage(RoleID As Integer, ApplicationTypeID As Integer) As List(Of AttestationQuestions) Implements IRN_AttestationQueries.GetRN_AttestationQuestionForPage
            Dim returnListAttestationQuestionsforPage As New List(Of Objects.AttestationQuestions)
            Dim Role_Category_Role_Sid As Integer

            Using context As New MAISContext
                Role_Category_Role_Sid = (From rC In context.Role_Category_Level_Xref _
                                          Where rC.Role_Category_Level_Sid = RoleID
                                          Select rC.Role_Sid).FirstOrDefault

                returnListAttestationQuestionsforPage = (From a In context.Attestations _
                                                         Join b In context.Attestation_Application_Type_Role_Xref On a.Attestation_Sid Equals b.Attestation_Sid _
                                                         Where b.Role_Sid = Role_Category_Role_Sid And b.Application_Type_Sid = ApplicationTypeID And a.Start_Date <= Now() And a.End_Date >= Now() _
                                                         Select New Objects.AttestationQuestions With {
                                                             .ApplicationType_Sid = b.Application_Type_Sid,
                                                             .Attestation_ApplicationType_Xref_Sid = b.Attestation_Application_Type_Role_Xref_Sid,
                                                             .Attestation_SID = b.Attestation_Sid,
                                                             .AttestationDesc = a.Attestation_Desc,
                                                             .Role_Sid = b.Role_Sid,
                                                             .StartDate = a.Start_Date,
                                                             .EndDate = a.End_Date}).ToList
            End Using
            Return returnListAttestationQuestionsforPage

        End Function

        Public Function GetAttestationForpageComplete(applicationID As Integer) As Object Implements IRN_AttestationQueries.GetAttestationForpageComplete
            Dim exists As Integer = 1
            Dim exists1 As Integer = 0
            Dim attestantID As Integer?
            Dim IsAddmen As Boolean = False
            Using context As New MAISContext()
                Try
                    'exists1 = (From app In context.Application_Attestation _
                    '                                        Where app.Application_Sid = applicationID And app.Acceptance_Flg = False Select app.Application_Sid).FirstOrDefault()
                    attestantID = (From app In context.Applications Where app.Application_Sid = applicationID And app.Attestant_Sid.HasValue Select app.Attestant_Sid).FirstOrDefault()
                    If (attestantID.HasValue) Then 'And exists1 = 0) Then
                        exists = 0
                    End If
                    IsAddmen = (From app In context.Applications Where app.Application_Sid = applicationID Select app.Is_Admin_Flg).FirstOrDefault()
                    If IsAddmen = True Then
                        exists = 0
                    End If
                Catch ex As Exception
                    Me.LogError("Error Getting attestation page complete rule.", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error attestation page complete rule.", True, False))
                    Throw
                End Try
            End Using
            Return exists
        End Function
    End Class
End Namespace