Namespace Model

    Public Class CEUDetails
        Public Property CEUs_Renewal_Sid As Integer
        Public Property Application_Sid As Integer?
        Public Property Permanent_Flg As Boolean
        Public Property Start_Date As Date
        Public Property End_Date As Date
        Public Property Category_Type_Sid As Integer
        Public Property Category_Type_Code As String
        Public Property Attended_Date As Date
        Public Property Total_CEUs As Double
        Public Property RN_Sid As Integer
        Public Property RN_Name As String
        Public Property Instructor_Name As String
        Public Property Title As String
        Public Property Course_Description As String
        Public Property DD_RN_Personnel_SID As String
        Public Property Active_Flg As Boolean
        Public ReadOnly Property AllowToRemove() As Boolean
            Get
                If Start_Date.ToShortDateString = Now.ToShortDateString Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property


    End Class

End Namespace