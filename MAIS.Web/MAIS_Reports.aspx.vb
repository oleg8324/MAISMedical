Imports System.Web.Script.Services
Imports MAIS.Business
Imports MAIS.Business.Helpers
Imports MAIS.Business.Services
Imports MAIS.Business.Model
Imports MAIS.Business.Model.Enums
Imports MAIS.Data
Imports System.IO
Public Class MAIS_Reports
    Inherits System.Web.UI.Page
    Private Shared NotationSvc As INotationService
    Private Property IgnoreFormContraint As Boolean = False
    Private param As New SearchParameters
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load       
        If Not IsPostBack Then
            SessionHelper.SessionUniqueID = String.Empty
            Master.HideLink = True
            Master.HideProgressBar = True
            SessionHelper.Notation_Flg = False
            pnlNotationOptions.Visible = False
            pnlEmployerSupervisor.Visible = False
            pnlRNDDSearchOptions.Visible = False
            pnlNotationSearch.Visible = False
            divButtons.Visible = False
            If ((UserAndRoleHelper.IsUserAdmin) OrElse (SessionHelper.MAISLevelUserRole = Enums.RoleLevelCategory.RNMaster_RLC)) Then
                rblSelect.Items(2).Enabled = True
                chkAll.Enabled = True
            Else
                rblSelect.Items(2).Enabled = False
                chkAll.Enabled = False
            End If
        End If
    End Sub
    Private Sub LoadCourse(Optional ByVal rn_Sid As Integer = 0)
        Dim reportSvc As IMAISReportService = StructureMap.ObjectFactory.GetInstance(Of IMAISReportService)()
        Dim lstCoursesAssign As List(Of Model.Course_Info)
        ddlCourses.Items.Clear()
        If (rn_Sid > 0) Then
            Dim lstCourses As List(Of Model.Course_Info) = reportSvc.GetAllCourseByRNID(rn_Sid)
            If (rblSelect.SelectedValue = "1") Then
                lstCoursesAssign = (From c In lstCourses
                                    Where Not c.Course_Number.Contains("DODD")
                                    Select c).ToList()
            Else
                lstCoursesAssign = (From c In lstCourses
                                   Where c.Course_Number.Contains("DODD")
                                   Select c).ToList()
            End If
            ddlCourses.Items.Add(New ListItem("--- Select One ---", "0"))
            ddlCourses.DataSource = lstCoursesAssign
            ddlCourses.DataTextField = "Course_Trainer_Dropdown"
            ddlCourses.DataValueField = "Course_Sid"
            ddlCourses.DataBind()
        Else
            Dim lstCourses As List(Of Model.Course_Info) = reportSvc.GetAllCourses()
            If (rblSelect.SelectedValue = "1") Then
                lstCoursesAssign = (From c In lstCourses
                                    Where Not c.Course_Number.Contains("DODD")
                                    Select c).ToList()
            Else
                lstCoursesAssign = (From c In lstCourses
                                   Where c.Course_Number.Contains("DODD")
                                   Select c).ToList()
            End If
            ddlCourses.Items.Add(New ListItem("--- Select One ---", "0"))
            ddlCourses.DataSource = lstCoursesAssign
            ddlCourses.DataTextField = "Course_Trainer_Dropdown"
            ddlCourses.DataValueField = "Course_Sid"
            ddlCourses.DataBind()
        End If

    End Sub

    Private Sub LoadRNTrainers(ByVal ddlList As DropDownList)
        Dim UserRNService As IUserRNDetailService = StructureMap.ObjectFactory.GetInstance(Of IUserRNDetailService)()
        Dim ListofRNs As New List(Of Business.Model.RN_UserDetails)
        ListofRNs = UserRNService.getAllRNDetails

        ddlList.Items.Clear()
        ddlList.Items.Add(New ListItem("--Select RN--", "0"))
        ddlList.DataSource = ListofRNs
        ddlList.DataTextField = "LastFirstname"
        ddlList.DataValueField = "RN_Sid"
        ddlList.DataBind()
        'For Each i As Business.Model.RN_UserDetails In ListofRNs
        '    ddlList.Items.Add(New ListItem(i.LastFirstname, i.RN_Sid))
        'Next
    End Sub
    Private Sub LoadCertificationTypes()
        ddlCertTypes.Items.Clear()
        ddlCertTypes.Items.Add(New ListItem("--- Select One ---", "0"))
        For Each RCL In [Enum].GetValues(GetType(Model.Enums.RoleLevelCategory))   ' 14 is secretary
            If ((rblSelect.SelectedValue = "1") And (RCL < "14")) Then
                ddlCertTypes.Items.Add(New ListItem(EnumHelper.GetEnumDescription(RCL), RCL))
            ElseIf ((rblSelect.SelectedValue = "2") And (RCL > "14")) Then
                ddlCertTypes.Items.Add(New ListItem(EnumHelper.GetEnumDescription(RCL), RCL))
            End If
        Next
    End Sub
    Private Sub GetParameters()
        If Not String.IsNullOrWhiteSpace(txt4SSN.Value.Trim()) Then
            param.Last4SSN = CInt(txt4SSN.Value.Trim())
        Else
            param.Last4SSN = 0
        End If
        If Not String.IsNullOrWhiteSpace(txtRNDDLicDDCode.Value.Trim()) Then
            param.Licence_Code = txtRNDDLicDDCode.Value.Trim()
        Else
            param.Licence_Code = String.Empty
        End If
        If Not String.IsNullOrWhiteSpace(txtFName.Value.Trim()) Then
            param.FirstName = txtFName.Value.Trim()
        Else
            param.FirstName = String.Empty
        End If
        If Not String.IsNullOrWhiteSpace(txtLName.Value.Trim()) Then
            param.LastName = txtLName.Value.Trim()
        Else
            param.LastName = String.Empty
        End If
        If Not String.IsNullOrWhiteSpace(txtEmpName.Value.Trim()) Then
            param.EmployerName = txtEmpName.Value.Trim()
        Else
            param.EmployerName = String.Empty
        End If
        If Not String.IsNullOrWhiteSpace(txtSupFirst.Value.Trim()) Then
            param.SupFirstName = txtSupFirst.Value.Trim()
        Else
            param.SupFirstName = String.Empty
        End If
        If Not String.IsNullOrWhiteSpace(txtSupLast.Value.Trim()) Then
            param.SupLastName = txtSupLast.Value.Trim()
        Else
            param.SupLastName = String.Empty
        End If
        If Not String.IsNullOrWhiteSpace(txtCEOFirst.Value.Trim()) Then
            param.CEOFirstName = txtCEOFirst.Value.Trim
        Else
            param.CEOFirstName = String.Empty
        End If
        If Not String.IsNullOrWhiteSpace(txtCEOLast.Value.Trim()) Then
            param.CEOLastName = txtCEOLast.Value.Trim
        Else
            param.CEOLastName = String.Empty
        End If
        If (ddlCertTypes.SelectedValue <> "0") Then
            param.Role_Level_Cat_Sid = CInt(ddlCertTypes.SelectedValue)
        Else
            param.Role_Level_Cat_Sid = 0
        End If
        If (ddlCertStatus.SelectedValue <> "0") Then
            param.Cert_Status_Type_Sid = CInt(ddlCertStatus.SelectedValue)
        Else
            param.Cert_Status_Type_Sid = 0
        End If
        If (ddlRNTrainer.SelectedValue <> "0") Then
            param.Trainer_RN_Sid = CInt(ddlRNTrainer.SelectedValue)
        Else
            param.Trainer_RN_Sid = 0
        End If
        If (ddlCourses.SelectedValue <> "0") Then
            param.Course_Sid = CInt(ddlCourses.SelectedValue)
            If (ddlSessions.SelectedValue <> "0") Then
                param.Session_sid = CInt(ddlSessions.SelectedValue)
            Else
                param.Session_sid = 0
            End If
        Else
            param.Course_Sid = 0
        End If
        If Not (String.IsNullOrWhiteSpace(txtDateFrom.Value.Trim())) Then
            '    param.ExpDateFrom = DateTime.Parse(String.Empty)
            'Else
            param.ExpDateFrom = DateTime.Parse(txtDateFrom.Value.Trim())
        End If
        If Not (String.IsNullOrWhiteSpace(txtDateTo.Value.Trim())) Then
            '    param.ExpDateTo = DateTime.Parse(String.Empty)
            'Else
            param.ExpDateTo = DateTime.Parse(txtDateTo.Value.Trim())
        End If

        Select Case rblExpWithIn.SelectedValue
            Case "1"
                param.ExpWithinLast30Days = True
                param.ExpWithinLast60Days = False
                param.ExpWithinLast90Days = False
                param.ExpWithinLast180Days = False
            Case "2"
                param.ExpWithinLast30Days = False
                param.ExpWithinLast60Days = True
                param.ExpWithinLast90Days = False
                param.ExpWithinLast180Days = False
            Case "3"
                param.ExpWithinLast30Days = False
                param.ExpWithinLast60Days = False
                param.ExpWithinLast90Days = True
                param.ExpWithinLast180Days = False
            Case "4"
                param.ExpWithinLast30Days = False
                param.ExpWithinLast60Days = False
                param.ExpWithinLast90Days = False
                param.ExpWithinLast180Days = True
            Case Else
                param.ExpWithinLast30Days = False
                param.ExpWithinLast60Days = False
                param.ExpWithinLast90Days = False
                param.ExpWithinLast180Days = False
        End Select

        param.AdminFlg_GettAllRecords = chkAll.Checked
       
    End Sub
    Private Sub LoadCertificationStatus()
        Dim reportSvc As IMAISReportService = StructureMap.ObjectFactory.GetInstance(Of IMAISReportService)()
        Dim lstStatus As List(Of Model.ReportCertificationStatus) = reportSvc.GetCertificationStaus()
        ddlCertStatus.Items.Add(New ListItem("--- Select One ---", "0"))
        ddlCertStatus.DataSource = lstStatus
        ddlCertStatus.DataTextField = "StatusCode"
        ddlCertStatus.DataValueField = "StatusID"
        ddlCertStatus.DataBind()
    End Sub
    Private Sub LoadNotationType()
        Dim NTypesList As List(Of Business.Model.NType)
        NotationSvc = StructureMap.ObjectFactory.GetInstance(Of INotationService)()
        NTypesList = NotationSvc.GetNotationTypes()
        ddlNotationType.Items.Add(New ListItem("--- Select One ---", "0"))
        ddlNotationType.DataSource = NTypesList
        ddlNotationType.DataValueField = "NTypeSid"
        ddlNotationType.DataTextField = "NTypeDesc"
        ddlNotationType.DataBind()

    End Sub
    Private Sub LoadNotationReason()
        Dim NReasonsList As List(Of Business.Model.NReason)
        NotationSvc = StructureMap.ObjectFactory.GetInstance(Of INotationService)()
        NReasonsList = NotationSvc.GetNotationReasons
        ddlNotationReason.Items.Add(New ListItem("--- Select One ---", "0"))
        ddlNotationReason.DataSource = NReasonsList
        ddlNotationReason.DataTextField = "NReasonDesc"
        ddlNotationReason.DataValueField = "NReasonSid"
        ddlNotationReason.DataBind()
    End Sub
    Protected Sub btnRun_click() Handles btnRun.ServerClick
        lblErrorMsg.Text = String.Empty
        grdDDList.DataSource = Nothing
        grdDDList.DataBind()
        grdRNList.DataSource = Nothing
        grdRNList.DataBind()
        grdDDSearch.DataSource = Nothing
        grdDDSearch.DataBind()
        grdRNSearch.DataSource = Nothing
        grdRNSearch.DataBind()
        grdEmployerList.DataSource = Nothing
        grdEmployerList.DataBind()
        grdSupervisorList.DataSource = Nothing
        grdSupervisorList.DataBind()

        If (rblSelect.SelectedValue = "1") Then 'RN Report        
            If (chkAll.Checked = True) Then
                LoadALLRNs()
            Else
                If ValidateParameters() Then
                    lblErrorMsg.Text = "Please select atleast one of the report search options."
                    Return
                Else
                    lblErrorMsg.Text = String.Empty
                    GetParameters()
                    LoadRNSearch()
                End If

            End If
        ElseIf (rblSelect.SelectedValue = "2") Then 'DD personnel Report

            If (chkAll.Checked = True) Then
                LoadALLDDs()
            Else
                If ValidateParameters() Then
                    lblErrorMsg.Text = "Please select atleast one of the report search options."
                    Return
                Else
                    lblErrorMsg.Text = String.Empty
                    GetParameters()
                    LoadDDSearch()
                End If
            End If
        ElseIf (rblSelect.SelectedValue = "3") Then 'Notation Report            
            If (rblNotation.SelectedValue = "1") Then
                LoadRNNotaions()
            ElseIf (rblNotation.SelectedValue = "2") Then
                LoadDDPersonnelNotations()
            Else
                LoadRNNotaions()
                LoadDDPersonnelNotations()
            End If
        ElseIf (rblSelect.SelectedValue = "4") Then 'Employer/CEO report
            If (rblEmpSup.SelectedValue = "1") Then 'RN employer
                LoadEmployerList(CInt(ddlRDDDList.SelectedValue), Trim(txtEmployerName.Text))
            ElseIf (rblEmpSup.SelectedValue = "2") Then 'DD personnel employers
                LoadEmployerList(CInt(ddlRDDDList.SelectedValue), Trim(txtEmployerName.Text))
            Else
                LoadEmployerList()
            End If
        ElseIf (rblSelect.SelectedValue = "5") Then 'Supervisor Report
            If (rblEmpSup.SelectedValue = "1") Then 'RN employer
                LoadSupervisors(CInt(ddlRDDDList.SelectedValue), Trim(txtEmployerName.Text), Trim(txtSupLastName.Text))
            ElseIf (rblEmpSup.SelectedValue = "2") Then 'DD personnel employers
                LoadSupervisors(CInt(ddlRDDDList.SelectedValue), Trim(txtEmployerName.Text), Trim(txtSupLastName.Text))
            Else
                LoadSupervisors()
            End If
        Else

        End If
    End Sub
    Private Sub LoadDDSearch()
        Try
            Dim reportSVC As IMAISReportService = StructureMap.ObjectFactory.GetInstance(Of IMAISReportService)()
            If (param IsNot Nothing) Then
                lblErrorMsg.Text = String.Empty
                Dim ddlist As List(Of MAIS_Report) = reportSVC.GetDDSearchReport(param)
                LoadDDGrid(ddlist)
            Else
                lblErrorMsg.Text = "No parameters set"
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub LoadRNSearch()
        Try
            Dim reportSVC As IMAISReportService = StructureMap.ObjectFactory.GetInstance(Of IMAISReportService)()
            If (param IsNot Nothing) Then
                lblErrorMsg.Text = String.Empty
                Dim rnlist As List(Of MAIS_Report) = reportSVC.GetRNSearchReport(param)
                LoadRNGrid(rnlist)
            Else
                lblErrorMsg.Text = "No parameters set"
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub LoadALLDDs()
        Dim reportSVC As IMAISReportService = StructureMap.ObjectFactory.GetInstance(Of IMAISReportService)()
        Dim ddList As List(Of MAIS_Report) = reportSVC.GetALLDDs()
        LoadDDGrid(ddList)

    End Sub

    Private Sub LoadDDGrid(ByVal ddList As List(Of MAIS_Report))
        If (ddList.Count > 0) Then
            grdDDList.DataSource = (From dd In ddList
                                    Order By dd.LastName
                                    Select dd).ToList()
            grdDDList.DataBind()
            lblErrorMsg.Text = String.Empty
            lblDDCount.Text = "Total DDPersonnels are : " + ddList.Count.ToString()
            If (ddList.Count > 250) Then
                lblErrorMsg.Text = "Search results exceed 250 records. Enter additional search criteria to narrow the search or if you require all records please contact MA."
            End If
        Else
            grdDDList.DataSource = Nothing
            grdDDList.DataBind()
            lblErrorMsg.Text = "No search results found for DDPersonnel"
            lblDDCount.Text = String.Empty
        End If
    End Sub
    Private Sub LoadRNGrid(ByVal RNList As List(Of MAIS_Report))
        If RNList.Count > 0 Then
            grdRNList.DataSource = (From rr In RNList
                                    Order By rr.LastName
                                    Select rr).ToList()
            grdRNList.DataBind()
            lblRNCount.Text = "Total RN's are : " + RNList.Count.ToString()
            lblErrorMsg.Text = String.Empty
            If (RNList.Count > 250) Then
                lblErrorMsg.Text = "Search results exceeded 250 records. Enter additional search criteria to narrow the search."
            End If
        Else
            grdRNList.DataSource = Nothing
            grdRNList.DataBind()
            lblRNCount.Text = String.Empty
            lblErrorMsg.Text = "No search results found for RN"
        End If
    End Sub
    Private Sub LoadALLRNs()
        Dim reportSVC As IMAISReportService = StructureMap.ObjectFactory.GetInstance(Of IMAISReportService)()
        Dim RNList As List(Of MAIS_Report) = reportSVC.GetALLRNs()                                          
        LoadRNGrid(RNList)
    End Sub
    Private Sub LoadDDPersonnelNotations()
        Dim reportSVC As IMAISReportService = StructureMap.ObjectFactory.GetInstance(Of IMAISReportService)()
        Dim ddList As List(Of DDPersonnelSearchResult) = reportSVC.GetDDNotations(CInt(ddlNotationType.SelectedValue), CInt(ddlNotationReason.SelectedValue), If(String.IsNullOrWhiteSpace(txtStartDateOccurance.Value), "12/31/9999", Convert.ToDateTime(txtStartDateOccurance.Value)), If(String.IsNullOrWhiteSpace(txtEndDateOccurance.Value), "12/31/9999", Convert.ToDateTime(txtEndDateOccurance.Value)))
        If (ddList.Count > 0) Then
            grdDDSearch.DataSource = (From d In ddList
                                      Order By d.LastName
                                      Select d).ToList()
            grdDDSearch.DataBind()
            lblErrorMsg.Text = String.Empty
            lblDDCount.Text = "Total DDPersonnels are : " + ddList.Count.ToString()
        Else
            grdDDSearch.DataSource = Nothing
            grdDDSearch.DataBind()
            lblErrorMsg.Text = "No search results found for DDPersonnel"
            lblDDCount.Text = String.Empty
        End If
    End Sub
    Private Sub LoadRNNotaions()
        Dim reportSVC As IMAISReportService = StructureMap.ObjectFactory.GetInstance(Of IMAISReportService)()
        Dim RNList As List(Of RNSearchResult) = reportSVC.GetRNNotations(CInt(ddlNotationType.SelectedValue), CInt(ddlNotationReason.SelectedValue), If(String.IsNullOrWhiteSpace(txtStartDateOccurance.Value), "12/31/9999", Convert.ToDateTime(txtStartDateOccurance.Value)), If(String.IsNullOrWhiteSpace(txtEndDateOccurance.Value), "12/31/9999", Convert.ToDateTime(txtEndDateOccurance.Value)))
        If RNList.Count > 0 Then
            grdRNSearch.DataSource = (From rrr In RNList
                                      Order By rrr.LastName
                                      Select rrr).ToList()
            grdRNSearch.DataBind()
            lblRNCount.Text = "Total RN's are : " + RNList.Count.ToString()
            lblErrorMsg.Text = String.Empty
        Else
            grdRNSearch.DataSource = Nothing
            grdRNSearch.DataBind()
            lblRNCount.Text = String.Empty
            lblErrorMsg.Text = "No search results found for RN"
        End If
    End Sub
    Private Sub LoadSupervisors(Optional ByVal ID As Integer = 0, Optional ByVal FirstName As String = "", Optional ByVal LastName As String = "")
        Dim reportSVC As IMAISReportService = StructureMap.ObjectFactory.GetInstance(Of IMAISReportService)()
        Dim retList As List(Of SupervisorDetails) = reportSVC.GetSupervisorList(ID, FirstName, LastName, rblEmpSup.SelectedValue)
        If (retList.Count > 0) Then
            grdSupervisorList.DataSource = (From ss In retList
                                            Order By ss.supLastName
                                            Select New With {
                                                    .supFirstName = ss.supFirstName,
                                                    .supLastName = ss.supLastName,
                                                    .EmailAddress = ss.EmailAddress,
                                                    .supEndDate = ss.supEndDate.ToShortDateString(),
                                                    .supStartDate = ss.supStartDate.ToShortDateString(),
                                                    .PhoneNumber = ss.PhoneNumber
                                                }).ToList()
            grdSupervisorList.DataBind()
            lblErrorMsg.Text = String.Empty
            lblCount.Text = "Total Supervisors are : " + retList.Count.ToString()
        Else
            grdSupervisorList.DataSource = Nothing
            grdSupervisorList.DataBind()
            lblErrorMsg.Text = "No Employers Found"
            lblCount.Text = String.Empty
        End If
    End Sub
    Private Sub LoadEmployerList(Optional ByVal ID As Integer = 0, Optional ByVal EmpName As String = "")
        Dim reportSVC As IMAISReportService = StructureMap.ObjectFactory.GetInstance(Of IMAISReportService)()
        Dim retList As List(Of EmployerDetails) = reportSVC.GetEmployersList(ID, EmpName, rblEmpSup.SelectedValue)
        If retList.Count > 0 Then
            grdEmployerList.DataSource = (From ee In retList
                                          Order By ee.EmployerName
                                          Select New With {
                                                .CEOFirstName = ee.CEOFirstName,
                                                .CEOLastName = ee.CEOLastName,
                                                .EmailAddress = ee.EmailAddress,
                                                .EmpEndDate = ee.EmpEndDate.ToShortDateString(),
                                                .EmployerName = ee.EmployerName,
                                                .EmpStartDate = ee.EmpStartDate.ToShortDateString(),
                                                .IdentificationValue = ee.IdentificationValue,
                                                .PhoneNumber = ee.PhoneNumber
                                              }).ToList()
            grdEmployerList.DataBind()
            lblErrorMsg.Text = String.Empty
            lblCount.Text = "Total Employers are : " + retList.Count.ToString()
        Else
            grdEmployerList.DataSource = Nothing
            grdEmployerList.DataBind()
            lblErrorMsg.Text = "No Employers Found"
            lblCount.Text = String.Empty
        End If
    End Sub
    Protected Sub btnExport_click() Handles btnExport.ServerClick
        lblErrorMsg.Text = String.Empty
        IgnoreFormContraint = True
        divSpinner.Style("display") = "none"
        If (rblSelect.SelectedValue = "1") Then 'RN Report
            Dim reportSVC As IMAISReportService = StructureMap.ObjectFactory.GetInstance(Of IMAISReportService)()
            If (chkAll.Checked = True) Then
                Dim RNList As List(Of MAISReportDetails) = reportSVC.GetALLRNsForExcel()
                If (RNList.Count > 0) Then
                    Dim grd1 As New GridView
                    grd1.DataSource = (From rnn In RNList
                                       Order By rnn.Last_Name
                                       Select rnn).ToList()
                    grd1.DataBind()
                    ExcelHelper.CreateExcel(Response, grd1, "MyMAISReport")
                End If
            Else
                If (ValidateParameters()) Then
                    lblErrorMsg.Text = "Please select atleast one of the report search options."
                Else
                    GetParameters()
                    lblErrorMsg.Text = String.Empty
                    Dim rnlist As List(Of MAISReportDetails) = reportSVC.GetRNSearchReportForExcel(param)
                    If (rnlist.Count > 0) Then
                        Dim grd As New GridView
                        grd.DataSource = (From r In rnlist
                                          Order By r.Last_Name
                                          Select r).ToList()
                        grd.DataBind()
                        ExcelHelper.CreateExcel(Response, grd, "MyMAISReport")
                    End If
                End If

            End If
        ElseIf (rblSelect.SelectedValue = "2") Then 'DD personnel Report
            Dim reportSVC As IMAISReportService = StructureMap.ObjectFactory.GetInstance(Of IMAISReportService)()
            If (chkAll.Checked = True) Then
                Dim DDList As List(Of MAISReportDetails) = reportSVC.GetALLDDsForExcel()
                If (DDList.Count > 0) Then
                    Dim grddd As New GridView
                    grddd.DataSource = (From dl In DDList
                                        Order By dl.Last_Name
                                        Select dl).ToList()
                    grddd.DataBind()
                    ExcelHelper.CreateExcel(Response, grddd, "MyMAISReport")
                End If
            Else
                If (ValidateParameters()) Then
                    lblErrorMsg.Text = "Please select atleast one of the report search options."
                Else
                    GetParameters()
                    lblErrorMsg.Text = String.Empty
                    Dim ddlist As List(Of MAISReportDetails) = reportSVC.GetDDSearchReportForExcel(param)
                    If (ddlist.Count > 0) Then
                        Dim grdd As New GridView
                        grdd.DataSource = (From ddl In ddlist
                                           Order By ddl.Last_Name
                                           Select ddl).ToList()
                        grdd.DataBind()
                        ExcelHelper.CreateExcel(Response, grdd, "MyMAISReport")
                    End If
                End If
            End If
        ElseIf (rblSelect.SelectedValue = "3") Then 'Notation Report
            If (rblNotation.SelectedValue = "1") Then
                LoadRNNotaions()
                ExcelHelper.CreateExcel(Response, grdRNSearch, "MyMAISReport")
            ElseIf (rblNotation.SelectedValue = "2") Then
                LoadDDPersonnelNotations()
                ExcelHelper.CreateExcel(Response, grdDDSearch, "MyMAISReport")
            Else
                LoadRNNotaions()
                LoadDDPersonnelNotations()
                ExcelHelper.CreateExcel(Response, grdRNSearch, "MyMAISReport")
                ExcelHelper.CreateExcel(Response, grdDDSearch, "MyMAISReport")
            End If
        ElseIf (rblSelect.SelectedValue = "4") Then 'Employer/CEO report
            If (rblEmpSup.SelectedValue = "1") Then 'RN employer
                LoadEmployerList(CInt(ddlRDDDList.SelectedValue), Trim(txtEmployerName.Text))
            ElseIf (rblEmpSup.SelectedValue = "2") Then 'DD personnel employers
                LoadEmployerList(CInt(ddlRDDDList.SelectedValue), Trim(txtEmployerName.Text))
            Else
                LoadEmployerList()
            End If
            ExcelHelper.CreateExcel(Response, grdEmployerList, "MyMAISReport")
        ElseIf (rblSelect.SelectedValue = "5") Then 'Supervisor Report
            If (rblEmpSup.SelectedValue = "1") Then 'RN employer
                LoadSupervisors(CInt(ddlRDDDList.SelectedValue), Trim(txtEmployerName.Text), Trim(txtSupLastName.Text))
            ElseIf (rblEmpSup.SelectedValue = "2") Then 'DD personnel employers
                LoadSupervisors(CInt(ddlRDDDList.SelectedValue), Trim(txtEmployerName.Text), Trim(txtSupLastName.Text))
            Else
                LoadSupervisors()
            End If
            ExcelHelper.CreateExcel(Response, grdSupervisorList, "MyMAISReport")
        End If
        IgnoreFormContraint = False

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As System.Web.UI.Control)

        ' So the Gridview renders properly for the Excel
        If Not IgnoreFormContraint Then
            MyBase.VerifyRenderingInServerForm(control)
        End If
    End Sub
    Protected Sub btnPrint_click() Handles btnPrint.ServerClick
        lblErrorMsg.Text = String.Empty
        divSpinner.Style("display") = "none"
        If (rblSelect.SelectedValue = "1") Then 'RN Report
            If (chkAll.Checked = True) Then
                LoadALLRNs()
            Else
                If (ValidateParameters()) Then
                    lblErrorMsg.Text = "Please select atleast one of the report search options."
                Else
                    GetParameters()
                    LoadRNSearch()
                    lblErrorMsg.Text = String.Empty
                End If
            End If
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "PrintGridData('6')", True)
        ElseIf (rblSelect.SelectedValue = "2") Then 'DD personnel Report
            If (chkAll.Checked = True) Then
                LoadALLDDs()
            Else
                If (ValidateParameters()) Then
                    lblErrorMsg.Text = "Please select atleast one of the report search options."
                Else
                    lblErrorMsg.Text = String.Empty
                    GetParameters()
                    LoadDDSearch()
                End If
            End If
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "PrintGridData('3')", True)
        ElseIf (rblSelect.SelectedValue = "3") Then 'Notation Report
            If (rblNotation.SelectedValue = "1") Then
                LoadRNNotaions()
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "PrintGridData('1')", True)
            ElseIf (rblNotation.SelectedValue = "2") Then
                LoadDDPersonnelNotations()
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "PrintGridData('2')", True)
            Else
                LoadRNNotaions()
                LoadDDPersonnelNotations()
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "PrintGridData('1')", True)
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "PrintGridData('2')", True)
            End If
        ElseIf (rblSelect.SelectedValue = "4") Then 'Employer/CEO report
            If (rblEmpSup.SelectedValue = "1") Then 'RN employer
                LoadEmployerList(CInt(ddlRDDDList.SelectedValue), Trim(txtEmployerName.Text))
            ElseIf (rblEmpSup.SelectedValue = "2") Then 'DD personnel employers
                LoadEmployerList(CInt(ddlRDDDList.SelectedValue), Trim(txtEmployerName.Text))
            Else
                LoadEmployerList()
            End If
            ' Dim str As String = "PrintGridData(" + grdSupervisorList.ClientID + ");"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "PrintGridData('4')", True)
        ElseIf (rblSelect.SelectedValue = "5") Then 'Supervisor Report
            LoadSupervisors()
            ' Dim str As String = "PrintGridData(" + grdSupervisorList.ClientID + ");"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "PrintGridData('5')", True)
        Else
            Return
        End If

    End Sub
    Private Sub rblSelect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblSelect.SelectedIndexChanged
        lblCount.Text = String.Empty
        lblDDCount.Text = String.Empty
        lblRNCount.Text = String.Empty
        lblErrorMsg.Text = String.Empty
        txtEmployerName.Text = String.Empty
        grdRNList.DataSource = Nothing
        grdRNList.DataBind()
        grdDDList.DataSource = Nothing
        grdDDList.DataBind()
        grdDDSearch.DataSource = Nothing
        grdDDSearch.DataBind()
        grdRNSearch.DataSource = Nothing
        grdRNSearch.DataBind()
        grdEmployerList.DataSource = Nothing
        grdEmployerList.DataBind()
        grdSupervisorList.DataSource = Nothing
        grdSupervisorList.DataBind()
        If (rblSelect.SelectedValue = "1") Then
            ClearFields()
            rblEmpSup.SelectedIndex = -1
            pnlNotationOptions.Visible = False
            pnlRNDDSearchOptions.Visible = True
            pnlEmployerSupervisor.Visible = False
            divButtons.Visible = True
            lbl4SSN.Enabled = False
            txt4SSN.Visible = False
            lbl4SSN.Visible = False
            chkAll.Text = "Search All RN's"
            lblRNDDLicenseDDCode.Text = "RN License No.:"
            LoadCertificationTypes() 'Roles
            LoadCertificationStatus() 'Certification status       
            LoadRNTrainers(ddlRNTrainer) 'Load RN Trainers
            LoadCourse() 'Load Courses
            'workLocationAddr.LoadStates()
            'workLocationAddr.LoadCounties()
        ElseIf (rblSelect.SelectedValue = "2") Then
            ClearFields()
            rblEmpSup.SelectedIndex = -1
            pnlNotationOptions.Visible = False
            pnlRNDDSearchOptions.Visible = True
            pnlEmployerSupervisor.Visible = False
            divButtons.Visible = True
            lbl4SSN.Enabled = True
            txt4SSN.Visible = True
            lbl4SSN.Visible = True
            chkAll.Text = "Search All DDPersonnels"
            lblRNDDLicenseDDCode.Text = "DDPersonnel Code:"
            LoadCertificationTypes() 'Roles
            LoadCertificationStatus() 'Certification status  
            LoadRNTrainers(ddlRNTrainer) 'Load RN Trainers
            LoadCourse() 'Load Courses
            'workLocationAddr.LoadStates()
            'workLocationAddr.LoadCounties()
        ElseIf (rblSelect.SelectedValue = "3") Then
            rblEmpSup.SelectedIndex = -1
            pnlNotationOptions.Visible = True
            pnlRNDDSearchOptions.Visible = False
            pnlEmployerSupervisor.Visible = False
            divButtons.Visible = True
            LoadNotationType() 'NotationType
            LoadNotationReason() 'NotationReason 
        ElseIf (rblSelect.SelectedValue = "4") Then
            rblEmpSup.SelectedIndex = -1
            pnlNotationOptions.Visible = False
            pnlRNDDSearchOptions.Visible = False
            pnlEmployerSupervisor.Visible = True
            pnlEmpSup.Visible = False
            divButtons.Visible = True
        ElseIf (rblSelect.SelectedValue = "5") Then
            rblEmpSup.SelectedIndex = -1
            pnlNotationOptions.Visible = False
            pnlRNDDSearchOptions.Visible = False
            pnlEmployerSupervisor.Visible = True
            pnlEmpSup.Visible = False
            divButtons.Visible = True
        Else
            pnlNotationOptions.Visible = False
            pnlRNDDSearchOptions.Visible = False
            pnlEmployerSupervisor.Visible = False
            divButtons.Visible = False
        End If
    End Sub

    Private Sub grdDDSearch_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdDDSearch.PageIndexChanging
        grdDDSearch.PageIndex = e.NewPageIndex
        If (rblSelect.SelectedValue = "3") Then
            LoadDDPersonnelNotations()
        ElseIf (chkAll.Checked = True) Then
            LoadALLDDs()
        End If

    End Sub


    Private Sub grdDDSearch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdDDSearch.SelectedIndexChanged
        SessionHelper.SessionUniqueID = grdDDSearch.SelectedValue
        Response.Redirect("MAISDetailReport.aspx")
    End Sub
    Private Sub grdRNSearch_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdRNSearch.PageIndexChanging
        grdRNSearch.PageIndex = e.NewPageIndex
        If (rblSelect.SelectedValue = "3") Then
            LoadRNNotaions()
        ElseIf (chkAll.Checked = True) Then
            LoadALLRNs()
        End If

    End Sub
    Private Sub grdDDList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdDDList.PageIndexChanging
        grdDDList.PageIndex = e.NewPageIndex
        If (chkAll.Checked = True) Then
            LoadALLDDs()
        Else
            GetParameters()
            LoadDDSearch()
        End If

    End Sub

    Private Sub grdRNList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdRNList.PageIndexChanging
        grdRNList.PageIndex = e.NewPageIndex
        If (chkAll.Checked = True) Then
            LoadALLRNs()
        Else
            GetParameters()
            LoadRNSearch()
        End If

    End Sub

    Private Sub grdEmployerList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdEmployerList.PageIndexChanging
        grdEmployerList.PageIndex = e.NewPageIndex
        If (rblEmpSup.SelectedValue = "1") Then 'RN employer
            LoadEmployerList(CInt(ddlRDDDList.SelectedValue), Trim(txtEmployerName.Text))
        ElseIf (rblEmpSup.SelectedValue = "2") Then 'DD personnel employers
            LoadEmployerList(CInt(ddlRDDDList.SelectedValue), Trim(txtEmployerName.Text))
        Else
            LoadEmployerList()
        End If
    End Sub

    Private Sub grdSupervisorList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdSupervisorList.PageIndexChanging
        grdSupervisorList.PageIndex = e.NewPageIndex
        LoadSupervisors()
    End Sub

    Private Sub chkAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkAll.CheckedChanged
        '   pnlAddr.Visible = False
        If (chkAll.Checked = True) Then
            pnlFileds.Visible = False
            pnlNewFeilds.Visible = False
        Else
            pnlFileds.Visible = True
            pnlNewFeilds.Visible = True
        End If

    End Sub

    Private Sub grdRNSearch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdRNSearch.SelectedIndexChanged
        SessionHelper.SessionUniqueID = grdRNSearch.SelectedValue
        Response.Redirect("MAISDetailReport.aspx")
    End Sub
    Private Sub grdDDList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdDDList.RowDataBound
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim gvCert As GridView = TryCast(e.Row.FindControl("gvDDCertification"), GridView)
                gvCert.DataSource = (From cer In CType(e.Row.DataItem, MAIS_Report).Cert_Info
                                     Order By cer.Certification_Sid
                                     Select New With {
                                         .Certification_Type = cer.Certification_Type,
                                         .Category_Code = cer.Category_Code,
                                         .Certification_Status = cer.Certification_Status,
                                         .Certification_Start_Date = cer.Certification_Start_Date.ToShortDateString(),
                                         .Certification_End_Date = cer.Certification_End_Date.ToShortDateString(),
                                         .Attestant_Name = cer.Attestant_Name,
                                         .RenewalCount = cer.RenewalCount
                                         }).ToList()
                gvCert.DataBind()
                Dim gvEmp As GridView = TryCast(e.Row.FindControl("gvDDEmployer"), GridView)
                gvEmp.DataSource = (From ep In CType(e.Row.DataItem, MAIS_Report).Emp_Info
                                    Order By ep.Employer_Sid
                                    Select New With {
                                        .Employer_Name = ep.Employer_Name,
                                        .CEO_First_Name = ep.CEO_First_Name,
                                        .CEO_Last_Name = ep.CEO_Last_Name,
                                        .Supervisor_First_Name = ep.Supervisor_First_Name,
                                        .Supervisor_Last_Name = ep.Supervisor_Last_Name,
                                        .WorkAddress = ep.WorkAddress,
                                        .WorkCounty = ep.WorkCounty
                                    }).ToList()
                gvEmp.DataBind()
                Dim gvCou As GridView = TryCast(e.Row.FindControl("gvDDCourse"), GridView)
                gvCou.DataSource = (From cou In CType(e.Row.DataItem, MAIS_Report).Course_Inof
                                    Order By cou.Course_Sid
                                    Select New With {
                                        .Trainer_Name = cou.Trainer_Name,
                                        .Course_Number = cou.Course_Number,
                                        .Session_Start_Date = cou.Session_Start_Date.ToShortDateString(),
                                        .Session_End_Date = cou.Session_End_Date.ToShortDateString(),
                                        .Session_CEUs = cou.Session_CEUs
                                        }).ToList()
                gvCou.DataBind()
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub grdRNList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdRNList.RowDataBound
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim gvCert As GridView = TryCast(e.Row.FindControl("gvCertification"), GridView)
                gvCert.DataSource = (From cer In CType(e.Row.DataItem, MAIS_Report).Cert_Info
                                     Order By cer.Certification_Sid
                                     Select New With {
                                         .Certification_Type = cer.Certification_Type,
                                         .Certification_Status = cer.Certification_Status,
                                         .Certification_Start_Date = cer.Certification_Start_Date.ToShortDateString(),
                                         .Certification_End_Date = cer.Certification_End_Date.ToShortDateString(),
                                         .Attestant_Name = cer.Attestant_Name,
                                         .RenewalCount = cer.RenewalCount
                                         }).ToList()
                gvCert.DataBind()
                Dim gvEmp As GridView = TryCast(e.Row.FindControl("gvEmployer"), GridView)
                gvEmp.DataSource = (From ep In CType(e.Row.DataItem, MAIS_Report).Emp_Info
                                    Order By ep.Employer_Sid
                                    Select New With {
                                        .Employer_Name = ep.Employer_Name,
                                        .CEO_First_Name = ep.CEO_First_Name,
                                        .CEO_Last_Name = ep.CEO_Last_Name,
                                        .Supervisor_First_Name = ep.Supervisor_First_Name,
                                        .Supervisor_Last_Name = ep.Supervisor_Last_Name,
                                        .WorkAddress = ep.WorkAddress,
                                        .WorkCounty = ep.WorkCounty
                                    }).ToList()
                gvEmp.DataBind()
                Dim gvCou As GridView = TryCast(e.Row.FindControl("gvCourse"), GridView)
                gvCou.DataSource = (From cou In CType(e.Row.DataItem, MAIS_Report).Course_Inof
                                    Order By cou.Course_Sid
                                    Select New With {
                                        .Trainer_Name = cou.Trainer_Name,
                                        .Course_Number = cou.Course_Number,
                                        .Session_Start_Date = cou.Session_Start_Date.ToShortDateString(),
                                        .Session_End_Date = cou.Session_End_Date.ToShortDateString(),
                                        .Session_CEUs = cou.Session_CEUs
                                        }).ToList()
                gvCou.DataBind()
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub rblNotation_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblNotation.SelectedIndexChanged
        pnlNotationSearch.Visible = True
    End Sub

    Private Sub ddlCourses_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCourses.SelectedIndexChanged
        LoadSessionByCourseID(CInt(ddlCourses.SelectedValue))
    End Sub
    Private Sub LoadSessionByCourseID(ByVal C_Sid As Integer)
        Dim reportSVC As IMAISReportService = StructureMap.ObjectFactory.GetInstance(Of IMAISReportService)()
        If C_Sid > 0 Then
            Dim cc As List(Of Course_Info) = reportSVC.GetSessionsByCourseID(C_Sid)
            If cc.Count > 0 Then
                ddlSessions.Items.Add(New ListItem("--- Select One ---", "0"))
                ddlSessions.DataSource = cc
                ddlSessions.DataTextField = "Session_Display_Dropdown"
                ddlSessions.DataValueField = "Session_Sid"
                ddlSessions.DataBind()
            End If
        End If

    End Sub

    Private Sub ddlRNTrainer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRNTrainer.SelectedIndexChanged
        ddlCourses.Items.Clear()
        ddlSessions.Items.Clear()
        If (ddlRNTrainer.SelectedValue <> "0") Then
            LoadCourse(CInt(ddlRNTrainer.SelectedValue))
        Else
            LoadCourse()
        End If
    End Sub

    Private Sub rblEmpSup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblEmpSup.SelectedIndexChanged
        grdEmployerList.DataSource = Nothing
        grdEmployerList.DataBind()
        grdSupervisorList.DataSource = Nothing
        grdSupervisorList.DataBind()
        lblCount.Text = String.Empty
        lblCount.Text = String.Empty
        If (rblSelect.SelectedValue = "4") Then
            lblEmpSup.Text = "Employer Name:"
            lblSupLast.Visible = False
            txtSupLastName.Visible = False
            If (rblEmpSup.SelectedValue = "1") Then
                lblRNDDIndividual.Text = "Choose RN:"
                pnlEmpSup.Visible = True
                LoadRNTrainers(ddlRDDDList)
            ElseIf (rblEmpSup.SelectedValue = "2") Then
                lblRNDDIndividual.Text = "Choose DDPersonnel:"
                pnlEmpSup.Visible = True
                LoadDDPersonnels(ddlRDDDList)
            ElseIf (rblEmpSup.SelectedValue = "3") Then
                pnlEmpSup.Visible = False
            End If
        ElseIf (rblSelect.SelectedValue = "5") Then
            lblSupLast.Visible = True
            txtSupLastName.Visible = True
            lblEmpSup.Text = "Supervisor First Name:"
            If (rblEmpSup.SelectedValue = "1") Then
                lblRNDDIndividual.Text = "Choose RN:"
                pnlEmpSup.Visible = True
                LoadRNTrainers(ddlRDDDList)
            ElseIf (rblEmpSup.SelectedValue = "2") Then
                lblRNDDIndividual.Text = "Choose DDPersonnel:"
                pnlEmpSup.Visible = True
                LoadDDPersonnels(ddlRDDDList)
            ElseIf (rblEmpSup.SelectedValue = "3") Then
                pnlEmpSup.Visible = False
            End If
        End If

    End Sub
    Private Sub LoadDDPersonnels(ByVal ddlPersonnel As DropDownList)
        Dim UserRNService As IUserRNDetailService = StructureMap.ObjectFactory.GetInstance(Of IUserRNDetailService)()
        Dim ListofRNs As New List(Of Business.Model.DD_Personnel)
        ListofRNs = UserRNService.GetAllDDPersonnel()
        ddlPersonnel.Items.Clear()
        ddlPersonnel.Items.Add(New ListItem("--Select DDPersonnel--", "0"))
        ddlPersonnel.DataSource = ListofRNs
        ddlPersonnel.DataTextField = "DDPersonnelNameSSN"
        ddlPersonnel.DataValueField = "DDPersonnel_Sid"
        ddlPersonnel.DataBind()
    End Sub
    Private Function ValidateParameters() As Boolean
        Dim flg As Boolean = False
        If ((String.IsNullOrWhiteSpace(Trim(txt4SSN.Value))) And
            (String.IsNullOrWhiteSpace(Trim(txtCEOFirst.Value))) And
            (String.IsNullOrWhiteSpace(Trim(txtCEOLast.Value))) And
            (String.IsNullOrWhiteSpace(Trim(txtDateFrom.Value))) And
            (String.IsNullOrWhiteSpace(Trim(txtDateTo.Value))) And
            (String.IsNullOrWhiteSpace(Trim(txtEmpName.Value))) And
            (String.IsNullOrWhiteSpace(Trim(txtRNDDLicDDCode.Value))) And
            (String.IsNullOrWhiteSpace(Trim(txtFName.Value))) And
            (String.IsNullOrWhiteSpace(Trim(txtLName.Value))) And
            (String.IsNullOrWhiteSpace(Trim(txtSupFirst.Value))) And
            (String.IsNullOrWhiteSpace(Trim(txtSupLast.Value))) And
            (ddlCertTypes.SelectedValue = "0") And
            (ddlCertStatus.SelectedValue = "0") And
            (ddlCourses.SelectedValue = "0") And
            (ddlSessions.SelectedValue = "0" Or ddlSessions.SelectedIndex = -1) And
            (ddlRNTrainer.SelectedValue = "0") And
            (rblExpWithIn.SelectedIndex = -1)) Then
            flg = True
        End If
        Return flg
    End Function
    Private Sub ClearFields()
        txt4SSN.Value = String.Empty
        txtCEOFirst.Value = String.Empty
        txtCEOLast.Value = String.Empty
        txtDateFrom.Value = String.Empty
        txtDateTo.Value = String.Empty
        txtEmpName.Value = String.Empty
        txtRNDDLicDDCode.Value = String.Empty
        txtFName.Value = String.Empty
        txtLName.Value = String.Empty
        txtSupFirst.Value = String.Empty
        txtSupLast.Value = String.Empty
        ddlCertTypes.SelectedValue = "0"
        ddlCertStatus.SelectedValue = "0"
        ddlCourses.SelectedValue = "0"
        ddlSessions.SelectedIndex = -1
        ddlRNTrainer.SelectedValue = "0"
        rblExpWithIn.SelectedIndex = -1       
    End Sub

    Private Sub grdDDList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdDDList.SelectedIndexChanged
        SessionHelper.SessionUniqueID = grdDDList.SelectedValue
        Response.Redirect("MAISDetailReport.aspx")
    End Sub

    Private Sub grdRNList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdRNList.SelectedIndexChanged
        SessionHelper.SessionUniqueID = grdRNList.SelectedValue
        Response.Redirect("MAISDetailReport.aspx")
    End Sub
End Class