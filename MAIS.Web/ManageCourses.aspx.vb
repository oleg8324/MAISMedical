Imports System.Web.Script.Services
Imports MAIS.Business.Model
Imports MAIS.Business.Services
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business
Imports MAIS.Business.Model.Enums
Imports ODMRDDHelperClassLibrary
Imports ODMRDDHelperClassLibrary.ODMRDDServiceProvider

Public Class ManageCourses
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.htMode.Value = "Search"
            Master.HideLink = True
            Master.HideProgressBar = True
            SessionHelper.Notation_Flg = False

            Me.hdfUserID.Value = MAIS_Helper.GetUserId

            If UserAndRoleHelper.IsUserRN Then
                Dim MaisRoleDetails As Business.Model.MAISRNDDRoleDetails

                MaisRoleDetails = MAIS_Helper.GetUserRoleUsingMAIS(hdfUserID.Value)
                Me.hdfLoginRole.Value = MaisRoleDetails.RoleName
                Me.hdfRNID.Value = MaisRoleDetails.RNSID


                Me.hdfMAISRole.Value = MaisRoleDetails.RoleName 'SessionHelper.MAISLevelUserRole





            Else
                If UserAndRoleHelper.IsUserAdmin Then
                    Me.hdfMAISRole.Value = "MAIS_Admin"
                    'Me.hdfRNID.Value = 1 'todo remove after testing for Rn Instructor
                End If
                If UserAndRoleHelper.IsUserSecretary Then
                    Me.hdfMAISRole.Value = "Secretary"
                    'Need to only allow read only. Turn of the Add Course button 
                    Me.bntAddNewCourse.Visible = False

                End If
                If UserAndRoleHelper.IsUserReadOnly Then
                    Me.hdfMAISRole.Value = "MAIN_ReadOnly"
                End If
            End If

            TestRoleAllowToAddCourse()
            JqueryIsPostBack()


        End If

    End Sub


    Private Sub TestRoleAllowToAddCourse()
        Select Case hdfMAISRole.Value
            Case "17 + Bed", "QA RN", "Secretary"
                Me.bntAddNewCourse.Visible = False

        End Select
    End Sub

    Private Function TestUserAllowedToAddSession(ByVal CourseRNID As Integer) As Boolean
        Dim RetVal As Boolean = False
        Select Case hdfMAISRole.Value
            Case "17 + Bed", "QA RN", "Secretary"
                RetVal = False
            Case "RN Trainer", "RN Instructor"
                If CourseRNID = MAIS_Helper.GetRN_SidbyUserID(SessionHelper.MAISUserID) Then
                    RetVal = True
                End If
            Case Else
                RetVal = True
        End Select
        Return RetVal

    End Function

    Protected Sub ServerValidate(source As Object, args As ServerValidateEventArgs)
        args.IsValid = True

    End Sub

    Protected Sub bntAddNewCourse_Click(sender As Object, e As EventArgs) Handles bntAddNewCourse.Click
        Me.htMode.Value = "AddCourse"
        Me.TestMode()

        'Dim NewCourseData As New List(Of MAIS.Business.Model.CourseDetails)
        'Dim newCourse As New MAIS.Business.Model.CourseDetails



        'NewCourseData.Add(newCourse)

        'Me.gvAddCourse.DataSource = NewCourseData
        'Me.gvAddCourse.DataBind()

    End Sub

    Private Sub LoadNewCourseDropDownList()

        Dim UserRNService As IUserRNDetailService = StructureMap.ObjectFactory.GetInstance(Of IUserRNDetailService)()
        Dim ListofRNs As New List(Of Business.Model.RN_UserDetails)
        ListofRNs = UserRNService.getAllRNDetails
        Me.ddlRnNames.Items.Clear()

        ' Dim rnNameDropDown As DropDownList = e.Row.FindControl("ddlRnNames")


        ' rnNameDropDown.Items.Clear()

        ddlRnNames.Items.Add(New ListItem("--Select RN--", "-1"))
        For Each i As Business.Model.RN_UserDetails In ListofRNs
            ddlRnNames.Items.Add(New ListItem(i.LastFirstname, i.RN_Sid))
        Next

        'Dim TrainingServies As ITrainingSkillsPageService = StructureMap.ObjectFactory.GetInstance(Of ITrainingSkillsPageService)()
        'Dim CourseDetailData As List(Of Model.CourseDetails)
        'CourseDetailData = TrainingServies.GetCourseAll()

        'If Not (CourseDetailData.Count = 0) Then
        '    Me.ddlCourseSelect.DataSource = CourseDetailData
        '    Me.ddlRnSelectCourse.DataValueField = "Course_Sid"
        '    Me.ddlCourseSelect.DataTextField = "OBNApprovalNumber"
        '    Me.ddlCourseSelect.DataBind()

        'End If
    End Sub

    Private Sub ReloadRNbyRole(ByVal iRole As Integer)
        Dim UserRnService As IUserRNDetailService = StructureMap.ObjectFactory.GetInstance(Of IUserRNDetailService)()
        Dim ListOfRns As New List(Of Business.Model.RN_UserDetails)
        ListOfRns = UserRnService.GetRNDetailsWithEmailsByRoleID(iRole)
        ddlRnNames.Items.Clear()

        ddlRnNames.Items.Add(New ListItem("--Select RN--", "-1"))
        For Each i As Business.Model.RN_UserDetails In ListOfRns
            ddlRnNames.Items.Add(New ListItem(i.LastFirstname, i.RN_Sid))
        Next
    End Sub
    Public Sub LoadCounties()
        'Dim lstCountyBoards As New ReturnObject(Of ODMRDDHelperClassLibrary.CountyBoardCollection)
        'Dim cs As CountyBoardService = New CountyBoardService(ConfigHelper.GetOIDDBConnectionString)
        'lstCountyBoards = cs.GetAllCountyBoards("mastr", True)
        Dim userSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Dim lstCountyBoards As List(Of Model.CountyDetails) = userSvc.GetAllCountyCodes()
        ddlSessionCounty.Items.Clear()
        'Counties
        ddlSessionCounty.Items.Add(New ListItem("--- County Selection ---", "--- County Selection ---"))
        For Each ct In lstCountyBoards
            ddlSessionCounty.Items.Add(New ListItem(ct.CountyAlias, ct.CountyID))
        Next
        'ddlSessionCounty.DataSource = lstCountyBoards.ReturnValue
        'ddlSessionCounty.DataTextField = "CountyAlias"
        'ddlSessionCounty.DataValueField = "CountyAlias"
        ddlSessionCounty.DataBind()
    End Sub
    Private Sub loadNewStateDropDownList()
        'Dim lstStates As New ReturnObject(Of ODMRDDHelperClassLibrary.StateCollection)
        'Dim ss As StateService = New StateService(ConfigHelper.GetOIDDBConnectionString)
        'lstStates = ss.GetAllStates(ConfigHelper.GetOIDDBConnectionString)
        Dim userSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Dim lstStates As List(Of Model.StateDetails) = userSvc.GetAllStates()
        ddlSessionStateNew.Items.Clear()


        ''states  
        ddlSessionStateNew.DataSource = lstStates
        ddlSessionStateNew.DataTextField = "StateAbr"
        ddlSessionStateNew.DataValueField = "StateID"
        ddlSessionStateNew.SelectedValue = "35"
        ddlSessionStateNew.DataBind()
    End Sub
    'Private Sub gvAddCourse_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvAddCourse.RowCancelingEdit
    '    ' Me.dvAddCourse.Visible = False
    '    ' Me.dvMainSearch.Visible = True
    'End Sub

    Private Sub gvAddSession_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Select Case e.CommandName
            Case "Insert_New_Session"

        End Select
    End Sub

    Private Sub TestMode()
        Select Case Me.htMode.Value
            Case "AddCourse"
              
                Me.dvMainSearch.Visible = False
                Me.dvNewRnType.Visible = True
                LoadNewCourseDropDownList()
                loadNewStateDropDownList()
                LoadCounties()
                SetCourseFieldsEnable(True)
                Me.rblNewRnTypeSelect.SelectedValue = Nothing
                Me.ddlRnNames.SelectedValue = -1
                Me.txtEffectiveStartDateNew.Text = Nothing
                Me.txtEffectiveEndDateNew.Text = Nothing
                Me.txtCourseNumberRnNew.Text = Nothing
                Me.txtCategoryACWSNew.Text = Nothing
                Me.txtTotalCEsNew.Text = Nothing
                Me.ddlLevelNew.SelectedValue = -1
                Me.ddlCategoryNew.SelectedValue = Nothing
                Me.txtCourseDescriptionNew.Text = Nothing
                Me.BntSaveCourseData.Visible = True
                Me.bntSaveCourse.Visible = True
                Me.lkbBack.Visible = True
                Me.BntSubitAll.Enabled = False
                SessionFieldReset()
                Session("SDDataList") = Nothing
                Session("NewCourseID") = Nothing
                Session("Syllabus") = Nothing
                Me.grvSyllabus.DataSource = Nothing
                Me.grvSyllabus.DataBind()
                Select Case SessionHelper.MAISLevelUserRole
                    Case Enums.RoleLevelCategory.RNTrainer_RLC
                        Me.rblNewRnTypeSelect.Items(0).Enabled = False
                        'Me.rblNewRnTypeSelect.DataBind()
                    Case Else

                End Select
                'JqueryIsPostBack()

            Case "AddSession"


                Me.BntSaveCourseData.Visible = False
                Me.bntSaveCourse.Visible = False

                Me.bntMoveToSessionDetails.Visible = True
                Me.BntSubitAll.Enabled = False
                Me.dvAddSessions.Visible = True
                Me.bntPriviousSession.Visible = False
                Me.bntMoveToSessionDetails.Visible = True
                Me.bntCancelCours.Visible = True
                Me.lkbBack.Visible = True
                Me.SetCourseFieldsEnable(False)
                Me.SetCourseSessionFieldsEnable(True)
                Me.SessionFieldReset()

            Case "Search"
                'Make sure all add Session and Course panels are not Vissable.
                If Me.dvSessionDetailAdd.Visible = True Then
                    Me.dvSessionDetailAdd.Visible = False
                End If
                If Me.dvAddSessions.Visible = True Then
                    Me.dvAddSessions.Visible = False
                End If
                If Me.dvNewRnType.Visible = True Then
                    Me.dvNewRnType.Visible = False
                End If
                Session("SDDataList") = Nothing
                Session("NewCourseID") = Nothing
                Session("Syllabus") = Nothing

                Me.lkbBack.Visible = False
                Me.dvMainSearch.Visible = True
                ' Me.txtRNNO.Text = String.Empty
                ' Me.txtFirstName.Text = String.Empty
                ' Me.txtLName.Text = String.Empty
                'Me.txtSearchSessionStartDate.Text = String.Empty
        End Select
    End Sub

    'Private Sub gvAddCourse_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvAddCourse.RowCommand
    '    Select Case e.CommandName
    '        Case "Submit"
    '            Dim newCourse As New Business.Model.CourseDetails
    '            With newCourse
    '                .Course_Sid = -1
    '                .RN_Sid = CType(Me.gvAddCourse.Controls(0).Controls(0).FindControl("ddlRnNames"), DropDownList).SelectedValue
    '                .InstructorName = CType(Me.gvAddCourse.Controls(0).Controls(0).FindControl("ddlRnNames"), DropDownList).SelectedItem.ToString
    '                .StartDate = CType(Me.gvAddCourse.Controls(0).Controls(0).FindControl("txtEffectiveStartDateNew"), TextBox).Text
    '                .EndDate = CType(Me.gvAddCourse.Controls(0).Controls(0).FindControl("txtEffectiveEndDateNew"), TextBox).Text
    '                .OBNApprovalNumber = CType(Me.gvAddCourse.Controls(0).Controls(0).FindControl("txtCourseNumberRnNew"), TextBox).Text
    '                .CategoryACEs = CType(Me.gvAddCourse.Controls(0).Controls(0).FindControl("txtCategoryACWSNew"), TextBox).Text
    '                .TotalCEs = CType(Me.gvAddCourse.Controls(0).Controls(0).FindControl("txtTotalCEsNew"), TextBox).Text
    '                .Level = CType(Me.gvAddCourse.Controls(0).Controls(0).FindControl("txtLevelNew"), TextBox).Text
    '                .Category = CType(Me.gvAddCourse.Controls(0).Controls(0).FindControl("txtCategoryNew"), TextBox).Text
    '                .CourseDescription = CType(Me.gvAddCourse.Controls(0).Controls(0).FindControl("txtCourseDescriptionNew"), TextBox).Text
    '            End With

    '            Dim NewList As New List(Of Business.Model.CourseDetails)
    '            NewList.Add(newCourse)
    '            Me.gvAddCourse.DataSource = NewList
    '            Me.gvAddCourse.DataBind()
    '        Case "Cancel"

    '        Case "Insert_New_Session"
    '            Dim DataCourse = CType(Me.gvAddCourse.DataSource, List(Of Model.CourseDetails))
    '            Dim newSession As New Model.CourseDetails
    '            ''Dim grVales As GridView = CType(e..Row.FindControl("gvAddSession"), GridView)
    '            'With newSession
    '            '    .Course_Sid = -1

    '            'End With

    '            'With DataCourse(0).SessionDetailList

    '            'End With
    '    End Select
    'End Sub


    'Private Sub gvAddCourse_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvAddCourse.RowDataBound
    '    If (e.Row.RowType = DataControlRowType.EmptyDataRow) Then
    '        LoadNewCourseDropDownList(e)
    '    Else
    '        If (e.Row.RowType = DataControlRowType.DataRow) Then
    '            Dim customerId As String = gvAddCourse.DataKeys(e.Row.RowIndex).Value.ToString()
    '            Dim grdSession As GridView = TryCast(e.Row.FindControl("gvAddSession"), GridView)

    '            AddHandler grdSession.RowDataBound, AddressOf OnSessionRowDataBound
    '            AddHandler grdSession.RowCommand, AddressOf gvAddSession_RowCommand

    '            grdSession.DataSource = CType(e.Row.DataItem, CourseDetails).SessionDetailList


    '            grdSession.DataBind()
    '        End If

    '    End If
    'End Sub

    'Protected Sub OnSessionRowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs)
    '    If e.Row.RowType = DataControlRowType.EmptyDataRow Then
    '        CType(e.Row.FindControl("txtNewSessionStartDate"), TextBox).CssClass = "date-pick"


    '    End If
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        'Dim grdSessionDates As GridView = TryCast(e.Row.FindControl("grdSessionDates"), GridView)

    '        'grdSessionDates.DataSource = CType(e.Row.DataItem, SessionAddressInformation).SessionInformationDetailsList
    '        'grdSessionDates.DataBind()
    '    End If
    'End Sub

    'Protected Sub lkbInsertNewSession_Click(sender As Object, e As EventArgs)

    'End Sub
