Imports System.Web.Script.Services
Imports MAIS.Business
Imports MAIS.Business.Helpers
Imports MAIS.Business.Services

Public Class ApplicationHistoryReport
    Inherits System.Web.UI.Page
    '
    Private Property IgnoreFormContraint As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.HideNotationLink = True
        Master.HideProgressBar = True
        Master.HideLink = True
        ScriptManager1.RegisterPostBackControl(Me.btnExport)
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
            json.Add(New With
                     {
                        .ApplicationStatusID = appStatus,
                        .ApplicationStatusType = EnumHelper.GetEnumDescription(appStatus)
                         })
        Next
        Return json
    End Function
    <WebMethod()>
   <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function BindApplicationTypeUsingRoleLevelDropDown(ByVal saveinfo As String) As Collection
        Dim json As New Collection
        If (saveInfo = "0") Then
            json.Add(New With
                         {
                            .ApplicationTypeID = 0,
                            .ApplicationTypeDescription = "Select One"
                             })
            For Each appStatus In [Enum].GetValues(GetType(Model.Enums.RoleLevelCategory))
                If (appStatus <> 15 And appStatus <> 16 And appStatus <> 17 And appStatus <> 14) Then
                    For Each apptype In [Enum].GetValues(GetType(Model.Enums.ApplicationType))
                        'If (apptype <> 4) Then
                        Dim AddApp As Boolean = True
                        Dim increment As Integer = 1
                        If (EnumHelper.GetEnumDescription(apptype) = "Initial") Then
                            Select Case EnumHelper.GetEnumDescription(appStatus)
                                Case "RNInstructor"
                                    AddApp = False
                                Case "RNMaster"
                                    AddApp = False
                            End Select
                        End If


                        If (AddApp) Then
                            json.Add(New With
                                     {
                                        .ApplicationTypeID = increment,
                                        .ApplicationTypeDescription = EnumHelper.GetEnumDescription(appStatus) + "-" + EnumHelper.GetEnumDescription(apptype)
                                         })
                        End If

                        increment = increment + 1
                        'End If
                    Next
                End If
            Next
        Else
            json.Add(New With
                        {
                           .ApplicationTypeID = 0,
                           .ApplicationTypeDescription = "Select One"
                            })
            For Each appStatus In [Enum].GetValues(GetType(Model.Enums.RoleLevelCategory))
                If (appStatus = 15 Or appStatus = 16 Or appStatus = 17) Then
                    For Each apptype In [Enum].GetValues(GetType(Model.Enums.ApplicationType))
                        'If (apptype <> 4) Then
                        Dim AddApp As Boolean = True
                        Dim increment As Integer = 1
                        If (EnumHelper.GetEnumDescription(apptype) = "Initial") Then
                            Select Case EnumHelper.GetEnumDescription(appStatus)
                                Case "DDPersonnelCat2"
                                    AddApp = False
                                Case "DDPersonnelCat3"
                                    AddApp = False
                            End Select
                        End If
                        If (EnumHelper.GetEnumDescription(apptype) = "AddOn") Then
                            Select Case EnumHelper.GetEnumDescription(appStatus)
                                Case "DDPersonnelCat1"
                                    AddApp = False
                            End Select
                        End If
                        If AddApp Then
                            json.Add(New With
                                     {
                                        .ApplicationTypeID = increment,
                                        .ApplicationTypeDescription = EnumHelper.GetEnumDescription(appStatus) + "-" + EnumHelper.GetEnumDescription(apptype)
                                         })
                        End If

                        increment = increment + 1
                        'End If
                    Next
                End If
            Next
        End If
        Return json
    End Function
    <WebMethod()>
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function BindRNNameDropDown() As Collection
        Dim json As New Collection
        Dim UserRNService As IUserRNDetailService = StructureMap.ObjectFactory.GetInstance(Of IUserRNDetailService)()
        Dim ListofRNs As New List(Of Business.Model.RN_UserDetails)
        ListofRNs = UserRNService.getAllRNDetails
        json.Add(New With
                     {
                        .RNSID = 0,
                        .LastFirstname = "Select One"
                         })
        For Each i As Business.Model.RN_UserDetails In ListofRNs
            json.Add(New With
                     {
                        .RNSID = i.RN_Sid,
                        .LastFirstname = i.LastFirstname
                         })
        Next
        Return json
    End Function

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        lblmessage.Visible = True
        grdApplicationDetail.DataSource = Nothing
        grdApplicationDetail.DataBind()
        PopulateGrid()
    End Sub
    Private Sub PopulateGrid()
        Dim appHistorySvc As IApplicationHistoryService = StructureMap.ObjectFactory.GetInstance(Of IApplicationHistoryService)()
        Dim searchModel As New Model.ApplicationHistorySearchCriteria
        Dim listOfResult As New List(Of Model.ApplicationHistoryModel)
        If (hdRNName.Value = "0" Or hdRNName.Value = "") Then
            searchModel.RNID = 0
        Else
            searchModel.RNID = Convert.ToInt32(hdRNName.Value)
        End If
        If (hdApplicationStatus.Value = "0" Or hdApplicationStatus.Value = "") Then
            searchModel.ApplicationStatus = 0
        Else
            searchModel.ApplicationStatus = Convert.ToInt32(hdApplicationStatus.Value)
        End If
        If (String.IsNullOrEmpty(txtStartDate.Value)) Then
            searchModel.StartDate = Nothing
        Else
            searchModel.StartDate = Convert.ToDateTime(txtStartDate.Value)
        End If
        If (String.IsNullOrEmpty(txtEndDate.Value)) Then
            searchModel.EndDate = Nothing
        Else
            searchModel.EndDate = Convert.ToDateTime(txtEndDate.Value)
        End If
        If (hdAppType.Value = "0" Or hdAppType.Value = "") Then
            searchModel.ApplicationTypeID = 0
            searchModel.RoleCategory = 0
        Else
            If (hdAppType.Value.Contains("RNTrainer")) Then
                searchModel.RoleCategory = Model.Enums.RoleLevelCategory.RNTrainer_RLC
            ElseIf (hdAppType.Value.Contains("RNInstructor")) Then
                searchModel.RoleCategory = Model.Enums.RoleLevelCategory.RNInstructor_RLC
            ElseIf (hdAppType.Value.Contains("RNMaster")) Then
                searchModel.RoleCategory = Model.Enums.RoleLevelCategory.RNMaster_RLC
            ElseIf (hdAppType.Value.Contains("17Bed")) Then
                searchModel.RoleCategory = Model.Enums.RoleLevelCategory.Bed17_RLC
            ElseIf (hdAppType.Value.Contains("QA")) Then
                searchModel.RoleCategory = Model.Enums.RoleLevelCategory.QARN_RLC
            ElseIf (hdAppType.Value.Contains("DDPersonnelCat1")) Then
                searchModel.RoleCategory = Model.Enums.RoleLevelCategory.DDPersonnel_RLC
            ElseIf (hdAppType.Value.Contains("DDPersonnelCat2")) Then
                searchModel.RoleCategory = Model.Enums.RoleLevelCategory.DDPersonnel2_RLC
            ElseIf (hdAppType.Value.Contains("DDPersonnelCat3")) Then
                searchModel.RoleCategory = Model.Enums.RoleLevelCategory.DDPersonnel3_RLC
            End If
            If (hdAppType.Value.Contains("Initial")) Then
                searchModel.ApplicationTypeID = Model.Enums.ApplicationType.Initial
            ElseIf (hdAppType.Value.Contains("Renewal")) Then
                searchModel.ApplicationTypeID = Model.Enums.ApplicationType.Renewal
            ElseIf (hdAppType.Value.Contains("AddOn")) Then
                searchModel.ApplicationTypeID = Model.Enums.ApplicationType.AddOn
            End If
        End If
        If (rblSelect.SelectedValue = "0") Then
            searchModel.RNDDFlag = True
            searchModel.RNLicenseNumber = txtRNDDLicSSN.Value
        Else
            searchModel.RNDDFlag = False
            searchModel.Last4SSN = txtRNDDLicSSN.Value
            searchModel.DDpersonnelCode = txtDDPersoonelCode.Value
        End If
        listOfResult = appHistorySvc.GetSearchResults(searchModel)
        If (listOfResult IsNot Nothing) Then
            If (listOfResult.Count > 0) Then
                grdApplicationDetail.DataSource = listOfResult
                '    (From p In listOfResult Order By p.ApplicationID
                '                  Select New With {
                '                      .ApplicationID = p.ApplicationID,
                '                      .ApplicationType = Trim(p.ApplicationType),
                '                      .FinalApplicationStatus = Trim(p.FinalApplicationStatus),
                '                      .DecisionMadeRNName = IIf(p.DecisionMadeRNName Is Nothing, String.Empty, Trim(p.DecisionMadeRNName)),
                '                      .TrainingEndDate = If(p.TrainingEndDate.HasValue, Trim(p.TrainingEndDate.ToString()), String.Empty),
                '                      .SkillsEndDate = If(p.SkillsEndDate.HasValue, Trim(p.SkillsEndDate.ToString()), String.Empty),
                '                      .AttestationDate = If(p.AttestationDate.HasValue, Trim(p.AttestationDate.ToString()), String.Empty),
                '                      .EmailEndDate = If(p.EmailEndDate.HasValue, Trim(p.EmailEndDate.ToString()), String.Empty),
                '                      .CertificatePrintDate = If(p.CertificatePrintDate.HasValue, Trim(p.CertificatePrintDate.ToString()), String.Empty),
                '.FinalDecisionName = Trim(p.FinalDecisionName)
                '                      }).ToList()
                grdApplicationDetail.DataBind()
                lblmessage.Text = "Total Number of Applications are:" + " " + Convert.ToString(listOfResult.Count)
            Else
                'lblmessage.ForeColor = Drawing.Color.Red
                lblmessage.Text = "No data found for this search criteria"
                grdApplicationDetail.DataSource = Nothing
                grdApplicationDetail.DataBind()
            End If
        Else
            'lblmessage.ForeColor = Drawing.Color.Red
            lblmessage.Text = "No data found for this search criteria"
            grdApplicationDetail.DataSource = Nothing
            grdApplicationDetail.DataBind()
        End If
    End Sub

    Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdApplicationDetail.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim appId As String = grdApplicationDetail.DataKeys(e.Row.RowIndex).Value.ToString()
            Dim grdApplicationStatusDetail As GridView = TryCast(e.Row.FindControl("grdApplicationStatusDetail"), GridView)
            grdApplicationStatusDetail.DataSource = CType(e.Row.DataItem, Model.ApplicationHistoryModel).ListOfApplicationDetail
            grdApplicationStatusDetail.DataBind()
        End If
    End Sub
    Protected Sub btnExport_click() Handles btnExport.ServerClick
        IgnoreFormContraint = True
        PopulateGrid()
        ExcelHelper.CreateExcel(Response, grdApplicationDetail, "Application_History_Report")
        IgnoreFormContraint = False
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As System.Web.UI.Control)

        ' So the Gridview renders properly for the Excel
        If Not IgnoreFormContraint Then
            MyBase.VerifyRenderingInServerForm(control)
        End If
    End Sub
End Class