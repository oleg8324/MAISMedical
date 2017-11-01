Imports System.Web.Script.Services
Imports MAIS.Business.Model
Imports MAIS.Business.Services
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business
Imports MAIS.Business.Model.Enums
Imports ODMRDDHelperClassLibrary
Imports ODMRDDHelperClassLibrary.ODMRDDServiceProvider

Public Class Skills
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If SessionHelper.ApplicationType.Replace(" ", "") = Enums.ApplicationType.UpdateProfile.ToString Then
                Master.HideProgressBar = True
                Me.pnlFooterNav.Visible = False
            End If
            Me.LoadSkillAddData()
            Me.LoadGridData()
            Me.hdfDDPeronalID.Value = SessionHelper.SessionUniqueID
              Select SessionHelper.ApplicationType.Replace(" ", "")
                Case Enums.ApplicationType.Initial.ToString, Enums.ApplicationType.AddOn.ToString
                    Me.tdSkillsSelectAll.Visible = True
                    Me.tdSkillsSelectAll2.Visible = True
                Case Else
                    Me.tdSkillsSelectAll.Visible = False
                    Me.tdSkillsSelectAll2.Visible = False
            End Select

        End If
    End Sub

    Private Sub LoadSkillAddData()
        Dim SKService As ISkillPageService = StructureMap.ObjectFactory.GetInstance(Of ISkillPageService)()

        If SessionHelper.ApplicationType.Replace(" ", "") = Enums.ApplicationType.UpdateProfile.ToString Then
            Me.txtCatetory.Visible = False
            Me.ddlSkillCategory.Visible = True

            Dim Cat As New List(Of Model.CategoryDetails)
            Cat = SKService.GetSkillCategorys

            Me.ddlSkillCategory.Items.Clear()

            Me.ddlSkillCategory.Items.Add(New ListItem("--Select Cat--", "-1"))

            For Each i In Cat
                Me.ddlSkillCategory.Items.Add(New ListItem(i.Category_Code, i.Category_Type_Sid))
            Next
        Else
            Dim MaisService As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
            Dim RoleCategoryDetails As New MAIS.Business.Model.RoleCategoryLevelDetails

            RoleCategoryDetails = MaisService.GetRoleCategoryLevelInfoByRoleCategoryLevelSid(SessionHelper.SelectedUserRole)
            txtCatetory.Text = RoleCategoryDetails.Category_Type_Name
            txtCatetory.Attributes.Add("Tag", RoleCategoryDetails.Category_Type_Sid)
            Me.LoadSkillCategoryByCategoryID(RoleCategoryDetails.Category_Type_Sid)

        End If


    End Sub
    Private Sub LoadGridData()
        Dim SKService As ISkillPageService = StructureMap.ObjectFactory.GetInstance(Of ISkillPageService)()

        Dim GidDataList As New List(Of Model.SkillVerificationTypeCheckListOnly)
        Select Case SessionHelper.ApplicationType
            Case Enums.ApplicationType.Initial.ToString, Enums.ApplicationType.AddOn.ToString
                GidDataList = SKService.GetSkillVerificationCheckListData(0, SessionHelper.ApplicationID)
            Case Else
                If SessionHelper.ApplicationID > 0 Then
                    GidDataList = SKService.GetSkillVerificationCheckListData(SessionHelper.SessionUniqueID, SessionHelper.ApplicationID)
                Else
                    GidDataList = SKService.GetSkillVerificationCheckListData(SessionHelper.SessionUniqueID)
                End If


        End Select
        Me.grvSkillsData.DataSource = GidDataList
        Me.grvSkillsData.DataBind()
    End Sub
    Protected Sub btnSaveAndContinue_Click(sender As Object, e As EventArgs) Handles btnSaveAndContinue.Click
        Response.Redirect(PagesHelper.GetNextPage(Master.CurrentPage))
    End Sub

    Protected Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Response.Redirect(PagesHelper.GetPreviousPage(Master.CurrentPage))
    End Sub

    Private Sub ddlSkillCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSkillCategory.SelectedIndexChanged
        'Load the Skills Drop Down Box
        If ddlSkillCategory.SelectedValue <> -1 Then
            Dim SKService As ISkillPageService = StructureMap.ObjectFactory.GetInstance(Of ISkillPageService)()
            Dim skList As New List(Of Model.SkillDetails)


            skList = SKService.GetSKillListByCategory(Me.ddlSkillCategory.SelectedValue)
            Me.ddlSkillVerified.Items.Clear()

            Me.ddlSkillVerified.Items.Add(New ListItem("-- Select Skill --", -1))
            For Each i In skList
                Me.ddlSkillVerified.Items.Add(New ListItem(i.Skill_Desc, i.SKill_Type_Sid))
            Next
        End If


    End Sub

    Private Sub LoadSkillCategoryByCategoryID(ByVal CategoryID As Integer)
        Dim SKService As ISkillPageService = StructureMap.ObjectFactory.GetInstance(Of ISkillPageService)()
        Dim skList As New List(Of Model.SkillDetails)


        skList = SKService.GetSKillListByCategory(CategoryID)
        Me.ddlSkillVerified.Items.Clear()

        Me.ddlSkillVerified.Items.Add(New ListItem("-- Select Skill --", -1))
        For Each i In skList
            Me.ddlSkillVerified.Items.Add(New ListItem(i.Skill_Desc, i.SKill_Type_Sid))
        Next

    End Sub

    Private Sub ddlSkillVerified_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSkillVerified.SelectedIndexChanged
        'load the Skills check list check box
        If ddlSkillVerified.SelectedValue <> -1 Then
            Dim SKService As ISkillPageService = StructureMap.ObjectFactory.GetInstance(Of ISkillPageService)()
            Dim SkCheckList As New List(Of Model.SkillCheckListDetails)

            SkCheckList = SKService.GetSkillCheckListbySkillID(ddlSkillVerified.SelectedValue)

            Me.cklSkillCheckList.DataSource = SkCheckList

            Me.cklSkillCheckList.DataBind()
            If cklSkillCheckList.Items.Count = 1 And cklSkillCheckList.Items(0).Text = "None" Then
                cklSkillCheckList.Items(0).Selected = True
                cklSkillCheckList.Items(0).Enabled = False
                cvSkillcheckList.Enabled = False
            Else
                cklSkillCheckList.Enabled = True
                cvSkillcheckList.Enabled = True
            End If

        End If
    End Sub

    Protected Sub bntSkillAdd_Click(sender As Object, e As EventArgs) Handles bntSkillAdd.Click
        Dim SKService As ISkillPageService = StructureMap.ObjectFactory.GetInstance(Of ISkillPageService)()
        Dim SaveData As New Model.SkillsVerificationDetails
        Dim MaisService As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()

        If Me.ckbSkillsSelectAll.Checked = True Then
            SaveAllSkills()
        Else

            Dim SaveToTemp As Boolean = True
            Dim CertDate As Date
            Select Case SessionHelper.ApplicationType.Replace(" ", "")
                Case Enums.ApplicationType.Initial.ToString
                    SaveToTemp = True
                    CertDate = Now.AddMonths(6)
                Case Enums.ApplicationType.Renewal.ToString
                    SaveToTemp = False
                    CertDate = MaisService.GetCertificationDate(SessionHelper.SessionUniqueID, SessionHelper.SelectedUserRole)
                Case Enums.ApplicationType.AddOn.ToString
                    SaveToTemp = True
                    CertDate = Now.AddMonths(6)
                Case Enums.ApplicationType.UpdateProfile.ToString
                    SaveToTemp = False
                    CertDate = MaisService.GetCertificationDateByCategoryID(SessionHelper.SessionUniqueID, ddlSkillCategory.SelectedValue)
            End Select

            With SaveData
                .RN_DD_Person_Type_Xref_SID_string = SessionHelper.SessionUniqueID
                If SessionHelper.ApplicationType.Replace(" ", "") = Enums.ApplicationType.UpdateProfile.ToString Then
                    .Category_Type_Sid = ddlSkillCategory.SelectedValue
                Else
                    .Category_Type_Sid = txtCatetory.Attributes("Tag")
                End If
                .Application_Sid = SessionHelper.ApplicationID
                .Permanent_Flg = False
                .Skill_Verification_Start_Date = txtSkillsDate.Text
                .Skill_Verification_End_Date = CertDate 'Now.AddMonths(6)
                .Skill_Verification_Active_Flg = True

                .Skill_Verification_Skill_Type_Sid = ddlSkillVerified.SelectedValue


                .Skill_Verification_Skill_Type_Active_Flag = True

                Dim itl As ListItem
                For Each itl In Me.cklSkillCheckList.Items
                    If itl.Selected Then
                        Dim scl As New Model.SkillVerificatonTypeCheckListDetails
                        With scl
                            .Skill_CheckList_Sid = itl.Value
                            .Verification_Date = Me.txtSkillsDate.Text
                            .Verified_Person_Name = Me.txtSkillNamePersonVerifying.Text
                            .Verified_Person_Title = Me.txtSkillTitle.Text
                            .Active_Flg = True
                        End With
                        If .SkillCheckList Is Nothing Then
                            Dim mSkillList As New List(Of Model.SkillVerificatonTypeCheckListDetails)
                            .SkillCheckList = mSkillList
                        End If
                        .SkillCheckList.Add(scl)
                    End If
                Next

                'Save to the database in the Temp Tables. 


                'Save to the Temp Tables in MAIS
                If SKService.SaveSkillVerificationData(SaveData, SaveToTemp) = True Then
                    'Reload the Grid
                    Me.LoadGridData()
                    'reset the Date Fiels
                    Me.ResetAddFields()

                Else 'set an error alters. 
                    'set error.

                End If


                'If SKService.SaveSkillVerificationData(SaveData) = True Then
                '    'reload the grid

                'Else
                '    'Set error message
                'End If
                'Reload Grid 

            End With
        End If
    End Sub

    Private Sub SaveAllSkills()
        Dim SKService As ISkillPageService = StructureMap.ObjectFactory.GetInstance(Of ISkillPageService)()
        Dim MaisService As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Dim SaveData As New List(Of Model.SkillsVerificationDetails)
        Dim skList As New List(Of Model.SkillDetails)
        Dim Category_Type_sid As Integer
        Dim SaveToTemp As Boolean = True
        Dim CertDate As Date

        Select Case SessionHelper.ApplicationType.Replace(" ", "")
            Case Enums.ApplicationType.Initial.ToString
                SaveToTemp = True
                CertDate = Now.AddMonths(6)
            Case Enums.ApplicationType.Renewal.ToString
                SaveToTemp = False
                CertDate = MaisService.GetCertificationDate(SessionHelper.SessionUniqueID, SessionHelper.SelectedUserRole)
            Case Enums.ApplicationType.AddOn.ToString
                SaveToTemp = True
                CertDate = Now.AddMonths(6)
            Case Enums.ApplicationType.UpdateProfile.ToString
                SaveToTemp = False
                CertDate = MaisService.GetCertificationDateByCategoryID(SessionHelper.SessionUniqueID, ddlSkillCategory.SelectedValue)
        End Select

        If SessionHelper.ApplicationType.Replace(" ", "") = Enums.ApplicationType.UpdateProfile.ToString Then
            Category_Type_sid = ddlSkillCategory.SelectedValue
        Else
            Category_Type_sid = txtCatetory.Attributes("Tag")
        End If

        skList = SKService.GetSKillListByCategory(Category_Type_sid)

        For Each skListItem In skList
            Dim nSaveDate As New Model.SkillsVerificationDetails
            With nSaveDate
                .Application_Sid = SessionHelper.ApplicationID
                .Permanent_Flg = False
                .Category_Type_Sid = Category_Type_sid
                .Skill_Verification_Start_Date = txtSkillsDate.Text
                .Skill_Verification_End_Date = CertDate 'Now.AddMonths(6)
                .Skill_Verification_Active_Flg = True

                .Skill_Verification_Skill_Type_Sid = skListItem.SKill_Type_Sid


                .Skill_Verification_Skill_Type_Active_Flag = True
            End With

            Dim SkCheckList As New List(Of Model.SkillCheckListDetails)

            SkCheckList = SKService.GetSkillCheckListbySkillID(skListItem.SKill_Type_Sid)
            For Each skCheckListItem In SkCheckList
                Dim nSkillCheckList As New Model.SkillVerificatonTypeCheckListDetails
                With nSkillCheckList
                    .Skill_CheckList_Sid = skCheckListItem.Skill_CheckList_Sid
                    .Verification_Date = Me.txtSkillsDate.Text
                    .Verified_Person_Name = Me.txtSkillNamePersonVerifying.Text
                    .Verified_Person_Title = Me.txtSkillTitle.Text
                    .Active_Flg = True
                End With
                If nSaveDate.SkillCheckList Is Nothing Then
                    nSaveDate.SkillCheckList = New List(Of Model.SkillVerificatonTypeCheckListDetails)
                End If
                nSaveDate.SkillCheckList.Add(nSkillCheckList)
            Next
            SaveData.Add(nSaveDate)
        Next

        If SKService.SaveSkillVerificationData(SaveData, SaveToTemp) Then
            Me.LoadGridData()
            'reset the Date Fiels
            Me.ResetAddFields()
        End If
    End Sub
    'Protected Sub grvSkillsData_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvSkillsData.RowDataBound
    '    If (e.Row.RowType = DataControlRowType.DataRow) Then
    '        Dim bl As BulletedList = e.Row.FindControl("blSkillCheckList")
    '        bl.DataSource = CType(e.Row.DataItem, SkillsVerificationDetails).SkillCheckList
    '        bl.DataBind()

    '    End If
    'End Sub

    Protected Sub grvSkillsData_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvSkillsData.RowCommand
        'Dim SKServices As ISkillPageService = StructureMap.ObjectFactory.GetInstance(Of ISkillPageService)()

        'Select Case e.CommandName
        '    Case "Delete"
        '        'Dim ID As Integer = sender.datakeys(0).value
        '        'Dim RemoveFromTemp As Boolean = True
        '        'Select Case SessionHelper.ApplicationType.Replace(" ", "")
        '        '    Case Enums.ApplicationType.Initial.ToString, Enums.ApplicationType.AddOn.ToString
        '        '        RemoveFromTemp = True
        '        '    Case Enums.ApplicationType.Renewal.ToString
        '        '        RemoveFromTemp = False

        '        '    Case Enums.ApplicationType.UpdateProfile.ToString
        '        '        RemoveFromTemp = False
        '        'End Select
        '        'If SKServices.DeleteSkillVerificationData(ID, RemoveFromTemp) = True Then
        '        '    Me.LoadGridData()

        '        'End If
        'End Select
    End Sub

    Private Sub ResetAddFields()
        'Me.txtSkillsDate.Text = String.Empty
        ' Me.txtSkillNamePersonVerifying.Text = String.Empty
        ' Me.txtSkillTitle.Text = String.Empty
        If SessionHelper.ApplicationType = Enums.ApplicationType.UpdateProfile.ToString Then
            'Me.ddlSkillCategory.SelectedValue = -1
        End If

        Me.ddlSkillVerified.SelectedValue = -1
        Me.cklSkillCheckList.Items.Clear()

        Me.DataBind()

    End Sub
    Protected Sub grvSkillsData_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grvSkillsData.RowDeleting
        Dim SKServices As ISkillPageService = StructureMap.ObjectFactory.GetInstance(Of ISkillPageService)()
        Dim int As Integer = e.RowIndex
        Dim ID As Integer = sender.datakeys(int).value
        Dim RemoveFromTemp As Boolean = True
        Select Case SessionHelper.ApplicationType.Replace(" ", "")
            Case Enums.ApplicationType.Initial.ToString, Enums.ApplicationType.AddOn.ToString
                RemoveFromTemp = True
            Case Enums.ApplicationType.Renewal.ToString
                RemoveFromTemp = False

            Case Enums.ApplicationType.UpdateProfile.ToString
                RemoveFromTemp = False
        End Select
        If SKServices.DeleteSkillVerificationData(ID, RemoveFromTemp) = True Then
            Me.LoadGridData()
        End If


    End Sub

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function GetDDPersonalCertificationMinStartDate(ByVal DDPersonalCodeID As Dictionary(Of String, String)) As Object
        Dim MAISService As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Dim ReturnDate As Date = MAISService.GetCertificationMinStartDateByDDPersonnelCode(DDPersonalCodeID("DDPersonalCodeID"))
        Dim jason As Object = Nothing
        jason = ReturnDate.ToShortDateString
        Return jason

    End Function

    Protected Sub bntCancel_Click(sender As Object, e As EventArgs) Handles bntCancel.Click
        Me.txtSkillsDate.Text = String.Empty
        Me.txtSkillNamePersonVerifying.Text = String.Empty
        Me.txtSkillTitle.Text = String.Empty
        If SessionHelper.ApplicationType = Enums.ApplicationType.UpdateProfile.ToString Then
            Me.ddlSkillCategory.SelectedValue = -1
        End If

        Me.ddlSkillVerified.SelectedValue = -1
        Me.cklSkillCheckList.Items.Clear()
        LoadGridData()

        'Me.DataBind()
    End Sub

    Protected Sub ckbSkillsSelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles ckbSkillsSelectAll.CheckedChanged

        If Me.ckbSkillsSelectAll.Checked = True Then
            Me.tblSkills.Visible = False
        Else
            Me.tdSkillsSelectAll.Visible = True
        End If

    End Sub
End Class