Imports MAIS.Business
Imports MAIS.Business.Services

Public Class SecurityMapping
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.HideLink = True
        Master.HideProgressBar = True
        SessionHelper.Notation_Flg = False
        lblRNErr.Visible = False
        lblCount.Visible = False
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        RN_Results()       
    End Sub

    Private Sub grdResults_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdResults.RowCancelingEdit
        grdResults.EditIndex = -1
        RN_Results()
    End Sub

    

    Private Sub grdResults_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdResults.RowEditing
        grdResults.EditIndex = e.NewEditIndex
        RN_Results()
        'Dim tb8 As DropDownList = grdResults.Rows(e.NewEditIndex).FindControl("ddlStatus")
        'tb8 = ddlStatus

    End Sub

    Private Sub grdResults_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdResults.RowUpdating
        lblRNErr.Visible = False
        Dim Rnid As Integer = CInt(grdResults.DataKeys(e.RowIndex).Value.ToString())
        Dim com As String = DirectCast(grdResults.Rows(e.RowIndex).FindControl("txtComments"), TextBox).Text
        Dim chFlg As Boolean = DirectCast(grdResults.Rows(e.RowIndex).FindControl("chMap"), CheckBox).Checked
        Dim rnMapSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        If Not (String.IsNullOrWhiteSpace(com)) Then
            If (rnMapSvc.UpdateRNMapping(Rnid, com, chFlg).ReturnValue) Then
                lblRNErr.Visible = True
                lblRNErr.InnerText = "Data is updated successfully"
                lblRNErr.Style.Add("color", "green")
                RN_Results()
            Else
                lblRNErr.Visible = True
                lblRNErr.InnerText = "Data is not updated"
                lblRNErr.Style.Add("color", "red")
            End If
        Else
            lblRNErr.Visible = True
            lblRNErr.InnerText = "Comments are required"
            lblRNErr.Style.Add("color", "red")
        End If
      
    End Sub

   
    Private Sub RN_Results()
        Dim rnMapSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Dim retResults As List(Of Model.RN_Mapping) = rnMapSvc.GetRNForMappings(txtRNLNO.Value, txtFName.Value, txtLName.Value, ddlStatus.SelectedValue)
        If retResults.Count > 0 Then
            For Each r In retResults
                Dim userService As ODMRDD_NET2.IUserService = New ODMRDD_NET2.UserService
                Dim user As ODMRDD_NET2.IUser = Nothing
                user = userService.GetUserByUserId(r.Last_Updated_By)
                r.EditedBy = user.FirstName + " " + user.LastName + " " + r.Last_Updated_Date.ToShortDateString()
            Next
            lblCount.Visible = True
            lblCount.InnerText = retResults.Count.ToString() + " RN's found in MAIS system"
            grdResults.DataSource = (From r In retResults
                                      Order By r.Last_Name
                                      Select New With {
                                            .RN_Sid = r.RN_Sid,
                                            .Un_Map_Flg = r.Un_Map_Flg,
                                            .RNLicenseNumber = r.RNLicenseNumber,
                                            .First_Name = r.First_Name,
                                            .Last_Name = r.Last_Name,
                                            .Comments = r.Comments,
                                            .EditedBy = r.EditedBy
                                          }).ToList()
            grdResults.DataBind()
        Else
            lblCount.Visible = False
            lblCount.InnerText = String.Empty
            lblRNErr.Visible = True
            lblRNErr.InnerText = "No RN's found"
            lblRNErr.Style.Add("color", "red")
            grdResults.DataSource = Nothing
            grdResults.DataBind()
        End If
    End Sub
End Class