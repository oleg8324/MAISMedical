Namespace Objects
    Public Class ApplicationHistory
        Public Property ApplicationID As Integer
        Public Property ApplicationType As String
        Public Property ApplicationTypeID As Integer
        Public Property RoleLevelCategory As Integer
        Public Property FinalApplicationStatus As String
        Public Property FinalApplicationStatusID As Integer
        Public Property TrainingEndDate As DateTime?
        Public Property SkillsEndDate As DateTime?
        Public Property AttestationDate As DateTime?
        Public Property DecisionMadeRNFirstName As String
        Public Property DecisionMadeRNLastName As String
        Public Property FinalDecisionLastName As String
        Public Property FinalDecisionFirstName As String
        Public Property EmailEndDate As DateTime?
        Public Property CertificatePrintDate As DateTime?
        Public Property CreateDate As DateTime
        Public Property LastUpdatedUserID As Integer
        Public Property DDPersonnelCode As String
        Public Property Last4SSNorRNLicenseNumber As String
        Public Property ListOfApplicationDetail As List(Of ApplicationHistoryStatusDetail)
    End Class
    Public Class ApplicationHistoryStatusDetail
        Public Property RNFirstName As String
        Public Property RNlastName As String
        Public Property ApplicationStatus As String
        Public Property ApplicationLatestUpdatedDate As DateTime
        Public Property UserRole As String
    End Class
End Namespace
