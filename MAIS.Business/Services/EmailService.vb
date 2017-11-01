Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data
Imports MAIS.Data.Objects
Imports System.Data.Linq
Imports System.Configuration
Imports System.Net.Mail
Imports System.IO
Imports System.Net.Mime

Namespace Services
    Public Interface IEmailService
        Inherits IBusinessBase

        Function SendEmail(ByVal toAddresses As String, ByVal fromAddresses As String, ByVal subject As String,
                         ByVal body As String, ByVal ms As MemoryStream, ByVal documentName As String, ByVal ccAddresses As String) As ReturnObject(Of Boolean)
        Function SendEmail1(ByVal toAddresses As String, ByVal FromAddresses As String, ByVal subject As String,
                           ByVal body As String, ByVal ms As List(Of DocumentUpload), ByVal ccAddresses As String) As ReturnObject(Of Boolean)

    End Interface

    Public Class EmailService
        Inherits BusinessBase
        Implements IEmailService

        Private Property EmailOverride As String = String.Empty
        <Obsolete("Use StructureMap.ObjectFactory.GetInstance(Of IEmailService)() instead!", True)>
        Public Sub New()
            Me.EmailOverride = ConfigurationManager.AppSettings("FromEmailAddress")
        End Sub
        ''' <summary>
        ''' Sends an email using the ODMRDD_NET2 dll.
        ''' </summary>
        ''' <param name="toAddresses">A semicolon delimited list of email addresses.</param>
        ''' <param name="fromAddress">The address that the email will come from</param>
        ''' <param name="subject">The subject of the email.</param>
        ''' <param name="body">The body of the email.</param>
        ''' <param name="ms">Memory stream which has pdf files.</param>
        ''' <param name="documentName">document name of pdf.</param>
        ''' <returns>A boolean indicating success or failure.</returns>
        ''' <remarks>This method wraps the functionality in ODMRDD_NET2.</remarks>
        Public Function SendEmail(ByVal toAddresses As String, ByVal fromAddress As String,
                                  ByVal subject As String, ByVal body As String, ByVal ms As MemoryStream, ByVal documentName As String, ByVal ccAddresses As String) As ReturnObject(Of Boolean) Implements IEmailService.SendEmail

            Dim retObj As New ReturnObject(Of Boolean)(ReturnValue:=False)
            Dim email As New ODMRDD_NET2.Email()


            Try
                With email
                    .ToList = toAddresses 'If(toAddresses.Contains("OPSR"), toAddresses, If(Not String.IsNullOrWhiteSpace(Me.EmailOverride), Me.EmailOverride, toAddresses))
                    .CCList = ccAddresses
                    .FromAddress = fromAddress
                    .Subject = subject
                    .Body = body
                    .IsBodyHtml = True

                    If Not String.IsNullOrEmpty(documentName) Then
                        'attach all the files to the email
                        'For Each attachFile In attachmentFilenames
                        .Attachments.Add(New Attachment(ms, documentName, "application/pdf"))
                        'Next
                    End If

                    Dim mailResponse As String = .Send()

                    If Not String.IsNullOrWhiteSpace(mailResponse) AndAlso "MESSAGE SENT".Equals(mailResponse.ToUpper()) Then
                        retObj.ReturnValue = True
                    Else
                        retObj.AddErrorMessage(String.Format("Unable to send email. {0}", mailResponse))
                    End If
                End With
            Catch ex As Exception
                retObj.AddErrorMessage(String.Format("Error Sending Email: {0}", ex.Message))
                Me.LogError("Error Sending Email in EmailService.SendEmail", CInt(Me.UserID), ex)
            Finally
                email.Dispose()
            End Try

            Return retObj
        End Function

        ''' <summary>
        ''' Sends an email using the ODMRDD_NET2 dll.
        ''' </summary>
        ''' <param name="toAddresses">A semicolon delimited list of email addresses.</param>
        ''' <param name="fromAddresses">The address that the email will come from</param>
        ''' <param name="subject">The subject of the email.</param>
        ''' <param name="body">The body of the email.</param>
        ''' <param name="ms">list of document that need to be attached to the email</param>
        ''' <returns>A boolean indicating success or failure.</returns>
        ''' <remarks>This method wraps the functionality in ODMRDD_NET2.</remarks>
        Public Function SendEmail1(toAddresses As String, FromAddresses As String, subject As String, body As String, ms As List(Of DocumentUpload), ccAddresses As String) As ReturnObject(Of Boolean) Implements IEmailService.SendEmail1
            Dim retObj As New ReturnObject(Of Boolean)(ReturnValue:=False)
            Dim email As New ODMRDD_NET2.Email()
            Try
                With email
                    .ToList = toAddresses
                    .CCList = ccAddresses
                    .FromAddress = FromAddresses
                    .Subject = subject
                    .Body = body
                    .IsBodyHtml = True
                    If ms IsNot Nothing Then
                        For Each attachFile In ms
                            Dim bstream As New MemoryStream
                            bstream.Write(attachFile.DocumentImage, 0, attachFile.DocumentImage.Length)
                            bstream.Position = 0
                            Dim docExtension() As String = attachFile.DocumentName.Split(".")
                            Dim lastPart As String = docExtension(1)
                            .Attachments.Add(New Attachment(bstream, attachFile.DocumentName, "application/" & lastPart))
                        Next
                    End If
                    Dim mailResponse As String = .Send()

                    If Not String.IsNullOrWhiteSpace(mailResponse) AndAlso "MESSAGE SENT".Equals(mailResponse.ToUpper()) Then
                        retObj.ReturnValue = True
                    Else
                        retObj.AddErrorMessage(String.Format("Unable to send email. {0}", mailResponse))
                    End If
                End With
            Catch ex As Exception
                retObj.AddErrorMessage(String.Format("Error Sending Email: {0}", ex.Message))
                Me.LogError("Error Sending Email in EmailService.SendEmail", CInt(Me.UserID), ex)
            Finally
                email.Dispose()
            End Try

            Return retObj
        End Function
    End Class
End Namespace
