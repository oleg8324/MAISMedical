Namespace Objects
    Public Class CourseDetails
        Public Property Course_Sid As Integer
        Public Property RN_Sid As Integer
        Public Property InstructorName As String
        Public Property Role_Calegory_Level_Sid As Integer
        Public Property EndDate As String
        Public Property StartDate As String
        Public Property OBNApprovalNumber As String
        Public Property CategoryACEs As String
        Public Property TotalCEs As String
        Public Property Level As String
        Public Property Category As String
        Public Property CourseDescription As String
        Public Property Create As Boolean
        'Public Property ZipCode5 As String
        'Public Property ZipCode4 As String
        'Public Property CountyID As Integer
        Public Property SessionDetailList As List(Of SessionAddressInformation)
    End Class
End Namespace

