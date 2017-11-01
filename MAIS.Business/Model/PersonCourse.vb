Namespace Model
    Public Class PersonCourse
        Public Property PersonCourseXrefSid As Integer
        Public Property RoleRNDDPersonelXrefSid As Integer
        Public Property CourseSid As Integer
        Public Property ActiveFlg As Boolean
    End Class

    Public Class PersonCourseSession
        Public Property PersonCourseSessionXref As Integer
        Public Property PersonCourseXrefSid As Integer
        Public Property SessionSid As Integer
        Public Property ActiveFlg As Boolean
    End Class
End Namespace

