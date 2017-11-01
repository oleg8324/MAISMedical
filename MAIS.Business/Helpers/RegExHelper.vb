
Namespace Helpers
    Public Class RegExHelper
        Public Shared Property Alphabetical As String = "^\'|-|[a-zA-Z]$"
        Public Shared Property Alphanumeric As String = " /^[0-9a-zA-Z ]+$/"
        Public Shared Property Numeric As String = "/^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$/"
        Public Shared Property Dates As String = "^(0?[1-9]|1[012])[/](0?[1-9]|[12][0-9]|3[01])[/](19|20)\d\d$"
        Public Shared Property Email As String = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$"

        Public Shared Property Phone As String = "^[0-9]{10}$"
        Public Shared Property TaxID As String = "^[0-9]{9}$"
        Public Shared Property Url As String = "((https?)://)?[-A-Za-z0-9+&@#/%?=~_|!:,.;]*[-A-Za-z0-9+&@#/%=~_|]"
    End Class
End Namespace