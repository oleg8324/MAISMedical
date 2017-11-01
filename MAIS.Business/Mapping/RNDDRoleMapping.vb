Imports MAIS.Data
Namespace Mapping
    Public Class RNDDRoleMapping
        Public Shared Function MapDBToModelRoleRNDDInfo(ByVal roleResult As Objects.MAISRNDDRoleDetails) As Model.MAISRNDDRoleDetails
            Dim retObj As New Model.MAISRNDDRoleDetails
            If roleResult IsNot Nothing Then
                With retObj
                    .DDPersonnel = roleResult.DDPersonnel
                    .DDSID = roleResult.DDSID
                    .RNLicenseNumber = roleResult.RNLicenseNumber
                    .RNSID = roleResult.RNSID
                    .RoleName = roleResult.RoleName
                    .RoleSID = roleResult.RoleSID
                End With                        
            End If
            Return retObj
        End Function
        Public Shared Function MapDBToModelUserInfo(ByVal roleResult As Model.UserMappingDetails) As Objects.UserMappingDetails
            Return (New Objects.UserMappingDetails() With
                    {
                        .UserMappingSid = roleResult.UserMappingSid,
                        .UserID = roleResult.UserID,
                        .PortalUserRole = roleResult.PortalUserRole,
                        .FirstName = roleResult.FirstName,
                        .LastName = roleResult.LastName,
                        .MiddleName = roleResult.MiddleName,
                        .Email = roleResult.Email,
                        .Is_Secretary = roleResult.Is_Secretary,
                        .User_Code = roleResult.User_Code
                        })
        End Function
        Public Shared Function MapDBToModelUserMappingInfo(ByVal roleResult As Model.UserLoginSearch) As Objects.UserLoginSearch
            Return (New Objects.UserLoginSearch() With
                    {
                        .RNLicenseNumber = roleResult.RNLicenseNumber,
                            .FirstName = roleResult.FirstName,
                            .LastName = roleResult.LastName,
                        .UserMappingID = roleResult.UserMappingID
                        })
        End Function
    End Class
End Namespace
