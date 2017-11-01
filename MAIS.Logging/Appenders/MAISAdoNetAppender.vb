Imports System.Configuration
Imports log4net.Appender

Namespace Appenders
    Public Class MAISAdoNetAppender
        Inherits AdoNetAppender

        Public Sub New()
            Me.ConnectionString = ConfigurationManager.ConnectionStrings("MAIS").ConnectionString
        End Sub
    End Class
End Namespace
