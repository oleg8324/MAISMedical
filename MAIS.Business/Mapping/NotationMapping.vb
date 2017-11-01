Imports MAIS.Data

Namespace Mapping
    Public Class NotationMapping
        Public Shared Function MapNotationToDB(ByVal notation As Model.NotationDetails) As Objects.NotationObject
            If notation Is Nothing Then
                Return Nothing
            Else
                Return New Objects.NotationObject() With
                       {
                           .NotationDate = notation.NotationDate,
                           .NotationReasons = MapNReasonListToData(notation.NotationReasons),
                           .NotationType = MapNTypeListToData(notation.NotationType),
                           .OccurenceDate = notation.OccurenceDate,
                           .PersonEnteringNotation = notation.PersonEnteringNotation,
                           .PersonTitle = notation.PersonTitle,
                           .UnflaggedDate = notation.UnflaggedDate,
                           .AllReasons = notation.AllReasons,
                           .AppId = notation.AppId
                       }

            End If
        End Function
        Public Shared Function MapNTypeToModel(ByVal nt As Objects.NType) As Model.NType
            Return (New Model.NType With {.NTypeDesc = nt.NTypeDesc, .NTypeSid = nt.NTypeSid})
        End Function
        Public Shared Function MapNReasonToModel(ByVal nt As Objects.NReason) As Model.NReason
            Return (New Model.NReason With {.NReasonDesc = nt.NReasonDesc, .NReasonSid = nt.NReasonSid})
        End Function
        Public Shared Function MapNTypeListToModel(ByVal nt As List(Of Objects.NType)) As List(Of Model.NType)
            Dim mntypelist As New List(Of Model.NType)
            If (nt.Count > 0) Then
                mntypelist = (From nte In nt
                              Select New Model.NType With {
                                     .NTypeDesc = nte.NTypeDesc,
                                    .NTypeSid = nte.NTypeSid
                                  }).ToList()
            End If
            'For Each nte As Objects.NType In nt
            '    mntypelist.Add(New Model.NType With {.NTypeDesc = nte.NTypeDesc, .NTypeSid = nte.NTypeSid})
            'Next
            Return mntypelist
        End Function
        Public Shared Function MapNReasonListToData(ByVal nt As List(Of Model.NReason)) As List(Of Objects.NReason)
            Dim mntypelist As New List(Of Objects.NReason)
            If (nt.Count > 0) Then
                mntypelist = (From nte In nt
                              Select New Objects.NReason With {
                                     .NReasonDesc = nte.NReasonDesc,
                                     .NReasonSid = nte.NReasonSid
                                  }).ToList()
            End If
            'For Each nte As Model.NReason In nt
            '    mntypelist.Add(New Objects.NReason With {.NReasonDesc = nte.NReasonDesc, .NReasonSid = nte.NReasonSid})
            'Next
            Return mntypelist
        End Function
        Public Shared Function MapNReasonListToModel(ByVal nt As List(Of Objects.NReason)) As List(Of Model.NReason)
            Dim mntypelist As New List(Of Model.NReason)
            If (nt.Count > 0) Then
                mntypelist = (From nte In nt
                              Select New Model.NReason With {
                                     .NReasonDesc = nte.NReasonDesc,
                                     .NReasonSid = nte.NReasonSid
                                  }).ToList()
            End If
            'For Each nte As Objects.NReason In nt
            '    mntypelist.Add(New Model.NReason With {.NReasonDesc = nte.NReasonDesc, .NReasonSid = nte.NReasonSid})
            'Next
            Return mntypelist
        End Function
        Public Shared Function MapCertStatusListToModel(ByVal nt As List(Of Objects.CertStatus)) As List(Of Model.CertStatus)
            Dim mntypelist As New List(Of Model.CertStatus)
            If (nt.Count > 0) Then
                mntypelist = (From nte In nt
                              Select New Model.CertStatus With {
                                    .CertStatusDesc = nte.CertStatusDesc,
                                    .CertStatusSid = nte.CertStatusSid
                                  }).ToList()
            End If
            'For Each nte As Objects.CertStatus In nt
            '    mntypelist.Add(New Model.CertStatus With {.CertStatusDesc = nte.CertStatusDesc, .CertStatusSid = nte.CertStatusSid})
            'Next
            Return mntypelist
        End Function
        Public Shared Function MapNTypeListToData(ByVal nt As Model.NType) As Objects.NType
            Return (New Objects.NType With {.NTypeDesc = nt.NTypeDesc, .NTypeSid = nt.NTypeSid})
        End Function
        Public Shared Function MapNReasonToData(ByVal nt As Model.NReason) As Objects.NReason
            Return (New Objects.NReason With {.NReasonDesc = nt.NReasonDesc, .NReasonSid = nt.NReasonSid})
        End Function
        Public Shared Function MapDBToNotation(ByVal notation As Objects.NotationObject) As Model.NotationDetails
            If notation Is Nothing Then
                Return Nothing
            Else
                Dim NotDet As New Model.NotationDetails
                NotDet.NotationDate = notation.NotationDate
                NotDet.OccurenceDate = notation.OccurenceDate
                NotDet.PersonEnteringNotation = notation.PersonEnteringNotation
                NotDet.PersonTitle = notation.PersonTitle
                NotDet.UnflaggedDate = notation.UnflaggedDate
                NotDet.AllReasons = notation.AllReasons
                NotDet.AppNotId = notation.AppNotId
                NotDet.AppId = notation.AppId
                Dim t As New Model.NType
                t.NTypeDesc = notation.NotationType.NTypeDesc
                t.NTypeSid = notation.NotationType.NTypeSid
                NotDet.NotationType = t
                If (notation.NotationReasons IsNot Nothing) Then
                    Dim nrl As New List(Of Model.NReason)
                    nrl = (From nr In notation.NotationReasons Select New Model.NReason With {.NReasonDesc = nr.NReasonDesc, .NReasonSid = nr.NReasonSid}).ToList

                    NotDet.NotationReasons = nrl
                End If
                Return NotDet

            End If
        End Function

    End Class
End Namespace