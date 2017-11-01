Imports System.Web.Script.Services
Imports MAIS.Business.Model
Imports MAIS.Business.Services
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business
Imports MAIS.Business.Model.Enums
Imports ODMRDDHelperClassLibrary
Imports ODMRDDHelperClassLibrary.ODMRDDServiceProvider

Public Class TrainingSkills
    Inherits System.Web.UI.Page

    Dim _course As New CourseDetails()
    Dim _session As New SessionDetails()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Me.LoadMainCourseData()
            Select Case SessionHelper.ApplicationType
                Case "Initial"
                    divCEUs.Visible = False
                    divCourse.Visible = True
                Case "Renewal"
                    divCEUs.Visible = True
                    divCourse.Visible = False
                    Me.LoadCEUSData()


                Case "AddOn"
                    divCEUs.Visible = False
                    divCourse.Visible = True
                Case "Update Profile"
                    divCourse.Visible = False
                    divCEUs.Visible = True
                    Master.HideProgressBar = True
                    Me.LoadCEUSData()

                    bntPrevious.Visible = False
                    bntSaveAndContiue.Visible = False
                    hlbBack.Visible = True
            End Select


            Select Case SessionHelper.RN_Flg
                Case True
                    'btnSkills.Visible = False
                Case False
                    'btnSkills.Visible = True

            End Select
        End If
        'Dim listOfState = New List(Of CourseDetails)
        ''_course.InstructorName = "Drew,Nancy"
        ''_course.StartDate = "09/25/2000"
        ''_course.EndDate = "09/25/2000"
        ''_course.OBNApprovalNumber = "AAA-000-000-XXXX"

        ''_course.CategoryACEs = 1
        ''_course.TotalCEs = 8
        ''_course.Level = 7
        ''_course.Category = 5
        ''_course.CourseDescription = "RN Trainer Basic Concepts"
        ''_course.Create = True
        ''listOfState.Add(_course)
        'grdCourse.DataSource = listOfState
        'grdCourse.DataBind()
        'btnSkills.Visible = True

    End Sub

    Protected Sub OnRowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdCourse.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim customerId As String = grdCourse.DataKeys(e.Row.RowIndex).Value.ToString()
            Dim grdSession As GridView = TryCast(e.Row.FindControl("grdSession"), GridView)

            AddHandler grdSession.RowDataBound, AddressOf OnSessionRowDataBound

            Dim listOfSession = New List(Of SessionDetails)
            '_session.SessionStartDate = "10/10/2010"
            '_session.SessionEndDate = "10/11/2010"
            '_session.StreetAddress = "11111 Hollywood Blvd"
            '_session.LocationName = "Universal Studios"
            '_session.City = "Hollywood"
            '_session.State = "CA"
            '_session.Zip = "70210"
            '_session.CreateSession = True
            'listOfSession.Add(_session)
            grdSession.DataSource = CType(e.Row.DataItem, CourseDetails).SessionDetailList
            'GridSessionDetail.DataSource = CType(e.Row.DataItem, CourseDetails).SessionDetailList(0).SessionInformationDetailsList
            ' Me.grdSessionDates()

            grdSession.DataBind()
        End If

    End Sub


    Protected Sub OnSessionRowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim grdSessionDates As GridView = TryCast(e.Row.FindControl("grdSessionDates"), GridView)

            grdSessionDates.DataSource = CType(e.Row.DataItem, SessionAddressInformation).SessionInformationDetailsList
            grdSessionDates.DataBind()
        End If
    End Sub
    Protected Sub LoadMainCourseData()
        Dim CourseService As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()
        Dim CourseDetails As New List(Of Model.CourseDetails)
        CourseDetails = CourseService.GetApplicationCourseAndSessionsByAppID(SessionHelper.ApplicationID)

        Me.grdCourse.DataSource = CourseDetails

        Me.grdCourse.DataBind()
    End Sub

    Protected Sub LoadCEUSData()
        'Load the Rn data 
        Dim UserRNService As IUserRNDetailService = StructureMap.ObjectFactory.GetInstance(Of IUserRNDetailService)()
        Dim MAISService As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Dim ListofRNs As New List(Of Business.Model.RN_UserDetails)
        ListofRNs = UserRNService.getAllRNDetails

        ddlRNs.Items.Clear()

        Me.ddlRNs.Items.Add(New ListItem("--Select RN--", "-1"))
        For Each i As Business.Model.RN_UserDetails In ListofRNs
            Me.ddlRNs.Items.Add(New ListItem(i.LastFirstname, i.RN_Sid))
        Next

        'Load the Hiden field with the Category Detail object that is being  Renewal for
        Dim TEServiers As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()
        Dim HoldOject As New Model.CategoryDetails

        Select Case SessionHelper.ApplicationType
            Case "Renewal"
                HoldOject = TEServiers.GetCategoryByRoleCategoryLevelSid(SessionHelper.SelectedUserRole)
                Me.lblCEURenewalFor.Text = "Renewal for " & HoldOject.CategoryCode_With_Desc
            Case "Update Profile"
                HoldOject = TEServiers.GetCategoryByRoleCategoryLevelSid(SessionHelper.ExistingUserRole)
                Me.lblCEURenewalFor.Text = "Update for " & HoldOject.CategoryCode_With_Desc
        End Select

        Me.HFCertStartDate.Value = MAISService.GetCertificationStartDate(SessionHelper.SessionUniqueID, SessionHelper.ExistingUserRole)


        Session("CatDetail") = HoldOject
        Me.LoadRenewalCEUData()

    End Sub
    'Protected Sub btnRenewal_Click(sender As Object, e As EventArgs) Handles btnRenewal.Click
    '    Dim dt = New DataTable()
    '    Dim dcID = New DataColumn("Date", GetType(String))
    '    Dim dcName = New DataColumn("CEUs", GetType(String))
    '    dt.Columns.Add(dcID)
    '    dt.Columns.Add(dcName)
    '    Dim dcName1 = New DataColumn("Instructor Name", GetType(String))
    '    dt.Columns.Add(dcName1)
    '    Dim dcName2 = New DataColumn("Title", GetType(String))
    '    dt.Columns.Add(dcName2)
    '    Dim dcName3 = New DataColumn("Course Description", GetType(String))
    '    dt.Columns.Add(dcName3)
    '    Dim Row1 As DataRow
    '    Row1 = dt.NewRow()
    '    Row1.Item(0) = "9/25/2012"
    '    Row1.Item(1) = "4"
    '    Row1.Item(2) = "Mary Joe"
    '    Row1.Item(3) = "Head RN"
    '    Row1.Item(4) = "How to be Nurse"
    '    dt.Rows.Add(Row1)
    '    grdRenewal.DataSource = dt
    '    grdRenewal.DataBind()
    'End Sub

    'Protected Sub btnSkills_Click(sender As Object, e As EventArgs) Handles btnSkills.Click
    '    Dim dt = New DataTable()
    '    Dim dcID = New DataColumn("Date", GetType(String))
    '    Dim dcName = New DataColumn("Name/Title Of person Verifying Skills", GetType(String))
    '    dt.Columns.Add(dcID)
    '    dt.Columns.Add(dcName)
    '    Dim dcName1 = New DataColumn("Certification Category", GetType(String))
    '    dt.Columns.Add(dcName1)
    '    Dim dcName2 = New DataColumn("Skills Verified", GetType(String))
    '    dt.Columns.Add(dcName2)
    '    Dim Row1 As DataRow
    '    Row1 = dt.NewRow()
    '    Row1.Item(0) = "10/10/2010"
    '    Row1.Item(1) = "Ops mgr.Susie Que"
    '    Row1.Item(2) = "Cat1"
    '    Row1.Item(3) = "Oral"
    '    dt.Rows.Add(Row1)
    '    GridView1.DataSource = dt
    '    GridView1.DataBind()
    'End Sub

    Private Sub LoadCreateNewPanel()
        Dim UserRNService As IUserRNDetailService = StructureMap.ObjectFactory.GetInstance(Of IUserRNDetailService)()
        Dim ListofRNs As New List(Of Business.Model.RN_UserDetails)
        ListofRNs = UserRNService.getAllRNDetails

        ddlRnNames.Items.Clear()

        Me.ddlRnNames.Items.Add(New ListItem("--Select RN--", "-1"))
        For Each i As Business.Model.RN_UserDetails In ListofRNs
            Me.ddlRnNames.Items.Add(New ListItem(i.LastFirstname, i.RN_Sid))
        Next

        'With Me.ddlRnNames
        '    .DataSource = ListofRNs
        '    .DataTextField = "LastFirstname"
        '    .DataValueField = "RN_Sid"

        'End With

        Me.ddlRnNames.DataBind()

    End Sub

    Protected Sub lnkBtnSelect_Click(sender As Object, e As EventArgs)




    End Sub

    Protected Sub bntCancelCours_Click(sender As Object, e As EventArgs) Handles bntCancelCours.Click
        Me.pnlNewCourse.Visible = False
        Me.divCourse.Visible = True
    End Sub

    Protected Sub ddlRnNames_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRnNames.SelectedIndexChanged
        Dim TrainingService As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()
        Dim TrainingItem As Data.Role_Category_Level_Xref
        TrainingItem = TrainingService.GetRNCategoryAndLevel(ddlRnNames.SelectedValue)

        If Not (TrainingItem Is Nothing) Then

            txtLevelNew.Text = TrainingItem.Level_Type_Sid
            txtCategoryNew.Text = TrainingItem.Category_Type_Sid
        Else
            txtLevelNew.Text = String.Empty
            txtCategoryNew.Text = String.Empty
        End If


    End Sub

    Protected Sub lkbAddCourse_Click(sender As Object, e As EventArgs)
        Me.divCourse.Visible = False
        Select Case True
            Case UserAndRoleHelper.IsUserAdmin
                Me.pnlAddCourse.Visible = True
            Case UserAndRoleHelper.IsUserSecretary
                Me.pnlAddCourse.Visible = True
                Me.divSearchRNmain.Visible = False
                Me.divSecretaryView.Visible = True
                LoadSecretaryRNs(MAIS_Helper.GetUserId)


            Case UserAndRoleHelper.IsUserRN
                Dim MaisRoleDetails As Business.Model.MAISRNDDRoleDetails

                MaisRoleDetails = MAIS_Helper.GetUserRoleUsingMAIS(MAIS_Helper.GetUserId)
                Dim strRole As String = MaisRoleDetails.RoleName
                strRole = strRole.Replace(" ", "")

                Select Case strRole

                    Case Enums.Mais_Roles.RNMaster.ToString
                        Me.pnlAddCourse.Visible = True

                    Case Enums.Mais_Roles.RNInstructor.ToString, Enums.Mais_Roles.RNTrainer.ToString
                        Me.pnlAddCourse.Visible = True
                        Me.divSearchRNmain.Visible = False
                        LoadRNData(MaisRoleDetails.RNLicenseNumber)

                    Case Enums.Mais_Roles.Secretary

                End Select
        End Select

        LoadPnlData()

    End Sub

    Private Sub LoadSecretaryRNs(ByVal SecretaryID As Integer)
        Dim TrainService As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()
        Dim RNList As New List(Of Model.RN_UserDetails)

        RNList = TrainService.GetSecretaryRNS(SecretaryID)

        Select Case RNList.Count
            Case Is > 1
                Me.ddlRNforSecretory.Items.Clear()

                Me.ddlRNforSecretory.Items.Add(New ListItem("--Select RN--", "-1"))
                For Each i As Business.Model.RN_UserDetails In RNList
                    Me.ddlRNforSecretory.Items.Add(New ListItem(i.LastFirstname, i.RNLicense_Number))
                Next

            Case Is = 1
                Me.divSecretaryView.Visible = False
                LoadRNData(RNList(0).RNLicense_Number)

            Case Is = 0
                Dim lit As New Literal
                lit.Text = "There are no RNs associated with this secretary."
                Me.pnlMessage.Controls.Add(lit)
        End Select

    End Sub

    Private Sub LoadRNData(ByVal RN_LicNumber As String)
        Dim SearchService As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()
        Dim ApplicationService As IApplicationDetailInformationService = StructureMap.ObjectFactory.GetInstance(Of IApplicationDetailInformationService)()



        Dim SearchData As New List(Of Model.SessionCourseInfoDetails)
        Dim AppCreateData As New Model.ApplicationInformationDetails
        AppCreateData = ApplicationService.GetApplicationInfromationByAppID(SessionHelper.ApplicationID)



        SearchData = SearchService.GetCourseSessionByRN_LicenseNumber(RN_LicNumber, SessionHelper.SelectedUserRole, AppCreateData.CreateDate)

        If Not (SearchData Is Nothing OrElse SearchData.Count = 0) Then

            Me.gvSearchData.DataSource = SearchData
            Me.gvSearchData.DataBind()

            Dim lit As New Literal
            lit.Text = "List of RN's session allowed for this Secretary."
            Me.pnlMessage.Controls.Add(lit)
        Else

            Dim showDate As Date
            showDate = AppCreateData.CreateDate
            Dim couresType As String = Nothing

            Select Case SessionHelper.SelectedUserRole
                Case Enums.Mais_Roles.RNTrainer
                    couresType = Enums.Mais_Roles.RNTrainer.ToString
                Case Enums.Mais_Roles.RNInstructor
                    couresType = Enums.Mais_Roles.RNInstructor.ToString
                Case Enums.Mais_Roles.Bed17
                    couresType = Enums.Mais_Roles.Bed17.ToString
                Case Enums.Mais_Roles.QARN
                    couresType = Enums.Mais_Roles.QARN.ToString

            End Select

            Me.gvSearchData.DataSource = Nothing
            Me.gvSearchData.DataBind()

            Dim lit As New Literal
            lit.Text = "There are no session three months before or after the application creation date of " & showDate.ToShortDateString & " with course type of " & couresType & "."
            Dim lit2 As New Literal
            lit2.Text = "<br/> If you need to add a session to this application that is outside of the three months before or after the application creation date, contact the Master RN. "
            Me.pnlMessage.Controls.Add(lit)

            Me.pnlMessage.Controls.Add(lit2)
        End If
    End Sub
    Private Sub LoadPnlData()
        Dim UserRNService As IUserRNDetailService = StructureMap.ObjectFactory.GetInstance(Of IUserRNDetailService)()
        'Dim ListofRNs As New List(Of Business.Model.RN_UserDetails)
        'ListofRNs = UserRNService.getAllRNDetails

        'ddlRnSelectCourse.Items.Clear()

        'Me.ddlRnSelectCourse.Items.Add(New ListItem("--Select RN--", "-1"))
        'For Each i As Business.Model.RN_UserDetails In ListofRNs
        '    Me.ddlRnSelectCourse.Items.Add(New ListItem(i.LastFirstname, i.RN_Sid))
        'Next
        'Dim TrainingServies As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()
        'Dim CourseDetailData As List(Of Model.CourseDetails)
        'CourseDetailData = TrainingServies.GetCourseAll()

        'If Not (CourseDetailData.Count = 0) Then
        '    Me.ddlCourseSelect.DataSource = CourseDetailData
        '    Me.ddlRnSelectCourse.DataValueField = "Course_Sid"
        '    Me.ddlCourseSelect.DataTextField = "OBNApprovalNumber"
        '    Me.ddlCourseSelect.DataBind()

        'End If
        ''Throw New NotImplementedException
    End Sub

    Protected Sub bntRNSerach_Click(sender As Object, e As EventArgs) Handles bntRNSearch.Click
        Select Case True
            Case Me.txtRNNumber.Text.Length > 0
                bntSearchRN_Click(sender, e)

            Case txtRNName.Text.Length > 0
                bntSearchRNName_Click(sender, e)
        End Select
    End Sub


    Protected Sub bntSearchRN_Click(sender As Object, e As EventArgs) Handles bntSearchRN.Click
        Dim SearchService As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()
        Dim SearchData As New List(Of Model.SessionCourseInfoDetails)
        Dim FilterStatus As Boolean = False
        SearchData = SearchService.GetCourseSessionByRN_LicenseNumber(Me.txtRNNumber.Text, SessionHelper.SelectedUserRole)

        If Not SearchData Is Nothing Then
            Me.gvSearchData.DataSource = SearchData
            Me.gvSearchData.DataBind()
            If SearchData.Count > 0 Then
                FilterStatus = True
            End If

        End If

        Me.SetFilter(FilterStatus)

    End Sub
    Protected Sub bntSearchRNName_Click(sender As Object, e As EventArgs) Handles bntSearchRNName.Click
        Dim SearchService As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()
        Dim SearchData As New List(Of Model.SessionCourseInfoDetails)
        Dim FilterStatus As Boolean = False

        SearchData = SearchService.GetCourseSessionByRN_Name(Me.txtRNName.Text, SessionHelper.SelectedUserRole)


        If Not SearchData Is Nothing Then
            Me.gvSearchData.DataSource = SearchData
            Me.gvSearchData.DataBind()
            If SearchData.Count > 0 Then
                FilterStatus = True
            End If
        End If
        Me.SetFilter(FilterStatus)

    End Sub


    Protected Sub bntRNFilter_Click(sender As Object, e As EventArgs) Handles bntRNFilter.Click
        Dim SearchService As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()
        Dim SearchData As New List(Of Model.SessionCourseInfoDetails)
        Select Case True
            Case Me.txtRNNumber.Text.Length > 0
                SearchData = SearchService.GetCourseSessionByRN_LicenseNumber(Me.txtRNNumber.Text, SessionHelper.SelectedUserRole)

            Case txtRNName.Text.Length > 0
                SearchData = SearchService.GetCourseSessionByRN_Name(Me.txtRNName.Text, SessionHelper.SelectedUserRole)
        End Select

        Me.gvSearchData.DataSource = (From s In SearchData
                                      Where s.StartDate >= Me.txtFilterStartDate.Text
                                      Select s).ToList
        Me.gvSearchData.DataBind()

    End Sub

    Protected Sub gvSearchData_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvSearchData.SelectedIndexChanged
        Dim userSelectedValue As GridView = CType(sender, GridView)
        Try


            If IsNumeric(userSelectedValue.SelectedValue) AndAlso userSelectedValue.SelectedValue > 0 Then
                Dim SearchService As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()
                If SearchService.SaveCourseSessoin(SessionHelper.ApplicationID, userSelectedValue.SelectedValue, MAIS_Helper.GetUserId) = True Then
                    Me.pnlAddCourse.Visible = False
                    Me.divCourse.Visible = True
                    Me.LoadMainCourseData()


                End If
            End If
        Catch ex As Exception
            Me.div_MessagesContent.InnerText = ex.Message
            'Dim b As MAIS.Business.MAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IBusinessBase)()
            'b.LogError(ex.Message)
        End Try
    End Sub



    Protected Sub bntAddCancel_Click(sender As Object, e As EventArgs) Handles bntAddCancel.Click
        Me.pnlAddCourse.Visible = False
        Me.divCourse.Visible = True
        Me.LoadMainCourseData()
    End Sub

    Private Sub grdCourse_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdCourse.RowDeleting

        Dim RemoveService As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()
        If RemoveService.DeleteCourseSession(SessionHelper.ApplicationID) = False Then
            Me.div_MessagesContent.InnerText = "Error on the removing the Session."
        Else
            Me.div_MessagesContent.InnerText = String.Empty
        End If
        Me.LoadMainCourseData()

    End Sub

    Protected Sub bntPrevious_Click(sender As Object, e As EventArgs) Handles bntPrevious.Click
        Response.Redirect(PagesHelper.GetPreviousPage(Master.CurrentPage))
    End Sub

    Protected Sub bntSaveAndContiue_Click(sender As Object, e As EventArgs) Handles bntSaveAndContiue.Click
        Response.Redirect(PagesHelper.GetNextPage(Master.CurrentPage))
    End Sub

    Protected Sub bntAddCEU_Click(sender As Object, e As EventArgs) Handles bntAddCEU.Click
        Dim TEService As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()
        Dim MAISService As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Dim ListOfCEU As New List(Of Model.CEUDetails)
        Dim NextCout As Integer = 0
        Dim CatObject As Model.CategoryDetails = CType(Session("CatDetail"), Model.CategoryDetails)

        'If Session("ListOfCEU") Is Nothing Then
        '    ListOfCEU = New List(Of Model.CEUDetails)
        'Else
        '    ListOfCEU = CType(Session("ListofCEU"), List(Of Model.CEUDetails))
        'End If
        'If ListOfCEU.Count > 0 Then
        '    NextCout = (From c In ListOfCEU
        '                               Where c.CEUs_Renewal_Sid < 0
        '                               Select c.CEUs_Renewal_Sid).Min
        'End If

        'NextCout -= 1

        Dim NCEU_Data As New Model.CEUDetails
        With NCEU_Data
            .Attended_Date = txtCEUDate.Text
            .Total_CEUs = txtCEUs.Text
            .RN_Sid = ddlRNs.SelectedValue
            .RN_Name = ddlRNs.SelectedItem.ToString
            .Instructor_Name = txtCEUInstructorName.Text
            .Title = txtCEUTitle.Text
            .Category_Type_Sid = CatObject.Category_Type_Sid
            .Category_Type_Code = CatObject.Category_Code
            .Course_Description = txtCEUCourseDescription.Text
            .Start_Date = Now()
            .End_Date = MAISService.GetCertificationDate(SessionHelper.SessionUniqueID, SessionHelper.ExistingUserRole) 'Now().AddYears(2)
            .Permanent_Flg = False
            .Active_Flg = True
            .DD_RN_Personnel_SID = SessionHelper.SessionUniqueID
        End With

        ListOfCEU.Add(NCEU_Data)

        If TEService.SaveCEUDetail(MAIS_Helper.GetUserId, ListOfCEU, SessionHelper.ApplicationID) = True Then
            'Reset Field date 
            Me.txtCEUDate.Text = String.Empty
            Me.txtCEUs.Text = String.Empty
            Me.ddlRNs.SelectedValue = -1
            Me.txtCEUInstructorName.Text = String.Empty
            Me.txtCEUTitle.Text = String.Empty
            Me.txtCEUCourseDescription.Text = String.Empty

            'Reload the Renewal grid. 
            LoadRenewalCEUData()

        Else

            'Set the Error masasge 

        End If

        'Me.grdRenewal.DataSource = ListOfCEU
        'Me.grdRenewal.DataBind()
        'Session("ListofCEU") = ListOfCEU


    End Sub

    Private Sub LoadRenewalCEUData()
        Dim TEService As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()
        Dim ListOfCEU As New List(Of Model.CEUDetails)
        Dim CatObject As Model.CategoryDetails = CType(Session("CatDetail"), Model.CategoryDetails)
        ListOfCEU = TEService.GetCEUByUserID(SessionHelper.SessionUniqueID, CatObject.Category_Type_Sid, SessionHelper.ApplicationID)

        Me.grdRenewal.DataSource = ListOfCEU
        Me.grdRenewal.DataBind()
        If ListOfCEU.Count > 0 Then
            Me.lblCEUListMessage.Visible = True
        Else
            Me.lblCEUListMessage.Visible = False
        End If
    End Sub

    Private Sub grdRenewal_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdRenewal.RowCommand
        Select Case e.CommandName
            Case "Delete"
                Dim ID As Integer = e.CommandArgument
                Dim TService As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()

                If TService.DeleteCEUByID(ID) = True Then
                    Me.LoadRenewalCEUData()

                End If
        End Select
    End Sub

    Private Function GetRNCertEndDate() As Date
        Dim MAISService As IMAISSerivce = StructureMap.ObjectFactory.GetAllInstances(Of IMAISSerivce)()

    End Function

    Private Sub grdRenewal_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdRenewal.RowDeleting

    End Sub

    Protected Sub hlbBack_Click(sender As Object, e As EventArgs) Handles hlbBack.Click
        Response.Redirect("UpdateExistingPage.aspx")
    End Sub

    Protected Sub ddlRNforSecretory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRNforSecretory.SelectedIndexChanged
        If ddlRNforSecretory.SelectedValue <> "-1" Then
            LoadRNData(Me.ddlRNforSecretory.SelectedValue)
        Else
            Me.gvSearchData.DataSource = Nothing
            Me.gvSearchData.DataBind()

            Me.pnlMessage.Controls.Clear()

        End If

    End Sub

    Protected Sub SetFilter(ByVal FilterStatus As Boolean)
        Me.bntRNFilter.Enabled = FilterStatus
        Me.txtFilterStartDate.Enabled = FilterStatus
        Me.txtFilterStartDate.Text = String.Empty
    End Sub


End Class