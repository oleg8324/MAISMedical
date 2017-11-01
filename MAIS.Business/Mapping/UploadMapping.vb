Imports MAIS.Business.Services
Imports System.Data.Linq
Imports MAIS.Data
Namespace Mapping
    Public Class UploadMapping
        Public Shared Function MapDBToModel(ByVal dbDocuments As List(Of MAIS.Data.Application_Uploaded_Document)) As List(Of Model.DocumentUpload)
            'Dim reqService As IRequiredDocumentService = StructureMap.ObjectFactory.GetInstance(Of IRequiredDocumentService)()
            Dim dl As New List(Of Model.DocumentUpload)
            'Using context As New MAISContext()
            For Each dbd As Application_Uploaded_Document In dbDocuments
                dl.Add(New Model.DocumentUpload With {.ImageSID = dbd.Document_Image_Store_Sid,
                                                      .DocumentName = dbd.File_Name,
                                                      .DocumentTypeID = dbd.Document_Type_Sid,
                                                      .UploadDate = dbd.Create_Date,
                .DocumentType = dbd.Document_Type_Sid.ToString
                    })
            Next
            '(From t In context.Document_Type Where t.Document_Type_Sid = dbd.Document_Type_Sid Select t.Document_Type_Desc).FirstOrDefault
            'End Using
            Return dl
            '(From doc In dbDocuments _
            '                    Select New Model.DocumentUpload With
            '                    {
            '                        .ImageSID = doc.Document_Image_Store_Sid,
            '                        .DocumentName = doc.File_Name,
            '                        .DocumentType = doc.Document_Type_Sid.ToString,
            '            .UploadDate = doc.Create_Date
            '                    }).ToList()
        End Function
        Public Shared Function MapDBToModelNotList(ByVal dbDocuments As MAIS.Data.Application_Uploaded_Document) As Model.DocumentUpload
            'Using context As New MAISContext()
            Return (New Model.DocumentUpload With {.ImageSID = dbDocuments.Document_Image_Store_Sid,
                                                      .DocumentName = dbDocuments.File_Name,
                                                      .DocumentTypeID = dbDocuments.Document_Type_Sid,
                                                      .UploadDate = dbDocuments.Create_Date,
                .DocumentType = dbDocuments.Document_Type_Sid.ToString
                                                  })
        End Function

        Public Shared Function MapDataStreamToDB(ByVal dataBytes As Byte()) As Data.Document_Image_Store
            Return (New Data.Document_Image_Store() With
                    {
                        .Image_Store = dataBytes
                    })
        End Function

        Public Shared Function MapModelToDB(ByVal uploadedDocument As MAIS.Business.Model.DocumentUpload, ByVal applicationID As Integer) As MAIS.Data.Application_Uploaded_Document
            Dim latestAppID As Integer?
            If (applicationID = 0) Then
                latestAppID = Nothing
            Else
                latestAppID = applicationID
            End If
            Return (New MAIS.Data.Application_Uploaded_Document With
                    {
                        .Application_Sid = latestAppID,
                        .File_Name = uploadedDocument.DocumentName,
                        .Active_Flg = True,
                        .Application_Notation_Sid = uploadedDocument.AppNotId,
                        .Document_Type_Sid = uploadedDocument.DocumentType,
                        .Document_Image_Store_Sid = uploadedDocument.ImageSID
                    })
        End Function
    End Class
End Namespace