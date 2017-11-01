Imports MAIS.Business.Services
'Imports MAIS.Business.Model.Enums
Imports MAIS.Business.Model
Imports MAIS.Business


Namespace Rules
    Public Class PageCompletionRules
        Private _maisAppID As Integer 'Model.MAISApplicationDetails
        Private _maisApplication As Model.ApplicationInformationDetails
        Private _RoleLeveCategoryInfo As Model.RoleCategoryLevelDetails
        Private _ExistingUserRoleID As Integer
        Private _uniqueCode As String
#Region "Private ReadOnly Preopeties"

        Private ReadOnly Property MAISApplication As Model.ApplicationInformationDetails
            Get
                If _maisApplication Is Nothing Then
                    Dim service As IApplicationDetailInformationService = StructureMap.ObjectFactory.GetInstance(Of IApplicationDetailInformationService)()
                    _maisApplication = service.GetApplicationInfromationByAppID(_maisAppID)
                End If
                Return _maisApplication
            End Get
        End Property

        Private ReadOnly Property RoleLeveCategoryInfo As Model.RoleCategoryLevelDetails
            Get
                If _RoleLeveCategoryInfo Is Nothing Then
                    Dim service As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
                    _RoleLeveCategoryInfo = service.GetRoleCategoryLevelInfoByRoleCategoryLevelSid(MAISApplication.RoleCategoryLevel_SID)

                End If
                Return _RoleLeveCategoryInfo
            End Get
        End Property
