Public Class UploadHelper
    Shared _acceptableExtensions() As String = New String() {".pdf", ".gif", ".jpg", ".jpeg", ".doc", ".docx", ".bmp", ".xls", ".xlsx", ".rtf", ".tif", ".xif", ".txt", ".png"}

    Public Shared ReadOnly Property AcceptableExtensions() As List(Of String)
        Get
            Return _acceptableExtensions.ToList()
        End Get
    End Property

    Public Shared Function IsAcceptableFileType(ByVal filename As String) As Boolean
        Try
            Dim periodIndex As Integer = filename.LastIndexOf(".")

            If periodIndex = -1 Then
                Return False
            Else
                Dim fileExtension As String = filename.Substring(periodIndex)

                Return _acceptableExtensions.Contains(fileExtension.ToLower())
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
