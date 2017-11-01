Imports ODMRDD_NET2
Imports MAIS.Business.Services
Imports MAIS.Business

Public Class UserAndRoleHelper

    Public Shared ReadOnly Property CurrentUser As IUser

        Get
            If GetSessionUser() IsNot Nothing Then
                Return GetSessionUser()
            Else
                SetSessionUser(MAIS_Helper.GetUser)
                Return GetSessionUser()
            End If
        End Get

    End Property

    Public Shared Function GetSessionUser()
        Return HttpContext.Current.Session("USER")

    End Function

    Public Shared Sub SetSessionUser(ByVal User As IUser)
        HttpContext.Current.Session("USER") = User
    End Sub

    Public Shared ReadOnly Property IsUserAdmin As Boolean 'MAIS admin
        Get
            Dim _isUserAdmin As Boolean = False

            If CurrentUser.HasRole("MAIS_Admin") Then
                _isUserAdmin = True
            End If

            Return _isUserAdmin
        End Get
    End Property

    Public Shared ReadOnly Property IsUserRN As Boolean 'registered nurse
        Get
            Dim _isUserRN As Boolean = False

            If CurrentUser.HasRole("MAIS_RN") Then
                _isUserRN = True
            End If

            Return _isUserRN
        End Get
    End Property

    Public Shared ReadOnly Property IsUserSecretary As Boolean 'Secretary
        Get
            Dim _isUserSecretary As Boolean = False

            If CurrentUser.HasRole("MAIS_Secretary") Then
                _isUserSecretary = True
            End If

            Return _isUserSecretary
        End Get
    End Property

    Public Shared ReadOnly Property IsUserReadOnly As Boolean 'read only
        Get
            Dim _isUserReadOnly As Boolean = False

            If CurrentUser.HasRole("MAIS_ReadOnly") Then
                _isUserReadOnly = True
            End If

            Return _isUserReadOnly
        End Get
    End Property
End Class
