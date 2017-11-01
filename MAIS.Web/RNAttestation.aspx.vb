Imports System.Web.Script.Services
Imports MAIS.Business.Model
Imports MAIS.Business.Services
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business
Imports MAIS.Business.Model.Enums
Imports ODMRDDHelperClassLibrary
Imports ODMRDDHelperClassLibrary.ODMRDDServiceProvider



Public Class RNAttestion
    Inherits System.Web.UI.Page

    Private numberofQuestions As Integer
    Private AppTypeID As Enums.ApplicationType

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '#If DEBUG Then
        '        SessionHelper.MAISUserID = 22863
        '#End If


        Dim rnAttService As IRN_AttestationService = StructureMap.ObjectFactory.GetInstance(Of IRN_AttestationService)()
        numberofQuestions = 1

        AppTypeID = DirectCast([Enum].Parse(GetType(Enums.ApplicationType), SessionHelper.ApplicationType), Enums.ApplicationType)

        For Each returnAttestationQuestionModel As AttestationQuestions In rnAttService.GetRN_AttestationQuestionForPage(SessionHelper.SelectedUserRole, AppTypeID)
            If Not returnAttestationQuestionModel.AttestationDesc Is Nothing And returnAttestationQuestionModel.AttestationDesc.Length > 0 Then
                'Dim lblQuestion As New Label()
                'lblQuestion.Text = returnAttestationQuestionModel.AttestationDesc
                'lblQuestion.Attributes.Add("class", "leftAlign")

                Dim pnlQuestion As New Panel()
                'pnlQuestion.Controls.Add(lblQuestion)
                pnlQuestion.ID = "pnl" & numberofQuestions
                pnlQuestion.GroupingText = "Question " & numberofQuestions
                pnlQuestion.BackColor = Drawing.Color.White
                pnlQuestion.Width = "720"

                Dim offenseControl As RN_AttestationControl = LoadControl("UserControls\RN_AttestationControl.ascx")

                offenseControl.SetControlParameters(returnAttestationQuestionModel.ApplicationType_Sid, numberofQuestions, returnAttestationQuestionModel.Attestation_SID, returnAttestationQuestionModel.ApplicationType_Sid, returnAttestationQuestionModel.AttestationDesc)

                offenseControl.Initialize()

                pnlQuestion.Controls.Add(offenseControl)

                pnlAttestationMain.Controls.Add(pnlQuestion)


            End If
            numberofQuestions = numberofQuestions + 1

        Next
        TestButtons()

    End Sub

    Private Sub RNAttestion_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Dim maincontextctl As Control = Page.Master.FindControl("maincontent")

        For currentpanelnumber As Integer = 1 To numberofQuestions - 1
            Dim currentpanel As Panel = maincontextctl.FindControl("pnl" & currentpanelnumber)
            For Each c As Control In currentpanel.Controls
                If TypeOf c Is RN_AttestationControl Then
                    Select Case True
                        Case UserAndRoleHelper.IsUserAdmin
                            DirectCast(c, RN_AttestationControl).ShowPanel()
                        Case UserAndRoleHelper.IsUserSecretary
                            DirectCast(c, RN_AttestationControl).ShowPanel_ReadOnly()
                        Case UserAndRoleHelper.IsUserReadOnly
                            DirectCast(c, RN_AttestationControl).ShowPanel_ReadOnly()
                        Case Else
                            Select Case MAIS_Helper.GetUserRoleUsingMAIS(MAIS_Helper.GetUserId).RoleName
                                Case "QA RN", "17 + Bed"
                                    DirectCast(c, RN_AttestationControl).ShowPanel_ReadOnly()
                                Case Else
                                    DirectCast(c, RN_AttestationControl).ShowPanel()
                            End Select
                    End Select
                    'DirectCast(c, RN_AttestationControl).ShowPanel()
                    'to test if error in master 
                End If
            Next
        Next
        SetSignaturField()

        TestButtons()

        'todo  ' we save here so that the yes/no answers aren't reset when the user hits "add". 
        Save()


    End Sub

    Private Sub SetSignaturField()
        Dim appInfo As IApplicationDetailInformationService = StructureMap.ObjectFactory.GetInstance(Of IApplicationDetailInformationService)()
        Dim Signature = appInfo.GetApplicationInfromationByAppID(SessionHelper.ApplicationID).Signature
        Select Case True
            Case Signature Is Nothing
                Me.txtInitials.Text = String.Empty
            Case String.IsNullOrWhiteSpace(Signature)
                Me.txtInitials.Text = String.Empty
            Case Else
                Me.txtInitials.Text = Signature
        End Select
    End Sub
    Private Sub Save()
        Dim currnetPanel As Panel

        For currnetPanelNumber As Integer = 1 To numberofQuestions - 1
            currnetPanel = Page.Master.FindControl("mainContent").FindControl("pnl" & currnetPanelNumber)

            If currnetPanel.Controls.Item(0).FindControl("rblQuestion" & currnetPanelNumber) IsNot Nothing Then
                Dim currentRadioButtonList As RadioButtonList = currnetPanel.Controls.Item(0).FindControl("rblQuestion" & currnetPanelNumber)
                Dim questionID As HiddenField = currnetPanel.Controls.Item(0).FindControl("hiddenQuestionID")

                If currentRadioButtonList.SelectedValue IsNot String.Empty Then
                    Dim modelForAttestationPanel As New MAIS.Business.Model.RN_AttestationPanel
                    Dim attService As IRN_AttestationService = StructureMap.ObjectFactory.GetInstance(Of IRN_AttestationService)()

                    modelForAttestationPanel.Application_SID = SessionHelper.ApplicationID
                    modelForAttestationPanel.Attestation_SID = questionID.Value
                    modelForAttestationPanel.YesNo = currentRadioButtonList.SelectedValue

                    'Adding attestant_sid for application _history JH 9/30/13
                    Dim UserRNService As IUserRNMappingService = StructureMap.ObjectFactory.GetInstance(Of IUserRNMappingService)()
                    Dim IsRn As Boolean = UserRNService.IsCurrentUser_RN_ByUserID(MAIS_Helper.GetUserId)
                    Dim RNID As Integer = 0
                    If IsRn Then
                        RNID = UserRNService.GetUserRNMappingByuserID(MAIS_Helper.GetUserId).RN_Sid                        
                    End If

                    attService.InsertRn_AttestationPanel(modelForAttestationPanel, RNID)

                    'todo -- CheckForErrorMessage on master page. 

                End If
            End If

        Next

        'todo CheckPageComplete()
        TestButtons()

    End Sub

    Private Sub TestButtons()


        Dim enableButton As Boolean = PagesHelper.GetPageCompletionRules(SessionHelper.ApplicationID).IsMAISAttestationPageCompleted(SessionHelper.SelectedUserRole, AppTypeID)

        Me.bntSaveAndContinue1.Enabled = enableButton
        'Me.bntSaveAndContinue.DataBind()



        'Dim isallQuestionTest As Boolean = False
        'For Each c As Control In pnlAttestationMain.Controls
        '    If TypeOf c.Controls(0) Is RadioButtonList Then
        '        Dim currentRadioButtonList As RadioButtonList = c.Controls(0)
        '        If currentRadioButtonList.SelectedValue IsNot String.Empty Then
        '            isallQuestionTest = True
        '        Else
        '            isallQuestionTest = False
        '        End If
        '    End If

        'Next
    End Sub

    Protected Sub btnAgreeInitials_Click(sender As Object, e As EventArgs) Handles btnAgreeInitials.Click
        'Need to save the initials to the ma_Application in the Attestant_Sid. 
        'The Attenstat_SID is the current user logged into the system that is entering the data for the applicant.
        If SessionHelper.ApplicationID > 0 Then
            Page.Validate()
            If txtInitials.Text.Trim.Length > 0 Then
                Dim applicationService As IApplicationDetailInformationService = StructureMap.ObjectFactory.GetInstance(Of IApplicationDetailInformationService)()

                Dim UserRNService As IUserRNMappingService = StructureMap.ObjectFactory.GetInstance(Of IUserRNMappingService)()

                'Need to test if the Current user is a RN 
                Dim IsRn As Boolean = UserRNService.IsCurrentUser_RN_ByUserID(MAIS_Helper.GetUserId)
                If IsRn Then
                    Dim RNID As Integer = UserRNService.GetUserRNMappingByuserID(MAIS_Helper.GetUserId).RN_Sid
                    Dim ItemSaved As Boolean = applicationService.UpdateApplicationSignature(SessionHelper.ApplicationID, Me.txtInitials.Text, MAIS_Helper.GetUserId, RNID)
                    If ItemSaved Then
                        lblAgreed.Text = "You have successfully clicked the Agree button and completed your attestation."
                        lblAttestationError.Text = String.Empty
                    Else
                        lblAttestationError.Text = "The attestations were not saved successfully"
                        lblAgreed.Text = "The attestations were not saved successfully... Please try again or contact technical support."
                    End If
                ElseIf UserAndRoleHelper.IsUserAdmin Then
                    Dim ItemSaved As Boolean = applicationService.UpdateApplicationSignature(SessionHelper.ApplicationID, Me.txtInitials.Text, MAIS_Helper.GetUserId, 0, True)
                    If ItemSaved Then
                        lblAgreed.Text = "You have successfully clicked the Agree button and completed your attestation."
                        lblAttestationError.Text = String.Empty
                    Else
                        lblAttestationError.Text = "The attestations were not saved successfully"
                        lblAgreed.Text = "The attestations were not saved successfully... Please try again or contact technical support."
                    End If
                Else
                    lblAttestationError.Text = "You cannot save this information because you are not a RN. Please contact your administrator."

                End If
            Else
                lblAttestationError.Text = "Attestation Initials Cannot Be Empty If Agreeing to Attestation!"
            End If

        End If
    End Sub

    Protected Sub bntSaveAndContinue1_Click(sender As Object, e As EventArgs) Handles bntSaveAndContinue1.Click
        Response.Redirect(PagesHelper.GetNextPage(Master.CurrentPage))
    End Sub

    Protected Sub bntPrevious_Click(sender As Object, e As EventArgs) Handles bntPrevious.Click
        Response.Redirect(PagesHelper.GetPreviousPage(Master.CurrentPage))
    End Sub
End Class
