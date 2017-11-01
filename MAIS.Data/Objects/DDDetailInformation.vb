Namespace Objects
    Public Class DDDetailInformation
        Property FirstName As String
        Property DDPersonnelCode As String
        Property Last4SSN As Integer
        Property LastName As String
        Property RoleRNDD As Integer
        Property dob As Date
        Property CertificateDetails As List(Of CertificateDetails)
    End Class
    Public Class TrainingSessionResults
        Property StartDate As DateTime
        Property EndDate As DateTime
        Property RNTrainerFirstName As String
        Property RNTrainerlastName As String
        Property CourseCategory As String
        Property County As String
        Property RNTrainerEmail As String
        Property OBNNumber As String
    End Class
End Namespace
