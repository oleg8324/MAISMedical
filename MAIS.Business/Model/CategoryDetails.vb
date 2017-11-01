Namespace Model
    Public Class CategoryDetails
        Public Property Category_Type_Sid As Integer
        Public Property Category_Code As String
        Public Property Category_Desc As String
        Public ReadOnly Property CategoryCode_With_Desc() As String
            Get
                Return Category_Code & " - " & Category_Desc
            End Get
        End Property


    End Class
End Namespace

