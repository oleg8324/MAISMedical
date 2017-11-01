Imports MAIS.Business.Services
Imports System.Data.Linq
Imports MAIS.Data
Namespace Mapping
    Public Class SummaryMapping
        Public Shared Function MapDBToModel(ByVal rndet As Objects.PersonalInformationDetails) As Model.RNInformationDetails
            Dim rnm As New Model.RNInformationDetails
            If (rndet IsNot Nothing) Then
                rnm.DateOforiginalRNLicIssuance = rndet.DOBDateOfIssuance
                rnm.FirstName = rndet.FirstName
                rnm.LastName = rndet.LastName
                rnm.MiddleName = rndet.MiddleName
                rnm.RNLicense = rndet.RNLicenseOrSSN
                rnm.Gender = rndet.Gender
            End If
            Return rnm
        End Function
        Public Shared Function MapCertsDBToModel(ByVal ol As List(Of Objects.Certificate)) As List(Of Model.Certificate)
            Dim lmodcerts As New List(Of Model.Certificate)
            If (ol IsNot Nothing) Then
                lmodcerts = (From li In ol Select New Model.Certificate With {.Category = li.Category, .EndDate = li.EndDate, .Level = li.Level, .Role = li.Role, .Role_Category_Level_Sid = li.Role_Category_Level_Sid, .RolePriority = li.RolePriority, .StartDate = li.StartDate, .Status = li.Status}).ToList
            End If
            Return lmodcerts
        End Function
        Public Shared Function MapASListToModel(ByVal ast As List(Of Objects.AppStatus)) As List(Of Model.AppStatus)
            Dim asmodelList As New List(Of Model.AppStatus)
            asmodelList = (From nte In ast Select New Model.AppStatus With {.ASTypeDesc = nte.ASTypeDesc, .ASTypeSid = nte.ASTypeSid}).ToList
            Return asmodelList
        End Function
    End Class
End Namespace