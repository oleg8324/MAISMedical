Imports System.Data.SqlClient
Imports MAIS.Business.Services

Public Class MAIS_Helper
    Public Shared Sub SetGUID(ByVal guid As String)
        HttpContext.Current.Session("GUID") = guid
    End Sub

    Public Shared Function GetGUID() As String
        Dim strGUID As String = String.Empty

        If Not HttpContext.Current.Session("GUID") Is Nothing Then
            strGUID = HttpContext.Current.Session("GUID")
        End If

        Return strGUID
    End Function

    Public Shared Sub SetUserID(ByVal userID As Integer)
        HttpContext.Current.Session("USERID") = userID
    End Sub
    Public Shared Function GetProviderConnection() As String
        Dim strPCW As String = String.Empty

        If ConfigurationManager.ConnectionStrings("Provider").ConnectionString IsNot Nothing Then
            strPCW = ConfigurationManager.ConnectionStrings("Provider").ConnectionString
        End If

        Return strPCW
    End Function

    Public Shared Function GetUserId() As Integer
        Dim intUserId As Integer = 0

        If Not String.IsNullOrWhiteSpace(HttpContext.Current.Session("USERID")) AndAlso
           IsNumeric(HttpContext.Current.Session("USERID")) Then

            If HttpContext.Current.Session("USERID") <> 0 Then
                intUserId = HttpContext.Current.Session("USERID")
            End If
        End If

        Return intUserId
    End Function

    Public Shared Function GetUser() As ODMRDD_NET2.IUser
        Dim user As ODMRDD_NET2.IUser = Nothing

        Try
            Dim userService As ODMRDD_NET2.IUserService = New ODMRDD_NET2.UserService

            If MAIS_Helper.GetGUID <> String.Empty Then
                user = userService.GetUserByGUID(MAIS_Helper.GetGUID)
                SetUserID(user.UserID)
            Else
                user = userService.GetUserByUserId(MAIS_Helper.GetUserId)
            End If

            If user.UserID = 0 Then
                LogOut()
            End If

        Catch ex As Exception
            LogOut()
        End Try

        Return user
    End Function

    Public Shared Function GetUserRoleUsingMAIS(ByVal userID As Integer) As MAIS.Business.Model.MAISRNDDRoleDetails
        Dim userSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Dim userRole As MAIS.Business.Model.MAISRNDDRoleDetails = Nothing
        userRole = userSvc.GetRoleUsingUserID(userID)
        Return userRole
    End Function

    Public Shared Function GetRN_SidbyUserID(ByVal userID As Integer) As Integer
        Dim retVal As Integer = 0
        Dim userRole As New MAIS.Business.Model.MAISRNDDRoleDetails
        userRole = GetUserRoleUsingMAIS(userID)
        If userRole IsNot Nothing Then
            retVal = userRole.RNSID
        End If
        Return retVal

    End Function
#Region "DODD Portal"
    'Public Shared Function GetExitURL() As String
    '    Dim LogoutURL As String = String.Empty

    '    If HttpContext.Current.Session("OldPortal") IsNot Nothing Then
    '        LogoutURL = HttpContext.Current.Session("OldPortal")
    '    Else
    '        Throw New InvalidOperationException("The EXITURL location has not been set in the configuration.")
    '    End If

    '    Return LogoutURL
    'End Function

    Public Shared Function GetSystemCode() As String
        Dim SystemCode As String = String.Empty

        If ConfigurationManager.AppSettings("SystemCode") IsNot Nothing Then
            SystemCode = ConfigurationManager.AppSettings("SystemCode")
        Else
            Throw New InvalidOperationException("The SystemCode has not been set in the configuration.")
        End If

        Return SystemCode
    End Function
    'Public Shared Function GetDODDPortalLogoutURL() As String
    '    Dim LogoutURL As String = String.Empty

    '    If HttpContext.Current.Session("NewPortal") IsNot Nothing Then
    '        LogoutURL = HttpContext.Current.Session("NewPortal") & "?SystemCode=" & GetSystemCode()
    '    Else
    '        Throw New InvalidOperationException("The PortalLogoutURL location has not been set in the configuration.")
    '    End If

    '    Return LogoutURL
    'End Function

    Public Shared Sub LogOut()
        'clear
        HttpContext.Current.Session("GUID") = ""
        HttpContext.Current.Session("USERCODE") = ""
        HttpContext.Current.Session("USERID") = ""
        ' redirect

        If HttpContext.Current.Request.Cookies("ShowHeaders") IsNot Nothing Then
            If HttpContext.Current.Request.Cookies("ShowHeaders").Value.ToLower = "false" Then
                HttpContext.Current.Response.Redirect("MAISLogout.aspx", False)
                HttpContext.Current.Response.End()
            Else
                HttpContext.Current.Response.Redirect("MAISLogout.aspx", False)
                HttpContext.Current.Response.End()
            End If
        Else
            HttpContext.Current.Response.Redirect("MAISLogout.aspx", False)
            HttpContext.Current.Response.End()
        End If
    End Sub

    Public Shared Sub SetLogOutUrls()
        Try
            Using cn As New SqlConnection(ConfigHelper.GetOIDDBConnectionString())
                Dim cmd As New SqlCommand("SELECT URL, Type FROM SCT_PortalLogout", cn)
                cmd.CommandType = CommandType.Text
                cn.Open()

                Dim sdr As SqlDataReader = cmd.ExecuteReader()

                While sdr.Read()
                    If "New".Equals(sdr("Type")) Then
                        HttpContext.Current.Session("NewPortal") = sdr("URL")
                    Else
                        HttpContext.Current.Session("OldPortal") = sdr("URL")
                    End If
                End While
                cn.Close()
            End Using
        Catch ex As Exception
            ' Log Message?
        End Try
    End Sub
#End Region
End Class