#Region "Add Courese, Session And Session Detail Button clicks"

    Protected Sub bntCancelCours_Click(sender As Object, e As EventArgs) Handles bntCancelCours.Click
        'Me.dvMainSearch.Visible = True
        'Me.dvNewRnType.Visible = False
        Me.htMode.Value = "Search"
        TestMode()

    End Sub

    Protected Sub bntSaveCourse_Click(sender As Object, e As EventArgs) Handles bntSaveCourse.Click
        Me.bntSaveCourse.Visible = False
        Me.bntCancelCours.Visible = False
        Me.bntSaveCourse.Visible = False
        Me.dvAddSessions.Visible = True
        SetCourseFieldsEnable(False)
        SetCourseSessionFieldsEnable(True)
        If Me.bntMoveToSessionDetails.Visible = False Then
            Me.bntMoveToSessionDetails.Visible = True
        End If
        If Me.bntPriviousSession.Visible = False Then
            Me.bntPriviousSession.Visible = True
        End If
    End Sub

    Protected Sub bntMoveToSessionDetails_Click(sender As Object, e As EventArgs) Handles bntMoveToSessionDetails.Click
        Me.bntMoveToSessionDetails.Visible = False
        Me.bntPriviousSession.Visible = False
        Me.dvSessionDetailAdd.Visible = True
        SetCourseSessionFieldsEnable(False)
        Session("SDDataList") = Nothing
        Me.gvAddSessionDetails.DataSource = Nothing
        Me.gvAddSessionDetails.DataBind()
        lblAddSessionError.Text = String.Empty
    End Sub

    Protected Sub bntPriviousSession_Click(sender As Object, e As EventArgs) Handles bntPriviousSession.Click
        Me.bntSaveCourse.Visible = True
        Me.bntCancelCours.Visible = True
        Me.bntSaveCourse.Visible = True
        Me.dvAddSessions.Visible = False
        SetCourseFieldsEnable(True)
    End Sub

    Protected Sub bntSessionDetailCancel_Click(sender As Object, e As EventArgs) Handles bntSessionDetailCancel.Click
        Me.bntMoveToSessionDetails.Visible = True
        If Me.htMode.Value = "AddCourse" Then
            Me.bntPriviousSession.Visible = True
        End If

        Me.dvSessionDetailAdd.Visible = False
        SetCourseSessionFieldsEnable(True)
    End Sub


    Protected Sub lkAddSessionDetail_Click(sender As Object, e As EventArgs) Handles lkAddSessionDetail.Click
        Dim SDDataList As New List(Of Model.SessionInformationDetails)
        If Session("SDDataList") Is Nothing Then

            Dim SSData As New Model.SessionInformationDetails
            With SSData
                .Session_Sid = -1
                .Session_Date = Me.txtAddClassDate.Text
                .Total_CEs = Me.txtAddTotalCEs.Text
            End With
            SDDataList.Add(SSData)
        Else
            SDDataList = CType(Session("SDDataList"), List(Of Model.SessionInformationDetails))
            Dim SSData As New Model.SessionInformationDetails
            With SSData
                .Session_Sid = SDDataList.Count - 1
                .Session_Date = Me.txtAddClassDate.Text
                .Total_CEs = Me.txtAddTotalCEs.Text
            End With
            SDDataList.Add(SSData)
        End If

        Session("SDDataList") = SDDataList
        Me.gvAddSessionDetails.DataSource = SDDataList
        Me.gvAddSessionDetails.DataBind()
        Me.txtAddTotalCEs.Text = String.Empty
        Me.txtAddClassDate.Text = String.Empty
        Me.BntSubitAll.Enabled = Me.CEsDayTotalEqualCourseCES()
    End Sub


    Public Sub JqueryIsPostBack()
        ClientScript.RegisterClientScriptBlock(GetType(Boolean), "isPostBack", "var isPostBack =" + IsPostBack.ToString().ToLower + ";", True)

    End Sub

    Protected Sub txtEffectiveStartDateNew_TextChanged(sender As Object, e As EventArgs) Handles txtEffectiveStartDateNew.TextChanged
        Dim maisService As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Dim EndDate As Date
        Dim RnID As Integer
        If Me.RequiredFieldValidator1.Enabled = False Then ' the control will not post back to the server if Enable is False. Need to set the RN From the Hiden field
            RnID = hdfRNID.Value
        Else
            RnID = ddlRnNames.SelectedValue
        End If
        EndDate = maisService.GetCertificationDateThatIsHighRoleProiorityByRNSID(RnID, Me.txtEffectiveStartDateNew.Text)
        Me.txtEffectiveEndDateNew.Text = EndDate

        'If String.IsNullOrEmpty(txtEffectiveEndDateNew.Text) Then
        '    Dim twoYear As Date = CDate(Me.txtEffectiveStartDateNew.Text).AddYears(2)
        '    Me.txtEffectiveEndDateNew.Text = twoYear
        'End If
    End Sub





    Protected Sub gvAddSessionDetails_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvAddSessionDetails.RowDeleting
        Dim DeleteItem As Integer = e.RowIndex
        Dim sDDataList As List(Of Model.SessionInformationDetails)
        sDDataList = CType(Session("SDDataList"), List(Of Model.SessionInformationDetails))
        sDDataList.RemoveAt(e.RowIndex)
        Me.gvAddSessionDetails.DataSource = sDDataList
        Session("SDDataList") = sDDataList
        Me.gvAddSessionDetails.DataBind()
        Me.BntSubitAll.Enabled = Me.CEsDayTotalEqualCourseCES()

    End Sub

    Private Sub rblNewRnTypeSelect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblNewRnTypeSelect.SelectedIndexChanged
        Dim MCService As IManageCourseService = StructureMap.ObjectFactory.GetInstance(Of IManageCourseService)()
        Dim dataLevel As New List(Of Model.LevelsDetails)
        If rblNewRnTypeSelect.SelectedValue = 0 Then 'This is RN  2:
            txtCategoryACWSNew.Visible = True
            rfvCategoryACES.Enabled = True
            lblCategoryACES.Visible = True
            dataLevel = MCService.GetLevelsByRoleID(2)       '
            Me.ddlLevelNew.Items.Clear()
            Me.ddlLevelNew.Items.Add(New ListItem("-- Select Level --", -1))
            For Each t As Model.LevelsDetails In dataLevel
                Me.ddlLevelNew.Items.Add(New ListItem(t.Level_Code, t.Level_Type_Sid))
            Next
            If rfvCourseNumberRnNew.Enabled = False Then
                rfvCourseNumberRnNew.Enabled = True
            End If
            'Need to reload the RN list to only show RN trainers if the user is a master RN or Admin.
            Select Case True
                Case SessionHelper.MAISLevelUserRole = Enums.RoleLevelCategory.RNMaster_RLC Or UserAndRoleHelper.IsUserAdmin
                    Me.ReloadRNbyRole(4)
                Case Else
                    'Do Nothing
            End Select

        Else ' This is DD 
            rfvCourseNumberRnNew.Enabled = False

            txtCategoryACWSNew.Visible = False
            rfvCategoryACES.Enabled = False
            lblCategoryACES.Visible = False

            dataLevel = MCService.GetLevelsByRoleID(3)
            Me.ddlLevelNew.Items.Clear()
            Me.ddlLevelNew.Items.Add(New ListItem("-- Select Level --", -1))
            For Each t As Model.LevelsDetails In dataLevel
                Me.ddlLevelNew.Items.Add(New ListItem(t.Level_Code, t.Level_Type_Sid))
            Next

            'Need to reload the RN list to only show RN trainers if the user is a master RN or Admin. 
            Select Case True
                Case SessionHelper.MAISLevelUserRole = Enums.RoleLevelCategory.RNMaster_RLC Or UserAndRoleHelper.IsUserAdmin
                    Me.ReloadRNbyRole(1)
                Case Else
                    'Do Nothing
            End Select
        End If

        Select Case hdfMAISRole.Value
            Case "RN Instructor", "RN Trainer"
                Me.RequiredFieldValidator1.Enabled = False
            Case Else
                Me.RequiredFieldValidator1.Enabled = True
        End Select

    End Sub

    Protected Sub ddlLevelNew_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlLevelNew.SelectedIndexChanged
        Dim MCService As IManageCourseService = StructureMap.ObjectFactory.GetInstance(Of IManageCourseService)()
        Dim LevelID = ddlLevelNew.SelectedValue

        If LevelID <> -1 Then
            Dim DataCategory As New List(Of CategoryDetails)
            Me.ddlCategoryNew.Items.Clear()

            DataCategory = MCService.GetCategories(LevelID)
            Me.ddlCategoryNew.Items.Add(New ListItem("-- Select Cat. -- ", -1))
            For Each c As Model.CategoryDetails In DataCategory
                Me.ddlCategoryNew.Items.Add(New ListItem(c.CategoryCode_With_Desc, c.Category_Type_Sid))
            Next


        End If

    End Sub

    Public Sub SetCourseFieldsEnable(ByVal SetValue As Boolean)
        Me.ddlRnNames.Enabled = SetValue
        Me.txtEffectiveStartDateNew.Enabled = SetValue
        Me.txtEffectiveEndDateNew.Enabled = SetValue
        Me.txtCourseNumberRnNew.Enabled = SetValue

        Me.txtCategoryACWSNew.Enabled = SetValue
        Me.txtTotalCEsNew.Enabled = SetValue
        Me.ddlLevelNew.Enabled = SetValue
        Me.ddlCategoryNew.Enabled = SetValue
        Me.txtCourseDescriptionNew.Enabled = SetValue
        Me.bntSyllabusUpload.Enabled = SetValue
        Me.grvSyllabus.Enabled = SetValue
        Me.uplSyllabusUpload.Enabled = SetValue
        Me.rblNewRnTypeSelect.Items(0).Enabled = SetValue
        Me.rblNewRnTypeSelect.Items(1).Enabled = SetValue

    End Sub

    Public Sub SetCourseSessionFieldsEnable(ByVal SetValue As Boolean)
        Me.txtSessionStartDateNew.Enabled = SetValue
        Me.txtSessionEndDateNew.Enabled = SetValue
        Me.txtSessionLocationNew.Enabled = SetValue
        Me.txtSessionAddressNew.Enabled = SetValue
        Me.txtSessionCityNew.Enabled = SetValue
        Me.ddlSessionStateNew.Enabled = SetValue
        Me.txtSessionZipNew.Enabled = SetValue
        Me.ddlSessionCounty.Enabled = SetValue
        Me.txtSessionZipPlusFour.Enabled = SetValue
    End Sub
    Public Sub SessionFieldReset()
        Me.txtSessionStartDateNew.Text = String.Empty
        Me.txtSessionEndDateNew.Text = String.Empty
        Me.txtSessionLocationNew.Text = String.Empty
        Me.txtSessionSponserNew.Text = String.Empty
        Me.txtSessionAddressNew.Text = String.Empty
        Me.txtSessionCityNew.Text = String.Empty
        Me.txtSessionZipNew.Text = String.Empty
        Me.ddlSessionStateNew.SelectedValue = "35"
    End Sub

    <WebMethod()>
   <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function DoesCourseNumberExist(ByVal CourseNumber As Dictionary(Of String, String)) As Object
        Dim MCService As IManageCourseService = StructureMap.ObjectFactory.GetInstance(Of IManageCourseService)()
        Dim Flag As Boolean = MCService.DoesCourseExitAlready(CourseNumber("CourseNumber"))
        'False: no match in database
        'True: Match in database
        Dim jason As Object = Nothing
        jason = Flag

        Return jason
    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function DoesSessionOverlap(ByVal SessionData As Dictionary(Of String, String)) As Object
        Dim MCService As IManageCourseService = StructureMap.ObjectFactory.GetInstance(Of IManageCourseService)()
        Dim Flag As Boolean = MCService.DoeseCourseSessionOverLap(SessionData("CourseNumber"), SessionData("SessionStartDate"), SessionData("SessionEndDate"))

        Dim jason As Object = Nothing
        jason = Flag

        Return jason

    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function GetRNsCerificationDate(ByVal RnData As Dictionary(Of String, String)) As Object
        Dim MAISService As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Dim ReturnDate As Date = MAISService.GetCertificationDateThatIsHighRoleProiorityByRNSID(RnData("RNsID"), RnData("StartDate"))
        Dim jason As Object = Nothing
        jason = ReturnDate.ToShortDateString

        Return jason
    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function GetRnsCertificationMinStartDate(ByVal RnData As Dictionary(Of String, String)) As Object
        Dim MAISService As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Dim ReturnDate As Date = MAISService.GetCertificationMinStartDateByRNSID(RnData("RNsID"))
        Dim jason As Object = Nothing
        jason = ReturnDate.ToShortDateString

        Return jason
    End Function
