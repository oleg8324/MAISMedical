
Namespace Objects
    Public Class Secretary_Association
        Public Property User_Mapping_Sid As Integer
        Public Property First_Name As String
        Public Property Last_Name As String
        Public Property Middle_Name As String
        Public Property SecretaryUserName As String
        Public Property Last_Updated_By As Integer
        Public Property Last_Updated_Date As DateTime
        Public Property Email As String
        ' Public Property Secretary_Details As List(Of Secretary_Detail)
    End Class
    Public Class Secretary_Detail
        Public Property User_Mapping_Sid As Integer
        Public Property RN_Secretary_Association_Sid As Integer
        Public Property RN_Sid As Integer
        Public Property F_Name As String
        Public Property L_Name As String
        Public Property M_Name As String
        Public Property RNLicense As String
        Public Property Last_Updated_By As Integer
        Public Property Last_Updated_Date As DateTime
        Public Property Comments As String
    End Class
End Namespace