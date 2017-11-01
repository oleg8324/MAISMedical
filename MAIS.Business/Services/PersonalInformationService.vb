Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data

Namespace Services
    Public Interface IPersonalInformationService
        Inherits IBusinessBase

        Function SavePersonalInformation(ByVal personalInfo As Model.PersonalInformationDetails, ByVal appId As Integer, ByVal rnOrDD As Boolean, ByVal name As String, ByVal brandNew As Boolean, ByVal uniqueCode As String, ByVal adminPerm As Boolean, ByVal query As String) As ReturnObject(Of Long)
        Function GetDDPersonnelInformation(ByVal applicationID As Integer) As Model.DDPersonnelDetails
        Function GetRNInformation(ByVal applicationID As Integer) As Model.RNInformationDetails
        Function GetRNInfoFromPermanent(ByVal rNLicenseNumber As String) As Model.RNInformationDetails
        Function GetDDPersonnelInformationFromPermanent(ByVal ddpersonnelCode As String) As Model.DDPersonnelDetails
        Function DeleteTheDataApplication(ByVal applicationID As Integer) As Boolean
        Function GetPersonalPageComplete(ByVal applicationID As Integer) As Integer
        Function GetPartialPersonalPageComplete(ByVal applicationID As Integer) As Integer
        ' Function GetOnlyPersonnalInformation(UniqueCode As String, RN_Flg As Boolean) As Model.PersonalInformationDetails
    End Interface
    Public Class PersonalInformationService
        Inherits BusinessBase
        Implements IPersonalInformationService
        Private _queries As Data.Queries.IPersonalInformationQueries
        Public Sub New(ByVal user As IUserIdentity, ByVal connectionstring As IConnectionIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of Data.Queries.IPersonalInformationQueries)()
            _queries.UserID = user.UserID.ToString()
            _queries.MAISConnectionString = connectionstring.ConnectionString
        End Sub
        'Public Function GetOnlyPersonnalInformation(UniqueCode As String, RN_Flg As Boolean) As Model.PersonalInformationDetails Implements IPersonalInformationService.GetOnlyPersonnalInformation
        '    Dim perInfo As New Model.PersonalInformationDetails
        '    Try
        '        perInfo = Mapping.PersonalInfoMapping.MapOnlyPErsonnalInfo(_queries.GetOnlyPersonnalInformation(UniqueCode, RN_Flg))
        '    Catch ex As Exception
        '        Me.LogError("Error fetching personnal information.", ex)
        '    End Try
        '    Return perInfo
        'End Function
        Public Function GetDDPersonnelInformation(applicationID As Integer) As Model.DDPersonnelDetails Implements IPersonalInformationService.GetDDPersonnelInformation
            Dim personalInfoValue As Model.DDPersonnelDetails = Nothing
            Try
                personalInfoValue = Mapping.PersonalInfoMapping.MapDBToModelPersonalInfo(_queries.GetDDPersonnelInformation(applicationID))
            Catch ex As Exception
                Me.LogError("Error Getting DDPersonnel Info", CInt(Me.UserID), ex)
            End Try
            Return personalInfoValue
        End Function
        Public Function GetDDPersonnelInformationFromPermanent(ddpersonnelCode As String) As DDPersonnelDetails Implements IPersonalInformationService.GetDDPersonnelInformationFromPermanent
            Dim personalInfoValue As Model.DDPersonnelDetails = Nothing
            Try
                personalInfoValue = Mapping.PersonalInfoMapping.MapDBToModelPersonalInfo(_queries.GetDDPersonnelInformationFromPermanent(ddpersonnelCode))
            Catch ex As Exception
                Me.LogError("Error Getting DDPersonnel Info From permanent", CInt(Me.UserID), ex)
            End Try
            Return personalInfoValue
        End Function
        Public Function GetRNInformation(ByVal applicationID As Integer) As Model.RNInformationDetails Implements IPersonalInformationService.GetRNInformation
            Dim rnInfoValue As Model.RNInformationDetails = Nothing
            Try
                rnInfoValue = Mapping.PersonalInfoMapping.MapDBToModelRNPersonalInfo(_queries.GetRNInformation(applicationID))
            Catch ex As Exception
                Me.LogError("Error Getting RN Info", CInt(Me.UserID), ex)
            End Try
            Return rnInfoValue
        End Function
        Public Function GetRNInfoFromPermanent(rNLicenseNumber As String) As RNInformationDetails Implements IPersonalInformationService.GetRNInfoFromPermanent
            Dim rnInfoValue As Model.RNInformationDetails = Nothing
            Try
                If Not (String.IsNullOrWhiteSpace(rNLicenseNumber)) Then
                    rnInfoValue = Mapping.PersonalInfoMapping.MapDBToModelRNPersonalInfo(_queries.GetRNInformationFromPermanent(rNLicenseNumber))
                End If
            Catch ex As Exception
                Me.LogError("Error Getting RN Info From permanent", CInt(Me.UserID), ex)
            End Try
            Return rnInfoValue
        End Function
        Public Function SavePersonalInformation(personalInfo As Model.PersonalInformationDetails, ByVal appId As Integer, ByVal rnOrDD As Boolean, ByVal name As String, ByVal brandNew As Boolean, ByVal uniqueCode As String, ByVal adminPerm As Boolean, ByVal queryString As String) As ReturnObject(Of Long) Implements IPersonalInformationService.SavePersonalInformation
            Dim retObj As New ReturnObject(Of Long)(-1L)
            Dim personalInfoValue As Objects.PersonalInformationDetails = Nothing
            Dim appDetails As MAISApplicationDetails = Nothing
            Try
                retObj = _queries.SavePersonalInformation(Mapping.PersonalInfoMapping.MapPersonalResultToDB(personalInfo), appId, rnOrDD, name, brandNew, uniqueCode, adminPerm, queryString)
            Catch ex As Exception
                Me.LogError("Error personal information services", CInt(Me.UserID), ex)
                retObj.AddErrorMessage("ex.Message")
            End Try

            Return retObj
        End Function
        Public Function DeleteTheDataApplication(applicationID As Integer) As Boolean Implements IPersonalInformationService.DeleteTheDataApplication
            Dim flag As Boolean = False
            Try
                flag = _queries.DeleteTheDataApplication(applicationID)
            Catch ex As Exception
                Me.LogError("Error Getting deleting application Info", CInt(Me.UserID), ex)
            End Try
            Return flag
        End Function
        Public Function GetPersonalPageComplete(applicationID As Integer) As Integer Implements IPersonalInformationService.GetPersonalPageComplete
            Dim personalPage As Integer = 0
            Try
                personalPage = _queries.GetPersonalPageComplete(applicationID)
            Catch ex As Exception
                Me.LogError("Error Getting personal page complete rule.", CInt(Me.UserID), ex)
            End Try
            Return personalPage
        End Function
        Public Function GetPartialPersonalPageComplete(applicationID As Integer) As Integer Implements IPersonalInformationService.GetPartialPersonalPageComplete
            Dim personalPage As Integer = 0
            Try
                personalPage = _queries.GetPartialPersonalPageComplete(applicationID)
            Catch ex As Exception
                Me.LogError("Error Getting partial personal page complete rule.", CInt(Me.UserID), ex)
            End Try
            Return personalPage
        End Function

    End Class
End Namespace