#Region "Saving Data"

    Protected Sub BntSaveCourseData_Click(sender As Object, e As EventArgs) Handles BntSaveCourseData.Click
        Dim MCService As IManageCourseService = StructureMap.ObjectFactory.GetInstance(Of IManageCourseService)()

        Dim NewCourse As New CourseDetails
        With NewCourse
            If Me.RequiredFieldValidator1.Enabled = False Then ' the control will not post back to the server if Enable is False. Need to set the RN From the Hiden field
                .RN_Sid = hdfRNID.Value
            Else
                .RN_Sid = ddlRnNames.SelectedValue
            End If

            .StartDate = txtEffectiveStartDateNew.Text
            .EndDate = txtEffectiveEndDateNew.Text
            If (rblNewRnTypeSelect.SelectedValue = 0) Then
                .OBNApprovalNumber = txtCourseNumberRnNew.Text
            End If
            If txtCategoryACWSNew.Visible = True Then
                .CategoryACEs = txtCategoryACWSNew.Text
            End If

            .TotalCEs = txtTotalCEsNew.Text
            .Level = ddlLevelNew.SelectedValue
            .Category = ddlCategoryNew.SelectedValue
            .CourseDescription = txtCourseDescriptionNew.Text
        End With

        Dim SaveTest As Integer = MCService.SaveMangeCourse(NewCourse)


        If SaveTest <> -1 Then
            Me.BntSaveCourseData.Visible = False
            'Need to lock all the Fields. Set them to enable = false
            Me.htMode.Value = "AddSession"

            If rblNewRnTypeSelect.SelectedValue = 1 Then
                'Need to set the DODD Course number to show to the user.
                setDODDCourseNumber(SaveTest)
            End If

            Session("NewCourseID") = SaveTest

            Me.SaveToUDS()
            Me.TestMode()

        End If
    End Sub

    Private Sub setDODDCourseNumber(ByVal CourseSID As Integer)
        Dim CourseInfo As New CourseDetails
        Dim MCService As IManageCourseService = StructureMap.ObjectFactory.GetInstance(Of IManageCourseService)()

        CourseInfo = MCService.getCourseByCourseID(CourseSID)
        Me.txtCourseNumberRnNew.Text = CourseInfo.OBNApprovalNumber
        Me.txtCourseDescriptionNew.DataBind()

    End Sub

