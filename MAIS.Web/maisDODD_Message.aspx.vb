Imports System.Web.Script.Services
Imports MAIS.Business.Model
Imports MAIS.Business.Services
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business
Imports MAIS.Business.Helpers
Imports MAIS.Business.Model.Enums
Imports ODMRDDHelperClassLibrary
Imports ODMRDDHelperClassLibrary.ODMRDDServiceProvider

Public Class maisDODD_Message
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Master.HideLink = True
            Master.HideProgressBar = True
            SessionHelper.Notation_Flg = False
            If Not Request.QueryString("MessageID") Is Nothing Then
                Dim iMessageID As Integer = Request.QueryString("MessageID")
                Me.divDODAdmin.Visible = False
                SetViewMessage(iMessageID)

            Else
                LoadRoles()
                LoadRNs()
                Me.divDODAdmin.Visible = False
                Me.dvListOfMessage.Visible = True
                LoadListMessage()
            End If

        Else

        End If
    End Sub

    Private Sub LoadRoles()
        Dim RoleList As New List(Of Enums.Mais_Roles)
        Me.lstRolesFrom.Items.Clear()


        For Each er In [Enum].GetValues(GetType(Model.Enums.Mais_Roles))
            Dim litem As New System.Web.UI.WebControls.ListItem(EnumHelper.GetEnumDescription(er), er)
            Me.lstRolesFrom.Items.Add(litem)
        Next


        ' Me.lstRolesFrom.DataSource = RoleList.GetEnumerator


    End Sub

    Private Sub LoadListMessage()
        Dim MessageService As IDODDMessagePageService = StructureMap.ObjectFactory.GetInstance(Of IDODDMessagePageService)()
        Me.gvListMessage.DataSource = MessageService.GetCurrentMessage
        Me.gvListMessage.DataBind()

    End Sub


    Protected Sub bntSearch_Click(sender As Object, e As EventArgs) Handles bntSearch.Click
        If String.IsNullOrWhiteSpace(txtSearchStartDate.Text) = False Then
            Dim MessageService As IDODDMessagePageService = StructureMap.ObjectFactory.GetInstance(Of IDODDMessagePageService)()

            Me.gvListMessage.DataSource = MessageService.SearchMessageDataByDates(Me.txtSearchStartDate.Text, IIf(String.IsNullOrWhiteSpace(Me.txtSearchEndDate.Text), Nothing, Me.txtSearchEndDate.Text))
            Me.gvListMessage.DataBind()
        End If


    End Sub

    Private Sub LoadRNs()
        Dim UserRNService As IUserRNDetailService = StructureMap.ObjectFactory.GetInstance(Of IUserRNDetailService)()
        Dim ListofRNs As New List(Of Business.Model.RN_UserDetails)
        Me.lstPersonFrom.Items.Clear()

        ListofRNs = UserRNService.getAllRNDetails

        For Each RN In ListofRNs.OrderBy(Function(Rns) Rns.LastName)
            Dim Litem As New System.Web.UI.WebControls.ListItem(RN.LastFirstname, RN.RN_Sid)
            Me.lstPersonFrom.Items.Add(Litem)
        Next
    End Sub

    Protected Sub bntUploadDoc_Click(sender As Object, e As EventArgs) Handles bntUploadDoc.Click
        Dim bytestForFiletoSaveToDatabase(Me.fulMessageDoc.PostedFile.InputStream.Length - 1) As Byte
        Dim MessageHold As New List(Of Model.DocumentUpload)

        If Session("MessageHold") IsNot Nothing Then
            MessageHold = CType(Session("MessageHold"), List(Of Model.DocumentUpload))
        End If

        fulMessageDoc.PostedFile.InputStream.Read(bytestForFiletoSaveToDatabase, 0, bytestForFiletoSaveToDatabase.Length)

        If bytestForFiletoSaveToDatabase.Length > 0 Then
            Dim MessageHold1 As New Model.DocumentUpload
            With MessageHold1
                .ImageSID = MessageHold.Count + 1
                .DocumentTypeID = 7 '7 = other in the Document_Type table in the database
                .DocumentType = "DoDD Message"
                .DocumentName = fulMessageDoc.FileName
                .FolderName = "DoDD Message"
                .SourcePage = "MaisDODD_Message.aspx"
                .UploadDate = Today
                .DocumentImage = bytestForFiletoSaveToDatabase
            End With

            MessageHold.Add(MessageHold1)
            Session("MessageHold") = MessageHold
            grvMessageDocuments.DataSource = MessageHold
            grvMessageDocuments.DataBind()
        End If
    End Sub

    Private Sub grvMessageDocuments_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grvMessageDocuments.RowDeleting
        Dim MessageHold = CType(Session("MessageHold"), List(Of Model.DocumentUpload))

        MessageHold.Remove(MessageHold(e.RowIndex))

        Session("MessageHold") = MessageHold
        Me.grvMessageDocuments.DataSource = MessageHold
        Me.grvMessageDocuments.DataBind()
    End Sub

    Private Sub grvMessageDocuments_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles grvMessageDocuments.SelectedIndexChanging
        Dim byteArrayForFileToView As Byte()
        Dim MessageHold = CType(Session("MessageHold"), List(Of Model.DocumentUpload))
        byteArrayForFileToView = MessageHold(e.NewSelectedIndex).DocumentImage

        Response.AddHeader("Content-Disposition", "attachment; filename=" & _
            MessageHold(e.NewSelectedIndex).DocumentName)

        Response.AddHeader("Content-Length", byteArrayForFileToView.Length.ToString)

        'Set HTTP MIME type for output stream
        Response.ContentType = "application/octet-stream"

        'Output the data to the client
        If byteArrayForFileToView.Length > 0 Then
            Dim ext = System.IO.Path.GetExtension(MessageHold(e.NewSelectedIndex).DocumentName)
            If ext = ".html" Then
            Else
                Response.BinaryWrite(byteArrayForFileToView)
            End If
            Response.End()
        End If
    End Sub

    Private Sub bntSaveMessage_Click(sender As Object, e As EventArgs) Handles bntSaveMessage.Click
        Dim ImessageService As IDODDMessagePageService = StructureMap.ObjectFactory.GetInstance(Of IDODDMessagePageService)()

        Dim val As Integer
        Dim NewMessage As New Model.DODDMessageInfo
        With NewMessage
            .DODD_Message_SID = hfMessageID.Value
            .Subject = txtSubject.Text
            .Description = txtMessage.Text
            .Priority = ckPriority.Checked
            .Message_Start_Date = txtStartDate.Text
            .Message_End_Date = txtEndDate.Text
            .CreateBy = SessionHelper.MAISUserID
            .Active_Flag = True
            .LastUpdateBy = SessionHelper.MAISUserID
            .RolesList = New List(Of DODDMessageInfoMaisRoles)
            .PersonList = New List(Of DODDMessageInfoMaisRNDDPerson)
            If lstRolesTo.Items.Count > 0 Then
                For Each RItem In lstRolesTo.Items
                    Dim nRole As New DODDMessageInfoMaisRoles
                    nRole.MAISRolesSid = RItem.value
                    nRole.DODD_Message_SID = -1
                    nRole.DODD_Message_MAIS_Role_XRef_SID = -1
                    nRole.Active_Flg = True
                    NewMessage.RolesList.Add(nRole)
                Next
            End If
            If lstPersonTo.Items.Count > 0 Then
                For Each Pitem In lstPersonTo.Items
                    Dim PRole As New DODDMessageInfoMaisRNDDPerson
                    PRole.DODD_Message_RN_DD_Person_Type_Xref_Sid = -1
                    PRole.DODD_Message_Sid = -1
                    PRole.Active_Flg = True
                    PRole.RN_Sid = Pitem.value()
                    NewMessage.PersonList.Add(PRole)

                Next
            End If
        End With

        val = ImessageService.Save_DODDMessage(NewMessage)
        'val = 10
        If val > 0 Then
            SaveToUDS(val)
            SendEmails()

            ShowListOfMessge()
        Else
            Master.SetError("An error has occurred  on saving the message.")

        End If
    End Sub

    Private Sub SendEmails()
        If ckbGroupSendEmail.Checked OrElse ckbPersonSendEmail.Checked Then
            Dim messageService As IDODDMessagePageService = StructureMap.ObjectFactory.GetInstance(Of IDODDMessagePageService)()

            Dim strToEmailAddress As String = Nothing

            If ckbGroupSendEmail.Checked Then
                'get emails of the groups in lstGroup
                strToEmailAddress = GetEmailsByGroup()

            End If
            If ckbPersonSendEmail.Checked Then
                'get emails of the person in the lstPerson
                If Not strToEmailAddress Is Nothing Then
                    strToEmailAddress = strToEmailAddress & ", " & GetPersonEmails()
                Else
                    strToEmailAddress = GetPersonEmails()
                End If
            End If

            Dim emailService As IEmailService = StructureMap.ObjectFactory.GetInstance(Of IEmailService)()
            Dim MessageHold = CType(Session("MessageHold"), List(Of Model.DocumentUpload))

            Dim retObj As ReturnObject(Of Boolean) = emailService.SendEmail1(strToEmailAddress, ConfigHelper.FromEmailAddress, txtSubject.Text, txtMessage.Text, MessageHold, String.Empty)

            If retObj.ReturnValue = True Then
                Master.SetError("")
            Else
                Master.SetError("The email service has an error. The emails were not sent.", False)

            End If

        End If

    End Sub

    Public Function GetEmailsByGroup() As String
        Dim strToEmailAddress As String = Nothing
        Dim UserRNService As IUserRNDetailService = StructureMap.ObjectFactory.GetInstance(Of IUserRNDetailService)()

        If Not String.IsNullOrEmpty(ConfigHelper.ToEmailAddress) Then
            strToEmailAddress = ConfigHelper.ToEmailAddress

        Else
            Dim RoleList As String = Nothing
            For Each RoleItem In lstRolesTo.Items
                If Not RoleList Is Nothing Then
                    RoleList = RoleList & ", " & RoleItem.Value()
                Else
                    RoleList = RoleItem.Value()
                End If
            Next
            Dim RNList = UserRNService.GetRNDetailsWithEmailsByRoleID(RoleList)
            Dim BuildEmailList As New ArrayList()
            Dim TestTypeStr As New String("1,2,3")


            For Each bl In RNList

                For Each blm In bl.EmailList.Where(Function(w) TestTypeStr.Contains(w.ContactType))
                    If Not BuildEmailList.Contains(blm.EmailAddress) Then
                        BuildEmailList.Add(blm.EmailAddress)
                    End If

                Next
            Next

            For Each mlist In BuildEmailList
                If Not strToEmailAddress Is Nothing Then

                    strToEmailAddress = strToEmailAddress & ", " & mlist


                Else
                    strToEmailAddress = mlist
                End If
            Next

        End If
        Return strToEmailAddress

    End Function

    Public Function GetPersonEmails() As String
        Dim strToEmailAddress As String = Nothing
        Dim UserRNService As IUserRNDetailService = StructureMap.ObjectFactory.GetInstance(Of IUserRNDetailService)()



        If Not String.IsNullOrEmpty(ConfigHelper.ToEmailAddress) Then
            strToEmailAddress = ConfigHelper.ToEmailAddress
        Else
            Dim BuildEmailList As New ArrayList()
            For Each em In lstPersonTo.Items
                Dim EmailList = UserRNService.GetRNDetailsWithEmailsByRN_Sid(em.value)

                Dim TestTypeStr As New String("1,2,3")
                For Each PMail In EmailList.EmailList.Where(Function(w) TestTypeStr.Contains(w.ContactType))
                    If Not BuildEmailList.Contains(PMail.EmailAddress) Then
                        BuildEmailList.Add(PMail.EmailAddress)
                    End If
                Next
            Next
            For Each bml In BuildEmailList
                If strToEmailAddress Is Nothing Then
                    strToEmailAddress = bml
                Else
                    strToEmailAddress = strToEmailAddress & ", " & bml
                End If
            Next

        End If
        Return strToEmailAddress

    End Function

    Private Sub SetMessageForEdit(ByVal MessageID As Integer)
        Dim MessageData = GetMessageDataByMessageID(MessageID)
        Me.hfMessageID.Value = MessageID

        Me.ResetAdminFields()

        If MessageData IsNot Nothing Then
            divDODAdmin.Visible = True
            dvListOfMessage.Visible = False
            With MessageData
                Me.txtStartDate.Text = .Message_Start_Date
                Me.txtEndDate.Text = .Message_End_Date
                Me.txtSubject.Text = .Subject
                Me.txtMessage.Text = .Description
                Me.ckViewPriority.Checked = .Priority
                If .RolesList.Count > 0 Then
                    Me.ckGroup.Checked = True
                    For Each rl In .RolesList
                        Dim Litem As New System.Web.UI.WebControls.ListItem(rl.MAISRoleName, rl.MAISRolesSid)
                        lstRolesTo.Items.Add(Litem)
                        lstRolesFrom.Items.Remove(Litem)
                    Next
                End If
                If .PersonList.Count > 0 Then
                    Me.ckPerson.Checked = True
                    For Each Pl In .PersonList
                        Dim Litem As New System.Web.UI.WebControls.ListItem(Pl.RN_Name, Pl.RN_Sid)
                        Me.lstPersonTo.Items.Add(Litem)
                        Me.lstPersonFrom.Items.Remove(Litem)
                    Next
                End If
            End With
            Dim UDSDoc = GetUDS_Documents(MessageData.DODD_Message_SID)
            If UDSDoc.Count > 0 Then
                Me.grAdminDocumentsInUDS.Visible = True
                Me.grAdminDocumentsInUDS.DataSource = UDSDoc
                Me.grAdminDocumentsInUDS.DataBind()
            End If
        End If
    End Sub

    Private Sub ResetAdminFields()
        Me.txtStartDate.Text = String.Empty
        Me.txtEndDate.Text = String.Empty
        Me.txtSubject.Text = String.Empty
        Me.txtMessage.Text = String.Empty
        Session("HoldMessage") = Nothing
        grViewUDSDoc.DataSource = Nothing
        grAdminDocumentsInUDS.DataSource = Nothing
        grAdminDocumentsInUDS.Visible = False
        Me.lstPersonTo.Items.Clear()
        Me.lstRolesTo.Items.Clear()

        Me.LoadRoles()
        Me.LoadRNs()

        Me.ckbGroupSendEmail.Checked = False
        Me.ckbPersonSendEmail.Checked = False
        Me.ckGroup.Checked = False
        Me.ckPerson.Checked = False
        Me.DataBind()

    End Sub
