Imports System.IO
Public Class ExcelHelper
    Public Shared Sub CreateExcel(ByVal res As HttpResponse, ByVal gv As GridView, ByVal filename As String)
        res.Clear()
        res.AddHeader("content-disposition", String.Format("attachment;filename={0}_" + Date.Now.ToString("yyyyMMdd_HHmmss") + ".xls", filename))
        res.Charset = ""
        res.ContentType = "application/vnd.xls"

        Dim sw As New StringWriter()
        Dim htw As New HtmlTextWriter(sw)

        gv.AllowPaging = False
        gv.AllowSorting = False
        gv.EnableViewState = False
        gv.DataBind()

        ExcelHelper.DisableLinksInGridviewForExcel(gv)

        gv.RenderControl(htw)

        res.Write(sw.ToString())
        res.End()
    End Sub
   
    Public Shared Sub DisableLinksInGridviewForExcel(ByVal c As WebControl)
        For i As Integer = 0 To c.Controls.Count - 1
            If TypeOf c.Controls(i) Is LinkButton Then
                DirectCast(c.Controls(i), LinkButton).Enabled = False
            End If

            If c.Controls(i).HasControls() Then
                DisableLinksInGridviewForExcel(c.Controls(i))
            End If
        Next
    End Sub
End Class
