Imports ODMRDDHelperClassLibrary
Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports MAIS.Data
Imports MAIS.Data.Objects
Imports System.Configuration
Namespace Services
    Public Interface IUploadService
        Inherits IBusinessBase
        Function DeleteDocumentByStoreSid(ByVal StoreSid As Integer) As Boolean
        'Function DeleteDocumentByFileName(ByVal applicationID As Long, ByVal fileName As String) As Boolean
        Function GetUploadedDocumentByImageSID(imageSID As Long) As Byte()
        Function GetUploadedDocuments(ByVal applicationID As Long) As List(Of Model.DocumentUpload)
        Function GetUploadedDocumentsByImageSID(ByVal imageSID As Long) As Model.DocumentUpload
        Function GetUploadedReissuedCert(ByVal code As String) As Model.DocumentUpload
        Function GetUploadedDocumentsImageStore(ByVal imageSID As Long) As Byte()
        Function GetUploadedDocumentsNotInUDS(ByVal applicationID As Long) As List(Of Model.DocumentUpload)
        'GetUploadedDocumentsNotInUDS
        'Function GetUploadedDocumentsByRequirementID(ByVal applicationID As Long, ByVal requirementID As Long) As List(Of Model.DocumentUpload)
        Function InsertUploadedDocumentIntoImageStore(ByVal dataBytes As Byte()) As Long
        Function InsertUploadedDocument(ByVal uploadedDocument As Model.DocumentUpload, ByVal applicationID As Integer) As Boolean
        Function SaveUploadedDocument(ByVal uploadedDocument As MAIS.Business.Model.DocumentUpload, ByVal applicationID As Long) As Boolean
        Function GetDocumentsByNotation(ByVal NotationID As Integer) As List(Of Model.DocumentUpload)
        Function GetDocumentUploadForpageComplete(ByVal applicationID As Integer) As Integer
        Function MarkDocumentSavedUDS(ByVal imageSID As Long) As Long
    End Interface
    Public Class UploadService
        Inherits BusinessBase
        Implements IUploadService
        Private _queries As Data.Queries.IUploadQueries
        Public Sub New(ByVal user As IUserIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of Data.Queries.IUploadQueries)()
            _queries.UserID = user.UserID.ToString()
            '_queries.MAISConnectionString = connectionstring.ConnectionString
        End Sub
        Public Function DeleteDocumentByStoreSid(ByVal StoreSid As Integer) As Boolean Implements IUploadService.DeleteDocumentByStoreSid
            Dim retVal As Boolean = False
            Try
                retVal = _queries.DeleteDocumentByStoreSid(StoreSid)
            Catch ex As Exception
                If Me._queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(Me._queries.Messages)
                    For Each message In Me._queries.Messages
                        If message.IsError Then
                            Me.LogError(message.ToString(), CInt(Me.UserID), ex)
                        ElseIf message.IsWarning Then
                            Me.LogWarning(message.ToString(), ex)
                        End If
                    Next
                    Me._queries.Messages.Clear()
                Else
                    Me._messages.Add(New ODMRDDHelperClassLibrary.Utility.ReturnMessage("Error while deleting document DeleteDocumentByStoreSid.", True, False))
                    Me.LogError("Error while deleting document DeleteDocumentByStoreSid.", CInt(Me.UserID), ex)
                End If
            End Try
            Return retVal
        End Function
        Public Function GetUploadedReissuedCert(ByVal code As String) As Model.DocumentUpload Implements IUploadService.GetUploadedReissuedCert
            Dim uploadedDocument As New Model.DocumentUpload
            Try
                uploadedDocument = (Mapping.UploadMapping.MapDBToModelNotList(_queries.GetUploadedReissuedCert(code)))
                uploadedDocument.DocumentType = _queries.GetTypeDesc(uploadedDocument.DocumentTypeID)

                If _queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(_queries.Messages)
                    Me._queries.Messages.Clear()
                End If
            Catch ex As Exception
                'TODO: add message helper and logger
                If Me._queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(Me._queries.Messages)
                    For Each message In Me._queries.Messages
                        If message.IsError Then
                            Me.LogError(message.ToString(), CInt(Me.UserID), ex)
                        ElseIf message.IsWarning Then
                            Me.LogWarning(message.ToString(), ex)
                        End If
                    Next
                    Me._queries.Messages.Clear()
                Else
                    Me._messages.Add(New ODMRDDHelperClassLibrary.Utility.ReturnMessage("Error while mapping the uploaded documents GetUploadedReissuedCert.", True, False))
                    Me.LogError("Error while mapping the uploaded documents GetUploadedReissuedCert.", CInt(Me.UserID), ex)
                End If
            End Try
            Return uploadedDocument
        End Function
        Public Function GetUploadedDocumentByImageSID(ByVal imageSID As Long) As Byte() Implements IUploadService.GetUploadedDocumentByImageSID
            Dim returnUploadedDocumentObject As New List(Of Byte)

            Try
                returnUploadedDocumentObject.AddRange(_queries.GetUploadedDocumentByImageSID(imageSID).Image_Store)

                If _queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(_queries.Messages)
                    Me._queries.Messages.Clear()
                End If
            Catch ex As Exception
                'TODO: add message helper and logger
                If Me._queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(Me._queries.Messages)
                    For Each message In Me._queries.Messages
                        If message.IsError Then
                            Me.LogError(message.ToString(), CInt(Me.UserID), ex)
                        ElseIf message.IsWarning Then
                            Me.LogWarning(message.ToString(), ex)
                        End If
                    Next
                    Me._queries.Messages.Clear()
                Else
                    Me._messages.Add(New ODMRDDHelperClassLibrary.Utility.ReturnMessage("Error while adding uploaded document to the UploadedDocuments List GetUploadedDocumentByImageSID.", True, False))
                    Me.LogError("Error while adding uploaded document to the UploadedDocuments List GetUploadedDocumentByImageSID.", CInt(Me.UserID), ex)
                End If
            End Try
            'Return Object As a Byte Array
            Return returnUploadedDocumentObject.ToArray()
        End Function
        Public Function GetUploadedDocuments(ByVal applicationID As Long) As List(Of Model.DocumentUpload) Implements IUploadService.GetUploadedDocuments
            Dim uploadedDocuments As New List(Of Model.DocumentUpload)
            Try
                uploadedDocuments.AddRange(Mapping.UploadMapping.MapDBToModel(_queries.GetUploadedDocuments(applicationID)))
                For Each m As Model.DocumentUpload In uploadedDocuments
                    m.DocumentType = _queries.GetTypeDesc(m.DocumentTypeID)
                Next

                If _queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(_queries.Messages)
                    Me._queries.Messages.Clear()
                End If
            Catch ex As Exception
                'TODO: add message helper and logger
                If Me._queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(Me._queries.Messages)
                    For Each message In Me._queries.Messages
                        If message.IsError Then
                            Me.LogError(message.ToString(), CInt(Me.UserID), ex)
                        ElseIf message.IsWarning Then
                            Me.LogWarning(message.ToString(), ex)
                        End If
                    Next
                    Me._queries.Messages.Clear()
                Else
                    Me._messages.Add(New ODMRDDHelperClassLibrary.Utility.ReturnMessage("Error while mapping the uploaded documents GetUploadedDocuments.", True, False))
                    Me.LogError("Error while mapping the uploaded documents GetUploadedDocuments.", CInt(Me.UserID), ex)
                End If
            End Try
            Return uploadedDocuments
        End Function
        Public Function GetUploadedDocumentsNotInUDS(ByVal applicationID As Long) As List(Of Model.DocumentUpload) Implements IUploadService.GetUploadedDocumentsNotInUDS
            Dim uploadedDocuments As New List(Of Model.DocumentUpload)
            Try
                uploadedDocuments.AddRange(Mapping.UploadMapping.MapDBToModel(_queries.GetUploadedDocumentsNotInUDS(applicationID)))
                For Each m As Model.DocumentUpload In uploadedDocuments
                    m.DocumentType = _queries.GetTypeDesc(m.DocumentTypeID)
                Next

                If _queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(_queries.Messages)
                    Me._queries.Messages.Clear()
                End If
            Catch ex As Exception
                'TODO: add message helper and logger
                If Me._queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(Me._queries.Messages)
                    For Each message In Me._queries.Messages
                        If message.IsError Then
                            Me.LogError(message.ToString(), CInt(Me.UserID), ex)
                        ElseIf message.IsWarning Then
                            Me.LogWarning(message.ToString(), ex)
                        End If
                    Next
                    Me._queries.Messages.Clear()
                Else
                    Me._messages.Add(New ODMRDDHelperClassLibrary.Utility.ReturnMessage("Error while mapping the uploaded documents GetUploadedDocumentsNotInUDS.", True, False))
                    Me.LogError("Error while mapping the uploaded documents GetUploadedDocumentsNotInUDS.", CInt(Me.UserID), ex)
                End If
            End Try
            Return uploadedDocuments
        End Function
        Public Function MarkDocumentSavedUDS(ByVal imageSID As Long) As Long Implements IUploadService.MarkDocumentSavedUDS
            Try
                Dim res As Long = _queries.MarkDocumentSavedUDS(imageSID)
                Return res
            Catch ex As Exception
                Return -1
                Me._messages.Add(New ODMRDDHelperClassLibrary.Utility.ReturnMessage("Error MarkDocumentSavedUDS.", True, False))
                Me.LogError("Error MarkDocumentSavedUDS.", CInt(Me.UserID), ex)
            End Try
        End Function

        Public Function GetDocumentsByNotation(ByVal NotationID As Integer) As List(Of Model.DocumentUpload) Implements IUploadService.GetDocumentsByNotation
            Dim uploadedDocuments As New List(Of Model.DocumentUpload)

            Try
                uploadedDocuments.AddRange(Mapping.UploadMapping.MapDBToModel(_queries.GetDocumentsByNotation(NotationID)))

                If _queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(_queries.Messages)
                    Me._queries.Messages.Clear()
                End If
            Catch ex As Exception
                'TODO: add message helper and logger
                If Me._queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(Me._queries.Messages)
                    For Each message In Me._queries.Messages
                        If message.IsError Then
                            Me.LogError(message.ToString(), CInt(Me.UserID), ex)
                        ElseIf message.IsWarning Then
                            Me.LogWarning(message.ToString(), ex)
                        End If
                    Next
                    Me._queries.Messages.Clear()
                Else
                    Me._messages.Add(New ODMRDDHelperClassLibrary.Utility.ReturnMessage("Error while mapping the uploaded documents GetDocumentsByNotation.", True, False))
                    Me.LogError("Error while mapping the uploaded documents GetDocumentsByNotation.", CInt(Me.UserID), ex)
                End If
            End Try

            Return uploadedDocuments
        End Function
        Public Function InsertUploadedDocumentIntoImageStore(ByVal dataBytes As Byte()) As Long Implements IUploadService.InsertUploadedDocumentIntoImageStore
            Dim returnImageSID As Long

            Try
                returnImageSID = _queries.AddUploadedDocumentToImageStore(Mapping.UploadMapping.MapDataStreamToDB(dataBytes))

                If _queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(_queries.Messages)
                    Me._queries.Messages.Clear()
                End If
            Catch ex As Exception
                'TODO: add message helper and logger
                If Me._queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(Me._queries.Messages)
                    For Each message In Me._queries.Messages
                        If message.IsError Then
                            Me.LogError(message.ToString(), CInt(Me.UserID), ex)
                        ElseIf message.IsWarning Then
                            Me.LogWarning(message.ToString(), ex)
                        End If
                    Next
                    Me._queries.Messages.Clear()
                Else
                    Me._messages.Add(New ODMRDDHelperClassLibrary.Utility.ReturnMessage("Error in mapping the uploaded document to DataStream InsertUploadedDocumentIntoImageStore.", True, False))
                    Me.LogError("Error in mapping the uploaded document to DataStream InsertUploadedDocumentIntoImageStore.", CInt(Me.UserID), ex)
                End If
            End Try

            Return returnImageSID
        End Function
        Public Function InsertUploadedDocument(ByVal uploadedDocument As Model.DocumentUpload, ByVal applicationID As Integer) As Boolean Implements IUploadService.InsertUploadedDocument
            Dim retVal As Boolean = False

            Try
                retVal = _queries.InsertUploadedDocument(Mapping.UploadMapping.MapModelToDB(uploadedDocument, applicationID))

                If _queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(_queries.Messages)
                    Me._queries.Messages.Clear()
                End If
            Catch ex As Exception
                'TODO: add message helper and logger
                If Me._queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(Me._queries.Messages)
                    For Each message In Me._queries.Messages
                        If message.IsError Then
                            Me.LogError(message.ToString(), CInt(Me.UserID), ex)
                        ElseIf message.IsWarning Then
                            Me.LogWarning(message.ToString(), ex)
                        End If
                    Next
                    Me._queries.Messages.Clear()
                Else
                    Me._messages.Add(New ODMRDDHelperClassLibrary.Utility.ReturnMessage("Error in mapping the uploaded document InsertUploadedDocument.", True, False))
                    Me.LogError("Error in mapping the uploaded document InsertUploadedDocument.", CInt(Me.UserID), ex)
                End If
            End Try

            Return retVal
        End Function

        Public Function SaveUploadedDocument(ByVal uploadedDocument As Model.DocumentUpload, ByVal applicationID As Long) As Boolean Implements IUploadService.SaveUploadedDocument
            Dim returnFlag As Boolean

            Try
                returnFlag = _queries.SaveUploadedDocument(Mapping.UploadMapping.MapModelToDB(uploadedDocument, applicationID))

                If _queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(_queries.Messages)
                    Me._queries.Messages.Clear()
                End If
            Catch ex As Exception
                'TODO: add message helper and logger
                If Me._queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(Me._queries.Messages)
                    For Each message In Me._queries.Messages
                        If message.IsError Then
                            Me.LogError(message.ToString(), CInt(Me.UserID), ex)
                        ElseIf message.IsWarning Then
                            Me.LogWarning(message.ToString(), ex)
                        End If
                    Next
                    Me._queries.Messages.Clear()
                Else
                    Me._messages.Add(New ODMRDDHelperClassLibrary.Utility.ReturnMessage("Error in mapping the uploaded document to DataStream SaveUploadedDocument.", True, False))
                    Me.LogError("Error in mapping the uploaded document to DataStream SaveUploadedDocument.", CInt(Me.UserID), ex)
                End If
            End Try

            Return returnFlag
        End Function
        Public Function GetDocumentUploadForpageComplete(ByVal applicationID As Integer) As Integer Implements IUploadService.GetDocumentUploadForpageComplete
            Dim exists As Integer = 0
            Try
                exists = _queries.GetDocumentUploadForpageComplete(applicationID)
            Catch ex As Exception
                Me.LogError("Error Getting document upload page complete rule.", CInt(Me.UserID), ex)
            End Try
            Return exists
        End Function

        Public Function GetUploadedDocumentsImageStore(imageSID As Long) As Byte() Implements IUploadService.GetUploadedDocumentsImageStore
            Dim exists() As Byte = Nothing
            Try
                exists = _queries.GetUploadedDocumentsImageStore(imageSID)
            Catch ex As Exception
                Me.LogError("Error Getting document image using document image id.", CInt(Me.UserID), ex)
            End Try
            Return exists
        End Function

        Public Function GetUploadedDocumentsByImageSID(imageSID As Long) As DocumentUpload Implements IUploadService.GetUploadedDocumentsByImageSID
            Dim uploadedDocuments As New Model.DocumentUpload
            Try
                uploadedDocuments = (Mapping.UploadMapping.MapDBToModelNotList(_queries.GetUploadedDocumentsByImageSID(imageSID)))
                'For Each m As Model.DocumentUpload In uploadedDocuments
                uploadedDocuments.DocumentType = _queries.GetTypeDesc(uploadedDocuments.DocumentTypeID)
                'Next

                If _queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(_queries.Messages)
                    Me._queries.Messages.Clear()
                End If
            Catch ex As Exception
                'TODO: add message helper and logger
                If Me._queries.Messages.Count <> 0 Then
                    Me._messages.AddRange(Me._queries.Messages)
                    For Each message In Me._queries.Messages
                        If message.IsError Then
                            Me.LogError(message.ToString(), CInt(Me.UserID), ex)
                        ElseIf message.IsWarning Then
                            Me.LogWarning(message.ToString(), ex)
                        End If
                    Next
                    Me._queries.Messages.Clear()
                Else
                    Me._messages.Add(New ODMRDDHelperClassLibrary.Utility.ReturnMessage("Error while mapping the uploaded documents using image sid.", True, False))
                    Me.LogError("Error while mapping the uploaded documents using image sid.", CInt(Me.UserID), ex)
                End If
            End Try
            Return uploadedDocuments
        End Function
    End Class

End Namespace