#End Region

    Private Function CEsDayTotalEqualCourseCES() As Boolean
        If Session("SDDataList") Is Nothing Then
            Return False
        Else
            Dim SDDataList As List(Of Model.SessionInformationDetails) = CType(Session("SDDataList"), List(Of Model.SessionInformationDetails))
            Dim TotalCouseCE As Double = txtTotalCEsNew.Text
            Dim DayCEs As Double = 0.0

            For Each ce In SDDataList
                DayCEs += ce.Total_CEs
            Next
            If DayCEs = TotalCouseCE Then
                lblAddSessionError.Text = String.Empty


                Return True
            Else
                Me.lblAddSessionError.Text = "The total CEs of " & DayCEs & " do not match the Course Total CEs of " & TotalCouseCE & "."
                Return False
            End If
        End If
    End Function

    Protected Sub BntSubitAll_Click(sender As Object, e As EventArgs) Handles BntSubitAll.Click
        Dim NewCourseInfo As New CourseDetails


        With NewCourseInfo
            If BntSaveCourseData.Visible = False Then
                Dim CID As Integer = CType(Session("NewCourseID"), Integer)
                NewCourseInfo.Course_Sid = CID
            End If
            If Me.RequiredFieldValidator1.Enabled = False Then ' the control will not post back to the server if Enable is False. Need to set the RN From the Hiden field
                .RN_Sid = hdfRNID.Value
            Else
                .RN_Sid = ddlRnNames.SelectedValue
            End If
            .StartDate = txtEffectiveStartDateNew.Text
            .EndDate = txtEffectiveEndDateNew.Text
            If (rblNewRnTypeSelect.SelectedValue = 0) Then
                .OBNApprovalNumber = txtCourseNumberRnNew.Text
            End If
            If txtCourseDescriptionNew.Visible = True Then
                .CategoryACEs = txtCategoryACWSNew.Text
            End If

            .TotalCEs = txtTotalCEsNew.Text
            .Level = ddlLevelNew.SelectedValue
            .Category = ddlCategoryNew.SelectedValue
            .CourseDescription = txtCourseDescriptionNew.Text
        End With

        Dim SDL As New List(Of SessionAddressInformation)

        Dim SDI As New SessionAddressInformation
        With SDI
            .Session_Start_Date = Me.txtSessionStartDateNew.Text
            .Session_End_Date = Me.txtSessionEndDateNew.Text
            .Location_Name = txtSessionLocationNew.Text
            .Total_CEs = txtTotalCEsNew.Text
            .Sponsor = txtSessionSponserNew.Text
            .Public_Access_Flg = ckbPublicView.Checked
            Dim SDA As New SessionAddress

            SDA.Address_Line1 = txtSessionAddressNew.Text
            SDA.Address_Type_Sid = 6
            SDA.City = txtSessionCityNew.Text
            SDA.State = ddlSessionStateNew.SelectedItem.ToString
            SDA.StateID = ddlSessionStateNew.SelectedValue
            SDA.Zip_Code = txtSessionZipNew.Text
            SDA.Zip_Code_Plus4 = txtSessionZipPlusFour.Text
            SDA.County = ddlSessionCounty.SelectedItem.ToString
            SDA.countyID = ddlSessionCounty.SelectedValue
            SDI.SessionAddressInfo = SDA

            Dim sDDataListSession As List(Of Model.SessionInformationDetails)
            Dim sDDataList As New List(Of Model.SessionInformationDetails)

            sDDataListSession = CType(Session("SDDataList"), List(Of Model.SessionInformationDetails))
            For Each sddi As SessionInformationDetails In sDDataListSession
                sDDataList.Add(sddi)
            Next
            SDI.SessionInformationDetailsList = sDDataList
            SDL.Add(SDI)
        End With
        NewCourseInfo.SessionDetailList = SDL



        Dim mcService As IManageCourseService = StructureMap.ObjectFactory.GetInstance(Of IManageCourseService)()
        'SaveToUDS()

        Dim flag As Integer
        flag = mcService.SaveMangeCourse(NewCourseInfo)
        If flag > 0 Then
            Master.SetError("")
            Me.htMode.Value = "Search"


            SaveToUDS(flag)
            TestMode()
            'Send search grid to new course session data
            ViewNewCourseSessionDatainGridSearchGridView(flag)
        Else
            Master.SetError("An error has occurred in the system. The data has not be saved.")
        End If
    End Sub
