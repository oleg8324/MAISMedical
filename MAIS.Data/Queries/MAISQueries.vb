Imports ODMRDDHelperClassLibrary.Utility
Imports System.Configuration
Imports System.Data.Objects
Imports MAIS.Data.Queries
Imports MAIS.Data.Objects

Namespace Queries
    Public Interface IMAISQueries
        Inherits IQueriesBase
        Function CheckPreviousRenewal(ByVal UniqueID As String, ByVal RCL_Sid As Integer) As String
        Function CheckRenewalDone(ByVal UniqueID As String, ByVal RCL_Sid As Integer, ByVal RN_DD_FLg As Boolean) As Boolean
        Function UnRegisterQA(ByVal RLC_Sid As Integer, ByVal Role_Person_Sid As Integer) As Boolean
        Function GetSecretaryByID(ByVal U_ID As Integer) As Secretary_Association
        Function GetSecreatryList() As List(Of Secretary_Association)
        Function RemoveRnForSecretary(ByVal rs_sid As Integer) As Boolean
        Function SaveSecretaryDetails(ByVal secInfo As Secretary_Detail) As Boolean
        Function GetAllSecretaries(ByVal Email As String, ByVal Fname As String, ByVal Lname As String, ByVal RN_SId As Integer, ByVal Status As String) As List(Of Secretary_Association)
        Function GetSecretaryDetails(ByVal U_Id As Integer) As List(Of Secretary_Detail)
        Function GetRNForMappings(ByVal rnNO As String, ByVal Fname As String, ByVal Lname As String, ByVal Status As String) As List(Of Objects.RN_Mapping)
        Function GetRoleUsingUserID(ByVal userID As Integer) As Objects.MAISRNDDRoleDetails
        Function GetExistingFlg(ByVal UniqueCode As String, ByVal RN_Flg As Boolean) As ReturnObject(Of Boolean)
        Function GetCertificationHistory(ByVal UniqueCode As String, ByVal RN_Flg As Boolean) As List(Of Objects.Certificate)
        Function GetRoleCategoryLevelInfoByRoleCategoryLevelSid(ByVal RoleCategoryLevelSid As Integer) As Objects.RoleCategoryLevelDetailsObject
        Function SaveUserLoggedData(ByVal userdetails As Objects.UserMappingDetails) As Integer
        Function SaveUserRNMappingData(usermappingDetails As Objects.UserLoginSearch) As ReturnObject(Of Long)
        Function CheckSecetaryMapping(userId As Integer) As Boolean
        Function CheckRNMapping(userId As Integer) As Objects.RN_Mapping
        Function GetCertificationStartDate(ByVal UserUnique As String, ByVal categoryLevelID As Integer) As Date
        Function GetCertificationDate(ByVal UserUnique As String, ByVal CategoryLevelID As Integer) As Date
        Function GetCertificationDateByCategoryID(ByVal UserUnique As String, ByVal CategoryID As Integer) As Date
        Function GetCertificationDateThatIsHighRoleProiorityByRNSID(ByVal RNs_Sid As Integer, ByVal StartDate As Date) As Date
        Function GetCertificationMinStartDateByRNSID(ByVal RNs_Sid As Integer) As Date
        Function GetCertificationMinStartDateByDDPersonnelCode(ByVal DDPersonelCode As String) As Date
        Function SetCertStatusAndDates(ByVal rnflag As Boolean, ByVal RoleDDRNXrefSid As Integer, ByVal StatusSid As Integer, Optional startDate As Date = #12/12/1990#, Optional endDate As Date = #12/31/9999#) As ReturnObject(Of Long)
        Function GetRNsName(ByVal RNs_Sid As Integer) As String
        Function GetRNsLicenseNumber(ByVal RNs_Sid As Integer) As String
        Function UpdateRNMapping(ByVal Rnid As Integer, ByVal com As String, ByVal chFlg As Boolean) As ReturnObject(Of Boolean)
        Function GetApplicantXrefSidByCode(ByVal code As String, ByVal RN_flg As Boolean) As Integer
        Function GetApplicantNameByCode(ByVal code As String, ByVal RN_flg As Boolean) As String
        Function GetAllStates() As List(Of Objects.StateDetails)
        Function GetAllCountyCodes() As List(Of Objects.CountyDetails)
        Function GetCountyIDByCodes(ByVal countyCode As String) As Integer
        Function GetStateIDByStates(ByVal StateAbr As String)
        Function CheckTheMandatoryFields(ByVal rnLicenseNumber As String) As Integer
        Function GetAppIDByRNLicenseNumber(ByVal rnLicenseNumber As String) As Integer
        Function GetCertificateExpirationTotals(ByVal RoleLevelCategory As Integer) As List(Of Objects.CertificateExpirationTotals)
        Function GetRNEmailAddressUsingRNsid(ByVal rnsidorrnsecetaryassociationID As Integer, ByVal flag As Integer) As String
        Function GetCourseInformationByCertificationID(ByVal CertificationID As Integer) As Objects.CourseDetails
        Function GetSessionCourseInfoDetailsBySesssionID(ByVal SessionID As Integer) As List(Of Objects.SessionCourseInfoDetails)
        Function GetCurrnetSessionWithCertificationID(CertificationID As Integer) As Objects.PersonCourseSession
        Function UpdateSessionCourseInfoSession(ByVal newSessionID As Integer, ByVal RoleRNDDPersonelXrefSID As Integer, ByVal CertificationID As Integer) As Boolean
        Function UserSessionMatch(ByVal AppID As Integer, ByVal uniqueID As String, ByVal OldAppID As Integer) As Boolean
        Function ChangeRNLicenseNumber(ByVal newRNNumber As String, ByVal exisitngRNNumber As String) As String
    End Interface
    Public Class MAISQueries
        Inherits QueriesBase
        Implements IMAISQueries

        Public Function CheckRenewalDone(UniqueID As String, RCL_Sid As Integer, RN_DD_FLg As Boolean) As Boolean Implements IMAISQueries.CheckRenewalDone
            Dim retFlg As Boolean = False
            Try
                Using context As New MAISContext
                    If RN_DD_FLg Then
                        Dim rn1 = (From r In context.RNs
                                Join rd In context.RN_DD_Person_Type_Xref On r.RN_Sid Equals rd.RN_DDPersonnel_Sid
                                Join roles In context.Role_RN_DD_Personnel_Xref On rd.RN_DD_Person_Type_Xref_Sid Equals roles.RN_DD_Person_Type_Xref_Sid
                                Join c In context.Certifications On c.Role_RN_DD_Personnel_Xref_Sid Equals roles.Role_RN_DD_Personnel_Xref_Sid
                                Where r.RNLicense_Number = UniqueID And roles.Role_Category_Level_Sid = RCL_Sid
                                Select c).ToList()
                        If rn1.Count > 1 Then
                            retFlg = True
                        End If
                    Else
                        Dim rn1 = (From dp In context.DDPersonnels
                               Join rd In context.RN_DD_Person_Type_Xref On dp.DDPersonnel_Sid Equals rd.RN_DDPersonnel_Sid
                               Join roles In context.Role_RN_DD_Personnel_Xref On rd.RN_DD_Person_Type_Xref_Sid Equals roles.RN_DD_Person_Type_Xref_Sid
                               Join c In context.Certifications On c.Role_RN_DD_Personnel_Xref_Sid Equals roles.Role_RN_DD_Personnel_Xref_Sid
                               Where dp.DDPersonnel_Code = UniqueID And roles.Role_Category_Level_Sid = RCL_Sid
                               Select c).ToList()
                        If rn1.Count > 1 Then
                            retFlg = True
                        End If
                    End If
                End Using
            Catch ex As Exception
                Me.LogError("Error fetching renwal done flag", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error fetching renwal done flag", True, False))
            End Try
            Return retFlg
        End Function
        Public Function CheckPreviousRenewal(ByVal UniqueID As String, ByVal RCL_Sid As Integer) As String Implements IMAISQueries.CheckPreviousRenewal
            Dim str As String = String.Empty
            Try
                Using context As New MAISContext
                    Dim rn1 = (From r In context.RNs
                              Join rd In context.RN_DD_Person_Type_Xref On r.RN_Sid Equals rd.RN_DDPersonnel_Sid
                              Join roles In context.Role_RN_DD_Personnel_Xref On rd.RN_DD_Person_Type_Xref_Sid Equals roles.RN_DD_Person_Type_Xref_Sid
                              Join rlc In context.Role_Category_Level_Xref On roles.Role_Category_Level_Sid Equals rlc.Role_Category_Level_Sid
                              Join maisRole In context.MAIS_Role On maisRole.Role_Sid Equals rlc.Role_Sid
                              Where r.RNLicense_Number = UniqueID Order By maisRole.Role_Priority
                              Select roles.Role_RN_DD_Personnel_Xref_Sid, roles.Role_Category_Level_Sid).Distinct.ToList()
                    'role_category_level_sid  4-RN_Trainer,5-RN_Instructor,6-RN_Master,15--DDPersonCat1---check for roles 
                    If rn1.Count > 0 Then
                        If RCL_Sid = 5 Then
                            For Each r In rn1
                                If r.Role_Category_Level_Sid = 4 Then
                                    Dim c = (From cr In context.Certifications
                                            Where cr.Role_RN_DD_Personnel_Xref_Sid = r.Role_RN_DD_Personnel_Xref_Sid
                                            Select cr).ToList()
                                    If c.Count > 1 Then
                                    Else
                                        If (r.Role_Category_Level_Sid = 4) Then
                                            str = "RN Trainer Renewal is required"
                                            Exit For
                                        End If
                                    End If
                                    'Else
                                    '    str = "Person do not have Roles prior to Instructor as RN Trianer"
                                End If
                            Next
                        ElseIf RCL_Sid = 6 Then
                            For Each r In rn1
                                If r.Role_Category_Level_Sid = 4 Or r.Role_Category_Level_Sid = 5 Then
                                    Dim c = (From cr In context.Certifications
                                                                        Where cr.Role_RN_DD_Personnel_Xref_Sid = r.Role_RN_DD_Personnel_Xref_Sid
                                                                        Select cr).ToList()
                                    If c.Count > 1 Then
                                    Else
                                        If (r.Role_Category_Level_Sid = 4) Then
                                            str = "RN Trainer Renewal is required"
                                            Exit For
                                        ElseIf (r.Role_Category_Level_Sid = 5) Then
                                            str = "RN Instructor renewal is required"
                                            Exit For
                                        End If
                                    End If
                                    'Else
                                    '    str = "Person do not have enough role prior to RN Master"
                                End If
                            Next
                        End If
                    End If
                End Using
            Catch ex As Exception
                Me.LogError("Error fetching secretary info", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error fetching secretary info", True, False))
            End Try
            Return str
        End Function
        Public Function UnRegisterQA(RLC_Sid As Integer, Role_Person_Sid As Integer) As Boolean Implements IMAISQueries.UnRegisterQA
            Dim rn As Boolean = False
            Try
                Using context As New MAISContext
                    Dim retRole As New Role_RN_DD_Personnel_Xref
                    retRole = (From rr In context.Role_RN_DD_Personnel_Xref
                               Where rr.Role_RN_DD_Personnel_Xref_Sid = Role_Person_Sid And rr.Role_Category_Level_Sid = RLC_Sid
                               Select rr).FirstOrDefault()
                    If retRole IsNot Nothing Then
                        retRole.Role_End_Date = DateTime.Today.AddDays(-1)
                        retRole.Last_Update_By = Me.UserID
                        retRole.Last_Update_Date = DateTime.Today
                        retRole.Active_Flg = False
                        context.SaveChanges()

                        Dim retcert As New Certification
                        retcert = (From cc In context.Certifications
                                   Where cc.Role_RN_DD_Personnel_Xref_Sid = Role_Person_Sid
                                   Select cc).FirstOrDefault()
                        If retcert IsNot Nothing Then
                            retcert.Certification_End_Date = DateTime.Today.AddDays(-1)
                            retcert.Last_Update_By = Me.UserID
                            retcert.Last_Update_Date = DateTime.Today
                            retcert.Active_Flg = True
                            context.SaveChanges()
                            Dim certStatus As New Certification_Status
                            certStatus = (From cs In context.Certification_Status
                                          Where cs.Certification_Sid = retcert.Certification_Sid Order By cs.Certification_Status_Sid Descending
                                          Select cs).FirstOrDefault()
                            If certStatus IsNot Nothing Then
                                certStatus.Status_End_Date = DateTime.Today.AddDays(-2)
                                certStatus.Last_Update_By = Me.UserID
                                certStatus.Last_Update_Date = DateTime.Today
                                context.SaveChanges()
                                Dim cerStatusNew As New Certification_Status
                                cerStatusNew.Certification_Sid = certStatus.Certification_Sid
                                cerStatusNew.Certification_Status_Type_Sid = (From cs In context.Certification_Status_Type Where cs.Certification_Status_Desc = "Unregistered" Select cs.Certification_Status_Type_Sid).FirstOrDefault 'unregister
                                cerStatusNew.Status_Start_Date = DateTime.Today.AddDays(-1)
                                cerStatusNew.Status_End_Date = DateTime.Today.AddDays(-1)
                                cerStatusNew.Active_Flg = False
                                cerStatusNew.Create_By = Me.UserID
                                cerStatusNew.Create_Date = DateTime.Today
                                cerStatusNew.Last_Update_By = Me.UserID
                                cerStatusNew.Last_Update_Date = DateTime.Today
                                context.Certification_Status.Add(cerStatusNew)
                                context.SaveChanges()
                            End If
                        End If
                    End If
                    rn = True
                End Using
            Catch ex As Exception
                Me.LogError("Error fetching secretary info", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error fetching secretary info", True, False))
            End Try
            Return rn
        End Function
        Public Function GetSecretaryByID(ByVal U_ID As Integer) As Secretary_Association Implements IMAISQueries.GetSecretaryByID
            Dim Secinfo As New Secretary_Association
            Try
                Using context As New MAISContext
                    Secinfo = (From ss In context.User_Mapping
                               Join e In context.Email1 On e.Email_SID Equals ss.Email_Sid
                               Where ss.User_Mapping_Sid = U_ID
                               Select New Objects.Secretary_Association() With {
                                   .Email = e.Email_Address,
                                   .First_Name = ss.First_Name,
                                   .Last_Name = ss.Last_Name,
                                   .Middle_Name = ss.Middle_Name,
                                   .SecretaryUserName = ss.User_Code,
                                   .User_Mapping_Sid = ss.User_Mapping_Sid,
                                   .Last_Updated_By = ss.Last_Update_By,
                                   .Last_Updated_Date = ss.Last_Update_Date
                                   }).FirstOrDefault()
                End Using
            Catch ex As Exception
                Me.LogError("Error fetching secretary info", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error fetching secretary info", True, False))
            End Try
            Return Secinfo
        End Function
        Public Function GetSecreatryList() As List(Of Secretary_Association) Implements IMAISQueries.GetSecreatryList
            Dim secList As New List(Of Secretary_Association)
            Try
                Using context As New MAISContext
                    secList = (From s In context.User_Mapping
                               Join e In context.Email1 On s.Email_Sid Equals e.Email_SID
                               Where s.Is_Secretary_Flg = True
                              Select New Objects.Secretary_Association() With {
                                      .SecretaryUserName = s.User_Code,
                                      .Middle_Name = s.Middle_Name,
                                      .Last_Updated_Date = s.Last_Update_Date,
                                      .Last_Updated_By = s.Last_Update_By,
                                      .Last_Name = s.Last_Name,
                                      .First_Name = s.First_Name,
                                      .Email = e.Email_Address,
                                      .User_Mapping_Sid = s.User_Mapping_Sid
                                  }).ToList()
                End Using
            Catch ex As Exception
                Me.LogError("Error geting list of secretaries", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error geting list of secretaries", True, False))
            End Try
            Return secList
        End Function
        Public Function RemoveRnForSecretary(rs_sid As Integer) As Boolean Implements IMAISQueries.RemoveRnForSecretary
            Dim retobj As Boolean = False
            Try
                If rs_sid > 0 Then
                    Using context As New MAISContext
                        Dim secInfo As RN_Secretary_Association
                        secInfo = (From r In context.RN_Secretary_Association
                                   Where r.RN_Secretary_Association_Sid = rs_sid
                                   Select r).FirstOrDefault()
                        If secInfo IsNot Nothing Then
                            secInfo.Last_Update_By = Me.UserID
                            secInfo.Last_Update_Date = DateTime.Today
                            secInfo.Active_Flg = False
                            context.SaveChanges()
                        End If
                        retobj = True
                    End Using
                End If

            Catch ex As Exception
                Me.LogError("Error removing secretary details", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error removing secretary details", True, False))
            End Try
            Return retobj
        End Function
        Public Function SaveSecretaryDetails(ByVal secInfo As Secretary_Detail) As Boolean Implements IMAISQueries.SaveSecretaryDetails
            Dim retObj As Boolean = False
            Try
                If secInfo IsNot Nothing Then

                    Using context As New MAISContext
                        If secInfo.RN_Sid > 0 Then
                            Dim sec As RN_Secretary_Association
                            sec = (From rs In context.RN_Secretary_Association
                                       Where rs.RN_Sid = secInfo.RN_Sid And rs.User_Mapping_Sid = secInfo.User_Mapping_Sid
                                       Select rs).FirstOrDefault()
                            If sec IsNot Nothing Then
                                sec.Comments = secInfo.Comments
                                sec.Active_Flg = True
                                sec.Last_Update_By = Me.UserID
                                sec.Last_Update_Date = DateTime.Today
                                context.SaveChanges()
                            Else
                                Dim sec_Details As New RN_Secretary_Association
                                sec_Details.User_Mapping_Sid = secInfo.User_Mapping_Sid
                                sec_Details.RN_Sid = secInfo.RN_Sid
                                sec_Details.Comments = secInfo.Comments
                                sec_Details.Active_Flg = True
                                sec_Details.Create_By = Me.UserID
                                sec_Details.Create_Date = DateTime.Today
                                sec_Details.Last_Update_By = Me.UserID
                                sec_Details.Last_Update_Date = DateTime.Today
                                context.RN_Secretary_Association.Add(sec_Details)
                                context.SaveChanges()
                            End If
                            retObj = True
                        End If
                    End Using
                End If
            Catch ex As Exception
                Me.LogError("Error in saving secretariy details.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error in saving secretary details.", True, False))
            End Try
            Return retObj
        End Function
        Public Function GetSecretaryDetails(ByVal U_Id As Integer) As List(Of Secretary_Detail) Implements IMAISQueries.GetSecretaryDetails
            Dim retObj As New List(Of Secretary_Detail)
            Try
                If U_Id > 0 Then
                    Using context As New MAISContext
                        retObj = (From rs In context.RN_Secretary_Association
                                    Join r In context.RNs On rs.RN_Sid Equals r.RN_Sid
                                    Where rs.User_Mapping_Sid = U_Id And rs.Active_Flg = True
                                    Select New Objects.Secretary_Detail() With {
                                        .RN_Secretary_Association_Sid = rs.RN_Secretary_Association_Sid,
                                        .Comments = rs.Comments,
                                        .F_Name = r.First_Name,
                                        .L_Name = r.Last_Name,
                                        .M_Name = r.Middle_Name,
                                        .RN_Sid = r.RN_Sid,
                                        .RNLicense = r.RNLicense_Number,
                                        .Last_Updated_By = rs.Last_Update_By,
                                        .Last_Updated_Date = rs.Last_Update_Date,
                                        .User_Mapping_Sid = rs.User_Mapping_Sid
                                    }).ToList()
                    End Using
                End If
            Catch ex As Exception
                Me.LogError("Error in fetching secretaries details.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error in fetching secretaries details.", True, False))
            End Try
            Return retObj
        End Function
        Public Function GetAllSecretaries(ByVal Email As String, ByVal Fname As String, ByVal Lname As String, ByVal RN_SId As Integer, ByVal Status As String) As List(Of Secretary_Association) Implements IMAISQueries.GetAllSecretaries
            Dim retObj As New List(Of Secretary_Association)
            Try
                Using context As New MAISContext
                    Dim sts As New Integer
                    Select Case Status
                        Case "0"
                            sts = 2
                        Case "1"
                            sts = 2
                        Case "2"
                            sts = 0
                        Case "3"
                            sts = 1
                    End Select
                    'Dim retObj1 = (From u In context.RN_Secretary_Association
                    '          Group u. = Fname By u.RN_Sid Into g = Group
                    '          Where g.Count > 0 Select g).ToList()

                    If (sts = 2) Then
                        If (RN_SId > 0) Then
                            retObj = (From s In context.User_Mapping
                                        Join e In context.Email1 On e.Email_SID Equals s.Email_Sid
                                        Join rs In context.RN_Secretary_Association On rs.User_Mapping_Sid Equals s.User_Mapping_Sid
                                         Where If(Email.Equals(String.Empty), True, e.Email_Address = Email) AndAlso _
                                               If(Fname.Equals(String.Empty), True, s.First_Name.Contains(Fname)) AndAlso _
                                               If(Lname.Equals(String.Empty), True, s.Last_Name.Contains(Lname)) AndAlso _
                                               rs.RN_Sid = RN_SId AndAlso rs.Active_Flg = True AndAlso _
                                               s.Is_Secretary_Flg = True
                                       Select New Objects.Secretary_Association() With {
                                                 .First_Name = s.First_Name,
                                                 .Last_Name = s.Last_Name,
                                                 .Middle_Name = s.Middle_Name,
                                                 .SecretaryUserName = s.User_Code,
                                                 .Email = e.Email_Address,
                                                 .User_Mapping_Sid = s.User_Mapping_Sid,
                                                 .Last_Updated_By = s.Last_Update_By,
                                                 .Last_Updated_Date = s.Last_Update_Date
                                             }).Distinct().ToList
                        Else
                            retObj = (From s In context.User_Mapping
                                        Join e In context.Email1 On e.Email_SID Equals s.Email_Sid
                                        Where If(Email.Equals(String.Empty), True, e.Email_Address = Email) AndAlso _
                                            If(Fname.Equals(String.Empty), True, s.First_Name.Contains(Fname)) AndAlso _
                                            If(Lname.Equals(String.Empty), True, s.Last_Name.Contains(Lname)) AndAlso _
                                            s.Is_Secretary_Flg = True
                                    Select New Objects.Secretary_Association() With {
                                                .First_Name = s.First_Name,
                                                .Last_Name = s.Last_Name,
                                                .Middle_Name = s.Middle_Name,
                                                .SecretaryUserName = s.User_Code,
                                                .Email = e.Email_Address,
                                                .User_Mapping_Sid = s.User_Mapping_Sid,
                                                .Last_Updated_By = s.Last_Update_By,
                                                .Last_Updated_Date = s.Last_Update_Date
                                            }).ToList()
                        End If

                    ElseIf ((sts = 0) Or (sts = 1)) Then

                        retObj = (From s In context.User_Mapping
                             Join e In context.Email1 On e.Email_SID Equals s.Email_Sid
                             Join rs In context.RN_Secretary_Association On rs.User_Mapping_Sid Equals s.User_Mapping_Sid
                              Where If(Email.Equals(String.Empty), True, e.Email_Address = Email) AndAlso _
                                    If(Fname.Equals(String.Empty), True, s.First_Name.Contains(Fname)) AndAlso _
                                    If(Lname.Equals(String.Empty), True, s.Last_Name.Contains(Lname)) AndAlso _
                                    If(sts = 0, rs.Active_Flg = True, rs.Active_Flg = False) AndAlso _
                                    If(RN_SId > 0, rs.RN_Sid = RN_SId, True) AndAlso _
                                    s.Is_Secretary_Flg = True
                            Select New Objects.Secretary_Association() With {
                                      .First_Name = s.First_Name,
                                      .Last_Name = s.Last_Name,
                                      .Middle_Name = s.Middle_Name,
                                      .SecretaryUserName = s.User_Code,
                                      .Email = e.Email_Address,
                                      .User_Mapping_Sid = s.User_Mapping_Sid,
                                      .Last_Updated_By = s.Last_Update_By,
                                      .Last_Updated_Date = s.Last_Update_Date
                                  }).Distinct().ToList
                        Dim retObjre As New List(Of Secretary_Association)
                        'retObjre = retObj
                        If sts = 1 And retObj.Count > 0 Then
                            For Each rr In retObj
                                Dim rf = (From r In context.RN_Secretary_Association
                                          Where r.Active_Flg = True And rr.User_Mapping_Sid = r.User_Mapping_Sid
                                          Select r.User_Mapping_Sid)

                                If rf.Count > 0 Then
                                    'retObjre.Add(rr)
                                Else
                                    retObjre.Add(rr)
                                End If
                            Next
                            retObj = retObjre
                        End If
                    End If
                End Using
            Catch ex As Exception
                Me.LogError("Error in fetching secretaries.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error in fetching secretaries.", True, False))
            End Try
            Return retObj
        End Function
        Public Function UpdateRNMapping(ByVal Rnid As Integer, ByVal com As String, ByVal chFlg As Boolean) As ReturnObject(Of Boolean) Implements IMAISQueries.UpdateRNMapping
            Dim retObj As New ReturnObject(Of Boolean)(False)
            Try
                Using context As New MAISContext
                    Dim retInfo As User_RN_Mapping
                    retInfo = (From ur In context.User_RN_Mapping
                               Where ur.RN_Sid = Rnid
                               Select ur).FirstOrDefault()
                    If retInfo IsNot Nothing Then
                        retInfo.Unmap_Flg = chFlg
                        retInfo.Comments = retInfo.Comments + " " + com
                        retInfo.Last_Update_By = Me.UserID
                        retInfo.Last_Update_Date = DateTime.Today
                        context.SaveChanges()
                        retObj.ReturnValue = True
                    End If
                End Using
            Catch ex As Exception
                Me.LogError("Error in updateing RN mappings.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error in updateing RN mappings.", True, False))
            End Try
            Return retObj
        End Function
        Public Function GetRNForMappings(ByVal rnNO As String, ByVal Fname As String, ByVal Lname As String, ByVal Status As String) As List(Of Objects.RN_Mapping) Implements IMAISQueries.GetRNForMappings
            Dim retObj As New List(Of Objects.RN_Mapping)
            Try
                Dim sts As New Integer
                Using context As New MAISContext
                    Select Case Status
                        Case "0"
                            sts = 2
                        Case "1"
                            sts = 2
                        Case "2"
                            sts = 0
                        Case "3"
                            sts = 1
                    End Select
                    If ((sts = 0) Or (sts = 1)) Then
                        retObj = (From r In context.RNs
                             Join ur In context.User_RN_Mapping On r.RN_Sid Equals ur.RN_Sid
                             Where If(rnNO.Equals(String.Empty), True, r.RNLicense_Number = rnNO) AndAlso _
                                   If(Fname.Equals(String.Empty), True, r.First_Name.Contains(Fname)) AndAlso _
                                   If(Lname.Equals(String.Empty), True, r.Last_Name.Contains(Lname)) AndAlso _
                                  If(sts = 1, ur.Unmap_Flg = True, ur.Unmap_Flg = False)
                             Select New Objects.RN_Mapping() With {
                                 .RN_Sid = r.RN_Sid,
                                 .First_Name = r.First_Name,
                                 .Last_Name = r.Last_Name,
                                 .Middle_Name = r.Middle_Name,
                                 .RNLicenseNumber = r.RNLicense_Number,
                                 .Comments = ur.Comments,
                                 .Un_Map_Flg = ur.Unmap_Flg,
                                 .Last_Updated_By = ur.Last_Update_By,
                                 .Last_Updated_Date = ur.Last_Update_Date
                                 }).ToList()
                    Else
                        retObj = (From r In context.RNs
                              Join ur In context.User_RN_Mapping On r.RN_Sid Equals ur.RN_Sid
                              Where If(rnNO.Equals(String.Empty), True, r.RNLicense_Number = rnNO) AndAlso _
                                    If(Fname.Equals(String.Empty), True, r.First_Name.Contains(Fname)) AndAlso _
                                    If(Lname.Equals(String.Empty), True, r.Last_Name.Contains(Lname))
                              Select New Objects.RN_Mapping() With {
                                  .RN_Sid = r.RN_Sid,
                                  .First_Name = r.First_Name,
                                  .Last_Name = r.Last_Name,
                                  .Middle_Name = r.Middle_Name,
                                  .RNLicenseNumber = r.RNLicense_Number,
                                  .Comments = ur.Comments,
                                  .Un_Map_Flg = ur.Unmap_Flg,
                                  .Last_Updated_By = ur.Last_Update_By,
                                  .Last_Updated_Date = ur.Last_Update_Date
                                  }).ToList()
                    End If

                End Using
            Catch ex As Exception
                Me.LogError("Error in geting existing RN mappings.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error in geting existing RN mappings.", True, False))
            End Try
            Return retObj
        End Function
        Public Function GetCertificationHistory(ByVal UniqueCode As String, ByVal RN_Flg As Boolean) As List(Of Objects.Certificate) Implements IMAISQueries.GetCertificationHistory
            Dim retObj As New List(Of Objects.Certificate)
            Try
                Dim ptypeSid As Integer = If(RN_Flg, 1, 2) 'lkp.Person_Type 1 = RN, 2 = DD
                Using context As New MAISContext
                    Dim maisRoles As New List(Of Role_RN_DD_Personnel_Xref)
                    If RN_Flg Then 'RN certification history
                        retObj = (From r In context.RNs
                                      Join rd In context.RN_DD_Person_Type_Xref On rd.RN_DDPersonnel_Sid Equals r.RN_Sid
                                      Join mrole In context.Role_RN_DD_Personnel_Xref On mrole.RN_DD_Person_Type_Xref_Sid Equals rd.RN_DD_Person_Type_Xref_Sid
                                      Join lkp_rlc In context.Role_Category_Level_Xref On lkp_rlc.Role_Category_Level_Sid Equals mrole.Role_Category_Level_Sid
                                      Join ch In context.Certifications On ch.Role_RN_DD_Personnel_Xref_Sid Equals mrole.Role_RN_DD_Personnel_Xref_Sid
                                      Join ro In context.MAIS_Role On ro.Role_Sid Equals lkp_rlc.Role_Sid
                                      Join caty In context.Category_Type On caty.Category_Type_Sid Equals lkp_rlc.Category_Type_Sid
                                      Join l In context.Level_Type On l.Level_Type_Sid Equals lkp_rlc.Level_Type_Sid
                                      Where r.RNLicense_Number = UniqueCode And rd.Person_Type_Sid = ptypeSid And ch.Active_Flg = True
                                      Select New Objects.Certificate() With {
                                          .Role_RN_DD_Personnel_Xref_Sid = mrole.Role_RN_DD_Personnel_Xref_Sid,
                                          .Category = caty.Category_Code,
                                          .Role_Category_Level_Sid = lkp_rlc.Role_Category_Level_Sid,
                                          .Level = l.Level_Code,
                                          .Role = ro.Role_Desc,
                                          .RolePriority = ro.Role_Priority,
                                          .StartDate = ch.Certification_Start_Date,
                                          .EndDate = ch.Certification_End_Date,
                                          .TestId = mrole.Role_RN_DD_Personnel_Xref_Sid,
                                          .Certification_Sid = ch.Certification_Sid,
                                          .Application_Sid = If(ch.Application_Sid(), ch.Application_Sid, 0)
                                      }).Distinct().ToList()
                        For Each rr In retObj
                            Dim str As String = (From cs In context.Certification_Status
                                                 Join cst In context.Certification_Status_Type On cs.Certification_Status_Type_Sid Equals cst.Certification_Status_Type_Sid
                                                 Where cs.Certification_Sid = rr.Certification_Sid Order By cs.Certification_Status_Sid Descending
                                                 Select cst.Certification_Status_Desc).FirstOrDefault()
                            rr.Status = If(String.IsNullOrWhiteSpace(str), String.Empty, str)

                            rr.ApplicationType = (From c In context.Renewal_History_Count
                            Join appType In context.Application_Type On appType.Application_Type_Sid Equals c.Application_type_SId
                            Join rd In context.RN_DD_Person_Type_Xref On rd.RN_DD_Person_Type_Xref_Sid Equals c.RN_DD_Person_Type_Xref_Sid
                            Join dd In context.RNs On rd.RN_DDPersonnel_Sid Equals dd.RN_Sid
                            Where c.Role_Category_Level_sid = rr.Role_Category_Level_Sid And dd.RNLicense_Number = UniqueCode Order By c.Last_Update_Date Descending
                                Select appType.Application_Code).FirstOrDefault
                            If (rr.ApplicationType Is Nothing) Then
                                rr.ApplicationType = "Renewal"
                            End If
                        Next
                        'Join cs In context.Certification_Status On cs.Certification_Sid Equals ch.Certification_Sid
                        '            Join cst In context.Certification_Status_Type On cs.Certification_Status_Type_Sid Equals cst.Certification_Status_Type_Sid
                        '  .Status = cst.Certification_Status_Desc,
                    Else 'DD certification history
                        retObj = (From dd In context.DDPersonnels
                                        Join rd In context.RN_DD_Person_Type_Xref On rd.RN_DDPersonnel_Sid Equals dd.DDPersonnel_Sid
                                        Join mrole In context.Role_RN_DD_Personnel_Xref On mrole.RN_DD_Person_Type_Xref_Sid Equals rd.RN_DD_Person_Type_Xref_Sid
                                        Join lkp_rlc In context.Role_Category_Level_Xref On lkp_rlc.Role_Category_Level_Sid Equals mrole.Role_Category_Level_Sid
                                        Join ch In context.Certifications On ch.Role_RN_DD_Personnel_Xref_Sid Equals mrole.Role_RN_DD_Personnel_Xref_Sid
                                        Join ro In context.MAIS_Role On ro.Role_Sid Equals lkp_rlc.Role_Sid
                                        Join caty In context.Category_Type On caty.Category_Type_Sid Equals lkp_rlc.Category_Type_Sid
                                        Join l In context.Level_Type On l.Level_Type_Sid Equals lkp_rlc.Level_Type_Sid
                                        Where dd.DDPersonnel_Code = UniqueCode And rd.Person_Type_Sid = ptypeSid And ch.Active_Flg = True
                                        Select New Objects.Certificate() With {
                                              .Role_RN_DD_Personnel_Xref_Sid = mrole.Role_RN_DD_Personnel_Xref_Sid,
                                              .Category = caty.Category_Code,
                                              .Role_Category_Level_Sid = lkp_rlc.Role_Category_Level_Sid,
                                              .Level = l.Level_Code,
                                              .Role = ro.Role_Desc,
                                              .RolePriority = ro.Role_Priority,
                                              .StartDate = ch.Certification_Start_Date,
                                              .EndDate = ch.Certification_End_Date,
                                              .Certification_Sid = ch.Certification_Sid,
                                              .Application_Sid = If(ch.Application_Sid(), ch.Application_Sid, 0)
                                          }).Distinct().ToList()
                        For Each rr In retObj
                            rr.ApplicationType = (From c In context.Renewal_History_Count
                            Join appType In context.Application_Type On appType.Application_Type_Sid Equals c.Application_type_SId
                            Join rd In context.RN_DD_Person_Type_Xref On rd.RN_DD_Person_Type_Xref_Sid Equals c.RN_DD_Person_Type_Xref_Sid
                            Join dd In context.DDPersonnels On rd.RN_DDPersonnel_Sid Equals dd.DDPersonnel_Sid
                            Where c.Role_Category_Level_sid = rr.Role_Category_Level_Sid And dd.DDPersonnel_Code = UniqueCode Order By c.Last_Update_Date Descending
                                Select appType.Application_Desc).FirstOrDefault
                        Next
                    End If
                    If retObj.Count > 0 Then
                        For Each rr In retObj
                            Dim str As String = (From cs In context.Certification_Status
                                                 Join cst In context.Certification_Status_Type On cs.Certification_Status_Type_Sid Equals cst.Certification_Status_Type_Sid
                                                 Where cs.Certification_Sid = rr.Certification_Sid Order By cs.Certification_Status_Sid Descending
                                                 Select cst.Certification_Status_Desc).FirstOrDefault()
                            rr.Status = If(String.IsNullOrWhiteSpace(str), String.Empty, str)
                        Next
                    End If
                End Using
            Catch ex As Exception
                Me.LogError("Error in geting existing certification history.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error in geting existing certification history.", True, False))
            End Try
            Return retObj
        End Function
        Public Function GetExistingFlg(ByVal UniueCode As String, ByVal RN_Flg As Boolean) As ReturnObject(Of Boolean) Implements IMAISQueries.GetExistingFlg
            Dim retObj As New ReturnObject(Of Boolean)(False)
            Try
                Using context As New MAISContext
                    If RN_Flg Then
                        Dim rnExist = (From r In context.RNs
                                       Where r.RNLicense_Number = UniueCode
                                       Select r).FirstOrDefault()
                        If rnExist IsNot Nothing Then
                            retObj.ReturnValue = True
                        End If
                    Else
                        Dim rnExist = (From dd In context.DDPersonnels
                                      Where dd.DDPersonnel_Code = UniueCode
                                      Select dd).FirstOrDefault()
                        If rnExist IsNot Nothing Then
                            retObj.ReturnValue = True
                        End If
                    End If
                End Using
            Catch ex As Exception
                Me.LogError("Error in geting existing flg from permanent.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error in geting existing flg from permanent.", True, False))
            End Try
            Return retObj
        End Function
        Public Function GetApplicantNameByCode(ByVal code As String, ByVal RN_flg As Boolean) As String Implements IMAISQueries.GetApplicantNameByCode
            Dim nm As String = ""
            Try
                Using context As New MAISContext
                    If RN_flg Then
                        nm = (From r In context.RNs Where r.RNLicense_Number = code
                                       Select r.First_Name + " " + r.Last_Name).FirstOrDefault()

                    Else
                        nm = (From r In context.DDPersonnels Where r.DDPersonnel_Code = code
                                       Select r.First_Name + " " + r.Last_Name).FirstOrDefault()

                    End If
                End Using
            Catch ex As Exception
                Me.LogError("Error in geting existing flg from permanent.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error in geting existing flg from permanent.", True, False))
            End Try
            Return nm
        End Function
        Public Function GetApplicantXrefSidByCode(ByVal code As String, ByVal RN_flg As Boolean) As Integer Implements IMAISQueries.GetApplicantXrefSidByCode
            Dim xrefSid As Integer = 0
            Try
                Using context As New MAISContext
                    If RN_flg Then
                        xrefSid = (From rx In context.RN_DD_Person_Type_Xref Join r In context.RNs On rx.RN_DDPersonnel_Sid Equals r.RN_Sid Where r.RNLicense_Number = code
                                       Select rx.RN_DD_Person_Type_Xref_Sid).FirstOrDefault()

                    Else
                        xrefSid = (From rx In context.RN_DD_Person_Type_Xref Join d In context.DDPersonnels On rx.RN_DDPersonnel_Sid Equals d.DDPersonnel_Sid Where d.DDPersonnel_Code = code
                                      Select rx.RN_DD_Person_Type_Xref_Sid).FirstOrDefault()

                    End If
                End Using
            Catch ex As Exception
                Me.LogError("Error in geting existing flg from permanent.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error in geting existing flg from permanent.", True, False))
            End Try
            Return xrefSid
        End Function
        Public Function GetRoleUsingUserID(userID As Integer) As Objects.MAISRNDDRoleDetails Implements Queries.IMAISQueries.GetRoleUsingUserID
            Dim roles As Objects.MAISRNDDRoleDetails = Nothing
            Dim IsSecretary As Boolean = False
            Try
                Using context As New MAISContext
                    IsSecretary = (From s In context.User_Mapping
                                   Where s.UserID = userID
                                   Select s.Is_Secretary_Flg).FirstOrDefault
                    If IsSecretary = False Then
                        roles = (From c In context.User_RN_Mapping _
                                 Join rn In context.RNs On rn.RN_Sid Equals c.RN_Sid _
                                 Join rnddPP In context.RN_DD_Person_Type_Xref On rnddPP.RN_DDPersonnel_Sid Equals rn.RN_Sid _
                                 Join mr In context.Role_RN_DD_Personnel_Xref On mr.RN_DD_Person_Type_Xref_Sid Equals rnddPP.RN_DD_Person_Type_Xref_Sid _
                                 Join rcl In context.Role_Category_Level_Xref On mr.Role_Category_Level_Sid Equals rcl.Role_Category_Level_Sid _
                                 Join r In context.MAIS_Role On r.Role_Sid Equals rcl.Role_Sid
                                 Where mr.Active_Flg = True And c.UserID = userID And mr.Role_End_Date >= DateTime.Now
                                 Order By r.Role_Priority Ascending
                                 Select New Objects.MAISRNDDRoleDetails With {
                                                    .Priority = r.Role_Priority,
                                                    .RNLicenseNumber = rn.RNLicense_Number,
                                                    .RNSID = rn.RN_Sid,
                                                    .RoleName = r.Role_Desc,
                                                    .RoleSID = rcl.Role_Category_Level_Sid,
                                                    .StartDate = rcl.Start_Date}).FirstOrDefault

                    Else
                        roles = (From s In context.User_Mapping
                                 Where s.UserID = userID
                                 Select New Objects.MAISRNDDRoleDetails With {
                                     .Priority = 7,
                                     .RNLicenseNumber = Nothing,
                                     .RNSID = Nothing,
                                     .RoleName = "Secretary",
                                     .RoleSID = 14,
                                     .StartDate = "3/4/2013"}).FirstOrDefault

                    End If

                End Using

            Catch ex As Exception
                Me.LogError("Error in getting role using userid", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error in getting role using userid", True, False))
                Throw
            End Try

            Return roles
        End Function
        Public Function SaveUserLoggedData(userdetails As Objects.UserMappingDetails) As Integer Implements IMAISQueries.SaveUserLoggedData
            Dim roles As Integer = 0
            Try
                If (userdetails IsNot Nothing) Then
                    Using context As New MAISContext
                        'Dim existsFlag As Boolean = False
                        Dim usermapping As User_Mapping = (From userMap In context.User_Mapping Where userMap.UserID = userdetails.UserID Select userMap).FirstOrDefault()
                        Dim em_sid As Integer = 0
                        If userdetails.Email.Length > 0 Then
                            em_sid = (From e In context.Email1
                                  Where e.Email_Address = userdetails.Email
                                  Select e.Email_SID).FirstOrDefault()
                            If (em_sid = 0) Then
                                Dim em1 As New Email1
                                em1.Email_Address = userdetails.Email
                                em1.Created = DateTime.Today
                                em1.Last_Updated = DateTime.Today
                                context.Email1.Add(em1)
                                context.SaveChanges()
                                em_sid = em1.Email_SID
                            End If
                        End If
                        If (usermapping IsNot Nothing) Then
                            usermapping.UserID = userdetails.UserID
                            usermapping.Portal_User_Role = userdetails.PortalUserRole
                            usermapping.Middle_Name = userdetails.MiddleName
                            usermapping.First_Name = userdetails.FirstName
                            usermapping.Last_Name = userdetails.LastName
                            usermapping.Is_Secretary_Flg = userdetails.Is_Secretary
                            usermapping.User_Code = userdetails.User_Code
                            If (em_sid > 0) Then
                                usermapping.Email_Sid = em_sid
                            End If
                            usermapping.Last_Update_By = Me.UserID
                            usermapping.Last_Update_Date = DateTime.Today
                        Else
                            Dim user As New User_Mapping
                            user.UserID = userdetails.UserID
                            user.Start_Date = DateTime.Today
                            user.Active_Flg = True
                            user.End_Date = Convert.ToDateTime("12/31/9999")
                            user.Portal_User_Role = userdetails.PortalUserRole
                            user.Middle_Name = userdetails.MiddleName
                            user.First_Name = userdetails.FirstName
                            user.Last_Name = userdetails.LastName
                            user.Is_Secretary_Flg = userdetails.Is_Secretary
                            user.User_Code = userdetails.User_Code
                            If (em_sid > 0) Then
                                user.Email_Sid = em_sid
                            End If
                            user.Last_Update_By = Me.UserID
                            user.Last_Update_Date = DateTime.Today
                            user.Create_By = Me.UserID
                            user.Create_Date = DateTime.Today
                            context.User_Mapping.Add(user)
                        End If
                        context.SaveChanges()
                        roles = (From userMap In context.User_Mapping Where userMap.UserID = userdetails.UserID Select userMap.User_Mapping_Sid).FirstOrDefault()
                    End Using
                End If
            Catch ex As Exception
                Me.LogError("Error occured when inserting data in user mapping", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured when inserting data in user mapping", True, False))
                Throw
            End Try
            Return roles
        End Function
        Public Function SaveUserRNMappingData(usermappingDetails As Objects.UserLoginSearch) As ReturnObject(Of Long) Implements IMAISQueries.SaveUserRNMappingData
            Dim roles As New ReturnObject(Of Long)(-1L)
            Try
                If (usermappingDetails IsNot Nothing) Then
                    Using context As New MAISContext
                        Dim rnDetails As RN = (From rn In context.RNs Where rn.RNLicense_Number = usermappingDetails.RNLicenseNumber _
                                            And rn.First_Name.Equals(usermappingDetails.FirstName, StringComparison.OrdinalIgnoreCase) And rn.Last_Name.Equals(usermappingDetails.LastName, StringComparison.OrdinalIgnoreCase) _
                                            Select rn).FirstOrDefault()
                        If (rnDetails IsNot Nothing) Then
                            Dim userMapping As User_RN_Mapping = (From user In context.User_RN_Mapping
                                                                  Join usermapping1 In context.User_Mapping On usermapping1.User_Mapping_Sid Equals user.User_Mapping_Sid
                                                                  Join rnDetail In context.RNs On rnDetail.RN_Sid Equals user.RN_Sid
                                                                  Where user.UserID = Me.UserID Select user).FirstOrDefault()
                            Dim userMappingID As Integer = (From usermapping1 In context.User_Mapping Where usermapping1.UserID = Me.UserID Select usermapping1.User_Mapping_Sid).FirstOrDefault()
                            If (userMapping Is Nothing) Then
                                Dim newUser As New User_RN_Mapping
                                newUser.UserID = Me.UserID
                                newUser.RN_Sid = rnDetails.RN_Sid
                                newUser.User_Mapping_Sid = userMappingID
                                newUser.Create_By = Me.UserID
                                newUser.Create_Date = DateTime.Today
                                newUser.Last_Update_By = Me.UserID
                                newUser.Last_Update_Date = DateTime.Today
                                roles.AddMessage(rnDetails.RNLicense_Number)
                                context.User_RN_Mapping.Add(newUser)
                                context.SaveChanges()
                            Else
                                Dim rnDetails1 As RN = (From rn In context.RNs Where rn.RN_Sid = userMapping.RN_Sid Select rn).FirstOrDefault()
                                If (rnDetails.RN_Sid = userMapping.RN_Sid) Then
                                    userMapping.Last_Update_By = Me.UserID
                                    userMapping.Last_Update_Date = DateTime.Today
                                    roles.AddMessage(rnDetails1.RNLicense_Number)
                                    context.SaveChanges()
                                Else
                                    roles.AddErrorMessage("You already mapped to the system with the RN License Number-" + rnDetails1.RNLicense_Number + " with name as " + rnDetails1.First_Name + " " + rnDetails1.Last_Name + "." _
                                                          + " Please contact DODD Admin for further Info")
                                End If
                            End If
                        Else
                            roles.AddErrorMessage("You are not mapped to the System with this credentials. Please contact RN instructor or DODD Admin for Access")
                        End If
                    End Using
                End If
            Catch ex As Exception
                Me.LogError("Error occured when inserting data in user rn mapping", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured when inserting data in user rn mapping", True, False))
                Throw
            End Try
            Return roles
        End Function
        Public Function CheckSecetaryMapping(userId As Integer) As Boolean Implements IMAISQueries.CheckSecetaryMapping
            Dim secetaryExists As Boolean = False
            Try
                If (userId > 0) Then
                    Using context As New MAISContext
                        Dim secetaryCount = (From secetaryDetail In context.RN_Secretary_Association
                                                          Join user In context.User_Mapping On secetaryDetail.User_Mapping_Sid Equals user.User_Mapping_Sid
                                                          Where user.UserID = userId And secetaryDetail.Active_Flg = True Select secetaryDetail).Count()
                        If (secetaryCount > 0) Then
                            secetaryExists = True
                        End If
                    End Using
                End If
            Catch ex As Exception
                Me.LogError("Error occured fetching the sectary existing", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured fetching the sectary existing", True, False))
                Throw
            End Try
            Return secetaryExists
        End Function

        Public Function CheckRNMapping(userId As Integer) As Objects.RN_Mapping Implements IMAISQueries.CheckRNMapping
            Dim rnExists As New Objects.RN_Mapping
            Try
                If (userId > 0) Then
                    Using context As New MAISContext
                        rnExists = (From rnDetail In context.User_RN_Mapping
                                                   Join rn In context.RNs On rn.RN_Sid Equals rnDetail.RN_Sid
                                                   Where rnDetail.UserID = userId
                                                   Select New Objects.RN_Mapping() With {
                                                     .RNLicenseNumber = rn.RNLicense_Number,
                                                     .RN_Sid = rn.RN_Sid,
                                                     .Un_Map_Flg = rnDetail.Unmap_Flg
                                                     }).FirstOrDefault()
                    End Using
                End If
            Catch ex As Exception
                Me.LogError("Error occured fetching the rn existing", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured fetching the rn existing", True, False))
                Throw
            End Try
            Return rnExists
        End Function

        Public Function GetRoleCategoryLevelInfoByRoleCategoryLevelSid(RoleCategoryLevelSid As Integer) As Objects.RoleCategoryLevelDetailsObject Implements IMAISQueries.GetRoleCategoryLevelInfoByRoleCategoryLevelSid
            Dim retVal As New Objects.RoleCategoryLevelDetailsObject
            Try
                Using context As New MAISContext
                    retVal = (From c In context.Role_Category_Level_Xref
                              Where c.Role_Category_Level_Sid = RoleCategoryLevelSid
                              Select New Objects.RoleCategoryLevelDetailsObject With {
                                  .Role_Category_Level_Sid = c.Role_Category_Level_Sid,
                                  .Role_Sid = c.Role_Sid,
                                  .Role_Name = c.MAIS_Role.Role_Desc,
                                  .Category_Type_Sid = c.Category_Type_Sid,
                                  .Category_Type_Name = c.Category_Type.Category_Code}).FirstOrDefault


                End Using


            Catch ex As Exception
                Me.LogError("Error occured fetching the Category Level Info existing", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured fetching the Category Level Info existing", True, False))
            End Try
            Return retVal
        End Function
        Public Function SetCertStatusAndDates(ByVal rnflag As Boolean, ByVal RoleDDRNXrefSid As Integer, ByVal StatusSid As Integer, Optional startDate As Date = #12/12/1990#, Optional endDate As Date = #12/31/9999#) As ReturnObject(Of Long) Implements IMAISQueries.SetCertStatusAndDates
            Dim retVal As New ReturnObject(Of Long)(-1L)
            Try
                Using context As New MAISContext
                    Dim cert As Certification = (From c In context.Certifications Where c.Role_RN_DD_Personnel_Xref_Sid = RoleDDRNXrefSid Select c Order By c.Certification_End_Date Descending).FirstOrDefault
                    Dim certstat As Certification_Status = (From cs In context.Certification_Status Where cs.Certification_Sid = cert.Certification_Sid Select cs Order By cs.Status_End_Date Descending).FirstOrDefault
                    Dim certRole As Role_RN_DD_Personnel_Xref = (From r In context.Role_RN_DD_Personnel_Xref Where r.Role_RN_DD_Personnel_Xref_Sid = RoleDDRNXrefSid Select r).FirstOrDefault
                    Dim StatDesc As String = (From s In context.Certification_Status_Type Where s.Certification_Status_Type_Sid = StatusSid Select s.Certification_Status_Desc).FirstOrDefault
                    Dim RNDDPTSid As Integer = (From r In context.Role_RN_DD_Personnel_Xref Where r.Role_RN_DD_Personnel_Xref_Sid = RoleDDRNXrefSid Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault
                    'retVal = (From c In context.Role_Category_Level_Xref
                    '          Where c.Role_Category_Level_Sid = RoleCategoryLevelSid
                    '          Select New Objects.RoleCategoryLevelDetailsObject With {
                    '              .Role_Category_Level_Sid = c.Role_Category_Level_Sid,
                    '              .Role_Sid = c.Role_Sid,
                    '              .Role_Name = c.MAIS_Role.Role_Desc,
                    '              .Category_Type_Sid = c.Category_Type_Sid,
                    '              .Category_Type_Name = c.Category_Type.Category_Code}).FirstOrDefault
                    If certstat.Certification_Status_Type_Sid <> StatusSid And startDate = #12/12/1990# Then
                        'its status change, not date change
                        If certstat.Status_Start_Date <> Date.Today Then
                            certstat.Status_End_Date = DateAdd(DateInterval.DayOfYear, -1, Date.Today)
                            certstat.Active_Flg = False
                            Dim newcs As New Certification_Status
                            newcs.Certification_Status_Type_Sid = StatusSid 'should be cert status
                            newcs.Status_Start_Date = Date.Today
                            newcs.Status_End_Date = CDate("12/31/9999")
                            newcs.Active_Flg = True
                            newcs.Create_By = Me.UserID
                            newcs.Create_Date = DateTime.Now
                            newcs.Last_Update_By = Me.UserID
                            newcs.Last_Update_Date = DateTime.Now
                            newcs.Certification = cert
                            context.Certification_Status.Add(newcs)
                            If StatDesc = "Revoked" Or StatDesc = "Voluntary Withdrawal" Then
                                cert.Certification_End_Date = DateAdd(DateInterval.DayOfYear, -1, Date.Today)
                                cert.Last_Update_By = Me.UserID
                                cert.Last_Update_Date = Date.Today
                                certRole.Role_End_Date = DateAdd(DateInterval.DayOfYear, -1, Date.Today)
                                certRole.Last_Update_By = Me.UserID
                                certRole.Last_Update_Date = Date.Today
                            End If
                            context.SaveChanges()
                            retVal.ReturnValue = 0
                        Else
                            retVal.AddGeneralMessage("Error: Cannot change status if certificate start date=today. ")
                            retVal.ReturnValue = -1
                            Return retVal
                        End If
                    ElseIf startDate <> #12/12/1990# Then
                        If startDate = cert.Certification_Start_Date Then
                            retVal.AddGeneralMessage("Error: Start date was not changed. ")
                            retVal.ReturnValue = -1
                            Return retVal
                        End If
                        'start date change
                        Dim dType As String = ""
                        Dim BiggerDate As Date = #12/12/1990#
                        For Each crs As Person_Course_Xref In (From course In context.Person_Course_Xref Where course.Role_RN_DD_Personnel_Xref_Sid = RoleDDRNXrefSid Select course).ToList

                            For Each sed As Date In (From sess In context.Person_Course_Session_Xref Where sess.Person_Course_Xref_Sid = crs.Person_Course_Xref_Sid Select sess.Session.End_Date).ToList
                                If sed > BiggerDate Then
                                    BiggerDate = sed
                                    dType = "Session end date"
                                End If
                            Next
                        Next
                        For Each psvEndDate As Date In (From sv In context.Skill_Verification Where sv.RN_DD_Person_Type_Xref_Sid = RNDDPTSid Select sv.End_Date).ToList
                            If psvEndDate > BiggerDate Then
                                BiggerDate = psvEndDate
                                dType = "Skills end date"
                            End If
                        Next
                        If rnflag Then
                            Dim rnddsid As Integer = (From pi In context.RN_DD_Person_Type_Xref Where pi.RN_DD_Person_Type_Xref_Sid = RNDDPTSid Select pi.RN_DDPersonnel_Sid).FirstOrDefault
                            Dim doi As Date = (From r In context.RNs Where r.RN_Sid = rnddsid Select r.Date_Of_Original_Issuance).FirstOrDefault
                            If doi > BiggerDate Then
                                BiggerDate = doi
                                dType = "Original date of issuance"
                            End If
                        End If
                        'Dim spandate As Date = IIf(rnflag, DateAdd(DateInterval.Year, -2, endDate), DateAdd(DateInterval.Year, -1, endDate))
                        'If spandate > BiggerDate Then
                        '    BiggerDate = spandate
                        '    dType = "Certificate Span Date"
                        'End If
                        If startDate < BiggerDate Then
                            retVal.AddGeneralMessage("Error: Start date cannot be prior to " & dType & ": " & BiggerDate.ToShortDateString)
                            retVal.ReturnValue = -1
                            Return retVal
                        End If

                        cert.Certification_Start_Date = startDate
                        cert.Certification_End_Date = endDate
                        cert.Last_Update_By = Me.UserID
                        cert.Last_Update_Date = Date.Today
                        certRole.Role_Start_Date = startDate
                        certRole.Role_End_Date = endDate
                        certRole.Last_Update_By = Me.UserID
                        certRole.Last_Update_Date = Date.Today

                        retVal.AddGeneralMessage("Certification Changed")
                        context.SaveChanges()
                        retVal.ReturnValue = 0
                    End If

                End Using


            Catch ex As Exception
                Me.LogError("Error occured ", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured ", True, False))
            End Try
            Return retVal
        End Function

        Public Function GetCertificationStartDate(UserUnique As String, categoryLevelID As Integer) As Date Implements IMAISQueries.GetCertificationStartDate
            Dim retVal As New Date
            Try
                Using context As New MAISContext
                    Dim RNDDPersonTypexrefSid As Integer

                    'Test for the user
                    If (UserUnique.Contains("DD")) Then
                        RNDDPersonTypexrefSid = ((From r In context.RN_DD_Person_Type_Xref _
                                                                Join ddP In context.DDPersonnels On ddP.DDPersonnel_Sid Equals r.RN_DDPersonnel_Sid
                                                               Where ddP.DDPersonnel_Code = UserUnique
                                                               Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault)
                    Else
                        RNDDPersonTypexrefSid = ((From r In context.RN_DD_Person_Type_Xref _
                                                        Join RN In context.RNs On RN.RN_Sid Equals r.RN_DDPersonnel_Sid
                                                        Where RN.RNLicense_Number = UserUnique
                                                        Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault)
                    End If
                    '------------------------- end of Test for user -----------------------------------------

                    retVal = (From c In context.Certifications
                              Where c.Role_RN_DD_Personnel_Xref.RN_DD_Person_Type_Xref_Sid = RNDDPersonTypexrefSid And _
                              c.Role_RN_DD_Personnel_Xref.Role_Category_Level_Sid = categoryLevelID
                              Select c.Certification_Start_Date).FirstOrDefault

                End Using
                Return retVal
            Catch ex As Exception
                Me.LogError("Error occured fetching the Certification Start Date in query GetCertificationStartDate", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured fetching the Certification Start Date in query GetCertificationStartDate", True, False))
                Return retVal
            End Try
        End Function

        Public Function GetCertificationDate(UserUnique As String, CategoryLevelID As Integer) As Date Implements IMAISQueries.GetCertificationDate
            Dim retVal As New Date
            Try
                Using context As New MAISContext
                    Dim RNDDPersonTypexrefSid As Integer

                    'Test for the user
                    If (UserUnique.Contains("DD")) Then
                        RNDDPersonTypexrefSid = ((From r In context.RN_DD_Person_Type_Xref _
                                                                Join ddP In context.DDPersonnels On ddP.DDPersonnel_Sid Equals r.RN_DDPersonnel_Sid
                                                               Where ddP.DDPersonnel_Code = UserUnique
                                                               Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault)
                    Else
                        RNDDPersonTypexrefSid = ((From r In context.RN_DD_Person_Type_Xref _
                                                        Join RN In context.RNs On RN.RN_Sid Equals r.RN_DDPersonnel_Sid
                                                        Where RN.RNLicense_Number = UserUnique
                                                        Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault)
                    End If
                    '------------------------- end of Test for user -----------------------------------------

                    retVal = (From c In context.Certifications
                              Where c.Role_RN_DD_Personnel_Xref.RN_DD_Person_Type_Xref_Sid = RNDDPersonTypexrefSid And _
                              c.Role_RN_DD_Personnel_Xref.Role_Category_Level_Sid = CategoryLevelID
                              Select c.Certification_End_Date).FirstOrDefault

                End Using
                Return retVal
            Catch ex As Exception
                Me.LogError("Error occured fetching the Certification Date in query GetCertificationDate", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured fetching the Certification Date in query GetCertificationDate", True, False))
                Return retVal
            End Try
        End Function

        Public Function GetCertificationDateByCategoryID(UserUnique As String, CategoryID As Integer) As Date Implements IMAISQueries.GetCertificationDateByCategoryID
            Dim retVal As New Date
            Try
                Using context As New MAISContext
                    Dim RNDDPersonTypexrefSid As Integer

                    'Test for the user
                    If (UserUnique.Contains("DD")) Then
                        RNDDPersonTypexrefSid = ((From r In context.RN_DD_Person_Type_Xref _
                                                                Join ddP In context.DDPersonnels On ddP.DDPersonnel_Sid Equals r.RN_DDPersonnel_Sid
                                                               Where ddP.DDPersonnel_Code = UserUnique
                                                               Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault)
                    Else
                        RNDDPersonTypexrefSid = ((From r In context.RN_DD_Person_Type_Xref _
                                                        Join RN In context.RNs On RN.RN_Sid Equals r.RN_DDPersonnel_Sid
                                                        Where RN.RNLicense_Number = UserUnique
                                                        Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault)
                    End If
                    '------------------------- end of Test for user -----------------------------------------

                    retVal = (From c In context.Certifications
                              Where c.Role_RN_DD_Personnel_Xref.RN_DD_Person_Type_Xref_Sid = RNDDPersonTypexrefSid And _
                              c.Role_RN_DD_Personnel_Xref.Role_Category_Level_Xref.Category_Type_Sid = CategoryID
                              Select c.Certification_End_Date).FirstOrDefault

                End Using
                Return retVal
            Catch ex As Exception
                Me.LogError("Error occured fetching the Certification Date in query GetCertificationDate", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured fetching the Certification Date in query GetCertificationDate", True, False))
                Return retVal
            End Try
        End Function

        Public Function GetCertificationDateThatIsHighRoleProiorityByRNSID(RNs_Sid As Integer, ByVal StartDate As Date) As Date Implements IMAISQueries.GetCertificationDateThatIsHighRoleProiorityByRNSID
            Dim retVal As New Date
            Try
                Using context As New MAISContext
                    retVal = (From c In context.Certifications
                             Where c.Role_RN_DD_Personnel_Xref.RN_DD_Person_Type_Xref.RN_DDPersonnel_Sid = RNs_Sid And c.Active_Flg = True And
                             c.Certification_Start_Date <= StartDate And c.Certification_End_Date >= StartDate
                             Select c.Certification_End_Date).FirstOrDefault
                End Using

                '              AndAlso
                'c.Active_Flg = 1 AndAlso c.Certification_Start_Date <= StartDate AndAlso c.Certification_End_Date >= StartDate
                '             Order By c.Role_RN_DD_Personnel_Xref.Role_Category_Level_Xref.MAIS_Role.Role_Priority Descending
                Return retVal
            Catch ex As Exception
                Me.LogError("Error occured fetching the Certification Date in query GetCertificationDateThatIsHighRoleProiorityByRNSID", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured fetching the Certification Date in query GetCertificationDateThatIsHighRoleProiorityByRNSID", True, False))
                Return retVal
            End Try

        End Function

        Public Function GetCertificationMinStartDateByRNSID(RNs_Sid As Integer) As Date Implements IMAISQueries.GetCertificationMinStartDateByRNSID
            Dim retVal As New Date
            Try
                Using resource As New MAISContext
                    retVal = (From c In resource.Certifications
                             Where c.Role_RN_DD_Personnel_Xref.RN_DD_Person_Type_Xref.RN_DDPersonnel_Sid = RNs_Sid
                             Select c.Certification_Start_Date).Min


                End Using
                Return retVal

            Catch ex As Exception
                Me.LogError("Error occured fetching the Certification Start Date in query GetCertificationMinStartDateByRNSID", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured fetching the Certification Start Date in query GetCertificationMinStartDateByRNSID", True, False))
                Return retVal
            End Try
        End Function

        Public Function GetCertificationMinStartDateByDDPersonnelCode(DDPersonelCode As String) As Date Implements IMAISQueries.GetCertificationMinStartDateByDDPersonnelCode
            Dim retval As New Date
            Try
                Using resource As New MAISContext
                    Dim myDDPersonalCodeID As Integer
                    myDDPersonalCodeID = (From d In resource.DDPersonnels
                                          Where DDPersonelCode = DDPersonelCode
                                          Select d.DDPersonnel_Sid).FirstOrDefault

                    retval = (From c In resource.Certifications
                              Where c.Role_RN_DD_Personnel_Xref.RN_DD_Person_Type_Xref.RN_DDPersonnel_Sid = myDDPersonalCodeID
                              Select c.Certification_Start_Date).Min

                End Using
                Return retval
            Catch ex As Exception
                Me.LogError("Error occured fetching the Certification Start Date in query GetCertificationMinStartDateByDDPersonnelCode", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured fetching the Certification Start Date in query GetCertificationMinStartDateByDDPersonnelCode", True, False))
                Return retval
            End Try
        End Function

        Public Function GetRNsName(RNs_Sid As Integer) As String Implements IMAISQueries.GetRNsName
            Dim retVal As String = String.Empty
            Try
                Using resource As New MAISContext
                    retVal = (From r In resource.RNs
                              Where r.RN_Sid = RNs_Sid
                              Select r.First_Name + " " + r.Last_Name).FirstOrDefault

                End Using
                Return retVal

            Catch ex As Exception
                Me.LogError("Error occured fetching the RN's Name in query GetRNsName", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured fetching the RN's Name in query GetRNsName", True, False))
                Return retVal
            End Try
        End Function

        Public Function GetRNsLicenseNumber(RNs_Sid As Integer) As String Implements IMAISQueries.GetRNsLicenseNumber
            Dim retVal As String = String.Empty
            Try
                Using resource As New MAISContext
                    retVal = (From r In resource.RNs
                             Where r.RN_Sid = RNs_Sid
                             Select r.RNLicense_Number).FirstOrDefault

                End Using
                Return retVal
            Catch ex As Exception
                Me.LogError("Error occured fetching the RN's License Number in query GetRNsLicenseNumber", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured fetching the RN's License Number in query GetRNsLicenseNumber", True, False))
                Return retVal
            End Try
        End Function

        Public Function GetAllCountyCodes() As List(Of CountyDetails) Implements IMAISQueries.GetAllCountyCodes
            Dim counties As New List(Of CountyDetails)
            Try
                Using resource As New MAISContext
                    counties = (From r In resource.Counties Where r.County_Alias <> "missing" And r.County_Alias <> "ALL COUNTIES"
                             Select New Objects.CountyDetails() With {
                    .CountyAlias = r.County_Alias,
                    .CountyID = r.County_ID
                                 }).ToList()
                End Using
            Catch ex As Exception
                Me.LogError("Error occured fetching all counties", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured fetching all counties", True, False))
            End Try
            Return counties
        End Function

        Public Function GetAllStates() As List(Of StateDetails) Implements IMAISQueries.GetAllStates
            Dim address As New List(Of StateDetails)
            Try
                Using resource As New MAISContext
                    address = (From r In resource.States
                             Select New Objects.StateDetails() With {
                    .StateAbr = r.State_Abbr,
                    .StateID = r.State_ID
                                 }).ToList()

                End Using
            Catch ex As Exception
                Me.LogError("Error occured fetching all counties", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured fetching all counties", True, False))
            End Try
            Return address
        End Function

        Public Function GetCountyIDByCodes(countyCode As String) As Integer Implements IMAISQueries.GetCountyIDByCodes
            Dim countyIDs As Integer = 0
            Try
                Using resource As New MAISContext
                    countyIDs = (From r In resource.Counties Where r.County_Alias = countyCode Select r.County_ID).FirstOrDefault()
                End Using
            Catch ex As Exception
                Me.LogError("Error occured fetching countyID", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured fetching countyID", True, False))
            End Try
            Return countyIDs
        End Function

        Public Function GetStateIDByStates(StateAbr As String) As Object Implements IMAISQueries.GetStateIDByStates
            Dim stateIDs As Integer = 0
            Try
                Using resource As New MAISContext
                    stateIDs = (From r In resource.States Where r.State_Abbr = StateAbr Select r.State_ID).FirstOrDefault()
                End Using
            Catch ex As Exception
                Me.LogError("Error occured fetching countyID", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured fetching countyID", True, False))
            End Try
            Return stateIDs
        End Function

        Public Function CheckTheMandatoryFields(rnLicenseNumber As String) As Integer Implements IMAISQueries.CheckTheMandatoryFields
            Dim _count As Integer = 0
            Try
                Using resource As New MAISContext
                    Dim varaible As DateTime = Convert.ToDateTime("12/31/9999")
                    Dim _currentSupervisor As Integer = 0
                    Dim _currentWorkLocation As Integer = 0
                    Dim _addressExists As Integer = (From add In resource.RN_DD_Person_Type_Address_Xref
                                          Join rnType In resource.RN_DD_Person_Type_Xref On rnType.RN_DD_Person_Type_Xref_Sid Equals add.RN_DD_Person_Type_Xref_Sid
                                          Join rn In resource.RNs On rn.RN_Sid Equals rnType.RN_DDPersonnel_Sid
                                          Where rn.Active_Flg = True And rn.RNLicense_Number = rnLicenseNumber And add.Address_Type_Sid = 2).Count
                    Dim _phoneExists As Integer = (From add In resource.RN_DD_Person_Type_Phone_Xref
                                          Join rnType In resource.RN_DD_Person_Type_Xref On rnType.RN_DD_Person_Type_Xref_Sid Equals add.RN_DD_Person_Type_Xref_Sid
                                          Join rn In resource.RNs On rn.RN_Sid Equals rnType.RN_DDPersonnel_Sid
                                          Where rn.Active_Flg = True And rn.RNLicense_Number = rnLicenseNumber).Count
                    Dim _emailExists As Integer = (From add In resource.RN_DD_Person_Type_Phone_Xref
                                          Join rnType In resource.RN_DD_Person_Type_Xref On rnType.RN_DD_Person_Type_Xref_Sid Equals add.RN_DD_Person_Type_Xref_Sid
                                          Join rn In resource.RNs On rn.RN_Sid Equals rnType.RN_DDPersonnel_Sid
                                          Where rn.Active_Flg = True And rn.RNLicense_Number = rnLicenseNumber).Count
                    Dim _currentEmployer As Integer = (From add In resource.Employer_RN_DD_Person_Type_Xref
                                          Join rnType In resource.RN_DD_Person_Type_Xref On rnType.RN_DD_Person_Type_Xref_Sid Equals add.RN_DD_Person_Type_Xref_Sid
                                          Join rn In resource.RNs On rn.RN_Sid Equals rnType.RN_DDPersonnel_Sid
                                          Where rn.Active_Flg = True And rn.RNLicense_Number = rnLicenseNumber And add.Employment_End_Date = varaible).Count
                    Dim _currentSupervisorDate As DateTime = (From add In resource.Employer_RN_DD_Person_Type_Xref
                                          Join rnType In resource.RN_DD_Person_Type_Xref On rnType.RN_DD_Person_Type_Xref_Sid Equals add.RN_DD_Person_Type_Xref_Sid
                                          Join rn In resource.RNs On rn.RN_Sid Equals rnType.RN_DDPersonnel_Sid
                                          Where rn.Active_Flg = True And rn.RNLicense_Number = rnLicenseNumber Order By add.Last_Update_By Descending Select add.Supervisor_End_date).FirstOrDefault
                    If (_currentSupervisorDate = varaible) Then
                        _currentSupervisor = 1
                    End If
                    Dim _currentWorkLocationDate As DateTime = (From add In resource.Employer_RN_DD_Person_Type_Address_Xref
                                          Join rnType In resource.RN_DD_Person_Type_Xref On rnType.RN_DD_Person_Type_Xref_Sid Equals add.RN_DD_Person_Type_Xref_Sid
                                          Join rn In resource.RNs On rn.RN_Sid Equals rnType.RN_DDPersonnel_Sid
                                          Where rn.Active_Flg = True And rn.RNLicense_Number = rnLicenseNumber And add.Address_Type_Sid = 4 Order By add.Last_Update_by Descending Select add.Agency_Work_Location_End_Date).FirstOrDefault
                    If (_currentWorkLocationDate = varaible) Then
                        _currentWorkLocation = 1
                    End If
                    Dim _currentAgencyAddress As Integer = (From add In resource.Employer_RN_DD_Person_Type_Address_Xref
                                          Join rnType In resource.RN_DD_Person_Type_Xref On rnType.RN_DD_Person_Type_Xref_Sid Equals add.RN_DD_Person_Type_Xref_Sid
                                          Join rn In resource.RNs On rn.RN_Sid Equals rnType.RN_DDPersonnel_Sid
                                          Where rn.Active_Flg = True And rn.RNLicense_Number = rnLicenseNumber And add.Address_Type_Sid = 3).Count
                    Dim _currentAgencyPhone As Integer = (From add In resource.Employer_RN_DD_Person_Type_Phone_Xref
                                          Join rnType In resource.RN_DD_Person_Type_Xref On rnType.RN_DD_Person_Type_Xref_Sid Equals add.RN_DD_Person_Type_Xref_Sid
                                          Join rn In resource.RNs On rn.RN_Sid Equals rnType.RN_DDPersonnel_Sid
                                          Where rn.Active_Flg = True And rn.RNLicense_Number = rnLicenseNumber And add.Contact_Type_Sid = 4).Count
                    Dim _currentWorkPhone As Integer = (From add In resource.Employer_RN_DD_Person_Type_Phone_Xref
                                         Join rnType In resource.RN_DD_Person_Type_Xref On rnType.RN_DD_Person_Type_Xref_Sid Equals add.RN_DD_Person_Type_Xref_Sid
                                         Join rn In resource.RNs On rn.RN_Sid Equals rnType.RN_DDPersonnel_Sid
                                         Where rn.Active_Flg = True And rn.RNLicense_Number = rnLicenseNumber And add.Contact_Type_Sid = 5).Count
                    Dim _currentAgencyEmail As Integer = (From add In resource.Employer_RN_DD_Person_Type_Email_Xref
                                          Join rnType In resource.RN_DD_Person_Type_Xref On rnType.RN_DD_Person_Type_Xref_Sid Equals add.RN_DD_Person_Type_Xref_Sid
                                          Join rn In resource.RNs On rn.RN_Sid Equals rnType.RN_DDPersonnel_Sid
                                          Where rn.Active_Flg = True And rn.RNLicense_Number = rnLicenseNumber And add.Contact_Type_Sid = 4).Count
                    Dim _currentWorkEmail As Integer = (From add In resource.Employer_RN_DD_Person_Type_Email_Xref
                                         Join rnType In resource.RN_DD_Person_Type_Xref On rnType.RN_DD_Person_Type_Xref_Sid Equals add.RN_DD_Person_Type_Xref_Sid
                                         Join rn In resource.RNs On rn.RN_Sid Equals rnType.RN_DDPersonnel_Sid
                                         Where rn.Active_Flg = True And rn.RNLicense_Number = rnLicenseNumber And add.Contact_Type_Sid = 5).Count
                    If (_addressExists = 0 Or _phoneExists = 0 Or _emailExists = 0 Or _currentEmployer = 0 Or _currentSupervisor = 0 Or _currentWorkLocation = 0 Or _currentAgencyAddress = 0 Or _currentAgencyPhone = 0 _
                       Or _currentWorkPhone = 0 Or _currentAgencyEmail = 0 Or _currentWorkEmail = 0) Then
                        _count = 0
                    Else
                        _count = 1
                    End If
                End Using
            Catch ex As Exception
                Me.LogError("Error occured when minimum requirements for RN", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured when minimum requirements for RN", True, False))
            End Try
            Return _count
        End Function

        Public Function GetAppIDByRNLicenseNumber(rnLicenseNumber As String) As Integer Implements IMAISQueries.GetAppIDByRNLicenseNumber
            Dim appID As Integer = 0
            Try
                Using resource As New MAISContext
                    appID = (From app In resource.Applications
                                Join rn In resource.RN_Application On app.Application_Sid Equals rn.Application_Sid
                                Join appStatus In resource.Application_Status_Type On app.Application_Status_Type_Sid Equals appStatus.Application_Status_Type_Sid
                                Where rn.RNLicense_Number = rnLicenseNumber And appStatus.Application_Status_Code = "Pending" Select app.Application_Sid).FirstOrDefault
                End Using
            Catch ex As Exception
                Me.LogError("Error occured when fetching appID for RN", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured when fetching appID for RN", True, False))
            End Try
            Return appID
        End Function

        Public Function GetCertificateExpirationTotals(RoleLevelCategory As Integer) As List(Of CertificateExpirationTotals) Implements IMAISQueries.GetCertificateExpirationTotals
            Dim retval As New List(Of Objects.CertificateExpirationTotals)

            Dim SearchList As New List(Of Data.Role_Category_Level_Xref)
            Try
                Using context As New MAISContext
                    Select Case RoleLevelCategory
                        Case 6
                            SearchList = (From cl In context.Role_Category_Level_Xref
                                 Where cl.Level_Type_Sid = 2 Or cl.Role_Category_Level_Sid = 3 Or cl.Role_Sid = 7
                                 Select cl).ToList
                        Case 5, 4
                            SearchList = (From cl In context.Role_Category_Level_Xref
                                          Where cl.Role_Sid = 2 Or cl.Role_Sid = 3 Or cl.Role_Sid = 7
                                          Select cl).ToList
                    End Select

                    For Each sl In SearchList
                        Dim Cert As List(Of Certification)
                        'Cert = (From c In context.Certifications
                        '        Join cs In context.Certification_Status On c.Certification_Sid Equals cs.Certification_Sid
                        '        Where cs.Certification_Status_Type_Sid = 1 And c.Role_RN_DD_Personnel_Xref.Role_Category_Level_Sid = sl.Role_Category_Level_Sid
                        '        Select c).ToList

                        Cert = (From c In context.Certifications
                                Join cs In context.Certification_Status On c.Certification_Sid Equals cs.Certification_Sid
                            Join mRP In context.Role_RN_DD_Personnel_Xref On mRP.Role_RN_DD_Personnel_Xref_Sid Equals c.Role_RN_DD_Personnel_Xref_Sid
                                Where cs.Certification_Status_Type_Sid = 1 And c.Role_RN_DD_Personnel_Xref.Role_Category_Level_Sid = sl.Role_Category_Level_Sid And _
                                c.Certification_End_Date = (From RWC In context.Certifications
                                                       Join csRWC In context.Certification_Status On csRWC.Certification_Sid Equals RWC.Certification_Sid
                                                       Where RWC.Role_RN_DD_Personnel_Xref_Sid = mRP.Role_RN_DD_Personnel_Xref_Sid And csRWC.Certification_Status_Type_Sid = 1 And RWC.Role_RN_DD_Personnel_Xref.Role_Category_Level_Sid = sl.Role_Category_Level_Sid
                                                       Group By RWC.Role_RN_DD_Personnel_Xref.Role_RN_DD_Personnel_Xref_Sid Into Latestsid = Group
                                                       Select Latestsid.Max(Function(l) l.RWC.Certification_End_Date)).FirstOrDefault
                                Select c).ToList



                        Dim t30 As Integer = 0
                        Dim t60 As Integer = 0
                        Dim t90 As Integer = 0
                        Dim t180 As Integer = 0



                        t30 = (From mC In Cert
                               Where mC.Certification_End_Date.AddDays(-30) <= Today
                               Select mC).Count
                        t60 = (From mC In Cert
                               Where mC.Certification_End_Date.AddDays(-60) <= Today
                               Select mC).Count
                        t60 = t60 - t30
                        t90 = (From mC In Cert
                               Where mC.Certification_End_Date.AddDays(-90) <= Today
                               Select mC).Count
                        t90 = t90 - t60 - t30
                        t180 = (From mC In Cert
                               Where mC.Certification_End_Date.AddDays(-180) <= Today
                               Select mC).Count
                        t180 = t180 - t90 - t60 - t30
                        Dim nList As New Objects.CertificateExpirationTotals
                        With nList
                            .Role_Category_Level_Sid = sl.Role_Category_Level_Sid

                            Select Case sl.Role_Category_Level_Sid
                                Case 15, 16, 17
                                    .Role = sl.Category_Type.Category_Desc
                                Case Else
                                    .Role = sl.MAIS_Role.Role_Desc
                            End Select
                            .Level = sl.Level_Type.Level_Desc
                            .Category = sl.Category_Type.Category_Desc
                            .Exp30Days = t30
                            .Exp60Days = t60
                            .Exp90Days = t90
                            .Exp180Days = t180
                        End With
                        retval.Add(nList)
                    Next

                End Using

            Catch ex As Exception
                Me.LogError("Error occured when fetching Certificate Expiration Totals", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured when fetching Certificate Expiration Totals", True, False))
            End Try

            Return retval


        End Function

        Public Function GetRNEmailAddressUsingRNsid(rnsidorrnsecetaryassociationID As Integer, flag As Integer) As String Implements IMAISQueries.GetRNEmailAddressUsingRNsid
            Dim retval As String = String.Empty
            Try
                Using resource As New MAISContext
                    If (flag = 1) Then
                        retval = (From rn In resource.RNs
                                  Join rnRef In resource.RN_DD_Person_Type_Xref On rnRef.RN_DDPersonnel_Sid Equals rn.RN_Sid
                                  Join rnEmailAddress In resource.RN_DD_Person_Type_Email_Xref On rnEmailAddress.RN_DD_Person_Type_Xref_Sid Equals rnRef.RN_DD_Person_Type_Xref_Sid
                                  Join email In resource.Email1 On email.Email_SID Equals rnEmailAddress.Email_Sid
                                  Where rn.RN_Sid = rnsidorrnsecetaryassociationID Select email.Email_Address).FirstOrDefault
                    Else
                        retval = (From secetary In resource.RN_Secretary_Association
                                  Join rn In resource.RNs On secetary.RN_Sid Equals rn.RN_Sid
                                  Join rnRef In resource.RN_DD_Person_Type_Xref On rnRef.RN_DDPersonnel_Sid Equals rn.RN_Sid
                                  Join rnEmailAddress In resource.RN_DD_Person_Type_Email_Xref On rnEmailAddress.RN_DD_Person_Type_Xref_Sid Equals rnRef.RN_DD_Person_Type_Xref_Sid
                                  Join email In resource.Email1 On email.Email_SID Equals rnEmailAddress.Email_Sid
                                  Where secetary.RN_Secretary_Association_Sid = rnsidorrnsecetaryassociationID Select email.Email_Address).FirstOrDefault

                    End If
                End Using
            Catch ex As Exception
                Me.LogError("Error occured when fetching Rn email address", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured when fetching Rn email address", True, False))
            End Try
            Return retval
        End Function

        Public Function GetCourseInformationByCertificationID(CertificationID As Integer) As CourseDetails Implements IMAISQueries.GetCourseInformationByCertificationID
            Dim retVal As New Objects.CourseDetails
            Dim Cdetail As New Objects.CertificationDetails
            Try
                Using resource As New MAISContext
                    Cdetail = (From c In resource.Certifications _
                                Join stc In resource.Certification_Status On stc.Certification_Sid Equals c.Certification_Sid _
                              Join cs In resource.Person_Course_Xref On cs.Role_RN_DD_Personnel_Xref_Sid Equals c.Role_RN_DD_Personnel_Xref_Sid _
                              Join css In resource.Person_Course_Session_Xref On css.Person_Course_Xref_Sid Equals cs.Person_Course_Xref_Sid _
                              Where c.Certification_Sid = CertificationID
                              Select New Objects.CertificationDetails With {
                                  .CertificateID = c.Certification_Sid,
                                  .CertificateStatus = stc.Certification_Status_Type.Certification_Status_Desc,
                                  .Course_SID = cs.Course_Sid,
                                  .Session_SID = css.Session_Sid}).FirstOrDefault

                    If Cdetail IsNot Nothing Then
                        Dim C = (From a In resource.Courses Where a.Course_sid = Cdetail.Course_SID Select a).FirstOrDefault
                        Dim hCourse As New Objects.CourseDetails
                        hCourse.Course_Sid = C.Course_sid
                        hCourse.RN_Sid = C.RN_Sid
                        hCourse.InstructorName = (From rnName In resource.RNs Where rnName.RN_Sid = C.RN_Sid Select (rnName.First_Name & " " & rnName.Last_Name)).FirstOrDefault
                        hCourse.Role_Calegory_Level_Sid = C.Role_Category_Level_Sid


                        hCourse.StartDate = C.Start_Date
                        hCourse.EndDate = C.End_Date
                        hCourse.OBNApprovalNumber = C.OBN_Course_Number
                        hCourse.CategoryACEs = C.Category_A_CEs
                        hCourse.TotalCEs = C.Total_CEs
                        hCourse.CourseDescription = C.Course_Description
                        hCourse.Level = (From l In resource.Role_Category_Level_Xref Where l.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select l.Level_Type_Sid).FirstOrDefault
                        hCourse.Category = (From CL In resource.Role_Category_Level_Xref Where CL.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select CL.Category_Type_Sid).FirstOrDefault

                        Dim CourseSessionList As New List(Of Objects.SessionAddressInformation)
                        For Each S As Data.Session In (From cs In resource.Sessions Where cs.Course_Sid = Cdetail.Course_SID)
                            Dim CourseSession As New Objects.SessionAddressInformation
                            CourseSession.Session_Sid = S.Session_Sid
                            CourseSession.Course_SID = S.Course_Sid
                            CourseSession.Session_Start_Date = S.Start_Date
                            CourseSession.Session_End_Date = S.End_Date
                            CourseSession.Sponsor = S.Sponsor
                            CourseSession.Location_Name = S.Location_Name
                            CourseSession.Total_CEs = S.Total_CEs
                            CourseSession.Public_Access_Flg = S.Public_Access_Flg
                            Dim sal As New Data.Session_Address_Xref
                            sal = (From dsal In resource.Session_Address_Xref Where dsal.Session_Sid = S.Session_Sid Select dsal).FirstOrDefault
                            'Dim sad As New Data.Address
                            'sad = (From dsad In context.Addresses Where dsad.Address_Sid = sal.Address_Sid Select dsad).FirstOrDefault
                            Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", sal.Address_Sid)
                            Dim sad As MAIS.Data.Address_Lookup_And_Insert_Result = resource.Address_Lookup_And_Insert(parameter, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).FirstOrDefault


                            Dim SessonAddress As New Objects.SessionAddress
                            With SessonAddress
                                .Address_Sid = sal.Address_Sid
                                .Address_Line1 = sad.Address_Line1
                                .Address_Line2 = sad.Address_Line2
                                .City = sad.City
                                .State = sad.State_Abbr
                                If sad.Zip.Length > 5 Then
                                    .Zip_Code = Mid(sad.Zip, 1, 5)
                                    .Zip_Code_Plus4 = Mid(6, 4)
                                Else
                                    .Zip_Code = sad.Zip
                                End If
                                '.Zip_Code = sad.Zip_Code
                                '.Zip_Code_Plus4 = sad.Zip_Code_Plus4
                                .County = sad.County_Desc
                                .CountyID = sad.CountyID
                                .Session_Address_Xref_Sid = sal.Session_Address_Xref_Sid
                                .Session_Sid = sal.Session_Sid
                                .Address_Type_Sid = sal.Address_Type_Sid
                            End With
                            CourseSession.SessionAddressInfo = SessonAddress

                            Dim mSessionInfoList As New List(Of Objects.SessionInformationDetails)
                            For Each SInfo As Data.Session_Information In (From dSI In resource.Session_Information Where dSI.Session_Sid = S.Session_Sid Select dSI)
                                Dim mSessionInfo As New Objects.SessionInformationDetails
                                With mSessionInfo
                                    .Session_Sid = SInfo.Session_Sid
                                    .Session_Information_SID = SInfo.Session_Informationl_Sid
                                    .Session_Date = SInfo.Session_Date
                                    .Total_CEs = SInfo.Total_CEs
                                End With
                                mSessionInfoList.Add(mSessionInfo)

                            Next
                            CourseSession.SessionInformationDetailsList = mSessionInfoList

                            CourseSessionList.Add(CourseSession)
                            hCourse.SessionDetailList = CourseSessionList

                        Next
                        If hCourse IsNot Nothing Then
                            retVal = hCourse
                        End If

                    End If


                End Using

            Catch ex As Exception
                Me.LogError("Error occured when fetching Training Session form GetCourseInformationByCertificationID. ", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured when fetching Training Session form GetCourseInformationByCertificationID. ", True, False))
            End Try
            Return retVal
        End Function

        Public Function GetSessionCourseInfoDetailsBySesssionID(SessionID As Integer) As List(Of SessionCourseInfoDetails) Implements IMAISQueries.GetSessionCourseInfoDetailsBySesssionID
            Dim retVal As New List(Of Objects.SessionCourseInfoDetails)
            Try
                Using context As New MAISContext
                    retVal = (From c In context.Courses _
                        Join s In context.Sessions On s.Course_Sid Equals c.Course_sid _
                        Join r In context.RNs On r.RN_Sid Equals c.RN_Sid _
                        Where s.Session_Sid = SessionID
                        Select New Objects.SessionCourseInfoDetails With {
                            .RN_Sid = r.RN_Sid,
                            .RN_Name = (r.First_Name & " " & r.Last_Name),
                            .Course_Sid = c.Course_sid,
                            .Session_sID = s.Session_Sid,
                            .CourseNumber = c.OBN_Course_Number,
                            .CourseDescription = c.Course_Description,
                            .SessionLocation = s.Location_Name,
                            .StartDate = s.Start_Date,
                            .EndDate = s.End_Date,
                            .TotalCEs = c.Total_CEs}).ToList
                End Using

            Catch ex As Exception
                Me.LogError("Error occured when fetching Training Session form GetSessionCourseInfoDetailsBySesssionID. ", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured when fetching Training Session form GetSessionCourseInfoDetailsBySesssionID. ", True, False))
            End Try
            Return retVal
        End Function

        Public Function UpdateSessionCourseInfoSession(newSessionID As Integer, RoleRNDDPersonelXrefSID As Integer, CertificationID As Integer) As Boolean Implements IMAISQueries.UpdateSessionCourseInfoSession
            Dim retObj As Boolean = False
            Try
                Using resource As New MAISContext
                    Dim PCXSid As Integer
                    PCXSid = (From c In resource.Certifications _
                                Join stc In resource.Certification_Status On stc.Certification_Sid Equals c.Certification_Sid _
                              Join cs In resource.Person_Course_Xref On cs.Role_RN_DD_Personnel_Xref_Sid Equals c.Role_RN_DD_Personnel_Xref_Sid _
                              Join css In resource.Person_Course_Session_Xref On css.Person_Course_Xref_Sid Equals cs.Person_Course_Xref_Sid _
                              Where c.Certification_Sid = CertificationID And cs.Role_RN_DD_Personnel_Xref_Sid = RoleRNDDPersonelXrefSID
                              Select cs.Person_Course_Xref_Sid).FirstOrDefault
                    Dim updataPersonSession = (From cs In resource.Person_Course_Session_Xref _
                                               Where cs.Person_Course_Xref_Sid = PCXSid
                                               Select cs).FirstOrDefault
                    With updataPersonSession
                        .Session_Sid = newSessionID
                        .Last_Update_By = UserID
                        .Last_Update_Date = Now()
                    End With

                    retObj = resource.SaveChanges


                End Using

            Catch ex As Exception
                Me.LogError("Error occured when updating Person Cournse Session form UpdateSessionCourseInfoSession. ", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured when updating courns Sessiong form UpdateSessionCourseInfoSession. ", True, False))
            End Try
            Return retObj
        End Function

        Public Function GetCurrnetSessionWithCertificationID(CertificationID As Integer) As PersonCourseSession Implements IMAISQueries.GetCurrnetSessionWithCertificationID
            Dim retObj As New Objects.PersonCourseSession
            Try
                Using resource As New MAISContext
                    retObj = (From c In resource.Certifications _
                              Join p In resource.Person_Course_Xref On p.Role_RN_DD_Personnel_Xref_Sid Equals c.Role_RN_DD_Personnel_Xref_Sid
                              Join ps In resource.Person_Course_Session_Xref On ps.Person_Course_Xref_Sid Equals p.Person_Course_Xref_Sid
                              Where c.Certification_Sid = CertificationID
                              Select New Objects.PersonCourseSession With {
                                  .PersonCourseSessionXref = ps.Person_Course_Session_Xref1,
                                  .PersonCourseXrefSid = ps.Person_Course_Xref_Sid,
                                  .SessionSid = ps.Session_Sid}).FirstOrDefault


                End Using

            Catch ex As Exception
                Me.LogError("Error occured when fetching Person Cournse Session form GetCurrnetSessionWithCertificationID. ", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured when fetching Training Session form GetCurrnetSessionWithCertificationID. ", True, False))
            End Try
            Return retObj
        End Function

        Public Function UserSessionMatch(AppID As Integer, uniqueID As String, OldAppID As Integer) As Boolean Implements IMAISQueries.UserSessionMatch
            Dim retObj As Boolean = False
            Try
                Using resource As New MAISContext

                    Dim MyTestUniqueCode = (From c In resource.Applications _
                                        Where c.Application_Sid = AppID
                                        Select c.RN_DD_Unique_Code).FirstOrDefault

                    If Not (String.IsNullOrWhiteSpace(MyTestUniqueCode)) Then
                        If MyTestUniqueCode = uniqueID Then
                            retObj = True
                        Else
                            retObj = False
                        End If
                    Else
                        If uniqueID.Contains("RN") Then
                            If AppID = OldAppID Then
                                Return True
                            Else
                                Return False
                            End If

                        Else
                            MyTestUniqueCode = String.Empty
                            If MyTestUniqueCode = uniqueID Then
                                If AppID = OldAppID Then
                                    retObj = True
                                Else
                                    retObj = False
                                End If

                            Else
                                retObj = False
                            End If
                        End If
                    End If
                End Using

            Catch ex As Exception
                Me.LogError("Error occured when testing if appliction ID data equals unique ID UserSessionMatch. ", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured when testing if appliction ID data equals unique ID UserSessionMatch.", True, False))
            End Try
            Return retObj
        End Function


        Public Function ChangeRNLicenseNumber(newRNNumber As String, exisitngRNNumber As String) As String Implements IMAISQueries.ChangeRNLicenseNumber
            Dim retObj As String = String.Empty
            Try
                Using context As New MAISContext
                    Dim rninfo As New Data.RN
                    rninfo = (From r In context.RNs
                              Where r.RNLicense_Number = exisitngRNNumber).FirstOrDefault()
                    If (rninfo IsNot Nothing) Then
                        Dim rnexist = (From re In context.RNs
                              Where re.RNLicense_Number = newRNNumber).FirstOrDefault()
                        Dim tempApp = (From ap In context.RN_Application
                                       Join a In context.Applications On ap.Application_Sid Equals a.Application_Sid
                                    Where ap.RNLicense_Number = newRNNumber And a.Application_Status_Type_Sid = 1).FirstOrDefault() 'pending applicationa
                        If (rnexist IsNot Nothing) Then
                            retObj = "New RN license number exists in database and it is tied to another RN."
                        End If
                        If ((rnexist Is Nothing) AndAlso (tempApp IsNot Nothing)) Then
                            retObj = "There is a pending application with this new RN license number."
                        End If
                        If ((rnexist Is Nothing) AndAlso (tempApp Is Nothing)) Then
                            rninfo.RNLicense_Number = newRNNumber
                            rninfo.Last_Update_By = UserID
                            rninfo.Last_Updated_Date = Now
                            context.SaveChanges()
                        End If
                    End If
                End Using

            Catch ex As Exception
                Me.LogError("Error occured when updating RN license number. ", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error occured when updating RN license number.", True, False))
            End Try
            Return retObj
        End Function
    End Class
End Namespace
