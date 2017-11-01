Imports MAIS.Data

Namespace Mapping
    Public Class AttestationPageMapping
        Public Shared Function MapDBToModelAttestationEntity(ByVal dbApplicationAttestation As IEnumerable(Of Model.RN_AttestationEntity)) As List(Of Data.Application_Attestation)
            Return (From ade In dbApplicationAttestation _
                    Select New Data.Application_Attestation With _
                           {
                               .Application_Attestation_Sid = ade.Application_Attestation_SID,
                               .Application_Sid = ade.Application_SID,
                               .Acceptance_Flg = ade.Acceptance_Flg,
                               .Attestation_Sid = ade.Attestation_Sid,
                               .Create_By = ade.CreateBy,
                               .Create_Date = ade.CreateDate,
                               .Last_Update_By = ade.LastUpdateBy,
                               .Last_Update_Date = ade.LastUpdateDate
                               }).ToList

        End Function

        Public Shared Function MapDBToModelAttestationPanel(ByVal db As Objects.AttestationPanel) As Model.RN_AttestationPanel
            Dim PanelInfo As New Model.RN_AttestationPanel
            If Not (db Is Nothing) Then
                With PanelInfo
                    .Application_SID = db.Application_SID
                    .Attestation_SID = db.Attestation_SID
                    .YesNo = db.YesNo
                End With
            End If

            Return PanelInfo

        End Function
        Public Shared Function MapDBtoModelAttestationQuestions(ByVal dbAttestationQuestions As List(Of Objects.AttestationQuestions)) As List(Of Model.AttestationQuestions)
            Dim QuestionInfo As New List(Of Model.AttestationQuestions)

            QuestionInfo = (From a In dbAttestationQuestions _
                            Select New Model.AttestationQuestions With
                            {
                                .Attestation_SID = a.Attestation_SID,
                                .ApplicationType_Sid = a.ApplicationType_Sid,
                                .Attestation_ApplicationType_Xref_Sid = a.Attestation_ApplicationType_Xref_Sid,
                                .AttestationDesc = a.AttestationDesc,
                                .EndDate = a.EndDate,
                                .Role_Sid = a.Role_Sid,
                                .StartDate = a.StartDate
                                }).ToList

            Return QuestionInfo


            'Return (From at In dbAttestationQuestions _
            '        Join aat In dbApp_Attestation On at.Attestation_SID Equals aat.Attestation_SID _
            '        Select New Model.AttestationQuestions With
            '               {
            '                   .Attestation_SID = aat.Attestation_SID,
            '                   .ApplicationType_Sid = aat.ApplicationType_Sid,
            '                   .Attestation_ApplicationType_Xref_Sid = aat.Attestation_ApplicationType_Xref_Sid,
            '                   .AttestationDesc = at.AttestationDesc,
            '                   .Role_Sid = aat.Role_Sid,
            '                    .StartDate = at.StartDate,
            '                   .EndDate = at.EndDate})


        End Function
    End Class
End Namespace