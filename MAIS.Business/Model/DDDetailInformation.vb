Namespace Models
    Public Class DDDetailInformation
        Property FirstName As String
        Property dob As Date
        Property DDPersonnelCode As String
        Property Last4SSN As String
        Property LastName As String
        Property CertificateDetails As List(Of CertificateDetails)
    End Class
    Public Class TrainingSessionResults
        Property StartDate As DateTime
        Property EndDate As DateTime
        Property RNTrainerName As String
        Property CourseCategory As String
        Property County As String
        Property RNTrainerEmail As String
        Property OBNNumber As String
    End Class
End Namespace