#Region "UDS reagion"

    Private Sub SaveToUDS(ByVal MessageID As Integer)
        Dim MessageHold = CType(Session("MessageHold"), List(Of Model.DocumentUpload))
        Dim strError As String = String.Empty

        If MessageHold IsNot Nothing Then
            Dim maisService As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
            Dim userName As String = UserAndRoleHelper.CurrentUser.LastName + "." + UserAndRoleHelper.CurrentUser.FirstName

            Dim RoleList As String = String.Empty
            Dim PersonList As String = String.Empty

            For Each iR In lstRolesTo.Items
                If RoleList IsNot String.Empty Then
                    RoleList = RoleList + ", " + iR.Text()
                Else
                    RoleList = iR.Text()
                End If
            Next

            For Each iP In lstPersonTo.Items
                If PersonList IsNot String.Empty Then
                    PersonList = PersonList + ", " + iP.Text()
                Else
                    PersonList = iP.Text()
                End If
            Next

            Dim CourseMessageNumber As String = String.Empty
            CourseMessageNumber = "DM" + MessageID.ToString

            Dim wsUDS As New UDSWebService.UDSService() With {.Url = ConfigHelper.UDSServiceEndpoint}

            Dim repositoryName As String = "UDS - MA"

            Dim wsDataSet As New DataSet
            Dim xml As String = "<Table> <Index> <Label> Course Number </Label> <Value> " + CourseMessageNumber + "</Value> </Index>" _
                             + "<Index> <Label> Application Type </Label> <Value> Other </Value> </Index>" _
                             + "<Index> <Label> NAME </Label> <Value> " + userName + " </Value> </Index>" _
                             + "<Index> <Label> RN Lics or 4 SSN </Label> <Value>  </Value> </Index>" _
                             + "<Index> <Label> DOB </Label> <Value> </Value> </Index>" _
                             + "<Index> <Label> Personnel Type </Label>  <Value> Other </Value> </Index>" _
                             + "<Index> <Label> CERT STATUS </Label> <Value>  </Value> </Index>" _
                             + "<Index> <Label>  Application ID </Label> <Value>  </Value> </Index>" _
                             + "<Index> <Label>  DD PERSONNEL CODE </Label> <Value> </Value> </Index>" _
                             + "</Table>"
            Dim Render = New System.IO.StringReader(xml)
            wsDataSet.ReadXml(Render)

            Dim Notes As String = String.Empty

            Select Case True
                Case RoleList.Length > 0 And PersonList.Length > 0
                    Notes = "message for role's " + RoleList + " and Person's " + PersonList + " with dates of " + txtStartDate.Text + " to " + txtEndDate.Text + "."

                Case RoleList.Length > 0 And PersonList.Length = 0
                    Notes = "message for role's " + RoleList + " with dates of " + txtStartDate.Text + " to " + txtEndDate.Text + "."
                Case RoleList.Length = 0 And PersonList.Length > 0
                    Notes = "message forPerson's " + PersonList + " with dates of " + txtStartDate.Text + " to " + txtEndDate.Text + "."
            End Select

            Dim Documnet_Int As Integer = 0
            For Each s In MessageHold
                Documnet_Int += 1
                Dim byteArrayForUDS As Byte() = s.DocumentImage
                Dim DocumentName As String = txtSubject.Text + "_" + MessageID.ToString + "_" + Documnet_Int.ToString + "_" + s.DocumentName


                Dim result As UDSWebService.Result = wsUDS.SaveToUDS(repositoryName, _
                                                                     byteArrayForUDS, _
                                                                     MAIS_Helper.GetUser.UserName, _
                                                                     DocumentName, _
                                                                     "Message", _
                                                                     "DODD Message", _
                                                                     Today, _
                                                                     Notes, _
                                                                     wsDataSet)
                Select Case result
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
#End Region

