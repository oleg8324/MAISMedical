Imports System

Public Class PortalFunctions
    Private _page As System.Web.UI.Page

    Public Function CheckForPortal() As Boolean
        Dim showHeaders As Nullable(Of Boolean) = ShowHeadersQueryStringValue()

        If (showHeaders.HasValue) Then
            If (showHeaders.Value) Then
                GoToFullAppMode()
                DeleteCookie()
                Return False
            Else
                GoToPortalMode()
                CreateCookie()
                Return True
            End If
        Else
            showHeaders = ShowHeadersCookieValue()

            If (showHeaders.HasValue) Then
                If (showHeaders.Value) Then
                    GoToFullAppMode()
                    Return False
                Else
                    GoToPortalMode()
                    Return True
                End If
            Else
                GoToFullAppMode()
                Return False
            End If
        End If
    End Function

    Private Sub New()

    End Sub

    Public Sub New(ByVal Page As System.Web.UI.Page)
        _page = Page
    End Sub

    Private Function ShowHeadersCookieValue() As Nullable(Of Boolean)
        If _page.Request.Cookies("ShowHeaders") IsNot Nothing AndAlso _page.Request.Cookies("ShowHeaders").Value = "false" Then Return False
        If _page.Request.Cookies("ShowHeaders") IsNot Nothing AndAlso _page.Request.Cookies("ShowHeaders").Value = "true" Then Return True
        Return Nothing
    End Function

    Private Function ShowHeadersQueryStringValue() As Nullable(Of Boolean)
        If _page.Request.QueryString("ShowHeaders") IsNot Nothing AndAlso _page.Request.QueryString("ShowHeaders").ToString = "false" Then Return False
        If _page.Request.QueryString("ShowHeaders") IsNot Nothing AndAlso _page.Request.QueryString("ShowHeaders").ToString = "true" Then Return True
        Return Nothing
    End Function

    Private Sub GoToPortalMode()
        Dim cssSheet As New System.Web.UI.WebControls.Literal()
        cssSheet.ID = "DODDPortalOverride"

        Dim cssUrl As String = _page.ResolveUrl("~/App_Themes/DODDPortalOverride/DODDPortalOverride.css")

        cssSheet.Text = "<link rel='stylesheet' type='text/css' href='" & cssUrl & "' />"

        Dim topMaster = FindTopLevelMasterPage()
        Dim headContent = topMaster.FindControl("PortalOverrideContent")
        If headContent IsNot Nothing Then headContent.Controls.Add(cssSheet)

    End Sub

    Private Sub GoToFullAppMode()
        Dim topMaster = FindTopLevelMasterPage()
        Dim headContent = topMaster.FindControl("PortalOverrideContent")

        If headContent IsNot Nothing AndAlso headContent.FindControl("DODDPortalOverride") IsNot Nothing Then
            headContent.Controls.Remove(headContent.FindControl("DODDPortalOverride"))
        End If
    End Sub

    Private Function FindTopLevelMasterPage() As System.Web.UI.MasterPage
        Dim currentMaster As System.Web.UI.MasterPage = _page.Master
        Do While True
            If (currentMaster.Master Is Nothing) Then Exit Do

            currentMaster = currentMaster.Master
        Loop

        Return currentMaster
    End Function

    Private Sub CreateCookie()
        Dim ShowHeaders As New System.Web.HttpCookie("ShowHeaders")
        ShowHeaders.Value = "false"
        ShowHeaders.Expires = DateTime.Now.AddDays(9999)
        _page.Response.Cookies.Add(ShowHeaders)
    End Sub


    Private Sub DeleteCookie()
        Dim ShowHeaders As New System.Web.HttpCookie("ShowHeaders")
        ShowHeaders.Value = "deleted"
        ShowHeaders.Expires = DateTime.Now.AddDays(-1D)
        _page.Response.Cookies.Add(ShowHeaders)
    End Sub

End Class
