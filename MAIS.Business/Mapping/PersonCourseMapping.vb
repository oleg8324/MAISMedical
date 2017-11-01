Imports MAIS.Data

Namespace Mapping
    Public Class PersonCourseMapping
        Public Shared Function MapDBToPersonCourseSessiong(ByVal db As Objects.PersonCourseSession) As Model.PersonCourseSession
            Dim retObj As New Model.PersonCourseSession
            If db IsNot Nothing Then
                With retObj
                    .PersonCourseSessionXref = db.PersonCourseSessionXref
                    .PersonCourseXrefSid = db.PersonCourseXrefSid
                    .ActiveFlg = db.ActiveFlg
                    .SessionSid = db.SessionSid
                End With
            End If

            Return retObj

        End Function
    End Class
End Namespace