#Region "Group box reagion"
    Protected Sub bntAdd_Click(sender As Object, e As EventArgs) Handles bntAdd.Click
        Dim icount As Integer = 0
        Dim moveItems As New List(Of System.Web.UI.WebControls.ListItem)
        For icount = 0 To lstRolesFrom.Items.Count - 1
            If lstRolesFrom.Items(icount).Selected = True Then
                moveItems.Add(lstRolesFrom.Items(icount))
            End If
        Next
        If moveItems.Count > 0 Then
            'move the items to the RoleTo
            For Each R In moveItems


                lstRolesTo.Items.Add(R)
                lstRolesFrom.Items.Remove(R)
            Next

        End If
        lstRolesTo.ClearSelection()
        lstRolesFrom.ClearSelection()
    End Sub

    Protected Sub bntRemove_Click(sender As Object, e As EventArgs) Handles bntRemove.Click
        Dim icount As Integer = 0
        Dim MoveBackItem As New List(Of System.Web.UI.WebControls.ListItem)

        For icount = 0 To lstRolesTo.Items.Count - 1
            If lstRolesTo.Items(icount).Selected = True Then
                MoveBackItem.Add(lstRolesTo.Items(icount))
            End If
        Next

        If MoveBackItem.Count > 0 Then
            'move the items to the RoleFrom
            For Each R In MoveBackItem
                lstRolesFrom.Items.Add(R)
                lstRolesTo.Items.Remove(R)
            Next
        End If
        lstRolesFrom.ClearSelection()
        lstRolesTo.ClearSelection()

        Dim slBrowser = New Dictionary(Of String, String)
        For Each PP In lstRolesFrom.Items
            slBrowser.Add(PP.Value, PP.text)
        Next
        Dim sortItems = (From dic In slBrowser Order By dic.Key Ascending Select dic)

        lstRolesFrom.DataSource = sortItems
        lstRolesFrom.DataValueField = "Key"
        lstRolesFrom.DataTextField = "Value"
        lstRolesFrom.DataBind()

    End Sub

    Protected Sub bntAddPerson_Click(sender As Object, e As EventArgs) Handles bntAddPerson.Click
        Dim icount As Integer = 0
        Dim MoveItems As New List(Of System.Web.UI.WebControls.ListItem)
        For icount = 0 To lstPersonFrom.Items.Count - 1
            If lstPersonFrom.Items(icount).Selected = True Then
                MoveItems.Add(lstPersonFrom.Items(icount))
            End If
        Next
        If MoveItems.Count > 0 Then
            'move the itmes to the PersonTo list box
            For Each P In MoveItems
                lstPersonTo.Items.Add(P)
                lstPersonFrom.Items.Remove(P)
            Next

        End If
        lstPersonFrom.ClearSelection()
        lstPersonTo.ClearSelection()

    End Sub

    Protected Sub bntRemovePerson_Click(sender As Object, e As EventArgs) Handles bntRemovePerson.Click
        Dim icount As Integer = 0
        Dim MoveItems As New List(Of System.Web.UI.WebControls.ListItem)

        For icount = 0 To lstPersonTo.Items.Count - 1
            If lstPersonTo.Items(icount).Selected = True Then
                MoveItems.Add(lstPersonTo.Items(icount))
            End If
        Next
        If MoveItems.Count > 0 Then
            For Each P In MoveItems
                lstPersonFrom.Items.Add(P)
                lstPersonTo.Items.Remove(P)
            Next
        End If
        lstPersonFrom.ClearSelection()
        lstPersonTo.ClearSelection()

        Dim slBrowser = New Dictionary(Of String, String)
        For Each PP In lstPersonFrom.Items
            If slBrowser.ContainsKey(PP.Value) = False Then
                slBrowser.Add(PP.Value, PP.text)
            End If

        Next
        Dim sortItems = (From dic In slBrowser Order By dic.Value Ascending Select dic)

        lstPersonFrom.DataSource = sortItems
        lstPersonFrom.DataValueField = "Key"
        lstPersonFrom.DataTextField = "Value"
        lstPersonFrom.DataBind()
    End Sub
