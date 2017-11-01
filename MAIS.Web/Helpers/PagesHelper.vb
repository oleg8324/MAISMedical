Imports System.ComponentModel
Imports MAIS.Business
Imports MAIS.Business.Model
Imports MAIS.Business.Rules
Imports MAIS.Business.Helpers
Imports MAIS.Business.Model.Enums

Public Class PagesHelper
    Private _appType As String
    Private Const _landingPage As String = "mais_home.aspx"
    Private Const _loginPage As String = "MAISLogin.aspx"
    Private Const _logoutPage As String = "MAISLogout.aspx"
    Private Const _existingCertpage As String = "UpdateExistingPage.aspx"
    Private Const _startPage As String = "StartPage.aspx"
    Private Const _personalInfo As String = "PersonalInformation.aspx"
    Private Const _employerInfo As String = "EmployerInformation.aspx"
    Private Const _workExperience As String = "WorkExperience.aspx"
    Private Const _trainingSkills As String = "TrainingSkills.aspx"
    Private Const _SkillsInfo As String = "Skills.aspx"
    Private Const _documentUpload As String = "DocumentUpload.aspx"
    Private Const _rnAttestion As String = "RNAttestation.aspx"
    Private Const _applicationSummary As String = "Summary.aspx"
    Private Const _notation As String = "Notation.aspx"
    Private Const _veiwCert As String = "ViewCertificate.aspx"
    Public Shared Function GetPageList() As List(Of PageModel)
        Dim pageList As New List(Of PageModel)
        If (SessionHelper.ApplicationType <> "Update Profile") Then
            With pageList
                .Add(New PageModel("Personal", _existingCertpage))
                .Add(New PageModel("Start Page", _startPage))
                .Add(New PageModel("Personal Information", _personalInfo))
                If (SessionHelper.RN_Flg) Then
                    If (SessionHelper.SelectedUserRole <> Enums.RoleLevelCategory.RNInstructor_RLC Or SessionHelper.SelectedUserRole <> Enums.RoleLevelCategory.RNMaster_RLC) Then
                        .Add(New PageModel("Current Employer Information", _employerInfo))
                    End If
                Else
                    .Add(New PageModel("Current Employer Information", _employerInfo))
                End If
                If (SessionHelper.RN_Flg) Then
                    If (SessionHelper.SelectedUserRole <> Enums.RoleLevelCategory.QARN_RLC And SessionHelper.MAISLevelUserRole <> Enums.RoleLevelCategory.QARN_RLC) Then
                        If (SessionHelper.SelectedUserRole = Enums.RoleLevelCategory.Bed17_RLC And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.Initial)) Then
                            .Add(New PageModel("Work Experience", _workExperience))
                        End If
                        If (SessionHelper.SelectedUserRole = Enums.RoleLevelCategory.RNTrainer_RLC And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.Initial)) Then
                            .Add(New PageModel("Work Experience", _workExperience))
                        End If
                        If (SessionHelper.SelectedUserRole = Enums.RoleLevelCategory.RNInstructor_RLC And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn)) Then
                            .Add(New PageModel("Work Experience", _workExperience))
                        End If
                        If (SessionHelper.SelectedUserRole = Enums.RoleLevelCategory.Bed17_RLC And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn) _
                            And SessionHelper.ExistingUserRole = Enums.RoleLevelCategory.QARN_RLC) Then
                            .Add(New PageModel("Work Experience", _workExperience))
                        End If
                    Else
                        If (SessionHelper.ExistingUserRole = 7) Then
                            If (SessionHelper.SelectedUserRole = Enums.RoleLevelCategory.RNTrainer_RLC And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn)) Then
                                .Add(New PageModel("Work Experience", _workExperience))
                            End If
                            If (SessionHelper.SelectedUserRole = Enums.RoleLevelCategory.Bed17_RLC And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn)) Then
                                .Add(New PageModel("Work Experience", _workExperience))
                            End If
                        End If
                    End If
                End If
                If (SessionHelper.RN_Flg) Then
                    If (SessionHelper.SelectedUserRole <> Enums.RoleLevelCategory.Bed17_RLC And SessionHelper.MAISLevelUserRole <> Enums.RoleLevelCategory.Bed17_RLC) Then
                        If ((SessionHelper.SelectedUserRole = Enums.RoleLevelCategory.RNTrainer_RLC)) Then
                            .Add(New PageModel("Training & CEUs", _trainingSkills))
                        End If
                        'If ((SessionHelper.SelectedUserRole = 7 And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn))) Then
                        '    .Add(New PageModel("Training & CEUs", _trainingSkills))
                        'End If
                    Else
                        If (SessionHelper.SelectedUserRole = Enums.RoleLevelCategory.Bed17_RLC And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.Initial)) Then
                            .Add(New PageModel("Training & CEUs", _trainingSkills))
                            '.Add(New PageModel("Skills", _SkillsInfo))
                        End If
                        If (SessionHelper.SelectedUserRole = Enums.RoleLevelCategory.Bed17_RLC And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.Renewal)) Then
                            .Add(New PageModel("Training & CEUs", _trainingSkills))
                        End If
                        If (SessionHelper.ExistingUserRole = Enums.RoleLevelCategory.QARN_RLC And SessionHelper.SelectedUserRole = Enums.RoleLevelCategory.Bed17_RLC _
                            And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn)) Then
                            .Add(New PageModel("Training & CEUs", _trainingSkills))
                        End If
                        If (SessionHelper.ExistingUserRole = 8) Then
                            If (SessionHelper.SelectedUserRole = Enums.RoleLevelCategory.RNTrainer_RLC And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn)) Then
                                .Add(New PageModel("Training & CEUs", _trainingSkills))
                            End If
                        End If
                    End If
                Else
                    .Add(New PageModel("Training & CEUs", _trainingSkills))
                    .Add(New PageModel("Skills Verification", _SkillsInfo))
                End If
                If (SessionHelper.RN_Flg) Then
                    If (SessionHelper.ApplicationType <> EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.Renewal)) Then
                        .Add(New PageModel("Upload Documents", _documentUpload))
                    End If
                End If
                If (SessionHelper.RN_Flg) Then
                    If (SessionHelper.SelectedUserRole <> Enums.RoleLevelCategory.QARN_RLC And SessionHelper.MAISLevelUserRole <> Enums.RoleLevelCategory.QARN_RLC) Then
                        If (SessionHelper.SelectedUserRole <> Enums.RoleLevelCategory.Bed17_RLC) Then
                            If (SessionHelper.SelectedUserRole <> Enums.RoleLevelCategory.RNMaster_RLC) Then
                                .Add(New PageModel("Attestation", _rnAttestion))
                            End If
                        Else
                            If (SessionHelper.ExistingUserRole = Enums.RoleLevelCategory.QARN_RLC And SessionHelper.SelectedUserRole = Enums.RoleLevelCategory.Bed17_RLC _
                           And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn)) Then
                                .Add(New PageModel("Attestation", _rnAttestion))
                            End If
                        End If
                        If (SessionHelper.SelectedUserRole = Enums.RoleLevelCategory.Bed17_RLC And SessionHelper.ApplicationType <> EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn)) Then
                            .Add(New PageModel("Attestation", _rnAttestion))
                        End If
                    End If
                Else
                    .Add(New PageModel("Attestation", _rnAttestion))
                End If
                .Add(New PageModel("Application Summary/Decision", _applicationSummary))
                If (SessionHelper.ExistingUserRole = Enums.RoleLevelCategory.QARN_RLC And SessionHelper.SelectedUserRole = Enums.RoleLevelCategory.Bed17_RLC _
                   And SessionHelper.ApplicationType = EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn)) Then
                    .Add(New PageModel("View/Print Certificate", _veiwCert))
                End If
                If ((SessionHelper.SelectedUserRole = Enums.RoleLevelCategory.Bed17_RLC And SessionHelper.ApplicationType <> EnumHelper.GetEnumDescription(Model.Enums.ApplicationType.AddOn))) Then
                    .Add(New PageModel("View/Print Certificate", _veiwCert))
                Else
                    If ((SessionHelper.SelectedUserRole <> Enums.RoleLevelCategory.RNMaster_RLC And SessionHelper.SelectedUserRole <> Enums.RoleLevelCategory.QARN_RLC And SessionHelper.SelectedUserRole <> Enums.RoleLevelCategory.Bed17_RLC)) Then
                        .Add(New PageModel("View/Print Certificate", _veiwCert))
                    End If
                End If
                .Add(New PageModel("Notation", _notation))
            End With
        Else
            With pageList
                .Add(New PageModel("Personal", _existingCertpage))
                .Add(New PageModel("Start Page", _startPage))
                .Add(New PageModel("Personal Information", _personalInfo))
                .Add(New PageModel("Current Employer Information", _employerInfo))
                .Add(New PageModel("Application Summary/Decision", _applicationSummary))
                .Add(New PageModel("View/Print Certificate", _veiwCert))
            End With
        End If
        Return pageList
    End Function
    Public Shared Function GetPage(ByVal currentPageAddress As String, ByVal diff As Integer) As String

        Dim pagesList As List(Of PageModel) = PagesHelper.GetPageList()

        If diff = 0 Then
            Return currentPageAddress
        End If

        For currPageIndex As Integer = 0 To pagesList.Count - 1
            If pagesList(currPageIndex).PageAddress.Equals(currentPageAddress) Then
                'If pagesList(currPageIndex).PageAddress.EndsWith(currentPageAddress) Then
                If currPageIndex + diff > 0 And currPageIndex + diff < pagesList.Count Then
                    Return pagesList(currPageIndex + diff).PageAddress
                End If
            End If
        Next

        Return _landingPage     ' Some error has occurred. Let's go to the landing page
    End Function
    Public Shared Function GetNextPage(ByVal thisPage As String) As String
        Return GetPage(thisPage, +1)
    End Function
    Public Shared Function GetPreviousPage(ByVal thisPage As String) As String
        Return GetPage(thisPage, -1)
    End Function
    Public Property ApplicationType As String
        Get
            Return _appType
        End Get
        Set(ByVal value As String)
            _appType = value
        End Set
    End Property
    Shared Function GetPageCompletionRules(p1 As Long) As Rules.PageCompletionRules
        Return New Rules.PageCompletionRules(p1, SessionHelper.ExistingUserRole)
        'Throw New NotImplementedException
    End Function
    Public Shared ReadOnly Property LandingPage As String
        Get
            Return _landingPage
        End Get
    End Property
    Public Shared ReadOnly Property LoginPage As String
        Get
            Return _loginPage
        End Get
    End Property
    Public Shared ReadOnly Property LogoutPage As String
        Get
            Return _logoutPage
        End Get
    End Property


    Public Shared Function LoginUserAllowedtoProcressApplication(ByVal UserLoginRole As Enums.RoleLevelCategory, ByVal AppRoleLevelCat As Enums.RoleLevelCategory) As Boolean
        Dim retValue As Boolean = False

        Select Case UserLoginRole
            Case Enums.RoleLevelCategory.QARN_RLC
                If AppRoleLevelCat = RoleLevelCategory.QARN_RLC Then
                    retValue = True
                End If
            Case Enums.RoleLevelCategory.Bed17_RLC
                Select Case AppRoleLevelCat
                    Case RoleLevelCategory.Bed17_RLC, RoleLevelCategory.QARN_RLC
                        retValue = True
                    Case Else
                        retValue = False
                End Select
            Case Enums.RoleLevelCategory.RNTrainer_RLC
                Select Case AppRoleLevelCat
                    Case RoleLevelCategory.QARN_RLC, RoleLevelCategory.DDPersonnel_RLC, RoleLevelCategory.DDPersonnel2_RLC, RoleLevelCategory.DDPersonnel3_RLC
                        retValue = True
                    Case Else
                        retValue = False
                End Select
            Case Enums.RoleLevelCategory.RNInstructor_RLC
                Select Case AppRoleLevelCat
                    Case RoleLevelCategory.RNMaster_RLC, RoleLevelCategory.RNInstructor_RLC
                        retValue = False
                    Case Else
                        retValue = True
                End Select
            Case Enums.RoleLevelCategory.RNMaster_RLC
                retValue = True
            Case Enums.RoleLevelCategory.DDPersonnel_RLC, Enums.RoleLevelCategory.DDPersonnel2_RLC, Enums.RoleLevelCategory.DDPersonnel3_RLC
                retValue = False
            Case Enums.RoleLevelCategory.Secretary_RLC
                retValue = True
            Case Else 'The person is a DODD Admin
                retValue = True
        End Select

        Return retValue
    End Function
End Class

