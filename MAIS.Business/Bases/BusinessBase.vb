Imports ODMRDDHelperClassLibrary.Utility

Public Interface IBusinessBase
    ReadOnly Property Messages() As List(Of ReturnMessage)
    Function HasErrors() As Boolean
    Function HasWarnings() As Boolean
End Interface

''' <summary>
''' A base class to facilitate message passing across layers related to business rules.
''' </summary>
''' <remarks></remarks>
Public Class BusinessBase
    Inherits Logging.AbstractLoggingBase
    Implements IBusinessBase

    Public Property UserID As String
    Protected _messages As New List(Of ReturnMessage)

    Public ReadOnly Property Messages() As List(Of ReturnMessage) Implements IBusinessBase.Messages
        Get
            Return _messages
        End Get
    End Property

    Public Function HasErrors() As Boolean Implements IBusinessBase.HasErrors
        Return (From m In Me._messages Where m.IsError = True).Any
    End Function

    Public Function HasWarnings() As Boolean Implements IBusinessBase.HasWarnings
        Return (From m In Me._messages Where m.IsWarning = True).Any
    End Function

    Protected Sub ImportMessages(ByVal m As List(Of ReturnMessage))
        If m.Count > 0 Then
            Me.Messages.AddRange(m)
            m.Clear()
        End If
    End Sub

    Protected Sub LogErrors(ByVal q As Data.QueriesBase, ByVal errMsg As String, ByVal ex As Exception)
        If q.Messages.Count > 0 Then

            For Each m In q.Messages
                If m.IsError Then
                    Me.LogError(m.ToString(), CInt(Me.UserID), ex)
                ElseIf m.IsWarning Then
                    Me.LogWarning(m.ToString(), ex)
                End If
            Next

            Me.Messages.AddRange(q.Messages)
            q.Messages.Clear()
        Else
            Me.Messages.Add(New ODMRDDHelperClassLibrary.Utility.ReturnMessage(errMsg, True, False))
            Me.LogError(errMsg, CInt(Me.UserID), ex)
        End If
    End Sub
End Class
