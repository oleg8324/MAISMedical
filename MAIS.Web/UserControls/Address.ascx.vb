Imports ODMRDDHelperClassLibrary
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business
Imports MAIS.Business.Services

Public Class Address
    Inherits System.Web.UI.UserControl
    Private _maisApp As MAIS.Business.Model.MAISApplicationDetails


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not DesignMode Then

        End If
    End Sub
    Public Sub PopulateAddressLine(ByVal sessionId As String)

    End Sub

    Public Property HideEmailAddress As Boolean
        Get
            Return Not txtEmailAddress.Visible()
        End Get
        Set(ByVal value As Boolean)
            txtEmailAddress.Visible = Not value
        End Set
    End Property
    Public Property HideLabelEmail As Boolean
        Get
            Return Not lblEmail.Visible()
        End Get
        Set(ByVal value As Boolean)
            lblEmail.Visible = Not value
        End Set
    End Property
    Public Property MandatoryEmailAddress As Boolean
        Get
            Return Not fntEmail.Visible()
        End Get
        Set(ByVal value As Boolean)
            fntEmail.Visible = Not value
        End Set
    End Property
    Public Property MandatoryPhone As Boolean
        Get
            Return Not fntPhone.Visible()
        End Get
        Set(ByVal value As Boolean)
            fntPhone.Visible = Not value
        End Set
    End Property
    Public Property HidePhoneNumber As Boolean
        Get
            Return Not txtPhoneNumber.Visible()
        End Get
        Set(ByVal value As Boolean)
            txtPhoneNumber.Visible = Not value
        End Set
    End Property
    Public Property HideLabelPhone As Boolean
        Get
            Return Not lblPhone.Visible()
        End Get
        Set(ByVal value As Boolean)
            lblPhone.Visible = Not value
        End Set
    End Property

    Public Property AddressLine1 As String
        Get
            Return txtAdressLine1.Value
        End Get
        Set(ByVal value As String)
            txtAdressLine1.Value = value
        End Set
    End Property
    Public Property AddressLine2 As String
        Get
            Return txtAddressLine2.Value
        End Get
        Set(ByVal value As String)
            txtAddressLine2.Value = value
        End Set
    End Property
    Public Property City As String
        Get
            Return txtCity.Value
        End Get
        Set(ByVal value As String)
            txtCity.Value = value
        End Set
    End Property
    Public Property State As String
        Get
            Return ddlState.Text
        End Get
        Set(ByVal value As String)
            ddlState.Text = value
        End Set
    End Property
    Public Property StateID As Integer
        Get
            Return ddlState.SelectedValue
        End Get
        Set(ByVal value As Integer)
            ddlState.SelectedValue = value
        End Set
    End Property
    Public Property County As String
        Get
            Return ddlCounty.Text
        End Get
        Set(ByVal value As String)
            ddlCounty.Text = value
        End Set
    End Property
    Public Property CountyID As Integer
        Get
            Return ddlCounty.SelectedValue
        End Get
        Set(ByVal value As Integer)
            ddlCounty.SelectedValue = value
        End Set
    End Property
    Public Property Zip As String
        Get
            Return txtZip.Value
        End Get
        Set(ByVal value As String)
            txtZip.Value = value
        End Set
    End Property
    Public Property ZipPlus As String
        Get
            Return txtZipPlus.Value
        End Get
        Set(ByVal value As String)
            txtZipPlus.Value = value
        End Set
    End Property
    Public Property PhoneNumber As String
        Get
            Return txtPhoneNumber.Value
        End Get
        Set(ByVal value As String)
            txtPhoneNumber.Value = value
        End Set
    End Property
    Public Property Email As String
        Get
            Return txtEmailAddress.Value
        End Get
        Set(ByVal value As String)
            txtEmailAddress.Value = value
        End Set
    End Property
    Public Property HideAddress1 As Boolean
        Get
            Return Not txtAdressLine1.Disabled()
        End Get
        Set(ByVal value As Boolean)
            txtAdressLine1.Disabled = Not value
        End Set
    End Property
    Public Property HideAddress2 As Boolean
        Get
            Return Not txtAddressLine2.Disabled()
        End Get
        Set(ByVal value As Boolean)
            txtAddressLine2.Disabled = Not value
        End Set
    End Property
    Public Property HideCity As Boolean
        Get
            Return Not txtCity.Disabled()
        End Get
        Set(ByVal value As Boolean)
            txtCity.Disabled = Not value
        End Set
    End Property
    Public Property EnableState As Boolean
        Get
            Return Not ddlState.Enabled()
        End Get
        Set(ByVal value As Boolean)
            ddlState.Enabled = Not value
        End Set
    End Property
    Public Property HideCounty As Boolean
        Get
            Return Not ddlCounty.Enabled()
        End Get
        Set(ByVal value As Boolean)
            ddlCounty.Enabled = Not value
        End Set
    End Property
    Public Property HideCountyMandatory As Boolean

        Get
            Return Not spanCounty.Visible
        End Get
        Set(value As Boolean)
            spanCounty.Visible = Not value
        End Set
    End Property
    Public Property HideZip As Boolean
        Get
            Return Not txtZip.Disabled()
        End Get
        Set(ByVal value As Boolean)
            txtZip.Disabled = Not value
        End Set
    End Property
    Public Property HideZipPlus As Boolean
        Get
            Return Not txtZipPlus.Disabled()
        End Get
        Set(ByVal value As Boolean)
            txtZipPlus.Disabled = Not value
        End Set
    End Property
    Public Property DisablePhone As Boolean
        Get
            Return Not txtPhoneNumber.Disabled()
        End Get
        Set(ByVal value As Boolean)
            txtPhoneNumber.Disabled = Not value
        End Set
    End Property
    Public Property DisableEmail As Boolean
        Get
            Return Not txtEmailAddress.Disabled()
        End Get
        Set(ByVal value As Boolean)
            txtEmailAddress.Disabled = Not value
        End Set
    End Property


    Public Sub LoadCounties()
        'Dim lstCountyBoards As New ReturnObject(Of ODMRDDHelperClassLibrary.CountyBoardCollection)
        'Dim cs As CountyBoardService = New CountyBoardService(ConfigHelper.GetOIDDBConnectionString)
        'lstCountyBoards = cs.GetAllCountyBoards("mastr", True)
        'ddlCounty.Items.Clear()
        'Counties
        Dim userSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Dim lstCountyBoards As List(Of Model.CountyDetails) = userSvc.GetAllCountyCodes()
        ddlCounty.Items.Add(New ListItem("--- County Selection ---", "-1"))
        ddlCounty.DataSource = lstCountyBoards
        ddlCounty.DataTextField = "CountyAlias"
        ddlCounty.DataValueField = "CountyID"
        ddlCounty.DataBind()
    End Sub
    Public Sub LoadStates()
        'Dim lstStates As New ReturnObject(Of ODMRDDHelperClassLibrary.StateCollection)
        'Dim ss As StateService = New StateService(ConfigHelper.GetOIDDBConnectionString)
        'lstStates = ss.GetAllStates(ConfigHelper.GetOIDDBConnectionString)
        Dim userSvc As IMAISSerivce = StructureMap.ObjectFactory.GetInstance(Of IMAISSerivce)()
        Dim lstStates As List(Of Model.StateDetails) = userSvc.GetAllStates()
        ddlState.Items.Clear()
        ddlState.SelectedValue = 35

        ''states  
        ddlState.DataSource = lstStates
        ddlState.DataTextField = "StateAbr"
        ddlState.DataValueField = "StateID"
        ddlState.DataBind()

    End Sub

End Class