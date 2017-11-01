Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data
Imports MAIS.Data.Objects
Imports System.Data.Linq
Imports System.Configuration

Namespace Services
    Public Interface IRN_AttestationService
        Inherits IBusinessBase

        ' Function InsertRN_Attestation(rn_Attestation As List(Of Model.RN_AttestationEntity)) As Boolean
        Function InsertRn_AttestationPanel(rn_AttestatonPanel As Model.RN_AttestationPanel, ByVal Attestant_SID As Integer) As Boolean
        Function GetRn_AttestationEntitiesByApplicationID_Attestation_ApplicationType_Xref_Sid(ByVal applicationID As Long, AttApplicationtype_xrefSid As Long) As List(Of Model.RN_AttestationEntity)
        Function GetRn_AttestationPanelbyApplicaitonID_Attestation_Applicationtype_xrefSid(ByVal applicationID As Long, Attapplicationtyp_xrefSid As Long) As Business.Model.RN_AttestationPanel
        Function GetRN_AttestationQuestionForPage(ByVal RoleID As Integer, ByVal ApplicationTypeID As Integer) As List(Of Model.AttestationQuestions)
        Function DeleteRn_AttestationEntitieByEntityID(ByVal entityid As Long) As Boolean
        Function DeleteRn_AttestationByID(ByVal Application_SiD As Integer, ByVal Attestatin_Applicationtypt_xref_Sid As Long) As Boolean
        Function GetAttestationForpageComplete(ByVal applicationID As Integer) As Integer
    End Interface



    Public Class RN_AttestationService
        Inherits BusinessBase
        Implements IRN_AttestationService



        Private _queries As Data.Queries.IRN_AttestationQueries


        <Obsolete("Use StructureMap.objectFactory.Getinstance(Of IRN_AttestationService)() instead!", True)> _
        Public Sub New()
            Throw New NotImplementedException("Method not usable. User StructureMap.ObjectFactory.GetInstance(of IRN_AttestationService)() instead!")
        End Sub

        Public Sub New(ByVal user As IUserIdentity, ByVal maisConnectionString As IConnectionIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of Data.Queries.RN_AttestationQueries)()
            _queries.UserID = user.UserID
            _queries.MAISConnectionString = maisConnectionString.ConnectionString
        End Sub
        Public Function DeleteRn_AttestationByID(Application_SiD As Integer, Attestatin_Applicationtypt_xref_Sid As Long) As Boolean Implements IRN_AttestationService.DeleteRn_AttestationByID
            Return Nothing
        End Function

        Public Function DeleteRn_AttestationEntitieByEntityID(entityid As Long) As Boolean Implements IRN_AttestationService.DeleteRn_AttestationEntitieByEntityID
            Return Nothing
        End Function

        Public Function GetRn_AttestationEntitiesByApplicationID_Attestation_ApplicationType_Xref_Sid(applicationID As Long, AttApplicationtype_xrefSid As Long) As List(Of RN_AttestationEntity) Implements IRN_AttestationService.GetRn_AttestationEntitiesByApplicationID_Attestation_ApplicationType_Xref_Sid
            Return New List(Of RN_AttestationEntity)


        End Function

        Public Function GetRn_AttestationPanelbyApplicaitonID_Attestation_Applicationtype_xrefSid(applicationID As Long, Attapplicationtyp_xrefSid As Long) As RN_AttestationPanel Implements IRN_AttestationService.GetRn_AttestationPanelbyApplicaitonID_Attestation_Applicationtype_xrefSid
            Dim retAttestationPanelInfoModel As New RN_AttestationPanel
            Try
                retAttestationPanelInfoModel = Mapping.AttestationPageMapping.MapDBToModelAttestationPanel(Me._queries.GetRn_AttestationPanlebyApplicaitonID_Attestation_Applicationtype_xrefSid(applicationID, Attapplicationtyp_xrefSid))

            Catch ex As Exception
                Me.LogError("Error pulling Attestation Panel information for the RN_Attestation page.", CInt(Me.UserID), ex)

            End Try
            Return retAttestationPanelInfoModel

        End Function

        'Public Function InsertRN_Attestation(rn_Attestation As List(Of RN_AttestationEntity)) As Boolean Implements IRN_AttestationService.InsertRN_Attestation
        '    Dim retVal As Boolean = False

        '    Try
        '        _queries.InsertRN_Attestation(Mapping.AttestationPageMapping.MapDBToModelAttestationEntity(rn_Attestation))
        '        If rn_Attestation.Count > 0 Then
        '            retVal = _queries.InsertRn_AttestationAnswer(rn_Attestation(0).Application_SID, rn_Attestation(0).Attestation_ApplicationType_Xref_Sid, rn_Attestation(0).Acceptance_Flg)

        '        End If
        '        If rn_Attestation.Count <> 0 Then
        '            Me._messages.AddRange(_queries.Messages)
        '            Me._queries.Messages.Clear()
        '        End If
        '    Catch ex As Exception
        '        If Me._queries.Messages.Count <> 0 Then
        '            Me._messages.AddRange(Me._queries.Messages)
        '            For Each message In Me._queries.Messages
        '                If message.IsError Then
        '                    Me.LogError(message.ToString(), ex)
        '                ElseIf message.IsWarning Then
        '                    Me.LogWarning(message.ToString(), ex)

        '                End If

        '            Next
        '            Me._queries.Messages.Clear()
        '        Else
        '            Me._messages.AddRange(New ODMRDDHelperClassLibrary.Utility.ReturnMessage("Error while populating Application Attestation Entity for Attestation mapping.", True, False))
        '        End If
        '    End Try
        '    Return retVal

        'End Function

        Public Function InsertRn_AttestationPanel(rn_AttestatonPanel As RN_AttestationPanel, Attestant_SID As Integer) As Boolean Implements IRN_AttestationService.InsertRn_AttestationPanel
            Dim retVal As Boolean = False

            Try
                retVal = _queries.InsertRn_AttestationAnswer(rn_AttestatonPanel.Application_SID, rn_AttestatonPanel.Attestation_SID, rn_AttestatonPanel.YesNo, Attestant_SID)

                If _queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(_queries.Messages)
                    Me._queries.Messages.Clear()
                End If
            Catch ex As Exception
                Me.LogError("InsertRn_AttestationPanel.", CInt(Me.UserID), ex)
                If Me._queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(Me._queries.Messages)
                    For Each message In Me._queries.Messages
                        If message.IsError Then
                            Me.LogError(message.ToString(), CInt(Me.UserID), ex)
                        ElseIf message.IsWarning Then
                            Me.LogWarning(message.ToString(), ex)
                        End If
                    Next
                    Me._queries.Messages.Clear()
                Else
                    Me.LogError(ex.Message, CInt(Me.UserID), ex)
                End If

            End Try
            Return retVal

        End Function

        Public Function GetRN_AttestationQuestionForPage(RoleID As Integer, ApplicationTypeID As Integer) As List(Of Model.AttestationQuestions) Implements IRN_AttestationService.GetRN_AttestationQuestionForPage
            Dim returnListofAttestationQuestionModel As New List(Of Model.AttestationQuestions)
            Try
                returnListofAttestationQuestionModel.AddRange(Mapping.AttestationPageMapping.MapDBtoModelAttestationQuestions(Me._queries.GetRN_AttestationQuestionForPage(RoleID, ApplicationTypeID)))

            Catch ex As Exception
                Me.LogError("Error pulling Attestation Questiong for the RN_Attestation page.", CInt(Me.UserID), ex)
            End Try
            Return returnListofAttestationQuestionModel
        End Function
        Public Function GetAttestationForpageComplete(ByVal applicationID As Integer) As Integer Implements IRN_AttestationService.GetAttestationForpageComplete
            Dim exists As Integer = 0
            Try
                exists = _queries.GetAttestationForpageComplete(applicationID)
            Catch ex As Exception
                Me.LogError("Error Getting document upload page complete rule.", CInt(Me.UserID), ex)
            End Try
            Return exists
        End Function
        
    End Class
End Namespace