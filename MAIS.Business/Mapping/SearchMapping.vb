Imports MAIS.Data
Namespace Mapping
    Public Class SearchMapping
        Public Shared Function MapDBToModelRNSearch(ByVal cd As List(Of Data.USP_Get_RN_Search_Results_Result), ByVal userRoleID As Integer, ByVal loginUserRNLicense As String, ByVal admin As Boolean, ByVal userID As Integer) As List(Of Model.RNSearchResult)
            Dim listSearch As New List(Of Model.RNSearchResult)
            listSearch = MapSearchCriteria(cd)
            'If (admin) Then
            '    'listSearch = MapSearchCriteria(cd)
            'Else
            '    If (userRoleID = 5 Or userRoleID = 6) Then
            '        'cd = cd.FindAll(Function(rn) rn.LastUpdateUserID = userID Or rn.RNLicenseNumber = loginUserRNLicense)
            '        'listSearch = MapSearchCriteria(cd)
            '    End If
            '    If (userRoleID = 4 Or userRoleID = 7 Or userRoleID = 8) Then
            '        'cd = cd.FindAll(Function(rn) rn.RNLicenseNumber = loginUserRNLicense)
            '        'listSearch = MapSearchCriteria(cd)
            '    End If
            'End If
            Return listSearch
        End Function
        Public Shared Function MapSearchCriteria(ByVal cd As List(Of Data.USP_Get_RN_Search_Results_Result))
            Dim listSearch As New List(Of Model.RNSearchResult)
            listSearch = (From search In cd
                          Select New Model.RNSearchResult With {
            .ApplicationID = search.AppID,
            .ApplicationStatus = search.ApplicationStatus,
            .County = search.County,
            .RNLicenseNumber = search.RNLicenseNumber,
            .FirstName = search.FirstName,
            .HomeAddress = search.HomeAddress,
            .LastName = search.LastName,
            .MiddleName = search.MiddleName,
            .ApplicationType = search.ApplicationType,
            .RNInstructor = search.RNInstructor,
            .RNMaster = search.RNMaster,
            .RNTrainer = search.RNTrainer,
            .QARN = search.QARN,
            .ICFRN = search.Bed17
                              }).ToList
            Return listSearch
        End Function
        Public Shared Function MapDBToModelDDSearch(ByVal cd As List(Of Data.USP_Get_DDPersonnel_Search_Results_Result)) As List(Of Model.DDPersonnelSearchResult)
            Dim listSearch As New List(Of Model.DDPersonnelSearchResult)
            listSearch = (From search In cd
                          Select New Model.DDPersonnelSearchResult With {
            .ApplicationID = search.AppID,
            .ApplicationStatus = search.ApplicationStatus,
            .County = search.County,
            .DateOfBirth = search.DOB,
            .FirstName = search.FirstName,
            .HomeAddress = search.HomeAddress,
            .LastName = search.LastName,
            .Last4SSN = search.SSN.PadLeft(4, "0"c),
            .DDPersonnelCode = search.DDPersonnelCode,
            .MiddleName = search.MiddleName,
            .ApplicationType = search.ApplicationType,
            .CAT1 = search.CAT1,
            .CAT2 = search.CAT2,
            .CAT3 = search.CAT3
                              }).ToList
            Return listSearch
        End Function
        Public Shared Function MapModelToObjects(ByVal map As Model.MAISSearchCriteria) As Objects.MAISSearchCriteria
            Dim searchCriteria As New Objects.MAISSearchCriteria
            searchCriteria.ApplicationID = map.ApplicationID
            searchCriteria.ApplicationStatus = map.ApplicationStatus
            searchCriteria.FirstName = map.FirstName
            searchCriteria.Last4SSN = map.Last4SSN
            searchCriteria.LastName = map.LastName
            searchCriteria.RNLicenseNumber = map.RNLicenseNumber
            searchCriteria.DateOfBirth = map.DateOfBirth
            searchCriteria.EmployerName = map.EmployerName
            searchCriteria.DDcode = map.DDCode
            Return searchCriteria
        End Function
    End Class
End Namespace
