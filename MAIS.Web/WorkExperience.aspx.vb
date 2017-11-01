Imports System.Web.Script.Services
Imports MAIS.Business.Model
Imports MAIS.Business.Services
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business
Imports MAIS.Business.Model.Enums
Imports ODMRDDHelperClassLibrary
Imports ODMRDDHelperClassLibrary.ODMRDDServiceProvider
Imports MAIS.Business.Helpers

Public Class WorkExperience
    Inherits System.Web.UI.Page
    Private CurrentExperience As Integer = 0
    Private DDExperienceFLg As Boolean = False
    Private RNExperienceFLg As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        workExpAddr.MandatoryEmailAddress = True
        workExpAddr.MandatoryPhone = True
        workExpAddr.HideCountyMandatory = True
        If Not IsPostBack Then
            pnlWorkGrd.Visible = False
            workExpAddr.LoadStates()
            workExpAddr.LoadCounties()
            If (SessionHelper.ApplicationID > 0 And SessionHelper.RN_Flg = True) Then
                'get RN license issuence date for dates validation for work experience
                GetRNLicenseIssueDate(SessionHelper.ApplicationID)
                LoadCurrentWorkExperience()
            End If
            lblerr.InnerText = String.Empty
            GetExperienceValidation()
        End If
    End Sub
    Private Sub GetExperienceValidation()
        Dim workSvc As IWorkExperienceService = StructureMap.ObjectFactory.GetInstance(Of IWorkExperienceService)()
        CurrentExperience = workSvc.GetExperience(SessionHelper.SessionUniqueID, SessionHelper.ApplicationID)
        If CurrentExperience > 0 Then
            lblTotalExperience.InnerText = "Current RN has " + CurrentExperience.ToString() + " Months Experience"
        Else
            lblTotalExperience.InnerText = "Current RN has " + CurrentExperience.ToString() + " Months Experience"
        End If
        If DDExperienceFLg = True Then
            lblTotalExperience.InnerText = lblTotalExperience.InnerText + " and has DD field experience,"
        Else
            lblTotalExperience.InnerText = lblTotalExperience.InnerText + " and has no DD field experience,"
        End If
        If RNExperienceFLg = True Then
            lblTotalExperience.InnerText = lblTotalExperience.InnerText + " and also has RN field experience."
        Else
            lblTotalExperience.InnerText = lblTotalExperience.InnerText + " and has no RN field experience."
        End If
      
    End Sub
    Private Sub GetRNLicenseIssueDate(ByVal AppId As Integer)
        Dim appSrv As IApplicationDetailInformationService = StructureMap.ObjectFactory.GetInstance(Of IApplicationDetailInformationService)()
        hdRNDate.Value = appSrv.GetRNLicenseIssuenceDateByAppID(AppId)
    End Sub
    Protected Sub LoadCurrentWorkExperience()
        Dim workSvc As IWorkExperienceService = StructureMap.ObjectFactory.GetInstance(Of IWorkExperienceService)()
        Dim listWorkExp As List(Of WorkExperienceDetails) = workSvc.GetAddedWorkExpList(SessionHelper.ApplicationID)
        If listWorkExp IsNot Nothing And listWorkExp.Count > 0 Then
            pnlWorkGrdCurrent.Visible = True
            DisplayWorkExpList(grdCurrent, listWorkExp)           
        Else
            pnlWorkGrdCurrent.Visible = False
        End If
        GetExperienceValidation()
    End Sub
    Protected Sub LoadExistingWorkExperience()
        Dim workSvc As IWorkExperienceService = StructureMap.ObjectFactory.GetInstance(Of IWorkExperienceService)()
        If (SessionHelper.SessionUniqueID IsNot Nothing) Then
            Dim listWorkExp As List(Of WorkExperienceDetails) = workSvc.GetExistingWorkExperience(SessionHelper.SessionUniqueID)
            If listWorkExp IsNot Nothing And listWorkExp.Count > 0 Then
                DisplayWorkExpList(grdWorkHistory, listWorkExp)               
            End If
        End If
    End Sub
    Private Sub DisplayWorkExpList(ByVal displayGrid As GridView, ByVal lstWorkExp As IEnumerable(Of WorkExperienceDetails))
        pnlWorkGrdCurrent.Visible = True
        Dim DDExp = (From ddflg In lstWorkExp
                     Where ddflg.ChkDDFlg = True
                     Select ddflg.ChkDDFlg).ToList()

        Dim RNExp = (From rnflg In lstWorkExp
                     Where rnflg.ChkRNFlg = True
                     Select rnflg.ChkRNFlg).ToList()
        If (RNExp.Count > 0) Then
            RNExperienceFLg = True
        End If

        If DDExp.Count > 0 Then
            DDExperienceFLg = True
        End If
        displayGrid.DataSource = (From E In lstWorkExp
                                  Order By E.EmpStartDate
                                  Select New With {
                                      .workID = E.WorkID,
                                      .EmpName = E.EmpName,
                                      .Title = E.Title,
                                      .EmpStartDate = E.EmpStartDate.Value.ToShortDateString(),
                                      .EmpEndDate = E.EmpEndDate.Value.ToShortDateString(),
                                      .ChkRNFlg = E.ChkRNFlg,
                                      .ChkDDFlg = E.ChkDDFlg
                                      }).ToList()
        displayGrid.DataBind()
    End Sub
    Public Function SaveWorkExperience() As String
        Dim retMsg As String = String.Empty
        Dim retObj As ReturnObject(Of Long)
        Dim _workExpInfo As New WorkExperienceDetails

        Dim workID As Integer = 0
        If (Not String.IsNullOrWhiteSpace(hdWorkID.Value)) Then
            workID = Convert.ToInt32(hdWorkID.Value)
        End If

        If SessionHelper.ApplicationID > 0 Then
            _workExpInfo.WorkID = workID
            _workExpInfo.AppID = SessionHelper.ApplicationID
            _workExpInfo.EmpName = txtEmpName.Value.Trim()
            _workExpInfo.ChkRNFlg = chkRNExp.Checked
            _workExpInfo.ChkDDFlg = chkDDExp.Checked
            _workExpInfo.EmpStartDate = Convert.ToDateTime(txtStartDate.Value.Trim())
            If (Not String.IsNullOrWhiteSpace(txtEndDate.Value.Trim())) Then
                _workExpInfo.EmpEndDate = Convert.ToDateTime(txtEndDate.Value.Trim())
            Else
                _workExpInfo.EmpEndDate = "#12/31/9999#"
            End If
            _workExpInfo.Title = txtDesignation.Value.Trim()
            _workExpInfo.JobDuties = txtRoles.Value.Trim()
            _workExpInfo.Address.AddressLine1 = workExpAddr.AddressLine1.Trim()
            _workExpInfo.Address.AddressLine2 = If(String.IsNullOrWhiteSpace(workExpAddr.AddressLine2.Trim()), String.Empty, workExpAddr.AddressLine2.Trim())
            _workExpInfo.Address.City = workExpAddr.City.Trim()
            _workExpInfo.Address.StateID = workExpAddr.StateID
            _workExpInfo.Address.CountyID = workExpAddr.CountyID
            _workExpInfo.Address.Zip = workExpAddr.Zip.Trim()
            _workExpInfo.Address.ZipPlus = If(String.IsNullOrWhiteSpace(workExpAddr.ZipPlus.Trim()), String.Empty, workExpAddr.ZipPlus.Trim())
            _workExpInfo.Address.AddressType = AddressType.WorkExperience
            _workExpInfo.Address.ContactType = ContactType.WorkExperience

            If (Not String.IsNullOrWhiteSpace(workExpAddr.PhoneNumber.Trim())) Then
                _workExpInfo.Address.Phone = workExpAddr.PhoneNumber.Replace("-", "").Trim()
            Else
                _workExpInfo.Address.Phone = String.Empty
            End If
            If (Not String.IsNullOrWhiteSpace(workExpAddr.Email.Trim())) Then
                _workExpInfo.Address.Email = workExpAddr.Email.Trim()
            Else
                _workExpInfo.Address.Email = String.Empty
            End If

            Dim wprkSvc As IWorkExperienceService = StructureMap.ObjectFactory.GetInstance(Of IWorkExperienceService)()
            retObj = wprkSvc.SaveWorkExperience(_workExpInfo)
            If (retObj.Messages.Count > 0) Then
                retMsg = retObj.Messages(0).ToString()
            End If
        Else
            retMsg = "Invalid AppID"
        End If
        Return retMsg
    End Function

    Private Sub grdCurrent_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdCurrent.RowDeleting
        Dim workID = Convert.ToInt32(grdCurrent.DataKeys(Convert.ToInt32(e.RowIndex)).Value)
        lblerr.InnerText = String.Empty
        If (workID > 0) Then
            Dim wprkSvc As IWorkExperienceService = StructureMap.ObjectFactory.GetInstance(Of IWorkExperienceService)()
            If wprkSvc.DeleteWorkExperienceByID(workID).ReturnValue Then
                lblerr.Attributes.Add("style", "color:green")
                lblerr.InnerText = "Selected work experience is deleted"
                ClearValues()
            Else
                lblerr.Attributes.Add("style", "color:red")
                lblerr.InnerText = "Error deleting selected work experience"
            End If
        Else
            lblerr.Attributes.Add("style", "color:red")
            lblerr.InnerText = "No row selected"
        End If
        LoadCurrentWorkExperience()
    End Sub

    Protected Sub grdCurrent_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdCurrent.SelectedIndexChanged
        Dim weID As Integer = Convert.ToInt32(grdCurrent.SelectedValue)
        hdWorkID.Value = weID
        lblerr.InnerText = String.Empty
        PopulateWorkExperienceInfo(hdWorkID.Value)
    End Sub

    Private Sub PopulateWorkExperienceInfo(WorkID)
        Dim workSvc As IWorkExperienceService = StructureMap.ObjectFactory.GetInstance(Of IWorkExperienceService)()
        Dim workExpInfo As WorkExperienceDetails = workSvc.GetWorkExperienceByID(WorkID)
        If workExpInfo IsNot Nothing Then
            txtEmpName.Value = workExpInfo.EmpName.Trim()
            chkDDExp.Checked = workExpInfo.ChkDDFlg
            chkRNExp.Checked = workExpInfo.ChkRNFlg
            txtStartDate.Value = workExpInfo.EmpStartDate.Value.ToShortDateString()
            txtEndDate.Value = workExpInfo.EmpEndDate.Value.ToShortDateString()
            txtDesignation.Value = workExpInfo.Title.Trim()
            txtRoles.Value = workExpInfo.JobDuties.Trim()
            workExpAddr.AddressLine1 = workExpInfo.Address.AddressLine1.Trim()
            workExpAddr.AddressLine2 = workExpInfo.Address.AddressLine2.Trim()
            workExpAddr.City = workExpInfo.Address.City.Trim()
            workExpAddr.StateID = workExpInfo.Address.StateID
            workExpAddr.CountyID = workExpInfo.Address.CountyID
            workExpAddr.Zip = workExpInfo.Address.Zip.Trim()
            workExpAddr.ZipPlus = workExpInfo.Address.ZipPlus.Trim()
            workExpAddr.PhoneNumber = If(String.IsNullOrWhiteSpace(workExpInfo.Address.Phone), String.Empty, workExpInfo.Address.Phone.Trim())
            workExpAddr.Email = If(String.IsNullOrWhiteSpace(workExpInfo.Address.Email), String.Empty, workExpInfo.Address.Email.Trim())
        End If
    End Sub



    Protected Sub btnSaveExp_Click(sender As Object, e As EventArgs) Handles btnSaveExp.Click

        lblerr.InnerText = String.Empty
        Dim strMsg = SaveWorkExperience()
        If strMsg.Length > 0 Then
            lblerr.Attributes.Add("style", "color:red")
            lblerr.InnerText = "Data is not Saved"

        Else
            lblerr.Attributes.Add("style", "color:green")
            If (Not String.IsNullOrWhiteSpace(hdWorkID.Value)) Then
                lblerr.InnerText = "Data is updated successfully"
            Else
                lblerr.InnerText = "Data is saved successfully"
            End If

            ClearValues()
            LoadCurrentWorkExperience()
        End If


    End Sub
    Private Sub ClearValues()
        txtEmpName.Value = String.Empty
        chkRNExp.Checked = False
        chkDDExp.Checked = False
        txtStartDate.Value = String.Empty
        txtEndDate.Value = String.Empty
        txtDesignation.Value = String.Empty
        txtRoles.Value = String.Empty
        workExpAddr.AddressLine1 = String.Empty
        workExpAddr.AddressLine2 = String.Empty
        workExpAddr.City = String.Empty
        workExpAddr.StateID = 35
        workExpAddr.Zip = String.Empty
        workExpAddr.ZipPlus = String.Empty
        workExpAddr.CountyID = -1
        workExpAddr.PhoneNumber = String.Empty
        workExpAddr.Email = String.Empty
        hdWorkID.Value = String.Empty
    End Sub

    Protected Sub btnAddWe_Click(sender As Object, e As EventArgs) Handles btnAddWe.Click
        ClearValues()
        lblerr.InnerText = String.Empty
    End Sub


    Protected Sub btnWorkHistoryWe_Click(sender As Object, e As EventArgs) Handles btnWorkHistoryWe.Click
        If btnWorkHistoryWe.Text = "Hide History" Then
            btnWorkHistoryWe.Text = "Work Experience History"
            pnlWorkGrd.Visible = False
        Else
            btnWorkHistoryWe.Text = "Hide History"
            pnlWorkGrd.Visible = True
            LoadExistingWorkExperience()
        End If
    End Sub

    Protected Sub btnSaveContinue_Click(sender As Object, e As EventArgs) Handles btnSaveContinue.Click       
        Response.Redirect(PagesHelper.GetNextPage(Master.CurrentPage))
    End Sub

    Protected Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Response.Redirect(PagesHelper.GetPreviousPage(Master.CurrentPage))
    End Sub
End Class




