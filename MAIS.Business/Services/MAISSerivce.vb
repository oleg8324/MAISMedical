Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data
Imports MAIS.Business.Services
Imports MAIS.Data.Queries

Namespace Services
    Public Interface IMAISSerivce
        Inherits IBusinessBase
        Function CheckRenewalDone(UniqueID As String, RCL_Sid As Integer, RN_DD_FLg As Boolean) As Boolean
        Function CheckPreviousRenewal(ByVal UniqueID As String, ByVal RCL_Sid As Integer) As String
        Function UnRegisterQA(ByVal RLC_Sid As Integer, ByVal Role_Person_Sid As Integer) As Boolean
        Function GetSecretaryByID(ByVal U_ID As Integer) As Secretary_Association
        Function GetSecreatryList() As List(Of Secretary_Association)
        Function RemoveRnForSecretary(ByVal rs_sid As Integer) As Boolean
        Function SaveSecretaryDetails(ByVal secInfo As Secretary_Detail) As Boolean
        Function GetSecretaryDetails(ByVal U_Id As Integer) As List(Of Secretary_Detail)
        Function GetAllSecretaries(ByVal Email As String, ByVal Fname As String, ByVal Lname As String, ByVal RN_SId As Integer, ByVal Status As String) As List(Of Secretary_Association)
        Function GetRNForMappings(ByVal rnNO As String, ByVal Fname As String, ByVal Lname As String, ByVal Status As String) As List(Of RN_Mapping)
        Function GetRoleUsingUserID(ByVal userID As Integer) As Model.MAISRNDDRoleDetails
        Function GetRoleCategoryLevelInfoByRoleCategoryLevelSid(ByVal RoleCategoryLevelSid As Integer) As Model.RoleCategoryLevelDetails
        Function GetExistingFlg(ByVal UniqueCode As String, ByVal RN_Flg As Boolean) As ReturnObject(Of Boolean)
        Function GetCertificationHistory(ByVal UniqueCode As String, ByVal RN_Flg As Boolean) As List(Of Model.Certificate)
        Function SaveUserLoggedData(ByVal userdetails As UserMappingDetails) As Integer
        Function SaveUserRNMappingData(ByVal usermappingDetails As UserLoginSearch) As ReturnObject(Of Long)
        Function CheckRNMapping(ByVal userId As Integer) As Model.RN_Mapping
        Function CheckSecetaryMapping(ByVal userId As Integer) As Boolean
        Function GetCertificationStartDate(ByVal UserUnique As String, ByVal categoryLevelID As Integer) As Date
        Function GetCertificationDate(ByVal UserUnique As String, ByVal CategoryLevelID As Integer) As Date
        Function GetCertificationDateByCategoryID(ByVal UserUnique As String, ByVal CategoryID As Integer) As Date
        Function GetCertificationDateThatIsHighRoleProiorityByRNSID(ByVal RNs_Sid As Integer, ByVal StartDate As Date) As Date
        Function GetCertificationMinStartDateByDDPersonnelCode(ByVal DDPersonelCode As String) As Date
        Function GetCertificationMinStartDateByRNSID(ByVal RNs_Sid As Integer) As Date
        Function GetRNsName(ByVal RNs_Sid As Integer) As String
        Function GetRNsLicenseNumber(ByVal RNs_Sid As Integer) As String
        Function UpdateRNMapping(ByVal Rnid As Integer, ByVal com As String, ByVal chFlg As Boolean) As ReturnObject(Of Boolean)
        Function GetApplicantXrefSidByCode(ByVal code As String, ByVal RN_flg As Boolean) As Integer
        Function GetApplicantNameByCode(ByVal code As String, ByVal RN_flg As Boolean) As String
        Function GetAllStates() As List(Of Model.StateDetails)
        Function SetCertStatusAndDates(ByVal rnflag As Boolean, ByVal RoleDDRNXrefSid As Integer, ByVal StatusSid As Integer, Optional startDate As Date = #12/12/1990#, Optional endDate As Date = #12/31/9999#) As ReturnObject(Of Long)
        Function GetAllCountyCodes() As List(Of Model.CountyDetails)
        Function GetCountyIDByCodes(ByVal countyCode As String) As Integer
        Function GetStateIDByStates(ByVal StateAbr As String) As Integer
        Function CheckTheMandatoryFields(ByVal rnLicenseNumber As String) As Integer
        Function GetAppIDByRNLicenseNumber(ByVal rnLicenseNumber As String) As Integer
        Function GetCertificateExpirationTotals(ByVal Role As Enums.RoleLevelCategory) As List(Of Model.CertificateExpirationTotals)
        Function GetRNEmailAddressUsingRNsid(ByVal rnsidorrnsecetaryassociationID As Integer, ByVal flag As Integer) As String
        Function GetCourseInformationByCertificationID(ByVal CertificationID As Integer) As Model.CourseDetails
        Function GetSessionCourseInfoDetailsBySesssionID(ByVal SessionID As Integer) As List(Of Model.SessionCourseInfoDetails)
        Function GetCurrnetSessionWithCertificationID(ByVal CertificationID As Integer) As Model.PersonCourseSession
        Function UpdateSessionCourseInfoSession(ByVal newSessionID As Integer, ByVal RoleRNDDPersonelXrefSID As Integer, CertificationID As Integer) As Boolean
        Function UserSessionMatch(ByVal AppID As Integer, ByVal uniqueID As String, ByVal OldAppID As Integer) As Boolean
        Function SendToErrorLog(ByVal msg As String) As ReturnObject(Of Long)
        Function ChangeRNLicenseNumber(ByVal newRNNumber As String, ByVal exisitngRNNumber As String) As String
    End Interface
End Namespace
Public Class MAISSerivce
    Inherits BusinessBase
    Implements IMAISSerivce

    Private _queries As IMAISQueries
    Public Sub New(ByVal user As IUserIdentity, ByVal connectionstring As IConnectionIdentity)
        _queries = StructureMap.ObjectFactory.GetInstance(Of IMAISQueries)()
        _queries.UserID = user.UserID.ToString()
        _queries.MAISConnectionString = connectionstring.ConnectionString
    End Sub
    Public Function SendToErrorLog(ByVal msg As String) As ReturnObject(Of Long) Implements IMAISSerivce.SendToErrorLog
        Dim ret As New ReturnObject(Of Long)(-1L)

        Me.LogError(msg + " " + Now.ToShortDateString + " " + Now.ToShortTimeString, CInt(Me.UserID))
        Return ret
    End Function
    Public Function CheckRenewalDone(UniqueID As String, RCL_Sid As Integer, RN_DD_FLg As Boolean) As Boolean Implements IMAISSerivce.CheckRenewalDone
        Dim retFlg As Boolean = False
        Try
            retFlg = _queries.CheckRenewalDone(UniqueID, RCL_Sid, RN_DD_FLg)
        Catch ex As Exception
            Me.LogError("Error fetching renwal done.", CInt(Me.UserID), ex)
        End Try
        Return retFlg
    End Function
    Public Function CheckPreviousRenewal(ByVal UniqueID As String, ByVal RCL_Sid As Integer) As String Implements IMAISSerivce.CheckPreviousRenewal
        Dim str As String = String.Empty
        Try
            str = _queries.CheckPreviousRenewal(UniqueID, RCL_Sid)
        Catch ex As Exception
            Me.LogError("Error un registering QA RN.", CInt(Me.UserID), ex)
        End Try
        Return str
    End Function
    Public Function UnRegisterQA(ByVal RLC_Sid As Integer, ByVal Role_Person_Sid As Integer) As Boolean Implements IMAISSerivce.UnRegisterQA
        Dim rn As Boolean = False
        Try
            rn = _queries.UnRegisterQA(RLC_Sid, Role_Person_Sid)
        Catch ex As Exception
            Me.LogError("Error un registering QA RN.", CInt(Me.UserID), ex)
        End Try
        Return rn
    End Function
    Public Function GetSecretaryByID(ByVal U_ID As Integer) As Secretary_Association Implements IMAISSerivce.GetSecretaryByID
        Dim secInfo As New Secretary_Association
        Try
            secInfo = Mapping.MAISMapping.MapDBToSecretaryAssociationInfo(_queries.GetSecretaryByID(U_ID))
        Catch ex As Exception
            Me.LogError("Error fetching secretary info.", CInt(Me.UserID), ex)
        End Try
        Return secInfo
    End Function
    Public Function GetSecreatryList() As List(Of Secretary_Association) Implements IMAISSerivce.GetSecreatryList
        Dim setList As New List(Of Secretary_Association)
        Try
            setList = Mapping.MAISMapping.MapDBToSecretaryAssociation(_queries.GetSecreatryList())
        Catch ex As Exception
            Me.LogError("Error geting list of secretaries.", CInt(Me.UserID), ex)
        End Try
        Return setList
    End Function
    Public Function RemoveRnForSecretary(ByVal rs_sid As Integer) As Boolean Implements IMAISSerivce.RemoveRnForSecretary
        Dim retobj As Boolean = False
        Try
            retobj = _queries.RemoveRnForSecretary(rs_sid)
        Catch ex As Exception
            Me.LogError("Error removing secretary details.", CInt(Me.UserID), ex)
        End Try
        Return retobj
    End Function
    Public Function SetCertStatusAndDates(ByVal rnflag As Boolean, ByVal RoleDDRNXrefSid As Integer, ByVal StatusSid As Integer, Optional startDate As Date = #12/12/1990#, Optional endDate As Date = #12/31/9999#) As ReturnObject(Of Long) Implements IMAISSerivce.SetCertStatusAndDates
        Dim ret As New ReturnObject(Of Long)(-1L)
        Try
            ret = _queries.SetCertStatusAndDates(rnflag, RoleDDRNXrefSid, StatusSid, startDate, endDate)
        Catch ex As Exception
            Me.LogError("Error setting certification status.", CInt(Me.UserID), ex)
        End Try
        Return ret
    End Function
    Public Function SaveSecretaryDetails(ByVal secInfo As Secretary_Detail) As Boolean Implements IMAISSerivce.SaveSecretaryDetails
        Dim retObj As Boolean = False
        Try
            retObj = _queries.SaveSecretaryDetails(Mapping.MAISMapping.MapModelToDBSecretaryDetails(secInfo))
        Catch ex As Exception
            Me.LogError("Error saving secretary details.", CInt(Me.UserID), ex)
        End Try

        Return retObj
    End Function
    Public Function GetSecretaryDetails(ByVal U_Id As Integer) As List(Of Secretary_Detail) Implements IMAISSerivce.GetSecretaryDetails
        Dim retObj As New List(Of Secretary_Detail)
        Try
            retObj = Mapping.MAISMapping.MapDBToSecreatryDetails(_queries.GetSecretaryDetails(U_Id))
        Catch ex As Exception
            Me.LogError("Error fetching secretary details.", CInt(Me.UserID), ex)
        End Try
        Return retObj
    End Function
    Public Function GetAllSecretaries(ByVal Email As String, ByVal Fname As String, ByVal Lname As String, ByVal RN_SId As Integer, ByVal Status As String) As List(Of Secretary_Association) Implements IMAISSerivce.GetAllSecretaries
        Dim retObj As New List(Of Secretary_Association)
        Try
            retObj = Mapping.MAISMapping.MapDBToSecretaryAssociation(_queries.GetAllSecretaries(Email, Fname, Lname, RN_SId, Status))
        Catch ex As Exception
            Me.LogError("Error fetching secretary RNs.", CInt(Me.UserID), ex)
        End Try
        Return retObj
    End Function
    Public Function UpdateRNMapping(ByVal Rnid As Integer, ByVal com As String, ByVal chFlg As Boolean) As ReturnObject(Of Boolean) Implements IMAISSerivce.UpdateRNMapping
        Dim retObj As New ReturnObject(Of Boolean)(False)
        Try
            retObj = _queries.UpdateRNMapping(Rnid, com, chFlg)
        Catch ex As Exception
            Me.LogError("Error Updateing RN Mapping.", CInt(Me.UserID), ex)
        End Try
        Return retObj
    End Function
    Public Function GetRNForMappings(ByVal rnNO As String, ByVal Fname As String, ByVal Lname As String, ByVal Status As String) As List(Of RN_Mapping) Implements IMAISSerivce.GetRNForMappings
        Dim retObj As New List(Of RN_Mapping)
        Try
            retObj = Mapping.MAISMapping.MapDBtoModelRNSecretaryMapping(_queries.GetRNForMappings(rnNO, Fname, Lname, Status))
        Catch ex As Exception
            Me.LogError("Error fetching RN Mapping.", CInt(Me.UserID), ex)
        End Try
        Return retObj
    End Function
    Public Function GetCertificationHistory(ByVal UniqueCode As String, ByVal RN_Flg As Boolean) As List(Of Model.Certificate) Implements IMAISSerivce.GetCertificationHistory
        Dim retObj As New List(Of Model.Certificate)
        Try
            retObj = Mapping.MAISMapping.MapDBToModelCertHistory(_queries.GetCertificationHistory(UniqueCode, RN_Flg))
        Catch ex As Exception
            Me.LogError("Error deleting Currently Added Work Experience.", CInt(Me.UserID), ex)
        End Try
        Return retObj
    End Function
    Public Function GetExistingFlg(ByVal UniqueCode As String, ByVal RN_Flg As Boolean) As ReturnObject(Of Boolean) Implements IMAISSerivce.GetExistingFlg
        Dim retObj As New ReturnObject(Of Boolean)(False)
        Try
            retObj.ReturnValue = _queries.GetExistingFlg(UniqueCode, RN_Flg).ReturnValue
        Catch ex As Exception
            Me.LogError("Error occured fetching exisitng flag", CInt(Me.UserID), ex)
        End Try
        Return retObj
    End Function
    Public Function GetRoleUsingUserID(ByVal userID As Integer) As Model.MAISRNDDRoleDetails Implements Services.IMAISSerivce.GetRoleUsingUserID
        Dim roles As Model.MAISRNDDRoleDetails = Nothing
        Try
            roles = Mapping.RNDDRoleMapping.MapDBToModelRoleRNDDInfo(_queries.GetRoleUsingUserID(userID))
        Catch ex As Exception
            Me.LogError("Error occured when fetching roles", CInt(Me.UserID), ex)
        End Try
        Return roles
    End Function
    Public Function SaveUserLoggedData(ByVal userdetails As Model.UserMappingDetails) As Integer Implements Services.IMAISSerivce.SaveUserLoggedData
        Dim userID As Integer = 0
        Try
            userID = _queries.SaveUserLoggedData(Mapping.RNDDRoleMapping.MapDBToModelUserInfo(userdetails))
        Catch ex As Exception
            Me.LogError("Error occured when inserting data in user mapping", CInt(Me.UserID), ex)
        End Try
        Return userID
    End Function
    Public Function SaveUserRNMappingData(usermappingDetails As UserLoginSearch) As ReturnObject(Of Long) Implements IMAISSerivce.SaveUserRNMappingData
        Dim retObj As New ReturnObject(Of Long)(-1L)
        Try
            retObj = _queries.SaveUserRNMappingData(Mapping.RNDDRoleMapping.MapDBToModelUserMappingInfo(usermappingDetails))
        Catch ex As Exception
            Me.LogError("Error occured when inserting data in user rn mapping", CInt(Me.UserID), ex)
        End Try
        Return retObj
    End Function
    Public Function CheckSecetaryMapping(userId As Integer) As Boolean Implements IMAISSerivce.CheckSecetaryMapping
        Dim flagsecetary As Boolean = False
        Try
            flagsecetary = _queries.CheckSecetaryMapping(userId)
        Catch ex As Exception
            Me.LogError("Error occured fetching the sectary existing", CInt(Me.UserID), ex)
        End Try
        Return flagsecetary
    End Function

    Public Function CheckRNMapping(userId As Integer) As RN_Mapping Implements IMAISSerivce.CheckRNMapping
        Dim rnMapping As New RN_Mapping
        Dim flagsecetary As Boolean = False
        Try
            rnMapping = Mapping.MAISMapping.MapDBtoOneRNSecretaryMapping(_queries.CheckRNMapping(userId))
        Catch ex As Exception
            Me.LogError("Error occured fetching the RN existing", CInt(Me.UserID), ex)
        End Try
        Return rnMapping
    End Function


    Public Function GetRoleCategoryLevelInfoByRoleCategoryLevelSid(RoleCategoryLevelSid As Integer) As RoleCategoryLevelDetails Implements IMAISSerivce.GetRoleCategoryLevelInfoByRoleCategoryLevelSid
        Dim retVal As New Model.RoleCategoryLevelDetails
        Try
            retVal = Mapping.MAISMapping.MapDBtoModelRoleCategoryLevelDetails(_queries.GetRoleCategoryLevelInfoByRoleCategoryLevelSid(RoleCategoryLevelSid))
        Catch ex As Exception
            Me.LogError("Erorr occured fetchign the Role Level Category information form MAIS Service", CInt(Me.UserID), ex)
        End Try
        Return retVal
    End Function

    Public Function GetCertificationDate(UserUnique As String, CategoryLevelID As Integer) As Date Implements IMAISSerivce.GetCertificationDate
        Dim res As Date
        Try
            res = _queries.GetCertificationDate(UserUnique, CategoryLevelID)
        Catch ex As Exception
            Me.LogError("Erorr in GetCertificationDate MAIS Service", CInt(Me.UserID), ex)
        End Try
        Return res

    End Function

    Public Function GetCertificationDateByCategoryID(UserUnique As String, CategoryID As Integer) As Date Implements IMAISSerivce.GetCertificationDateByCategoryID
        Dim res As Date
        Try
            res = _queries.GetCertificationDateByCategoryID(UserUnique, CategoryID)
        Catch ex As Exception
            Me.LogError("Erorr in GetCertificationDate MAIS Service", CInt(Me.UserID), ex)
        End Try
        Return res

    End Function

    Public Function GetCertificationDateThatIsHighRoleProiorityByRNSID(RNs_Sid As Integer, StartDate As Date) As Date Implements IMAISSerivce.GetCertificationDateThatIsHighRoleProiorityByRNSID
        Dim res As Date
        Try
            res = _queries.GetCertificationDateThatIsHighRoleProiorityByRNSID(RNs_Sid, StartDate)
        Catch ex As Exception
            Me.LogError("Erorr in GetCertificationDate MAIS Service", CInt(Me.UserID), ex)
        End Try
        Return res

    End Function

    Public Function GetCertificationMinStartDateByRNSID(RNs_Sid As Integer) As Date Implements IMAISSerivce.GetCertificationMinStartDateByRNSID
        Dim res As Date
        Try
            res = _queries.GetCertificationMinStartDateByRNSID(RNs_Sid)
        Catch ex As Exception
            Me.LogError("Erorr in GetCertificationDate MAIS Service", CInt(Me.UserID), ex)
        End Try
        Return res

    End Function

    Public Function GetCertificationMinStartDateByDDPersonnelCode(DDPersonelCode As String) As Date Implements IMAISSerivce.GetCertificationMinStartDateByDDPersonnelCode
        Dim res As Date
        Try
            res = _queries.GetCertificationMinStartDateByDDPersonnelCode(DDPersonelCode)
        Catch ex As Exception
            Me.LogError("Erorr in GetCertificationDate MAIS Service", CInt(Me.UserID), ex)
        End Try
        Return res

    End Function

    Public Function GetRNsName(RNs_Sid As Integer) As String Implements IMAISSerivce.GetRNsName
        Dim retval As String = Nothing
        Try
            retval = _queries.GetRNsName(RNs_Sid)
        Catch ex As Exception
            Me.LogError("Erorr in GetCertificationDate MAIS Service", CInt(Me.UserID), ex)
        End Try
        Return retval

    End Function
    Public Function GetApplicantXrefSidByCode(ByVal code As String, ByVal RN_flg As Boolean) As Integer Implements IMAISSerivce.GetApplicantXrefSidByCode
        Dim retval As Integer
        Try
            retval = _queries.GetApplicantXrefSidByCode(code, RN_flg)
        Catch ex As Exception
            Me.LogError("Erorr in GetCertificationDate MAIS Service", CInt(Me.UserID), ex)
        End Try
        Return retval

    End Function
    Public Function GetApplicantNameByCode(ByVal code As String, ByVal RN_flg As Boolean) As String Implements IMAISSerivce.GetApplicantNameByCode
        Dim retval As String = Nothing
        Try
            retval = _queries.GetApplicantNameByCode(code, RN_flg)
        Catch ex As Exception
            Me.LogError("Erorr in GetCertificationDate MAIS Service", CInt(Me.UserID), ex)
        End Try
        Return retval

    End Function

    Public Function GetRNsLicenseNumber(RNs_Sid As Integer) As String Implements IMAISSerivce.GetRNsLicenseNumber
        Dim retVal As String = Nothing
        Try
            retVal = _queries.GetRNsLicenseNumber(RNs_Sid)
        Catch ex As Exception
            Me.LogError("Erorr in GetCertificationDate MAIS Service", CInt(Me.UserID), ex)
        End Try

        Return retVal
    End Function

    Public Function GetCertificationStartDate(UserUnique As String, categoryLevelID As Integer) As Date Implements IMAISSerivce.GetCertificationStartDate
        Dim retVal As Date
        Try
            retVal = _queries.GetCertificationStartDate(UserUnique, categoryLevelID)
        Catch ex As Exception
            Me.LogError("Erorr in GetCertificationDate MAIS Service", CInt(Me.UserID), ex)
        End Try
        Return retVal

    End Function

    Public Function GetAllCountyCodes() As List(Of CountyDetails) Implements IMAISSerivce.GetAllCountyCodes
        Dim retObj As New List(Of CountyDetails)
        Try
            retObj = Mapping.MAISMapping.MapDBtoModelCounties(_queries.GetAllCountyCodes())
        Catch ex As Exception
            Me.LogError("Error occured when fetching data from county code", CInt(Me.UserID), ex)
        End Try
        Return retObj
    End Function

    Public Function GetAllStates() As List(Of StateDetails) Implements IMAISSerivce.GetAllStates
        Dim retObj As New List(Of StateDetails)
        Try
            retObj = Mapping.MAISMapping.MapDBtoModelStates(_queries.GetAllStates())
        Catch ex As Exception
            Me.LogError("Error occured when fetching data from state", CInt(Me.UserID), ex)
        End Try
        Return retObj
    End Function

    Public Function GetCountyIDByCodes(countyCode As String) As Integer Implements IMAISSerivce.GetCountyIDByCodes
        Dim retObj As Integer = 0
        Try
            retObj = _queries.GetCountyIDByCodes(countyCode)
        Catch ex As Exception
            Me.LogError("Error occured when fetching ID from state", CInt(Me.UserID), ex)
        End Try
        Return retObj
    End Function

    Public Function GetStateIDByStates(StateAbr As String) As Integer Implements IMAISSerivce.GetStateIDByStates
        Dim retObj As Integer = 0
        Try
            retObj = _queries.GetStateIDByStates(StateAbr)
        Catch ex As Exception
            Me.LogError("Error occured when fetching ID from state", CInt(Me.UserID), ex)
        End Try
        Return retObj
    End Function

    Public Function CheckTheMandatoryFields(rnLicenseNumber As String) As Integer Implements IMAISSerivce.CheckTheMandatoryFields
        Dim retObj As Integer = 0
        Try
            retObj = _queries.CheckTheMandatoryFields(rnLicenseNumber)
        Catch ex As Exception
            Me.LogError("Error occured when minimum requirements for RN", CInt(Me.UserID), ex)
        End Try
        Return retObj
    End Function

    Public Function GetAppIDByRNLicenseNumber(rnLicenseNumber As String) As Integer Implements IMAISSerivce.GetAppIDByRNLicenseNumber
        Dim retObj As Integer = 0
        Try
            retObj = _queries.GetAppIDByRNLicenseNumber(rnLicenseNumber)
        Catch ex As Exception
            Me.LogError("Error occured when fetching appID for RN", CInt(Me.UserID), ex)
        End Try
        Return retObj
    End Function

    Public Function GetCertificateExpirationTotals(Role As Enums.RoleLevelCategory) As List(Of CertificateExpirationTotals) Implements IMAISSerivce.GetCertificateExpirationTotals
        Dim retObj As New List(Of Model.CertificateExpirationTotals)
        Try
            retObj = Mapping.MAISMapping.MapDbToCertificateExpirationTotls(_queries.GetCertificateExpirationTotals(Role))

        Catch ex As Exception
            Me.LogError("Error occred whne fetching total Certification Exipration Total", CInt(Me.UserID), ex)
        End Try
        Return retObj
    End Function

    Public Function GetRNEmailAddressUsingRNsid(rnsidorrnsecetaryassociationID As Integer, flag As Integer) As String Implements IMAISSerivce.GetRNEmailAddressUsingRNsid
        Dim retObj As String = String.Empty
        Try
            retObj = _queries.GetRNEmailAddressUsingRNsid(rnsidorrnsecetaryassociationID, flag)

        Catch ex As Exception
            Me.LogError("Error occred fetching Rn email address using RNsid", CInt(Me.UserID), ex)
        End Try
        Return retObj
    End Function

    Public Function GetCourseInformationByCertificationID(CertificationID As Integer) As CourseDetails Implements IMAISSerivce.GetCourseInformationByCertificationID
        Dim retObj As New Model.CourseDetails
        Try
            retObj = Mapping.TrainingSkillsPageMapping.MapDBToModelCourseDetailesList(_queries.GetCourseInformationByCertificationID(CertificationID))
        Catch ex As Exception
            Me.LogError("Error occred fetching Course Data by CertifictionID in GetCourseInformationByCertificationID", CInt(Me.UserID), ex)
        End Try
        Return retObj

    End Function

    Public Function GetSessionCourseInfoDetailsBySesssionID(SessionID As Integer) As List(Of SessionCourseInfoDetails) Implements IMAISSerivce.GetSessionCourseInfoDetailsBySesssionID
        Dim retObj As New List(Of SessionCourseInfoDetails)
        Try
            retObj = Mapping.TrainingSkillsPageMapping.mapDBtoModelSessionCourseDetails(_queries.GetSessionCourseInfoDetailsBySesssionID(SessionID))
        Catch ex As Exception
            Me.LogError("Error occred fetching Course Data by CertifictionID in GetCourseInformationByCertificationID", CInt(Me.UserID), ex)
        End Try
        Return retObj
    End Function

    Public Function UpdateSessionCourseInfoSession(newSessionID As Integer, RoleRNDDPersonelXrefSID As Integer, CertificationID As Integer) As Boolean Implements IMAISSerivce.UpdateSessionCourseInfoSession

        Dim retObj As Boolean = False
        Try
            retObj = _queries.UpdateSessionCourseInfoSession(newSessionID, RoleRNDDPersonelXrefSID, CertificationID)
        Catch ex As Exception
            Me.LogError("Error in UpdateSessionCourseInfoSession", CInt(Me.UserID), ex)
        End Try
        Return retObj

    End Function

    Public Function GetCurrnetSessionWithCertificationID(CertificationID As Integer) As PersonCourseSession Implements IMAISSerivce.GetCurrnetSessionWithCertificationID
        Dim retObj As New Model.PersonCourseSession
        Try
            retObj = Mapping.PersonCourseMapping.MapDBToPersonCourseSessiong(_queries.GetCurrnetSessionWithCertificationID(CertificationID))
        Catch ex As Exception
            Me.LogError("Error in UpdateSessionCourseInfoSession", CInt(Me.UserID), ex)
        End Try
        Return retObj

    End Function

    Public Function UserSessionMatch(AppID As Integer, uniqueID As String, OldAppID As Integer) As Boolean Implements IMAISSerivce.UserSessionMatch
        Dim retObj As Boolean = False
        Try
            If AppID > 0 Then
                retObj = _queries.UserSessionMatch(AppID, uniqueID, OldAppID)
            Else
                retObj = True
            End If
        Catch ex As Exception
            Me.LogError("Error in UpdateSessionCourseInfoSession", CInt(Me.UserID), ex)
        End Try
        Return retObj
    End Function

    Public Function ChangeRNLicenseNumber(newRNNumber As String, exisitngRNNumber As String) As String Implements IMAISSerivce.ChangeRNLicenseNumber
        Dim retObj As String = String.Empty
        Try
            If ((Not String.IsNullOrWhiteSpace(newRNNumber)) AndAlso (Not String.IsNullOrWhiteSpace(exisitngRNNumber))) Then
                retObj = _queries.ChangeRNLicenseNumber(newRNNumber, exisitngRNNumber)
            End If
        Catch ex As Exception
            Me.LogError("Error in Updating RN License Number", CInt(Me.UserID), ex)
        End Try
        Return retObj
    End Function
End Class
