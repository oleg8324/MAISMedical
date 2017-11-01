'Use the string "1" for RN and "2" for DD
Public Class SessionHelper
    Public Shared Property RN_Flg As Boolean
        Get
            Return HttpContext.Current.Session("RN_Flg")
        End Get
        Set(ByVal value As Boolean)
            HttpContext.Current.Session("RN_Flg") = value
        End Set
    End Property
    Public Shared Property BrandNew As Boolean
        Get
            Return HttpContext.Current.Session("BrandNew")
        End Get
        Set(ByVal value As Boolean)
            HttpContext.Current.Session("BrandNew") = value
        End Set
    End Property
    Public Shared Property Notation_Flg As Boolean
        Get
            Return HttpContext.Current.Session("Notation_Flg")
        End Get
        Set(ByVal value As Boolean)
            HttpContext.Current.Session("Notation_Flg") = value
        End Set
    End Property
    Public Shared Property MyUpdate_Profile As Boolean
        Get
            Return HttpContext.Current.Session("MyUpdate_Profile")
        End Get
        Set(ByVal value As Boolean)
            HttpContext.Current.Session("MyUpdate_Profile") = value
        End Set
    End Property
    Public Shared Property SessionUniqueID As String ' DODD Personel code or RN License number
        Get
            Return HttpContext.Current.Session("SessionUniqueID")
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session("SessionUniqueID") = value
        End Set
    End Property
    Public Shared Property ApplicationStatus As String ' DODD Personel code or RN License number
        Get
            Return HttpContext.Current.Session("ApplicationStatus")
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session("ApplicationStatus") = value
        End Set
    End Property
    Public Shared Property Name As String ' DODD Personel Name or RN Name
        Get
            Return HttpContext.Current.Session("Name")
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session("Name") = value
        End Set
    End Property
    Public Shared Property ApplicationID As Integer
        Get
            Return HttpContext.Current.Session("ApplicationID")
        End Get

        Set(ByVal value As Integer)
            HttpContext.Current.Session("ApplicationID") = value
        End Set
    End Property

    Public Shared Property MAISUserID As Integer 'Local to MAIS application not Portal level
        Get
            Return If(HttpContext.Current.Session("MAIS_UserID"), 0)
        End Get
        Set(ByVal value As Integer)
            HttpContext.Current.Session("MAIS_UserID") = value
        End Set
    End Property
    Public Shared Property ApplicationType As String
        Get
            Return If(HttpContext.Current.Session("ApplicationType"), String.Empty)
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session("ApplicationType") = value
        End Set
    End Property
    Public Shared Property MAISLevelUserRole As Integer 'this role is RoleCategoryLevel reference table ID such as 'Rn trainer has - 4, QA RN has 7
        Get
            Return (HttpContext.Current.Session("MAISLevelUserRole"))
        End Get
        Set(ByVal value As Integer)
            HttpContext.Current.Session("MAISLevelUserRole") = value
        End Set
    End Property
    Public Shared Property LoginUsersRNLicense As String 'this role is RoleCategoryLevel reference table ID such as 'Rn trainer has - 4, QA RN has 7
        Get
            Return (HttpContext.Current.Session("LoginUsersRNLicense"))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session("LoginUsersRNLicense") = value
        End Set
    End Property
    Public Shared Property SelectedUserRole As Integer
        Get
            Return (HttpContext.Current.Session("SelectedUserRole"))
        End Get
        Set(ByVal value As Integer)
            HttpContext.Current.Session("SelectedUserRole") = value
        End Set
    End Property
    Public Shared Property ExistingUserRole As Integer
        Get
            Return (HttpContext.Current.Session("ExistingUserRole"))
        End Get
        Set(ByVal value As Integer)
            HttpContext.Current.Session("ExistingUserRole") = value
        End Set
    End Property

    Public Shared Sub ClearSessionHelpValues()
        HttpContext.Current.Session("RN_Flg") = Nothing
        HttpContext.Current.Session("BrandNew") = Nothing
        HttpContext.Current.Session("Notation_Flg") = Nothing
        HttpContext.Current.Session("MyUpdate_Profile") = Nothing
        HttpContext.Current.Session("SessionUniqueID") = Nothing
        HttpContext.Current.Session("ApplicationStatus") = Nothing
        HttpContext.Current.Session("Name") = Nothing
        HttpContext.Current.Session("ApplicationID") = Nothing
        HttpContext.Current.Session("MAIS_UserID") = Nothing
        HttpContext.Current.Session("ApplicationType") = Nothing
        HttpContext.Current.Session("MAISLevelUserRole") = Nothing
        HttpContext.Current.Session("SelectedUserRole") = Nothing
        HttpContext.Current.Session("ExistingUserRole") = Nothing
    End Sub
End Class
