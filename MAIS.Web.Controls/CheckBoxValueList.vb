Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO


<DefaultProperty("Text"), ToolboxData("<{0}:CheckBoxValueList runat=server></{0}:CheckBoxValueList>")> _
Public Class CheckBoxValueList
    Inherits CheckBoxList

    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        Dim sb As New StringBuilder()
        Dim tw As New StringWriter(sb)
        Dim originalStream As New HtmlTextWriter(tw)

        Me.RepeatColumns = 2        ' Hard-coded because this control only supports this number of columns.
        MyBase.Render(originalStream)

        Dim renderedText As String = sb.ToString()
        Dim start As Integer = 0
        Dim labelStart As Integer = 0

        'Dim endText As Integer = renderedText.Length
        Dim leftColumnCurrItem As Integer = 0
        Dim rightColumnCurrItem As Integer = Me.Items.Count \ 2

        If Me.Items.Count Mod 2 <> 0 Then
            rightColumnCurrItem += 1
        End If


        For i As Integer = 0 To Me.Items.Count - 1 Step 1
            Dim currItem As Integer = 0

            If Me.RepeatDirection = WebControls.RepeatDirection.Vertical Then
                If i Mod 2 = 0 Then
                    currItem = leftColumnCurrItem
                    leftColumnCurrItem += 1
                Else
                    currItem = rightColumnCurrItem
                    rightColumnCurrItem += 1
                End If
            Else
                currItem = i
            End If

            Dim itemAttributeBuilder As New StringBuilder()

            labelStart = renderedText.IndexOf("<label", start, renderedText.Length - start)
            start = renderedText.IndexOf("<input", start, renderedText.Length - start)

            Me.Items(currItem).Attributes.Render(New HtmlTextWriter(New StringWriter(itemAttributeBuilder)))

            renderedText = renderedText.Insert(labelStart + 7, itemAttributeBuilder.ToString() + " ")
            renderedText = renderedText.Insert(start + 7, String.Format("{0} value=""{1}"" ", itemAttributeBuilder.ToString(), Me.Items(currItem).Value))
            start = renderedText.IndexOf("/>", start, renderedText.Length - start)
        Next

        writer.Write(renderedText)
    End Sub
End Class
