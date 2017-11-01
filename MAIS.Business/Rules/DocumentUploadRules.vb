Imports ODMRDDHelperClassLibrary.Utility
Imports System.Text.RegularExpressions
Imports MAIS.Business.Services

Namespace Rules
    Public Class DocumentUploadRules
        Inherits BusinessBase

        Public Shared Function CheckReqDocs(ByVal app As Integer) As ReturnObject(Of Boolean)
            Dim hasApp As Boolean = False
            Dim hasSec As Boolean = False
            Dim retObj As New ReturnObject(Of Boolean)()
            retObj.ReturnValue = False
            Dim res As Integer = -1
            'Dim uploadedFiles As New List(Of Business.Model.DocumentUpload)
            Dim uploadSvc As IUploadService = StructureMap.ObjectFactory.GetInstance(Of IUploadService)()

            res = uploadSvc.GetDocumentUploadForpageComplete(app)
            If res > 0 Then
                retObj.ReturnValue = True
            ElseIf res = -1 Then
                retObj.AddErrorMessage("Application upload is Required to proceed.")
            ElseIf res = -2 Then
                retObj.AddErrorMessage("Security Disclosure Form upload is Required to proceed.")
            ElseIf res < -2 Then
                retObj.AddErrorMessage("Application upload is Required to proceed.")
                retObj.AddErrorMessage("Security Disclosure Form upload is Required to proceed.")
            End If

            Return retObj
        End Function
    End Class
End Namespace