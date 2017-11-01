Imports ODMRDDHelperClassLibrary
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data
'Imports MAIS.Data.Objects
Imports MAIS.Business.Services
Imports System.String
Imports System.IO

Public Class Notation
    Inherits System.Web.UI.Page
    Private Shared _nd As NotationDetails
    Private Shared _appID As Integer
    Private Shared NotationSvc As INotationService
    Private Shared PersSvc As IPersonalInformationService
    Private Shared SumSvc As ISummaryService
    Private Shared SaveToPermSvc As IMoveTempToPermService
    Private Shared MSvc As IMAISSerivce
    Private Shared _rnorDD As Boolean
    'Public Shared usr As ODMRDD_NET2.IUser
    Public Shared Property RNorDD As String
        Private Get
            Return _rnorDD
        End Get
        Set(ByVal value As String)
            _rnorDD = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim uploadedFiles As New List(Of Business.Model.DocumentUpload)
        Dim uploadSvc As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
        SaveToPermSvc = StructureMap.ObjectFactory.GetInstance(Of IMoveTempToPermService)()
        MSvc = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        PersSvc = StructureMap.ObjectFactory.GetInstance(Of IPersonalInformationService)()
        Dim queryString As String = Request.QueryString("App")

        If (queryString = "Notation") Then
            Master.HideProgressBar = True
        Else
            Master.HideProgressBar = False
        End If
        'hStatus.Value = ""
        pnote.Style("display") = "none"
        lblDt.Text = Date.Today.ToShortDateString
        'ScriptManager.RegisterStartupScript(Me, Me.GetType, "jqscript", "<script language='javascript' src='Scripts/jquery/jquery.1.6.2.min.js'></script><script language='javascript' src='Scripts/jquery/jquery.calendars.picker.pack.js'></script><script language='javascript' src='Scripts/jquery/jquery.calendars.pack.js'></script>", False)
        'ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType, "dtpickscript", "<script language='javascript' src='Scripts/jquery/jquery.calendars.picker.pack.js'></script>", False)
        If Not IsPostBack Then
            AppID = SessionHelper.ApplicationID
            RNorDD = SessionHelper.RN_Flg
            Session("NotDocs") = Nothing
            'If MSvc.GetExistingFlg(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg).ReturnValue Then
            chbUnflag.Enabled = False

            'usr = MAIS_Helper.GetUser()
            'txtPerson.Value = usr.LastName & " " & usr.FirstName
            'set hidden field for javascript and saving later
            Dim NTypesList As List(Of Business.Model.NType)
            Dim NReasonsList As List(Of Business.Model.NReason)
            'Dim CStatuses As List(Of Objects.CertStatus)
            NotationSvc = StructureMap.ObjectFactory.GetInstance(Of INotationService)()
            If UserAndRoleHelper.IsUserAdmin Then
                txtPersonTitle.Value = "Admin"
                txtPerson.Value = "Admin"
            ElseIf UserAndRoleHelper.IsUserSecretary Then
                txtPersonTitle.Value = "Secretary"
                txtPerson.Value = "Secretary"
            Else
                Dim RDet As Business.Model.MAISRNDDRoleDetails = MSvc.GetRoleUsingUserID(SessionHelper.MAISUserID)
                txtPersonTitle.Value = RDet.RoleName ' query1.GetRole(SessionHelper.MAISLevelUserRole)
                txtPerson.Value = MSvc.GetRNsName(RDet.RNSID)
            End If

            NTypesList = NotationSvc.GetNotationTypes
            NReasonsList = NotationSvc.GetNotationReasons
            'cklReason.DataSource = NReasonsList
            'cklReason.DataTextField = "NReasonDesc"
            'cklReason.DataValueField = "NReasonSid"
            'cklReason.DataBind()
            'CStatuses = query1.GetCertStatuses()
            'ddlStatus.DataSource = CStatuses
            'ddlStatus.DataTextField = "CertStatusDesc"
            'ddlStatus.DataValueField = "CertStatusSid"
            'ddlStatus.DataBind()
            ddNotationType.DataSource = NTypesList
            ddNotationType.DataValueField = "NTypeSid"
            ddNotationType.DataTextField = "NTypeDesc"
            ddNotationType.DataBind()
            lbHReasons.DataSource = NReasonsList
            lbHReasons.DataTextField = "NReasonDesc"
            lbHReasons.DataValueField = "NReasonSid"
            lbHReasons.DataBind()
            PEnterNotation.Style("display") = "none"

            If Not Request.QueryString("appstatus") Is Nothing Then
                hStatus.Value = Server.HtmlDecode(Request.QueryString("appstatus"))

                Dim notStat As String = GetNotTypeByStatusType(hStatus.Value)
                PEnterNotation.Style("display") = "inline"
                btnShowAddNotation.Disabled = True
                For Each li As ListItem In ddNotationType.Items
                    If li.Text = notStat Then
                        li.Selected = True
                        NotTypeChanged(Me.ddNotationType, System.EventArgs.Empty)
                        pError.Visible = True
                        pError.InnerHtml = "You must enter notation for this status: " & notStat
                    End If
                Next
                ddNotationType.Enabled = False
            End If
            For Each l As ListItem In ddNotationType.Items
                If l.Text = "Suspended" Or l.Text = "Intent to Revoke" Or l.Text = "Revoked" Or l.Text = "Certificate Date Change" Then
                    l.Enabled = False
                End If
            Next
            'Else
            '    PEnterNotation.Visible = False
            '    pError.Visible = True
            '    pError.InnerHtml = "Person doesn't exist. Please finish the application for this person first."
            '    dNotButtons.Style("display") = "none"
            '    Exit Sub
            'End If
        Else
            ' hPb.Value = "1"
        End If

        grdNotations.DataSource = NotationSvc.GetNotations(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg).ReturnValue
        grdNotations.DataBind()
    End Sub

    Public Function GetNotTypeByStatusType(ByVal s As String) As String
        Dim nStat As String = ""
        If s = "Removed From Registry" Then
            nStat = "Unregistered"
        Else
            nStat = s
        End If
        Return nStat
    End Function
    Public Sub upPreRender()
    End Sub
    Private Sub SaveToUDS()
        Dim NotDocsHold = CType(Session("NotDocs"), List(Of Business.Model.DocumentUpload))
        Dim strError As String = String.Empty

        If NotDocsHold IsNot Nothing Then
            Dim maisSerivce As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
            Dim rnmod As Business.Model.RNInformationDetails = Nothing
            Dim ddmod As Business.Model.DDPersonnelDetails = Nothing
            If SessionHelper.RN_Flg Then
                'rnmod = PersSvc.GetRNInfoFromPermanent(SessionHelper.SessionUniqueID)
            Else
                ddmod = PersSvc.GetDDPersonnelInformationFromPermanent(SessionHelper.SessionUniqueID)
            End If
            Dim RnNoOrSSN As String = ""
            Dim dob As Date = Date.Today
            If Not IsNothing(ddmod) Then
                RnNoOrSSN = ddmod.DODDLast4SSN
                dob = ddmod.DODDDateOfBirth
            Else
                RnNoOrSSN = SessionHelper.SessionUniqueID
            End If
            'RnNoOrSSN = IIf(IsNothing(ddmod), SessionHelper.SessionUniqueID, ddmod.DODDLast4SSN)
            Dim UserName As String = "N/A"
            Dim UserSid As String
            ' If Me.RequiredFieldValidator1.Enabled = False Then ' the control will not post back to the server if Enable is False. Need to set the RN From the Hiden field
            UserName = maisSerivce.GetApplicantNameByCode(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg) 'getApplicant Name maisSerivce.GetRNsName(hdfRNID.Value)
            userSid = maisSerivce.GetApplicantXrefSidByCode(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg) 'getApplicant Name maisSerivce.GetRNsName(hdfRNID.Value)
            'Else
            '    UserName = "" 'getApplicant Name maisSerivce.GetRNsName(hdfRNID.Value) maisSerivce.GetRNsName(ddlRnNames.SelectedValue)
            '    userSid = "" 'getApplicant Name maisSerivce.GetRNsName(hdfRNID.Value) maisSerivce.GetRNsLicenseNumber(ddlRnNames.SelectedValue).Replace("RN", "")

            'End If
            UserName = UserName.Replace(" ", ".")
            Dim wsUDS As New UDSWebService.UDSService() With {.Url = ConfigHelper.UDSServiceEndpoint}

            Dim repositoryName As String = "UDS - MA"

            Dim wsDataSet As New DataSet 'wsUDS.GetGenericIndexes(repositoryName)

            If Not IsNothing(SessionHelper.ApplicationID) Then
                AppID = SessionHelper.ApplicationID
            Else
                AppID = 0
            End If
            Dim xml As String = "<Table> <Index> <Label> Course Number </Label> <Value> " + CStr(hNotId.Value) + "</Value> </Index>" _
                                + "<Index> <Label> Application ID </Label> <Value> " + CStr(AppID) + " </Value> </Index>" _
                                + "<Index> <Label> Application Type </Label> <Value> Other </Value> </Index>" _
                                + "<Index> <Label> DD Personnel Code </Label> <Value> " + UserSid + " </Value> </Index>" _
                                + "<Index> <Label> NAME </Label> <Value> " + UserName + " </Value> </Index>" _
                                + "<Index> <Label> RN Lics or 4 SSN </Label> <Value> " + RnNoOrSSN + " </Value> </Index>" _
                                + "<Index> <Label> DOB </Label> <Value> " + String.Format("{0}/{1}/{2}", dob.Year.ToString.PadLeft(4, "0"), dob.Month.ToString.PadLeft(2, "0"), dob.Day.ToString.PadLeft(2, "0")) + " </Value> </Index>" _
                                + "<Index> <Label> Personnel Type </Label> <Value> " + IIf(SessionHelper.RN_Flg, "RN", "DDPERS") + " </Value> </Index>" _
                                + "<Index> <Label> CERT STATUS </Label> <Value>  </Value> </Index>" _
                                + "</Table>"

            Dim Render = New System.IO.StringReader(xml)
            wsDataSet.ReadXml(Render)

            ' rows.FirstOrDefault(Function(r) r("label") = "Course Number")("Value") = Me.txtCourseNumberRnNew.Text
            Dim Document_Int As Integer = 0
            For Each s In NotDocsHold
                Document_Int += 1
                Dim byteArrayForUDS As Byte() = s.DocumentImage
                Dim DocumentName As String = hNotId.Value + "_" + Document_Int.ToString + "_" + s.DocumentName

                Dim result As UDSWebService.Result = wsUDS.SaveToUDS(repositoryName, _
                                                                     byteArrayForUDS, _
                                                                     UserName, _
                                                                     DocumentName, _
                                                                     "NOTATION", _
                                                                     "NOTATION DOCUMENT", _
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
    Private Function getNotationDocs(ByVal notid As String) As List(Of MAIS.Web.UDSWebService.IndexedDocument)
        'Dim RetVal As New List(Of MAIS.Web.UDSWebService.IndexedDocument)
        Dim wsUDS As New UDSWebService.UDSService() With {.Url = ConfigHelper.UDSServiceEndpoint}
        Dim SearchDS As New DataSet

        Dim repositoryName As String = "UDS - MA"

        Dim xml As String = "<Table> <Index> <Label> Course Number </Label> <Value> " + notid + "</Value> </Index>" _
                           + "</Table>"
        Dim Render = New System.IO.StringReader(xml)
        SearchDS.ReadXml(Render)

        Dim randomNum = (New Random()).Next(0, 999999999).ToString.PadLeft(10, "0")
        Dim Int As Integer?

        Dim ListDoc = wsUDS.SearchUDS(repositoryName, Int, Int, "", "", "", "", "NOTATION", "", "", "", SearchDS)
        For Each e In ListDoc
            Dim udsUrl As String = e.DownloadURL.Replace("download.aspx", "madownload.aspx")
            e.DownloadURL = udsUrl + "&UID=" & randomNum & MAIS_Helper.GetUserId & randomNum
        Next
        Return ListDoc.ToList

    End Function

    Public Sub Upload_Click() Handles btnUpload.ServerClick
        Try
            Dim strValid As Boolean = True
            Dim uploadedFiles As New List(Of Business.Model.DocumentUpload)
            'Dim rulesUpload As New Rules.RequiredDocumentRules
            'Dim strErrors As List(Of String) = rulesUpload.ValidatePage(cblUploadRequirements.SelectedIndex, uplCWUploadFile.HasFile,
            '                                                            uplCWUploadFile.PostedFile.ContentLength)
            If Not UploadHelper.IsAcceptableFileType(uplNotationFile.FileName) Then
                strValid = False
                'File is of an unacceptable filetype
                'valsum1.lblErrorLabel.Text += "Please select a file that is in one of the following formats: "
                'Display list of acceptable extensions
                'For Each extension As String In UploadHelper.AcceptableExtensions
                '    lblErrorLabel.Text += extension & " "
                'Next
            End If

            If strValid = True Then
                'File is acceptable for upload
                Dim bytesForFileToSaveToDatabase(uplNotationFile.PostedFile.InputStream.Length - 1) As Byte
                Dim NotDocsHold As New List(Of Business.Model.DocumentUpload)
                If Session("NotDocs") IsNot Nothing Then
                    NotDocsHold = CType(Session("NotDocs"), List(Of Business.Model.DocumentUpload))
                End If
                uplNotationFile.PostedFile.InputStream.Read(bytesForFileToSaveToDatabase, 0, bytesForFileToSaveToDatabase.Length)

                If bytesForFileToSaveToDatabase.Length > 0 Then
                    'Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
                    'Dim uploadDocumentNumber As Long = uploadService.InsertUploadedDocumentIntoImageStore(bytesForFileToSaveToDatabase)

                    'If uploadDocumentNumber > 0 Then
                    Dim uploadedDoc As New Business.Model.DocumentUpload With {
                        .ImageSID = NotDocsHold.Count + 1,
                        .DocumentTypeID = 3,
                        .AppNotId = hNotId.Value,
                        .DocumentName = uplNotationFile.FileName,
                        .UploadDate = Date.Today,
                        .DocumentImage = bytesForFileToSaveToDatabase,
                        .DocumentType = "Notation Document",
                        .FolderName = "Notations",
                        .SourcePage = "Notation.aspx"
                    }
                    NotDocsHold.Add(uploadedDoc)
                    Session("NotDocs") = NotDocsHold
                    gvFiles.DataSource = NotDocsHold
                    gvFiles.DataBind()
                    ' Save file info to database
                    'UploadService.InsertUploadedDocument(uploadedDoc, SessionHelper.ApplicationID) 'sessionhelper.applicationId should be used
                    ''End If
                    ''lblErrorLabel.Text = uplCWUploadFile.PostedFile.FileName & " has been uploaded"
                    ''lblErrorLabel.ForeColor = Drawing.Color.Black
                    'If Not hNotId.Value = "0" Then
                    '    uploadedFiles = UploadService.GetDocumentsByNotation(hNotId.Value) 'SessionHelper.ApplicationID
                    '    gvFiles.DataSource = uploadedFiles
                    '    gvFiles.DataBind()
                    'End If
                End If
            End If
        Catch ex As Exception
            'lblErrorLabel.Text = "Error Uploading File"
            'lblErrorLabel.ForeColor = Drawing.Color.Red
        End Try
    End Sub
    Protected Sub gvFiles_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs) Handles gvFiles.RowDeleting
        Dim NotDocsHold = CType(Session("NotDocs"), List(Of Business.Model.DocumentUpload))
        NotDocsHold.Remove(NotDocsHold(e.RowIndex))
        Session("NotDocs") = NotDocsHold
        gvFiles.DataSource = NotDocsHold
        gvFiles.DataBind()

        'Dim idToDel As Integer = e.Keys.Values(0) '  gvFiles.DataKeys(e.Keys.NewSelectedIndex).Values(0)
        ''Dim filenameToDelete As String = gridViewUploadedFiles.Rows(e.RowIndex).Cells.Item(2).Text
    End Sub
    Public Sub NotTypeChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For Each it As ListItem In cklReason.Items
            it.Selected = False
        Next
        cklReason.Items.Clear()
        Dim i = 0
        Dim dnmr As Boolean = IIf(ddNotationType.SelectedItem.Text = "Did Not Meet Requirements", True, False)
        If dnmr Then
            For Each l1 As ListItem In lbHReasons.Items
                If l1.Text = "DODD" Then
                    Exit For
                End If
                i = i + 1
                cklReason.Items.Add(l1)
            Next
        Else
            For Val As Integer = 5 To lbHReasons.Items.Count - 1
                cklReason.Items.Add(lbHReasons.Items(Val))
            Next
        End If
        For Each it As ListItem In cklReason.Items
            it.Selected = False
        Next
    End Sub
    Public Sub btnAddClicked(ByVal sender As Object, ByVal e As System.EventArgs)
        PEnterNotation.Style("display") = "inline"
        hNotId.Value = "0"
        ddNotationType.Enabled = True
        lblDt.Text = Date.Today.ToShortDateString
        Session("NotDocs") = Nothing
        gvFiles.DataBind()
        gvUdsFiles.DataSource = Nothing
        gvUdsFiles.DataBind()
        txtDtOccurance.Disabled = False
        txtDtOccurance.Value = ""
        txtUFDate.Value = ""
        chbUnflag.Checked = False
        chbUnflag.Enabled = False
        ddNotationType.SelectedValue = 0
        For Each li As ListItem In ddNotationType.Items
            If li.Text = "Certificate Date Change" Then
                li.Enabled = False
            End If
        Next
        cklReason.Enabled = True
        For Each it As ListItem In cklReason.Items
            it.Selected = False
        Next
        cklReason.Items.Clear()
    End Sub
    Protected Sub ServerValidate(source As Object, args As ServerValidateEventArgs)
        args.IsValid = False
        For Each li As ListItem In cklReason.Items
            If li.Selected = True Then
                args.IsValid = True
            End If
        Next
    End Sub
    Public Sub SaveNotation() Handles btnSaveNotation.ServerClick
        Dim res As Long
        Dim nd As New NotationDetails
        Dim result As New ReturnObject(Of Long)(-1L)
        If hNotId.Value = "0" Then
            Dim NotDocsHold2 = CType(Session("NotDocs"), List(Of Business.Model.DocumentUpload))
            If IsNothing(NotDocsHold2) Then
                pError.Visible = True
                pError.InnerHtml = "You must upload a document with this notation."
                divSpinner.Style("display") = "none"
                Exit Sub

            End If
            If MSvc.GetExistingFlg(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg).ReturnValue = False Then
                result = SaveToPermSvc.InsertPersonIfNotExists(AppID, RNorDD)
                If result.ReturnValue = 0 Then
                    For Each mstring As ReturnMessage In result.GeneralMessages
                        If mstring.Message Like "*niqueCode*" Then
                            SessionHelper.SessionUniqueID = mstring.Message.Substring(11)
                            Master.RNLicenseOrSSN = SessionHelper.SessionUniqueID
                        End If
                    Next
                Else
                    pError.Visible = True
                    For Each mstring As ReturnMessage In result.GeneralMessages
                        If mstring.Message Like "*niqueCode*" Then
                            SessionHelper.SessionUniqueID = mstring.Message.Substring(11)
                        End If
                    Next
                    pError.InnerHtml = "An Error occured while saving notation information."
                    Exit Sub
                End If
            End If
            nd.NotationDate = Date.Today
            nd.OccurenceDate = Convert.ToDateTime(txtDtOccurance.Value)
            nd.PersonEnteringNotation = txtPerson.Value
            nd.PersonTitle = txtPersonTitle.Value
            nd.NotationType = New NType With {.NTypeSid = ddNotationType.SelectedValue}
            If Not IsNothing(SessionHelper.ApplicationID) And SessionHelper.ApplicationID > 0 Then
                nd.AppId = SessionHelper.ApplicationID
            End If
            Dim nrlist As New List(Of NReason)
            For Each it As ListItem In cklReason.Items
                If it.Selected Then
                    nrlist.Add(New NReason With {.NReasonSid = CInt(it.Value)})
                End If
            Next
            nd.NotationReasons = nrlist
            '_nd = New NotationDetails
            '_nd.NotationDate = DateTime.Now
            '_nd.OccurenceDate = Convert.ToDateTime(txtDtOccurance.Value)
            '_nd.PersonEnteringNotation = txtPerson.Value
            '_nd.PersonTitle = txtPersonTitle.Value
            ''Add reasons later
            ''_nd.NotationType = ddReasonNotation.SelectedItem.Text
            res = NotationSvc.SaveNotation(nd, SessionHelper.SessionUniqueID, SessionHelper.RN_Flg).ReturnValue
            hNotId.Value = res
            SaveToUDS()
            divSpinner.Style("display") = "none"
            Session("NotDocs") = Nothing
            hNotId.Value = "0"
            If hStatus.Value <> "" Then
                'NotationSvc.SetAppStatus(SessionHelper.ApplicationID, hStatus.Value)
                'SessionHelper.ApplicationStatus = hStatus.Value
                'set app status to the right status, and sessionhelper appstatus too
                Response.Redirect("Summary.aspx?selstat=" & hStatus.Value.Replace(" ", "-"))
            End If
            gvFiles.DataBind()

        Else
            nd = NotationSvc.GetNotationByNotID(CInt(hNotId.Value)).ReturnValue
            If chbUnflag.Checked = True Then 'IsDate(txtUFDate.Value.ToString) Then
                nd.UnflaggedDate = Date.Today
            End If
            nd.OccurenceDate = Convert.ToDateTime(txtDtOccurance.Value)
            nd.AllReasons = ""
            Dim nrlist As New List(Of NReason)
            For Each it As ListItem In cklReason.Items
                If it.Selected Then
                    nrlist.Add(New NReason With {.NReasonSid = CInt(it.Value)})
                End If
            Next
            nd.NotationReasons = nrlist
            res = NotationSvc.UpdateNotation(nd, CInt(hNotId.Value)).ReturnValue

            SaveToUDS()
            Session("NotDocs") = Nothing
            gvFiles.DataBind()
        End If
        divSpinner.Style("display") = "none"
        pError.Visible = False
        pnote.Style("display") = "inline"
        pnote.InnerHtml = "Saved Successfuly."
        'ValidationSummary1.
        'errors PEnterNotation.Style("display") = "inline"
        grdNotations.DataSource = NotationSvc.GetNotations(SessionHelper.SessionUniqueID, SessionHelper.RN_Flg).ReturnValue
        grdNotations.DataBind()
        PEnterNotation.Style("display") = "none"
        'res = query1.SaveNotation(no, 11, 2).ReturnValue
    End Sub

    Protected Sub EditCmd(sender As Object, e As GridViewCommandEventArgs)
        Dim aid As Integer = CInt(grdNotations.DataKeys(Convert.ToInt32(e.CommandArgument)).Value.ToString())
        'Dim query2 As Data.Queries.NotationQueries = Nothing
        Dim uploadedFiles As New List(Of Business.Model.DocumentUpload)
        Dim uploadSvc As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
        Dim n As Business.Model.NotationDetails = Nothing ' Business.Model.NotationDetails = Nothing
        'query2 = StructureMap.ObjectFactory.GetInstance(Of Data.Queries.NotationQueries)()
        Session("NotDocs") = Nothing
        gvFiles.DataBind()
        n = NotationSvc.GetNotationByNotID(aid).ReturnValue
        ddNotationType.SelectedValue = n.NotationType.NTypeSid
        lblDt.Text = n.NotationDate.ToShortDateString()
        txtDtOccurance.Value = n.OccurenceDate.ToShortDateString()
        txtPerson.Value = n.PersonEnteringNotation.ToString()
        If IsDate(n.UnflaggedDate) Then
            txtUFDate.Value = n.UnflaggedDate
            chbUnflag.Checked = True
            chbUnflag.Enabled = False
            'chbUnflag.Attributes.Clear()
            'chbUnflag.Attributes.Add("disabled", "true")
        Else
            txtUFDate.Value = ""
            chbUnflag.Checked = False
            If (SessionHelper.MAISLevelUserRole = Enums.RoleLevelCategory.RNMaster_RLC Or UserAndRoleHelper.IsUserAdmin = True) Then
                chbUnflag.Enabled = True
            End If
            'chbUnflag.Attributes.Clear()
        End If
        PEnterNotation.Style("display") = "inline"
        'txtPersonTitle.Value = IIf(IsNullOrEmpty(n.PersonTitle), "", n.PersonTitle.ToString)
        NotTypeChanged(ddNotationType, System.EventArgs.Empty)
        For Each it As ListItem In cklReason.Items
            it.Selected = False
        Next
        For Each r As Business.Model.NReason In n.NotationReasons
            cklReason.Items.FindByValue(r.NReasonSid).Selected = True
        Next
        If (SessionHelper.MAISLevelUserRole = Enums.RoleLevelCategory.RNMaster_RLC Or UserAndRoleHelper.IsUserAdmin = True) Then
            cklReason.Enabled = True
        Else
            cklReason.Enabled = False
        End If
        hNotId.Value = aid
        ddNotationType.Enabled = False
        'txtDtOccurance.Disabled = True

        If hNotId.Value = "0" Then
        Else
            If (SessionHelper.MAISLevelUserRole = Enums.RoleLevelCategory.RNMaster_RLC Or UserAndRoleHelper.IsUserAdmin = True) Then
                Dim udslist As New List(Of MAIS.Web.UDSWebService.IndexedDocument)
                udslist = getNotationDocs(hNotId.Value) ' As List(Of MAIS.Web.UDSWebService.IndexedDocument)
                gvUdsFiles.DataSource = udslist
                gvUdsFiles.DataBind()
                'Debug.Print(udslist(0).DownloadURL)
                'uploadedFiles = uploadSvc.GetDocumentsByNotation(hNotId.Value) 'SessionHelper.ApplicationID
                'gvFiles.DataSource = uploadedFiles
                'gvFiles.DataBind()
            End If
        End If

    End Sub
   
    Public Function GetColumnIndexByHeaderText(g As GridView, ct As String)
        Dim cell As TableCell
        For i As Integer = 0 To g.HeaderRow.Cells.Count
            cell = g.HeaderRow.Cells(i)
            If cell.Text.ToString() = ct Then
                Return i
            End If
        Next
        Return -1
    End Function
    Protected Sub gvFiles_SelectedIndexChanging(ByVal sender As Object, ByVal e As GridViewSelectEventArgs) Handles gvFiles.SelectedIndexChanging
        'Transmit copy of selected file to user for preview
        'Dim uploadService As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()
        Dim byteArrayForFileToView As Byte() '= uploadService.GetUploadedDocumentByImageSID(gvFiles.DataKeys(e.NewSelectedIndex).Values(0))
        Dim NotDocsHold = CType(Session("NotDocs"), List(Of Business.Model.DocumentUpload))
        byteArrayForFileToView = NotDocsHold(e.NewSelectedIndex).DocumentImage
        'Clear all content output from buffer stream
        Response.Clear()
        Dim filenameCell As Integer = 2

        If gvFiles.AutoGenerateDeleteButton = False Then
            filenameCell = 1
        End If

        'Add HTTP header to output stream to specify a default filename, and file length
        Response.AddHeader("Content-Disposition", "attachment; filename=" & _
            NotDocsHold(e.NewSelectedIndex).DocumentName)
        Response.AddHeader("Content-Length", byteArrayForFileToView.Length.ToString())
        'Set the HTTP MIME type for output stream
        Response.ContentType = "application/octet-stream"
        'Output the data to the client
        If byteArrayForFileToView.Length > 0 Then
            'Dim ext = System.IO.Path.GetExtension(gvFiles.Rows(e.NewSelectedIndex).Cells(filenameCell).Text.ToString)
            'If ext = ".html" Then
            '    Response.Redirect(PagesHelper.AttestationPrintPage)
            'Else
            Response.BinaryWrite(byteArrayForFileToView)
            'End If
            Response.End()
        End If
    End Sub
    Public Shared Property AppID As Integer
        Private Get
            Return _appID
        End Get
        Set(ByVal value As Integer)
            _appID = value
        End Set
    End Property

End Class