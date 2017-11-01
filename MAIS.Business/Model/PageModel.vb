Namespace Model
    Public Class PageModel
        Private _pageName As String
        Private _pageAddress As String

        Public Property PageName As String
            Get
                Return _pageName
            End Get
            Private Set(ByVal value As String)
                _pageName = value
            End Set
        End Property
        Public Property PageAddress As String
            Get
                Return _pageAddress
            End Get
            Private Set(ByVal value As String)
                _pageAddress = value
            End Set
        End Property

        Public Sub New(ByVal name As String, ByVal address As String)
            Me.PageName = name
            Me.PageAddress = address
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("{0};{1}", Me.PageName, Me.PageAddress)
        End Function
    End Class
End Namespace