#End Region

#Region "Search"


    Protected Sub bntSearch_Click(sender As Object, e As EventArgs) Handles bntSearch.Click
        Dim MCService As IManageCourseService = StructureMap.ObjectFactory.GetInstance(Of IManageCourseService)()
        Dim mCourseData As New List(Of Model.CourseDetails)

        mCourseData = MCService.SearchMangeCourse(Me.txtRNNO.Text, Trim(Me.txtFirstName.Text), Trim(Me.txtLName.Text), IIf(String.IsNullOrWhiteSpace(Me.txtSearchSessionStartDate.Text), New Date, Me.txtSearchSessionStartDate.Text))

        'Select Case False
        '    Case String.IsNullOrWhiteSpace(txtRNNO.Text)
        '        mCourseData = MCService.SearchCourseByRN(txtRNNO.Text)
        '    Case String.IsNullOrWhiteSpace(txtFirstName.Text)


        '        mCourseData = MCService.SearchCourseByName(txtFirstName.Text, "FirstName")
        '    Case String.IsNullOrWhiteSpace(txtLName.Text)
        '        mCourseData = MCService.SearchCourseByName(txtLName.Text, "LastName")
        '    Case String.IsNullOrWhiteSpace(txtSearchSessionStartDate.Text)
        '        mCourseData = MCService.SearchSessionByStartDate(CDate(txtSearchSessionStartDate.Text))

        'End Select



        Me.grdCourse.DataSource = mCourseData
        Me.grdCourse.DataBind()


    End Sub


    Protected Sub OnRowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdCourse.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim customerId As String = grdCourse.DataKeys(e.Row.RowIndex).Value.ToString()
            Dim grdSession As GridView = TryCast(e.Row.FindControl("grdSession"), GridView)
            Dim mRepeter As Repeater = TryCast(e.Row.FindControl("rpSyllabus"), Repeater)

            e.Row.Cells(11).Visible = TestUserAllowedToAddSession(e.Row.DataItem.RN_Sid)
            mRepeter.DataSource = getCourseSyllabus(e.Row.DataItem.OBNApprovalNumber)

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
            mRepeter.DataBind()

        End If

    End Sub

    Private Function getCourseSyllabus(ByVal ObnApprovealNumber As String) As List(Of MAIS.Web.UDSWebService.IndexedDocument)
        'Dim RetVal As New List(Of MAIS.Web.UDSWebService.IndexedDocument)
        Dim wsUDS As New UDSWebService.UDSService() With {.Url = ConfigHelper.UDSServiceEndpoint}
        Dim SearchDS As New DataSet

        Dim repositoryName As String = "UDS - MA"

        Dim xml As String = "<Table> <Index> <Label> Course Number </Label> <Value> " + ObnApprovealNumber + "</Value> </Index>" _
                           + "</Table>"
        Dim Render = New System.IO.StringReader(xml)
        SearchDS.ReadXml(Render)
        Dim randomNum = (New Random()).Next(0, 999999999).ToString.PadLeft(10, "0")
        Dim Int As Integer?

        Dim ListDoc = wsUDS.SearchUDS(repositoryName, Int, Int, "", "", "", "", "", "", "", "", SearchDS)
        For Each e In ListDoc
            Dim udsUrl As String = e.DownloadURL.Replace("download.aspx", "madownload.aspx")
            e.DownloadURL = udsUrl + "&UID=" & randomNum & MAIS_Helper.GetUserId & randomNum
        Next
        Return ListDoc.ToList

    End Function

    Protected Sub OnSessionRowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim grdSessionDates As GridView = TryCast(e.Row.FindControl("grdSessionDates"), GridView)

            e.Row.Cells(10).Visible = TestUserAllowedToAddSession(CType(e.Row.Parent.DataItemContainer, GridViewRow).DataItem.RN_Sid)

            grdSessionDates.DataSource = CType(e.Row.DataItem, SessionAddressInformation).SessionInformationDetailsList
            grdSessionDates.DataBind()
        End If
    End Sub

    Private Sub grdCourse_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdCourse.RowCommand


        Select Case e.CommandName
            Case "AddSession"
                Me.htMode.Value = "AddSession"
                Dim courseID As Integer
                courseID = CType(e.CommandArgument, Integer)
                AddNewSession(courseID)
            Case "DeleteSession"
                'Dim SessionID As Integer
                'SessionID = CType(e.CommandArgument, Integer)
                DeleteSession(CType(e.CommandArgument, Integer))

        End Select



    End Sub

    Private Sub DeleteSession(ByVal SessionID As Integer)
        Dim MCService As IManageCourseService = StructureMap.ObjectFactory.GetInstance(Of IManageCourseService)()
        Dim saved As Boolean
        If SessionID > 0 Then
            saved = MCService.DeleteSessionBySessionSid(SessionID)

            If saved = True Then
                bntSearch_Click(Me, New System.EventArgs)
            End If
        End If

    End Sub

    Private Sub AddNewSession(ByVal CourseID As Integer)
        Me.htMode.Value = "AddSession"
        Dim CourseInfo As New CourseDetails
        Dim MCService As IManageCourseService = StructureMap.ObjectFactory.GetInstance(Of IManageCourseService)()

        CourseInfo = MCService.getCourseByCourseID(CourseID)

        If CourseInfo Is Nothing Then
            Exit Sub
        End If

        'turn of search panel
        Me.dvMainSearch.Visible = False
        Me.dvNewRnType.Visible = True
        LoadNewCourseDropDownList()
        loadNewStateDropDownList()
        LoadCounties()
        Me.ddlSessionStateNew.SelectedValue = "35"
        'Me.DataBind()

        If CourseInfo.OBNApprovalNumber.Contains("DODD") Then
            Me.rblNewRnTypeSelect.SelectedValue = 1
        Else
            Me.rblNewRnTypeSelect.SelectedValue = 0
        End If

        'Need to load the Drop down list for level
        rblNewRnTypeSelect_SelectedIndexChanged(Me.rblNewRnTypeSelect, New EventArgs)


        'set Course data field

        Me.ddlRnNames.SelectedValue = CourseInfo.RN_Sid
        Me.txtEffectiveStartDateNew.Text = CDate(CourseInfo.StartDate).ToShortDateString
        Me.txtEffectiveEndDateNew.Text = CDate(CourseInfo.EndDate).ToShortDateString
        Me.txtCourseNumberRnNew.Text = CourseInfo.OBNApprovalNumber
        Me.txtCategoryACWSNew.Text = CourseInfo.CategoryACEs
        Me.txtTotalCEsNew.Text = CourseInfo.TotalCEs
        Me.ddlLevelNew.SelectedValue = CourseInfo.Level
        Me.ddlLevelNew.DataBind()

        'need to laod the Catgory drop down list after the level is value is set. 
        ddlLevelNew_SelectedIndexChanged(Me.ddlLevelNew, New EventArgs)

        Me.ddlCategoryNew.SelectedValue = CourseInfo.Category
        Me.ddlCategoryNew.DataBind()

        Me.txtCourseDescriptionNew.Text = CourseInfo.CourseDescription

        TestMode()
        Session("NewCourseID") = CourseInfo.Course_Sid
    End Sub

    Private Sub ViewNewCourseSessionDatainGridSearchGridView(ByVal CourseID As Integer)
        Dim MCSerivce As IManageCourseService = StructureMap.ObjectFactory.GetInstance(Of IManageCourseService)()
        Dim CList As New List(Of Model.CourseDetails)
        CList = MCSerivce.GetAllCourseAndSessionInfoByCourseID(CourseID)
        Me.grdCourse.DataSource = CList
        Me.grdCourse.DataBind()
    End Sub

