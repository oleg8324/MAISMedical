Imports MAIS.Business
Imports MAIS.Business.Services
Imports MAIS.Business.Model
Imports ODMRDDHelperClassLibrary.Utility

Public Class SecretaryAssociation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.HideLink = True
        Master.HideProgressBar = True
        SessionHelper.Notation_Flg = False
        If Not IsPostBack Then
            lblErr.Text = String.Empty
            lblSecErr.Visible = False
            pnlSearch.Visible = True
            pnlResults.Visible = True
            pnlAssocoate.Visible = False
            LoadRN()
        End If
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        SecretaryResults()
    End Sub
    Private Sub LoadRN()
        Dim UserRNService As IUserRNDetailService = StructureMap.ObjectFactory.GetInstance(Of IUserRNDetailService)()
        Dim ListofRNs As List(Of Business.Model.RN_UserDetails)
        ListofRNs = UserRNService.getAllRNDetails

        ddlRNSearch.Items.Clear()

        Me.ddlRNSearch.Items.Add(New ListItem("--Select RN--", "-1"))
        For Each i As Business.Model.RN_UserDetails In ListofRNs
            Me.ddlRNSearch.Items.Add(New ListItem(i.LastFirstname, i.RN_Sid))
        Next
        Me.ddlRNSearch.DataBind()

        ddlRNsList.Items.Clear()
        Me.ddlRNsList.Items.Add(New ListItem("--Select RN--", "-1"))
        For Each i As Business.Model.RN_UserDetails In ListofRNs
            Me.ddlRNsList.Items.Add(New ListItem(i.LastFirstname, i.RN_Sid))
        Next
        Me.ddlRNsList.DataBind()

        If ((UserAndRoleHelper.IsUserRN) And Not (SessionHelper.MAISLevelUserRole = Enums.RoleLevelCategory.RNMaster_RLC)) Then
            ddlRNSearch.Enabled = False
            ddlRNsList.Enabled = False
            Dim RNID = MAIS_Helper.GetRN_SidbyUserID(SessionHelper.MAISUserID)
            If RNID > 0 Then
                ddlRNSearch.SelectedValue = RNID
                ddlRNsList.SelectedValue = RNID
            Else
                ddlRNSearch.SelectedValue = -1
                ddlRNsList.SelectedValue = -1
            End If
        Else
            ddlRNSearch.Enabled = True
            ddlRNsList.Enabled = True
        End If

    End Sub

    Private Sub SecretaryResults()
        Try
            Dim secMapSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
            Dim retResults As List(Of Secretary_Association) = secMapSvc.GetAllSecretaries(txtEmail.Value, txtFName.Value, txtLName.Value, ddlRNSearch.SelectedValue, ddlStatus.SelectedValue)
            If retResults.Count > 0 Then
                lblSecErr.Visible = False
                For Each r In retResults
                    Dim userService As ODMRDD_NET2.IUserService = New ODMRDD_NET2.UserService
                    Dim user As ODMRDD_NET2.IUser = Nothing
                    user = userService.GetUserByUserId(r.Last_Updated_By)
                    r.EditedBy = user.FirstName + " " + user.LastName + " " + r.Last_Updated_Date.ToShortDateString()
                Next
                grdResults.DataSource = (From r In retResults
                                              Order By r.First_Name
                                              Select New With {
                                                 .User_Mapping_Sid = r.User_Mapping_Sid,
                                                 .First_Name = r.First_Name,
                                                 .Last_Name = r.Last_Name,
                                                 .Middle_Name = r.Middle_Name,
                                                 .SecretaryUserName = r.SecretaryUserName,
                                                 .Email = r.Email,
                                                 .EditedBy = r.EditedBy
                                             }).ToList()
                grdResults.DataBind()
            Else
                grdResults.DataSource = Nothing
                grdResults.DataBind()
                lblSecErr.Visible = True
            End If
        Catch ex As Exception
            Master.SetError(ex.Message)
        End Try
    End Sub

    Private Sub grdResults_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdResults.RowCommand
        lblErr.Text = String.Empty
        If (e.CommandName = "Associate") Then
            pnlAssocoate.Visible = True
            pnlResults.Visible = False
            pnlSearch.Visible = False
            pnlSecretaryList.Visible = True
            ViewState("User_Mapping_Sid") = String.Empty
            Dim U_Id As Integer = Convert.ToInt32(e.CommandArgument)
            Dim gvRow As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            lblFirstName.Text = grdResults.Rows(gvRow.RowIndex).Cells(1).Text.Trim()
            lblLastName.Text = grdResults.Rows(gvRow.RowIndex).Cells(2).Text.Trim()
            lblMiddleName.Text = grdResults.Rows(gvRow.RowIndex).Cells(3).Text.Trim()
            lblUserCode.Text = grdResults.Rows(gvRow.RowIndex).Cells(4).Text.Trim()
            txt_Email.Value = grdResults.Rows(gvRow.RowIndex).Cells(5).Text.Trim()
            'Get list of RN's associated
            If U_Id > 0 Then
                LoadSecretaryList()
                ddlSecretaryList.SelectedValue = U_Id
                ViewState("User_Mapping_Sid") = U_Id
                getRnAssociationsForSecretary()
            End If

        End If
    End Sub
    Private Sub grdRNAssociated_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdRNAssociated.RowCommand
        lblErr.Text = String.Empty
        If (e.CommandName = "DisAssociate") Then
            ViewState("RN_Secretary_Association_Sid") = String.Empty
            Dim rs_Id As Integer = Convert.ToInt32(e.CommandArgument)
            If rs_Id > 0 Then
                ViewState("RN_Secretary_Association_Sid") = rs_Id
                Dim secMapSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
                Dim retSec As Boolean = secMapSvc.RemoveRnForSecretary(ViewState("RN_Secretary_Association_Sid"))
                Dim retVal As String = secMapSvc.GetRNEmailAddressUsingRNsid(ViewState("RN_Secretary_Association_Sid"), 0)
                If retSec Then
                    lblErr.Text = "Removed successfully"
                    lblErr.Style.Add("color", "green")
                    If GenerateEmailNotification(0, retVal) = -1 Then
                        lblErr.Text = "Error in email generation"
                        lblErr.Style.Add("color", "red")
                    End If
                    getRnAssociationsForSecretary()
                Else
                    lblErr.Text = "Error in removing association"
                    lblErr.Style.Add("color", "red")
                End If

            End If
        End If
    End Sub
    Private Sub getRnAssociationsForSecretary()
        Dim secMapSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Dim retSecList As List(Of Secretary_Detail) = secMapSvc.GetSecretaryDetails(ViewState("User_Mapping_Sid"))
        If retSecList.Count > 0 Then
            For Each r In retSecList
                Dim userService As ODMRDD_NET2.IUserService = New ODMRDD_NET2.UserService
                Dim user As ODMRDD_NET2.IUser = Nothing
                user = userService.GetUserByUserId(r.Last_Updated_By)
                r.EditedBy = user.FirstName + " " + user.LastName + " " + r.Last_Updated_Date.ToShortDateString()
            Next
            grdRNAssociated.DataSource = (From aa1 In retSecList
                                          Order By aa1.RN_Name
                                          Select New With {
                                             .Comments = aa1.Comments,
                                             .RN_Secretary_Association_Sid = aa1.RN_Secretary_Association_Sid,
                                             .RNLicense = aa1.RNLicense,
                                             .EditedBy = aa1.EditedBy,
                                             .RN_Name = aa1.RN_Name,
                                             .RN_Sid = aa1.RN_Sid
                                              }).ToList()
            grdRNAssociated.DataBind()
            'grdRNAssociated.Columns(0).Visible = False
            'grdRNAssociated.Columns(1).Visible = False
        Else
            grdRNAssociated.DataSource = Nothing
            grdRNAssociated.DataBind()
        End If
    End Sub


    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            lblErr.Text = String.Empty
            If ((String.IsNullOrWhiteSpace(txt_Email.Value)) OrElse (String.IsNullOrWhiteSpace(txtComments.Text)) OrElse (ddlRNsList.SelectedValue = "-1")) Then
                lblErr.Text = "Please enter mandatory data EMail,Comments and select an RN to save the information."
            Else
                If ViewState("User_Mapping_Sid") IsNot Nothing Then
                    Dim secInfo As New Secretary_Detail
                    secInfo.User_Mapping_Sid = ViewState("User_Mapping_Sid")
                    secInfo.RN_Sid = ddlRNsList.SelectedValue
                    secInfo.Comments = txtComments.Text.Trim()
                    Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
                    Dim retObj As Boolean = maisSvc.SaveSecretaryDetails(secInfo)
                    Dim retVal As String = maisSvc.GetRNEmailAddressUsingRNsid(secInfo.RN_Sid, 1)
                    If retObj Then
                        lblErr.Text = "Data is Successfully saved"
                        lblErr.Style.Add("color", "green")
                        txtComments.Text = String.Empty
                        ddlRNsList.SelectedValue = "-1"
                        If GenerateEmailNotification(1, Trim(retVal)) = -1 Then
                            lblErr.Text = "Error in email generation"
                            lblErr.Style.Add("color", "red")
                        End If
                        LoadRN()
                        getRnAssociationsForSecretary()
                    Else
                        lblErr.Text = "Data is not saved"
                        lblErr.Style.Add("color", "red")
                    End If
                End If
            End If
        Catch ex As Exception
            Master.SetError(ex.Message)
        End Try
    End Sub
    Private Function GenerateEmailNotification(ByVal mappingorUnmapping As Integer, ByVal rnEmail As String) As Integer
        Dim retVal As String = String.Empty
        Dim strToEmailAddress As String = String.Empty
        If (String.IsNullOrEmpty(ConfigHelper.ToEmailAddress)) Then
            strToEmailAddress = Trim(txt_Email.Value) + ";" + Trim(rnEmail)
        Else
            strToEmailAddress = ConfigHelper.ToEmailAddress
        End If
        Dim emailSvc As IEmailService = StructureMap.ObjectFactory.GetInstance(Of IEmailService)()
        If (mappingorUnmapping = 1) Then
            Dim strBodyMessage As String = "You are successfully mapped to MAIS system.<br/><br/>"
            strBodyMessage = strBodyMessage + "You should feel free to contact your RN " + Trim(rnEmail) + " or DODD if you have any questions or concerns about mapping or unmapping to MAIS system.<br/><br/>"
            strBodyMessage = strBodyMessage + "Thank you!"
            Dim retObj As ReturnObject(Of Boolean) = emailSvc.SendEmail(Trim(strToEmailAddress),
                                                                            ConfigHelper.FromEmailAddress,
                                                                            ConfigHelper.EmailMappingSubjectStatus,
                                                                            strBodyMessage, Nothing, String.Empty, String.Empty)
            If retObj.ReturnValue Then
                retVal = "An email containing your status of this application was sent successfully."
            Else
                If retObj.Messages.Count > 0 Then
                    retVal = retObj.MessageStrings.First()
                Else
                    retVal = "ERROR: An error has occurred while trying to send an email for QA registeration."
                End If
                Return -1
            End If
        Else
            Dim strBodyMessage As String = "You are unmapped to MAIS system.<br/><br/>"
            strBodyMessage = strBodyMessage + "You should feel free to contact your RN " + Trim(rnEmail) + " or DODD if you have any questions or concerns about mapping or unmapping to MAIS system.<br/><br/>"
            strBodyMessage = strBodyMessage + "Thank you!"
            Dim retObj As ReturnObject(Of Boolean) = emailSvc.SendEmail(strToEmailAddress.Trim(),
                                                                            ConfigHelper.FromEmailAddress,
                                                                            ConfigHelper.EmailMappingSubjectStatus,
                                                                            strBodyMessage, Nothing, String.Empty, String.Empty)
            If retObj.ReturnValue Then
                retVal = "An email containing your status of this application was sent successfully."
            Else
                If retObj.Messages.Count > 0 Then
                    retVal = retObj.MessageStrings.First()
                Else
                    retVal = "ERROR: An error has occurred while trying to send an email for QA registeration."
                End If
                Return -1
            End If
        End If
    End Function

    Protected Sub btnAddWe_Click(sender As Object, e As EventArgs) Handles btnAddWe.Click
        ClearFields()
    End Sub

    Protected Sub lnkGoBack_Click(sender As Object, e As EventArgs) Handles lnkGoBack.Click
        pnlSearch.Visible = True
        pnlResults.Visible = True
        pnlAssocoate.Visible = False
        pnlSecretaryList.Visible = False
        lblErr.Text = String.Empty
    End Sub
    Private Sub ClearFields()
        lblFirstName.Text = String.Empty
        lblLastName.Text = String.Empty
        lblMiddleName.Text = String.Empty
        txt_Email.Value = String.Empty
        txtComments.Text = String.Empty
        ddlRNsList.SelectedValue = "-1"
        lblUserCode.Text = String.Empty
        grdRNAssociated.DataSource = Nothing
        grdRNAssociated.DataBind()
    End Sub

    Protected Sub lnkAddSecretary_Click(sender As Object, e As EventArgs) Handles lnkAddSecretary.Click
        pnlSearch.Visible = False
        pnlResults.Visible = False
        pnlAssocoate.Visible = True
        pnlSecretaryList.Visible = True
        ClearFields()
        LoadSecretaryList()
        LoadRN()
    End Sub
    Private Sub LoadSecretaryList()

        Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Dim secList As List(Of Secretary_Association)
        secList = maisSvc.GetSecreatryList()

        ddlSecretaryList.Items.Clear()

        If secList.Count > 0 Then
            Me.ddlSecretaryList.Items.Add(New ListItem("--Select Secretary--", "-1"))
            For Each i As Business.Model.Secretary_Association In secList
                Me.ddlSecretaryList.Items.Add(New ListItem(i.First_Name + " " + i.Last_Name + " " + i.Middle_Name, i.User_Mapping_Sid))
            Next
            Me.ddlSecretaryList.DataBind()
        End If

    End Sub

    Private Sub ddlSecretaryList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSecretaryList.SelectedIndexChanged
        ViewState("User_Mapping_Sid") = String.Empty
        Dim U_Id As Integer = Convert.ToInt32(ddlSecretaryList.SelectedValue)
        If U_Id > 0 Then
            ViewState("User_Mapping_Sid") = U_Id
            Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
            Dim secInfo As Secretary_Association = maisSvc.GetSecretaryByID(U_Id)
            If secInfo IsNot Nothing Then
                lblFirstName.Text = secInfo.First_Name
                lblLastName.Text = secInfo.Last_Name
                lblMiddleName.Text = secInfo.Middle_Name
                lblUserCode.Text = secInfo.SecretaryUserName
                txt_Email.Value = secInfo.Email
                getRnAssociationsForSecretary()
            End If
        End If
    End Sub

    Private Sub grdRNAssociated_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdRNAssociated.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lnkAct As LinkButton = CType(e.Row.FindControl("lnkSecAssociation"), LinkButton)
            Dim gr_RN_Sid As Integer = CType(e.Row.FindControl("hdRN_Sid"), HiddenField).Value
            If ((UserAndRoleHelper.IsUserRN) And Not (SessionHelper.MAISLevelUserRole = Enums.RoleLevelCategory.RNMaster_RLC)) Then
                Dim RNID = MAIS_Helper.GetRN_SidbyUserID(SessionHelper.MAISUserID)
                If ((gr_RN_Sid > 0) And (RNID > 0) And (gr_RN_Sid = RNID)) Then
                    lnkAct.Visible = True
                Else
                    lnkAct.Visible = False
                End If
            End If
        End If
    End Sub
End Class