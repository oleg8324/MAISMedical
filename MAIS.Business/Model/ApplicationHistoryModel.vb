Namespace Model
    Public Class ApplicationHistoryModel
        Public Property ApplicationID As Integer
        Public Property ApplicationType As String
        Public Property UniqueCodeOrLicense As String
        Public Property FinalApplicationStatus As String
        Public Property TrainingEndDate As DateTime?
        Public Property SkillsEndDate As DateTime?
        Public Property AttestationDate As DateTime?
        Public Property DecisionMadeRNName As String
        Public Property FinalDecisionName As String
        Public Property EmailEndDate As DateTime?
        Public Property CertificatePrintDate As DateTime?
        Public Property ListOfApplicationDetail As List(Of ApplicationHistoryStatusDetail)
    End Class
    Public Class ApplicationHistoryStatusDetail
        Public Property RNName As String
        Public Property ApplicationStatus As String
        Public Property ApplicationLatestUpdatedDate As DateTime
        Public Property UserRole As String
    End Class
End Namespace
