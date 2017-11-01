Imports MAIS.Data
Namespace Mapping
    Public Class CertificateMapping
        Public Shared Function MapDBToPermCertInformation(ByVal certInfo As List(Of Objects.CertificationInfo), ByVal applicationType As String) As List(Of Model.CertificateInfo)
            Dim list As New List(Of Model.CertificateInfo)
            If (applicationType <> "Renewal") Then
                For Each cert As Objects.CertificationInfo In certInfo
                    Dim lstCert As New Model.CertificateInfo
                    lstCert.DatesOfTraining = cert.StartDatesOfTraining.ToShortDateString() + " " + "-" + " " + cert.EndDatesOfTraining.ToShortDateString()
                    lstCert.LocationOfTraining = cert.LocationOfTraining
                    lstCert.OBNNumber = cert.OBNNumber
                    If (Not String.IsNullOrEmpty(cert.RNMiddleInstructorName)) Then
                        lstCert.RNInstructorName = cert.RNInstructorName + " " + cert.RNMiddleInstructorName + " " + cert.RNLastInstructorName
                    Else
                        lstCert.RNInstructorName = cert.RNInstructorName + " " + cert.RNLastInstructorName
                    End If
                    If (String.IsNullOrWhiteSpace(lstCert.RNInstructorName)) Then
                        lstCert.RNInstructorName = "DODD Admin"
                    End If
                    If (Not String.IsNullOrEmpty(cert.RNMiddleName)) Then
                        lstCert.RNName = cert.RNName + " " + cert.RNMiddleName + " " + cert.RNLastName
                    Else
                        lstCert.RNName = cert.RNName + " " + cert.RNLastName
                    End If
                    lstCert.SponsorName = cert.SponsorName
                    lstCert.TotalACEs = cert.TotalACEs.ToString()
                    lstCert.TotalCEs = cert.TotalCEs
                    lstCert.DatesOfCertification = cert.StartDatesOfCertification.ToShortDateString() + " " + "-" + " " + cert.EndDatesOfCertification.ToShortDateString()
                    list.Add(lstCert)
                Next
            End If
            If (applicationType = "Renewal") Then
                list = (From cert In certInfo
                        Select New Model.CertificateInfo With {
                .DatesOfTraining = String.Empty,
                .LocationOfTraining = String.Empty,
                .OBNNumber = String.Empty,
                .RNInstructorName = String.Empty,
                    .RNName = If(Not String.IsNullOrEmpty(cert.RNMiddleName), cert.RNName + " " + cert.RNMiddleName + " " + cert.RNLastName, cert.RNName + " " + cert.RNLastName),
                .SponsorName = String.Empty,
                .TotalACEs = String.Empty,
                .TotalCEs = String.Empty,
                .DatesOfCertification = cert.StartDatesOfCertification.ToShortDateString() + " " + "-" + " " + cert.EndDatesOfCertification.ToShortDateString()
                            }).ToList
            End If
            Return list
        End Function
        Public Shared Function MapDBToPermDDCertInformation(ByVal certInfo As List(Of Objects.CertificationInfo), ByVal applicationType As String) As List(Of Model.CertificateDDInfo)
            Dim list As New List(Of Model.CertificateDDInfo)
            If (applicationType <> "Renewal") Then
                list = (From cert In certInfo
                        Select New Model.CertificateDDInfo With {
                .RNTrainerName = If(Not String.IsNullOrEmpty(cert.RNMiddleInstructorName), cert.RNInstructorName + " " + cert.RNMiddleInstructorName + " " + cert.RNLastInstructorName, cert.RNInstructorName + " " + cert.RNLastInstructorName),
                .DDName = If(Not String.IsNullOrEmpty(cert.RNMiddleName), cert.RNName + " " + cert.RNMiddleName + " " + cert.RNLastName, cert.RNName + " " + cert.RNLastName),
            .TotalCEs = cert.TotalCEs,
            .CertificateDatesOfTraining = cert.StartDatesOfCertification.ToShortDateString() + " " + "To" + " " + cert.EndDatesOfCertification.ToShortDateString(),
            .EndDatesOfTraining = cert.EndDatesOfTraining.ToShortDateString(),
            .StartDatesOfTraining = cert.StartDatesOfTraining.ToShortDateString()
                            }).ToList
            End If
            If (applicationType = "Renewal") Then
                For Each cert As Objects.CertificationInfo In certInfo
                    Dim lstCert As New Model.CertificateDDInfo
                    If (Not String.IsNullOrEmpty(cert.RNMiddleInstructorName)) Then
                        lstCert.RNTrainerName = cert.RNInstructorName + " " + cert.RNMiddleInstructorName + " " + cert.RNLastInstructorName
                    Else
                        lstCert.RNTrainerName = cert.RNInstructorName + " " + cert.RNLastInstructorName
                    End If
                    If (String.IsNullOrWhiteSpace(lstCert.RNTrainerName)) Then
                        lstCert.RNTrainerName = "DODD Admin"
                    End If
                    If (Not String.IsNullOrEmpty(cert.RNMiddleName)) Then
                        lstCert.DDName = cert.RNName + " " + cert.RNMiddleName + " " + cert.RNLastName
                    Else
                        lstCert.DDName = cert.RNName + " " + cert.RNLastName
                    End If
                    lstCert.TotalCEs = String.Empty
                    lstCert.CertificateDatesOfTraining = cert.StartDatesOfCertification.ToShortDateString() + " " + "to" + " " + cert.EndDatesOfCertification.ToShortDateString()
                    lstCert.EndDatesOfTraining = String.Empty
                    lstCert.StartDatesOfTraining = String.Empty
                    list.Add(lstCert)
                Next
            End If
            Return list
        End Function
        Public Shared Function MapDBToPermCertDetailsInformation(ByVal certDetail As List(Of Objects.CertificationDetails)) As List(Of Model.CertificationDetails)
            Dim list As New List(Of Model.CertificationDetails)
            list = (From cert In certDetail
                    Select New Model.CertificationDetails With {
                        .ApplicationType = cert.ApplicationType,
            .CertificateID = cert.CertificateID,
            .RoleLevelCategoryID = cert.RoleLevelCategoryID
                        }).ToList
            Return list
        End Function
    End Class
End Namespace
