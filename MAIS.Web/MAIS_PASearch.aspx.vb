Public Class MAIS_PASearch
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            rblSelect.SelectedValue = "-1"
            pnlDDSearch.Visible = False
            pnlRnSearch.Visible = False
            pnlSessions.Visible = False
        End If
        If (rblSelect.SelectedValue = "" OrElse CInt(rblSelect.SelectedValue) <= 0) Then
            btnRun.Visible = False
        Else
            btnRun.Visible = True
        End If
    End Sub
    Private Sub rblSelect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblSelect.SelectedIndexChanged
        pError.Visible = False
        If (rblSelect.SelectedValue = "1") Then
            grdRNList.Visible = False
            grdDDList.Visible = False
            grdSessionSearch.Visible = False
            ClearFields()
            pnlDDSearch.Visible = False
            pnlRnSearch.Visible = True
            pnlSessions.Visible = False
        ElseIf (rblSelect.SelectedValue = "2") Then
            grdRNList.Visible = False
            grdDDList.Visible = False
            grdSessionSearch.Visible = False
            ClearFields()
            pnlDDSearch.Visible = True
            pnlRnSearch.Visible = False
            pnlSessions.Visible = False
        ElseIf (rblSelect.SelectedValue = "3") Then
            grdRNList.Visible = False
            grdDDList.Visible = False
            grdSessionSearch.Visible = False
            LoadCounties()
            ClearFields()
            hRnSession.Value = "1"
            pnlDDSearch.Visible = False
            pnlRnSearch.Visible = False
            pnlSessions.Visible = True
        ElseIf (rblSelect.SelectedValue = "4") Then
            grdRNList.Visible = False
            grdDDList.Visible = False
            grdSessionSearch.Visible = False
            LoadCounties()
            ClearFields()
            hRnSession.Value = "0"
            pnlDDSearch.Visible = False
            pnlRnSearch.Visible = False
            pnlSessions.Visible = True
        End If
    End Sub
    Public Sub LoadCounties()
        'Dim lstCountyBoards As New ReturnObject(Of ODMRDDHelperClassLibrary.CountyBoardCollection)
        'Dim cs As CountyBoardService = New CountyBoardService(ConfigHelper.GetOIDDBConnectionString)
        'lstCountyBoards = cs.GetAllCountyBoards("mastr", True)
        'ddlCounty.Items.Clear()
        'Counties
        Dim userSvc As Business.Services.IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of Business.Services.IMAISSerivce)()
        Dim lstCountyBoards As List(Of Business.Model.CountyDetails) = userSvc.GetAllCountyCodes()
        ddlCounty.Items.Add(New ListItem("--- County Selection ---", "-1"))
        ddlCounty.DataSource = lstCountyBoards
        ddlCounty.DataTextField = "CountyAlias"
        ddlCounty.DataValueField = "CountyID"
        ddlCounty.DataBind()
    End Sub
    Protected Sub btnRun_click() Handles btnRun.ServerClick
        pError.Visible = False
        grdRNList.Visible = False
        grdDDList.Visible = False
        grdSessionSearch.Visible = False
        If Not ValidateParameters() Then
            pError.Visible = True
            pError.InnerHtml = "Please enter either one of the below search criteria."
            Exit Sub
        End If
        Dim pas As New PublicAccessService.MAIS_DODD_WCF() With {.Url = ConfigHelper.PublicAccessEndpoint}
        If (rblSelect.SelectedValue = "1") Then
            Dim rninf As PublicAccessService.RNDetailInformation() = pas.GetRNData(txtLname.Value, txtFname.Value, txtRNLicenseNo.Value)

            grdRNList.Visible = True
            grdRNList.DataSource = rninf
            grdRNList.DataBind()
            'pnlRnSearch.Visible = True
            divSpinner.Style("display") = "none"
        ElseIf (rblSelect.SelectedValue = "2") Then
            Dim ddinf As PublicAccessService.DDDetailInformation() = pas.GetDDData(txtDODDIdNo.Value, txLast4ssn.Text, txtDDLname.Value, txtDDFname.Value, txtEmployer.Value)
            grdDDList.Visible = True
            grdDDList.DataSource = ddinf
            grdDDList.DataBind()
            divSpinner.Style("display") = "none"
        ElseIf (rblSelect.SelectedValue = "3") Then
            If Not String.IsNullOrEmpty(Trim(txtEndDate.Value)) Then
                If Not IsDate(Trim(txtEndDate.Value)) Then
                    pError.Visible = True
                    pError.InnerHtml = "Please enter a valid date."
                    Exit Sub
                End If
            End If
            If Not String.IsNullOrEmpty(Trim(txtStDate.Value)) Then
                If Not IsDate(Trim(txtStDate.Value)) Then
                    pError.Visible = True
                    pError.InnerHtml = "Please enter a valid date."
                    Exit Sub
                End If
            End If
            Dim countystring As String = IIf(ddlCounty.SelectedValue = -1, "", ddlCounty.SelectedItem.Text)
            Dim sessinf As PublicAccessService.TrainingSessionResults() = pas.GetTrainingSessionData(txtStDate.Value, txtEndDate.Value, countystring, txtRNLname.Value, txtRNFname.Value, hRnSession.Value.ToString)
            grdSessionSearch.Visible = True
            grdSessionSearch.DataSource = sessinf
            grdSessionSearch.DataBind()

            divSpinner.Style("display") = "none"
        ElseIf (rblSelect.SelectedValue = "4") Then
            If Not String.IsNullOrEmpty(Trim(txtEndDate.Value)) Then
                If Not IsDate(Trim(txtEndDate.Value)) Then
                    pError.Visible = True
                    pError.InnerHtml = "Please enter a valid date."
                    Exit Sub
                End If
            End If
            If Not String.IsNullOrEmpty(Trim(txtStDate.Value)) Then
                If Not IsDate(Trim(txtStDate.Value)) Then
                    pError.Visible = True
                    pError.InnerHtml = "Please enter a valid date."
                    Exit Sub
                End If
            End If
            Dim countystring As String = IIf(ddlCounty.SelectedValue = -1, "", ddlCounty.SelectedItem.Text)
            Dim sessinf As PublicAccessService.TrainingSessionResults() = pas.GetTrainingSessionData(txtStDate.Value, txtEndDate.Value, countystring, txtRNLname.Value, txtRNFname.Value, hRnSession.Value.ToString)
            grdSessionSearch.Visible = True
            grdSessionSearch.DataSource = sessinf
            grdSessionSearch.DataBind()

            divSpinner.Style("display") = "none"
        End If

        'Debug.Print(rninf(0).RNLicenseNumber)
    End Sub
    Private Function ValidateParameters() As Boolean
        Dim flg As Boolean = True
        If ((String.IsNullOrWhiteSpace(Trim(txtRNLicenseNo.Value))) And
            (String.IsNullOrWhiteSpace(Trim(txtFname.Value))) And
            (String.IsNullOrWhiteSpace(Trim(txtLname.Value))) And
            (String.IsNullOrWhiteSpace(txLast4ssn.Text.ToString)) And
            (String.IsNullOrWhiteSpace(Trim(txtDODDIdNo.Value))) And
            (String.IsNullOrWhiteSpace(Trim(txtDDLname.Value))) And
            (String.IsNullOrWhiteSpace(Trim(txtDDFname.Value))) And
            (String.IsNullOrWhiteSpace(Trim(txtEmployer.Value))) And
            (String.IsNullOrWhiteSpace(Trim(txtStDate.Value))) And
            (String.IsNullOrWhiteSpace(Trim(txtEndDate.Value))) And
            IIf((rblSelect.SelectedValue = 3 Or rblSelect.SelectedValue = 4), (ddlCounty.SelectedValue.ToString = "" Or ddlCounty.SelectedValue.ToString = "-1"), True) And
            (String.IsNullOrWhiteSpace(Trim(txtRNLname.Value))) And
            (String.IsNullOrWhiteSpace(Trim(txtRNFname.Value)))) Then
            flg = False
        End If
        Return flg
    End Function

    Private Sub grdRNList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdRNList.RowDataBound
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim gvCert As GridView = TryCast(e.Row.FindControl("gvCertification"), GridView)
                gvCert.DataSource = (From cer In CType(e.Row.DataItem, PublicAccessService.RNDetailInformation).CertificateDetails
                                     Order By cer.ExpirationDate
                                     Select New With {
                                         .RoleDescription = cer.RoleDescription,
                                         .CurrentStatus = cer.CurrentStatus,
                                         .EffectiveDate = cer.EffectiveDate,
                                         .ExpirationDate = cer.ExpirationDate,
                                         .ConsectiveRenewals = cer.ConsectiveRenewals
                                         }).ToList()
                gvCert.DataBind()
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub grdDDList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDDList.RowDataBound
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim gvCert As GridView = TryCast(e.Row.FindControl("gvDDCertification"), GridView)
                gvCert.DataSource = (From cer In CType(e.Row.DataItem, PublicAccessService.DDDetailInformation).CertificateDetails
                                     Order By cer.ExpirationDate
                                     Select New With {
                                         .RoleDescription = cer.RoleDescription,
                                         .CurrentStatus = cer.CurrentStatus,
                                         .EffectiveDate = cer.EffectiveDate,
                                         .ExpirationDate = cer.ExpirationDate,
                                         .ConsectiveRenewals = cer.ConsectiveRenewals
                                         }).ToList()
                gvCert.DataBind()
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub ClearFields()
        ddlCounty.SelectedValue = -1
        txtDDFname.Value = String.Empty
        txtDDLname.Value = String.Empty
        txtDODDIdNo.Value = String.Empty
        txtEmployer.Value = String.Empty
        txtEndDate.Value = String.Empty
        txtFname.Value = String.Empty
        txLast4ssn.Text = String.Empty
        txtLname.Value = String.Empty
        txtRNFname.Value = String.Empty
        txtRNLicenseNo.Value = String.Empty
        txtRNLname.Value = String.Empty
        txtStDate.Value = String.Empty
    End Sub

End Class