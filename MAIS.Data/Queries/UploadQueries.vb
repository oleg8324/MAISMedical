Imports System.Data.Linq
Imports ODMRDDHelperClassLibrary.Utility
Imports System.Configuration
Imports System.Data.Objects
Imports MAIS.Data
Namespace Queries
    Public Interface IUploadQueries
        Inherits IQueriesBase
        Function SaveUploadedDocument(ByVal uploadedDoc As Data.Application_Uploaded_Document) As Boolean
        Function InsertUploadedDocument(ByVal uploadedDoc As Data.Application_Uploaded_Document) As Boolean
        Function AddUploadedDocumentToImageStore(ByRef uploadedDoc As Data.Document_Image_Store) As Long
        Function GetUploadedDocumentByImageSID(ByVal imageSID As Long) As Data.Document_Image_Store
        Function MarkDocumentSavedUDS(ByVal imageSID As Long) As Long
        Function GetUploadedDocuments(ByVal applicationID As Integer) As List(Of Data.Application_Uploaded_Document)
        Function GetUploadedDocumentsByImageSID(ByVal imageSID As Integer) As Data.Application_Uploaded_Document
        Function GetUploadedReissuedCert(ByVal code As String) As Data.Application_Uploaded_Document
        Function GetDocumentTypeSids(ByVal ls As List(Of String)) As List(Of Integer)
        Function GetTypeDesc(ByVal tid As Integer) As String
        Function GetDocumentsByNotation(ByVal NotationID As Integer) As List(Of Data.Application_Uploaded_Document)
        Function DeleteDocumentByStoreSid(ByVal StoreSid As Integer) As Boolean
        Function GetDocumentUploadForpageComplete(ByVal applicationID As Integer) As Integer
        Function GetUploadedDocumentsImageStore(imageSID As Long) As Byte()
        Function GetUploadedDocumentsNotInUDS(ByVal applicationID As Integer) As List(Of Data.Application_Uploaded_Document)
    End Interface
    Public Class UploadQueries
        Inherits QueriesBase
        Implements IUploadQueries
        Public Function SaveUploadedDocument(ByVal uploadedDoc As Data.Application_Uploaded_Document) As Boolean Implements IUploadQueries.SaveUploadedDocument
            Dim retbool As Boolean = False
            Try
                Using context As New MAISContext()
                    'Dim uploadedDocuments As List(Of Data.Application_Uploaded_Document)
                    'uploadedDocuments = GetUploadedDocumentByFileName(uploadedDoc.ApplcntApplSID, uploadedDoc.FileName)
                    'If uploadedDocuments IsNot Nothing AndAlso uploadedDocuments.Count > 0 Then
                    '    ' Delete existing documents.
                    '    For Each doc In uploadedDocuments.ToList()
                    '        DeleteDocumentByFileName(doc.ApplcntApplSID, doc.FileName)
                    '        context.SubmitChanges()
                    '    Next
                    'End If

                    ' insert new uploaded doc
                    With uploadedDoc
                        .Create_By = Me.UserID
                        .Create_Date = DateTime.Now
                        .Last_Update_By = Me.UserID
                        .Last_Update_Date = DateTime.Now
                    End With

                    context.Application_Uploaded_Document.Add(uploadedDoc)
                    context.SaveChanges()
                    retbool = True
                End Using
            Catch ex As Exception
                'TODO: add message from message helper and add logger as well.
                Me.LogError("Error while storing uploaded document.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error while storing uploaded document.", True, False))

            End Try
            Return retbool
        End Function

        Public Function InsertUploadedDocument(ByVal uploadedDoc As Data.Application_Uploaded_Document) As Boolean Implements IUploadQueries.InsertUploadedDocument
            Dim retVal As Boolean = False

            Try
                Using context As New MAISContext()
                    If Not context.Application_Uploaded_Document.Any(Function(u) (u.Application_Sid = uploadedDoc.Application_Sid And
                                                                              u.File_Name = uploadedDoc.File_Name)) Then
                        ' insert new uploaded doc
                        With uploadedDoc
                            .Create_By = Me.UserID
                            .Create_Date = DateTime.Now
                            .Last_Update_By = Me.UserID
                            .Last_Update_Date = DateTime.Now
                        End With

                        context.Application_Uploaded_Document.Add(uploadedDoc)
                        context.SaveChanges()
                        retVal = True
                    End If
                End Using
            Catch ex As Exception
                'TODO: add message from message helper and add logger as well.
                Me.LogError("Error while Inserting uploaded document.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error while Inserting uploaded document.", True, False))
            End Try

            Return retVal
        End Function

        Public Function AddUploadedDocumentToImageStore(ByRef uploadedDoc As Data.Document_Image_Store) As Long Implements IUploadQueries.AddUploadedDocumentToImageStore
            Dim returnAddedDocumentImageSID As Long = 0
            Try
                Using context As New MAISContext()
                    uploadedDoc.Create_By = Me.UserID
                    uploadedDoc.Create_Date = DateTime.Now
                    uploadedDoc.Last_Update_By = Me.UserID
                    uploadedDoc.Last_Update_Date = DateTime.Now
                    uploadedDoc.Source_Page = ""
                    uploadedDoc.UDS_Uploaded_Flg = False
                    context.Document_Image_Store.Add(uploadedDoc)
                    context.SaveChanges()
                    returnAddedDocumentImageSID = uploadedDoc.Document_Image_Store_Sid
                End Using
            Catch ex As Exception
                'TODO: add message from message helper and add logger as well.
                Me.LogError("Error while adding uploaded document.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error while adding uploaded document.", True, False))

            End Try

            Return returnAddedDocumentImageSID
        End Function

        Public Function GetUploadedDocumentByImageSID(ByVal imageSID As Long) As Data.Document_Image_Store Implements IUploadQueries.GetUploadedDocumentByImageSID
            Dim returnUploadedDocument As Data.Document_Image_Store = Nothing

            Try
                Using context As New MAISContext()
                    returnUploadedDocument = (From i In context.Document_Image_Store Where i.Document_Image_Store_Sid = imageSID
                                      Select i).SingleOrDefault
                End Using
            Catch ex As Exception
                Me.LogError("Error while retrieving documents by ImageSID.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error while retrieving documents by ImageSID.", True, False))

            End Try

            Return returnUploadedDocument
        End Function
        Public Function MarkDocumentSavedUDS(ByVal imageSID As Long) As Long Implements IUploadQueries.MarkDocumentSavedUDS

            Try
                Using context As New MAISContext()
                    Dim Doc As Data.Document_Image_Store
                    Doc = (From i In context.Document_Image_Store Where i.Document_Image_Store_Sid = imageSID
                                      Select i).SingleOrDefault
                    If Not IsNothing(Doc) Then
                        Doc.UDS_Uploaded_Flg = True
                    End If
                    context.SaveChanges()
                End Using
            Catch ex As Exception
                Me.LogError("Error while saving documents by ImageSID.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error while saving documents by ImageSID.", True, False))
            End Try
            Return imageSID
        End Function

        Public Function GetUploadedDocuments(ByVal applicationID As Integer) As List(Of Data.Application_Uploaded_Document) Implements IUploadQueries.GetUploadedDocuments
            Dim retDocuments As New List(Of Data.Application_Uploaded_Document)()

            Try
                Using context As New MAISContext()
                    retDocuments = (From u In context.Application_Uploaded_Document
                                    Where u.Application_Sid = applicationID And u.Document_Type.Document_Type_Desc <> "Notation Document"
                                      Select u).ToList

                    'retDocuments.AddRange(_GetUploadedDocuments(context, applicationID))
                End Using
            Catch ex As Exception
                'TODO: add message from message helper and add logger as well.
                Me.LogError("Error while returning list of uploaded documents.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error while returning list of uploaded documents.", True, False))

            End Try
            Return retDocuments

        End Function
        Public Function GetUploadedDocumentsByImageSID(ByVal imageSID As Integer) As Data.Application_Uploaded_Document Implements IUploadQueries.GetUploadedDocumentsByImageSID
            Dim retDocuments As New Data.Application_Uploaded_Document

            Try
                Using context As New MAISContext()
                    retDocuments = (From u In context.Application_Uploaded_Document
                                    Where u.Document_Image_Store_Sid = imageSID And u.Document_Type.Document_Type_Desc = "Certificate"
                                      Select u).FirstOrDefault

                    'retDocuments.AddRange(_GetUploadedDocuments(context, applicationID))
                End Using
            Catch ex As Exception
                'TODO: add message from message helper and add logger as well.
                Me.LogError("Error while returning list of uploaded documents. by Image id", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error while returning list of uploaded documents. by Image id", True, False))

            End Try
            Return retDocuments

        End Function
        Public Function GetUploadedReissuedCert(ByVal code As String) As Data.Application_Uploaded_Document Implements IUploadQueries.GetUploadedReissuedCert
            Dim retDocument As New Data.Application_Uploaded_Document

            Try
                Using context As New MAISContext()
                    retDocument = (From u In context.Application_Uploaded_Document Join istore In context.Document_Image_Store On u.Document_Image_Store_Sid Equals istore.Document_Image_Store_Sid Where istore.UDS_Uploaded_Flg = False AndAlso u.File_Name.Contains("Reissue_" & code) Select u).FirstOrDefault

                    'retDocuments.AddRange(_GetUploadedDocuments(context, applicationID))
                End Using
            Catch ex As Exception
                'TODO: add message from message helper and add logger as well.
                Me.LogError("Error while returning  uploaded documents reissue cert", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error while returning  uploaded documents reissue cert", True, False))

            End Try
            Return retDocument
        End Function
        Public Function GetUploadedDocumentsNotInUDS(ByVal applicationID As Integer) As List(Of Data.Application_Uploaded_Document) Implements IUploadQueries.GetUploadedDocumentsNotInUDS
            Dim retDocuments As New List(Of Data.Application_Uploaded_Document)()
            Try
                Using context As New MAISContext()
                    retDocuments = (From u In context.Application_Uploaded_Document Join istore In context.Document_Image_Store On u.Document_Image_Store_Sid Equals istore.Document_Image_Store_Sid Where u.Application_Sid = applicationID And istore.UDS_Uploaded_Flg = False
                                      Select u).ToList
                    'retDocuments.AddRange(_GetUploadedDocuments(context, applicationID))
                End Using
            Catch ex As Exception
                'TODO: add message from message helper and add logger as well.
                Me.LogError("Error while returning list of uploaded documents not in UDS.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error while returning list of uploaded documents not in UDS.", True, False))
            End Try
            Return retDocuments
        End Function
        Public Function GetDocumentTypeSids(ByVal ls As List(Of String)) As List(Of Integer) Implements IUploadQueries.GetDocumentTypeSids
            Dim li As New List(Of Integer)
            Try
                Using context As New MAISContext()
                    For Each s As String In ls
                        li.Add((From dt In context.Document_Type Where dt.Document_Type_Desc = s Select dt.Document_Type_Sid).FirstOrDefault)
                    Next
                End Using
            Catch ex As Exception
                Me.LogError("Error while returning  documents by sid.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error while returning  documents by sid..", True, False))
            End Try
            Return li
        End Function
        Public Function GetTypeDesc(ByVal tid As Integer) As String Implements IUploadQueries.GetTypeDesc
            Dim retstr As String = String.Empty
            Try
                Using context As New MAISContext()
                    retstr = (From t In context.Document_Type Where t.Document_Type_Sid = tid Select t.Document_Type_Desc).FirstOrDefault
                End Using
            Catch ex As Exception
                Me.LogError("Error while returning  documents description.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error while returning  documents description.", True, False))
            End Try
            Return retstr
        End Function
        Public Function GetDocumentsByNotation(ByVal NotationID As Integer) As List(Of Data.Application_Uploaded_Document) Implements IUploadQueries.GetDocumentsByNotation
            Dim retDocuments As New List(Of Data.Application_Uploaded_Document)()
            Try
                Using context As New MAISContext()
                    retDocuments = (From u In context.Application_Uploaded_Document Where u.Application_Notation_Sid = NotationID
                                      Select u).ToList
                    'retDocuments.AddRange(_GetUploadedDocuments(context, applicationID))
                End Using
            Catch ex As Exception
                'TODO: add message from message helper and add logger as well.
                Me.LogError("Error while returning list of uploaded documents for notations.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error while returning list of uploaded documents for notations.", True, False))
            End Try
            Return retDocuments
        End Function
        'Public Function GetUploadedDocumentByFileName(ByVal applicationID As Integer, ByVal fileName As String) As List(Of Data.ApplicantUpload) 
        '    Dim retDocuments As New List(Of Data.ApplicantUpload)()

        '    Try
        '        Using context As New ProviderDataContext(Me._connProviderDB)
        '            retDocuments.AddRange(_GetUploadedDocumentByFileName(context, applicationID, fileName))
        '        End Using
        '    Catch ex As Exception
        '        'TODO: add message from message helper and add logger as well.
        '        Me._messages.Add(New ReturnMessage("Error while returning list of uploaded documents.", True, False))
        '        Throw ex
        '    End Try

        '    Return retDocuments
        'End Function

        'Public Function GetUploadedDocumentByRequirementID(ByVal applicationID As Long, ByVal requirementID As Long) As List(Of Data.ApplicantUpload)
        '    Dim retDocuments As New List(Of Data.ApplicantUpload)()

        '    Try
        '        Using context As New ProviderDataContext(Me._connProviderDB)
        '            retDocuments.AddRange(_GetUploadedDocumentByRequirementID(context, applicationID, requirementID))
        '        End Using
        '    Catch ex As Exception
        '        'TODO: add message from message helper and add logger as well.
        '        Me._messages.Add(New ReturnMessage("Error while returning list of uploaded documents.", True, False))
        '        Throw ex
        '    End Try

        '    Return retDocuments
        'End Function

        'Public Function DeleteDocumentByFileName(ByVal applicationID As Long, ByVal fileName As String) As Boolean
        '    Dim retVal As Boolean = False
        '    Dim DelDoc As Data.Application_Uploaded_Document
        '    Dim DelDocStore As Data.Document_Image_Store
        '    Try
        '        Using context As New MAISContext()
        '            DelDoc=(From d In context.Application_Uploaded_Document
        '            context.Application_Uploaded_Document.Remove().ApplicantUploads.DeleteAllOnSubmit(_GetUploadedDocumentByFileName(context, applicationID, fileName).ToList())
        '            context.SubmitChanges()
        '            retVal = True
        '        End Using
        '    Catch ex As Exception
        '        'TODO: add message from message helper and add logger as well.
        '        Me._messages.Add(New ReturnMessage("Error while deleting of uploaded documents.", True, False))
        '        Throw ex
        '    End Try

        '    Return retVal
        'End Function
        Public Function DeleteDocumentByStoreSid(ByVal StoreSid As Integer) As Boolean Implements IUploadQueries.DeleteDocumentByStoreSid
            Dim retVal As Boolean = False
            Dim DelDoc As Data.Application_Uploaded_Document
            Dim DelDocStore As Data.Document_Image_Store
            Try
                Using context As New MAISContext()
                    DelDoc = (From d In context.Application_Uploaded_Document Where d.Document_Image_Store_Sid = StoreSid Select d).FirstOrDefault
                    DelDocStore = (From dis In context.Document_Image_Store Where dis.Document_Image_Store_Sid = StoreSid Select dis).FirstOrDefault
                    context.Application_Uploaded_Document.Remove(DelDoc)
                    context.Document_Image_Store.Remove(DelDocStore)
                    '.ApplicantUploads.DeleteAllOnSubmit(_GetUploadedDocumentByFileName(context, ApplicationId, fileName).ToList())
                    context.SaveChanges()
                    retVal = True
                End Using
            Catch ex As Exception
                'TODO: add message from message helper and add logger as well.
                Me.LogError("Error while deleting uploaded documents by sid.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error while deleting uploaded documents by sid.", True, False))

            End Try
            Return retVal
        End Function
        Public Function GetDocumentUploadForpageComplete(ByVal applicationID As Integer) As Integer Implements IUploadQueries.GetDocumentUploadForpageComplete
            Dim uploadedFiles As List(Of Data.Application_Uploaded_Document)
            Dim hasApp As Boolean = False
            Dim hasSec As Boolean = False
            Dim exists As Integer = 0
            Using context As New MAISContext()
                Try
                    'Dim aptype As String = (From a In context.Applications Where a.Application_Sid = applicationID Select a.Application_Type.Application_Desc).FirstOrDefault
                    uploadedFiles = Me.GetUploadedDocuments(applicationID)
                    For Each m As Data.Application_Uploaded_Document In uploadedFiles
                        Dim dtype As String = (From ut In context.Document_Type Where ut.Document_Type_Sid = m.Document_Type_Sid Select ut.Document_Type_Desc).FirstOrDefault
                        If dtype = "Application" Then
                            hasApp = True
                        ElseIf dtype = "Security Disclosure Form" Then
                            hasSec = True
                        End If
                    Next
                    If hasApp Then
                        exists = 1
                        Return exists
                    End If
                    'If (aptype = "Renewal" Or aptype = "Update") Then
                    '    exists = 2
                    '    Return exists
                    'End If
                    If hasApp = False Then
                        exists = exists - 1
                    End If
                    'If hasSec = False Then
                    '    exists = exists - 2
                    'End If
                    Return exists
                Catch ex As Exception
                    Me.LogError("Error Getting upload page complete rule.", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error upload page complete rule.", True, False))
                End Try
            End Using
            Return exists
        End Function
        Public Function GetUploadedDocumentsImageStore(imageSID As Long) As Byte() Implements IUploadQueries.GetUploadedDocumentsImageStore
            Dim retDocuments() As Byte = Nothing
            Try
                Using context As New MAISContext()
                    retDocuments = (From u In context.Document_Image_Store Where u.Document_Image_Store_Sid = imageSID
                                      Select u.Image_Store).FirstOrDefault
                    'retDocuments.AddRange(_GetUploadedDocuments(context, applicationID))
                End Using
            Catch ex As Exception
                'TODO: add message from message helper and add logger as well.
                Me.LogError("Error Getting document image store.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error Getting document image store.", True, False))
            End Try
            Return retDocuments
        End Function
    End Class
End Namespace
