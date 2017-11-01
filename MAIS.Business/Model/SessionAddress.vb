Namespace Model
    Public Class SessionAddress
        Public Property Session_Address_Xref_Sid As Integer
        Public Property Session_Sid As Integer
        Public Property Address_Sid As Integer
        Public Property Address_Type_Sid As Integer
        Public Property Address_Line1 As String
        Public Property Address_Line2 As String
        Public Property City As String
        Public Property State As String
        Public Property StateID As Integer
        Public Property Zip_Code As String
        Public Property Zip_Code_Plus4 As String
        Public Property County As String
        Public Property countyID As Integer
        Public ReadOnly Property Street_Address As String
            Get
                If (Address_Line2 Is Nothing) Then
                    Return Address_Line1
                Else
                    Return Address_Line1 & " " & Address_Line2
                End If
            End Get
        End Property
        Public ReadOnly Property ZipWithPlus4 As String
            Get
                If String.IsNullOrWhiteSpace(Zip_Code_Plus4) Then
                    Return Zip_Code

                Else
                    Return Zip_Code & "_" & Zip_Code_Plus4
                End If
            End Get
        End Property
    End Class
End Namespace
