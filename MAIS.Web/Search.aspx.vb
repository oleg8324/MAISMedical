Imports System.Web.Script.Services
Imports MAIS.Business
Imports MAIS.Business.Helpers
Imports MAIS.Business.Services
Imports MAIS.Business.Model
Imports MAIS.Business.Model.Enums

Public Class About
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            grdDDSearch.DataSource = Nothing
            grdDDSearch.DataBind()
            grdRNSearch.DataSource = Nothing
            grdRNSearch.DataBind()
            Master.HideLink = True
            Master.HideProgressBar = True
            SessionHelper.ApplicationID = 0
            SessionHelper.ApplicationType = String.Empty
            SessionHelper.SessionUniqueID = String.Empty
            SessionHelper.SelectedUserRole = 0
            SessionHelper.ExistingUserRole = 0
            SessionHelper.Name = String.Empty
            SessionHelper.Notation_Flg = False
            SessionHelper.ApplicationStatus = String.Empty
            'pnlLabel.Visible = False
            'If (UserAndRoleHelper.IsUserSecretary) Then
            '    rblSelect.Items(0).Enabled = False
            'Else
            '    rblSelect.Items(0).Enabled = True
            'End If
        End If
        AddLinkButton()
        AddLinkButton1()
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        grdDDSearch.DataSource = Nothing
        grdDDSearch.DataBind()
        grdRNSearch.DataSource = Nothing
        grdRNSearch.DataBind()
        If (rblSelect.SelectedValue = 0) Then
            SessionHelper.RN_Flg = True
        Else
            SessionHelper.RN_Flg = False
        End If
        PopulateGrid(SessionHelper.RN_Flg)
    End Sub
    Private Sub PopulateGrid(ByVal flag As Boolean)
        System.Threading.Thread.Sleep(1000)
        Dim searchSvc As ISearchService = StructureMap.ObjectFactory.GetInstance(Of ISearchService)()
        Dim search As New MAISSearchCriteria
        Dim listRNInfo As List(Of RNSearchResult) = Nothing
        Dim listDDInfo As List(Of DDPersonnelSearchResult) = Nothing
        If (flag) Then
            search.RNLicenseNumber = Trim(txtRNDDLicSSN.Value)
            search.LastName = Trim(txtLastName.Value)
            search.FirstName = Trim(txtFirstName.Value)
            search.EmployerName = Trim(txtEmployer.Value)
            If (Not String.IsNullOrEmpty(txtRNDateDDDOB.Value)) Then
                search.DateOfBirth = Convert.ToDateTime(txtRNDateDDDOB.Value)
            End If
            If (hdApplicationStatus.Value = "0") Then
                search.ApplicationStatus = String.Empty
            Else
                Select Case Convert.ToInt32(hdApplicationStatus.Value)
                    Case 1
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.Pending)
                    Case 2
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.DNMR)
                    Case 3
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.MeetsRequirements)
                    Case 4
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.AddedToRegistry)
                    Case 5
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.RemovedFromRegistry)
                    Case 6
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.DODD_Review)
                    Case 7
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.VoidedApplication)
                    Case 10
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.Certified)
                    Case 11
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.Denied)
                    Case 12
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.IntentToDeny)
                End Select
            End If
            If (String.IsNullOrEmpty(txtAppID.Value)) Then
                search.ApplicationID = 0
            Else
                search.ApplicationID = Convert.ToInt32(txtAppID.Value)
            End If
            'SessionHelper.LoginUsersRNLicense = "RN123456"
            listRNInfo = searchSvc.GetRNInfoFromTables(search, SessionHelper.MAISLevelUserRole, SessionHelper.LoginUsersRNLicense, UserAndRoleHelper.IsUserAdmin, SessionHelper.MAISUserID)
        Else
            Dim ssn As Integer = 0
            Dim ssnCriteria As Boolean = False
            If (Not String.IsNullOrEmpty(txtRNDDLicSSN.Value)) Then
                ssn = Convert.ToInt32(txtRNDDLicSSN.Value)
                ssnCriteria = True
            Else
                ssn = Nothing
            End If
            search.Last4SSN = ssn.ToString()
            search.LastName = Trim(txtLastName.Value)
            search.FirstName = Trim(txtFirstName.Value)
            search.EmployerName = Trim(txtEmployer.Value)
            search.DDCode = Trim(txtDDPersoonelCode.Value)
            If (Not String.IsNullOrEmpty(txtRNDateDDDOB.Value)) Then
                search.DateOfBirth = Convert.ToDateTime(txtRNDateDDDOB.Value)
            End If
            If (hdApplicationStatus.Value = "0") Then
                search.ApplicationStatus = String.Empty
            Else
                Select Case Convert.ToInt32(hdApplicationStatus.Value)
                    Case 1
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.Pending)
                    Case 2
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.DNMR)
                    Case 3
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.MeetsRequirements)
                    Case 4
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.AddedToRegistry)
                    Case 5
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.RemovedFromRegistry)
                    Case 6
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.DODD_Review)
                    Case 7
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.VoidedApplication)
                    Case 10
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.Certified)
                    Case 11
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.Denied)
                    Case 12
                        search.ApplicationStatus = EnumHelper.GetEnumDescription(ApplicationStatusType.IntentToDeny)
                End Select
            End If
            If (String.IsNullOrEmpty(txtAppID.Value)) Then
                search.ApplicationID = 0
            Else
                search.ApplicationID = Convert.ToInt32(txtAppID.Value)
            End If
            listDDInfo = searchSvc.GetDDInfoFromTables(search, ssnCriteria)
        End If
        DisplayResults(listRNInfo, listDDInfo, search, flag)
        'lblText.Text = "Processing completed"
        If (UserAndRoleHelper.IsUserReadOnly) Then
            pnlLabel.Visible = False
        End If

        If UserAndRoleHelper.IsUserAdmin = False Then
            'QA RN and 17 + Bed can not add inital application for DD Personnel I, II, or III from crosswalk Sec Roles Last updated 6/17/2013.
            If (rblSelect.SelectedValue = 1 And UserAndRoleHelper.IsUserRN) Then
                Select Case MAIS_Helper.GetUserRoleUsingMAIS(MAIS_Helper.GetUserId).RoleName
                    Case "QA RN"
                        pnlLabel.Visible = False
                    Case Is = "17 + Bed"
                        pnlLabel.Visible = False
                End Select
            End If
        End If

    End Sub
    Private Sub DisplayResults(ByVal results As IEnumerable(Of RNSearchResult), ByVal ddresults As IEnumerable(Of DDPersonnelSearchResult), ByVal searchCriteria As MAISSearchCriteria, ByVal flag As Boolean)
        If (flag) Then
            If results IsNot Nothing Then
                If results.Count > 0 Then
                    pnlLabel.Visible = False
                    grdDDSearch.DataSource = Nothing
                    grdDDSearch.DataBind()
                    grdDDSearch.Visible = False
                    grdRNSearch.Visible = True
                    grdRNSearch.DataSource = (From p In results
                                              Order By p.LastName
                                              Select New With
                                                     {
                                                         .RNLicenseNumber = p.RNLicenseNumber.Trim(),
                                                         .LastName = p.LastName.Trim(),
                                                         .FirstName = p.FirstName.Trim(),
                                                         .MiddleName = p.MiddleName,
                                                         .HomeAddress = p.HomeAddress,
                                                         .County = p.County,
                                                         .RNTrainer = p.RNTrainer.Trim(),
                                                         .RNInstructor = p.RNInstructor.Trim(),
                                                         .RNMaster = p.RNMaster.Trim(),
                                                         .ICFRN = p.ICFRN.Trim(),
                                                         .QARN = p.QARN.Trim(),
                                                         .ApplicationID = p.ApplicationID.ToString().Trim(),
                    .ApplicationStatus = p.ApplicationStatus,
                    .ApplicationType = p.ApplicationType
                                                  }).ToList()
                    grdRNSearch.DataBind()
                    If (Not String.IsNullOrEmpty(searchCriteria.RNLicenseNumber)) Then
                        pnlLabel.Visible = False
                        'lblPermission.ForeColor = Drawing.Color.Red
                        'lblPermission.Text = "No records are Found."
                        'If (results Is Nothing) Then
                        '    lblPermission.ForeColor = Drawing.Color.Red
                        '    lblPermission.Text = "No records are Found."
                        'End If
                    Else
                        If (searchCriteria.ApplicationID > 0) Then
                            pnlLabel.Visible = False
                        Else
                            pnlLabel.Visible = True
                        End If
                    End If
                Else
                    grdRNSearch.DataSource = Nothing
                    grdRNSearch.DataBind()
                    pnlLabel.Visible = True
                    lblText.ForeColor = Drawing.Color.Red
                    lblText.Text = "No data found"
                End If
            Else
                grdRNSearch.DataSource = Nothing
                grdRNSearch.DataBind()
                pnlLabel.Visible = True
                lblText.ForeColor = Drawing.Color.Red
                lblText.Text = "No data found"
                'If (UserAndRoleHelper.IsUserAdmin = False) Then
                '    If (SessionHelper.MAISLevelUserRole = 5 Or SessionHelper.MAISLevelUserRole = 6) Then
                '        pnlLabel.Visible = True
                '    Else
                '        pnlLabel.Visible = False
                '        lblPermission.ForeColor = Drawing.Color.Red
                '        lblPermission.Text = "You donot have permission to search for this RN details"
                '    End If
                'Else
                '    pnlLabel.Visible = True
                'End If
            End If
        Else
            If ddresults IsNot Nothing Then
                If ddresults.Count > 0 Then
                    grdRNSearch.DataSource = Nothing
                    grdRNSearch.DataBind()
                    grdRNSearch.Visible = False
                    pnlLabel.Visible = False
                    grdDDSearch.Visible = True
                    grdDDSearch.DataSource = (From p In ddresults
                                              Order By p.LastName
                                              Select New With
                                                     {
                                                         .Last4SSN = p.Last4SSN.ToString(),
                                                         .DDPersonnelCode = Trim(p.DDPersonnelCode),
                                                         .LastName = p.LastName.Trim(),
                                                         .FirstName = p.FirstName.Trim(),
                                                         .MiddleName = p.MiddleName,
                                                         .HomeAddress = p.HomeAddress,
                                                         .County = p.County,
                                                         .DateOfBirth = p.DateOfBirth.ToShortDateString().Trim(),
                                                         .CAT1 = p.CAT1.Trim(),
                                                         .CAT2 = p.CAT2.Trim(),
                                                         .CAT3 = p.CAT3.Trim(),
                                                         .ApplicationID = p.ApplicationID.ToString().Trim(),
                                                         .ApplicationStatus = p.ApplicationStatus,
                                                         .ApplicationType = p.ApplicationType
                                                  }).ToList()
                    grdDDSearch.DataBind()
                    pnlLabel.Visible = True
                Else
                    grdDDSearch.DataSource = Nothing
                    grdDDSearch.DataBind()
                    pnlLabel.Visible = True
                    lblText.ForeColor = Drawing.Color.Red
                    lblText.Text = "No data found"
                End If
            Else
                grdDDSearch.DataSource = Nothing
                grdDDSearch.DataBind()
                pnlLabel.Visible = True
                lblText.ForeColor = Drawing.Color.Red
                lblText.Text = "No data found"
            End If
            lblPermission.Text = String.Empty
        End If
    End Sub
    Protected Sub grdDDSearch_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdDDSearch.RowDataBound
        If (e.Row.DataItem IsNot Nothing) Then
            Dim lb As New LinkButton()
            lb.CommandArgument = e.Row.Cells(0).Text + "*" + e.Row.Cells(10).Text + "*" + e.Row.Cells(11).Text + "*" + e.Row.Cells(13).Text + "*" + e.Row.Cells(1).Text + "*" + e.Row.Cells(2).Text + "*" + e.Row.Cells(14).Text
            lb.Text = e.Row.Cells(0).Text
            lb.CommandName = "RNClick"
            AddHandler lb.Command, AddressOf LinkButton_Command
            e.Row.Cells(0).Controls.Add(lb)
        End If
    End Sub
    Private Sub AddLinkButton()
        For Each row As GridViewRow In grdDDSearch.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim lb As New LinkButton()
                lb.CommandArgument = row.Cells(0).Text + "*" + row.Cells(10).Text + "*" + row.Cells(11).Text + "*" + row.Cells(13).Text + "*" + row.Cells(1).Text + "*" + row.Cells(2).Text + "*" + row.Cells(14).Text
                lb.Text = row.Cells(0).Text
                lb.CommandName = "RNClick"
                AddHandler lb.Command, AddressOf LinkButton_Command
                row.Cells(0).Controls.Add(lb)
            End If
        Next
    End Sub
    Private Sub AddLinkButton1()
        For Each row As GridViewRow In grdRNSearch.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim lb As New LinkButton()
                lb.CommandArgument = row.Cells(0).Text + "*" + row.Cells(11).Text + "*" + row.Cells(12).Text + "*" + row.Cells(14).Text + "*" + row.Cells(1).Text + "*" + row.Cells(2).Text
                lb.Text = row.Cells(0).Text
                lb.CommandName = "RNClick"
                AddHandler lb.Command, AddressOf LinkButton_Command1
                row.Cells(0).Controls.Add(lb)
            End If
        Next
    End Sub
    Protected Sub LinkButton_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim str() As String
        str = e.CommandArgument.ToString().Split("*")
        SessionHelper.SessionUniqueID = If(str(6) = "&nbsp;", String.Empty, str(6))
        'SessionHelper.SelectedUserRole = str(1)
        If (str(2) = "&nbsp;") Then
            SessionHelper.ApplicationID = 0
        Else
            SessionHelper.ApplicationID = str(2)
        End If
        If (str(3) = "&nbsp;") Then
            SessionHelper.ApplicationType = String.Empty
        Else
            SessionHelper.ApplicationType = str(3)
        End If
        SessionHelper.Name = str(4) + "," + str(5)
        If (e.CommandName = "RNClick") Then
            Response.Redirect("UpdateExistingPage.aspx")
        End If
    End Sub
    Protected Sub LinkButton_Command1(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim str() As String
        str = e.CommandArgument.ToString().Split("*")
        SessionHelper.SessionUniqueID = str(0)
        'SessionHelper.SelectedUserRole = str(1)
        If (str(2) = "&nbsp;") Then
            SessionHelper.ApplicationID = 0
        Else
            SessionHelper.ApplicationID = str(2)
        End If
        If (str(3) = "&nbsp;") Then
            SessionHelper.ApplicationType = String.Empty
        Else
            SessionHelper.ApplicationType = str(3)
        End If
        SessionHelper.Name = str(4) + "," + str(5)
        If (e.CommandName = "RNClick") Then
            Response.Redirect("UpdateExistingPage.aspx")
        End If
    End Sub
    Protected Sub grdRNSearch_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdRNSearch.RowDataBound
        If (e.Row.DataItem IsNot Nothing) Then
            Dim lb As New LinkButton()
            lb.CommandArgument = e.Row.Cells(0).Text + "*" + e.Row.Cells(11).Text + "*" + e.Row.Cells(12).Text + "*" + e.Row.Cells(14).Text + ";" + e.Row.Cells(1).Text + "*" + e.Row.Cells(2).Text
            lb.Text = e.Row.Cells(0).Text
            lb.CommandName = "RNClick"
            AddHandler lb.Command, AddressOf LinkButton_Command1
            e.Row.Cells(0).Controls.Add(lb)
        End If
    End Sub
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function BindApplicationTypeDropDown() As Collection
        Dim json As New Collection
        json.Add(New With
                     {
                        .ApplicationStatusID = 0,
                        .ApplicationStatusType = "Select One"
                         })
        For Each appStatus In [Enum].GetValues(GetType(Model.Enums.ApplicationStatusType))
            If (appStatus = 1 Or appStatus = 6) Then
                json.Add(New With
                         {
                            .ApplicationStatusID = appStatus,
                            .ApplicationStatusType = EnumHelper.GetEnumDescription(appStatus)
                             })
            End If
        Next
        Return json
    End Function

    Private Sub LinkButton_Command(sender As Object, e As GridViewRowEventArgs)
        Throw New NotImplementedException
    End Sub

    Private Sub LinkButton_Command1(sender As Object, e As GridViewRowEventArgs)
        Throw New NotImplementedException
    End Sub

End Class