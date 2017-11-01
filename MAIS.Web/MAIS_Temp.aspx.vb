Imports ODMRDD_NET2
Imports MAIS
Imports MAIS.Business.Model
Imports MAIS.Business.Services
Imports ODMRDDHelperClassLibrary.Utility


Public Class MAIS_temp
    Inherits System.Web.UI.Page



    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init


        Session.Clear() 'When a user come here, we need to clear the session becouse of cross-user problems.
        MAIS_Helper.SetLogOutUrls()
        UserAndRoleHelper.SetSessionUser(Nothing)




        'If Request.QueryString("ShowHeaders") IsNot Nothing Then
        '    If Request.QueryString("ShowHeaders").ToString.ToLower = "false" Then
        '        Request.Cookies("ShowHeaders").Value = "false"
        '    Else
        '        Response.Cookies("ShowHeaders").Value = "true"
        '    End If
        'End If



        If Trim(CStr(Request.QueryString("UserID"))) <> String.Empty Then

            MAIS_Helper.SetGUID(Trim(CStr(Request.QueryString("UserID"))))


            RedirectToLanding()
        End If



        If Trim(CStr(Request.QueryString("GUID"))) <> String.Empty Then

            MAIS_Helper.SetGUID(Trim(CStr(Request.QueryString("GUID"))))


            RedirectToLanding()


        ElseIf ConfigurationManager.AppSettings("USERID") <> String.Empty Then
            MAIS_Helper.SetUserID(ConfigurationManager.AppSettings("USERID"))
            RedirectToLanding()
            'needed for load test
            'ElseIf Request.Url.ToString.Contains("loadApps") Then
            '    PCW_Helper.SetUserID(CInt(Request.QueryString("UserID")))
            '    RedirectToLanding()
        Else
            MAIS_Helper.LogOut()
        End If

    End Sub

    Private Sub RedirectToLanding()

        Dim queryString = String.Empty
        Dim RNLicenseFromGUID As String = String.Empty
        If Request.QueryString("GUID") IsNot Nothing AndAlso Trim(CStr(Request.QueryString("GUID"))) <> String.Empty Then
            queryString &= "GUID=" & Trim(CStr(Request.QueryString("GUID")))
        End If

        If Request.QueryString("RNLicense") IsNot Nothing AndAlso Trim(CStr(Request.QueryString("RNLicense"))) <> String.Empty Then
            If (Not String.IsNullOrEmpty(queryString)) Then
                queryString &= "&"
            End If
            RNLicenseFromGUID = Request.QueryString("RNLicense")
            queryString &= "RNLicense=" & Trim(CStr(Request.QueryString("RNLicense")))
        End If

        If Request.QueryString("ShowHeaders") IsNot Nothing Then
            If (Not String.IsNullOrEmpty(queryString)) Then
                queryString &= "&"
            End If

            queryString &= "showheaders=" & Request.QueryString("ShowHeaders")
        End If

        If (UserAndRoleHelper.IsUserAdmin Or UserAndRoleHelper.IsUserReadOnly) Then
            If Not String.IsNullOrEmpty(queryString) Then
                Response.Redirect(PagesHelper.LandingPage + "?" + queryString, False)
            Else
                Response.Redirect(PagesHelper.LandingPage, False)
            End If
        End If

        If (UserAndRoleHelper.IsUserRN) Then
            If ((ConfigHelper.CreateUserAccountAccess) And (Not String.IsNullOrWhiteSpace(RNLicenseFromGUID))) Then 'Auto map users in user RN mapping table from portal to identify RN's
                If (UserAndRoleHelper.CurrentUser.UserID > 0) Then
                    Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
                    InsertUser()
                    'Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
                    Dim _rnInfo As RN_Mapping = maisSvc.CheckRNMapping(UserAndRoleHelper.CurrentUser.UserID)
                    If _rnInfo IsNot Nothing And _rnInfo.RN_Sid > 0 Then
                        SessionHelper.LoginUsersRNLicense = _rnInfo.RNLicenseNumber
                        If ((Not String.IsNullOrEmpty(_rnInfo.RNLicenseNumber)) And (_rnInfo.Un_Map_Flg = False)) Then
                            If Not String.IsNullOrEmpty(queryString) Then
                                Response.Redirect(PagesHelper.LandingPage + "?" + queryString, False)
                            Else
                                Response.Redirect(PagesHelper.LandingPage, False)
                            End If
                        ElseIf ((Not String.IsNullOrEmpty(_rnInfo.RNLicenseNumber)) And (_rnInfo.Un_Map_Flg = True)) Then
                            If Not String.IsNullOrEmpty(queryString) Then
                                Response.Redirect(PagesHelper.LogoutPage + "?" + queryString, False)
                            Else
                                Response.Redirect(PagesHelper.LogoutPage, False)
                            End If
                        End If
                    Else
                        Dim userDetails As New UserLoginSearch
                        userDetails.FirstName = UserAndRoleHelper.CurrentUser.FirstName
                        userDetails.LastName = UserAndRoleHelper.CurrentUser.LastName
                        userDetails.RNLicenseNumber = RNLicenseFromGUID.Trim()

                        Dim retval As New ReturnObject(Of Long)(-1L)
                        retval = maisSvc.SaveUserRNMappingData(userDetails)
                        If (retval.Errors.Count = 0) Then
                            SessionHelper.LoginUsersRNLicense = retval.Messages(0).Message
                            Response.Redirect("mais_home.aspx")
                        Else
                            lblmsg.Text = retval.Errors(0).Message
                            If Not String.IsNullOrEmpty(queryString) Then
                                Response.Redirect(PagesHelper.LogoutPage + "?" + queryString, False)
                            Else
                                Response.Redirect(PagesHelper.LogoutPage, False)
                            End If
                        End If
                        'If Not String.IsNullOrEmpty(queryString) Then
                        '    Response.Redirect(PagesHelper.LoginPage + "?" + queryString, False)
                        'Else
                        '    Response.Redirect(PagesHelper.LoginPage, False)
                        'End If
                    End If

                End If
            Else
                If (UserAndRoleHelper.CurrentUser.UserID > 0) Then
                    InsertUser()
                    Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
                    Dim _rnInfo As RN_Mapping = maisSvc.CheckRNMapping(UserAndRoleHelper.CurrentUser.UserID)
                    If _rnInfo IsNot Nothing And _rnInfo.RN_Sid > 0 Then
                        SessionHelper.LoginUsersRNLicense = _rnInfo.RNLicenseNumber
                        If ((Not String.IsNullOrEmpty(_rnInfo.RNLicenseNumber)) And (_rnInfo.Un_Map_Flg = False)) Then
                            If Not String.IsNullOrEmpty(queryString) Then
                                Response.Redirect(PagesHelper.LandingPage + "?" + queryString, False)
                            Else
                                Response.Redirect(PagesHelper.LandingPage, False)
                            End If
                        ElseIf ((Not String.IsNullOrEmpty(_rnInfo.RNLicenseNumber)) And (_rnInfo.Un_Map_Flg = True)) Then
                            If Not String.IsNullOrEmpty(queryString) Then
                                Response.Redirect(PagesHelper.LogoutPage + "?" + queryString, False)
                            Else
                                Response.Redirect(PagesHelper.LogoutPage, False)
                            End If
                        End If
                    Else
                        If Not String.IsNullOrEmpty(queryString) Then
                            Response.Redirect(PagesHelper.LoginPage + "?" + queryString, False)
                        Else
                            Response.Redirect(PagesHelper.LoginPage, False)
                        End If
                    End If

                End If
            End If
            'Else
            'HttpContext.Current.Response.Redirect("MAISLogout.aspx", False)
        End If
        If (UserAndRoleHelper.IsUserSecretary) Then
            If (UserAndRoleHelper.CurrentUser.UserID > 0) Then
                InsertUser()
                Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
                Dim flag As Boolean = maisSvc.CheckSecetaryMapping(UserAndRoleHelper.CurrentUser.UserID)
                If (flag) Then
                    If Not String.IsNullOrEmpty(queryString) Then
                        Response.Redirect(PagesHelper.LandingPage + "?" + queryString, False)
                    Else
                        Response.Redirect(PagesHelper.LandingPage, False)
                    End If
                Else
                    If Not String.IsNullOrEmpty(queryString) Then
                        Response.Redirect(PagesHelper.LogoutPage + "?" + queryString, False)
                    Else
                        Response.Redirect(PagesHelper.LogoutPage, False)
                    End If
                End If
            End If
        End If
    End Sub
    Protected Sub InsertUser()
        Dim retNum As Integer = 0
        Dim userDetails As New UserMappingDetails
        userDetails.UserID = UserAndRoleHelper.CurrentUser.UserID
        If (UserAndRoleHelper.CurrentUser.Roles IsNot Nothing) Then
            If (UserAndRoleHelper.IsUserRN) Then
                Dim role = UserAndRoleHelper.CurrentUser.Roles.FindAll(Function(rn) rn.RoleName.Contains("MAIS_RN"))
                userDetails.PortalUserRole = role(0).RoleName
            ElseIf (UserAndRoleHelper.IsUserSecretary) Then
                Dim role1 = UserAndRoleHelper.CurrentUser.Roles.FindAll(Function(rn) rn.RoleName.Contains("MAIS_Secretary"))
                userDetails.PortalUserRole = role1(0).RoleName
            End If
        Else
            userDetails.PortalUserRole = String.Empty
        End If
        userDetails.FirstName = UserAndRoleHelper.CurrentUser.FirstName
        userDetails.LastName = UserAndRoleHelper.CurrentUser.LastName
        userDetails.MiddleName = UserAndRoleHelper.CurrentUser.MiddleNameorInitial
        userDetails.Email = UserAndRoleHelper.CurrentUser.Email
        userDetails.User_Code = UserAndRoleHelper.CurrentUser.UserCode
        If (UserAndRoleHelper.IsUserSecretary) Then
            userDetails.Is_Secretary = True
        Else
            userDetails.Is_Secretary = False
        End If
        Dim maisSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        retNum = maisSvc.SaveUserLoggedData(userDetails)
        If Not (retNum > 0) Then
            lblmsg.Text = "Error saving user"
        Else
            lblmsg.Text = String.Empty
        End If
    End Sub
    'Public Property UserMappingID As Integer
    '    Get
    '        Return _usermapping
    '    End Get
    '    Set(ByVal value As Integer)
    '        _usermapping = value
    '    End Set
    'End Property
End Class