#End Region

#Region "Div View Messages"

    Private Sub SetViewMessage(ByVal MessageID As Integer)

        Dim MessageData = GetMessageDataByMessageID(MessageID)


        If MessageData IsNot Nothing Then
            dvViewMessage.Visible = True
            dvListOfMessage.Visible = False

            With MessageData
                Me.txtViewStartDate.Text = .Message_Start_Date
                Me.txtViewStartDate.Enabled = False
                Me.txtViewEndDate.Text = .Message_End_Date
                Me.txtViewEndDate.Enabled = False
                Me.txtViewSubject.Text = .Subject
                Me.txtViewSubject.ReadOnly = True
                Me.txtViewMessage.Text = .Description
                Me.txtViewMessage.ReadOnly = True
                Me.ckViewPriority.Checked = .Priority
                Me.ckViewPriority.Enabled = False
              
            End With


            Me.grViewUDSDoc.DataSource = GetUDS_Documents(MessageID)
            Me.grViewUDSDoc.DataBind()
        End If
    End Sub

    Private Function GetMessageDataByMessageID(ByVal MessageID As Integer) As Model.DODDMessageInfo
        Dim MessageSerive As IDODDMessagePageService = StructureMap.ObjectFactory.GetInstance(Of IDODDMessagePageService)()

        Return MessageSerive.GetMessageDataByMessageID(MessageID)

    End Function

    Private Function GetUDS_Documents(ByVal MessageID As Integer) As List(Of MAIS.Web.UDSWebService.IndexedDocument)
        Dim wsUDS As New UDSWebService.UDSService() With {.Url = ConfigHelper.UDSServiceEndpoint}
        Dim SearchDS As New DataSet

        Dim repositoryName As String = "UDS - MA"

        Dim xml As String = "<Table><Index> <Label> Course Number </Label><Value> DM" + MessageID.ToString + "</Value> </Index> </Table>"

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
#End Region

    Protected Sub ShowListOfMessge()
        Session("MessageHold") = Nothing
        Me.divDODAdmin.Visible = False
        Me.dvListOfMessage.Visible = True
        Me.LoadListMessage()
        Master.SetError("")
    End Sub

    Protected Sub bntNewMessage_Click(sender As Object, e As EventArgs) Handles bntNewMessage.Click
        ResetAdminFields()
        Me.dvListOfMessage.Visible = False
        Me.divDODAdmin.Visible = True
        hfMessageID.Value = -1
    End Sub

    Protected Sub bntCancelMessage_Click(sender As Object, e As EventArgs) Handles bntCancelMessage.Click
        Me.divDODAdmin.Visible = False
        Me.dvListOfMessage.Visible = True
        If Me.ckbSearchMessage.Checked = False Then
            Me.LoadListMessage()
        Else
            Me.bntSearch_Click(sender, e)
        End If

        Master.SetError("")
    End Sub

    

    Protected Sub gvListMessage_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvListMessage.SelectedIndexChanged
        Dim MessageID As Integer = CType(sender, GridView).SelectedDataKey.Value
        SetViewMessage(MessageID)
        bntBackToHomePage.Text = "Back"

    End Sub



    Private Sub bntBackToHomePage_Click(sender As Object, e As EventArgs) Handles bntBackToHomePage.Click
        If bntBackToHomePage.Text = "Back" Then
            Me.dvViewMessage.Visible = False
            Me.dvListOfMessage.Visible = True
        Else
            Response.Redirect(PagesHelper.LandingPage)
        End If
    End Sub

    Protected Sub lnkEditMessage_Click(sender As Object, e As EventArgs)
        Dim MessageID As Integer = CType(sender, LinkButton).CommandArgument
        SetMessageForEdit(MessageID)

    End Sub

    Protected Sub gvListMessage_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvListMessage.RowEditing
        'to nothin Need for the Grid view Row Editing event. 
        e.Cancel = True
    End Sub

    
    Protected Sub ckbSearchMessage_CheckedChanged(sender As Object, e As EventArgs) Handles ckbSearchMessage.CheckedChanged
        If ckbSearchMessage.Checked = False Then
            Me.LoadListMessage()
        End If
    End Sub
End Class