Imports MAIS.Data

Namespace Mapping
    Public Class MAISMapping
        Public Shared Function MapDBToModelCertHistory(ByVal lstCert As List(Of Objects.Certificate)) As List(Of Model.Certificate)
            Dim certInfo As New List(Of Model.Certificate)
            If lstCert.Count > 0 Then
                certInfo = (From cert In lstCert
                            Order By cert.RolePriority
                            Select New Model.Certificate With {
                                .Role_RN_DD_Personnel_Xref_Sid = cert.Role_RN_DD_Personnel_Xref_Sid,
                                .Role_Category_Level_Sid = cert.Role_Category_Level_Sid,
                                .Category = cert.Category,
                                .Level = cert.Level,
                                .Role = cert.Role,
                                .Certification_Sid = cert.Certification_Sid,
                                .Application_Sid = cert.Application_Sid,
                                .StartDate = cert.StartDate.ToShortDateString(),
                                .EndDate = cert.EndDate.ToShortDateString(),
                                .Status = cert.Status,
                                .RolePriority = cert.RolePriority,
                .ApplicationType = cert.ApplicationType
                                }).ToList()
            End If
            Return certInfo
        End Function

        Public Shared Function MapDBtoModelRoleCategoryLevelDetails(ByVal db As Objects.RoleCategoryLevelDetailsObject) As Model.RoleCategoryLevelDetails
            Dim RoleCategInfo As New Model.RoleCategoryLevelDetails
            With RoleCategInfo
                .Role_Category_Level_Sid = db.Role_Category_Level_Sid
                .Role_Sid = db.Role_Sid
                .Role_Name = db.Role_Name
                .Category_Type_Sid = db.Category_Type_Sid
                .Category_Type_Name = db.Category_Type_Name
            End With
            Return RoleCategInfo

        End Function

        Public Shared Function MapDBtoModelRNSecretaryMapping(ByVal rs As List(Of Objects.RN_Mapping)) As List(Of Model.RN_Mapping)
            Dim rsList As New List(Of Model.RN_Mapping)
            If rs.Count > 0 Then
                rsList = (From rinfo In rs
                          Select New Model.RN_Mapping With {
                                .RN_Sid = rinfo.RN_Sid,
                                .First_Name = rinfo.First_Name,
                                .Last_Name = rinfo.Last_Name,
                                .Middle_Name = rinfo.Middle_Name,
                                .RNLicenseNumber = rinfo.RNLicenseNumber,
                                .Comments = rinfo.Comments,
                                .Un_Map_Flg = rinfo.Un_Map_Flg,
                                .Last_Updated_By = rinfo.Last_Updated_By,
                                .Last_Updated_Date = rinfo.Last_Updated_Date
                              }).ToList()
            End If
            Return rsList
        End Function
        Public Shared Function MapDBtoModelCounties(ByVal rs As List(Of Objects.CountyDetails)) As List(Of Model.CountyDetails)
            Dim rsList As New List(Of Model.CountyDetails)
            If rs.Count > 0 Then
                rsList = (From rinfo In rs
                          Select New Model.CountyDetails With {
                                 .CountyAlias = rinfo.CountyAlias,
                                .CountyID = rinfo.CountyID
                              }).ToList()
            End If
            Return rsList
        End Function
        Public Shared Function MapDBtoModelStates(ByVal rs As List(Of Objects.StateDetails)) As List(Of Model.StateDetails)
            Dim rsList As New List(Of Model.StateDetails)
            If rs.Count > 0 Then
                rsList = (From rinfo In rs
                          Select New Model.StateDetails With {
                                 .StateAbr = rinfo.StateAbr,
                                .StateID = rinfo.StateID
                              }).ToList()
            End If
            Return rsList
        End Function
        Public Shared Function MapDBtoOneRNSecretaryMapping(ByVal rsObj As Objects.RN_Mapping) As Model.RN_Mapping
            Dim rsOne As New Model.RN_Mapping
            If rsObj IsNot Nothing Then
                rsOne.RN_Sid = rsObj.RN_Sid
                rsOne.First_Name = rsObj.First_Name
                rsOne.Last_Name = rsObj.Last_Name
                rsOne.Middle_Name = rsObj.Middle_Name
                rsOne.RNLicenseNumber = rsObj.RNLicenseNumber
                rsOne.Comments = rsObj.Comments
                rsOne.Un_Map_Flg = rsObj.Un_Map_Flg
                rsOne.Last_Updated_By = rsObj.Last_Updated_By
                rsOne.Last_Updated_Date = rsObj.Last_Updated_Date
            End If
            Return rsOne
        End Function
        Public Shared Function MapDBToSecretaryAssociation(ByVal sa As List(Of Objects.Secretary_Association)) As List(Of Model.Secretary_Association)
            Dim saList As New List(Of Model.Secretary_Association)
            If sa.Count > 0 Then
                saList = (From sa1 In sa
                         Select New Model.Secretary_Association With {
                                .First_Name = sa1.First_Name,
                                .Last_Name = sa1.Last_Name,
                                .Middle_Name = sa1.Middle_Name,
                                .Email = sa1.Email,
                                .SecretaryUserName = sa1.SecretaryUserName,
                                .User_Mapping_Sid = sa1.User_Mapping_Sid,
                                .Last_Updated_Date = sa1.Last_Updated_Date,
                                .Last_Updated_By = sa1.Last_Updated_By
                             }).ToList()

            End If
            Return saList
        End Function
        Public Shared Function MapDBToSecretaryAssociationInfo(ByVal sa1 As Objects.Secretary_Association) As Model.Secretary_Association
            Dim saOne As New Model.Secretary_Association
            If sa1 IsNot Nothing Then
                saOne.First_Name = sa1.First_Name
                saOne.Last_Name = sa1.Last_Name
                saOne.Middle_Name = sa1.Middle_Name
                saOne.Email = sa1.Email
                saOne.SecretaryUserName = sa1.SecretaryUserName
                saOne.User_Mapping_Sid = sa1.User_Mapping_Sid
                saOne.Last_Updated_Date = sa1.Last_Updated_Date
                saOne.Last_Updated_By = sa1.Last_Updated_By
            End If
            Return saOne
        End Function
        Public Shared Function MapDBToSecreatryDetails(ByVal aa As List(Of Objects.Secretary_Detail)) As List(Of Model.Secretary_Detail)
            Dim aaList As New List(Of Model.Secretary_Detail)
            If aa.Count > 0 Then
                aaList = (From aa1 In aa
                       Select New Model.Secretary_Detail With {
                                .Comments = aa1.Comments,
                                .RN_Secretary_Association_Sid = aa1.RN_Secretary_Association_Sid,
                                .RNLicense = aa1.RNLicense,
                                .Last_Updated_By = aa1.Last_Updated_By,
                                .Last_Updated_Date = aa1.Last_Updated_Date,
                                .RN_Name = aa1.L_Name + " " + aa1.F_Name + " " + aa1.M_Name,
                                .RN_Sid = aa1.RN_Sid,
                                .User_Mapping_Sid = aa1.User_Mapping_Sid
                           }).ToList()

            End If
            Return aaList
        End Function
        Public Shared Function MapModelToDBSecretaryDetails(ByVal sec As Model.Secretary_Detail) As Objects.Secretary_Detail
            Dim ss As New Objects.Secretary_Detail
            If sec IsNot Nothing Then
                ss.User_Mapping_Sid = sec.User_Mapping_Sid
                ss.RN_Sid = sec.RN_Sid
                ss.Comments = sec.Comments                
            End If
            Return ss
        End Function

        Public Shared Function MapDbToCertificateExpirationTotls(ByVal DBCertificationExpiration As List(Of Objects.CertificateExpirationTotals)) As List(Of Model.CertificateExpirationTotals)
            Dim retVal As List(Of Model.CertificateExpirationTotals)
            retVal = (From c In DBCertificationExpiration
                      Select New Model.CertificateExpirationTotals With {.Category = c.Category,
                                                                         .Exp180Days = c.Exp180Days,
                                                                         .Exp30Days = c.Exp30Days,
                                                                         .Exp60Days = c.Exp60Days,
                                                                         .Exp90Days = c.Exp90Days,
                                                                         .Level = c.Level,
                                                                         .Role = c.Role,
                                                                         .Role_Category_Level_Sid = c.Role_Category_Level_Sid,
                                                                         .Role_RN_DD_Personnel_Xref_Sid = c.Role_RN_DD_Personnel_Xref_Sid,
                                                                         .RolePriority = c.RolePriority,
                                                                         .Status = c.Status}).ToList
            Return retVal
        End Function
    End Class
End Namespace