#End Region



        Public Function IsStartPageComplete() As Boolean
            If _maisAppID > 0 Then
                Return True
            End If
            Return False
        End Function
        Public Function IsPartialPersonalInformationPageComplete() As Boolean
            If _maisAppID > 0 Then
                Dim maisPersonalPage As IPersonalInformationService = StructureMap.ObjectFactory.GetInstance(Of IPersonalInformationService)()
                Dim exists As Integer = maisPersonalPage.GetPartialPersonalPageComplete(_maisAppID)
                If (exists > 0) Then
                    Return True
                End If
            End If
            Return False
        End Function
        Public Function IsPersonalInformationPageComplete() As Boolean
            If _maisAppID > 0 Then
                Dim maisPersonalPage As IPersonalInformationService = StructureMap.ObjectFactory.GetInstance(Of IPersonalInformationService)()
                Dim exists As Integer = maisPersonalPage.GetPersonalPageComplete(_maisAppID)
                If (exists > 0) Then
                    Return True
                End If
            End If
            Return False
        End Function
        Public Function IsEmployerInformationPageComplete() As Boolean
            Dim RetFlag As Boolean = False
            If _maisAppID > 0 Then
                Dim maisEmployerPage As IEmployerInformationService = StructureMap.ObjectFactory.GetInstance(Of IEmployerInformationService)()
                Dim exists As Integer = maisEmployerPage.GetEmployerPageComplete(_maisAppID, Me.MAISApplication.RNDDUnique_Code)
                If (exists > 0) Then
                    RetFlag = True
                End If
            End If
            Return RetFlag
        End Function
        Public Function IsWorkExperienceInformationPageComplete() As Boolean
            Dim RetFlag As Boolean = True
            If _maisAppID > 0 Then
                Dim wrkSrv As IWorkExperienceService = StructureMap.ObjectFactory.GetInstance(Of IWorkExperienceService)()
                If MAISApplication.RN_Flg Then
                    Dim expCount As Integer = wrkSrv.GetExperience(MAISApplication.RNDDUnique_Code, _maisAppID)
                    Dim RNDDExpFlg As RN_DD_Flags = wrkSrv.GetDDExperienceFlg(MAISApplication.RNDDUnique_Code, _maisAppID)
                    If ((expCount > 0) And (RNDDExpFlg.ChkDDFlg = True) And (RNDDExpFlg.ChkRNFlg = True)) Then
                        Select Case MAISApplication.ApplicationType_SID
                            Case Enums.ApplicationType.Initial
                                Select Case RoleLeveCategoryInfo.Role_Sid
                                    Case Enums.Mais_Roles.RNTrainer
                                        If expCount < 18 Then
                                            RetFlag = False
                                        End If
                                    Case Enums.Mais_Roles.Bed17
                                        If expCount < 18 Then
                                            RetFlag = False
                                        End If
                                End Select
                            Case Enums.ApplicationType.AddOn
                                Select Case RoleLeveCategoryInfo.Role_Sid
                                    Case Enums.Mais_Roles.RNInstructor
                                        If expCount < 60 Then
                                            RetFlag = False
                                        End If
                                    Case Enums.Mais_Roles.RNTrainer
                                        If expCount < 18 Then
                                            RetFlag = False
                                        End If
                                    Case Enums.Mais_Roles.Bed17
                                        If expCount < 18 Then
                                            RetFlag = False
                                        End If
                                End Select
                        End Select
                    Else
                        RetFlag = False
                    End If
                End If
            End If
            Return RetFlag
        End Function
        Public Function IsDocumentUploadPageComplete() As Boolean
            If _maisAppID > 0 Then
                Dim maisEmployerPage As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
                Dim exists As Integer = maisEmployerPage.GetDocumentUploadForpageComplete(_maisAppID)
                If (exists > 0) Then
                    Return True
                End If
            End If
            Return False
        End Function
        Public Function IsAttestationPageComplete() As Boolean
            If _maisAppID > 0 Then
                Dim maisEmployerPage As IRN_AttestationService = StructureMap.ObjectFactory.GetInstance(Of IRN_AttestationService)()
                Dim exists As Integer = maisEmployerPage.GetAttestationForpageComplete(_maisAppID)
                If (exists = 0) Then
                    Return True
                End If
            End If
            Return False
        End Function
        Public Function IsSummaryPageComplete() As Boolean
            Dim exists As Boolean = False
            If _maisAppID > 0 Then
                Dim maisEmployerPage As ISummaryService = StructureMap.ObjectFactory.GetInstance(Of ISummaryService)()
                exists = maisEmployerPage.GetApplicationStatusSummary(_maisAppID)
            End If
            Return exists
        End Function
        Public Function IsTrainingPageComplete() As Boolean
            Dim TraingService As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()
            Dim RetFlag As Boolean = False

            Select Case MAISApplication.ApplicationType_SID
                Case Enums.ApplicationType.Initial
                    Select Case RoleLeveCategoryInfo.Role_Sid
                        Case Enums.Mais_Roles.RNTrainer
                            If TraingService.GetTrainingPageTotalHrOfSession(Me.MAISApplication.Application_SID) >= 8.0 Then
                                RetFlag = True
                            End If
                        Case Enums.Mais_Roles.RNInstructor
                            RetFlag = True

                        Case Enums.Mais_Roles.RNMaster
                            RetFlag = True
                        Case Enums.Mais_Roles.QARN
                            RetFlag = True
                        Case Enums.Mais_Roles.Bed17
                            If TraingService.GetTrainingPageTotalHrOfSession(Me.MAISApplication.Application_SID) >= 4.0 Then
                                RetFlag = True
                            End If
                        Case Enums.Mais_Roles.DDPersonnel
                            If TraingService.GetTrainingPageTotalHrOfSession(Me.MAISApplication.Application_SID) >= 14.0 Then
                                RetFlag = True
                            End If
                    End Select
                Case Enums.ApplicationType.Renewal
                    Select Case RoleLeveCategoryInfo.Role_Sid
                        Case Enums.Mais_Roles.RNTrainer
                            If TraingService.GetTrainingPageTotalCEUs(Me.MAISApplication.Application_SID, Me.MAISApplication.RNDDUnique_Code) >= 4.0 Then
                                RetFlag = True
                            End If
                        Case Enums.Mais_Roles.RNInstructor
                            RetFlag = True
                        Case Enums.Mais_Roles.RNMaster
                            RetFlag = True
                        Case Enums.Mais_Roles.QARN
                            RetFlag = True
                        Case Enums.Mais_Roles.Bed17
                            If TraingService.GetTrainingPageTotalCEUs(Me.MAISApplication.Application_SID, Me.MAISApplication.RNDDUnique_Code) >= 2.0 Then
                                RetFlag = True
                            End If
                        Case Enums.Mais_Roles.DDPersonnel
                            Select Case Me.RoleLeveCategoryInfo.Category_Type_Sid
                                Case 1
                                    If TraingService.GetTrainingPageTotalCESs(Me.MAISApplication.Application_SID, Me.MAISApplication.RNDDUnique_Code, Me.RoleLeveCategoryInfo.Category_Type_Sid) >= 2 Then
                                        RetFlag = True
                                    End If
                                Case Else
                                    If TraingService.GetTrainingPageTotalCESs(Me.MAISApplication.Application_SID, Me.MAISApplication.RNDDUnique_Code, Me.RoleLeveCategoryInfo.Category_Type_Sid) >= 1 Then
                                        RetFlag = True
                                    End If

                            End Select



                    End Select


                Case Enums.ApplicationType.UpdateProfile
                    RetFlag = True

                Case Enums.ApplicationType.AddOn

                    Select Case _ExistingUserRoleID
                        Case Enums.RoleLevelCategory.RNTrainer_RLC
                            Select Case RoleLeveCategoryInfo.Role_Category_Level_Sid
                                Case Enums.RoleLevelCategory.QARN_RLC
                                    RetFlag = True
                                Case Enums.RoleLevelCategory.Bed17_RLC
                                    RetFlag = True
                                Case Enums.RoleLevelCategory.RNInstructor_RLC
                                    RetFlag = True
                                Case Else
                                    RetFlag = True
                            End Select

                        Case Enums.RoleLevelCategory.RNInstructor_RLC
                            RetFlag = True
                        Case Enums.RoleLevelCategory.RNMaster_RLC
                            RetFlag = True

                        Case Enums.RoleLevelCategory.QARN_RLC
                            Select Case RoleLeveCategoryInfo.Role_Category_Level_Sid

                                Case Enums.RoleLevelCategory.RNTrainer_RLC
                                    If TraingService.GetTrainingPageTotalHrOfSession(MAISApplication.Application_SID) >= 8.0 Then
                                        RetFlag = True
                                    End If

                                Case Enums.RoleLevelCategory.Bed17_RLC
                                    If TraingService.GetTrainingPageTotalHrOfSession(MAISApplication.Application_SID) >= 4.0 Then
                                        RetFlag = True
                                    End If
                                Case Else
                                    RetFlag = True
                            End Select

                        Case Enums.RoleLevelCategory.Bed17_RLC
                            Select Case RoleLeveCategoryInfo.Role_Sid
                                Case Enums.Mais_Roles.RNTrainer
                                    If TraingService.GetTrainingPageTotalHrOfSession(MAISApplication.Application_SID) >= 8.0 Then
                                        RetFlag = True
                                    End If
                                Case Else
                                    RetFlag = True
                            End Select
                        Case Enums.RoleLevelCategory.DDPersonnel_RLC
                            If TraingService.GetTrainingPageTotalHrOfSession(MAISApplication.Application_SID) >= 4.0 Then
                                RetFlag = True
                            End If
                    End Select

                    'Select Case RoleLeveCategoryInfo.Role_Sid
                    '    Case Enums.Mais_Roles.RNTrainer
                    '        Select Case _ExistingUserRoleID
                    '            Case Enums.RoleLevelCategory.QARN_RLC
                    '                RetFlag = True ' No Additial Requirements needed.
                    '            Case Enums.RoleLevelCategory.Bed17_RLC
                    '                RetFlag = True ' No Additial Requiremnets needed.

                    '        End Select
                    '        If TraingService.GetTrainingPageTotalHrOfSession(MAISApplication.Application_SID) >= 8.0 Then
                    '            RetFlag = True
                    '        End If

                    '    Case Enums.Mais_Roles.RNInstructor

                    '        RetFlag = True
                    '    Case Enums.Mais_Roles.RNMaster
                    '        RetFlag = True
                    '    Case Enums.Mais_Roles.QARN
                    '        RetFlag = True
                    '    Case Enums.Mais_Roles.Bed17

                    '        If TraingService.GetTrainingPageTotalHrOfSession(Me.MAISApplication.Application_SID) >= 4 Then
                    '            RetFlag = True
                    '        End If

                    '    Case Enums.Mais_Roles.DDPersonnel
                    '        If TraingService.GetTrainingPageTotalHrOfSession(Me.MAISApplication.Application_SID) >= 4 Then
                    '            RetFlag = True
                    '        End If
                    'End Select
                Case Else

            End Select
            'Dim exists As Integer = 0
            'If _maisApp > 0 Then
            '    Dim maisEmployerPage As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()
            '    exists = maisEmployerPage.GetTrainingPageHelper(_maisApp)
            '    If (exists > 0) Then
            '        Return True
            '    End If
            'End If
            'Return False
            Return RetFlag

        End Function

        Public Function IsSkillsPageComplete() As Boolean
            Dim SkillService As ISkillPageService = StructureMap.ObjectFactory.GetInstance(Of ISkillPageService)()
            Dim RetFlag As Boolean = False

            Select Case MAISApplication.ApplicationType_SID
                Case Enums.ApplicationType.Initial
                    RetFlag = SkillService.GetSkillVerificationPageCompletion(Me.MAISApplication.RNDDUnique_Code, Me.RoleLeveCategoryInfo.Category_Type_Sid, True, Me.MAISApplication.Application_SID)
                Case Enums.ApplicationType.Renewal
                    RetFlag = SkillService.GetSkillVerificationPageCompletion(Me.MAISApplication.RNDDUnique_Code, Me.RoleLeveCategoryInfo.Category_Type_Sid, False, Me.MAISApplication.Application_SID)
                Case Enums.ApplicationType.UpdateProfile
                    RetFlag = True
                Case Enums.ApplicationType.AddOn
                    RetFlag = SkillService.GetSkillVerificationPageCompletion(Me.MAISApplication.RNDDUnique_Code, Me.RoleLeveCategoryInfo.Category_Type_Sid, True, Me.MAISApplication.Application_SID)
            End Select
            Return RetFlag

        End Function
        Public Sub New(ByVal maisAppID As Integer, ByVal ExistingUserRoleID As Integer)
            _maisAppID = maisAppID
            _ExistingUserRoleID = ExistingUserRoleID
        End Sub

        Public Function IsMAISAttestationPageCompleted(ByVal RoleID As Integer, ByVal AppTypeID As Integer) As Boolean
            Dim returnVal As Boolean = False
            Dim maisService As IRN_AttestationService = StructureMap.ObjectFactory.GetInstance(Of IRN_AttestationService)()
            For Each returnApplicaitonQuestionModel In maisService.GetRN_AttestationQuestionForPage(RoleID, AppTypeID)
                If Not returnApplicaitonQuestionModel Is Nothing Then
                    Dim listOfAttestations = maisService.GetRn_AttestationPanelbyApplicaitonID_Attestation_Applicationtype_xrefSid(_maisAppID, returnApplicaitonQuestionModel.Attestation_SID)

                    ' RN_AttestationPanel = maisService.GetRn_AttestationPanelbyApplicaitonID_Attestation_Applicationtype_xrefSid(QuestionInApp.Application_SID, QuestionInApp.Attestation_ApplicationType_Xref_Sid)

                    Select Case listOfAttestations.YesNo
                        Case 0, 1
                            returnVal = True
                        Case Else
                            returnVal = False
                            Exit For
                    End Select
                Else
                    returnVal = False
                    Exit For
                End If

            Next
            'Test if the Applicant Initials are field in. 


            If returnVal = True Then

                returnVal = IsAttestationSigned()

            End If

            Return returnVal

        End Function
        Public Function IsAttestationPageCompletedYes(ByVal RoleID As Integer, ByVal AppTypeID As Integer) As Boolean
            Dim returnVal As Boolean = False
            Dim maisService As IRN_AttestationService = StructureMap.ObjectFactory.GetInstance(Of IRN_AttestationService)()
            For Each returnApplicaitonQuestionModel In maisService.GetRN_AttestationQuestionForPage(RoleID, AppTypeID)
                If Not returnApplicaitonQuestionModel Is Nothing Then
                    Dim listOfAttestations = maisService.GetRn_AttestationPanelbyApplicaitonID_Attestation_Applicationtype_xrefSid(_maisAppID, returnApplicaitonQuestionModel.Attestation_SID)

                    ' RN_AttestationPanel = maisService.GetRn_AttestationPanelbyApplicaitonID_Attestation_Applicationtype_xrefSid(QuestionInApp.Application_SID, QuestionInApp.Attestation_ApplicationType_Xref_Sid)

                    Select Case listOfAttestations.YesNo
                        Case 1
                            returnVal = True
                        Case Else
                            returnVal = False
                            Exit For
                    End Select
                Else
                    returnVal = False
                    Exit For
                End If

            Next
            'Test if the Applicant Initials are field in. 


            If returnVal = True Then

                returnVal = IsAttestationSigned()

            End If

            Return returnVal

        End Function

        Public Function IsAttestationSigned() As Boolean
            Dim retrnVal As Boolean = False
            Dim AppService As Services.IApplicationDetailInformationService = StructureMap.ObjectFactory.GetInstance(Of Services.IApplicationDetailInformationService)()
            Dim appInformationDetail As Model.ApplicationInformationDetails = AppService.GetApplicationInfromationByAppID(_maisAppID)

            If appInformationDetail.IsAdminFlag = False Then
                Select Case True
                    Case String.IsNullOrEmpty(appInformationDetail.Signature)
                        retrnVal = False
                    Case appInformationDetail.Signature Is Nothing
                        retrnVal = False
                    Case appInformationDetail.Signature.Length > 0
                        retrnVal = True
                End Select
                'retrnVal = (appInformationDetail.Signature)
            Else
                retrnVal = True
            End If


            Return retrnVal


        End Function
    End Class
End Namespace

