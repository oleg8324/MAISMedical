Public Interface IAbstractLoggingBase
    Sub LogError(ByVal message As Object, ByVal userId As Integer, Optional ByVal ex As Exception = Nothing)
    Sub LogDebug(ByVal message As Object, Optional ByVal ex As Exception = Nothing)
    Sub LogInfo(ByVal message As Object, Optional ByVal ex As Exception = Nothing)
    Sub LogFatal(ByVal message As Object, Optional ByVal ex As Exception = Nothing)
    Sub LogWarning(ByVal message As Object, Optional ByVal ex As Exception = Nothing)   
End Interface

Public MustInherit Class AbstractLoggingBase
    Implements IAbstractLoggingBase

    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(Me.GetType().ToString())
   
    Public Sub LogError(ByVal message As Object, ByVal userId As Integer, Optional ByVal ex As System.Exception = Nothing) Implements IAbstractLoggingBase.LogError
        log4net.GlobalContext.Properties("userid") = userId
        If ex Is Nothing Then
            Me.log.Error(message)
        Else
            Me.log.Error(message, ex)
        End If
    End Sub

    Public Sub LogDebug(ByVal message As Object, Optional ByVal ex As System.Exception = Nothing) Implements IAbstractLoggingBase.LogDebug
        If ex Is Nothing Then
            Me.log.Debug(message)
        Else
            Me.log.Debug(message, ex)
        End If
    End Sub

    Public Sub LogFatal(ByVal message As Object, Optional ByVal ex As System.Exception = Nothing) Implements IAbstractLoggingBase.LogFatal
        If ex Is Nothing Then
            Me.log.Fatal(message)
        Else
            Me.log.Fatal(message, ex)
        End If
    End Sub

    Public Sub LogInfo(ByVal message As Object, Optional ByVal ex As System.Exception = Nothing) Implements IAbstractLoggingBase.LogInfo
        If ex Is Nothing Then
            Me.log.Info(message)
        Else
            Me.log.Info(message, ex)
        End If
    End Sub

    Public Sub LogWarning(ByVal message As Object, Optional ByVal ex As System.Exception = Nothing) Implements IAbstractLoggingBase.LogWarning
        If ex Is Nothing Then
            Me.log.Warn(message)
        Else
            Me.log.Warn(message, ex)
        End If
    End Sub
  
End Class