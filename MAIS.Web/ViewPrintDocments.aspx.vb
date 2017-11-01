Imports System.Web.Script.Services
Imports MAIS.Business.Model
Imports MAIS.Business.Services
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business
Imports MAIS.Business.Model.Enums
Imports ODMRDDHelperClassLibrary
Imports ODMRDDHelperClassLibrary.ODMRDDServiceProvider

Public Class ViewPrintDocments
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.HideProgressBar = True
        Master.HideLink = True
        If IsPostBack = False Then
            Dim returnPageString As String = Request.QueryString("ReturnPage")
            If Not (returnPageString Is Nothing) Then
                Me.hlbBack.Visible = True
                Me.hlbBack.PostBackUrl = returnPageString
                SetRNData()
            End If
        End If

    End Sub

    Protected Sub bntSearchRN_Click(sender As Object, e As EventArgs) Handles bntSearchRN.Click
        PullUDSData(True)
    End Sub

    Protected Sub BntSearchDD_Click(sender As Object, e As EventArgs) Handles BntSearchDD.Click
        PullUDSData(False)
    End Sub

    Protected Sub SetRNData()
        If SessionHelper.SessionUniqueID.Contains("RN") Then
            Me.rblRnTypeSelect.SelectedValue = 0
            Me.txtRNNO.Text = SessionHelper.SessionUniqueID
            ' Me.gvUDSData.Visible = True
            PullUDSData(True)
        Else
            Me.rblRnTypeSelect.SelectedValue = 1
            Me.txtDODDID.Text = SessionHelper.SessionUniqueID
            ' Me.gvUDSData.Visible = True
            PullUDSData(False)
        End If

    End Sub

    Protected Sub PullUDSData(ByVal RN As Boolean)
        Me.gvUDSData.Visible = True


        Dim wsUDS As New UDSWebService.UDSService() With {.Url = ConfigHelper.UDSServiceEndpoint}
        Dim SearchDS As New DataSet

        Dim repositoryName As String = "UDS - MA"
        Dim xml As String

        If RN = True Then
            xml = "<Table> <Index> <Label> RN Lics or 4 SSN </Label> <Value> " + Me.txtRNNO.Text + "</Value> </Index>" _
                    + "<Index> <Label> Name </Label> <Value> " + IIf(Me.txtLName.Text.Length = 0, "", "%" + Me.txtLName.Text) + "</Value> </Index>" _
                     + "<Index> <Label> Personnel Type </Label>  <Value> RN  </Value> </Index>" _
                    + "</Table>"
        Else
            xml = "<Table> <Index> <Label> RN Lics or 4 SSN </Label> <Value> " + Me.txtDODDSSN.Text + "</Value> </Index>" _
                  + "<Index> <Label> DD Personnel Code </Label> <Value> " + Me.txtDODDID.Text + "</Value> </Index>" _
                  + "<Index> <Label> Personnel Type </Label>  <Value> DDPERS </Value> </Index>" _
                  + "</Table>"
        End If

        Dim Render = New System.IO.StringReader(xml)

        SearchDS.ReadXml(Render)

        Dim RandomNum = (New Random()).Next(0, 999999999).ToString.PadLeft(10, "0")

        Dim Int As Integer?

        Dim ListDoc = wsUDS.SearchUDS(repositoryName, Int, Int, "", "", "", "", "", "", "", "", SearchDS)
        For Each e In ListDoc
            Dim udsUrl As String = e.DownloadURL.Replace("download.aspx", "madownload.aspx")
            e.DownloadURL = udsUrl + "&UID=" & RandomNum & MAIS_Helper.GetUserId & RandomNum
        Next

        Dim AllowNotfication As Boolean = False
        Select Case True
            Case UserAndRoleHelper.IsUserAdmin
                AllowNotfication = True

            Case UserAndRoleHelper.IsUserRN
                Select Case SessionHelper.MAISLevelUserRole
                    Case Enums.RoleLevelCategory.RNMaster_RLC
                        AllowNotfication = True
                    Case Else
                        AllowNotfication = False
                End Select
        End Select

        If AllowNotfication = False Then
            Dim ListDoc2 = (From c In ListDoc
                       Where c.DocumentType <> "NOTATION"
                       Select c).ToList
            Me.gvUDSData.DataSource = ListDoc2

        Else
            Me.gvUDSData.DataSource = ListDoc
        End If


        Me.gvUDSData.DataBind()



        ' Return ListDoc.ToList


    End Sub

  
    Private Sub rblRnTypeSelect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblRnTypeSelect.SelectedIndexChanged
        Me.gvUDSData.DataSource = Nothing
        Me.gvUDSData.DataBind()
        Me.gvUDSData.Visible = False
        resetFields()

    End Sub

    Private Sub resetFields()
        Me.txtRNNO.Text = String.Empty
        Me.txtLName.Text = String.Empty
        Me.txtDODDID.Text = String.Empty
        Me.txtDODDSSN.Text = String.Empty
    End Sub
End Class