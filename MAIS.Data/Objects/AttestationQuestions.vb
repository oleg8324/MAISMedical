Imports System.ComponentModel.DataAnnotations

Namespace Objects
    Public Class AttestationQuestions
        Public Property Attestation_SID As Integer
        Public Property AttestationDesc As String
        Public Property Attestation_ApplicationType_Xref_Sid As Integer
        Public Property ApplicationType_Sid As Integer
        Public Property StartDate As DateTime
        Public Property EndDate As DateTime
        Public Property Role_Sid As Integer
    End Class

End Namespace