Imports System.Drawing
Imports EvoPdf.HtmlToPdf

Namespace Services
    Public Interface IDocumentService
        Inherits IBusinessBase

        Function CreateApplicationPDFDocument(ByVal htmlStr As String, ByVal baseUrl As String, ByVal footerText As String) As Byte()
        Function SavePdfFromHtmlStreamToFile(ByVal htmlStrm As System.IO.Stream, ByVal baseUrl As String, ByVal outfile As String) As Boolean
    End Interface

    Public Class DocumentService
        Inherits BusinessBase
        Implements IDocumentService

        Private Property LicenseKey As String

        <Obsolete("Use StructureMap.ObjectFactory.GetInstance(Of IDocumentService)() instead!", True)>
        Public Sub New()
            Throw New NotImplementedException("Method not usable. Use StructureMap.ObjectFactory.GetInstance(Of IDocumentService)() instead!")
        End Sub
       
        Public Sub New(ByVal user As Infrastructure.IUserIdentity)
            Me.LicenseKey = System.Configuration.ConfigurationManager.AppSettings("PdfConverterLicenseKey")
        End Sub

        Public Function CreateApplicationPDFDocument(ByVal htmlStr As String, ByVal baseUrl As String, ByVal footerText As String) As Byte() Implements IDocumentService.CreateApplicationPDFDocument
            Dim pdfBytes As Byte() = Nothing

            Try
                Dim pdfConverter As New PdfConverter()

                With pdfConverter
                    .LicenseKey = Me.LicenseKey
                    .PdfDocumentOptions.ShowHeader = True
                    .PdfDocumentOptions.ShowFooter = True

                    .PdfHeaderOptions.HeaderHeight = 20
                    .PdfHeaderOptions.TextArea = New TextArea(0, 5, "Page &p; of &P;   ", New Font(New FontFamily("Arial"), 8, GraphicsUnit.Point))
                    .PdfHeaderOptions.TextArea.TextAlign = HorizontalTextAlign.Right
                    .PdfHeaderOptions.TextArea.VerticalTextAlign = VerticalTextAlign.Middle
                    .PdfHeaderOptions.TextArea.EmbedTextFont = True

                    .PdfFooterOptions.FooterHeight = 20
                    .PdfFooterOptions.TextArea = New TextArea(0, 5, footerText, New Font(New FontFamily("Arial"), 8, GraphicsUnit.Point))
                    .PdfFooterOptions.TextArea.TextAlign = HorizontalTextAlign.Center
                    .PdfFooterOptions.TextArea.VerticalTextAlign = VerticalTextAlign.Middle
                    .PdfFooterOptions.TextArea.EmbedTextFont = True

                    pdfBytes = .GetPdfBytesFromHtmlString(htmlStr, baseUrl)
                End With
            Catch ex As Exception
                Me.LogError("Error Converting PDF", CInt(Me.UserID), ex)
            End Try

            Return pdfBytes
        End Function

        Public Function SavePdfFromHtmlStreamToFile(ByVal htmlStrm As System.IO.Stream, ByVal baseUrl As String, ByVal outfile As String) As Boolean Implements IDocumentService.SavePdfFromHtmlStreamToFile
            Dim blnSuccess As Boolean = False

            Try
                Dim pdfConverter As New PdfConverter()

                With pdfConverter
                    .LicenseKey = Me.LicenseKey
                    .PdfDocumentOptions.JpegCompressionEnabled = False
                    .PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.NoCompression
                    .PdfDocumentOptions.ShowHeader = False
                    .PdfDocumentOptions.ShowFooter = False
                    .PdfHeaderOptions.HeaderHeight = 0
                    .PdfFooterOptions.FooterHeight = 0
                    .PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter
                    .PdfDocumentOptions.TopMargin = 36
                    .PdfDocumentOptions.LeftMargin = 54
                    .PdfDocumentOptions.RightMargin = 54
                    .PdfDocumentOptions.BottomMargin = 36
                    .SavePdfFromHtmlStreamToFile(htmlStrm, System.Text.Encoding.UTF8, outfile, baseUrl)
                End With

                If System.IO.File.Exists(outfile) Then
                    blnSuccess = True
                End If
            Catch ex As Exception
                Me.LogError("Error Converting PDF", CInt(Me.UserID), ex)
            End Try

            Return blnSuccess
        End Function
    End Class
End Namespace
