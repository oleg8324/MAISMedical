Imports System.Web.Script.Services
Imports MAIS.Business
Imports MAIS.Business.Helpers
Imports MAIS.Business.Services
Imports MAIS.Business.Model
Imports MAIS.Business.Model.Enums
Imports MAIS.Data
Imports ODMRDDHelperClassLibrary.Utility

Public Class Mais_Resources
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.HideLink = True
        Master.HideProgressBar = True
        Master.HideNotationLink = True
        SessionHelper.Notation_Flg = False

        If Not IsPostBack Then
            Dim ResSvc As IResourcePageService = StructureMap.ObjectFactory.GetInstance(Of IResourcePageService)()
            Dim messageList As New List(Of Model.Resource)

            messageList = ResSvc.GetCurrentResource()

            Me.rptResources.DataSource = messageList
            Me.rptResources.DataBind()

        End If
    End Sub
    Protected Sub lkbSubjectMessage_Click(sender As Object, e As EventArgs)
        Dim MessageID As Integer = CType(sender, LinkButton).CommandArgument

        Response.Redirect("mais_resource_detail.aspx?MessageID=" & MessageID)
    End Sub

End Class