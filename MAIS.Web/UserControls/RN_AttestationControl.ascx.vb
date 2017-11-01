Imports ODMRDDHelperClassLibrary
Imports MAIS.Business.Services
Imports MAIS.Business
Imports MAIS.Business.Rules
Imports MAIS.Business.Model

Public Class RN_AttestationControl
    Inherits System.Web.UI.UserControl

    Private _message As New List(Of Utility.ReturnMessage)

    Public ReadOnly Property Messages() As List(Of Utility.ReturnMessage)
        Get
            Return _message
        End Get
    End Property

    Private PanelNumber As Integer
    Private QuestionNumber As Integer
    Private ApplicationType As Integer
    Private Attestation_Sid As Long

 


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Page.DataBind()
    End Sub

    Public Sub SetControlParameters(ByVal panelNumber As Integer, ByVal questionNumber As Integer, ByVal attestation_Sid As Integer, ByVal applicationType As Integer, ByVal QuestionText As String)
        Me.PanelNumber = panelNumber
        Me.QuestionNumber = questionNumber
        Me.Attestation_Sid = attestation_Sid
        Me.ApplicationType = applicationType
        Me.lblQuestion.Text = QuestionText
    End Sub

    Public Sub Initialize()
        Dim currentPanel As Panel
        Select Case Me.PanelNumber
            Case Else
                currentPanel = pnlYesNo

        End Select
        currentPanel.Visible = True
        For Each controlPanel As Control In currentPanel.Controls
            If TypeOf controlPanel Is RadioButtonList Then
                CType(controlPanel, RadioButtonList).ID = "rblQuestion" & Me.QuestionNumber
            End If
        Next
    End Sub

    Public Sub ShowPanel()
        Dim RnAttService As IRN_AttestationService = StructureMap.ObjectFactory.GetInstance(Of IRN_AttestationService)()

        Dim listOfRnAttestationModel As List(Of RN_AttestationEntity) = RnAttService.GetRn_AttestationEntitiesByApplicationID_Attestation_ApplicationType_Xref_Sid(SessionHelper.ApplicationID, Me.Attestation_Sid)
        Dim RnAttPanelModel As RN_AttestationPanel = RnAttService.GetRn_AttestationPanelbyApplicaitonID_Attestation_Applicationtype_xrefSid(SessionHelper.ApplicationID, Me.Attestation_Sid)


        'If RnAttPanelModel.YesNo Is Nothing Then
        '    lblErrMsg.Text = "Please select Yes or No for the question."
        'Else
        '    lblErrMsg.Text = String.Empty
        'End If

        If (Not (RnAttPanelModel.YesNo Is Nothing)) And listOfRnAttestationModel.Count = 0 Then
            Dim Rn_AttInsert As New MAIS.Business.Model.RN_AttestationEntity
            With Rn_AttInsert
                .Application_SID = SessionHelper.ApplicationID
                .Attestation_Sid = Me.Attestation_Sid
                .CreateBy = MAIS_Helper.GetUserId
                .CreateDate = Now()
                .LastUpdateBy = MAIS_Helper.GetUserId
                .LastUpdateDate = Now()
            End With
            listOfRnAttestationModel.Add(Rn_AttInsert)

            '  RnAttService.InsertRN_Attestation(listOfRnAttestationModel)
        End If

        Dim CurrentPanel As Panel
        Select Case Me.PanelNumber

            Case Else
                CurrentPanel = pnlYesNo
        End Select
        CurrentPanel.Visible = True
        For Each controlInPanel As Control In CurrentPanel.Controls
            If TypeOf controlInPanel Is RadioButtonList Then
                If (Not IsPostBack) And (RnAttPanelModel.YesNo IsNot Nothing) And (RnAttPanelModel.YesNo = 0 Or RnAttPanelModel.YesNo = 1) Then
                    CType(controlInPanel, RadioButtonList).SelectedValue = CInt(RnAttPanelModel.YesNo)
                End If
            End If

        Next

        If Me.RadioButtonList5.SelectedValue Is String.Empty Then
            lblErrMsg.Text = "Please select Yes or No for the question."
        Else
            lblErrMsg.Text = ""
        End If
        _message.AddRange(RnAttService.Messages)
        RnAttService.Messages.Clear()

        hiddenQuestionID.Value = Me.Attestation_Sid

    End Sub


    Public Sub ShowPanel_ReadOnly()
        ShowPanel()

        Me.pnlYesNo.Enabled = False
    End Sub
End Class