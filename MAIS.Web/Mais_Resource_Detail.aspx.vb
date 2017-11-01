Imports System.Web.Script.Services
Imports MAIS.Business.Model
Imports MAIS.Business.Services
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business
Imports MAIS.Business.Helpers
Imports MAIS.Business.Model.Enums
Imports ODMRDDHelperClassLibrary

Public Class Mais_Resource_Detail
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
                Me.divDODAdmin.Visible = False
                Me.dvListOfMessage.Visible = True
                LoadListMessage()
            End If

        Else

        End If
    End Sub
    Private Sub LoadListMessage()
        Dim rsvc As IResourcePageService = StructureMap.ObjectFactory.GetInstance(Of IResourcePageService)()
        Me.gvListMessage.DataSource = rsvc.GetCurrentResource
        Me.gvListMessage.DataBind()

    End Sub

    Private Sub SetMessageForEdit(ByVal MessageID As Integer)
        Dim MessageData = GetMessageDataByMessageID(MessageID)
        Me.hfMessageID.Value = MessageID

        Me.ResetAdminFields()

        If MessageData IsNot Nothing Then
            divDODAdmin.Visible = True
            dvListOfMessage.Visible = False
            With MessageData
                Me.txtSubject.Text = .Subject
                Me.txtMessage.Text = .Description
                Me.ckViewPriority.Checked = .Priority
            End With
            Dim UDSDoc = GetUDS_Documents(MessageData.Resource_SID)
            If UDSDoc.Count > 0 Then
                Me.grAdminDocumentsInUDS.Visible = True
                Me.grAdminDocumentsInUDS.DataSource = UDSDoc
                Me.grAdminDocumentsInUDS.DataBind()
            End If
        End If
    End Sub
   
    Private Function GetMessageDataByMessageID(ByVal MessageID As Integer) As Model.Resource
        Dim ResSvc As IResourcePageService = StructureMap.ObjectFactory.GetInstance(Of IResourcePageService)()

        Return ResSvc.GetResourceDataByResourceID(MessageID)

    End Function

    Private Sub ResetAdminFields()
        Me.txtStartDate.Text = String.Empty
        Me.txtEndDate.Text = String.Empty
        Me.txtSubject.Text = String.Empty
        Me.txtMessage.Text = String.Empty
        Session("HoldMessage") = Nothing
        grViewUDSDoc.DataSource = Nothing
        grAdminDocumentsInUDS.DataSource = Nothing
        grAdminDocumentsInUDS.Visible = False
        'Me.lstPersonTo.Items.Clear()
        'Me.lstRolesTo.Items.Clear()

        'Me.LoadRoles()
        'Me.LoadRNs()

        'Me.ckbGroupSendEmail.Checked = False
        'Me.ckbPersonSendEmail.Checked = False
        'Me.ckGroup.Checked = False
        'Me.ckPerson.Checked = False
        Me.DataBind()

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
                .DocumentType = "Resource"
                .DocumentName = fulMessageDoc.FileName
                .FolderName = "DoDD Message"
                .SourcePage = "Mais_Resource_Detail.aspx"
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
        Dim ResSvc As IResourcePageService = StructureMap.ObjectFactory.GetInstance(Of IResourcePageService)()

        Dim val As Integer
        Dim NewMessage As New Model.Resource
        With NewMessage
            .Subject = txtSubject.Text
            .Resource_SID = CInt(hfMessageID.Value)
            .Description = txtMessage.Text
            .Priority = ckPriority.Checked
            .CreateBy = SessionHelper.MAISUserID
            .Active_Flag = True
            .LastUpdateBy = SessionHelper.MAISUserID
            .CreateByName = SessionHelper.MAISUserID
            .lastUpdatedByName = SessionHelper.MAISUserID
        End With

        val = ResSvc.Save_Resource(NewMessage)
        'val = 10
        If val >= 0 Then
            SaveToUDS(val)

            ShowListOfMessge()
        Else
            Master.SetError("An error has occurred  on saving the resource.")

        End If
    End Sub
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
        Me.LoadListMessage()

        Master.SetError("")
    End Sub



    Protected Sub gvListMessage_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvListMessage.SelectedIndexChanged
        Dim MessageID As Integer = CType(sender, GridView).SelectedDataKey.Value
        SetViewMessage(MessageID)
        bntBackToHomePage.Text = "Back"

    End Sub
    Private Sub SetViewMessage(ByVal MessageID As Integer)

        Dim MessageData = GetMessageDataByMessageID(MessageID)

        If MessageData IsNot Nothing Then
            dvViewMessage.Visible = True
            dvListOfMessage.Visible = False

            With MessageData
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

#Region "UDS reagion"
    Private Function GetUDS_Documents(ByVal MessageID As Integer) As List(Of MAIS.Web.UDSWebService.IndexedDocument)
        Dim wsUDS As New UDSWebService.UDSService() With {.Url = ConfigHelper.UDSServiceEndpoint}
        Dim SearchDS As New DataSet

        Dim repositoryName As String = "UDS - MA"

        Dim xml As String = "<Table><Index> <Label> Course Number </Label><Value> RS" + MessageID.ToString + "</Value> </Index> </Table>"

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

    Private Sub SaveToUDS(ByVal MessageID As Integer)
        Dim MessageHold = CType(Session("MessageHold"), List(Of Model.DocumentUpload))
        Dim strError As String = String.Empty

        If MessageHold IsNot Nothing Then
            Dim maisService As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
            Dim userName As String = UserAndRoleHelper.CurrentUser.LastName + "." + UserAndRoleHelper.CurrentUser.FirstName

            Dim CourseMessageNumber As String = String.Empty
            CourseMessageNumber = "RS" + MessageID.ToString

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
End Class