#End Region



    Protected Sub bntSyllabusUpload_Click(sender As Object, e As EventArgs) Handles bntSyllabusUpload.Click

        Dim BytestForFileToSaveToDatabase(Me.uplSyllabusUpload.PostedFile.InputStream.Length - 1) As Byte
        Dim SyllabusHold As New List(Of Model.DocumentUpload)

        If Session("Syllabus") IsNot Nothing Then
            SyllabusHold = CType(Session("Syllabus"), List(Of Model.DocumentUpload))
        End If
        uplSyllabusUpload.PostedFile.InputStream.Read(BytestForFileToSaveToDatabase, 0, BytestForFileToSaveToDatabase.Length)


        If BytestForFileToSaveToDatabase.Length > 0 Then
            'Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
            'Dim uploadDocumentNumber As Long = uploadService.InsertUploadedDocumentIntoImageStore(BytestForFileToSaveToDatabase)
            Dim SyllabusHold1 As New Model.DocumentUpload
            'If uploadDocumentNumber > 0 Then
            With SyllabusHold1
                .ImageSID = SyllabusHold.Count + 1
                .DocumentTypeID = 4 ' 4 = Syllabus in the Document_Type table in the database
                .DocumentType = "Syllabus"
                .DocumentName = uplSyllabusUpload.FileName
                .FolderName = "Syllabus"
                .SourcePage = "ManageCourse.aspx"
                .UploadDate = Today
                .DocumentImage = BytestForFileToSaveToDatabase
            End With
            SyllabusHold.Add(SyllabusHold1)

            'End If
            Session("Syllabus") = SyllabusHold

            grvSyllabus.DataSource = SyllabusHold
            grvSyllabus.DataBind()


        End If

    End Sub

    Private Sub grvSyllabus_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grvSyllabus.RowDeleting
        Dim SyllabusHold = CType(Session("Syllabus"), List(Of Model.DocumentUpload))

        SyllabusHold.Remove(SyllabusHold(e.RowIndex))

        Session("Syllabus") = SyllabusHold
        Me.grvSyllabus.DataSource = SyllabusHold
        Me.grvSyllabus.DataBind()
    End Sub

    Protected Sub grvSyllabus_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles grvSyllabus.SelectedIndexChanging
        Dim byteArrayForFileToView As Byte()
        Dim SyllabusHold = CType(Session("Syllabus"), List(Of Model.DocumentUpload))
        byteArrayForFileToView = SyllabusHold(e.NewSelectedIndex).DocumentImage

        Response.AddHeader("Content-Disposition", "attachment; filename=" & _
            SyllabusHold(e.NewSelectedIndex).DocumentName)

        Response.AddHeader("Content-Length", byteArrayForFileToView.Length.ToString)

        'Set HTTP MIME type for output stream
        Response.ContentType = "application/octet-stream"

        'Output the data to the client
        If byteArrayForFileToView.Length > 0 Then
            Dim ext = System.IO.Path.GetExtension(SyllabusHold(e.NewSelectedIndex).DocumentName)
            If ext = ".html" Then
            Else
                Response.BinaryWrite(byteArrayForFileToView)
            End If
            Response.End()
        End If
    End Sub

    Protected Sub lkbBack_Click(sender As Object, e As EventArgs) Handles lkbBack.Click
        Session("SDDataList") = Nothing
        Session("NewCourseID") = Nothing
        Session("Syllabus") = Nothing
        Me.bntCancelCours_Click(sender, e)
    End Sub


    Private Sub SaveToUDS(Optional CourseID As Integer = 0)
        Dim SyllabusHold = CType(Session("Syllabus"), List(Of Model.DocumentUpload))
        Dim strError As String = String.Empty

        If SyllabusHold IsNot Nothing Then
            Dim maisSerivce As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()

            Dim UserName As String = "N/A"
            Dim RNNumber As String
            If Me.RequiredFieldValidator1.Enabled = False Then ' the control will not post back to the server if Enable is False. Need to set the RN From the Hiden field
                UserName = maisSerivce.GetRNsName(hdfRNID.Value)
                'RNNumber = maisSerivce.GetRNsLicenseNumber(hdfRNID.Value).Replace("RN", "")
                RNNumber = maisSerivce.GetRNsLicenseNumber(hdfRNID.Value)
            Else
                UserName = maisSerivce.GetRNsName(ddlRnNames.SelectedValue)
                'RNNumber = maisSerivce.GetRNsLicenseNumber(ddlRnNames.SelectedValue).Replace("RN", "")
                RNNumber = maisSerivce.GetRNsLicenseNumber(ddlRnNames.SelectedValue)
            End If
            Dim CourseNumber As String = String.Empty
            If courseID > 0 Then
                Dim CourseInfo As New CourseDetails
                Dim MCService As IManageCourseService = StructureMap.ObjectFactory.GetInstance(Of IManageCourseService)()

                CourseInfo = MCService.getCourseByCourseID(CourseID)
                CourseNumber = CourseInfo.OBNApprovalNumber
            Else
                CourseNumber = Me.txtCourseNumberRnNew.Text
            End If

            UserName = UserName.Replace(" ", ".")

            Dim wsUDS As New UDSWebService.UDSService() With {.Url = ConfigHelper.UDSServiceEndpoint}

            Dim repositoryName As String = "UDS - MA"

            Dim wsDataSet As New DataSet 'wsUDS.GetGenericIndexes(repositoryName)
            'Dim dataColumnToAdd As New DataColumn
            'dataColumnToAdd.ColumnName = "Value"
            'wsDataSet.Tables(0).Columns.Add(dataColumnToAdd)

            ' Dim rows = wsDataSet.Tables(0).Rows.OfType(Of DataRow)()

            Dim xml As String = "<Table> <Index> <Label> Course Number </Label> <Value> " + CourseNumber + "</Value> </Index>" _
                                + "<Index> <Label> Application Type </Label> <Value> Other </Value> </Index>" _
                                + "<Index> <Label> NAME </Label> <Value> " + UserName + " </Value> </Index>" _
                                + "<Index> <Label> RN Lics or 4 SSN </Label> <Value> " + RNNumber + " </Value> </Index>" _
                                + "<Index> <Label> DOB </Label> <Value> </Value> </Index>" _
                                + "<Index> <Label> Personnel Type </Label>  <Value> RN </Value> </Index>" _
                                + "<Index> <Label> CERT STATUS </Label> <Value>  </Value> </Index>" _
                                + "<Index> <Label>  Application ID </Label> <Value>  </Value> </Index>" _
                                + "<Index> <Label>  DD PERSONNEL CODE </Label> <Value> </Value> </Index>" _
                                + "</Table>"
            Dim Render = New System.IO.StringReader(xml)
            wsDataSet.ReadXml(Render)

            ' rows.FirstOrDefault(Function(r) r("label") = "Course Number")("Value") = Me.txtCourseNumberRnNew.Text
            Dim Document_Int As Integer = 0
            For Each s In SyllabusHold
                Document_Int += 1
                Dim byteArrayForUDS As Byte() = s.DocumentImage
                Dim DocumentName As String = CourseNumber + "_" + Document_Int.ToString + "_" + s.DocumentName

                Dim result As UDSWebService.Result = wsUDS.SaveToUDS(repositoryName, _
                                                                     byteArrayForUDS, _
                                                                     MAIS_Helper.GetUser.UserName, _
                                                                     DocumentName, _
                                                                     "SYLLABUS", _
                                                                     "COURSE SYLLABUS", _
                                                                     Today, _
                                                                     String.Empty, _
                                                                     wsDataSet)



                Select Case result
                    'Case UDSWebService.Result.AlreadyExists
                    '   strError = "UDSWebService.Result.AlreadyExists"
                    Case UDSWebService.Result.EmptyFile
                        strError = "UDSWebService.Result.EmptyFile"
                        'blnSaveFileToUDS = False
                    Case UDSWebService.Result.InError
                        strError = "UDSWebService.Result.InError"
                        'blnSaveFileToUDS = False
                    Case UDSWebService.Result.Invalid
                        strError = "UDSWebService.Result.Invalid"
                        'blnSaveFileToUDS = False
                    Case UDSWebService.Result.Success
                        'blnSaveFileToUDS = True

                End Select
            Next



        End If


    End Sub


    Protected Sub grdCourse_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdCourse.SelectedIndexChanged


    End Sub

    Protected Sub ddlCategoryNew_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCategoryNew.SelectedIndexChanged
        Select Case ddlCategoryNew.SelectedValue
            Case Enums.CategoryType.Cat1
                Me.rvTotalCES.Enabled = True
                Me.rvTotalCES.MinimumValue = 14.0
                Me.rvTotalCES.ErrorMessage = "Minimum value of 14.0 is needed for Total CES for Cat-I. "

            Case Enums.CategoryType.CatIII
                Me.rvTotalCES.Enabled = True
                Me.rvTotalCES.MinimumValue = 4
                Me.rvTotalCES.ErrorMessage = "Minimum value of 4.0 is needed for Total CES for Cat-II. "

            Case Enums.CategoryType.CatIV
                Me.rvTotalCES.Enabled = True
                Me.rvTotalCES.MinimumValue = 4.0
                Me.rvTotalCES.ErrorMessage = "Minimum value of 4.0 is needed for Total CES for Cat- III. "

            Case Enums.CategoryType.CatVI
                Me.rvTotalCES.Enabled = True
                Me.rvTotalCES.MinimumValue = 8.0
                Me.rvTotalCES.ErrorMessage = "Minimum value of 8.0 is needed for Total CES for RN Trainer. "

            Case 5 'This is the 17+Bed RN

                Me.rvTotalCES.Enabled = True
                Me.rvTotalCES.MinimumValue = 4.0
                Me.rvTotalCES.ErrorMessage = "Minimum value of 8.0 is needed for Total CES for 17+bed RN. "
            Case Else
                Me.rvTotalCES.Enabled = False
        End Select
    End Sub
End Class
