Imports MAIS.Data
Namespace Mapping
    Public Class WorkExperienceMapping
        Public Shared Function MapFlags(ByVal flg As Objects.RN_DD_Flags) As Model.RN_DD_Flags
            Dim rdf As New Model.RN_DD_Flags
            If flg IsNot Nothing Then
                rdf.ChkDDFlg = flg.ChkDDFlg
                rdf.ChkRNFlg = flg.ChkRNFlg
            End If
            Return rdf
        End Function
        Public Shared Function MapWorkResultToDB(ByVal workExp As Model.WorkExperienceDetails) As Data.Objects.WorkExperienceDetails
            If workExp Is Nothing Then
                Return Nothing
            Else
                Dim obj As New Data.Objects.WorkExperienceDetails()
                With obj
                    .WorkID = workExp.WorkID
                    .AppID = workExp.AppID
                    .ChkDDFlg = workExp.ChkDDFlg
                    .ChkRNFlg = workExp.ChkRNFlg
                    .EmpName = workExp.EmpName.Trim()
                    .EmpStartDate = workExp.EmpStartDate
                    .EmpEndDate = workExp.EmpEndDate
                    .Title = workExp.Title.Trim()
                    .JobDuties = workExp.JobDuties.Trim()
                    With .Address
                        .AddressLine1 = workExp.Address.AddressLine1.Trim()
                        .AddressLine2 = workExp.Address.AddressLine2.Trim()
                        .City = workExp.Address.City.Trim()
                        .StateID = workExp.Address.StateID
                        .Zip = workExp.Address.Zip.Trim()
                        .ZipPlus = workExp.Address.ZipPlus.Trim()
                        .CountyID = workExp.Address.CountyID()
                        .Phone = workExp.Address.Phone.Trim()
                        .Email = workExp.Address.Email.Trim()
                        .AddressType = workExp.Address.AddressType
                        .ContactType = workExp.Address.ContactType
                    End With
                End With
                Return obj
            End If
        End Function
       
        Public Shared Function MapDBToModelWorkExp(ByVal cd As List(Of Objects.WorkExperienceDetails)) As List(Of Model.WorkExperienceDetails)
            Dim expCount As Integer = 0
            Dim workInfo As New List(Of Model.WorkExperienceDetails)
            If (cd.Count > 0) Then
                workInfo = (From we In cd _
                              Select New Model.WorkExperienceDetails With {
                                    .WorkID = we.WorkID,
                                    .AppID = we.AppID,
                                    .ChkDDFlg = we.ChkDDFlg,
                                    .ChkRNFlg = we.ChkRNFlg,
                                    .EmpName = we.EmpName,
                                    .EmpStartDate = we.EmpStartDate,
                                    .EmpEndDate = we.EmpEndDate,
                                    .JobDuties = we.JobDuties,
                                    .Title = we.Title,
                                     .Address = MapDBToWrokExpAddress(we.Address)
                              }).ToList
            End If
            Return workInfo
        End Function
        Public Shared Function MapDBToWrokExpAddress(ByVal add As Objects.AddressControlDetails) As Model.AddressControlDetails
            Dim addr As New Model.AddressControlDetails
            With addr
                .AddressLine1 = add.AddressLine1
                .AddressLine2 = add.AddressLine2
                .City = add.City
                .StateID = add.StateID
                .Zip = add.Zip
                .ZipPlus = add.ZipPlus
                .CountyID = add.CountyID
                .Phone = add.Phone
                .Email = add.Email
                .AddressType = add.AddressType
                .ContactType = add.ContactType
            End With
            Return addr
        End Function

        Public Shared Function MapDBToWorkResult(ByVal workExpInfo As Data.Objects.WorkExperienceDetails) As Model.WorkExperienceDetails
            If workExpInfo Is Nothing Then
                Return Nothing
            Else
                Dim weInfo As New Model.WorkExperienceDetails()
                With weInfo
                    .WorkID = workExpInfo.WorkID
                    .AppID = workExpInfo.AppID
                    .ChkDDFlg = workExpInfo.ChkDDFlg
                    .ChkRNFlg = workExpInfo.ChkRNFlg
                    .EmpName = workExpInfo.EmpName
                    .EmpStartDate = workExpInfo.EmpStartDate
                    .EmpEndDate = workExpInfo.EmpEndDate
                    .Title = workExpInfo.Title
                    .JobDuties = workExpInfo.JobDuties
                    With .Address
                        .AddressLine1 = workExpInfo.Address.AddressLine1
                        .AddressLine2 = workExpInfo.Address.AddressLine2
                        .City = workExpInfo.Address.City
                        .StateID = workExpInfo.Address.StateID
                        .Zip = workExpInfo.Address.Zip
                        .ZipPlus = workExpInfo.Address.ZipPlus
                        .CountyID = workExpInfo.Address.CountyID
                        .Phone = workExpInfo.Address.Phone
                        .Email = workExpInfo.Address.Email
                        .AddressType = workExpInfo.Address.AddressType
                        .ContactType = workExpInfo.Address.ContactType
                    End With
                End With
                Return weInfo
            End If
        End Function
    End Class
End Namespace
