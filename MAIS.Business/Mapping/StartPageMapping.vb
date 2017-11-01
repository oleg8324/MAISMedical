Imports MAIS.Data
Namespace Mapping
    Public Class StartPageMapping
        Public Shared Function MapDBToModelStartpageInfo(ByVal certResult As Objects.CertificationEligibleInfo) As Model.CertificationEligibleInfo
            Dim certInfo As New Model.CertificationEligibleInfo
            If (certResult IsNot Nothing) Then
                certInfo.AddOnCertificationReq = certResult.AddOnCertificationReq
                certInfo.CertificationType = certResult.CertificationType
                certInfo.InitialCertificationReq = certResult.InitialCertificationReq
                certInfo.RenewalCertificationReq = certResult.RenewalCertificationReq
            End If
            Return certInfo
        End Function
        Public Shared Function MapDBToModelStartpageToSaveInfo(ByVal certResult As Model.MAISApplicationDetails) As Objects.ApplicationInformation
            Dim appInfo As New Objects.ApplicationInformation
            appInfo.ApplicationID = certResult.ApplicationID
            appInfo.ApplicationStatusTypeID = certResult.ApplicationStatusTypeID
            appInfo.ApplicationTypeID = certResult.ApplicationTypeID
            appInfo.RoleCategoryLevelID = certResult.RoleCategoryLevelID
            appInfo.UniqueCode = certResult.DDPersonnelRNID
            appInfo.RNFlag = certResult.RNFlag
            Return appInfo
        End Function
        Public Shared Function MapDBToModelObjectInfo(ByVal result As Objects.ApplicationInformation) As Model.MAISApplicationDetails
            Dim info As New Model.MAISApplicationDetails
            If (result IsNot Nothing) Then
                info.ApplicationID = result.ApplicationID
                info.ApplicationStatusTypeID = result.ApplicationStatusTypeID
                info.ApplicationTypeID = result.ApplicationTypeID
                info.RoleCategoryLevelID = result.RoleCategoryLevelID
                info.RNFlag = result.RNFlag
            End If
            Return info
        End Function
    End Class
End Namespace
